using System;

using UnityEngine;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      public class Value<T> : ITween
      {
            public static Value<T> Empty = new Blank();
            private sealed class Blank : Value<T>
            {
                  public override Value<T> setDelay(float _) => this;
                  public override Value<T> setEase(Ease.Type _) => this;
                  public override Value<T> setEase(AnimationCurve _) => this;
                  public override Value<T> setTarget(T _) => this;
                  public override Value<T> setTimeMode(Delta _) => this;

                  public override Value<T> onStart(Action _) => this;
                  public override Value<T> onUpdate(Action<float> _) => this;
                  public override Value<T> onComplete(Action _) => this;
                  public override Value<T> onKill(Action _) => this;
                  public override Value<T> onFinish(Action _) => this;

                  public override void Pause() { }
                  public override void Resume() { }
                  public override void Kill() { }

                  public override string ToString() => $"{nameof(Value<T>)}<{typeof(T).Name}>.{nameof(Blank)}";
            }


            public delegate T Evaluator(T a, T b, float ratio);

            private T a, b;
            private Delta timeMode;
            private readonly float inverseDuration;
            private float progress;
            private bool bootstrapped;

            private readonly Action initTween;
            private readonly Action<T> easeTween;
            private Ease.Function easingMethod;
            private readonly Evaluator evaluator;

            private readonly UObject linkedTarget; // used to auto-kill tween if this object is destroyed

            private LoopType cycleMode;
            private int cyclesRemaining;
            private float cycleDelay;
            private sbyte direction = 1;

            private Action actionOnStart, actionOnComplete, actionOnKill, actionOnFinish;
            private Action<float> actionOnUpdate;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => ReferenceEquals(this, Empty);
            public bool IsDestroyed => linkedTarget == null;


            private Value()
            {
                  easingMethod = Ease.Linear;
            }
            private Value(UObject link, float duration, Action<T> ease, Evaluator evaluator) : this()
            {
                  linkedTarget = link;
                  inverseDuration = 1F / duration;
                  easeTween = ease;
                  this.evaluator = evaluator;
            }
            internal Value(UObject link, Func<T> capture, T target, float duration, Action<T> update, Evaluator evaluator) : this(link, duration, update, evaluator)
            {
                  initTween = () => a = capture();
                  b = target;
            }
            internal Value(UObject link, Func<T> capture, Func<T> dynamicTarget, float duration, Action<T> update, Evaluator evaluator) : this(link, duration, update, evaluator)
            {
                  initTween = () =>
                  {
                        a = capture();
                        b = dynamicTarget();
                  };
            }

            void ITween.Init()
            {
                  if (!IsDestroyed) Phase = Phase.Active;
            }
            void ITween.Update()
            {
                  if (IsDestroyed)
                  {
                        Kill();
                        return;
                  }

                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;
                  progress = Mathf.Min(progress + deltaTime * inverseDuration, 1F);

                  if (progress < 0F) return; // processing delay

                  if (!bootstrapped)
                  {
                        bootstrapped = true;
                        initTween();
                        Utils.SafeInvoke(actionOnStart);
                  }

                  float ratio = direction > 0 ? progress : 1F - progress;
                  float easedRatio = easingMethod(ratio);
                  T value = evaluator(a, b, easedRatio);
                  easeTween(value);
                  Utils.SafeInvoke(actionOnUpdate, easedRatio);

                  if (progress < 1F) return;

                  if (cycleMode != LoopType.None && cyclesRemaining != 0)
                  {
                        if (cyclesRemaining > 0) cyclesRemaining--; // decrement if finite
                        if (cycleMode is LoopType.Yoyo) direction *= -1;

                        progress = -cycleDelay;
                        return;
                  }

                  Utils.SafeInvoke(actionOnComplete);
                  Phase = Phase.Complete;

                  Utils.SafeInvoke(actionOnFinish);
            }

            public virtual void Pause()
            {
                  if (Phase == Phase.Active) Phase = Phase.Paused;
            }
            public virtual void Resume()
            {
                  if (Phase == Phase.Paused) Phase = Phase.Active;
            }
            public virtual void Kill()
            {
                  Utils.SafeInvoke(actionOnKill);
                  Phase = Phase.None;

                  Utils.SafeInvoke(actionOnFinish);
            }
            public virtual void DisableLoop()
            {
                  cycleMode = LoopType.None;
                  cyclesRemaining = 0;
            }

            #region Fluent API
#pragma warning disable IDE1006 // Naming Styles
            public virtual Value<T> setDelay(float seconds) { progress = Mathf.Min(0F, -seconds); return this; }
            public virtual Value<T> setEase(Ease.Type easeType) { easingMethod = Ease.TypeMap[easeType]; return this; }
            public virtual Value<T> setEase(AnimationCurve curve) { easingMethod = curve.Evaluate; return this; }
            public virtual Value<T> setLoop(int cycles, LoopType type, float delay = 0F) { cyclesRemaining = (cycles <= 0) ? 0 : cycles; cycleMode = type; cycleDelay = Mathf.Max(0F, delay); return this; }
            public virtual Value<T> setLoopInfinite(LoopType type, float delay = 0F) => setLoop(-1, type, delay);
            public virtual Value<T> setTarget(T value) { b = value; return this; }
            public virtual Value<T> setTimeMode(Delta mode) { timeMode = mode; return this; }

            public virtual Value<T> onStart(Action action) { actionOnStart = action; return this; }
            public virtual Value<T> onUpdate(Action<float> action) { actionOnUpdate = action; return this; }
            public virtual Value<T> onComplete(Action action) { actionOnComplete = action; return this; }
            public virtual Value<T> onKill(Action action) { actionOnKill = action; return this; }
            public virtual Value<T> onFinish(Action action) { actionOnFinish = action; return this; }
#pragma warning restore IDE1006
            #endregion

            public override string ToString() => this.Summarize($"Progress: {progress:P0} | A: {a} - B: {b} | Duration: {1F / inverseDuration} | Mode: {timeMode}");
      }
}