using UnityEngine;

namespace Emp37.Tweening
{
      using static Tweens;

      public static class ComponentExtensions
      {
            // L I G H T
            public static Value<Color> TweenColour(this Light light, Color target, float duration) => Value(() => light.color, target, value => light.color = value, duration);
            public static Value<float> TweenRange(this Light light, float target, float duration, bool relative = false) => Value(() => light.range, target, value => light.range = value, duration, relative);
            public static Value<float> TweenIntensity(this Light light, float target, float duration, bool relative = false) => Value(() => light.intensity, target, value => light.intensity = value, duration, relative);


            // C A M E R A
            public static Value<float> TweenFOV(this Camera camera, float target, float duration, bool relative = false) => Value(() => camera.fieldOfView, target, value => camera.fieldOfView = value, duration, relative);
            public static Value<float> TweenOrthographicSize(this Camera camera, float target, float duration, bool relative = false)
            {
                  if (camera.orthographic) return Value(() => camera.orthographicSize, target, value => camera.orthographicSize = value, duration, relative);
                  else
                  {
                        Log.Error($"Cannot tween '{nameof(camera.orthographicSize)}' on a non-orthographic camera", camera);
                        return Value<float>.Blank;
                  }
            }


            // A U D I O
            public static Value<float> TweenVolume(this AudioSource source, float target, float duration, bool relative = false) => Value(() => source.volume, target, value => source.volume = value, duration, relative);
            public static Value<float> TweenPitch(this AudioSource source, float target, float duration, bool relative = false) => Value(() => source.pitch, target, value => source.pitch = value, duration, relative);


            // R E N D E R E R
            public static Value<float> TweenFade(this SpriteRenderer renderer, float target, float duration) => Value(() => renderer.color.a, target, value => { var color = renderer.color; color.a = value; renderer.color = color; }, duration);
            public static Value<Color> TweenColor(this SpriteRenderer renderer, Color target, float duration) => Value(() => renderer.color, target, value => renderer.color = value, duration);
      }
}