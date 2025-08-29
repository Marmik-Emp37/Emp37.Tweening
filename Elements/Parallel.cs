using System.Collections.Generic;

namespace Emp37.Tweening.Element
{
      public sealed class Parallel : IElement
      {
            private readonly List<IElement> list;

            public Phase Phase { get; private set; }
            public bool IsEmpty => list.Count == 0;

            private Parallel() => list = new();
            internal Parallel(params IElement[] elements) : this()
            {
                  for (int i = 0; i < elements.Length; i++)
                  {
                        var e = elements[i];
                        if (!e.IsEmpty) list.Add(e);
                  }
            }
            internal Parallel(IEnumerable<IElement> elements) : this()
            {
                  foreach (var e in elements)
                  {
                        if (!e.IsEmpty) list.Add(e);
                  }
            }

            void IElement.Init()
            {
                  if (Phase != Phase.None) return;

                  foreach (var e in list) e.Init();
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;

                  for (int i = list.Count - 1; i >= 0; i--)
                  {
                        IElement current = list[i];
                        current.Update();
                        if (current.Phase is Phase.Complete or Phase.None)
                        {
                              int last = list.Count - 1;
                              list[i] = list[last];
                              list.RemoveAt(last);

                              if (list.Count == 0) Phase = Phase.Complete;
                        }
                  }
            }

            public void Pause()
            {
                  if (Phase != Phase.Active) return;

                  foreach (var e in list) e.Pause();
                  Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase != Phase.Paused) return;

                  foreach (var e in list) e.Resume();
                  Phase = Phase.Active;
            }
            public void Kill()
            {
                  foreach (var e in list) e.Kill();
                  list.Clear();
                  Phase = Phase.None;
            }
      }
}