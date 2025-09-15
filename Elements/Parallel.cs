using System.Collections.Generic;

namespace Emp37.Tweening.Element
{
      public sealed class Parallel : IElement
      {
            const int SwapStrategyThreshold = 15;

            private readonly List<IElement> elements;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => elements.Count == 0;


            internal Parallel(IEnumerable<IElement> elements)
            {
                  this.elements = new();
                  foreach (var element in elements)
                  {
                        if (!element.IsEmpty)
                        {
                              this.elements.Add(element);
                        }
                  }
            }

            void IElement.Init()
            {
                  foreach (var element in elements) element.Init();
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  for (int i = elements.Count - 1; i >= 0; i--)
                  {
                        IElement current = elements[i];

                        if (current.Phase is Phase.Active) current.Update();
                        if (current.Phase is not Phase.Complete and not Phase.None) continue;

                        FastRemoveElement(i);
                        if (elements.Count == 0) Phase = Phase.Complete;
                  }
            }

            public void Pause()
            {
                  if (Phase != Phase.Active) return;

                  foreach (var element in elements) element.Pause();
                  Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase != Phase.Paused) return;

                  foreach (var element in elements) element.Resume();
                  Phase = Phase.Active;
            }
            public void Kill()
            {
                  foreach (var element in elements) element.Kill();
                  elements.Clear();
                  Phase = Phase.None;
            }

            private void FastRemoveElement(int index)
            {
                  if (elements.Count > SwapStrategyThreshold)
                  {
                        int last = elements.Count - 1;
                        elements[index] = elements[last];
                        index = last;
                  }
                  elements.RemoveAt(index);
            }
      }
}