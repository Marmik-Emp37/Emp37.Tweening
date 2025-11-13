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

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => originalTime <= 0F && predicate == null;
            public TweenInfo Info => new(nameof(Delay), remainingTime == 0F ? 1F : 1F - (remainingTime / originalTime), ("Remaining", remainingTime), ("Predicate", predicate?.Method.Name ?? "null"), ("Time Mode", timeMode));


            internal Delay(float duration, Delta mode) { originalTime = duration; timeMode = mode; }
            internal Delay(Func<bool> until) => predicate = until;
            internal Delay(float duration, Func<bool> until, Delta mode) { originalTime = duration; timeMode = mode; predicate = until; }

            void ITween.Init()
            {
                  remainingTime = originalTime;
                  Phase = Phase.Active;
            }
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
                        Phase = Phase.Complete;
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
      }
}