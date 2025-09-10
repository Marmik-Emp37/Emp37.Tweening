using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public sealed partial class Factory : MonoBehaviour
      {
            private static readonly ElementArray tweens = new(64);

            /// <summary>
            /// Maximum number of concurrent tweens.
            /// </summary>
            /// <remarks>Setting this value resizes the underlying pool. If the new value is less than the current active tween count, the capacity is clamped to prevent data loss.</remarks>
            public static int MaxTweens { get => tweens.Capacity; set => tweens.Capacity = value; }
            public int AvailableTweens => MaxTweens - tweens.Count;

            private void LateUpdate()
            {
                  for (int i = tweens.Count - 1; i >= 0; i--) // iterate backwards so RemoveAt (swap-remove) doesn't skip elements
                  {
                        IElement element = tweens[i];

                        if (element.Phase is Phase.Active) element.Update();
                        if (element.Phase is not (Phase.Complete or Phase.None)) continue;

                        tweens.RemoveAt(i);
                        if (tweens.Count == 0) enabled = false;
                  }
            }

            /// <summary>
            /// Starts playing a tween element.
            /// </summary>
            /// <param name="element">The tween element to execute</param>
            public static void Play(IElement element)
            {
                  if (!Application.isPlaying || element.IsEmpty) return;
                  if (!tweens.Add(element))
                  {
                        Debug.LogWarning($"{typeof(Factory).FullName}: Active tween limit ({MaxTweens}) reached. Increase {nameof(MaxTweens)} to allow more tweens.");
                        return;
                  }
                  element.Init();
                  instance.enabled = true;
            }

            private static void ProcessTweens(string tag, Action<IElement> action)
            {
                  if (string.IsNullOrEmpty(tag))
                  {
                        tweens.ForEach(action);
                  }
                  else
                  {
                        tweens.ForEach(element =>
                        {
                              if (!string.IsNullOrEmpty(element.Tag) && string.Equals(element.Tag, tag, StringComparison.Ordinal))
                              {
                                    action(element);
                              }
                        });
                  }
            }
            public static void Pause(string tag = null) => ProcessTweens(tag, element => element.Pause());
            public static void Resume(string tag = null) => ProcessTweens(tag, element => element.Resume());
            public static void Kill(string tag = null) => ProcessTweens(tag, element => element.Kill());

            static partial void OnFactoryDestroy() => tweens.Clear();
      }
}