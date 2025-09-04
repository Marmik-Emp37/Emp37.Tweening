using UnityEngine;
using UnityEngine.UI;

namespace Emp37.Tweening
{
      using Element;

      using static Tween;

      public static class Extensions
      {
            // A U D I O
            public static Value<float> TweenPitch(this AudioSource source, float target, float duration) => Value(() => source.pitch, target, duration, value => source.pitch = value, source);
            public static Value<float> TweenVolume(this AudioSource source, float target, float duration) => Value(() => source.volume, target, duration, value => source.volume = value, source);

            // C A M E R A
            public static Value<float> TweenFOV(this Camera camera, float target, float duration) => Value(() => camera.fieldOfView, target, duration, value => camera.fieldOfView = value, camera);
            public static Value<float> TweenSize(this Camera camera, float target, float duration)
            {
                  if (camera.orthographic)
                  {
                        return Value(() => camera.orthographicSize, target, duration, value => camera.orthographicSize = value, camera);
                  }
                  else
                  {
                        Debug.LogWarning($"{nameof(TweenSize)} called on perspective camera '{camera.name}'. Only orthographic cameras are supported.", camera);
                        return Element.Value<float>.Empty;
                  }
            }

            // R E N D E R E R S
            public static Value<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration) => Value(() => renderer.color, target, duration, value => renderer.color = value, renderer);
            public static Value<float> TweenAlpha(this SpriteRenderer renderer, float target, float duration) => Value(() => renderer.color.a, target, duration, value => { var color = renderer.color; color.a = value; renderer.color = color; }, renderer);

            // T R A N S F O R M
            public static Value<Vector2> TweenMove(this Transform transform, Vector2 target, float duration) => Value(() => (Vector2) transform.position, target, duration, value => transform.position = value, transform);
            public static Value<Vector3> TweenMove(this Transform transform, Vector3 target, float duration) => Value(() => transform.position, target, duration, value => transform.position = value, transform);
            public static Value<float> TweenMoveX(this Transform transform, float target, float duration) => Value(() => transform.position.x, target, duration, value => { var pos = transform.position; pos.x = value; transform.position = pos; }, transform);
            public static Value<float> TweenMoveY(this Transform transform, float target, float duration) => Value(() => transform.position.y, target, duration, value => { var pos = transform.position; pos.y = value; transform.position = pos; }, transform);
            public static Value<float> TweenMoveZ(this Transform transform, float target, float duration) => Value(() => transform.position.z, target, duration, value => { var pos = transform.position; pos.z = value; transform.position = pos; }, transform);
            public static Value<Vector2> TweenMoveLocal(this Transform transform, Vector2 target, float duration) => Value(() => (Vector2) transform.localPosition, target, duration, value => transform.localPosition = value, transform);
            public static Value<Vector3> TweenMoveLocal(this Transform transform, Vector3 target, float duration) => Value(() => transform.localPosition, target, duration, value => transform.localPosition = value, transform);
            public static Value<float> TweenMoveLocalX(this Transform transform, float target, float duration) => Value(() => transform.localPosition.x, target, duration, value => { var pos = transform.localPosition; pos.x = value; transform.localPosition = pos; }, transform);
            public static Value<float> TweenMoveLocalY(this Transform transform, float target, float duration) => Value(() => transform.localPosition.y, target, duration, value => { var pos = transform.localPosition; pos.y = value; transform.localPosition = pos; }, transform);
            public static Value<float> TweenMoveLocalZ(this Transform transform, float target, float duration) => Value(() => transform.localPosition.z, target, duration, value => { var pos = transform.localPosition; pos.z = value; transform.localPosition = pos; }, transform);
            public static Value<Quaternion> TweenRotate(this Transform transform, Quaternion target, float duration) => Value(() => transform.rotation, target, duration, value => transform.rotation = value, transform);
            public static Value<Quaternion> TweenRotate(this Transform transform, Vector3 target, float duration) => Value(() => transform.rotation, Quaternion.Euler(target), duration, value => transform.rotation = value, transform);
            public static Value<float> TweenRotateX(this Transform transform, float target, float duration) => Value(() => transform.eulerAngles.x, transform.eulerAngles.x + Mathf.DeltaAngle(transform.eulerAngles.x, target), duration, value => { var euler = transform.eulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); }, transform);
            public static Value<float> TweenRotateY(this Transform transform, float target, float duration) => Value(() => transform.eulerAngles.y, transform.eulerAngles.y + Mathf.DeltaAngle(transform.eulerAngles.y, target), duration, value => { var euler = transform.eulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); }, transform);
            public static Value<float> TweenRotateZ(this Transform transform, float target, float duration) => Value(() => transform.eulerAngles.z, transform.eulerAngles.z + Mathf.DeltaAngle(transform.eulerAngles.z, target), duration, value => { var euler = transform.eulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); }, transform);
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Quaternion target, float duration) => Value(() => transform.localRotation, target, duration, value => transform.localRotation = value, transform);
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Vector3 target, float duration) => Value(() => transform.localRotation, Quaternion.Euler(target), duration, value => transform.localRotation = value, transform);
            public static Value<float> TweenRotateLocalX(this Transform transform, float target, float duration) => Value(() => transform.localEulerAngles.x, transform.localEulerAngles.x + Mathf.DeltaAngle(transform.localEulerAngles.x, target), duration, value => { var euler = transform.localEulerAngles; euler.x = value; transform.localRotation = Quaternion.Euler(euler); }, transform);
            public static Value<float> TweenRotateLocalY(this Transform transform, float target, float duration) => Value(() => transform.localEulerAngles.y, transform.localEulerAngles.y + Mathf.DeltaAngle(transform.localEulerAngles.y, target), duration, value => { var euler = transform.localEulerAngles; euler.y = value; transform.localRotation = Quaternion.Euler(euler); }, transform);
            public static Value<float> TweenRotateLocalZ(this Transform transform, float target, float duration) => Value(() => transform.localEulerAngles.z, transform.localEulerAngles.z + Mathf.DeltaAngle(transform.localEulerAngles.z, target), duration, value => { var euler = transform.localEulerAngles; euler.z = value; transform.localRotation = Quaternion.Euler(euler); }, transform);
            public static Value<Vector3> TweenScale(this Transform transform, Vector3 target, float duration) => Value(() => transform.localScale, target, duration, value => transform.localScale = value, transform);
            public static Value<float> TweenScaleX(this Transform transform, float target, float duration) => Value(() => transform.localScale.x, target, duration, value => { var scale = transform.localScale; scale.x = value; transform.localScale = scale; }, transform);
            public static Value<float> TweenScaleY(this Transform transform, float target, float duration) => Value(() => transform.localScale.y, target, duration, value => { var scale = transform.localScale; scale.y = value; transform.localScale = scale; }, transform);
            public static Value<float> TweenScaleZ(this Transform transform, float target, float duration) => Value(() => transform.localScale.z, target, duration, value => { var scale = transform.localScale; scale.z = value; transform.localScale = scale; }, transform);

            // U I
            public static Value<float> TweenAlpha(this Graphic graphic, float target, float duration) => Value(() => graphic.color.a, target, duration, value => { var color = graphic.color; color.a = value; graphic.color = color; }, graphic);
            public static Value<float> TweenFade(this CanvasGroup group, float target, float duration) => Value(() => group.alpha, target, duration, value => group.alpha = value, group);
            public static Value<Color> TweenColor(this Image image, Color target, float duration) => Value(() => image.color, target, duration, value => image.color = value, image);
            public static Value<float> TweenFill(this Image image, float target, float duration) => Value(() => image.fillAmount, target, duration, value => image.fillAmount = value, image);
            public static Value<Vector2> TweenSize(this RectTransform transform, Vector2 target, float duration) => Value(() => transform.sizeDelta, target, duration, value => transform.sizeDelta = value, transform);
            public static Value<Vector2> TweenMove(this RectTransform transform, Vector2 target, float duration) => Value(() => transform.anchoredPosition, target, duration, value => transform.anchoredPosition = value, transform);
            public static Value<Vector3> TweenMove(this RectTransform transform, Vector3 target, float duration) => Value(() => transform.anchoredPosition3D, target, duration, value => transform.anchoredPosition3D = value, transform);
      }
}