using System;

namespace Emp37.Tweening
{
      public sealed class Callback : ITween
      {
            private readonly Action action;

            bool ITween.AutoKill { get; set; } = true;
            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => action == null;
            public Info Info => new(nameof(Callback), new Info.Property("Method", action?.Method?.Name ?? "null"));

            internal Callback(Action action) => this.action = action;


            void ITween.Update()
            {
                  Utils.SafeInvoke(action);
                  Kill();
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
                  Phase = Phase.Active;
            }
      }
}