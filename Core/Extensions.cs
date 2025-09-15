using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Emp37.Tweening
{
      using Element;

      using static Tween;

      public static class Extensions
      {
            // P O S I T I O N
            public static Value<Vector3> TweenMove(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.position, target, duration, value => transform.position = value);
            public static Value<Vector3> TweenMoveBY(this Transform transform, Vector3 offset, float duration) => Value(transform, () => transform.position, () => offset + transform.position, duration, value => transform.position = value);
            public static Value<Vector2> TweenMove(this Transform transform, Vector2 target, float duration) => Value(transform, () => (Vector2) transform.position, target, duration, value => transform.position = value);
            public static Value<Vector2> TweenMoveBY(this Transform transform, Vector2 offset, float duration) => Value(transform, () => (Vector2) transform.position, () => offset + (Vector2) transform.position, duration, value => transform.position = value);
            public static Value<float> TweenMoveX(this Transform transform, float target, float duration) => Value(transform, () => transform.position.x, target, duration, value => { var pos = transform.position; pos.x = value; transform.position = pos; });
            public static Value<float> TweenMoveXBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.position.x, () => offset + transform.position.x, duration, value => { var pos = transform.position; pos.x = value; transform.position = pos; });
            public static Value<float> TweenMoveY(this Transform transform, float target, float duration) => Value(transform, () => transform.position.y, target, duration, value => { var pos = transform.position; pos.y = value; transform.position = pos; });
            public static Value<float> TweenMoveYBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.position.y, () => offset + transform.position.y, duration, value => { var pos = transform.position; pos.y = value; transform.position = pos; });
            public static Value<float> TweenMoveZ(this Transform transform, float target, float duration) => Value(transform, () => transform.position.z, target, duration, value => { var pos = transform.position; pos.z = value; transform.position = pos; });
            public static Value<float> TweenMoveZBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.position.z, () => offset + transform.position.z, duration, value => { var pos = transform.position; pos.z = value; transform.position = pos; });


            // L O C A L   P O S I T I O N
            public static Value<Vector3> TweenMoveLocal(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.localPosition, target, duration, value => transform.localPosition = value);
            public static Value<Vector3> TweenMoveLocalBY(this Transform transform, Vector3 offset, float duration) => Value(transform, () => transform.localPosition, () => offset + transform.localPosition, duration, value => transform.localPosition = value);
            public static Value<Vector2> TweenMoveLocal(this Transform transform, Vector2 target, float duration) => Value(transform, () => (Vector2) transform.localPosition, target, duration, value => transform.localPosition = value);
            public static Value<Vector2> TweenMoveLocalBY(this Transform transform, Vector2 offset, float duration) => Value(transform, () => (Vector2) transform.localPosition, () => offset + (Vector2) transform.localPosition, duration, value => transform.localPosition = value);
            public static Value<float> TweenMoveLocalX(this Transform transform, float target, float duration) => Value(transform, () => transform.localPosition.x, target, duration, value => { var pos = transform.localPosition; pos.x = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalXBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localPosition.x, () => offset + transform.localPosition.x, duration, value => { var pos = transform.localPosition; pos.x = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalY(this Transform transform, float target, float duration) => Value(transform, () => transform.localPosition.y, target, duration, value => { var pos = transform.localPosition; pos.y = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalYBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localPosition.y, () => offset + transform.localPosition.y, duration, value => { var pos = transform.localPosition; pos.y = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalZ(this Transform transform, float target, float duration) => Value(transform, () => transform.localPosition.z, target, duration, value => { var pos = transform.localPosition; pos.z = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalZBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localPosition.z, () => offset + transform.localPosition.z, duration, value => { var pos = transform.localPosition; pos.z = value; transform.localPosition = pos; });


            // R O T A T I O N
            public static Value<Quaternion> TweenRotate(this Transform transform, Quaternion target, float duration) => Value(transform, () => transform.rotation, target, duration, value => transform.rotation = value);
            public static Value<Quaternion> TweenRotate(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.rotation, () => Quaternion.Euler(target), duration, value => transform.rotation = value);
            public static Value<float> TweenRotateX(this Transform transform, float target, float duration) => Value(transform, () => transform.eulerAngles.x, () => transform.eulerAngles.x + Mathf.DeltaAngle(transform.eulerAngles.x, target), duration, value => { var euler = transform.eulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateXBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.eulerAngles.x, () => offset + transform.eulerAngles.x, duration, value => { var euler = transform.eulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateY(this Transform transform, float target, float duration) => Value(transform, () => transform.eulerAngles.y, () => transform.eulerAngles.y + Mathf.DeltaAngle(transform.eulerAngles.y, target), duration, value => { var euler = transform.eulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateYBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.eulerAngles.y, () => offset + transform.eulerAngles.y, duration, value => { var euler = transform.eulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateZ(this Transform transform, float target, float duration) => Value(transform, () => transform.eulerAngles.z, () => transform.eulerAngles.z + Mathf.DeltaAngle(transform.eulerAngles.z, target), duration, value => { var euler = transform.eulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateZBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.eulerAngles.z, () => offset + transform.eulerAngles.z, duration, value => { var euler = transform.eulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); });


            // L O C A L   R O T A T I O N
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Quaternion target, float duration) => Value(transform, () => transform.localRotation, target, duration, value => transform.localRotation = value);
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.localRotation, () => Quaternion.Euler(target), duration, value => transform.localRotation = value);
            public static Value<float> TweenRotateLocalX(this Transform transform, float target, float duration) => Value(transform, () => transform.localEulerAngles.x, () => transform.localEulerAngles.x + Mathf.DeltaAngle(transform.localEulerAngles.x, target), duration, value => { var euler = transform.localEulerAngles; euler.x = value; transform.localRotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalXBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localEulerAngles.x, () => offset + transform.localEulerAngles.x, duration, value => { var euler = transform.localEulerAngles; euler.x = value; transform.localRotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalY(this Transform transform, float target, float duration) => Value(transform, () => transform.localEulerAngles.y, () => transform.localEulerAngles.y + Mathf.DeltaAngle(transform.localEulerAngles.y, target), duration, value => { var euler = transform.localEulerAngles; euler.y = value; transform.localRotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalYBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localEulerAngles.y, () => offset + transform.localEulerAngles.y, duration, value => { var euler = transform.localEulerAngles; euler.y = value; transform.localRotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalZ(this Transform transform, float target, float duration) => Value(transform, () => transform.localEulerAngles.z, () => transform.localEulerAngles.z + Mathf.DeltaAngle(transform.localEulerAngles.z, target), duration, value => { var euler = transform.localEulerAngles; euler.z = value; transform.localRotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalZBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localEulerAngles.z, () => offset + transform.localEulerAngles.z, duration, value => { var euler = transform.localEulerAngles; euler.z = value; transform.localRotation = Quaternion.Euler(euler); });


            // S C A L E
            public static Value<Vector3> TweenScale(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.localScale, target, duration, value => transform.localScale = value);
            public static Value<Vector3> TweenScaleBY(this Transform transform, Vector3 offset, float duration) => Value(transform, () => transform.localScale, () => offset + transform.localScale, duration, value => transform.localScale = value);
            public static Value<float> TweenScaleX(this Transform transform, float target, float duration) => Value(transform, () => transform.localScale.x, target, duration, value => { var scale = transform.localScale; scale.x = value; transform.localScale = scale; });
            public static Value<float> TweenScaleXBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localScale.x, () => offset + transform.localScale.x, duration, value => { var scale = transform.localScale; scale.x = value; transform.localScale = scale; });
            public static Value<float> TweenScaleY(this Transform transform, float target, float duration) => Value(transform, () => transform.localScale.y, target, duration, value => { var scale = transform.localScale; scale.y = value; transform.localScale = scale; });
            public static Value<float> TweenScaleYBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localScale.y, () => offset + transform.localScale.y, duration, value => { var scale = transform.localScale; scale.y = value; transform.localScale = scale; });
            public static Value<float> TweenScaleZ(this Transform transform, float target, float duration) => Value(transform, () => transform.localScale.z, target, duration, value => { var scale = transform.localScale; scale.z = value; transform.localScale = scale; });
            public static Value<float> TweenScaleZBY(this Transform transform, float offset, float duration) => Value(transform, () => transform.localScale.z, () => offset + transform.localScale.z, duration, value => { var scale = transform.localScale; scale.z = value; transform.localScale = scale; });


            // R E N D E R E R
            public static Value<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration) => Value(renderer, () => renderer.color, target, duration, value => renderer.color = value);
            public static Value<float> TweenPropertyBlock(this Renderer renderer, string property, float target, float duration)
            {
                  MaterialPropertyBlock block = new();
                  renderer.GetPropertyBlock(block);

                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain float property '{property}'.");
                        return Element.Value<float>.Empty;
                  }

                  int id = Shader.PropertyToID(property);
                  return Value(renderer, () => block.GetFloat(id), target, duration, value => { block.SetFloat(id, value); renderer.SetPropertyBlock(block); });
            }
            public static Value<Color> TweenPropertyBlock(this Renderer renderer, string property, Color target, float duration)
            {
                  MaterialPropertyBlock block = new();
                  renderer.GetPropertyBlock(block);

                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain float property '{property}'.");
                        return Element.Value<Color>.Empty;
                  }

                  int id = Shader.PropertyToID(property);
                  return Value(renderer, () => block.GetColor(id), target, duration, value => { block.SetColor(id, value); renderer.SetPropertyBlock(block); });
            }
            public static Value<Vector4> TweenPropertyBlock(this Renderer renderer, string property, Vector4 target, float duration)
            {
                  MaterialPropertyBlock block = new();
                  renderer.GetPropertyBlock(block);

                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween ($"Renderer '{renderer.name}' does not contain float property '{property}'.");
                        return Element.Value<Vector4>.Empty;
                  }

                  int id = Shader.PropertyToID(property);
                  return Value(renderer, () => block.GetVector(id), target, duration, value => { block.SetVector(id, value); renderer.SetPropertyBlock(block); });
            }


            // M A T E R I A L
            public static Value<Color> TweenColor(this Material material, Color target, float duration) => Value(material, () => material.color, target, duration, value => material.color = value);
            public static Value<float> TweenProperty(this Material material, string property, float target, float duration)
            {
                  if (!material.HasProperty(property))
                  {
                        Log.RejectTween($"Material '{material.name}' does not contain float property '{property}'.");
                        return Element.Value<float>.Empty;
                  }

                  int id = Shader.PropertyToID(property);
                  return Value(material, () => material.GetFloat(id), target, duration, value => material.SetFloat(id, value));
            }
            public static Value<Color> TweenProperty(this Material material, string property, Color target, float duration)
            {
                  if (!material.HasProperty(property))
                  {
                        Log.RejectTween($"Material '{material.name}' does not contain Color property '{property}'.");
                        return Element.Value<Color>.Empty;
                  }

                  int id = Shader.PropertyToID(property);
                  return Value(material, () => material.GetColor(id), target, duration, value => material.SetColor(id, value));
            }
            public static Value<Vector4> TweenProperty(this Material material, string property, Vector4 target, float duration)
            {
                  if (!material.HasProperty(property))
                  {
                        Log.RejectTween($"Material '{material.name}' does not contain Vector property '{property}'.");
                        return Element.Value<Vector4>.Empty;
                  }

                  int id = Shader.PropertyToID(property);
                  return Value(material, () => material.GetVector(id), target, duration, value => material.SetVector(id, value));
            }


            // U I
            public static Value<Vector2> TweenMove(this RectTransform transform, Vector2 target, float duration) => Value(transform, () => transform.anchoredPosition, target, duration, value => transform.anchoredPosition = value);
            public static Value<Vector3> TweenMove(this RectTransform transform, Vector3 target, float duration) => Value(transform, () => transform.anchoredPosition3D, target, duration, value => transform.anchoredPosition3D = value);
            public static Value<Vector2> TweenSize(this RectTransform transform, Vector2 target, float duration) => Value(transform, () => transform.sizeDelta, target, duration, value => transform.sizeDelta = value);
            public static Value<float> TweenFade(this CanvasGroup group, float target, float duration) => Value(group, () => group.alpha, target, duration, value => group.alpha = value);


            // U I - G R A P H I C S
            public static Value<float> TweenAlpha(this Graphic graphic, float target, float duration) => Value(graphic, () => graphic.color.a, target, duration, value => { var color = graphic.color; color.a = value; graphic.color = color; });
            public static Value<Color> TweenColor(this Graphic graphic, Color target, float duration) => Value(graphic, () => graphic.color, target, duration, value => graphic.color = value);
            public static Value<float> TweenFill(this Image image, float target, float duration) => Value(image, () => image.fillAmount, target, duration, value => image.fillAmount = value);
            public static Value<float> TweenText(this Text text, string target, float duration) => Value(text, () => 0, target.Length, duration, value => { int count = Mathf.Clamp(Mathf.FloorToInt(value), 0, target.Length); text.text = target[..count]; });
            public static Value<float> TweenText(this TMP_Text text, string target, float duration) => Value(text, () => 0, target.Length, duration, value => { int count = Mathf.Clamp(Mathf.FloorToInt(value), 0, target.Length); text.text = target[..count]; });
            public static Value<float> TweenNumber(this Text text, float target, float duration, string format) => Value(text, () => float.TryParse(text.text, out float value) ? value : 0F, target, duration, value => text.text = value.ToString(format));
            public static Value<float> TweenNumber(this TMP_Text text, float target, float duration, string format) => Value(text, () => float.TryParse(text.text, out float value) ? value : 0F, target, duration, value => text.text = value.ToString(format));


            // C A M E R A
            public static Value<float> TweenFOV(this Camera camera, float target, float duration) => Value(camera, () => camera.fieldOfView, target, duration, value => camera.fieldOfView = value);
            public static Value<float> TweenOrthographicSize(this Camera camera, float target, float duration)
            {
                  if (camera.orthographic)
                  {
                        return Value(camera, () => camera.orthographicSize, target, duration, value => camera.orthographicSize = value);
                  }
                  else
                  {
                        Log.RejectTween($"Cannot tween non-orthographic camera '{camera.name}'.");
                        return Element.Value<float>.Empty;
                  }
            }


            // A U D I O
            public static Value<float> TweenVolume(this AudioSource source, float target, float duration) => Value(source, () => source.volume, target, duration, value => source.volume = value);
            public static Value<float> TweenPitch(this AudioSource source, float target, float duration) => Value(source, () => source.pitch, target, duration, value => source.pitch = value);
      }
}