using UnityEngine;

namespace Emp37.Tweening
{
      public sealed partial class Factory : MonoBehaviour
      {
            private static readonly ElementArray tweens = new(64);
            public static int MaxTweens { get => tweens.Capacity; set => tweens.Capacity = value; }
            public int AvailableTweens => MaxTweens - tweens.Count;

            private void LateUpdate()
            {
                  for (int i = tweens.Count - 1; i >= 0; i--)
                  {
                        IElement element = tweens[i];

                        if (element.Phase is Phase.Active) element.Update();
                        if (element.Phase is not (Phase.Complete or Phase.None)) continue;

                        tweens.RemoveAt(i);
                        if (tweens.Count == 0) enabled = false;
                  }
            }

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
            public static void Pause() => tweens.ForEach(e => e.Pause());
            public static void Resume() => tweens.ForEach(e => e.Resume());
            public static void Kill() => tweens.ForEach(e => e.Kill());

            static partial void OnFactoryDestroy() => tweens.Clear();
      }
}