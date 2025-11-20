using System;

namespace Emp37.Tweening
{
      public sealed class Callback : ITween
      {
            private readonly Action action;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => action == null;
            public Info Info => new(nameof(Callback), 1F, new Info.Property("Method", action?.Method?.Name ?? "null"));


            public Callback(Action action) => this.action = action;

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
      }
}