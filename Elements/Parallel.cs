using System.Collections.Generic;

namespace Emp37.Tweening
{
      public sealed class Parallel : ITween
      {
            private readonly List<ITween> tweens;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => tweens.Count == 0;
            public Info Info
            {
                  get
                  {
                        int total = tweens.Count, active = 0, paused = 0, finished = 0;
                        float ratio = 1F;
                        if (total > 0)
                        {
                              for (int i = 0; i < total; i++)
                              {
                                    ITween tween = tweens[i];

                                    switch (tween.Phase) { case Phase.Active: active++; break; case Phase.Paused: paused++; break; default: finished++; break; }
                                    float progress = tween.Info.Ratio;

                                    if (progress < ratio) ratio = progress;
                              }
                        }
                        return new(nameof(Parallel), ratio, new("Children", tweens.Count), new("Active", active), new("Paused", paused), new("Finished", finished));
                  }
            }


            internal Parallel(IEnumerable<ITween> tweens)
            {
                  this.tweens = new();
                  foreach (ITween tween in tweens)
                        if (tween != null && !tween.IsEmpty) this.tweens.Add(tween);
            }

            void ITween.Init()
            {
                  for (int i = 0, count = tweens.Count; i < count; i++)
                  {
                        tweens[i].Init();
                  }
                  Phase = Phase.Active;
            }
            void ITween.Update()
            {
                  for (int i = tweens.Count - 1; i >= 0; i--)
                  {
                        ITween current = tweens[i];

                        if (current.Phase is Phase.Active) current.Update();
                        if (current.Phase is not Phase.Finished and not Phase.None) continue;

                        int last = tweens.Count - 1;
                        if (i != last) tweens[i] = tweens[last];
                        tweens.RemoveAt(last);

                        if (tweens.Count == 0) Phase = Phase.Finished;
                  }
            }

            public void Pause()
            {
                  if (Phase != Phase.Active) return;
                  for (int i = 0, count = tweens.Count; i < count; i++)
                  {
                        tweens[i].Pause();
                  }
                  Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase != Phase.Paused) return;
                  for (int i = 0, count = tweens.Count; i < count; i++)
                  {
                        tweens[i].Resume();
                  }
                  Phase = Phase.Active;
            }
            public void Kill()
            {
                  for (int i = 0, count = tweens.Count; i < count; i++)
                  {
                        tweens[i].Kill();
                  }
                  tweens.Clear();
                  Phase = Phase.None;
            }
      }
}