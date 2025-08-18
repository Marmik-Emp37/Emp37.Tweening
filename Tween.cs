using System;

using UnityEngine;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      using Element;

      public static class Tween
      {
            // E X T E N S I O N S
            public static void Play(this IElement element) => Factory.Play(element);
            public static Sequence Then(this IElement current, IElement next) => new(current, next);


            // E L E M E N T   M E T H O D S
            /// <summary>
            /// Creates a new tween for a value type.
            /// </summary>
            /// <typeparam name="T">The value type being interpolated.</typeparam>
            /// <param name="startValue">A delegate that retrieves the starting value for the tween at the moment it begins execution. This is called only once unless the loop configuration is set to recapture it.</param>
            /// <param name="target">The final value to interpolate toward.</param>
            /// <param name="duration">The time in seconds over which the tween runs. Must be greater than zero.</param>
            /// <param name="onValueChange">A delegate invoked each frame with the interpolated value.</param>
            /// <param name="evaluator">A function used to evaluate the interpolation between the start and target values.</param>
            /// <returns>A configured <see cref="Value{T}"/> instance, or an empty tween if parameters are invalid.</returns>
            public static Value<T> Create<T>(Func<T> startValue, T target, float duration, Action<T> onValueChange, Value<T>.Evaluator evaluator, UObject link = null) where T : struct
            {
                  bool isValid = true;
                  if (duration <= 0F) { warn($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (startValue == null) { warn($"Missing required delegate: {nameof(startValue)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (onValueChange == null) { warn($"Missing required delegate: {nameof(onValueChange)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }
                  if (evaluator == null) { warn($"Missing required delegate: {nameof(evaluator)}. This delegate defines how the tween interpolates between two values and must not be null."); isValid = false; }
                  return isValid ? new Value<T>(startValue, target, duration, onValueChange, evaluator, link) : Value<T>.Empty;

                  static void warn(string message) => Debug.LogWarning($"{nameof(Value<T>)} creation failed: {message}");
            }
            public static Value<float> Create(Func<float> startValue, float target, float duration, Action<float> onValueChange, UObject link = null) => Create(startValue, target, duration, onValueChange, Mathf.LerpUnclamped, link);
            public static Value<Vector2> Create(Func<Vector2> startValue, Vector2 target, float duration, Action<Vector2> onValueChange, UObject link = null) => Create(startValue, target, duration, onValueChange, Vector2.LerpUnclamped, link);
            public static Value<Vector3> Create(Func<Vector3> startValue, Vector3 target, float duration, Action<Vector3> onValueChange, UObject link = null) => Create(startValue, target, duration, onValueChange, Vector3.LerpUnclamped, link);
            public static Value<Quaternion> Create(Func<Quaternion> startValue, Quaternion target, float duration, Action<Quaternion> onValueChange, UObject link = null) => Create(startValue, target, duration, onValueChange, Quaternion.LerpUnclamped, link);
            public static Value<Color> Create(Func<Color> startValue, Color target, float duration, Action<Color> onValueChange, UObject link = null) => Create(startValue, target, duration, onValueChange, Color.LerpUnclamped, link);

            public static Parallel Parallel(params IElement[] elements) => new(elements);
            public static Delay Delay(Func<bool> waitUntil) => new(waitUntil);
            public static Delay Delay(float value, Delta mode = Delta.Scaled, Func<bool> waitUntil = null) => new(value, mode, waitUntil);
            public static Sequence Sequence(params IElement[] elements) => new(elements);
      }
}