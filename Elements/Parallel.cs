using System.Collections.Generic;
using System.Linq;

namespace Emp37.Tweening.Element
{
      public sealed class Parallel : IElement
      {
            private readonly List<IElement> elementList;

            public Phase Phase { get; private set; }
            public bool IsEmpty => elementList.Count == 0;

            private Parallel()
            {
            }
            internal Parallel(params IElement[] elements) => elementList = elements?.Where(e => !e.IsEmpty).ToList() ?? new();

            void IElement.Init()
            {
                  if (Phase != Phase.None) return;

                  foreach (var e in elementList) e.Init();
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;

                  for (int i = elementList.Count - 1; i > -1; i--)
                  {
                        var element = elementList[i];
                        if (element.Phase == Phase.Active)
                        {
                              element.Update();
                        }
                        if (element.Phase is Phase.None or Phase.Complete)
                        {
                              elementList.RemoveAt(i);
                              if (elementList.Count == 0)
                              {
                                    Phase = Phase.Complete;
                                    return;
                              }
                        }
                  }
            }

            public void Pause()
            {
                  if (Phase != Phase.Active) return;

                  foreach (var e in elementList) e.Pause();
                  Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase != Phase.Paused) return;

                  foreach (var e in elementList) e.Resume();
                  Phase = Phase.Active;
            }
            public void Kill()
            {
                  foreach (var e in elementList) e.Kill();
                  Phase = Phase.None;
                  elementList.Clear();
            }
      }
}