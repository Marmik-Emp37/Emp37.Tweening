using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Emp37.Tweening
{
      using static Ease;

      public static class Tween
      {
            #region E X T E N S I O N S
            public static void Play<T>(this T element) where T : IElement
            {
                  element.Init();
                  Factory.Add(element);
            }
            public static Sequence Then(this IElement current, IElement next) => new(current, next);
            #endregion

            #region F A C T O R Y   M E T H O D S
            /// <summary>
            /// Creates a new tween for a value type.
            /// </summary>
            /// <typeparam name="T">The value type being interpolated.</typeparam>
            /// <param name="startValue">A delegate that retrieves the starting value for the tween at the moment it begins execution. This is called only once unless the loop configuration is set to recapture it.</param>
            /// <param name="target">The final value to interpolate toward.</param>
            /// <param name="duration">The time in seconds over which the tween runs. Must be greater than zero.</param>
            /// <param name="onValueChange">A delegate invoked each frame with the interpolated value.</param>
            /// <param name="evaluator">A function used to evaluate the interpolation between the start and target values.</param>
            /// <returns>A configured <see cref="Value{T}"/> instance, or an empty tween if parameters are invalid.</returns>
            public static Value<T> Create<T>(Func<T> startValue, T target, float duration, Action<T> onValueChange, Value<T>.Evaluator evaluator) where T : struct
            {
                  bool isValid = true;
                  if (duration <= 0F) { warn($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (startValue == null) { warn($"Missing required delegate: {nameof(startValue)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (onValueChange == null) { warn($"Missing required delegate: {nameof(onValueChange)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }
                  if (evaluator == null) { warn($"Missing required delegate: {nameof(evaluator)}. This delegate defines how the tween interpolates between two values and must not be null."); isValid = false; }
                  return isValid ? new Value<T>(startValue, target, duration, onValueChange, evaluator) : Value<T>.Empty;

                  static void warn(string message) => Debug.LogWarning($"{nameof(Value<T>)} creation failed: {message}");
            }
            public static Value<float> Create(Func<float> startValue, float target, float duration, Action<float> onValueChange) => Create(startValue, target, duration, onValueChange, Mathf.LerpUnclamped);
            public static Value<Vector2> Create(Func<Vector2> startValue, Vector2 target, float duration, Action<Vector2> onValueChange) => Create(startValue, target, duration, onValueChange, Vector2.LerpUnclamped);
            public static Value<Vector3> Create(Func<Vector3> startValue, Vector3 target, float duration, Action<Vector3> onValueChange) => Create(startValue, target, duration, onValueChange, Vector3.LerpUnclamped);
            public static Value<Quaternion> Create(Func<Quaternion> startValue, Quaternion target, float duration, Action<Quaternion> onValueChange) => Create(startValue, target, duration, onValueChange, Quaternion.LerpUnclamped);
            public static Value<Color> Create(Func<Color> startValue, Color target, float duration, Action<Color> onValueChange) => Create(startValue, target, duration, onValueChange, Color.LerpUnclamped);
            #endregion

            #region M O D U L E S
            [Serializable]
            public struct Loop
            {
                  public enum Type
                  {
                        None,
                        Repeat,
                        Yoyo,
                  }

                  public Type Mode;
                  public int Count;
                  public float Delay;
                  public bool IsDynamic;

                  public static readonly Loop Default = new(Type.None, 0);

                  /// <summary>
                  /// Creates a new loop configuration for a tween.
                  /// </summary>
                  /// <param name="type">The loop behavior type.</param>
                  /// <param name="count">The number of times the tween should loop. Set to -1 for infinite loops.</param>
                  /// <param name="interval">Optional delay in seconds between loop cycles.</param>
                  /// <param name="isDynamic"> If true, the tween does not re-capture the start value after the initial run. Use this to lock the initial state during loops.
                  /// </param>
                  public Loop(Type type, int count, float interval = 0F, bool isDynamic = false) { Mode = type; Count = count; Delay = interval; IsDynamic = isDynamic; }
            }
            #endregion

            #region E L E M E N T S
            public class Value<T> : IElement where T : struct
            {
                  public sealed class Blank : Value<T>
                  {
                        public override Value<T> SetAutoReturn(float _ = 0) => this;
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

                  public static readonly Blank Empty = new() { Phase = Phase.None };

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

                  private Action onStart;
                  private Action<float> onUpdate;
                  private Action onComplete;

                  public Phase Phase { get; private set; }
                  public bool IsEmpty => ReferenceEquals(this, Empty);

                  private protected Value() { }
                  internal Value(Func<T> fetchValue, T target, float duration, Action<T> apply, Evaluator evaluator)
                  {
                        b = target;
                        inverseDuration = 1F / duration;
                        this.evaluator = evaluator;

                        initTween = () => a = fetchValue();
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
                              delay = Mathf.Max(delay - deltaTime, 0F);
                              if (delay != 0F) return;
                              // It is what it is
                              if (loop.IsDynamic && bootstrapped) initTween();
                        }

                        if (!bootstrapped)
                        {
                              bootstrapped = true;

                              initTween();
                              Utils.SafeInvoke(onStart);
                        }

                        progress = Mathf.Min(progress + deltaTime * inverseDuration, 1F);
                        float eased = easingFunction(progress);
                        updateTween(evaluator(a, b, eased));
                        Utils.SafeInvoke(onUpdate, eased);

                        if (progress < 1F) return;

                        if (loop.Mode == 0 || loop.Count == 0)
                        {
                              Phase = Phase.Complete;
                              Utils.SafeInvoke(onComplete);
                              return;
                        }
                        if (loop.Count > 0) loop.Count--;
                        switch (loop.Mode)
                        {
                              case Loop.Type.Repeat: break;
                              case Loop.Type.Yoyo: (a, b) = (b, a); break;
                        }
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
                  public virtual void Kill()
                  {
                        Phase = Phase.None;
                  }
                  public virtual void TerminateLoop() => SetLoop(Loop.Default);

                  #region C O N F I G U R A T I O N
                  public virtual Value<T> SetAutoReturn(float delay = 0F) => SetLoop(new(Tween.Loop.Type.Yoyo, 1, delay));
                  /// <summary>
                  /// Sets the easing function using a predefined easing type.
                  /// </summary>
                  public virtual Value<T> SetEase(Type type) { easingFunction = TypeMap[type]; return this; }
                  /// <summary>
                  /// Sets a custom easing to this tween using an <see cref="AnimationCurve"/>. The curve maps normalized progress (0 to 1) to interpolation output.
                  /// </summary>
                  /// <param name="curve">The curve used to control easing over time.</param>
                  /// <remarks>
                  /// For predefined animation curves, see <see cref="Curves"/>.
                  /// </remarks>
                  public virtual Value<T> SetEase(AnimationCurve curve) { easingFunction = curve.Evaluate; return this; }
                  /// <summary>
                  /// Sets the delay before the tween starts.
                  /// </summary>
                  /// <param name="seconds">In seconds.</param>
                  public virtual Value<T> SetDelay(float seconds) { delay = seconds; return this; }
                  /// <summary>
                  /// Set this tween to repeat based on a custom loop strategy.
                  /// </summary>
                  public virtual Value<T> SetLoop(in Loop config) { loop = config; return this; }
                  public virtual Value<T> SetTarget(T value) { b = value; return this; }
                  /// <summary>
                  /// Sets the time scale mode (scaled or unscaled) used by the tween.
                  /// </summary>
                  public virtual Value<T> SetTimeMode(Delta type) { timeMode = type; return this; }
                  /// <summary>
                  /// Sets a callback to invoke when the tween starts.
                  /// </summary>
                  public virtual Value<T> OnStart(Action action) { onStart = action; return this; }
                  /// <summary>
                  /// Sets a callback to invoke during each tween update.
                  /// </summary>
                  public virtual Value<T> OnUpdate(Action<float> action) { onUpdate = action; return this; }
                  /// <summary>
                  /// Sets a callback to invoke when the tween completes.
                  /// </summary>
                  public virtual Value<T> OnComplete(Action action) { onComplete = action; return this; }
                  #endregion
            }
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
                  public Sequence Append(float delay) => Append(new Delay(delay));
                  public Sequence Append(Func<bool> predicate) => Append(new Delay(predicate));
                  public Sequence Append(float delay, Delta type, Func<bool> predicate) => Append(new Delay(delay, type, predicate));

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
                  private readonly List<IElement> activeElements;

                  public Phase Phase { get; private set; }
                  public bool IsEmpty => activeElements.Count == 0;

                  private Parallel() { }
                  public Parallel(params IElement[] elements) => activeElements = elements?.Where(e => !e.IsEmpty).ToList() ?? new();

                  void IElement.Init()
                  {
                        Phase = Phase.Active;
                        foreach (var e in activeElements)
                        {
                              e.Init();
                        }
                  }
                  void IElement.Update()
                  {
                        if (Phase != Phase.Active) return;

                        for (int i = activeElements.Count - 1; i >= 0; i--)
                        {
                              var element = activeElements[i];
                              if (element.Phase == Phase.Active)
                              {
                                    element.Update();
                              }
                              if (element.Phase is Phase.None or Phase.Complete)
                              {
                                    activeElements.RemoveAt(i);
                                    if (activeElements.Count == 0)
                                    {
                                          Phase = Phase.Complete;
                                          return;
                                    }
                              }
                        }
                  }

                  public void Pause()
                  {
                        if (Phase is Phase.Active)
                        {
                              Phase = Phase.Paused;
                              foreach (var e in activeElements)
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
                              foreach (var e in activeElements)
                              {
                                    e.Resume();
                              }
                        }
                  }
                  public void Kill()
                  {
                        Phase = Phase.None;
                        foreach (var e in activeElements)
                        {
                              e.Kill();
                        }
                        activeElements.Clear();
                  }
            }
            public class Delay : IElement
            {
                  private float time;
                  private readonly Delta timeMode;
                  private readonly Func<bool> predicate;

                  public Phase Phase { get; private set; }
                  public bool IsEmpty => time <= 0F && predicate == null;

                  private Delay() { }
                  public Delay(Func<bool> waitUntil) => predicate = waitUntil;
                  public Delay(float value, Delta mode = Delta.Scaled, Func<bool> waitUntil = null) : this(waitUntil)
                  {
                        time = value;
                        timeMode = mode;
                  }

                  void IElement.Init() => Phase = Phase.Active;
                  void IElement.Update()
                  {
                        if (Phase != Phase.Active) return;

                        if (time > 0F)
                        {
                              time -= timeMode == Delta.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                        }
                        else if (predicate == null || predicate())
                        {
                              Phase = Phase.Complete;
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
                  public void Kill() => Phase = Phase.None;
            }
            #endregion
      }
}