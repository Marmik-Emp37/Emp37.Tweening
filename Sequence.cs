using System.Collections.Generic;

namespace Emp37.Tweening
{
      public sealed class Sequence : IElement
      {
            private readonly Queue<IElement> queue = new();

            private IElement current;

            public Phase Phase { get; private set; }
            public bool IsEmpty => queue.Count == 0;

            public static Sequence Create => new();
            public Sequence Append(IElement element)
            {
                  if (!element.IsEmpty) queue.Enqueue(element);
                  return this;
            }

            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;

                  if (current == null)
                  {
                        if (queue.Count == 0)
                        {
                              Phase = Phase.Complete;
                        }
                        current = queue.Dequeue();
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
            }
            public void Resume()
            {
                  Phase = Phase is Phase.Paused ? Phase.Active : Phase;
            }
            public void Kill()
            {
                  Phase = Phase.None;
                  current.Kill();
                  queue.Clear();
            }
            public void Reset()
            {
                  Phase = Phase.Active;
            }
      }
}