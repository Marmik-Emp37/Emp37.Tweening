using System.Collections.Generic;

namespace Emp37.Tweening
{
      public sealed class Sequence : ITween
      {
            private readonly List<ITween> all;
            private int index;
            private ITween current;
            private int total;
            private bool isAutoKill = true;

            bool ITween.AutoKill { get => isAutoKill; set => isAutoKill = value; }
            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => all.Count == 0;
            public Info Info
            {
                  get
                  {
                        if (total == 0) return new Info(nameof(Sequence), 1F);

                        int completed = index;
                        float currentRatio = current != null ? current.Info.Ratio : 0F, ratio = (completed + currentRatio) / total;

                        return new Info(
                              nameof(Sequence),
                              ratio,
                              new("Current", current == null ? "null" : current.Info.Title),
                              new("Pending", total - index - (current != null ? 1 : 0)));
                  }
            }


            internal Sequence() => all = new();
            internal Sequence(IEnumerable<ITween> tweens) : this() => Append(tweens);

            public Sequence Append(ITween tween)
            {
                  if (tween == null || tween.IsEmpty) return this;

                  all.Add(tween);
                  return this;
            }
            public Sequence Append(IEnumerable<ITween> tweens)
            {
                  if (tweens == null) return this;
                  foreach (ITween tween in tweens) Append(tween);
                  return this;
            }
            public Sequence Append(params ITween[] tweens) => Append((IEnumerable<ITween>) tweens);

            void ITween.Update()
            {
                  if (current.Phase is Phase.Active) current.Update();
                  if (current.Phase is not Phase.Finished and not Phase.None) return;

                  index++;
                  if (index >= total)
                  {
                        current = null;
                        Phase = Phase.Finished;

                        if (isAutoKill) Kill();
                        return;
                  }
                  current = all[index];
                  current.Reset();
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
                  for (int i = 0, count = all.Count; i < count; i++) all[i].Kill();

                  current = null;
                  index = 0;

                  Phase = Phase.None;
            }
            public void Reset()
            {
                  for (int i = 0, count = all.Count; i < count; i++) all[i].Reset();

                  index = 0;
                  total = all.Count;
                  (current = all[index]).Reset();

                  Phase = Phase.Active;
            }
      }
}