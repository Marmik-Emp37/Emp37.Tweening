using System;
using System.Text.RegularExpressions;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening.Element
{
      using static Ease;
      using static PlasticGui.WorkspaceWindow.CodeReview.ReviewChanges.Summary.CommentSummaryData;

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
            private bool bootstrapped;
            private Loop loop;

            private float delay, progress;
            private readonly float inverseDuration;
            private readonly Action initTween;
            private readonly Evaluator evaluator;
            private Function easingFunction;
            private readonly Action<T> updateTween;
            private readonly bool isLinked;
            private readonly UObject linkedTarget; // auto-kill tween if this object is destroyed

            private Action onStart;
            private Action<float> onUpdate;
            private Action onComplete;

            public Phase Phase { get; private set; }
            public bool IsDestroyed => isLinked && linkedTarget == null;

            internal Value(Func<T> init, T target, float duration, Action<T> update, Evaluator evaluator, UObject link = null)
            {
                  b = target;
                  inverseDuration = 1F / duration;
                  this.evaluator = evaluator;

                  // capture initial value at the moment the tween begins
                  initTween = () => a = init();
                  updateTween = update;

                  if (link != null)
                  {
                        linkedTarget = link;
                        isLinked = true;
                  }

                  easingFunction = Linear;
            }

            void IElement.Init()
            {
                  if (!IsDestroyed) Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (IsDestroyed)
                  {
                        Kill();
                        return;
                  }

                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;

                  if (delay > 0F)
                  {
                        delay = Mathf.Max(delay - deltaTime, 0F);
                        if (delay != 0F) return;
                  }

                  if (!bootstrapped)
                  {
                        bootstrapped = true;

                        initTween();
                        Utils.SafeInvoke(onStart);
                  }

                  progress = Mathf.Min(progress + deltaTime * inverseDuration, 1F);
                  float eased = easingFunction(progress);
                  T value = evaluator(a, b, eased);
                  updateTween(value);
                  Utils.SafeInvoke(onUpdate, eased);

                  if (progress < 1F) return;

                  if (loop.Mode == 0 || loop.Count == 0)
                  {
                        Phase = Phase.Complete;
                        Utils.SafeInvoke(onComplete);
                        return;
                  }

                  // decrement if finite
                  if (loop.Count > 0) loop.Count--;

                  if (loop.Mode == Loop.Type.Yoyo) (a, b) = (b, a);

                  progress = 0F;
                  delay = loop.Delay;
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
            /// <param name="curve">The curve that maps normalized progress (0–1) to interpolation output. Values may extend outside the [0,1] range depending on the curve.</param>
            public virtual Value<T> SetEase(AnimationCurve curve) { easingFunction = curve.Evaluate; return this; }
            /// <summary>
            /// Sets a delay before the tween begins playing.
            /// </summary>
            /// <param name="value">In seconds.</param>
            public virtual Value<T> SetDelay(float value) { delay = value; return this; }
            /// <summary>
            /// Configures this tween to repeat according to a specified loop strategy.
            /// </summary>
            public virtual Value<T> SetLoop(in Loop config) { loop = config; return this; }
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
            /// <param name="action">The callback that receives the eased progress value (0–1) each frame.</param>
            public virtual Value<T> OnUpdate(Action<float> action) { onUpdate = action; return this; }
            /// <summary>
            /// Registers a callback to invoke once when the tween completes.
            /// </summary>
            public virtual Value<T> OnComplete(Action action) { onComplete = action; return this; }
            #endregion
      }
}