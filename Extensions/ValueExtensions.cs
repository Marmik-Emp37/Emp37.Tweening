using System.Runtime.CompilerServices;

using UnityEngine;

namespace Emp37.Tweening
{
      public static class ValueExtensions
      {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static float Step(float value, float step) => step <= 0F ? value : Mathf.Round(value / step) * step;

            public static Value<float> Snap(this Value<float> tween, float step) => tween.SetModifier(value => Step(value, step));
            public static Value<Vector2> Snap(this Value<Vector2> tween, Vector2 step) => tween.SetModifier(value => new(Step(value.x, step.x), Step(value.y, step.y)));
            public static Value<Vector3> Snap(this Value<Vector3> tween, Vector3 step) => tween.SetModifier(value => new(Step(value.x, step.x), Step(value.y, step.y), Step(value.z, step.z)));
            public static Value<Vector4> Snap(this Value<Vector4> tween, Vector4 step) => tween.SetModifier(value => new(Step(value.x, step.x), Step(value.y, step.y), Step(value.z, step.z), Step(value.w, step.w)));
            public static Value<Color> Snap(this Value<Color> tween, Color step) => tween.SetModifier(value => new(Step(value.r, step.r), Step(value.g, step.g), Step(value.b, step.b), Step(value.a, step.a)));
      }
}