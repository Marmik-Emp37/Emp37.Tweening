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


            // T W E E N   C R E A T O R S
            public static Value<T> Value<T>(UObject link, Func<T> initialization, T target, float duration, Action<T> update, Value<T>.Evaluator evaluator) where T : struct => Tweening.Value<T>.Fetch(link, initialization, () => target, duration, update, evaluator);
            public static Value<T> Value<T>(UObject link, Func<T> initialization, Func<T> dynamicTarget, float duration, Action<T> update, Value<T>.Evaluator evaluator) where T : struct => Tweening.Value<T>.Fetch(link, initialization, dynamicTarget, duration, update, evaluator);
            public static Parallel Parallel(params ITween[] tweens) => new(tweens);
            public static Parallel Parallel(IEnumerable<ITween> tweens) => new(tweens);
            public static Delay Delay(float duration, Delta mode = Delta.Scaled) => new(duration, mode);
            public static Delay Delay(Func<bool> until) => new(until);
            public static Delay Delay(float duration, Func<bool> until, Delta mode = Delta.Scaled) => new(duration, until, mode);
            public static Sequence Sequence(params ITween[] tweens) => new(tweens);
            public static Sequence Sequence(IEnumerable<ITween> tweens) => new(tweens);
            public static Callback Callback(Action action) => new(action);


            // V A L U E   H E L P E R S
            private static readonly Value<float>.Evaluator lerpF = Mathf.LerpUnclamped;
            public static Value<float> Value(UObject link, Func<float> initialization, float target, float duration, Action<float> update) => Value(link, initialization, target, duration, update, lerpF);
            public static Value<float> Value(UObject link, Func<float> initialization, Func<float> dynamicTarget, float duration, Action<float> update) => Value(link, initialization, dynamicTarget, duration, update, lerpF);


            private static readonly Value<Vector2>.Evaluator lerpV2 = Vector2.LerpUnclamped;
            public static Value<Vector2> Value(UObject link, Func<Vector2> initialization, Vector2 target, float duration, Action<Vector2> update) => Value(link, initialization, target, duration, update, lerpV2);
            public static Value<Vector2> Value(UObject link, Func<Vector2> initialization, Func<Vector2> dynamicTarget, float duration, Action<Vector2> update) => Value(link, initialization, dynamicTarget, duration, update, lerpV2);


            private static readonly Value<Vector3>.Evaluator lerpV3 = Vector3.LerpUnclamped;
            public static Value<Vector3> Value(UObject link, Func<Vector3> initialization, Vector3 target, float duration, Action<Vector3> update) => Value(link, initialization, target, duration, update, lerpV3);
            public static Value<Vector3> Value(UObject link, Func<Vector3> initialization, Func<Vector3> dynamicTarget, float duration, Action<Vector3> update) => Value(link, initialization, dynamicTarget, duration, update, lerpV3);


            private static readonly Value<Vector4>.Evaluator lerpV4 = Vector4.LerpUnclamped;
            public static Value<Vector4> Value(UObject link, Func<Vector4> initialization, Vector4 target, float duration, Action<Vector4> update) => Value(link, initialization, target, duration, update, lerpV4);
            public static Value<Vector4> Value(UObject link, Func<Vector4> initialization, Func<Vector4> dynamicTarget, float duration, Action<Vector4> update) => Value(link, initialization, dynamicTarget, duration, update, lerpV4);


            private static readonly Value<Quaternion>.Evaluator lerpQ = Quaternion.LerpUnclamped;
            public static Value<Quaternion> Value(UObject link, Func<Quaternion> initialization, Quaternion target, float duration, Action<Quaternion> update) => Value(link, initialization, target, duration, update, lerpQ);
            public static Value<Quaternion> Value(UObject link, Func<Quaternion> initialization, Func<Quaternion> dynamicTarget, float duration, Action<Quaternion> update) => Value(link, initialization, dynamicTarget, duration, update, lerpQ);


            private static readonly Value<Color>.Evaluator lerpColor = Color.LerpUnclamped;
            public static Value<Color> Value(UObject link, Func<Color> initialization, Color target, float duration, Action<Color> update) => Value(link, initialization, target, duration, update, lerpColor);
            public static Value<Color> Value(UObject link, Func<Color> initialization, Func<Color> dynamicTarget, float duration, Action<Color> update) => Value(link, initialization, dynamicTarget, duration, update, lerpColor);
      }
}