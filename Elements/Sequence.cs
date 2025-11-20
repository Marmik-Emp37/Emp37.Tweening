using System.Collections.Generic;

namespace Emp37.Tweening
{
      public sealed class Sequence : ITween
      {
            private readonly Queue<ITween> tweens;
            private int total;
            private ITween current;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => current == null && tweens.Count == 0;
            public Info Info
            {
                  get
                  {
                        int pending = tweens.Count, completed = total - pending - 1;
                        float currentRatio = current != null ? current.Info.Ratio : 1F, sequenceRatio = (completed + currentRatio) / total;

                        return new Info(nameof(Sequence), sequenceRatio, new("Current", current == null ? "null" : current.Info.Title), new("Pending", pending));
                  }
            }


            internal Sequence() => tweens = new();
            internal Sequence(IEnumerable<ITween> tweens) : this() => Append(tweens);

            public Sequence Append(ITween tween)
            {
                  if (tween == null || tween.IsEmpty) return this;

                  if (current == null) current = tween;
                  else tweens.Enqueue(tween);

                  return this;
            }
            public Sequence Append(IEnumerable<ITween> tweens)
            {
                  if (tweens == null) return this;
                  foreach (ITween item in tweens) Append(item);
                  return this;
            }
            public Sequence Append(params ITween[] tweens) => Append((IEnumerable<ITween>) tweens);

            void ITween.Init()
            {
                  current.Init();
                  total = tweens.Count + 1;
                  Phase = Phase.Active;
            }
            void ITween.Update()
            {
                  if (current.Phase is Phase.Active) current.Update();
                  if (current.Phase is not Phase.Complete and not Phase.None) return;

                  if (tweens.Count == 0)
                  {
                        current = null;
                        Phase = Phase.Complete;
                  }
                  else
                  {
                        current = tweens.Dequeue();
                        current.Init();
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
                  current?.Kill();
                  current = null;
                  tweens.Clear();
                  Phase = Phase.None;
            }
      }
}