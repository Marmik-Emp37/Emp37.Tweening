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

            private int count, completed;
            private LoopType mode;
            private sbyte direction;

            internal readonly int Completed => completed;
            internal readonly int Count => count;
            internal readonly LoopType Mode => mode;
            internal readonly sbyte Direction => direction;

            internal readonly bool IsInfinite => count is -1;
            internal void Configure(int iterations, LoopType type)
            {
                  count = iterations < 0 ? -1 : iterations;
                  mode = count is 0 ? LoopType.None : type;
            }
            private readonly bool Continue(sbyte playbackDirection)
            {
                  if (mode == LoopType.None) return false;
                  if (IsInfinite) return true;

                  return playbackDirection > 0 ? (completed < count) : (completed > 0);
            }
            internal bool TryAdvance(sbyte effectiveDirection)
            {
                  if (!Continue(effectiveDirection)) return false;
                  if (!IsInfinite) completed += effectiveDirection;
                  if (mode == LoopType.Yoyo) direction = (sbyte) -direction;
                  return true;
            }
            internal void Reset()
            {
                  completed = 0;
                  direction = 1;
            }
      }
}
