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
                        if (tweens.Count == 0)
                        {
                              enabled = false;
                              return; // short-circuit if multiple tweens finish at the same frame
                        }
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
                        Logger.Warning($"{typeof(Factory).FullName}: Active tween limit ({MaxTweens}) reached. Increase {nameof(MaxTweens)} to allow more tweens.");
                        return;
                  }
                  element.Init();
                  instance.enabled = true;
            }
            public static void Pause() => tweens.ForEach(element => element.Pause());
            public static void Resume() => tweens.ForEach(element => element.Resume());
            public static void Kill() => tweens.ForEach(element => element.Kill());
            public static void Pause(string tag) => tweens.ForEachTagged(tag, element => element.Pause());
            public static void Resume(string tag) => tweens.ForEachTagged(tag, element => element.Resume());
            public static void Kill(string tag) => tweens.ForEachTagged(tag, element => element.Kill());

            static partial void OnFactoryDestroy() => tweens.Clear();
      }
}