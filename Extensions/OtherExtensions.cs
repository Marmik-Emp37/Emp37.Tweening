using UnityEngine;

namespace Emp37.Tweening
{
      using static Tween;

      public static class OtherExtensions
      {
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
      }
}