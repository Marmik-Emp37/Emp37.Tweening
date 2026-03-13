using System;

using UnityEngine;

namespace Emp37.Tweening
{
	public static class Tweens
	{
		// V A L U E   H E L P E R S
		public static Value<T> Value<T>(Value<T>.Getter a, Value<T>.Getter b, Value<T>.Setter update, float duration, Value<T>.Interpolator lerp) where T : struct
		{
			var tween = Tweening.Value<T>.Fetch(a, b, update, duration, lerp);
			Factory.Register(tween);
			return tween;
		}

		#region B U I L T - I N   I N T E R P O L A T O R S
		private static readonly Value<float>.Interpolator lerpV1C = Mathf.Lerp, lerpV1 = Mathf.LerpUnclamped;
		private static readonly Value<Vector2>.Interpolator lerpV2 = Vector2.LerpUnclamped;
		private static readonly Value<Vector3>.Interpolator lerpV3 = Vector3.LerpUnclamped;
		private static readonly Value<Vector4>.Interpolator lerpV4 = Vector4.LerpUnclamped;
		private static readonly Value<Quaternion>.Interpolator lerpQ = Quaternion.LerpUnclamped;
		private static readonly Value<Color>.Interpolator lerpC = Color.LerpUnclamped;
		#endregion

		public static Value<float> ValueClamped(Value<float>.Getter a, float b, Value<float>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV1C);
		public static Value<float> Value(Value<float>.Getter a, float b, Value<float>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV1);
		public static Value<float> Value(Value<float>.Getter a, Value<float>.Getter b, Value<float>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV1);
		public static Value<Vector2> Value(Value<Vector2>.Getter a, Vector2 b, Value<Vector2>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV2);
		public static Value<Vector2> Value(Value<Vector2>.Getter a, Value<Vector2>.Getter b, Value<Vector2>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV2);
		public static Value<Vector3> Value(Value<Vector3>.Getter a, Vector3 b, Value<Vector3>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV3);
		public static Value<Vector3> Value(Value<Vector3>.Getter a, Value<Vector3>.Getter b, Value<Vector3>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV3);
		public static Value<Vector4> Value(Value<Vector4>.Getter a, Vector4 b, Value<Vector4>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpV4);
		public static Value<Vector4> Value(Value<Vector4>.Getter a, Value<Vector4>.Getter b, Value<Vector4>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpV4);
		public static Value<Quaternion> Value(Value<Quaternion>.Getter a, Quaternion b, Value<Quaternion>.Setter update, float duration) => Value(a, () => b, update, duration, lerpQ);
		public static Value<Quaternion> Value(Value<Quaternion>.Getter a, Value<Quaternion>.Getter b, Value<Quaternion>.Setter update, float duration) => Value(a, b, update, duration, lerpQ);
		public static Value<Color> Value(Value<Color>.Getter a, Color b, Value<Color>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b : () => b, update, duration, lerpC);
		public static Value<Color> Value(Value<Color>.Getter a, Value<Color>.Getter b, Value<Color>.Setter update, float duration, bool relative = false) => Value(a, relative ? () => a() + b() : b, update, duration, lerpC);
	}
}