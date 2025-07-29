using UnityEngine;
using UnityEngine.UI;

namespace Emp37.Tweening
{
      public static class Tweens
      {
            #region U T I L I T Y
            public static Vector3 ReplaceAxis(Vector3 original, int axis, float value)
            {
                  original[axis] = value;
                  return original;
            }
            #endregion

            #region A U D I O
            public static Handle<float> TweenPitch(this AudioSource source, float target, float duration)
            {
                  var tween = Factory.Create(() => source.pitch, target, duration, value => source.pitch = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<float> TweenVolume(this AudioSource source, float target, float duration)
            {
                  var tween = Factory.Create(() => source.volume, target, duration, value => source.volume = value);
                  Factory.Add(tween);
                  return tween;
            }
            #endregion
            #region C A M E R A
            public static Handle<float> TweenFOV(this Camera camera, float target, float duration)
            {
                  var tween = Factory.Create(() => camera.fieldOfView, target, duration, value => camera.fieldOfView = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<float> TweenSize(this Camera camera, float target, float duration)
            {
                  if (camera.orthographic)
                  {
                        var tween = Factory.Create(() => camera.orthographicSize, target, duration, value => camera.orthographicSize = value);
                        Factory.Add(tween);
                        return tween;
                  }
                  else
                  {
                        Debug.LogWarning($"{nameof(TweenSize)} called on perspective camera '{camera.name}'. Only orthographic cameras are supported.", camera);
                        return Handle<float>.Empty;
                  }
            }
            #endregion
            #region R E N D E R E R S
            public static Handle<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration)
            {
                  var tween = Factory.Create(() => renderer.color, target, duration, value => renderer.color = value);
                  Factory.Add(tween);
                  return tween;
            }
            #endregion
            #region S Y S T EM
            public static Handle<float> TweenTimeScale(float target, float duration)
            {
                  var tween = Factory.Create(() => Time.timeScale, target, duration, value => Time.timeScale = value);
                  Factory.Add(tween);
                  return tween;
            }
            #endregion
            #region T R A N S F O R M
            public static Handle<Vector2> TweenMove(this Transform transform, Vector2 target, float duration)
            {
                  var tween = Factory.Create(() => (Vector2) transform.position, target, duration, value => transform.position = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector2> TweenMoveLocal(this Transform transform, Vector2 target, float duration)
            {
                  var tween = Factory.Create(() => (Vector2) transform.localPosition, target, duration, value => transform.localPosition = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector3> TweenMove(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Factory.Create(() => transform.position, target, duration, value => transform.position = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector3> TweenMoveX(this Transform transform, float target, float duration) => TweenMove(transform, ReplaceAxis(transform.position, 0, target), duration);
            public static Handle<Vector3> TweenMoveY(this Transform transform, float target, float duration) => TweenMove(transform, ReplaceAxis(transform.position, 1, target), duration);
            public static Handle<Vector3> TweenMoveZ(this Transform transform, float target, float duration) => TweenMove(transform, ReplaceAxis(transform.position, 2, target), duration);
            public static Handle<Vector3> TweenMoveLocal(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Factory.Create(() => transform.localPosition, target, duration, value => transform.localPosition = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector3> TweenMoveLocalX(this Transform transform, float target, float duration) => TweenMoveLocal(transform, ReplaceAxis(transform.localPosition, 0, target), duration);
            public static Handle<Vector3> TweenMoveLocalY(this Transform transform, float target, float duration) => TweenMoveLocal(transform, ReplaceAxis(transform.localPosition, 1, target), duration);
            public static Handle<Vector3> TweenMoveLocalZ(this Transform transform, float target, float duration) => TweenMoveLocal(transform, ReplaceAxis(transform.localPosition, 2, target), duration);
            public static Handle<Quaternion> TweenRotate(this Transform transform, Quaternion target, float duration)
            {
                  var tween = Factory.Create(() => transform.rotation, target, duration, value => transform.rotation = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Quaternion> TweenRotate(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Factory.Create(() => transform.rotation, Quaternion.Euler(target), duration, value => transform.rotation = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Quaternion> TweenRotateX(this Transform transform, float target, float duration) => TweenRotate(transform, ReplaceAxis(transform.eulerAngles, 0, target), duration);
            public static Handle<Quaternion> TweenRotateY(this Transform transform, float target, float duration) => TweenRotate(transform, ReplaceAxis(transform.eulerAngles, 1, target), duration);
            public static Handle<Quaternion> TweenRotateZ(this Transform transform, float target, float duration) => TweenRotate(transform, ReplaceAxis(transform.eulerAngles, 2, target), duration);
            public static Handle<Quaternion> TweenRotateLocal(this Transform transform, Quaternion target, float duration)
            {
                  var tween = Factory.Create(() => transform.localRotation, target, duration, value => transform.localRotation = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Quaternion> TweenRotateLocal(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Factory.Create(() => transform.localRotation, Quaternion.Euler(target), duration, value => transform.localRotation = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Quaternion> TweenRotateLocalX(this Transform transform, float target, float duration) => TweenRotateLocal(transform, ReplaceAxis(transform.localEulerAngles, 0, target), duration);
            public static Handle<Quaternion> TweenRotateLocalY(this Transform transform, float target, float duration) => TweenRotateLocal(transform, ReplaceAxis(transform.localEulerAngles, 1, target), duration);
            public static Handle<Quaternion> TweenRotateLocalZ(this Transform transform, float target, float duration) => TweenRotateLocal(transform, ReplaceAxis(transform.localEulerAngles, 2, target), duration);
            public static Handle<Vector3> TweenScale(this Transform transform, Vector3 target, float duration)
            {
                  var tween = Factory.Create(() => transform.localScale, target, duration, value => transform.localScale = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector3> TweenScaleX(this Transform transform, float target, float duration) => TweenScale(transform, ReplaceAxis(transform.localScale, 0, target), duration);
            public static Handle<Vector3> TweenScaleY(this Transform transform, float target, float duration) => TweenScale(transform, ReplaceAxis(transform.localScale, 1, target), duration);
            public static Handle<Vector3> TweenScaleZ(this Transform transform, float target, float duration) => TweenScale(transform, ReplaceAxis(transform.localScale, 2, target), duration);
            #endregion
            #region U I
            public static Handle<float> TweenAlpha(this Graphic graphic, float target, float duration)
            {
                  var tween = Factory.Create(() => graphic.color.a, target, duration, value => { Color c = graphic.color; c.a = value; graphic.color = c; });
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Color> TweenColor(this Image image, Color target, float duration)
            {
                  var tween = Factory.Create(() => image.color, target, duration, value => image.color = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<float> TweenFade(this CanvasGroup group, float target, float duration)
            {
                  var tween = Factory.Create(() => group.alpha, target, duration, value => group.alpha = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<float> TweenFill(this Image image, float target, float duration)
            {
                  var tween = Factory.Create(() => image.fillAmount, target, duration, value => image.fillAmount = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector2> TweenMove(this RectTransform transform, Vector2 target, float duration)
            {
                  var tween = Factory.Create(() => transform.anchoredPosition, target, duration, value => transform.anchoredPosition = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector3> TweenMove(this RectTransform transform, Vector3 target, float duration)
            {
                  var tween = Factory.Create(() => transform.anchoredPosition3D, target, duration, value => transform.anchoredPosition3D = value);
                  Factory.Add(tween);
                  return tween;
            }
            public static Handle<Vector2> TweenSize(this RectTransform rect, Vector2 target, float duration)
            {
                  var tween = Factory.Create(() => rect.sizeDelta, target, duration, value => rect.sizeDelta = value);
                  Factory.Add(tween);
                  return tween;
            }
            #endregion
      }
}