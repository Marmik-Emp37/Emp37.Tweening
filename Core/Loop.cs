using System;

using UnityEngine;

namespace Emp37.Tweening
{
      /// <summary>
      /// Loop configuration applied to a tween.
      /// <para>
      /// <b>Cycles exclude the initial play.</b> (e.g. <c>Restart(2)</c> = play once + 2 repeats)
      /// <br>Use <c>-1</c> cycles for infinite looping.</br>
      /// <br><c>Delay</c> (if &gt; 0) is a pause inserted <i>between</i> loop iterations.</br>
      /// </para>
      /// </summary>
      [Serializable]
      public readonly struct Loop : IEquatable<Loop>
      {
            public enum Type { None, Restart, Yoyo, }
            public enum Preset { Default, Return, InfiniteYoyo, InfiniteRestart }

            public readonly Type Mode;
            public readonly int Cycles;
            public readonly float Delay;

            /// <summary>
            /// Create a loop configuration.
            /// </summary>
            /// <param name="type">The loop behavior type.</param>
            /// <param name="cycles">The number of times the tween should loop <b>excluding the initial play</b>. Set to -1 for infinite loops.</param>
            /// <param name="interval">Optional delay in seconds between loop cycles.</param>
            public Loop(Type type, int cycles, float interval = 0F) { Mode = type; Cycles = Mathf.Max(cycles, -1); Delay = interval; }
            public Loop(Preset preset, float interval = 0F)
            {
                  switch (preset)
                  {
                        case Preset.Return: Mode = Type.Yoyo; Cycles = 1; break;
                        case Preset.InfiniteYoyo: Mode = Type.Yoyo; Cycles = -1; break;
                        case Preset.InfiniteRestart: Mode = Type.Restart; Cycles = -1; break;
                        default: Mode = Type.None; Cycles = 0; break;
                  }
                  Delay = interval;
            }


            public bool Equals(Loop other) => Mode == other.Mode && Cycles == other.Cycles && Delay.Equals(other.Delay);
            public override bool Equals(object obj) => obj is Loop other && Equals(other);
            public override int GetHashCode() => HashCode.Combine((int) Mode, Cycles, Delay);
      }
}