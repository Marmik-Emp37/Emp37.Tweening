using System;
using System.Collections.Generic;

using UnityEngine;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      /// <summary>
      /// Main entry point for creating and controlling tweens.
      /// </summary>
      public static class Tween
      {
            // E X T E N S I O N S
            public static T Play<T>(this T tween) where T : ITween { Factory.Play(tween); return tween; }
            public static T WithTag<T>(this T tween, string tag) where T : ITween { tween.Tag = tag; return tween; }
            public static Sequence Then(this ITween current, ITween next) => Sequence(current, next);

            // E L E M E N T   M E T H O D S
            private static bool ValidateArguments(UObject link, object capture, object target, float duration, object update, object evaluator)
            {
                  bool ok = true;
                  if (link == null) { Log.RejectTween($"Missing {nameof(link)}."); ok = false; }
                  if (capture == null) { Log.RejectTween($"Missing {nameof(capture)} delegate."); ok = false; }
                  if (target == null) { Log.RejectTween($"Missing {nameof(target)} value or getter."); ok = false; }
                  if (duration <= 0F) { Log.RejectTween($"{nameof(duration)} must be > 0 (got {duration})."); ok = false; }
                  if (update == null) { Log.RejectTween($"Missing {nameof(update)} callback."); ok = false; }
                  if (evaluator == null) { Log.RejectTween($"Missing {nameof(evaluator)} function."); ok = false; }
                  return ok;
            }
            public static Value<T> Value<T>(UObject link, Func<T> capture, T target, float duration, Action<T> update, Value<T>.Evaluator evaluator) where T : struct => ValidateArguments(link, capture, target, duration, update, evaluator) ? new Value<T>(link, capture, target, duration, update, evaluator) : Tweening.Value<T>.Empty;
            public static Value<T> Value<T>(UObject link, Func<T> capture, Func<T> dynamicTarget, float duration, Action<T> update, Value<T>.Evaluator evaluator) where T : struct => ValidateArguments(link, capture, dynamicTarget, duration, update, evaluator) ? new Value<T>(link, capture, dynamicTarget, duration, update, evaluator) : Tweening.Value<T>.Empty;


            private static readonly Value<float>.Evaluator evaluatorFloat = Mathf.LerpUnclamped;
            public static Value<float> Value(UObject link, Func<float> capture, float target, float duration, Action<float> update) => Value(link, capture, target, duration, update, evaluatorFloat);
            public static Value<float> Value(UObject link, Func<float> capture, Func<float> dynamicTarget, float duration, Action<float> update) => Value(link, capture, dynamicTarget, duration, update, evaluatorFloat);


            private static readonly Value<Vector2>.Evaluator evaluatorV2 = Vector2.LerpUnclamped;
            public static Value<Vector2> Value(UObject link, Func<Vector2> capture, Vector2 target, float duration, Action<Vector2> update) => Value(link, capture, target, duration, update, evaluatorV2);
            public static Value<Vector2> Value(UObject link, Func<Vector2> capture, Func<Vector2> dynamicTarget, float duration, Action<Vector2> update) => Value(link, capture, dynamicTarget, duration, update, evaluatorV2);


            private static readonly Value<Vector3>.Evaluator evaluatorV3 = Vector3.LerpUnclamped;
            public static Value<Vector3> Value(UObject link, Func<Vector3> capture, Vector3 target, float duration, Action<Vector3> update) => Value(link, capture, target, duration, update, evaluatorV3);
            public static Value<Vector3> Value(UObject link, Func<Vector3> capture, Func<Vector3> dynamicTarget, float duration, Action<Vector3> update) => Value(link, capture, dynamicTarget, duration, update, evaluatorV3);


            private static readonly Value<Vector4>.Evaluator evaluatorV4 = Vector4.LerpUnclamped;
            public static Value<Vector4> Value(UObject link, Func<Vector4> capture, Vector4 target, float duration, Action<Vector4> update) => Value(link, capture, target, duration, update, evaluatorV4);
            public static Value<Vector4> Value(UObject link, Func<Vector4> capture, Func<Vector4> dynamicTarget, float duration, Action<Vector4> update) => Value(link, capture, dynamicTarget, duration, update, evaluatorV4);


            private static readonly Value<Quaternion>.Evaluator evaluatorQuaternion = Quaternion.LerpUnclamped;
            public static Value<Quaternion> Value(UObject link, Func<Quaternion> capture, Quaternion target, float duration, Action<Quaternion> update) => Value(link, capture, target, duration, update, evaluatorQuaternion);
            public static Value<Quaternion> Value(UObject link, Func<Quaternion> capture, Func<Quaternion> dynamicTarget, float duration, Action<Quaternion> update) => Value(link, capture, dynamicTarget, duration, update, evaluatorQuaternion);


            private static readonly Value<Color>.Evaluator evaluatorColor = Color.LerpUnclamped;
            public static Value<Color> Value(UObject link, Func<Color> capture, Color target, float duration, Action<Color> update) => Value(link, capture, target, duration, update, evaluatorColor);
            public static Value<Color> Value(UObject link, Func<Color> capture, Func<Color> dynamicTarget, float duration, Action<Color> update) => Value(link, capture, dynamicTarget, duration, update, evaluatorColor);

            public static Parallel Parallel(params ITween[] elements) => new(elements);
            public static Parallel Parallel(IEnumerable<ITween> elements) => new(elements);
            public static Delay Delay(float duration, Delta mode = Delta.Scaled) => new(duration, mode);
            public static Delay Delay(Func<bool> until) => new(until);
            public static Delay Delay(float duration, Func<bool> until, Delta mode = Delta.Scaled) => new(duration, until, mode);
            public static Sequence Sequence(params ITween[] elements) => new(elements);
            public static Sequence Sequence(IEnumerable<ITween> elements) => new(elements);
            public static Invoke Invoke(Action action) => new(action);
      }
}