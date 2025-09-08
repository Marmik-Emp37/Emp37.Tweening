﻿using System;
using System.Collections.Generic;

using UnityEngine;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      using Element;

      /// <summary>
      /// Main entry point for creating and controlling tweens.
      /// </summary>
      public static class Tween
      {
            // E X T E N S I O N S
            /// <summary>
            /// Immediately starts playing this tween element
            /// </summary>
            public static void Play(this IElement element) => Factory.Play(element);
            /// <summary>
            /// Creates a sequence by chaining the current element with another
            /// </summary>
            public static Sequence Then(this IElement current, IElement next) => new(current, next);

            // E L E M E N T   M E T H O D S
            /// <summary>
            /// Creates a new tween for a value type.
            /// </summary>
            /// <typeparam name="T">The value type being interpolated.</typeparam>
            /// <param name="init">A delegate that retrieves the starting value for the tween at the moment it begins execution. This is called only once unless the loop configuration is set to recapture it.</param>
            /// <param name="target">The final value to interpolate toward.</param>
            /// <param name="duration">The time in seconds over which the tween runs. Must be greater than zero.</param>
            /// <param name="update">A delegate invoked each frame with the interpolated value.</param>
            /// <param name="evaluator">A function used to evaluate the interpolation between the start and target values.</param>
            /// <returns>A configured <see cref="Element.Value{T}"/> instance, or an empty tween if parameters are invalid.</returns>
            public static Value<T> Value<T>(Func<T> init, T target, float duration, Action<T> update, Value<T>.Evaluator evaluator, UObject link = null) where T : struct
            {
                  bool isValid = true;
                  if (duration <= 0F) { warn($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (init == null) { warn($"Missing required delegate: {nameof(init)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (update == null) { warn($"Missing required delegate: {nameof(update)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }
                  if (evaluator == null) { warn($"Missing required delegate: {nameof(evaluator)}. This delegate defines how the tween interpolates between two values and must not be null."); isValid = false; }
                  return isValid ? new Value<T>(init, target, duration, update, evaluator, link) : Element.Value<T>.Empty;

                  static void warn(string message) => Debug.LogWarning($"{nameof(Element.Value<T>)} creation failed: {message}");
            }

            /// <summary>
            /// Creates a value tween using <see cref="Mathf.LerpUnclamped(float, float, float)"/>.
            /// </summary>
            public static Value<float> Value(Func<float> init, float target, float duration, Action<float> update, UObject link = null) => Value(init, target, duration, update, Mathf.LerpUnclamped, link);

            /// <summary>
            /// Creates a value tween using <see cref="Vector2.LerpUnclamped(Vector2, Vector2, float)"/>.
            /// </summary>
            public static Value<Vector2> Value(Func<Vector2> init, Vector2 target, float duration, Action<Vector2> update, UObject link = null) => Value(init, target, duration, update, Vector2.LerpUnclamped, link);

            /// <summary>
            /// Creates a value tween using <see cref="Vector3.LerpUnclamped(Vector3, Vector3, float)"/>.
            /// </summary>
            public static Value<Vector3> Value(Func<Vector3> init, Vector3 target, float duration, Action<Vector3> update, UObject link = null) => Value(init, target, duration, update, Vector3.LerpUnclamped, link);

            /// <summary>
            /// Creates a value tween using <see cref="Vector4.LerpUnclamped(Vector4, Vector4, float)"/>.
            /// </summary>
            public static Value<Vector4> Value(Func<Vector4> init, Vector4 target, float duration, Action<Vector4> update, UObject link = null) => Value(init, target, duration, update, Vector4.LerpUnclamped, link);

            /// <summary>
            /// Creates a value tween using <see cref="Quaternion.LerpUnclamped(Quaternion, Quaternion, float)"/>.
            /// </summary>
            public static Value<Quaternion> Value(Func<Quaternion> init, Quaternion target, float duration, Action<Quaternion> update, UObject link = null) => Value(init, target, duration, update, Quaternion.LerpUnclamped, link);

            /// <summary>
            /// Creates a value tween using <see cref="Color.LerpUnclamped(Color, Color, float)"/>.
            /// </summary>
            public static Value<Color> Value(Func<Color> init, Color target, float duration, Action<Color> update, UObject link = null) => Value(init, target, duration, update, Color.LerpUnclamped, link);

            public static Parallel Parallel(params IElement[] elements) => new(elements);
            public static Parallel Parallel(IEnumerable<IElement> elements) => new(elements);
            public static Delay Delay(Func<bool> waitUntil) => new(waitUntil);
            public static Delay Delay(float value, Delta mode = Delta.Scaled, Func<bool> waitUntil = null) => new(value, mode, waitUntil);
            public static Sequence Sequence(params IElement[] elements) => new(elements);
            public static Sequence Sequence(IEnumerable<IElement> elements) => new(elements);
            public static Invoke Invoke(Action action) => new(action);
      }
}