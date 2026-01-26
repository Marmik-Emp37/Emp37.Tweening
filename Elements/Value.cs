using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      using static Ease;

      public class Value<T> : ITween where T : struct
      {
            internal static readonly Value<T> Empty = new Blank();
            private sealed class Blank : Value<T>
            {
                  public override Value<T> addModifier(Modifier method) => this;
                  public override Value<T> disableLoop() => this;
                  public override Value<T> setRecyclable(bool _) => this;
                  public override Value<T> setDelay(float _) => this;
                  public override Value<T> setEase(Type _) => this;
                  public override Value<T> setEase(AnimationCurve _) => this;
                  public override Value<T> setEase(Method _) => this;
                  public override Value<T> setLoop(int _, LoopType __, float ___) => this;
                  public override Value<T> setProgress(float _) => this;
                  public override Value<T> setTarget(T _, bool __) => this;
                  public override Value<T> setTimeMode(Delta _) => this;
                  public override Value<T> onStart(Action _) => this;
                  public override Value<T> onUpdate(Action<float> _) => this;
                  public override Value<T> onComplete(Action _) => this;
                  public override Value<T> onKill(Action _) => this;
                  public override Value<T> onConclude(Action _) => this;

                  public override void Pause() { }
                  public override void Resume() { }
                  public override void Kill() { }

                  public override string ToString() => $"{nameof(Value<T>)}<{typeof(T).Name}>.{nameof(Blank)}";
            }


            /// <summary>Transforms a value during interpolation.</summary>
            public delegate T Modifier(T value);

            /// <summary>Interpolates between two values.</summary>
            public delegate T Evaluator(T a, T b, float ratio);


            private static readonly ObjectPool<Value<T>> pool = new(() => new Value<T>(), actionOnGet: v => v.OnGet(), actionOnRelease: v => v.OnRelease(), collectionCheck: true, defaultCapacity: 64);
            private bool isRecyclable;

            private T a, b, current;
            private Delta timeMode;
            private float inverseDuration, progress, delay;
            private bool bootstrapped;

            private Action initTween;
            private Action<T> easeTween;
            private Method easingMethod;
            private Modifier modifier;
            private Evaluator evaluate;

            private UObject linkedTarget;

            private LoopType loopType;
            private int remainingLoops;
            private float loopInterval;
            private sbyte direction;

            private Action actionOnStart, actionOnComplete, actionOnKill, actionOnConclude;
            private Action<float> actionOnUpdate;

            private static readonly IReadOnlyDictionary<Type, Method> easeTypeMap = new Dictionary<Type, Method>
            {
                  { Type.Linear, Linear },
                  { Type.InSine, InSine },
                  { Type.OutSine, OutSine },
                  { Type.InOutSine, InOutSine },
                  { Type.InCubic, InCubic },
                  { Type.OutCubic, OutCubic },
                  { Type.InOutCubic, InOutCubic },
                  { Type.InQuint, InQuint },
                  { Type.OutQuint, OutQuint },
                  { Type.InOutQuint, InOutQuint },
                  { Type.InCirc, InCirc },
                  { Type.OutCirc, OutCirc },
                  { Type.InOutCirc, InOutCirc },
                  { Type.InQuad, InQuad },
                  { Type.OutQuad, OutQuad },
                  { Type.InOutQuad, InOutQuad },
                  { Type.InQuart, InQuart },
                  { Type.OutQuart, OutQuart },
                  { Type.InOutQuart, InOutQuart },
                  { Type.InExpo, InExpo },
                  { Type.OutExpo, OutExpo },
                  { Type.InOutExpo, InOutExpo },
                  { Type.InBack, InBack },
                  { Type.OutBack, OutBack },
                  { Type.InOutBack, InOutBack },
                  { Type.InElastic, InElastic },
                  { Type.OutElastic, OutElastic },
                  { Type.InOutElastic, InOutElastic },
                  { Type.InBounce, InBounce },
                  { Type.OutBounce, OutBounce },
                  { Type.InOutBounce, InOutBounce }
            };

            public string Tag { get; set; }
            public Phase Phase { get; private set; }
            public bool IsEmpty => ReferenceEquals(this, Empty) || IsDestroyed;
            public Info Info => new($"{nameof(Value<T>)}<{typeof(T).Name}>", progress, new Info.Property[]
            {
                  new("Target", linkedTarget == null ? "Destroyed" : linkedTarget.name),
                  new("Delay", $"{delay:0.00}s"),
                  new("Range", $"Initial - {a} → Target - {b}"),
                  new("Duration", $"{1F / inverseDuration: 0.###}s"),
                  new("Loop Info", $"Type - {loopType} | Remaining - {remainingLoops} | Interval - {loopInterval}"),
                  new("Ease", easingMethod ?.Method ?.Name ?? "None"),
                  new("Time Mode", timeMode),
                  new("Action On Start", actionOnStart ?.Method.Name ?? "None"),
                  new("Action On Update", actionOnUpdate ?.Method.Name ?? "None"),
                  new("Action On Complete", actionOnComplete ?.Method.Name ?? "None"),
                  new("Action On Kill", actionOnKill ?.Method.Name ?? "None"),
                  new("Action On Conclude", actionOnConclude ?.Method.Name ?? "None")
            });

            private bool IsDestroyed => linkedTarget == null;


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

                  if (delay > 0F)
                  {
                        if ((delay -= deltaTime) > 0F) return;
                        deltaTime = -delay; // carry over excess time
                        delay = 0F;
                  }

                  if (!bootstrapped)
                  {
                        bootstrapped = true;
                        try { initTween(); } catch (Exception ex) { HandleException(ex); return; }
                        Utils.SafeInvoke(actionOnStart);
                  }

                  progress = Mathf.Min(progress + deltaTime * inverseDuration, 1F);
                  float ratio = direction > 0 ? progress : 1F - progress;
                  float easedRatio = easingMethod(ratio);
                  T value = evaluate(a, b, easedRatio);
                  if (modifier != null)
                  {
                        try { value = modifier(value); } catch (Exception ex) { Log.Error($"Modifier exception: {ex.Message}"); }
                  }
                  try { easeTween(current = value); } catch (Exception ex) { HandleException(ex); return; }

                  Utils.SafeInvoke(actionOnUpdate, easedRatio);

                  if (progress < 1F) return;

                  if (loopType != LoopType.None && remainingLoops != 0)
                  {
                        if (remainingLoops > 0) remainingLoops--; // decrement if finite
                        if (loopType is LoopType.Yoyo) direction *= -1;

                        progress = 0F;
                        delay = loopInterval;
                        return;
                  }

                  Phase = Phase.Finished;
                  Utils.SafeInvoke(actionOnComplete);

                  Conclude();
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
                  if (Phase is Phase.None or Phase.Finished) return;

                  Phase = Phase.None;
                  Utils.SafeInvoke(actionOnKill);

                  Conclude();
            }

            private void Conclude()
            {
                  Utils.SafeInvoke(actionOnConclude);
                  if (isRecyclable)
                  {
                        pool.Release(this);
                  }
            }
            private void HandleException(Exception ex)
            {
                  Log.Exception(ex);
                  Kill();
            }


            #region F L U E N T   A P I
#pragma warning disable IDE1006 // naming styles
            public virtual Value<T> addModifier(Modifier method) { if (!IsDestroyed && method != null) modifier = modifier == null ? method : (value => method(modifier(value))); return this; }
            public virtual Value<T> disableLoop() { loopType = LoopType.None; remainingLoops = 0; return this; }
            public virtual Value<T> setRecyclable(bool value) { isRecyclable = value; return this; }
            public virtual Value<T> setDelay(float seconds) { delay = seconds; return this; }
            public virtual Value<T> setEase(Type type) { if (!IsDestroyed) easingMethod = easeTypeMap[type]; return this; }
            public virtual Value<T> setEase(Method method) { if (!IsDestroyed) easingMethod = method; return this; }
            public virtual Value<T> setEase(AnimationCurve curve) { if (!IsDestroyed) easingMethod = curve.Evaluate; return this; }
            public virtual Value<T> setLoop(int cycles, LoopType type, float delay = 0F) { remainingLoops = (cycles <= 0) ? -1 : cycles; loopType = type; loopInterval = Mathf.Max(0F, delay); return this; }
            public virtual Value<T> setProgress(float normalizedValue) { progress = Mathf.Clamp01(normalizedValue); return this; }
            public virtual Value<T> setTarget(T value, bool rebaseStart = false) { if (rebaseStart) a = current; b = value; return this; }
            public virtual Value<T> setTimeMode(Delta mode) { timeMode = mode; return this; }

            public virtual Value<T> onStart(Action action) { if (!IsDestroyed) actionOnStart = action; return this; }
            public virtual Value<T> onUpdate(Action<float> action) { if (!IsDestroyed) actionOnUpdate = action; return this; }
            public virtual Value<T> onComplete(Action action) { if (!IsDestroyed) actionOnComplete = action; return this; }
            public virtual Value<T> onKill(Action action) { if (!IsDestroyed) actionOnKill = action; return this; }
            public virtual Value<T> onConclude(Action action) { if (!IsDestroyed) actionOnConclude = action; return this; }
#pragma warning restore IDE1006
            #endregion

            #region P O O L   A C T I O N S
            private void OnGet()
            {
                  Phase = Phase.None;
                  isRecyclable = true;
                  a = b = current = default;
                  timeMode = Delta.Scaled;
                  inverseDuration = 0F;
                  progress = 0F;
                  delay = 0F;
                  bootstrapped = false;
                  easingMethod = Linear;
                  evaluate = null;
                  loopType = LoopType.None;
                  remainingLoops = 0;
                  loopInterval = 0F;
                  direction = 1;
                  Tag = null;
            }
            private void OnRelease()
            {
                  initTween = null;
                  easeTween = null;
                  easingMethod = null;
                  modifier = null;
                  actionOnStart = null;
                  actionOnComplete = null;
                  actionOnKill = null;
                  actionOnConclude = null;
                  actionOnUpdate = null;
                  linkedTarget = null;
            }
            #endregion


            private static bool Validate(UObject link, object initialization, object target, float duration, object update, object evaluator)
            {
                  bool ok = true;
                  if (link == null) { Log.RejectTween($"No valid ({nameof(link)}) provided."); ok = false; }
                  if (initialization == null) { Log.RejectTween($"Missing ({nameof(initialization)}) delegate to capture the start value."); ok = false; }
                  if (target == null) { Log.RejectTween($"Missing ({nameof(target)}) value or getter."); ok = false; }
                  if (duration <= 0F) { Log.RejectTween($"Duration must be greater than 0 (received  {duration})."); ok = false; }
                  if (update == null) { Log.RejectTween($"Missing ({nameof(update)}) callback to apply tweened values."); ok = false; }
                  if (evaluator == null) { Log.RejectTween($"Missing ({nameof(evaluator)}) function to compute interpolated values."); ok = false; }
                  return ok;
            }
            internal static Value<T> Fetch(UObject link, Func<T> initialization, Func<T> target, float duration, Action<T> update, Evaluator evaluator)
            {
                  if (!Validate(link, initialization, target, duration, update, evaluator)) return Empty;
                  Value<T> value = pool.Get();
                  value.linkedTarget = link;
                  value.initTween = () =>
                  {
                        value.a = initialization();
                        value.b = target();
                        value.current = value.a;
                  };
                  value.inverseDuration = 1F / duration;
                  value.easeTween = update;
                  value.evaluate = evaluator;
                  return value;
            }
      }
}