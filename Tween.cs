using System;

using UnityEngine;

namespace Emp37.Tweening
{
      using static Ease;

      public static class Tween
      {
            #region P A R A M E T E R S
            [Serializable]
            public struct LoopParams
            {
                  public enum Type
                  {
                        None,
                        Repeat,
                        Yoyo,
                  }

                  public Type LoopType;
                  public int Count;
                  public float Interval;
                  public bool CaptureOnce;

                  public static LoopParams Default => new(Type.None, 0);

                  /// <summary>
                  /// Creates a new loop configuration for a tween.
                  /// </summary>
                  /// <param name="type">The loop behavior type.</param>
                  /// <param name="count">The number of times the tween should loop. Set to -1 for infinite loops.</param>
                  /// <param name="interval">Optional delay in seconds between loop cycles.</param>
                  /// <param name="initTweenOnInterval"> If true, the tween does not re-capture the start value after the initial run. Use this to lock the initial state during loops.
                  /// </param>
                  public LoopParams(Type type, int count, float interval = 0F, bool initTweenOnInterval = false)
                  {
                        LoopType = type;
                        Count = count;
                        Interval = interval;
                        CaptureOnce = initTweenOnInterval;
                  }
            }
            #endregion

            public static Tween<T> Create<T>(Func<T> onInitialize, T target, float duration, Action<T> onValueChange, Tween<T>.Evaluator evaluate) where T : struct
            {
                  bool isValid = true;

                  if (duration <= 0F) { warn($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (onInitialize == null) { warn($"Missing required delegate: {nameof(onInitialize)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (onValueChange == null) { warn($"Missing required delegate: {nameof(onValueChange)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }

                  return isValid ? new Tween<T>(onInitialize, target, duration, onValueChange, evaluate) : Tween<T>.Empty;

                  static void warn(string message) => Debug.LogWarning($"{nameof(Tween<T>)} creation failed: {message}");
            }
            public static Tween<float> Create(Func<float> onInitialize, float target, float duration, Action<float> onValueChange) => Create(onInitialize, target, duration, onValueChange, Mathf.LerpUnclamped);
            public static Tween<Vector2> Create(Func<Vector2> onInitialize, Vector2 target, float duration, Action<Vector2> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector2.LerpUnclamped);
            public static Tween<Vector3> Create(Func<Vector3> onInitialize, Vector3 target, float duration, Action<Vector3> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector3.LerpUnclamped);
            public static Tween<Quaternion> Create(Func<Quaternion> onInitialize, Quaternion target, float duration, Action<Quaternion> onValueChange) => Create(onInitialize, target, duration, onValueChange, Quaternion.LerpUnclamped);
            public static Tween<Color> Create(Func<Color> onInitialize, Color target, float duration, Action<Color> onValueChange) => Create(onInitialize, target, duration, onValueChange, Color.LerpUnclamped);
      }
      public sealed class Blank<T> : Tween<T> where T : struct
      {
            public override Tween<T> SetEase(Type _) => this;
            public override Tween<T> SetEase(AnimationCurve _) => this;
            public override Tween<T> SetDelay(float _) => this;
            public override Tween<T> SetTimeMode(Delta _) => this;
            public override Tween<T> SetLoop(in Tween.LoopParams _) => this;
            public override Tween<T> SetAutoReturn(float _ = 0) => this;
            public override Tween<T> SetTarget(T _) => this;
            public override Tween<T> OnStart(Action _) => this;
            public override Tween<T> OnComplete(Action _) => this;
            public override Tween<T> OnUpdate(Action<float> _) => this;
            public override string ToString() => $"{nameof(Tween<T>)}<{typeof(T).Name}>.Null";
      }
      public class Tween<T> : IElement where T : struct
      {
            public static readonly Blank<T> Empty = new() { Phase = Phase.None };

            public delegate T Evaluator(T a, T b, float ratio);

            private T a, b;
            private Delta timeMode;
            private bool bootstrapped;
            private Tween.LoopParams loop;

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
                  b = target;
                  inverseDuration = 1F / duration;
                  this.evaluator = evaluator;

                  initTween = () => a = initialize();
                  updateTween = apply;

                  onUpdate = _ => { };
                  easingFunction = Linear;
            }

            void IElement.Init()
            {
                  Phase = Phase.Active;
            }
            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;
                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;
                  if (delay > 0F)
                  {
                        delay = Mathf.Clamp(delay - deltaTime, 0F, float.MaxValue);
                        if (delay != 0F) return;
                        if (!loop.CaptureOnce && bootstrapped) initTween();
                  }
                  if (!bootstrapped)
                  {
                        bootstrapped = true;

                        initTween();
                        onStart?.Invoke();
                  }
                  progress = Mathf.Min(progress + deltaTime * inverseDuration, 1F);
                  float eased = easingFunction(progress);
                  T current = evaluator(a, b, eased);
                  updateTween(current);
                  onUpdate(eased);

                  if (progress >= 1F)
                  {
                        if (loop.Count == 0 || loop.LoopType == Tween.LoopParams.Type.None)
                        {
                              Phase = Phase.Complete;
                              onComplete?.Invoke();
                        }
                        else
                        {
                              if (loop.Count > 0) loop.Count--;
                              switch (loop.LoopType)
                              {
                                    case Tween.LoopParams.Type.Yoyo:
                                          (a, b) = (b, a);
                                          break;
                              }
                              progress = 0F;
                              delay = loop.Interval;
                        }
                  }
            }

            public void Pause()
            {
                  if (Phase == Phase.Active) Phase = Phase.Paused;
            }
            public void Resume()
            {
                  if (Phase == Phase.Paused) Phase = Phase.Active;
            }
            public void Kill()
            {
                  Phase = Phase.None;
            }
            public void TerminateLoop() => SetLoop(Tween.LoopParams.Default);

            #region B U I L D E R
            public virtual Tween<T> SetAutoReturn(float delay = 0F) => SetLoop(new(Tween.LoopParams.Type.Yoyo, 1, delay));
            /// <summary>
            /// Sets the easing function using a predefined easing type.
            /// </summary>
            public virtual Tween<T> SetEase(Type type)
            {
                  if (Map.TryGetValue(type, out Function function))
                  {
                        easingFunction = function;
                  }
                  return this;
            }
            /// <summary>
            /// Sets a custom easing to this tween using an <see cref="AnimationCurve"/>. The curve maps normalized progress (0 to 1) to interpolation output.
            /// </summary>
            /// <param name="curve">The curve used to control easing over time.</param>
            /// <remarks>
            /// For predefined animation curves, see <see cref="Profiles"/>.
            /// </remarks>
            public virtual Tween<T> SetEase(AnimationCurve curve) { easingFunction = curve.Evaluate; return this; }
            /// <summary>
            /// Sets the delay before the tween starts.
            /// </summary>
            /// <param name="seconds">In seconds.</param>
            public virtual Tween<T> SetDelay(float seconds) { delay = seconds; return this; }
            /// <summary>
            /// Set this tween to repeat based on a custom loop strategy.
            /// </summary>
            public virtual Tween<T> SetLoop(in Tween.LoopParams config) { loop = config; return this; }
            public virtual Tween<T> SetTarget(T value) { b = value; return this; }
            /// <summary>
            /// Sets the time scale mode (scaled or unscaled) used by the tween.
            /// </summary>
            public virtual Tween<T> SetTimeMode(Delta value) { timeMode = value; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween starts.
            /// </summary>
            public virtual Tween<T> OnStart(Action action) { onStart = action; return this; }
            /// <summary>
            /// Sets a callback to invoke during each tween update.
            /// </summary>
            public virtual Tween<T> OnUpdate(Action<float> action) { onUpdate = action; return this; }
            /// <summary>
            /// Sets a callback to invoke when the tween completes.
            /// </summary>
            public virtual Tween<T> OnComplete(Action action) { onComplete = action; return this; }
            #endregion
      }
}