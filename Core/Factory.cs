using System;

using UnityEngine;

namespace Emp37.Tweening
{
      [AddComponentMenu(""), DisallowMultipleComponent]
      public sealed class Factory : MonoBehaviour
      {
            private static Factory instance = null!;

            private static IElement[] elements = new IElement[64];
            private static int count;

            public static int ActiveTweens => count;
            public static int MaxTweens
            {
                  get => elements.Length;
                  set
                  {
                        if (value == elements.Length) return;
                        int limit = Mathf.Max(value, count);
                        if (limit == count)
                        {
                              Log.Warning($"Requested capacity {value} is below active count ({count}). Using {limit}.");
                        }
                        Array.Resize(ref elements, limit);
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
                        IElement element = elements[i];

                        if (element.Phase is Phase.Active) element.Update();
                        if (element.Phase is not (Phase.Complete or Phase.None)) continue;

                        int last = --count;
                        elements[i] = elements[last];
                        elements[last] = null;

                        if (count == 0) enabled = false;
                  }
            }
            private void OnDestroy()
            {
                  if (instance != this) return;

                  Array.Clear(elements, 0, count); // bulk-null active range
                  count = 0;

                  instance = null;
            }

            public static void Play(IElement element)
            {
                  if (!Application.isPlaying || element.IsEmpty) return;
                  if (count == MaxTweens)
                  {
                        Log.Warning($"{typeof(Factory).FullName}: Active tween limit ({MaxTweens}) reached. Increase {nameof(MaxTweens)} to allow more tweens.");
                        return;
                  }

                  elements[count++] = element;
                  element.Init();

                  instance.enabled = true;
            }

            public static void Pause(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < count; i++) elements[i].Pause();
                  }
                  else
                  {
                        for (int i = 0; i < count; i++)
                        {
                              IElement e = elements[i];
                              if (e.Tag != null && e.Tag.Equals(tag, StringComparison.Ordinal)) e.Pause();
                        }
                  }
            }
            public static void Resume(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < count; i++) elements[i].Resume();
                  }
                  else
                  {
                        for (int i = 0; i < count; i++)
                        {
                              IElement e = elements[i];
                              if (e.Tag != null && e.Tag.Equals(tag, StringComparison.Ordinal)) e.Resume();
                        }
                  }
            }
            public static void Kill(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < count; i++) elements[i].Kill();
                  }
                  else
                  {
                        for (int i = 0; i < count; i++)
                        {
                              IElement e = elements[i];
                              if (e.Tag != null && e.Tag.Equals(tag, StringComparison.Ordinal)) e.Kill();
                        }
                  }
            }
      }
}