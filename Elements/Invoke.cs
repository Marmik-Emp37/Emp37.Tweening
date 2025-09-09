using System;

namespace Emp37.Tweening.Element
{
      public sealed class Invoke : IElement
      {
            private readonly Action action;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => action == null;

            private Invoke()
            {
            }
            public Invoke(Action action) => this.action = action;

            void IElement.Init() => Phase = Phase.Active;
            void IElement.Update()
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
            public void Kill() => Phase = Phase.None;
      }
}