using UnityEngine;

namespace Emp37.Tweening
{
      using static Tween;

      public static class TransformExtensions
      {
            // P O S I T I O N
            public static Value<Vector3> TweenMove(this Transform transform, Transform target, float duration) => Value(transform, () => transform.position, () => target.position, duration, value => transform.position = value);
            public static Value<Vector3> TweenMove(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.position, () => relative ? transform.position + target : target, duration, value => transform.position = value);
            public static Value<Vector2> TweenMove(this Transform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => (Vector2) transform.position, () => relative ? (Vector2) transform.position + target : target, duration, value => transform.position = value);
            public static Value<float> TweenMoveX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.position.x, () => relative ? transform.position.x + target : target, duration, value => { var pos = transform.position; pos.x = value; transform.position = pos; });
            public static Value<float> TweenMoveY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.position.y, () => relative ? transform.position.y + target : target, duration, value => { var pos = transform.position; pos.y = value; transform.position = pos; });
            public static Value<float> TweenMoveZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.position.z, () => relative ? transform.position.z + target : target, duration, value => { var pos = transform.position; pos.z = value; transform.position = pos; });


            // L O C A L   P O S I T I O N
            public static Value<Vector3> TweenMoveLocal(this Transform transform, Transform target, float duration) => Value(transform, () => transform.localPosition, () => target.localPosition, duration, value => transform.localPosition = value);
            public static Value<Vector3> TweenMoveLocal(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.localPosition, () => relative ? transform.localPosition + target : target, duration, value => transform.localPosition = value);
            public static Value<Vector2> TweenMoveLocal(this Transform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => (Vector2) transform.localPosition, () => relative ? (Vector2) transform.localPosition + target : target, duration, value => transform.localPosition = value);
            public static Value<float> TweenMoveLocalX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localPosition.x, () => relative ? transform.localPosition.x + target : target, duration, value => { var pos = transform.localPosition; pos.x = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localPosition.y, () => relative ? transform.localPosition.y + target : target, duration, value => { var pos = transform.localPosition; pos.y = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localPosition.z, () => relative ? transform.localPosition.z + target : target, duration, value => { var pos = transform.localPosition; pos.z = value; transform.localPosition = pos; });


            // R O T A T I O N
            public static Value<Quaternion> TweenRotate(this Transform transform, Transform target, float duration) => Value(transform, () => transform.rotation, () => target.rotation, duration, value => transform.rotation = value);
            public static Value<Quaternion> TweenRotate(this Transform transform, Quaternion target, float duration) => Value(transform, () => transform.rotation, target, duration, value => transform.rotation = value);
            public static Value<Quaternion> TweenRotate(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.rotation, () => Quaternion.Euler(target), duration, value => transform.rotation = value);
            public static Value<float> TweenRotateX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.eulerAngles.x, () => transform.eulerAngles.x + (relative ? target : Mathf.DeltaAngle(transform.eulerAngles.x, target)), duration, value => { var euler = transform.eulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.eulerAngles.y, () => transform.eulerAngles.y + (relative ? target : Mathf.DeltaAngle(transform.eulerAngles.y, target)), duration, value => { var euler = transform.eulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.eulerAngles.z, () => transform.eulerAngles.z + (relative ? target : Mathf.DeltaAngle(transform.eulerAngles.z, target)), duration, value => { var euler = transform.eulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); });


            // L O C A L   R O T A T I O N
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Transform target, float duration) => Value(transform, () => transform.localRotation, () => target.localRotation, duration, value => transform.localRotation = value);
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Quaternion target, float duration) => Value(transform, () => transform.localRotation, target, duration, value => transform.localRotation = value);
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.localRotation, () => Quaternion.Euler(target), duration, value => transform.localRotation = value);
            public static Value<float> TweenRotateLocalX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localEulerAngles.x, () => transform.localEulerAngles.x + (relative ? target : Mathf.DeltaAngle(transform.localEulerAngles.x, target)), duration, value => { var euler = transform.localEulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localEulerAngles.y, () => transform.localEulerAngles.y + (relative ? target : Mathf.DeltaAngle(transform.localEulerAngles.y, target)), duration, value => { var euler = transform.localEulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localEulerAngles.z, () => transform.localEulerAngles.z + (relative ? target : Mathf.DeltaAngle(transform.localEulerAngles.z, target)), duration, value => { var euler = transform.localEulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); });


            // S C A L E
            public static Value<Vector3> TweenScale(this Transform transform, Transform target, float duration) => Value(transform, () => transform.localScale, () => target.localScale, duration, value => transform.localScale = value);
            public static Value<Vector3> TweenScale(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.localScale, () => relative ? transform.localScale + target : target, duration, value => transform.localScale = value);
            public static Value<float> TweenScaleX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localScale.x, () => relative ? transform.localScale.x + target : target, duration, value => { var scale = transform.localScale; scale.x = value; transform.localScale = scale; });
            public static Value<float> TweenScaleY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localScale.y, () => relative ? transform.localScale.y + target : target, duration, value => { var scale = transform.localScale; scale.y = value; transform.localScale = scale; });
            public static Value<float> TweenScaleZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localScale.z, () => relative ? transform.localScale.z + target : target, duration, value => { var scale = transform.localScale; scale.z = value; transform.localScale = scale; });
      }
}