using UnityEngine;
using UnityEngine.UI;

namespace Emp37.Tweening
{
      public static class Extensions
      {
            #region U T I L I T Y
            private static Vector3 ReplaceAxis(Vector3 original, int axis, float value)
            {
                  original[axis] = value;
                  return original;
            }
            #endregion

            #region A U D I O
            public static Tween<float> TweenPitch(this AudioSource source, float target, float duration) => Tween.Create(() => source.pitch, target, duration, value => source.pitch = value);
            public static Tween<float> TweenVolume(this AudioSource source, float target, float duration) => Tween.Create(() => source.volume, target, duration, value => source.volume = value);
            #endregion

            #region C A M E R A
            public static Tween<float> TweenFOV(this Camera camera, float target, float duration) => Tween.Create(() => camera.fieldOfView, target, duration, value => camera.fieldOfView = value);
            public static Tween<float> TweenSize(this Camera camera, float target, float duration)
            {
                  if (camera.orthographic)
                  {
                        return Tween.Create(() => camera.orthographicSize, target, duration, value => camera.orthographicSize = value);
                  }
                  else
                  {
                        Debug.LogWarning($"{nameof(TweenSize)} called on perspective camera '{camera.name}'. Only orthographic cameras are supported.", camera);
                        return Tween<float>.Empty;
                  }
            }
            #endregion

            #region R E N D E R E R S
            public static Tween<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration) => Tween.Create(() => renderer.color, target, duration, value => renderer.color = value);
            public static Tween<float> TweenAlpha(this SpriteRenderer renderer, float target, float duration) => Tween.Create(() => renderer.color.a, target, duration, value => { var color = renderer.color; color.a = value; renderer.color = color; });
            #endregion

            #region S Y S T EM
            public static Tween<float> TweenTimeScale(float target, float duration) => Tween.Create(() => Time.timeScale, target, duration, value => Time.timeScale = value);
            #endregion

            #region T R A N S F O R M
            public static Tween<Vector2> TweenMove(this Transform transform, Vector2 target, float duration) => Tween.Create(() => (Vector2) transform.position, target, duration, value => transform.position = value);
            public static Tween<Vector2> TweenMoveLocal(this Transform transform, Vector2 target, float duration) => Tween.Create(() => (Vector2) transform.localPosition, target, duration, value => transform.localPosition = value);

            public static Tween<Vector3> TweenMove(this Transform transform, Vector3 target, float duration) => Tween.Create(() => transform.position, target, duration, value => transform.position = value);
            public static Tween<float> TweenMoveX(this Transform transform, float target, float duration) => Tween.Create(() => transform.position.x, target, duration, value => transform.position = ReplaceAxis(transform.position, 0, value));
            public static Tween<float> TweenMoveY(this Transform transform, float target, float duration) => Tween.Create(() => transform.position.y, target, duration, value => transform.position = ReplaceAxis(transform.position, 1, value));
            public static Tween<float> TweenMoveZ(this Transform transform, float target, float duration) => Tween.Create(() => transform.position.z, target, duration, value => transform.position = ReplaceAxis(transform.position, 2, value));

            public static Tween<Vector3> TweenMoveLocal(this Transform transform, Vector3 target, float duration) => Tween.Create(() => transform.localPosition, target, duration, value => transform.localPosition = value);
            public static Tween<float> TweenMoveLocalX(this Transform transform, float target, float duration) => Tween.Create(() => transform.localPosition.x, target, duration, value => transform.localPosition = ReplaceAxis(transform.localPosition, 0, value));
            public static Tween<float> TweenMoveLocalY(this Transform transform, float target, float duration) => Tween.Create(() => transform.localPosition.y, target, duration, value => transform.localPosition = ReplaceAxis(transform.localPosition, 1, value));
            public static Tween<float> TweenMoveLocalZ(this Transform transform, float target, float duration) => Tween.Create(() => transform.localPosition.z, target, duration, value => transform.localPosition = ReplaceAxis(transform.localPosition, 2, value));

            public static Tween<Quaternion> TweenRotate(this Transform transform, Quaternion target, float duration) => Tween.Create(() => transform.rotation, target, duration, value => transform.rotation = value);
            public static Tween<Quaternion> TweenRotate(this Transform transform, Vector3 target, float duration) => Tween.Create(() => transform.rotation, Quaternion.Euler(target), duration, value => transform.rotation = value);
            public static Tween<float> TweenRotateX(this Transform transform, float target, float duration) => Tween.Create(() => transform.eulerAngles.x, transform.eulerAngles.x + Mathf.DeltaAngle(transform.eulerAngles.x, target), duration, value => transform.rotation = Quaternion.Euler(ReplaceAxis(transform.eulerAngles, 0, value)));
            public static Tween<float> TweenRotateY(this Transform transform, float target, float duration) => Tween.Create(() => transform.eulerAngles.y, transform.eulerAngles.y + Mathf.DeltaAngle(transform.eulerAngles.y, target), duration, value => transform.rotation = Quaternion.Euler(ReplaceAxis(transform.eulerAngles, 1, value)));
            public static Tween<float> TweenRotateZ(this Transform transform, float target, float duration) => Tween.Create(() => transform.eulerAngles.z, transform.eulerAngles.z + Mathf.DeltaAngle(transform.eulerAngles.z, target), duration, value => transform.rotation = Quaternion.Euler(ReplaceAxis(transform.eulerAngles, 2, value)));

            public static Tween<Quaternion> TweenRotateLocal(this Transform transform, Quaternion target, float duration) => Tween.Create(() => transform.localRotation, target, duration, value => transform.localRotation = value);
            public static Tween<Quaternion> TweenRotateLocal(this Transform transform, Vector3 target, float duration) => Tween.Create(() => transform.localRotation, Quaternion.Euler(target), duration, value => transform.localRotation = value);
            public static Tween<float> TweenRotateLocalX(this Transform transform, float target, float duration) => Tween.Create(() => transform.localEulerAngles.x, transform.localEulerAngles.x + Mathf.DeltaAngle(transform.localEulerAngles.x, target), duration, value => transform.localRotation = Quaternion.Euler(ReplaceAxis(transform.localEulerAngles, 0, value)));
            public static Tween<float> TweenRotateLocalY(this Transform transform, float target, float duration) => Tween.Create(() => transform.localEulerAngles.y, transform.localEulerAngles.y + Mathf.DeltaAngle(transform.localEulerAngles.y, target), duration, value => transform.localRotation = Quaternion.Euler(ReplaceAxis(transform.localEulerAngles, 1, value)));
            public static Tween<float> TweenRotateLocalZ(this Transform transform, float target, float duration) => Tween.Create(() => transform.localEulerAngles.z, transform.localEulerAngles.z + Mathf.DeltaAngle(transform.localEulerAngles.z, target), duration, value => transform.localRotation = Quaternion.Euler(ReplaceAxis(transform.localEulerAngles, 2, value)));

            public static Tween<Vector3> TweenScale(this Transform transform, Vector3 target, float duration) => Tween.Create(() => transform.localScale, target, duration, value => transform.localScale = value);
            public static Tween<float> TweenScaleX(this Transform transform, float target, float duration) => Tween.Create(() => transform.localScale.x, target, duration, value => transform.localScale = ReplaceAxis(transform.localScale, 0, value));
            public static Tween<float> TweenScaleY(this Transform transform, float target, float duration) => Tween.Create(() => transform.localScale.y, target, duration, value => transform.localScale = ReplaceAxis(transform.localScale, 1, value));
            public static Tween<float> TweenScaleZ(this Transform transform, float target, float duration) => Tween.Create(() => transform.localScale.z, target, duration, value => transform.localScale = ReplaceAxis(transform.localScale, 2, value));
            #endregion

            #region U I
            public static Tween<float> TweenAlpha(this Graphic graphic, float target, float duration) => Tween.Create(() => graphic.color.a, target, duration, value => { var color = graphic.color; color.a = value; graphic.color = color; });
            public static Tween<float> TweenFade(this CanvasGroup group, float target, float duration) => Tween.Create(() => group.alpha, target, duration, value => group.alpha = value);

            public static Tween<Color> TweenColor(this Image image, Color target, float duration) => Tween.Create(() => image.color, target, duration, value => image.color = value);
            public static Tween<float> TweenFill(this Image image, float target, float duration) => Tween.Create(() => image.fillAmount, target, duration, value => image.fillAmount = value);

            public static Tween<Vector2> TweenMove(this RectTransform transform, Vector2 target, float duration) => Tween.Create(() => transform.anchoredPosition, target, duration, value => transform.anchoredPosition = value);
            public static Tween<Vector3> TweenMove(this RectTransform transform, Vector3 target, float duration) => Tween.Create(() => transform.anchoredPosition3D, target, duration, value => transform.anchoredPosition3D = value);
            public static Tween<Vector2> TweenSize(this RectTransform rect, Vector2 target, float duration) => Tween.Create(() => rect.sizeDelta, target, duration, value => rect.sizeDelta = value);
            #endregion
      }
}