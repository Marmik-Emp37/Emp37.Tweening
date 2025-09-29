using System;

namespace Emp37.Tweening
{
      public sealed class Invoke : ITween
      {
            private readonly Action action;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => action == null;


            public Invoke(Action action)
            {
                  this.action = action;
            }

            void ITween.Init()
            {
                  Phase = Phase.Active;
            }
            void ITween.Update()
            {
                  Utils.SafeInvoke(action);
                  Phase = Phase.Complete;
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

            public override string ToString() => this.Summarize(action?.Method?.Name ?? "null");
      }
}