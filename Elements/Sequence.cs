using System.Collections.Generic;

namespace Emp37.Tweening.Element
{
      public sealed class Sequence : IElement
      {
            private readonly Queue<IElement> queue;
            private IElement current;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => current == null && queue.Count == 0;

            internal Sequence() => queue = new();
            internal Sequence(params IElement[] elements) : this() => Append(elements);
            internal Sequence(IEnumerable<IElement> elements) : this() => Append(elements);

            public Sequence Append(IElement element)
            {
                  if (!element.IsEmpty)
                  {
                        queue.Enqueue(element);
                  }
                  return this;
            }
            public Sequence Append(params IElement[] elements)
            {
                  for (int i = 0; i < elements.Length; i++)
                  {
                        Append(elements[i]);
                  }
                  return this;
            }
            public Sequence Append(IEnumerable<IElement> elements)
            {
                  foreach (var e in elements)
                  {
                        Append(e);
                  }
                  return this;
            }

            void IElement.Init()
            {
                  ActivateNext();
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (current.Phase is Phase.Active) current.Update();
                  if (current.Phase is not Phase.Complete and not Phase.None) return;

                  if (queue.Count == 0)
                  {
                        current = null;
                        Phase = Phase.Complete;
                  }
                  else
                  {
                        ActivateNext();
                  }
            }

            public void Pause()
            {
                  if (Phase != Phase.Active) return;

                  current?.Pause();
                  Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase != Phase.Paused) return;

                  current?.Resume();
                  Phase = Phase.Active;
            }
            public void Kill()
            {
                  current?.Kill(); current = null;
                  queue.Clear();
                  Phase = Phase.None;
            }

            private void ActivateNext()
            {
                  current = queue.Dequeue();
                  current.Init();
            }
      }
}