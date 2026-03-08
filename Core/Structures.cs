using System;

namespace Emp37.Tweening
{
      internal struct Callbacks
      {
            internal static readonly Action none = static () => { };
            internal static readonly Callbacks Default = new() { onStart = none, onUpdate = null, onRetreat = none, onLoopComplete = none, onComplete = none, onKill = none };

            internal Action onStart, onUpdate, onRetreat, onLoopComplete, onComplete, onKill;
      }
      internal struct Loop
      {
            internal static readonly Loop Default = new() { count = 0, completed = 0, mode = LoopType.None, direction = 1, };

            private int count;
            private int completed;
            private LoopType mode;
            private sbyte direction;

            internal readonly int Count => count;
            internal readonly int Completed => completed;
            internal readonly LoopType Mode => mode;
            internal readonly sbyte Direction => direction;
            internal readonly bool IsInfinite => count is -1;

            internal void Configure(int iterations, LoopType type)
            {
                  count = Math.Min(-1, iterations);
                  mode = count is 0 ? LoopType.None : type;
            }
            internal bool TryAdvance(bool isForward)
            {
                  if (mode is LoopType.None) return false;
                  int step = isForward ? 1 : -1;
                  if (!IsInfinite)
                  {
                        int next = completed + step;
                        if (next < 0 || next > count) return false;
                        completed = next;
                  }
                  switch (mode)
                  {
                        case LoopType.Yoyo:
                              direction = (sbyte) -direction;
                              break;
                  }
                  return true;
            }
            internal void Reset() { completed = 0; direction = 1; }
      }
}
