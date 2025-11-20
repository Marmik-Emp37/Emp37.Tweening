using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Emp37.Tweening
{
      using static Tween;

      public static class Extensions
      {
            // P O S I T I O N
            public static Value<Vector3> TweenMove(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.position, () => relative ? transform.position + target : target, duration, value => transform.position = value);
            public static Value<Vector2> TweenMove(this Transform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => (Vector2) transform.position, () => relative ? (Vector2) transform.position + target : target, duration, value => transform.position = value);
            public static Value<float> TweenMoveX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.position.x, () => relative ? transform.position.x + target : target, duration, value => { var pos = transform.position; pos.x = value; transform.position = pos; });
            public static Value<float> TweenMoveY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.position.y, () => relative ? transform.position.y + target : target, duration, value => { var pos = transform.position; pos.y = value; transform.position = pos; });
            public static Value<float> TweenMoveZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.position.z, () => relative ? transform.position.z + target : target, duration, value => { var pos = transform.position; pos.z = value; transform.position = pos; });


            // L O C A L   P O S I T I O N
            public static Value<Vector3> TweenMoveLocal(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.localPosition, () => relative ? transform.localPosition + target : target, duration, value => transform.localPosition = value);
            public static Value<Vector2> TweenMoveLocal(this Transform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => (Vector2) transform.localPosition, () => relative ? (Vector2) transform.localPosition + target : target, duration, value => transform.localPosition = value);
            public static Value<float> TweenMoveLocalX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localPosition.x, () => relative ? transform.localPosition.x + target : target, duration, value => { var pos = transform.localPosition; pos.x = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localPosition.y, () => relative ? transform.localPosition.y + target : target, duration, value => { var pos = transform.localPosition; pos.y = value; transform.localPosition = pos; });
            public static Value<float> TweenMoveLocalZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localPosition.z, () => relative ? transform.localPosition.z + target : target, duration, value => { var pos = transform.localPosition; pos.z = value; transform.localPosition = pos; });


            // R O T A T I O N
            public static Value<Quaternion> TweenRotate(this Transform transform, Quaternion target, float duration) => Value(transform, () => transform.rotation, target, duration, value => transform.rotation = value);
            public static Value<Quaternion> TweenRotate(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.rotation, () => Quaternion.Euler(target), duration, value => transform.rotation = value);
            public static Value<float> TweenRotateX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.eulerAngles.x, () => transform.eulerAngles.x + (relative ? target : Mathf.DeltaAngle(transform.eulerAngles.x, target)), duration, value => { var euler = transform.eulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.eulerAngles.y, () => transform.eulerAngles.y + (relative ? target : Mathf.DeltaAngle(transform.eulerAngles.y, target)), duration, value => { var euler = transform.eulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.eulerAngles.z, () => transform.eulerAngles.z + (relative ? target : Mathf.DeltaAngle(transform.eulerAngles.z, target)), duration, value => { var euler = transform.eulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); });


            // L O C A L   R O T A T I O N
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Quaternion target, float duration) => Value(transform, () => transform.localRotation, target, duration, value => transform.localRotation = value);
            public static Value<Quaternion> TweenRotateLocal(this Transform transform, Vector3 target, float duration) => Value(transform, () => transform.localRotation, () => Quaternion.Euler(target), duration, value => transform.localRotation = value);
            public static Value<float> TweenRotateLocalX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localEulerAngles.x, () => transform.localEulerAngles.x + (relative ? target : Mathf.DeltaAngle(transform.localEulerAngles.x, target)), duration, value => { var euler = transform.localEulerAngles; euler.x = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localEulerAngles.y, () => transform.localEulerAngles.y + (relative ? target : Mathf.DeltaAngle(transform.localEulerAngles.y, target)), duration, value => { var euler = transform.localEulerAngles; euler.y = value; transform.rotation = Quaternion.Euler(euler); });
            public static Value<float> TweenRotateLocalZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localEulerAngles.z, () => transform.localEulerAngles.z + (relative ? target : Mathf.DeltaAngle(transform.localEulerAngles.z, target)), duration, value => { var euler = transform.localEulerAngles; euler.z = value; transform.rotation = Quaternion.Euler(euler); });


            // S C A L E
            public static Value<Vector3> TweenScale(this Transform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.localScale, () => relative ? transform.localScale + target : target, duration, value => transform.localScale = value);
            public static Value<float> TweenScaleX(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localScale.x, () => relative ? transform.localScale.x + target : target, duration, value => { var scale = transform.localScale; scale.x = value; transform.localScale = scale; });
            public static Value<float> TweenScaleY(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localScale.y, () => relative ? transform.localScale.y + target : target, duration, value => { var scale = transform.localScale; scale.y = value; transform.localScale = scale; });
            public static Value<float> TweenScaleZ(this Transform transform, float target, float duration, bool relative = false) => Value(transform, () => transform.localScale.z, () => relative ? transform.localScale.z + target : target, duration, value => { var scale = transform.localScale; scale.z = value; transform.localScale = scale; });


            // R E N D E R E R
            public static Value<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration) => Value(renderer, () => renderer.color, target, duration, value => renderer.color = value);
            public static Value<float> TweenMaterialPropertyBlock(this Renderer renderer, string property, float target, float duration)
            {
                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain float property '{property}'.");
                        return Value<float>.Empty;
                  }
                  MaterialPropertyBlock block = new();
                  int id = Shader.PropertyToID(property);
                  return Value(renderer,
                        () =>
                        {
                              renderer.GetPropertyBlock(block);
                              return block.GetFloat(id);
                        },
                        target, duration,
                        value =>
                        {
                              block.SetFloat(id, value);
                              renderer.SetPropertyBlock(block);
                        });
            }
            public static Value<Color> TweenMaterialPropertyBlock(this Renderer renderer, string property, Color target, float duration)
            {
                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain float property '{property}'.");
                        return Value<Color>.Empty;
                  }
                  MaterialPropertyBlock block = new();
                  int id = Shader.PropertyToID(property);
                  return Value(renderer,
                        () =>
                        {
                              renderer.GetPropertyBlock(block);
                              return block.GetColor(id);
                        },
                        target, duration,
                        value =>
                        {
                              block.SetColor(id, value);
                              renderer.SetPropertyBlock(block);
                        });
            }
            public static Value<Vector4> TweenMaterialPropertyBlock(this Renderer renderer, string property, Vector4 target, float duration)
            {
                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain float property '{property}'.");
                        return Value<Vector4>.Empty;
                  }
                  MaterialPropertyBlock block = new();
                  int id = Shader.PropertyToID(property);
                  return Value(renderer,
                        () =>
                        {
                              renderer.GetPropertyBlock(block);
                              return block.GetVector(id);
                        },
                        target, duration,
                        value =>
                        {
                              block.SetVector(id, value);
                              renderer.SetPropertyBlock(block);
                        });
            }


            // L I G H T
            public static Value<float> TweenIntensity(this Light light, float target, float duration, bool relative = false) => Value(light, () => light.intensity, () => relative ? light.intensity + target : target, duration, value => light.intensity = value);
            public static Value<Color> TweenColor(this Light light, Color target, float duration) => Value(light, () => light.color, target, duration, value => light.color = value);
            public static Value<float> TweenRange(this Light light, float target, float duration) => Value(light, () => light.range, target, duration, value => light.range = value);


            // M A T E R I A L
            public static Value<Color> TweenColor(this Material material, Color target, float duration) => Value(material, () => material.color, target, duration, value => material.color = value);
            public static Value<float> TweenProperty(this Material material, string property, float target, float duration)
            {
                  if (!material.HasProperty(property))
                  {
                        Log.RejectTween($"Material '{material.name}' does not contain float property '{property}'.");
                        return Value<float>.Empty;
                  }
                  int id = Shader.PropertyToID(property);
                  return Value(material, () => material.GetFloat(id), target, duration, value => material.SetFloat(id, value));
            }
            public static Value<Color> TweenProperty(this Material material, string property, Color target, float duration)
            {
                  if (!material.HasProperty(property))
                  {
                        Log.RejectTween($"Material '{material.name}' does not contain Color property '{property}'.");
                        return Value<Color>.Empty;
                  }
                  int id = Shader.PropertyToID(property);
                  return Value(material, () => material.GetColor(id), target, duration, value => material.SetColor(id, value));
            }
            public static Value<Vector4> TweenProperty(this Material material, string property, Vector4 target, float duration)
            {
                  if (!material.HasProperty(property))
                  {
                        Log.RejectTween($"Material '{material.name}' does not contain Vector property '{property}'.");
                        return Value<Vector4>.Empty;
                  }
                  int id = Shader.PropertyToID(property);
                  return Value(material, () => material.GetVector(id), target, duration, value => material.SetVector(id, value));
            }


            // U I
            public static Value<Vector2> TweenMove(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => transform.anchoredPosition, () => relative ? transform.anchoredPosition + target : target, duration, value => transform.anchoredPosition = value);
            public static Value<Vector3> TweenMove(this RectTransform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.anchoredPosition3D, () => relative ? transform.anchoredPosition3D + target : target, duration, value => transform.anchoredPosition3D = value);
            public static Value<Vector2> TweenSize(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => transform.sizeDelta, () => relative ? transform.sizeDelta + target : target, duration, value => transform.sizeDelta = value);
            public static Value<float> TweenFade(this CanvasGroup group, float target, float duration) => Value(group, () => group.alpha, target, duration, value => group.alpha = value);


            // U I - G R A P H I C S
            public static Value<float> TweenAlpha(this Graphic graphic, float target, float duration) => Value(graphic, () => graphic.color.a, target, duration, value => { var color = graphic.color; color.a = value; graphic.color = color; });
            public static Value<Color> TweenColor(this Graphic graphic, Color target, float duration) => Value(graphic, () => graphic.color, target, duration, value => graphic.color = value);
            public static Value<float> TweenFill(this Image image, float target, float duration) => Value(image, () => image.fillAmount, target, duration, value => image.fillAmount = value);
            public static Value<float> TweenText(this Text text, string target, float duration)
            {
                  if (string.IsNullOrEmpty(target))
                  {
                        Log.RejectTween("Cannot tween to null or empty text.");
                        return Value<float>.Empty;
                  }
                  return Value(text, () => 0, target.Length, duration, value => { int count = Mathf.Clamp(Mathf.FloorToInt(value), 0, target.Length); text.text = count == 0 ? string.Empty : target[..count]; });
            }
            public static Value<float> TweenText(this TMP_Text text, string target, float duration)
            {
                  if (string.IsNullOrEmpty(target))
                  {
                        Log.RejectTween("Cannot tween to null or empty text.");
                        return Value<float>.Empty;
                  }
                  return Value(text, () => 0, target.Length, duration, value => { int count = Mathf.Clamp(Mathf.FloorToInt(value), 0, target.Length); text.text = count == 0 ? string.Empty : target[..count]; });
            }
            public static Value<float> TweenNumber(this Text text, float target, float duration, string format, bool relative = false) => Value(text, () => float.TryParse(text.text, out float value) ? value : 0F, () => relative ? (float.TryParse(text.text, out float value) ? value : 0F) + target : target, duration, value => text.text = value.ToString(format));
            public static Value<float> TweenNumber(this TMP_Text text, float target, float duration, string format, bool relative = false) => Value(text, () => float.TryParse(text.text, out float value) ? value : 0F, () => relative ? (float.TryParse(text.text, out float value) ? value : 0F) + target : target, duration, value => text.text = value.ToString(format));


            // C A M E R A
            public static Value<float> TweenFOV(this Camera camera, float target, float duration, bool relative = false) => Value(camera, () => camera.fieldOfView, () => relative ? camera.fieldOfView + target : target, duration, value => camera.fieldOfView = value);
            public static Value<float> TweenOrthographicSize(this Camera camera, float target, float duration, bool relative = false)
            {
                  if (camera.orthographic)
                  {
                        return Value(camera, () => camera.orthographicSize, () => relative ? camera.orthographicSize + target : target, duration, value => camera.orthographicSize = value);
                  }
                  else
                  {
                        Log.RejectTween($"Cannot tween non-orthographic camera '{camera.name}'.");
                        return Value<float>.Empty;
                  }
            }


            // A U D I O
            public static Value<float> TweenVolume(this AudioSource source, float target, float duration, bool relative = false) => Value(source, () => source.volume, () => relative ? source.volume + target : target, duration, value => source.volume = value);
            public static Value<float> TweenPitch(this AudioSource source, float target, float duration, bool relative = false) => Value(source, () => source.pitch, () => relative ? source.pitch + target : target, duration, value => source.pitch = value);
      }
}