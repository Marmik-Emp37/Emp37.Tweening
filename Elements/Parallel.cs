using System.Collections.Generic;

namespace Emp37.Tweening.Element
{
      public sealed class Parallel : IElement
      {
            private readonly List<IElement> list;

            public string Tag { get; set; }
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
                  foreach (var e in list) e.Init();
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  for (int id = list.Count - 1; id >= 0; id--)
                  {
                        IElement current = list[id];

                        if (current.Phase is Phase.Active) current.Update();
                        if (current.Phase is not Phase.Complete and not Phase.None) continue;

                        int last = list.Count - 1;
                        list[id] = list[last];
                        list.RemoveAt(last);

                        if (list.Count == 0) Phase = Phase.Complete;
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