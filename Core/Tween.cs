using System;
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
            public static void Play(this IElement element, string tag = null)
            {
                  element.Tag = tag;
                  Factory.Play(element);
            }
            public static Sequence Then(this IElement current, IElement next) => new(current, next);

            // E L E M E N T   M E T H O D S
            private static bool ValidateArgs(object init, object target, float duration, object update, object evaluator)
            {
                  bool isValid = true;
                  if (init == null) { warn($"Missing required delegate: {nameof(init)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (target == null) { warn($"Missing required delegate: {nameof(target)}. This delegate provides the target value for the tween and must not be null."); isValid = false; }
                  if (duration <= 0F) { warn($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (update == null) { warn($"Missing required delegate: {nameof(update)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }
                  if (evaluator == null) { warn($"Missing required delegate: {nameof(evaluator)}. This delegate defines how the tween interpolates between two values and must not be null."); isValid = false; }
                  return isValid;
                  static void warn(string message) => Debug.LogWarning($"Tween creation failed: {message}");
            }
            public static Value<T> Value<T>(Func<T> init, T target, float duration, Action<T> update, Value<T>.Evaluator evaluator, UObject link = null) where T : struct => ValidateArgs(init, target, duration, update, evaluator) ? new Value<T>(init, target, duration, update, evaluator, link) : Element.Value<T>.Empty;
            public static Value<T> Value<T>(Func<T> init, Func<T> dynamicTarget, float duration, Action<T> update, Value<T>.Evaluator evaluator, UObject link = null) where T : struct => ValidateArgs(init, dynamicTarget, duration, update, evaluator) ? new Value<T>(init, dynamicTarget, duration, update, evaluator, link) : Element.Value<T>.Empty;

            public static Value<float> Value(Func<float> init, float target, float duration, Action<float> update, UObject link = null) => Value(init, target, duration, update, Mathf.LerpUnclamped, link);
            public static Value<float> Value(Func<float> init, Func<float> dynamicTarget, float duration, Action<float> update, UObject link = null) => Value(init, dynamicTarget, duration, update, Mathf.LerpUnclamped, link);

            public static Value<Vector2> Value(Func<Vector2> init, Vector2 target, float duration, Action<Vector2> update, UObject link = null) => Value(init, target, duration, update, Vector2.LerpUnclamped, link);
            public static Value<Vector2> Value(Func<Vector2> init, Func<Vector2> dynamicTarget, float duration, Action<Vector2> update, UObject link = null) => Value(init, dynamicTarget, duration, update, Vector2.LerpUnclamped, link);

            public static Value<Vector3> Value(Func<Vector3> init, Vector3 target, float duration, Action<Vector3> update, UObject link = null) => Value(init, target, duration, update, Vector3.LerpUnclamped, link);
            public static Value<Vector3> Value(Func<Vector3> init, Func<Vector3> dynamicTarget, float duration, Action<Vector3> update, UObject link = null) => Value(init, dynamicTarget, duration, update, Vector3.LerpUnclamped, link);

            public static Value<Vector4> Value(Func<Vector4> init, Vector4 target, float duration, Action<Vector4> update, UObject link = null) => Value(init, target, duration, update, Vector4.LerpUnclamped, link);
            public static Value<Vector4> Value(Func<Vector4> init, Func<Vector4> dynamicTarget, float duration, Action<Vector4> update, UObject link = null) => Value(init, dynamicTarget, duration, update, Vector4.LerpUnclamped, link);

            public static Value<Quaternion> Value(Func<Quaternion> init, Quaternion target, float duration, Action<Quaternion> update, UObject link = null) => Value(init, target, duration, update, Quaternion.LerpUnclamped, link);
            public static Value<Quaternion> Value(Func<Quaternion> init, Func<Quaternion> dynamicTarget, float duration, Action<Quaternion> update, UObject link = null) => Value(init, dynamicTarget, duration, update, Quaternion.LerpUnclamped, link);

            public static Value<Color> Value(Func<Color> init, Color target, float duration, Action<Color> update, UObject link = null) => Value(init, target, duration, update, Color.LerpUnclamped, link);
            public static Value<Color> Value(Func<Color> init, Func<Color> dynamicTarget, float duration, Action<Color> update, UObject link = null) => Value(init, dynamicTarget, duration, update, Color.LerpUnclamped, link);

            public static Parallel Parallel(params IElement[] elements) => new(elements);
            public static Parallel Parallel(IEnumerable<IElement> elements) => new(elements);
            public static Delay Delay(Func<bool> waitUntil) => new(waitUntil);
            public static Delay Delay(float value, Delta mode = Delta.Scaled, Func<bool> waitUntil = null) => new(value, mode, waitUntil);
            public static Sequence Sequence(params IElement[] elements) => new(elements);
            public static Sequence Sequence(IEnumerable<IElement> elements) => new(elements);
            public static Invoke Invoke(Action action) => new(action);
      }
}