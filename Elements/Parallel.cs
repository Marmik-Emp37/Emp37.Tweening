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
                        var element = elements[i];
                        if (!element.IsEmpty) list.Add(element);
                  }
            }
            internal Parallel(IEnumerable<IElement> elements) : this()
            {
                  foreach (var element in elements)
                  {
                        if (!element.IsEmpty) list.Add(element);
                  }
            }

            void IElement.Init()
            {
                  foreach (var element in list) element.Init();
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  for (int i = list.Count - 1; i >= 0; i--)
                  {
                        IElement element = list[i];

                        if (element.Phase is Phase.Active) element.Update();
                        if (element.Phase is not Phase.Complete and not Phase.None) continue;

                        int last = list.Count - 1;
                        list[i] = list[last];
                        list.RemoveAt(last);

                        if (list.Count == 0) Phase = Phase.Complete;
                  }
            }

            public void Pause()
            {
                  if (Phase != Phase.Active) return;

                  foreach (var element in list) element.Pause();
                  Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase != Phase.Paused) return;

                  foreach (var element in list) element.Resume();
                  Phase = Phase.Active;
            }
            public void Kill()
            {
                  foreach (var element in list) element.Kill();
                  list.Clear();
                  Phase = Phase.None;
            }
      }
}