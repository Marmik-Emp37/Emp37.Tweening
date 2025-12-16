using UnityEngine;

namespace Emp37.Tweening
{
      using static Tween;

      public static class ComponentExtensions
      {
            // L I G H T
            public static Value<Color> TweenColor(this Light light, Color target, float duration) => Value(light, () => light.color, target, duration, value => light.color = value);
            public static Value<float> TweenRange(this Light light, float target, float duration, bool relative = false) => Value(light, () => light.range, () => relative ? light.range + target : target, duration, value => light.range = value);
            public static Value<float> TweenIntensity(this Light light, float target, float duration, bool relative = false) => Value(light, () => light.intensity, () => relative ? light.intensity + target : target, duration, value => light.intensity = value);


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


            // R E N D E R E R
            public static Value<float> TweenPropertyBlock(this Renderer renderer, string property, float target, float duration, bool relative = false)
            {
                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain float property '{property}'.");
                        return Value<float>.Empty;
                  }

                  MaterialPropertyBlock block = new();
                  int id = Shader.PropertyToID(property);

                  return Value(renderer, init, () => relative ? init() + target : target, duration, value => { block.SetFloat(id, value); renderer.SetPropertyBlock(block); });

                  float init()
                  {
                        renderer.GetPropertyBlock(block);
                        return block.GetFloat(id);
                  }
            }
            public static Value<Color> TweenPropertyBlock(this Renderer renderer, string property, Color target, float duration)
            {
                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain Color property '{property}'.");
                        return Value<Color>.Empty;
                  }

                  MaterialPropertyBlock block = new();
                  int id = Shader.PropertyToID(property);

                  return Value(renderer, init, target, duration, value => { block.SetColor(id, value); renderer.SetPropertyBlock(block); });

                  Color init()
                  {
                        renderer.GetPropertyBlock(block);
                        return block.GetColor(id);
                  }
            }
            public static Value<Vector4> TweenPropertyBlock(this Renderer renderer, string property, Vector4 target, float duration)
            {
                  if (!renderer.sharedMaterial.HasProperty(property))
                  {
                        Log.RejectTween($"Renderer '{renderer.name}' does not contain Vector4 property '{property}'.");
                        return Value<Vector4>.Empty;
                  }

                  MaterialPropertyBlock block = new();
                  int id = Shader.PropertyToID(property);

                  return Value(renderer, init, target, duration, value => { block.SetVector(id, value); renderer.SetPropertyBlock(block); });

                  Vector4 init()
                  {
                        renderer.GetPropertyBlock(block);
                        return block.GetColor(id);
                  }
            }

            public static Value<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration) => Value(renderer, () => renderer.color, target, duration, value => renderer.color = value);
      }
}