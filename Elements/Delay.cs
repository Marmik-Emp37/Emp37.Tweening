using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public class Delay : ITween
      {
            private float remainingTime;
            private readonly float originalTime;
            private readonly Delta timeMode;
            private readonly Func<bool> predicate;

            bool ITween.AutoKill { get; set; }
            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => originalTime <= 0F && predicate == null;
            public Info Info => new(nameof(Delay), remainingTime == 0F ? 1F : 1F - (remainingTime / originalTime), new("Remaining", remainingTime), new("Predicate", predicate?.Method.Name ?? "null"), new("Time Mode", timeMode));

            internal Delay(float duration, Delta mode) { originalTime = duration; timeMode = mode; }
            internal Delay(Func<bool> until) => predicate = until;
            internal Delay(float duration, Func<bool> until, Delta mode) { originalTime = duration; timeMode = mode; predicate = until; }


            void ITween.Update()
            {
                  if (remainingTime > 0F)
                  {
                        float delta = timeMode == Delta.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                        remainingTime = Mathf.Max(0F, remainingTime - delta);
                  }
                  else
                  if (predicate == null || predicate())
                  {
                        Kill();
                  }
            }

            public void Pause()
            {
                  if (Phase == Phase.Active) Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase == Phase.Paused) Phase = Phase.Active;
            }
            public void Kill()
            {
                  Phase = Phase.None;
            }
            public void Reset()
            {
                  remainingTime = 0F;
                  Phase = Phase.Active;
            }
      }
}