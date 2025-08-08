using System.Collections.Generic;

namespace Emp37.Tweening
{
      public sealed class Sequence : IElement
      {
            private readonly Queue<IElement> queue = new();

            private IElement current;

            public Phase Phase { get; private set; }
            public bool IsEmpty => queue.Count == 0 && current == null;

            public static Sequence Create => new();
            public Sequence Append(IElement element)
            {
                  if (!element.IsEmpty)
                  {
                        queue.Enqueue(element);
                  }
                  return this;
            }

            void IElement.Init()
            {
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;

                  if (current == null)
                  {
                        if (queue.Count == 0)
                        {
                              Phase = Phase.Complete;
                              return;
                        }
                        current = queue.Dequeue();
                        current.Init();
                  }
                  current.Update();
                  if (current.Phase is Phase.None or Phase.Complete)
                  {
                        current = null;
                  }
            }

            public void Pause()
            {
                  Phase = Phase is Phase.Active ? Phase.Paused : Phase;
                  current?.Pause();
            }
            public void Resume()
            {
                  Phase = Phase is Phase.Paused ? Phase.Active : Phase;
                  current?.Resume();
            }
            public void Kill()
            {
                  Phase = Phase.None;
                  current?.Kill();
                  while (queue.Count > 0) queue.Dequeue().Kill();
            }
      }
}