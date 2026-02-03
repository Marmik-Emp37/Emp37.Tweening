using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public static class Tweens
      {
            // V A L U E   H E L P E R S
            public static Value<T> Value<T>(Func<T> a, Func<T> b, Action<T> update, float duration, Value<T>.Lerp lerp) where T : struct
            {
                  var tween = Tweening.Value<T>.Fetch(a, b, update, duration, lerp);
                  Factory.Register(tween);
                  return tween;
            }

            #region B U I L T - I N   I N T E R P O L A T O R S
            private static readonly Value<float>.Lerp lerpV1C = Mathf.Lerp, lerpV1 = Mathf.LerpUnclamped;
            private static readonly Value<Vector2>.Lerp lerpV2 = Vector2.LerpUnclamped;
            private static readonly Value<Vector3>.Lerp lerpV3 = Vector3.LerpUnclamped;
            private static readonly Value<Vector4>.Lerp lerpV4 = Vector4.LerpUnclamped;
            private static readonly Value<Quaternion>.Lerp lerpQ = Quaternion.LerpUnclamped;
            private static readonly Value<Color>.Lerp lerpC = Color.LerpUnclamped;
            #endregion

            public static Value<float> ValueClamped(Func<float> a, float b, Action<float> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV1C);
            public static Value<float> Value(Func<float> a, float b, Action<float> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV1);
            public static Value<float> Value(Func<float> a, Func<float> b, Action<float> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV1);
            public static Value<Vector2> Value(Func<Vector2> a, Vector2 b, Action<Vector2> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV2);
            public static Value<Vector2> Value(Func<Vector2> a, Func<Vector2> b, Action<Vector2> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV2);
            public static Value<Vector3> Value(Func<Vector3> a, Vector3 b, Action<Vector3> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV3);
            public static Value<Vector3> Value(Func<Vector3> a, Func<Vector3> b, Action<Vector3> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV3);
            public static Value<Vector4> Value(Func<Vector4> a, Vector4 b, Action<Vector4> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV4);
            public static Value<Vector4> Value(Func<Vector4> a, Func<Vector4> b, Action<Vector4> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV4);
            public static Value<Quaternion> Value(Func<Quaternion> a, Quaternion b, Action<Quaternion> update, float duration) => Value(a, () => b, update, duration, lerpQ);
            public static Value<Quaternion> Value(Func<Quaternion> a, Func<Quaternion> b, Action<Quaternion> update, float duration) => Value(a, b, update, duration, lerpQ);
            public static Value<Color> Value(Func<Color> a, Color b, Action<Color> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpC);
            public static Value<Color> Value(Func<Color> a, Func<Color> b, Action<Color> update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpC);
      }
}