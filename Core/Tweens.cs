using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public static class Tweens
      {
            public static Value<T> Value<T>(Func<T> source, Func<T> destination, float duration, Action<T> update, Value<T>.Lerp lerpFunction) where T : struct
            {
                  var tween = Tweening.Value<T>.Fetch(source, destination, duration, update, lerpFunction);
                  Factory.Register(tween);
                  return tween;
            }

            // V A L U E   H E L P E R S
            public static Value<float> ValueClamped(Func<float> source, Func<float> destination, float duration, Action<float> update) => Value(source, destination, duration, update, lerpV1C);
            public static Value<float> Value(Func<float> source, Func<float> destination, float duration, Action<float> update) => Value(source, destination, duration, update, lerpV1);
            public static Value<Vector2> Value(Func<Vector2> source, Func<Vector2> destination, float duration, Action<Vector2> update) => Value(source, destination, duration, update, lerpV2);
            public static Value<Vector3> Value(Func<Vector3> source, Func<Vector3> destination, float duration, Action<Vector3> update) => Value(source, destination, duration, update, lerpV3);
            public static Value<Vector4> Value(Func<Vector4> source, Func<Vector4> destination, float duration, Action<Vector4> update) => Value(source, destination, duration, update, lerpV4);
            public static Value<Quaternion> Value(Func<Quaternion> source, Func<Quaternion> destination, float duration, Action<Quaternion> update) => Value(source, destination, duration, update, lerpQ);
            public static Value<Color> Value(Func<Color> source, Func<Color> destination, float duration, Action<Color> update) => Value(source, destination, duration, update, lerpC);

            #region B U I L T - I N   I N T E R P O L A T O R S
            private static readonly Value<float>.Lerp lerpV1C = Mathf.Lerp, lerpV1 = Mathf.LerpUnclamped;
            private static readonly Value<Vector2>.Lerp lerpV2 = Vector2.LerpUnclamped;
            private static readonly Value<Vector3>.Lerp lerpV3 = Vector3.LerpUnclamped;
            private static readonly Value<Vector4>.Lerp lerpV4 = Vector4.LerpUnclamped;
            private static readonly Value<Quaternion>.Lerp lerpQ = Quaternion.LerpUnclamped;
            private static readonly Value<Color>.Lerp lerpC = Color.LerpUnclamped;
            #endregion
      }
}