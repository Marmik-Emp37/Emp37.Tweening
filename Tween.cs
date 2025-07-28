using System;

using UnityEngine;

namespace Emp37.Tweening
{
      using static Ease;

      public class Tween<T> : IElement where T : struct
      {
            public sealed class Blank : Tween<T>
            {
                  internal Blank() => Phase = Phase.None;
                  public override Tween<T> SetEase(Type type) => this;
                  public override Tween<T> SetEase(AnimationCurve curve) => this;
                  public override Tween<T> SetDelay(float duration) => this;
                  public override Tween<T> SetTimeMode(Delta value) => this;
                  public override Tween<T> SetOnStart(Action action) => this;
                  public override Tween<T> SetOnComplete(Action action) => this;
                  public override Tween<T> SetOnUpdate(Action<float> action) => this;
                  public override string ToString() => $"{nameof(Tween<T>)}<{typeof(T).Name}>.Null";
            }

            public static readonly Blank Empty = new();

            public delegate T Evaluator(T a, T b, float ratio);

            private T a;
            private readonly T b;
            private Delta timeMode;
            private bool bootstraped;

            private float delay, progress;
            private readonly float inverseDuration;
            private readonly Action initTween;
            private readonly Evaluator evaluator;
            private Function easingFunction;
            private readonly Action<T> updateTween;

            private Action onStart;
            private Action<float> onUpdate;
            private Action onComplete;

            public bool IsEmpty => ReferenceEquals(this, Empty);
            public Phase Phase { get; private set; }


            private protected Tween() { }
            internal Tween(Func<T> initialize, T target, float duration, Action<T> apply, Evaluator evaluator)
            {
                  Phase = Phase.Active;

                  b = target;
                  inverseDuration = 1F / duration;
                  this.evaluator = evaluator;

                  initTween = () => a = initialize();
                  updateTween = apply;

                  onUpdate = _ => { };
                  easingFunction = Linear;
            }

            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;
                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;
                  if (delay > 0F)
                  {
                        delay -= deltaTime;
                        return;
                  }
                  if (!bootstraped)
                  {
                        bootstraped = true;

                        initTween();
                        onStart?.Invoke();
                  }
                  progress = Mathf.Clamp01(progress + deltaTime * inverseDuration);
                  float eased = easingFunction(progress);
                  T current = evaluator(a, b, eased);
                  updateTween(current);
                  onUpdate(eased);

                  if (progress >= 1F)
                  {
                        Phase = Phase.Complete;
                        onComplete?.Invoke();
                  }
            }
            public void Kill() => Phase = Phase.None;
            public void Pause()
            {
                  if (Phase == Phase.Active) Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase == Phase.Paused) Phase = Phase.Active;
            }

            #region B U I L D E R
            public virtual Tween<T> SetEase(Type type)
            {
                  easingFunction = type switch
                  {
                        Type.InSine => InSine,
                        Type.OutSine => OutSine,
                        Type.InOutSine => InOutSine,
                        Type.InCubic => InCubic,
                        Type.OutCubic => OutCubic,
                        Type.InOutCubic => InOutCubic,
                        Type.InQuint => InQuint,
                        Type.OutQuint => OutQuint,
                        Type.InOutQuint => InOutQuint,
                        Type.InCirc => InCirc,
                        Type.OutCirc => OutCirc,
                        Type.InOutCirc => InOutCirc,
                        Type.InQuad => InQuad,
                        Type.OutQuad => OutQuad,
                        Type.InOutQuad => InOutQuad,
                        Type.InQuart => InQuart,
                        Type.OutQuart => OutQuart,
                        Type.InOutQuart => InOutQuart,
                        Type.InExpo => InExpo,
                        Type.OutExpo => OutExpo,
                        Type.InOutExpo => InOutExpo,
                        Type.InBack => InBack,
                        Type.OutBack => OutBack,
                        Type.InOutBack => InOutBack,
                        Type.InElastic => InElastic,
                        Type.OutElastic => OutElastic,
                        Type.InOutElastic => InOutElastic,
                        Type.InBounce => InBounce,
                        Type.OutBounce => OutBounce,
                        Type.InOutBounce => InOutBounce,
                        _ => Linear
                  };
                  return this;
            }
            /// <summary>
            /// Sets a custom easing to this tween using an <see cref="AnimationCurve"/>. The curve maps normalized progress (0 to 1) to interpolation output.
            /// </summary>
            /// <param name="curve">The curve used to control easing over time.</param>
            /// <remarks>
            /// For predefined animation curves, see <see cref="Curves"/>.
            /// </remarks>
            public virtual Tween<T> SetEase(AnimationCurve curve) { easingFunction = curve.Evaluate; return this; }
            /// <summary>
            /// Sets the delay before the tween starts.
            /// </summary>
            /// <param name="duration">In seconds.</param>
            public virtual Tween<T> SetDelay(float duration) { delay = duration; return this; }
            /// <summary>
            /// Sets the time scale mode (scaled or unscaled) used by the tween.
            /// </summary>
            public virtual Tween<T> SetTimeMode(Delta value) { timeMode = value; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween starts.
            /// </summary>
            public virtual Tween<T> SetOnStart(Action action) { onStart = action; return this; }
            /// <summary>
            /// Sets a callback to invoke during each tween update.
            /// </summary>
            public virtual Tween<T> SetOnUpdate(Action<float> action) { onUpdate = action; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween completes.
            /// </summary>
            public virtual Tween<T> SetOnComplete(Action action) { onComplete = action; return this; }
            #endregion
      }
}