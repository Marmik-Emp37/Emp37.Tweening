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
            public static void Play(this IElement element) => Factory.Play(element);
            public static void PlayWithTag(this IElement element, string tag) { element.Tag = tag; Factory.Play(element); }
            public static Sequence Then(this IElement current, IElement next) => new(current, next);

            // E L E M E N T   M E T H O D S
            internal static void Warn(string message) => Debug.LogWarning($"Tween creation failed: {message}");

            private static bool ValidateArguments(UObject link, object init, object target, float duration, object update, object evaluator)
            {
                  bool isValid = true;
                  if (link == null) { Warn($"Missing required object '{nameof(link)}' that ties the tween’s lifecycle to a Unity Object."); isValid = false; }
                  if (init == null) { Warn($"Missing required delegate '{nameof(init)}' that provides a starting value for the tween."); isValid = false; }
                  if (target == null) { Warn($"Missing required delegate '{nameof(target)}' that provides the target value for the tween."); isValid = false; }
                  if (duration <= 0F) { Warn($"Invalid duration '{duration}', must be greater than 0s to perform a tween."); isValid = false; }
                  if (update == null) { Warn($"Missing required delegate '{nameof(update)}' that applies interpolated values to the target each frame."); isValid = false; }
                  if (evaluator == null) { Warn($"Missing required delegate '{nameof(evaluator)}' that defines how values are interpolated between start and end."); isValid = false; }
                  return isValid;
            }
            public static Value<T> Value<T>(UObject link, Func<T> init, T target, float duration, Action<T> update, Value<T>.Evaluator evaluator) where T : struct => ValidateArguments(link, init, target, duration, update, evaluator) ? new Value<T>(link, init, target, duration, update, evaluator) : Element.Value<T>.Empty;
            public static Value<T> Value<T>(UObject link, Func<T> init, Func<T> dynamicTarget, float duration, Action<T> update, Value<T>.Evaluator evaluator) where T : struct => ValidateArguments(link, init, dynamicTarget, duration, update, evaluator) ? new Value<T>(link, init, dynamicTarget, duration, update, evaluator) : Element.Value<T>.Empty;
            public static Value<float> Value(UObject link, Func<float> init, float target, float duration, Action<float> update) => Value(link, init, target, duration, update, Mathf.LerpUnclamped);
            public static Value<float> Value(UObject link, Func<float> init, Func<float> dynamicTarget, float duration, Action<float> update) => Value(link, init, dynamicTarget, duration, update, Mathf.LerpUnclamped);
            public static Value<Vector2> Value(UObject link, Func<Vector2> init, Vector2 target, float duration, Action<Vector2> update) => Value(link, init, target, duration, update, Vector2.LerpUnclamped);
            public static Value<Vector2> Value(UObject link, Func<Vector2> init, Func<Vector2> dynamicTarget, float duration, Action<Vector2> update) => Value(link, init, dynamicTarget, duration, update, Vector2.LerpUnclamped);
            public static Value<Vector3> Value(UObject link, Func<Vector3> init, Vector3 target, float duration, Action<Vector3> update) => Value(link, init, target, duration, update, Vector3.LerpUnclamped);
            public static Value<Vector3> Value(UObject link, Func<Vector3> init, Func<Vector3> dynamicTarget, float duration, Action<Vector3> update) => Value(link, init, dynamicTarget, duration, update, Vector3.LerpUnclamped);
            public static Value<Vector4> Value(UObject link, Func<Vector4> init, Vector4 target, float duration, Action<Vector4> update) => Value(link, init, target, duration, update, Vector4.LerpUnclamped);
            public static Value<Vector4> Value(UObject link, Func<Vector4> init, Func<Vector4> dynamicTarget, float duration, Action<Vector4> update) => Value(link, init, dynamicTarget, duration, update, Vector4.LerpUnclamped);
            public static Value<Quaternion> Value(UObject link, Func<Quaternion> init, Quaternion target, float duration, Action<Quaternion> update) => Value(link, init, target, duration, update, Quaternion.LerpUnclamped);
            public static Value<Quaternion> Value(UObject link, Func<Quaternion> init, Func<Quaternion> dynamicTarget, float duration, Action<Quaternion> update) => Value(link, init, dynamicTarget, duration, update, Quaternion.LerpUnclamped);
            public static Value<Color> Value(UObject link, Func<Color> init, Color target, float duration, Action<Color> update) => Value(link, init, target, duration, update, Color.LerpUnclamped);
            public static Value<Color> Value(UObject link, Func<Color> init, Func<Color> dynamicTarget, float duration, Action<Color> update) => Value(link, init, dynamicTarget, duration, update, Color.LerpUnclamped);

            public static Parallel Parallel(params IElement[] elements) => new(elements);
            public static Parallel Parallel(IEnumerable<IElement> elements) => new(elements);
            public static Delay Delay(Func<bool> waitUntil) => new(waitUntil);
            public static Delay Delay(float value, Delta mode = Delta.Scaled, Func<bool> waitUntil = null) => new(value, mode, waitUntil);
            public static Sequence Sequence(params IElement[] elements) => new(elements);
            public static Sequence Sequence(IEnumerable<IElement> elements) => new(elements);
            public static Invoke Invoke(Action action) => new(action);
      }
}