using System;

using UnityEngine;

namespace Emp37.Tweening
{
      [DefaultExecutionOrder(1), AddComponentMenu(""), DisallowMultipleComponent]
      public sealed class Factory : MonoBehaviour
      {
            private static Factory instance = null!;

            private static ITween[] tweens;
            private static int count;

            public static int ActiveTweens => count;
            public static int MaxTweens
            {
                  get => tweens.Length;
                  set
                  {
                        if (value == tweens.Length) return;

                        int limit = Mathf.Max(count, value);
                        if (limit == count)
                        {
                              Log.Warning($"[{typeof(Factory).FullName}] Cannot shrink tween capacity to {value} as {count} tweens are currently active. Keeping capacity at {limit}.");
                        }
                        Array.Resize(ref tweens, limit);
                  }
            }


            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
            private static void Initialize()
            {
                  if (instance != null)
                  {
                        DestroyImmediate(instance.gameObject);
                        instance = null;
                  }
                  instance = new GameObject("~" + nameof(Factory)) { hideFlags = HideFlags.DontSave }.AddComponent<Factory>();
            }

            private Factory()
            {
                  tweens = new ITween[64];
            }

            private void Awake()
            {
                  enabled = false;
                  DontDestroyOnLoad(this);
            }
            private void LateUpdate()
            {
                  for (int i = count - 1; i >= 0; i--) // iterate backwards so swap-removal doesn't skip elements
                  {
                        ITween tween = tweens[i];

                        if (tween.Phase is Phase.Active) tween.Update();
                        if (tween.Phase is not (Phase.Complete or Phase.None)) continue;

                        int last = --count;
                        tweens[i] = tweens[last];
                        tweens[last] = null;

                        if (count == 0) enabled = false;
                  }
            }
            private void OnDestroy()
            {
                  if (instance != this) return;

                  Clear();
                  instance = null;
            }

            public static void Play(ITween tween)
            {
                  if (!Application.isPlaying || tween == null || tween.IsInvalid) return;
                  if (count == MaxTweens)
                  {
                        Log.Warning($"[{typeof(Factory).FullName}] Active tween limit ({MaxTweens}) reached. Increase '{nameof(MaxTweens)}' to allow more tweens.");
                        return;
                  }

                  tweens[count++] = tween;
                  tween.Init();

                  instance.enabled = true;
            }

            public static void Pause(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < count; i++) tweens[i].Pause();
                  }
                  else
                  {
                        for (int i = 0; i < count; i++)
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
                        for (int i = 0; i < count; i++) tweens[i].Resume();
                  }
                  else
                  {
                        for (int i = 0; i < count; i++)
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
                        for (int i = 0; i < count; i++)
                        {
                              tweens[i].Kill();
                        }
                        Clear();
                  }
                  else
                  {
                        for (int i = 0; i < count; i++)
                        {
                              ITween e = tweens[i];
                              if (string.Equals(e.Tag, tag, StringComparison.Ordinal)) e.Kill();
                        }
                  }
            }

            private static void Clear()
            {
                  Array.Clear(tweens, 0, count);
                  count = 0;
            }
      }
}