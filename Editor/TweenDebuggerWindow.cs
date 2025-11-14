using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

namespace Emp37.Tweening.Editor
{
      using static EditorStyles;

      public sealed class TweenDebuggerWindow : EditorWindow
      {
            private struct Stats
            {
                  public string Type;
                  public int Total, Active, Paused;

                  public Stats(string name, int total, int active, int paused)
                  {
                        Type = name;
                        Total = total; Active = active; Paused = paused;
                  }
            }

            private const double REFRESH_INTERVAL = 0.1;
            private double lastRefreshTime;

            private string searchQuery = string.Empty;

            private Vector2 scrollPosition;


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
                  if (!Application.isPlaying) return;

                  double time = EditorApplication.timeSinceStartup;
                  if (time - lastRefreshTime > REFRESH_INTERVAL)
                  {
                        lastRefreshTime = time;
                        Repaint();
                  }
            }
            private void OnGUI()
            {
                  #region T O O L B A R
                  using (new EditorGUILayout.HorizontalScope(toolbar))
                  {
                        if (button("Pause All")) Factory.Pause();
                        if (button("Resume All")) Factory.Resume();
                        if (button("Kill All")) Factory.Kill();

                        GUILayout.FlexibleSpace();

                        static bool button(string name) => GUILayout.Button(name, toolbarButton, GUILayout.MinWidth(40F), GUILayout.MaxWidth(80F));
                  }
                  #endregion

                  IReadOnlyList<ITween> list = Factory.Tweens;
                  bool hasTweens = list != null && list.Count > 0;

                  #region S T A T I S T I C S
                  using (new EditorGUILayout.VerticalScope(helpBox))
                  {
                        // title bar
                        stringRow(boldLabel, nameof(Stats.Type), nameof(Stats.Total), nameof(Stats.Active), nameof(Stats.Paused));

                        if (hasTweens)
                        {
                              Dictionary<Type, Stats> typeStats = new(8);

                              int total = list.Count;
                              Stats composite = new("-", total, 0, 0);

                              for (int i = 0; i < total; i++)
                              {
                                    ITween element = list[i];
                                    Type type = element.GetType();

                                    if (!typeStats.TryGetValue(type, out Stats value))
                                    {
                                          value = default;
                                          value.Type = type.Name;
                                    }
                                    value.Total++;

                                    switch (element.Phase)
                                    {
                                          case Phase.Active:
                                                composite.Active++;
                                                value.Active++;
                                                break;

                                          case Phase.Paused:
                                                composite.Paused++;
                                                value.Paused++;
                                                break;
                                    }
                                    typeStats[type] = value;
                              }
                              statisticRow(composite);

                              // separator
                              Rect rect = EditorGUILayout.GetControlRect(false, 2F);
                              EditorGUI.DrawRect(rect, new(0.3F, 0.3F, 0.3F));

                              foreach (var entry in typeStats) statisticRow(entry.Value);
                        }
                        else
                        {
                              statisticRow(default);
                        }

                        static void stringRow(GUIStyle style, string value1, string value2, string value3, string value4)
                        {
                              EditorGUILayout.BeginHorizontal();
                              foreach (string value in new string[4] { value1, value2, value3, value4 }) EditorGUILayout.LabelField(value, style, GUILayout.MinWidth(40F));
                              EditorGUILayout.EndHorizontal();
                        }
                        static void statisticRow(Stats stats) => stringRow(label, stats.Type, stats.Total.ToString(), stats.Active.ToString(), stats.Paused.ToString());
                  }
                  #endregion

                  EditorGUILayout.Space(4F);

                  #region S E A R C H   B A R
                  using (new EditorGUILayout.HorizontalScope())
                  {
                        GUIContent label = new(EditorGUIUtility.IconContent("d_Search Icon")) { text = "Tag:" };
                        searchQuery = EditorGUILayout.TextField(label, searchQuery);

                        if (GUILayout.Button("Clear", GUILayout.Width(50F)))
                        {
                              searchQuery = string.Empty;
                              GUIUtility.keyboardControl = 0;
                        }
                  }
                  #endregion

                  EditorGUILayout.Space(4F);

                  #region L I S T
                  using EditorGUILayout.ScrollViewScope scrollView = new(scrollPosition);
                  scrollPosition = scrollView.scrollPosition;

                  if (hasTweens)
                  {
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                        {
                              list = list.Where(t => t.Tag != null && t.Tag.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                              if (list.Count == 0)
                              {
                                    EditorGUILayout.HelpBox("No tweens match the current filters.", MessageType.Info);
                                    return;
                              }
                        }

                        foreach (ITween tween in list)
                        {
                              using (new EditorGUILayout.HorizontalScope())
                              {
                                    EditorGUILayout.HelpBox(tween.ToString(), MessageType.None);

                                    GUILayout.FlexibleSpace();

                                    bool isPaused = tween.Phase == Phase.Paused;
                                    if (GUILayout.Button(isPaused ? "Resume" : "Pause", GUILayout.Width(60F)))
                                    {
                                          if (isPaused) tween.Resume();
                                          else tween.Pause();
                                    }
                                    if (GUILayout.Button("Kill", GUILayout.Width(60F))) tween.Kill();
                              }
                        }
                  }
                  else
                  {
                        EditorGUILayout.HelpBox("No tweens are running.", MessageType.Info);
                  }
                  #endregion
            }

            private void OnPlayModeChanged(PlayModeStateChange _) => Repaint();
      }
}