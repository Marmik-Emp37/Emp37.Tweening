using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Emp37.Tweening
{
      using static Ease;

      public static class Tween
      {
            #region Extensions
            public static void Play<T>(this T element) where T : IElement
            {
                  element.Init();
                  Factory.Add(element);
            }
            public static Sequence Then(this IElement current, IElement next) => new(current, next);
            #endregion

            #region Factory Methods
            public static Tween<T> Create<T>(Func<T> onInitialize, T target, float duration, Action<T> onValueChange, Tween<T>.Evaluator evaluator) where T : struct
            {
                  bool isValid = true;
                  if (duration <= 0F) { warn($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (onInitialize == null) { warn($"Missing required delegate: {nameof(onInitialize)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (onValueChange == null) { warn($"Missing required delegate: {nameof(onValueChange)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }
                  return isValid ? new Tween<T>(onInitialize, target, duration, onValueChange, evaluator) : Tween<T>.Empty;

                  static void warn(string message) => Debug.LogWarning($"{nameof(Tween<T>)} creation failed: {message}");
            }
            public static Tween<float> Create(Func<float> onInitialize, float target, float duration, Action<float> onValueChange) => Create(onInitialize, target, duration, onValueChange, Mathf.LerpUnclamped);
            public static Tween<Vector2> Create(Func<Vector2> onInitialize, Vector2 target, float duration, Action<Vector2> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector2.LerpUnclamped);
            public static Tween<Vector3> Create(Func<Vector3> onInitialize, Vector3 target, float duration, Action<Vector3> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector3.LerpUnclamped);
            public static Tween<Quaternion> Create(Func<Quaternion> onInitialize, Quaternion target, float duration, Action<Quaternion> onValueChange) => Create(onInitialize, target, duration, onValueChange, Quaternion.LerpUnclamped);
            public static Tween<Color> Create(Func<Color> onInitialize, Color target, float duration, Action<Color> onValueChange) => Create(onInitialize, target, duration, onValueChange, Color.LerpUnclamped);
            #endregion

            public static class Curves
            {
                  private static readonly Keyframe Zero = new(0F, 0F), One = new(1F, 1F), Exit = new(1F, 0F);
                  public static AnimationCurve Anticipate => new(Zero, new(0.3F, -0.3F), One);
                  public static AnimationCurve Pop => new(Zero, new(0.6F, 0.05F, 0.25F, 0.75F), new(0.85F, 0.9F, 1.25F, 1.25F), One);
                  public static AnimationCurve Punch => new(Zero, new(0.1F, 1F), new(0.25F, -0.6F), new(0.5F, 0.4F), new(0.7F, -0.2F), Exit);
                  public static AnimationCurve Shake => new(Zero, new(0.1F, 0.5F), new(0.2F, -0.5F), new(0.3F, 0.4F), new(0.4F, -0.4F), new(0.5F, 0.3F), new(0.6F, -0.3F), new(0.7F, 0.2F), new(0.8F, -0.2F), new(0.9F, 0.1F), Exit);
                  public static AnimationCurve Snappy => new(Zero, new(0.3F, 1.05F, 0.75F, 0.75F), new(0.6F, 0.95F), One);
                  public static AnimationCurve Spring => new(Zero, new(0.3F, 1.3F), new(0.6F, 0.8F), new(0.8F, 1.05F), One);
            }

            #region Settings
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

                  public static readonly LoopParams Default = new(Type.None, 0);

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

            #region Elements
            public sealed class Sequence : IElement
            {
                  private readonly Queue<IElement> elementsQueue = new();
                  private IElement current;

                  public Phase Phase { get; private set; }
                  public bool IsEmpty => current == null && elementsQueue.Count == 0;


                  private Sequence() { }
                  public Sequence(params IElement[] elements) : this() => Append(elements);

                  public Sequence Append(IElement element)
                  {
                        if (!element.IsEmpty)
                        {
                              elementsQueue.Enqueue(element);
                        }
                        return this;
                  }
                  public Sequence Append(params IElement[] elements)
                  {
                        foreach (var e in elements)
                        {
                              Append(e);
                        }
                        return this;
                  }
                  public Sequence Join(params IElement[] elements) => Append(new Parallel(elements));

                  void IElement.Init() => Phase = Phase.Active;
                  void IElement.Update()
                  {
                        if (Phase != Phase.Active) return;

                        if (current == null)
                        {
                              if (elementsQueue.Count == 0)
                              {
                                    Phase = Phase.Complete;
                                    return;
                              }
                              current = elementsQueue.Dequeue();
                              current.Init();
                        }
                        current.Update();
                        if (current.Phase is Phase.None or Phase.Complete)
                        {
                              current = null;
                        }
                  }

                  public void Pause()
                  {
                        if (Phase is Phase.Active)
                        {
                              Phase = Phase.Paused;
                              current?.Pause();
                        }
                  }
                  public void Resume()
                  {
                        if (Phase is Phase.Paused)
                        {
                              Phase = Phase.Active;
                              current?.Resume();
                        }
                  }
                  public void Kill()
                  {
                        Phase = Phase.None;
                        current?.Kill(); current = null;
                        elementsQueue.Clear();
                  }
            }
            public sealed class Parallel : IElement
            {
                  private readonly List<IElement> elements;

                  public Phase Phase { get; private set; }
                  public bool IsEmpty => elements.Count == 0;

                  private Parallel() { }
                  public Parallel(params IElement[] elements) => this.elements = elements?.Where(e => !e.IsEmpty).ToList() ?? new();

                  void IElement.Init()
                  {
                        Phase = Phase.Active;
                        for (int i = elements.Count - 1; i >= 0; i--)
                        {
                              elements[i].Init();
                        }
                  }
                  void IElement.Update()
                  {
                        if (Phase is not Phase.Active) return;

                        bool isComplete = true;
                        for (int i = elements.Count - 1; i >= 0; i--)
                        {
                              var element = elements[i];
                              switch (element.Phase)
                              {
                                    case Phase.None or Phase.Complete:
                                          continue;
                                    case Phase.Active:
                                          {
                                                element.Update();
                                                isComplete = false;
                                          }
                                          break;
                              }
                        }
                        if (isComplete)
                        {
                              Phase = Phase.Complete;
                        }
                  }

                  public void Pause()
                  {
                        if (Phase is Phase.Active)
                        {
                              Phase = Phase.Paused;
                              foreach (var e in elements)
                              {
                                    e.Pause();
                              }
                        }
                  }
                  public void Resume()
                  {
                        if (Phase is Phase.Paused)
                        {
                              Phase = Phase.Active;
                              foreach (var e in elements)
                              {
                                    e.Resume();
                              }
                        }
                  }
                  public void Kill()
                  {
                        Phase = Phase.None;
                        foreach (var e in elements)
                        {
                              e.Kill();
                        }
                        elements.Clear();
                  }
            }
            #endregion
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

            public Phase Phase { get; private set; }
            public bool IsEmpty => ReferenceEquals(this, Empty);

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
                  bootstrapped = false;
            }
            void IElement.Update()
            {
                  if (Phase != Phase.Active) return;

                  float deltaTime = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;

                  if (delay > 0F)
                  {
                        delay -= deltaTime;
                        if (delay > 0F) return;

                        delay = 0F;
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
                  updateTween(evaluator(a, b, eased));
                  onUpdate(eased);

                  if (progress < 1F) return;

                  if (loop.LoopType == 0 || loop.Count == 0)
                  {
                        Phase = Phase.Complete;
                        onComplete?.Invoke();
                        return;
                  }
                  if (loop.Count > 0) loop.Count--;
                  switch (loop.LoopType)
                  {
                        case Tween.LoopParams.Type.Repeat: break;
                        case Tween.LoopParams.Type.Yoyo: (a, b) = (b, a); break;
                  }
                  progress = 0F;
                  delay = loop.Interval;
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
                  Phase = Phase.None;
            }
            public virtual void TerminateLoop() => SetLoop(Tween.LoopParams.Default);

            #region Config
            public virtual Tween<T> SetAutoReturn(float delay = 0F) => SetLoop(new(Tween.LoopParams.Type.Yoyo, 1, delay));
            /// <summary>
            /// Sets the easing function using a predefined easing type.
            /// </summary>
            public virtual Tween<T> SetEase(Type type) { easingFunction = TypeMap[type]; return this; }
            /// <summary>
            /// Sets a custom easing to this tween using an <see cref="AnimationCurve"/>. The curve maps normalized progress (0 to 1) to interpolation output.
            /// </summary>
            /// <param name="curve">The curve used to control easing over time.</param>
            /// <remarks>
            /// For predefined animation curves, see <see cref="Tween.Curves"/>.
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