using System;

using UnityEngine;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening.Element
{
      using static Ease;

      /// <summary>
      /// A tween element that interpolates between two values of type T over a specified duration. Supports easing, looping, delays and lifecycle callbacks.
      /// </summary>
      /// <typeparam name="T">The value type being interpolated (must be a struct)</typeparam>
      public partial class Value<T> : IElement where T : struct
      {
            /// <summary>
            /// Function that returns the interpolated value between start and target.
            /// </summary>
            public delegate T Evaluator(T a, T b, float ratio);

            private T a, b;
            private Delta timeMode;
            private float progress;
            private readonly float inverseDuration;

            private Loop loop;
            private int loopCounter;

            private readonly Action initTween;
            private readonly Evaluator evaluate;
            private Function easingFunction;
            private readonly Action<T> updateTween;

            private readonly UObject linkedTarget; // auto-kill tween if this object is destroyed

            private Action onStart;
            private Action<float> onUpdate;
            private Action onComplete;

            public string Tag { get; set; }
            public Phase Phase { get; private set; }

            private Value(UObject link, float duration, Action<T> update, Evaluator evaluator)
            {
                  linkedTarget = link;
                  inverseDuration = 1F / duration;
                  updateTween = update;
                  evaluate = evaluator;

                  easingFunction = Linear;
            }
            internal Value(UObject link, Func<T> init, T target, float duration, Action<T> update, Evaluator evaluator) : this(link, duration, update, evaluator)
            {
                  b = target;
                  initTween = () => a = init();
            }
            internal Value(UObject link, Func<T> init, Func<T> dynamicTarget, float duration, Action<T> update, Evaluator evaluator) : this(link, duration, update, evaluator)
            {
                  initTween = () =>
                  {
                        a = init();
                        b = dynamicTarget();
                  };
            }

            void IElement.Init()
            {
                  if (linkedTarget == null) return;

                  initTween();
                  Utils.SafeInvoke(onStart);
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (linkedTarget == null) { Kill(); return; }

                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;
                  progress = Mathf.Min(progress + deltaTime * inverseDuration, 1F);

                  if (progress < 0F) return; // processing delay

                  float eased = easingFunction(progress);
                  T value = evaluate(a, b, eased);
                  updateTween(value);
                  Utils.SafeInvoke(onUpdate, eased);

                  if (progress < 1F) return;

                  if (loop.Mode != Loop.Type.None && loopCounter != 0)
                  {
                        if (loopCounter > 0) loopCounter--; // decrement if finite
                        if (loop.Mode is Loop.Type.Yoyo) (a, b) = (b, a);

                        progress = loop.Delay > 0F ? -loop.Delay : 0F;
                  }
                  else
                  {
                        Phase = Phase.Complete;
                        Utils.SafeInvoke(onComplete);
                  }
            }

            public virtual void Pause()
            {
                  if (Phase == Phase.Active) Phase = Phase.Paused;
            }
            public virtual void Resume()
            {
                  if (Phase == Phase.Paused) Phase = Phase.Active;
            }
            public virtual void Kill() => Phase = Phase.None;

            /// <summary>
            /// Stops looping and allows the current cycle to complete naturally.
            /// </summary>
            public virtual void TerminateLoop() => SetLoop(Loop.Default);

            public override string ToString() => $"{nameof(Value<T>)}<{typeof(T).Name}> (Phase: {Phase}, Progress: {progress:P0})";


            #region F L U E N T
            public virtual Value<T> SetTag(string tag) { Tag = tag; return this; }
            /// <summary>
            /// Configures this tween to automatically play forward, then reverse once using Yoyo loop.
            /// </summary>
            /// <param name="delay">In seconds.</param>
            public virtual Value<T> SetReturnOnce(float delay = 0F) => SetLoop(new(Loop.Type.Yoyo, 1, delay));
            /// <summary>
            /// Sets the easing function using a predefined easing type.
            /// </summary>
            public virtual Value<T> SetEase(Type type) { easingFunction = TypeMap[type]; return this; }
            /// <summary>
            /// Sets a custom easing function using an <see cref="AnimationCurve"/>.
            /// </summary>
            /// <param name="curve">The curve that maps normalized progress (0�1) to interpolation output. Values may extend outside the [0,1] range depending on the curve.</param>
            public virtual Value<T> SetEase(AnimationCurve curve) { easingFunction = curve.Evaluate; return this; }
            /// <summary>
            /// Configures this tween to repeat according to a specified loop strategy.
            /// </summary>
            public virtual Value<T> SetLoop(in Loop config) { loop = config; loopCounter = loop.Cycles; return this; }
            /// <summary>
            /// Sets a new target value for this tween. The current progress will interpolate toward this new target.
            /// </summary>
            public virtual Value<T> SetTarget(T value) { b = value; return this; }
            /// <summary>
            /// Sets the time scale mode used by this tween.
            /// </summary>
            public virtual Value<T> SetTimeMode(Delta type) { timeMode = type; return this; }
            /// <summary>
            /// Registers a callback to invoke once when the tween starts playing.
            /// </summary>
            public virtual Value<T> OnStart(Action action) { onStart = action; return this; }
            /// <summary>
            /// Registers a callback to invoke during each update of the tween.
            /// </summary>
            /// <param name="action">The callback that receives the eased progress value (0�1) each frame.</param>
            public virtual Value<T> OnUpdate(Action<float> action) { onUpdate = action; return this; }
            /// <summary>
            /// Registers a callback to invoke once when the tween completes.
            /// </summary>
            public virtual Value<T> OnComplete(Action action) { onComplete = action; return this; }
            #endregion
      }
}