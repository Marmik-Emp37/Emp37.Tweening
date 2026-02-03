//using System.Collections.Generic;

//namespace Emp37.Tweening
//{
//      public sealed class Sequence : Tween
//      {
//            private readonly List<Tween> steps = new(16);

//            private int index;
//            private bool bootstrapped;

//            public override bool IsEmpty => steps.Count == 0 || IsDestroyed;

//            public Sequence Add(Tween tween)
//            {
//                  if (tween != null && !tween.IsEmpty) steps.Add(tween);
//                  return this;
//            }

//            internal override void Init()
//            {
//                  initialLoopSettings = loopSettings;
//                  index = 0;
//                  direction = 1;
//                  bootstrapped = false;
//                  for (int i = 0; i < steps.Count; i++) steps[i].Init();
//                  Phase = State.Active;
//            }
//            internal override void Update()
//            {
//                  if (IsDestroyed)
//                  {
//                        Kill();
//                        return;
//                  }
//                  if (!bootstrapped)
//                  {
//                        bootstrapped = true;
//                        TryInvoke(actionOnStart);
//                  }
//                  if (steps.Count == 0)
//                  {
//                        Conclude();
//                        return;
//                  }
//                  // Bounds check (in case of yoyo direction flips)
//                  if (index < 0) index = 0;
//                  if (index >= steps.Count) index = steps.Count - 1;

//                  Tween current = steps[index];
//                  if (current.Phase == State.Paused)
//                  {
//                        return;
//                  }
//                  if (current.Phase == State.None)
//                  {
//                        MoveNext();
//                        return;
//                  }
//                  if (current.Phase == State.Finished)
//                  {
//                        MoveNext();
//                        return;
//                  }
//                  if (current.Phase != State.Active) current.Resume();

//                  current.Update();
//            }

//            private void MoveNext()
//            {
//                  index += direction;
//                  bool outOfRange = direction > 0 ? index >= steps.Count : index < 0;
//                  if (!outOfRange) return;

//                  if (loopSettings.Type != LoopType.None && loopSettings.Count != 0)
//                  {
//                        if (loopSettings.Count > 0) loopSettings.Count--;
//                        if (loopSettings.Type == LoopType.Yoyo) direction *= -1;
//                        RestartChildrenForNextLoop();
//                        index = direction > 0 ? 0 : steps.Count - 1;
//                        return;
//                  }
//                  Conclude();
//            }

//            private void RestartChildrenForNextLoop()
//            {
//                  for (int i = 0; i < steps.Count; i++) steps[i].Replay();
//            }
//            private void Conclude()
//            {
//                  Phase = State.Finished;
//                  TryInvoke(actionOnComplete);
//                  if ((settings & Settings.AutoKill) != 0) Kill();
//            }

//            internal override void Pause()
//            {
//                  if (Phase == State.Active)
//                  {
//                        Phase = State.Paused;
//                        for (int i = 0; i < steps.Count; i++) steps[i].Pause();
//                  }
//            }
//            internal override void Resume()
//            {
//                  if (Phase == State.Paused)
//                  {
//                        Phase = State.Active;
//                        if (steps.Count > 0 && index >= 0 && index < steps.Count) steps[index].Resume();
//                  }
//            }
//            public override void Replay()
//            {
//                  loopSettings = initialLoopSettings;
//                  direction = 1;
//                  index = 0;
//                  bootstrapped = false;
//                  Phase = State.Active;
//                  for (int i = 0; i < steps.Count; i++) steps[i].Replay();
//            }
//            public override void Kill()
//            {
//                  if (Phase == State.None) return;
//                  Phase = State.None;
//                  for (int i = 0; i < steps.Count; i++) steps[i].Kill();
//                  TryInvoke(actionOnKill);
//            }
//      }
//}
