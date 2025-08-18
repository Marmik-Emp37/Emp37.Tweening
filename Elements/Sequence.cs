using System;
using System.Collections.Generic;

namespace Emp37.Tweening.Element
{
      public sealed class Sequence : IElement
      {
            private readonly Queue<IElement> elementQueue;
            private IElement current;

            public Phase Phase { get; private set; }
            public bool IsEmpty => current == null && elementQueue.Count == 0;

            internal Sequence() => elementQueue = new();
            internal Sequence(params IElement[] elements) : this() => Append(elements);

            public Sequence Append(IElement element)
            {
                  if (!element.IsEmpty)
                  {
                        elementQueue.Enqueue(element);
                  }
                  return this;
            }
            public Sequence Append(params IElement[] elements)
            {
                  foreach (var e in elements)
                  {
                        Append(e);
                  }
                  return this;
            }
            public Sequence Append(float delay) => Append(new Delay(delay));
            public Sequence Append(Func<bool> waitUntil) => Append(new Delay(waitUntil));
            public Sequence Append(float delay, Delta type, Func<bool> predicate) => Append(new Delay(delay, type, predicate));

            public Sequence Join(params IElement[] elements) => Append(new Parallel(elements));

            void IElement.Init()
            {
                  if (Phase != Phase.None) return;
                 Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;

                  if (current == null)
                  {
                        if (elementQueue.Count == 0)
                        {
                              Phase = Phase.Complete;
                              return;
                        }
                        current = elementQueue.Dequeue();
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
                  if (Phase != Phase.Active) return;
                  Phase = Phase.Paused;
                  current?.Pause();
            }
            public void Resume()
            {
                  if (Phase != Phase.Paused) return;
                  Phase = Phase.Active;
                  current?.Resume();
            }
            public void Kill()
            {
                  Phase = Phase.None;
                  current?.Kill(); current = null;
                  elementQueue.Clear();
            }
      }
}
