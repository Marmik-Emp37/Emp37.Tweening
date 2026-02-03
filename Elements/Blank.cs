using System;
using UnityEngine;

namespace Emp37.Tweening
{
      public sealed class Blank<T> : Value<T> where T : struct
      {
            public override bool IsEmpty => true;

            protected override void OnPause() { }
            protected override void OnResume() { }
            protected override void Clear() { }

            public override Value<T> SetModifier(Func<T, T> _) => this;
            public override Value<T> SetEase(Ease.Type _) => this;
            public override Value<T> SetEase(AnimationCurve curve) => this;
            public override Value<T> SetEase(Ease.Method _) => this;
            public override Value<T> SetTarget(T _, bool __) => this;
      }
}