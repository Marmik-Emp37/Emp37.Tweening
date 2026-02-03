using UnityEngine;
using UnityEngine.UI;

namespace Emp37.Tweening
{
      using static Tweens;

      public static class ComponentExtensions
      {
            // L I G H T
            public static Value<Color> TweenColour(this Light light, Color target, float duration) => Value(() => light.color, () => target, duration, value => light.color = value);
            public static Value<float> TweenRange(this Light light, float target, float duration, bool relative = false) => Value(() => light.range, () => relative ? light.range + target : target, duration, value => light.range = value);
            public static Value<float> TweenIntensity(this Light light, float target, float duration, bool relative = false) => Value(() => light.intensity, () => relative ? light.intensity + target : target, duration, value => light.intensity = value);


            // C A M E R A
            public static Value<float> TweenFOV(this Camera camera, float target, float duration, bool relative = false) => Value(() => camera.fieldOfView, () => relative ? camera.fieldOfView + target : target, duration, value => camera.fieldOfView = value);
            public static Value<float> TweenOrthographicSize(this Camera camera, float target, float duration, bool relative = false)
            {
                  if (camera.orthographic)
                  {
                        return Value(() => camera.orthographicSize, () => relative ? camera.orthographicSize + target : target, duration, value => camera.orthographicSize = value);
                  }
                  else
                  {
                        Log.Error($"Cannot tween '{nameof(camera.orthographicSize)}' on a non-orthographic camera", camera);
                        return Value<float>.Blank;
                  }
            }


            // A U D I O
            public static Value<float> TweenVolume(this AudioSource source, float target, float duration, bool relative = false) => Value(() => source.volume, () => Mathf.Clamp01(relative ? source.volume + target : target), duration, value => source.volume = value);
            public static Value<float> TweenPitch(this AudioSource source, float target, float duration, bool relative = false) => Value(() => source.pitch, () => relative ? source.pitch + target : target, duration, value => source.pitch = value);


            // R E N D E R E R
            public static Value<float> TweenFade(this SpriteRenderer renderer, float target, float duration) => Value(() => renderer.color.a, () => target, duration, value => { var color = renderer.color; color.a = value; renderer.color = color; });
            public static Value<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration) => Value(() => renderer.color, () => target, duration, value => renderer.color = value);
      }
}