using System;

using UnityEngine;

namespace Emp37.Tweening.Element
{
      public partial class Value<T> where T : struct
      {
            private Value() { }

            public static readonly Value<T> Empty = new Blank() { Phase = Phase.None };
            public bool IsEmpty => ReferenceEquals(this, Empty);

            private sealed class Blank : Value<T>
            {
                  public override Value<T> SetReturnOnce(float _) => this;
                  public override Value<T> SetEase(Ease.Type _) => this;
                  public override Value<T> SetEase(AnimationCurve _) => this;
                  public override Value<T> SetDelay(float _) => this;
                  public override Value<T> SetLoop(in Loop _) => this;
                  public override Value<T> SetTarget(T _) => this;
                  public override Value<T> SetTimeMode(Delta _) => this;
                  public override Value<T> OnStart(Action _) => this;
                  public override Value<T> OnComplete(Action _) => this;
                  public override Value<T> OnUpdate(Action<float> _) => this;

                  public override void Pause() { }
                  public override void Resume() { }
                  public override void Kill() { }
                  public override void TerminateLoop() { }

                  public override string ToString() => $"{nameof(Value<T>)}<{typeof(T).Name}>.{nameof(Blank)}";
            }
      }
}