using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public sealed class Blank<T> : Tween<T> where T : struct
      {
            public override Tween<T> SetAutoReturn(float _ = 0) => this;
            public override Tween<T> SetEase(Ease.Type _) => this;
            public override Tween<T> SetEase(AnimationCurve _) => this;
            public override Tween<T> SetDelay(float _) => this;
            public override Tween<T> SetLoop(in Tween.LoopParams _) => this;
            public override Tween<T> SetTarget(T _) => this;
            public override Tween<T> SetTimeMode(Delta _) => this;
            public override Tween<T> OnStart(Action _) => this;
            public override Tween<T> OnComplete(Action _) => this;
            public override Tween<T> OnUpdate(Action<float> _) => this;

            public override void Pause() { }
            public override void Resume() { }
            public override void Kill() { }
            public override void TerminateLoop() { }

            public override string ToString() => $"{nameof(Tween<T>)}<{typeof(T).Name}>.{nameof(Blank<T>)}";
      }
}