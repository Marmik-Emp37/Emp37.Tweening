using UnityEngine;

namespace Emp37.Tweening
{
      public sealed class Blank<T> : Value<T> where T : struct
      {
		internal sealed override bool IsEmpty => true;
		internal sealed override bool CanMoveBack => false;
		internal sealed override bool CanMoveForward => false;

            protected sealed override void OnInitialize() { }
            protected sealed override void RestoreToDefault() { }
            protected sealed override void OnPause() { }
            protected sealed override void OnResume() { }
            protected sealed override void OnReset(bool _) { }
            protected sealed override void OnLoop(LoopType _, bool __) { }
            protected sealed override void Clear() { }
            protected sealed override void OnRecycle() { }

            public sealed override Value<T> SetModifier(Modifier _) => this;
            public sealed override Value<T> SetEase(Ease.Type _) => this;
            public sealed override Value<T> SetEase(AnimationCurve _) => this;
            public sealed override Value<T> SetEase(Ease.Method _) => this;
            public sealed override Value<T> SetTarget(T _, bool __) => this;
      }
}