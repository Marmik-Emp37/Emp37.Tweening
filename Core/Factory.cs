using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Tweening
{
      [DefaultExecutionOrder(1), AddComponentMenu(""), DisallowMultipleComponent]
      public sealed class Factory : MonoBehaviour
      {
            private static Factory instance = null!;

            private static readonly List<ITween> tweens = new(128);

            public static IReadOnlyList<ITween> Tweens => tweens;
            public static int ActiveTweens => tweens.Count;


            private Factory()
            {
            }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            private static void Initialize()
            {
                  if (instance != null)
                  {
                        DestroyImmediate(instance.gameObject);
                        instance = null;
                  }
                  instance = new GameObject("~" + nameof(Factory)).AddComponent<Factory>();
            }

            private void Awake()
            {
                  enabled = false;
                  DontDestroyOnLoad(this);
            }
            private void LateUpdate()
            {
                  for (int i = tweens.Count - 1; i >= 0; i--)
                  {
                        ITween tween = tweens[i];

                        if (tween.Phase is Phase.Active) tween.Update();
                        if (tween.Phase is not (Phase.Finished or Phase.None)) continue;

                        int last = tweens.Count - 1;
                        if (i != last) tweens[i] = tweens[last];
                        tweens.RemoveAt(last);

                        if (tweens.Count == 0)
                        {
                              enabled = false;
                              break;
                        }
                  }
            }
            private void OnDestroy()
            {
                  if (instance != this) return;

                  tweens.Clear();
                  instance = null;
            }

            public static void Play(ITween tween)
            {
                  if (instance == null || tween == null || tween.IsEmpty) return;
                  if (tweens.Count == tweens.Capacity)
                  {
                        Log.Warning($"[{typeof(Factory).FullName}] Tween capacity ({tweens.Capacity}) reached. Factory is scaling up, check for leaks or unintended bursts.");
                  }
                  tween.Init();
                  tweens.Add(tween);

                  instance.enabled = true;
            }

            public static void Pause(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < tweens.Count; i++) tweens[i].Pause();
                  }
                  else
                  {
                        for (int i = 0; i < tweens.Count; i++)
                        {
                              ITween e = tweens[i];
                              if (string.Equals(e.Tag, tag, StringComparison.Ordinal)) e.Pause();
                        }
                  }
            }
            public static void Resume(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < tweens.Count; i++) tweens[i].Resume();
                  }
                  else
                  {
                        for (int i = 0; i < tweens.Count; i++)
                        {
                              ITween e = tweens[i];
                              if (string.Equals(e.Tag, tag, StringComparison.Ordinal)) e.Resume();
                        }
                  }
            }
            public static void Kill(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < tweens.Count; i++)
                        {
                              tweens[i].Kill();
                        }
                        tweens.Clear();
                        instance.enabled = false;
                  }
                  else
                  {
                        for (int i = 0; i < tweens.Count; i++)
                        {
                              ITween e = tweens[i];
                              if (string.Equals(e.Tag, tag, StringComparison.Ordinal)) e.Kill();
                        }
                  }
            }
      }
}