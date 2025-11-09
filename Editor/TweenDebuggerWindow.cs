using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Emp37.Tweening.Editor
{
      using static EditorStyles;


      public sealed class TweenDebuggerWindow : EditorWindow
      {
            private Vector2 scrollPosition;

            private bool autoRefresh = true;
            private const double REFRESH_INTERVAL = 0.25;
            private double lastRefreshTime;

            private string searchFilter = string.Empty;
            private int selectedTypeIndex = 0;
            private bool showCompleted = false;

            private readonly string[] typeFilters = { "All", "Value", "Sequence", "Parallel", "Delay", "Invoke" };


            [MenuItem("Tools/Emp37/Tweening.Debugger")]
            public static void ShowWindow()
            {
                  var window = GetWindow<TweenDebuggerWindow>("Tween Debugger");
                  window.minSize = new Vector2(420F, 320F);
                  window.Show();
            }

            private void OnEnable()
            {
                  EditorApplication.playModeStateChanged += OnPlayModeChanged;
            }
            private void OnDisable()
            {
                  EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            }
            private void Update()
            {
                  if (!Application.isPlaying || !autoRefresh) return;

                  double time = EditorApplication.timeSinceStartup;
                  if (time - lastRefreshTime > REFRESH_INTERVAL)
                  {
                        lastRefreshTime = time;
                        Repaint();
                  }
            }
            private void OnGUI()
            {
                  //if (!Application.isPlaying)
                  //{
                  //      EditorGUILayout.HelpBox("Enter Play Mode to inspect active tweens.", MessageType.Info);
                  //      return;
                  //}

                  GUILayoutOption[] size = { GUILayout.MinWidth(40F), GUILayout.MaxWidth(80F) };

                  #region T O O L B A R
                  EditorGUILayout.BeginHorizontal(toolbar);
                  if (GUILayout.Button("Pause All", toolbarButton, size)) Factory.Pause();
                  if (GUILayout.Button("Resume All", toolbarButton, size)) Factory.Resume();
                  if (GUILayout.Button("Kill All", toolbarButton, size) && EditorUtility.DisplayDialog("Kill All Tweens", "Are you sure you want to kill all active tweens?", "Yes", "Cancel")) Factory.Kill();
                  if (GUILayout.Button("Refresh", toolbarButton, size)) Repaint();

                  GUILayout.FlexibleSpace();

                  autoRefresh = GUILayout.Toggle(autoRefresh, "Auto Refresh", toolbarButton, GUILayout.Width(120F));
                  EditorGUILayout.EndHorizontal();
                  #endregion

                  #region S T A T I S T I C S
                  List<ITween> tweens = GetActiveTweens();
                  if (tweens != null)
                  {
                        int total = tweens.Count, active = tweens.Count(tween => tween.Phase == Phase.Active), paused = tweens.Count(tween => tween.Phase == Phase.Paused);

                        EditorGUILayout.LabelField("Statistics", boldLabel);
                        EditorGUILayout.BeginHorizontal(helpBox);
                        EditorGUILayout.LabelField("Total", miniBoldLabel, size);
                        EditorGUILayout.LabelField(total.ToString(), size);
                        EditorGUILayout.LabelField("Active", miniBoldLabel, size);
                        EditorGUILayout.LabelField(active.ToString(), size);
                        EditorGUILayout.LabelField("Paused", miniBoldLabel, size);
                        EditorGUILayout.LabelField(paused.ToString(), size);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Tween Type", miniBoldLabel, GUILayout.Width(180));
                        EditorGUILayout.LabelField("Count", miniBoldLabel, GUILayout.Width(60));
                        EditorGUILayout.EndHorizontal();

                        foreach (var group in tweens.GroupBy(t => t.GetType().Name).OrderByDescending(g => g.Count()))
                        {
                              EditorGUILayout.BeginHorizontal();
                              EditorGUILayout.LabelField(group.Key, GUILayout.Width(180));
                              EditorGUILayout.LabelField(group.Count().ToString(), GUILayout.Width(60));
                              EditorGUILayout.EndHorizontal();
                        }
                  }
                  #endregion

                  EditorGUILayout.Space(5F);
                  DrawTweenList();
            }

            private void OnPlayModeChanged(PlayModeStateChange _) => Repaint();

            private void DrawTweenList()
            {
                  // Filters
                  EditorGUILayout.BeginHorizontal();
                  {
                        searchFilter = EditorGUILayout.TextField("Search Tag:", searchFilter);

                        selectedTypeIndex = EditorGUILayout.Popup(selectedTypeIndex, typeFilters, GUILayout.Width(100));

                        if (GUILayout.Button("Clear", GUILayout.Width(50)))
                        {
                              searchFilter = string.Empty;
                              selectedTypeIndex = 0;
                        }
                  }
                  EditorGUILayout.EndHorizontal();

                  showCompleted = EditorGUILayout.Toggle("Show Completed", showCompleted);
                  EditorGUILayout.Space(5);

                  // Tween list
                  var tweens = GetFilteredTweens();
                  scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                  {
                        if (tweens.Count == 0)
                        {
                              EditorGUILayout.HelpBox("No tweens match the current filters.", MessageType.Info);
                        }
                        else
                        {
                              foreach (var tween in tweens)
                                    DrawTweenItem(tween);
                        }
                  }
                  EditorGUILayout.EndScrollView();
            }
           private void DrawTweenItem(ITween tween)
            {
                  EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                  {
                        EditorGUILayout.BeginHorizontal();
                        {
                              GUILayout.Label(tween.GetType().Name, EditorStyles.miniButtonMid, GUILayout.Width(80));
                              EditorGUILayout.LabelField($"Tag: {tween.Tag ?? "None"}", GUILayout.Width(140));

                              var prevColor = GUI.color;
                              GUI.color = GetPhaseColor(tween.Phase);
                              EditorGUILayout.LabelField($"[{tween.Phase}]", EditorStyles.boldLabel, GUILayout.Width(80));
                              GUI.color = prevColor;

                              GUILayout.FlexibleSpace();

                              if (tween.Phase == Phase.Active && GUILayout.Button("Pause", GUILayout.Width(60)))
                                    tween.Pause();
                              else if (tween.Phase == Phase.Paused && GUILayout.Button("Resume", GUILayout.Width(60)))
                                    tween.Resume();

                              if (GUILayout.Button("Kill", GUILayout.Width(50)))
                                    tween.Kill();
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel++;
                        EditorGUILayout.LabelField(tween.ToString(), EditorStyles.wordWrappedMiniLabel);
                        EditorGUI.indentLevel--;
                  }
                  EditorGUILayout.EndVertical();
            }
            private List<ITween> GetActiveTweens()
            {
                  var tweensField = typeof(Factory).GetField("tweens", BindingFlags.NonPublic | BindingFlags.Static);
                  if (tweensField?.GetValue(null) is List<ITween> list)
                        return list;

                  return new List<ITween>();
            }
            private List<ITween> GetFilteredTweens()
            {
                  var tweens = GetActiveTweens();
                  if (tweens == null || tweens.Count == 0)
                        return new List<ITween>();

                  if (!showCompleted)
                        tweens = tweens.Where(t => t.Phase != Phase.Complete && t.Phase != Phase.None).ToList();

                  if (!string.IsNullOrEmpty(searchFilter))
                        tweens = tweens.Where(t => t.Tag != null && t.Tag.Contains(searchFilter, StringComparison.OrdinalIgnoreCase)).ToList();

                  string selectedType = typeFilters[selectedTypeIndex];
                  if (selectedType != "All")
                        tweens = tweens.Where(t => t.GetType().Name.Contains(selectedType, StringComparison.OrdinalIgnoreCase)).ToList();

                  return tweens;
            }
            private static Color GetPhaseColor(Phase phase)
            {
                  return phase switch
                  {
                        Phase.Active => new Color(0.5f, 1f, 0.5f),
                        Phase.Paused => new Color(1f, 0.8f, 0.3f),
                        Phase.Complete => new Color(0.7f, 0.7f, 0.7f),
                        Phase.None => new Color(1f, 0.4f, 0.4f),
                        _ => Color.white
                  };
            }
      }
}