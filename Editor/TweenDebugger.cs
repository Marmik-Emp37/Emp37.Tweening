using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

namespace Emp37.Tweening.Editor
{
      using static EditorStyles;

      public sealed class TweenDebugger : EditorWindow
      {
            private struct Stats
            {
                  public string Type;
                  public int Total, Active, Paused;
                  public Stats(string type, int total, int active, int paused) { Type = type; Total = total; Active = active; Paused = paused; }
            }

            private const double REFRESH_INTERVAL = 0.1;
            private double lastRefreshTime;

            private string searchQuery = string.Empty;

            private Vector2 scrollPosition;

            private static readonly Color activeColor = new(0.3F, 0.9F, 0.3F), pausedColor = new(1F, 0.7F, 0.2F);

            private readonly List<ITween> trackedTweens = new();
            private readonly List<(Info Info, string Tag)> history = new();


            [MenuItem("Tools/Emp37/Tweening.Debugger")]
            public static void ShowWindow()
            {
                  var window = GetWindow<TweenDebugger>("Tween Debugger");
                  window.minSize = new Vector2(420F, 320F);
                  window.Show();
            }

            private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeChanged;
            private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            private void Update()
            {
                  if (!EditorApplication.isPlaying) return;

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

                        if (button("Clear History")) history.Clear();

                        static bool button(string name) => GUILayout.Button(name, toolbarButton, GUILayout.MinWidth(40F), GUILayout.MaxWidth(80F));
                  }
                  #endregion

                  IReadOnlyList<ITween> runningTweens = Factory.Tweens;
                  bool isActive = runningTweens != null && runningTweens.Count > 0;

                  if (isActive) trackedTweens.AddRange(Factory.Tweens.Where(tween => !trackedTweens.Contains(tween)));
                  for (int i = trackedTweens.Count - 1; i >= 0; i--)
                  {
                        ITween tween = trackedTweens[i];
                        if (tween.Phase is not (Phase.Complete or Phase.None)) continue;

                        history.Add((tween.Info, tween.Tag));
                        trackedTweens.RemoveAt(i);
                  }

                  #region S T A T I S T I C S
                  using (new EditorGUILayout.VerticalScope(helpBox))
                  {
                        // title bar
                        stringRow(boldLabel, nameof(Stats.Type), nameof(Stats.Total), nameof(Stats.Active), nameof(Stats.Paused));

                        if (isActive)
                        {
                              Dictionary<Type, Stats> typeStats = new(8);
                              int total = runningTweens.Count;
                              Stats composite = new("-", total, 0, 0);

                              for (int i = 0; i < total; i++)
                              {
                                    ITween tween = runningTweens[i];
                                    Type type = tween.GetType();

                                    if (!typeStats.TryGetValue(type, out Stats value))
                                    {
                                          value.Type = type.Name;
                                    }
                                    value.Total++;

                                    switch (tween.Phase)
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

                        if (GUILayout.Button("X", GUILayout.Width(25F)))
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

                  List<ITween> tweens = isActive ? runningTweens.ToList() : null;
                  List<(Info Info, string Tag)> histories = history;

                  if (!string.IsNullOrWhiteSpace(searchQuery))
                  {
                        if (isActive) tweens = Filter(tweens, t => t.Tag);
                        histories = Filter(histories, t => t.Tag);

                        if (histories.Count == 0 && tweens.Count == 0)
                        {
                              EditorGUILayout.LabelField("No tweens found.", GUILayout.Height(21F));
                              return;
                        }

                        List<T> Filter<T>(IEnumerable<T> source, Func<T, string> tagSelector) =>
                              source.Where(item => { string tag = tagSelector(item); return tag != null && tag.Contains(searchQuery, StringComparison.OrdinalIgnoreCase); }).ToList();
                  }

                  if (isActive)
                  {
                        foreach (ITween tween in tweens)
                        {
                              DrawTween(tween.Info, tween.Tag, tween.Phase is Phase.Active ? activeColor : pausedColor);

                              // control buttons
                              using (new EditorGUILayout.HorizontalScope(GUILayout.Height(21F)))
                              {
                                    bool isPaused = tween.Phase == Phase.Paused;
                                    if (GUILayout.Button(isPaused ? "Resume" : "Pause", GUILayout.ExpandHeight(true)))
                                    {
                                          if (isPaused) tween.Resume();
                                          else tween.Pause();
                                    }
                                    if (GUILayout.Button("Kill", GUILayout.ExpandHeight(true)))
                                    {
                                          tween.Kill();
                                    }
                              }
                        }
                  }
                  foreach ((Info info, string tag) in histories)
                  {
                        DrawTween(info, tag, Color.gray);
                  }
                  #endregion
            }

            private void OnPlayModeChanged(PlayModeStateChange _) => Repaint();
            private void DrawTween(Info info, string tag, Color color)
            {
                  using (new EditorGUILayout.VerticalScope(helpBox))
                  {
                        EditorGUILayout.LabelField($"{info.Title} : {(string.IsNullOrEmpty(tag) ? "(none)" : tag)}", whiteLargeLabel, GUILayout.Height(21F));

                        Rect rect = EditorGUILayout.GetControlRect(false, 2F);
                        rect.width *= info.Ratio;
                        EditorGUI.DrawRect(rect, color);

                        foreach (var property in info.Properties) EditorGUILayout.LabelField($"{property.Label}: {property.Value}", textField);
                  }
            }
      }
}