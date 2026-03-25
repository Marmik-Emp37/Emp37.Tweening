using System;
using UnityEngine;

namespace Emp37.Tweening
{
	using static Tweens;

	public static class TransformExtensions
	{
		// P O S I T I O N
		public static Value<Vector3> TweenMove(this Transform transform, Transform target, float duration, bool relative = false) => Value(() => transform.position, () => target.position, value => transform.position = value, duration, relative);
		public static Value<Vector3> TweenMove(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(() => transform.position, target, value => transform.position = value, duration, relative);
		public static Value<Vector2> TweenMove(this Transform transform, Vector2 target, float duration, bool relative = false) => Value(() => (Vector2) transform.position, target, value => transform.position = value, duration, relative);
		public static Value<float> TweenMoveX(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.position.x, target, value => { var pos = transform.position; pos.x = value; transform.position = pos; }, duration, relative);
		public static Value<float> TweenMoveY(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.position.y, target, value => { var pos = transform.position; pos.y = value; transform.position = pos; }, duration, relative);
		public static Value<float> TweenMoveZ(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.position.z, target, value => { var pos = transform.position; pos.z = value; transform.position = pos; }, duration, relative);


		// L O C A L   P O S I T I O N
		public static Value<Vector3> TweenMoveLocal(this Transform transform, Transform target, float duration, bool relative = false) => Value(() => transform.localPosition, () => target.localPosition, value => transform.localPosition = value, duration, relative);
		public static Value<Vector3> TweenMoveLocal(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(() => transform.localPosition, target, value => transform.localPosition = value, duration, relative);
		public static Value<Vector2> TweenMoveLocal(this Transform transform, Vector2 target, float duration, bool relative = false) => Value(() => (Vector2) transform.localPosition, target, value => transform.localPosition = value, duration, relative);
		public static Value<float> TweenMoveLocalX(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.localPosition.x, target, value => { var pos = transform.localPosition; pos.x = value; transform.localPosition = pos; }, duration, relative);
		public static Value<float> TweenMoveLocalY(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.localPosition.y, target, value => { var pos = transform.localPosition; pos.y = value; transform.localPosition = pos; }, duration, relative);
		public static Value<float> TweenMoveLocalZ(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.localPosition.z, target, value => { var pos = transform.localPosition; pos.z = value; transform.localPosition = pos; }, duration, relative);


		// R O T A T I O N
		public static Value<Quaternion> TweenRotate(this Transform transform, Transform target, float duration) => Value(() => transform.rotation, () => target.rotation, value => transform.rotation = value, duration);
		public static Value<Quaternion> TweenRotate(this Transform transform, Quaternion target, float duration) => Value(() => transform.rotation, target, value => transform.rotation = value, duration);
		public static Value<Quaternion> TweenRotate(this Transform transform, Vector3 target, float duration) => Value(() => transform.rotation, Quaternion.Euler(target), value => transform.rotation = value, duration);
		public static Value<float> TweenRotateX(this Transform transform, float target, float duration, bool relative = false)
		{
			Value<float>.Getter dst = relative ? () => transform.eulerAngles.x + target : () => transform.eulerAngles.x + Mathf.DeltaAngle(transform.eulerAngles.x, target);
			return Value(() => transform.eulerAngles.x, dst, value => { var euler = transform.eulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); }, duration, false);
		}
		public static Value<float> TweenRotateY(this Transform transform, float target, float duration, bool relative = false)
		{
			Value<float>.Getter dst = relative ? () => transform.eulerAngles.y + target : () => transform.eulerAngles.y + Mathf.DeltaAngle(transform.eulerAngles.y, target);
			return Value(() => transform.eulerAngles.y, dst, value => { var euler = transform.eulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); }, duration, false);
		}
		public static Value<float> TweenRotateZ(this Transform transform, float target, float duration, bool relative = false)
		{
			Value<float>.Getter dst = relative ? () => transform.eulerAngles.z + target : () => transform.eulerAngles.z + Mathf.DeltaAngle(transform.eulerAngles.z, target);
			return Value(() => transform.eulerAngles.z, dst, value => { var euler = transform.eulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); }, duration, false);
		}


		// L O C A L   R O T A T I O N
		public static Value<Quaternion> TweenRotateLocal(this Transform transform, Transform target, float duration) => Value(() => transform.localRotation, () => target.localRotation, value => transform.localRotation = value, duration);
		public static Value<Quaternion> TweenRotateLocal(this Transform transform, Quaternion target, float duration) => Value(() => transform.localRotation, target, value => transform.localRotation = value, duration);
		public static Value<Quaternion> TweenRotateLocal(this Transform transform, Vector3 target, float duration) => Value(() => transform.localRotation, Quaternion.Euler(target), value => transform.localRotation = value, duration);
		public static Value<float> TweenRotateLocalX(this Transform transform, float target, float duration, bool relative = false)
		{
			Value<float>.Getter dst = relative ? () => transform.localEulerAngles.x + target : () => transform.localEulerAngles.x + Mathf.DeltaAngle(transform.localEulerAngles.x, target);
			return Value(() => transform.localEulerAngles.x, dst, value => { var euler = transform.localEulerAngles; euler.x = value; transform.localRotation = Quaternion.Euler(euler); }, duration, false);
		}
		public static Value<float> TweenRotateLocalY(this Transform transform, float target, float duration, bool relative = false)
		{
			Value<float>.Getter dst = relative ? () => transform.localEulerAngles.y + target : () => transform.localEulerAngles.y + Mathf.DeltaAngle(transform.localEulerAngles.y, target);
			return Value(() => transform.localEulerAngles.y, dst, value => { var euler = transform.localEulerAngles; euler.y = value; transform.localRotation = Quaternion.Euler(euler); }, duration, false);
		}
		public static Value<float> TweenRotateLocalZ(this Transform transform, float target, float duration, bool relative = false)
		{
			Value<float>.Getter dst = relative ? () => transform.localEulerAngles.z + target : () => transform.localEulerAngles.z + Mathf.DeltaAngle(transform.localEulerAngles.z, target);
			return Value(() => transform.localEulerAngles.z, dst, value => { var euler = transform.localEulerAngles; euler.z = value; transform.localRotation = Quaternion.Euler(euler); }, duration, false);
		}


		// S C A L E
		public static Value<Vector3> TweenScale(this Transform transform, Transform target, float duration, bool relative = false) => Value(() => transform.localScale, () => target.localScale, value => transform.localScale = value, duration, relative);
		public static Value<Vector3> TweenScale(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(() => transform.localScale, target, value => transform.localScale = value, duration, relative);
		public static Value<float> TweenScaleX(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.localScale.x, target, value => { var scale = transform.localScale; scale.x = value; transform.localScale = scale; }, duration, relative);
		public static Value<float> TweenScaleY(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.localScale.y, target, value => { var scale = transform.localScale; scale.y = value; transform.localScale = scale; }, duration, relative);
		public static Value<float> TweenScaleZ(this Transform transform, float target, float duration, bool relative = false) => Value(() => transform.localScale.z, target, value => { var scale = transform.localScale; scale.z = value; transform.localScale = scale; }, duration, relative);
	}
}