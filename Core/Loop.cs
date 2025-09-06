using System;

namespace Emp37.Tweening
{
      [Serializable]
      public struct Loop
      {
            public enum Type
            {
                  None,
                  Repeat,
                  Yoyo,
            }

            public readonly Type Mode;
            public int Count;
            public readonly float Delay;

            public static readonly Loop Default = new(Type.None, 0);

            /// <summary>
            /// Creates a new loop configuration for a tween.
            /// </summary>
            /// <param name="type">The loop behavior type.</param>
            /// <param name="count">The number of times the tween should loop <b>excluding the initial play</b>. Set to -1 for infinite loops.</param>
            /// <param name="interval">Optional delay in seconds between loop cycles.</param>
            /// </param>
            public Loop(Type type, int count, float interval = 0F) { Mode = type; Count = count; Delay = interval; }
      }
}