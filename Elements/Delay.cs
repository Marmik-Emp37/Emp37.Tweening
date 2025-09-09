using System;

using UnityEngine;

namespace Emp37.Tweening.Element
{
      public class Delay : IElement
      {
            private float time;
            private readonly Delta timeMode;
            private readonly Func<bool> predicate;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => time <= 0F && predicate == null;


            private Delay()
            {
            }
            internal Delay(Func<bool> waitUntil) => predicate = waitUntil;
            internal Delay(float value, Delta mode = Delta.Scaled, Func<bool> waitUntil = null) : this(waitUntil)
            {
                  time = value;
                  timeMode = mode;
            }

            void IElement.Init() => Phase = Phase.Active;
            void IElement.Update()
            {
                  if (time > 0F)
                  {
                        float delta = timeMode == Delta.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                        time = Mathf.Max(0F, time - delta);
                  }
                  else if (predicate == null || predicate())
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
            public void Kill() => Phase = Phase.None;
      }
}