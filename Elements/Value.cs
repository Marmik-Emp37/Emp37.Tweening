using System;

using UnityEngine;
using UnityEngine.Pool;

namespace Emp37.Tweening
{
      using static Ease;

      public class Value<TValue> : Tween where TValue : struct
      {
            // D E L E G A T E S
            public delegate TValue Modifier(TValue value);
            public delegate TValue Interpolator(TValue a, TValue b, float ratio);

            // S T A T I C S
            private static readonly ObjectPool<Value<TValue>> pool = new(
                  createFunc: () => new Value<TValue>(),
                  actionOnGet: v => v.Reset(),
                  collectionCheck: true,
                  defaultCapacity: 64,
                  maxSize: 4096);

            public static readonly Value<TValue> Blank = new Blank<TValue>();

            // F I E L D S
            private bool initializationPending;
            private bool reversePlayback;

            private float elapsed;
            private TValue a, b, current;
            private float delayTime;
            private float inverseDuration;

            private Action bootstrap;
            private Action<TValue> setter;
            private Method easeFunction;
            private Interpolator interpolator;
            private Modifier valueModifier;

            public override bool IsEmpty => ReferenceEquals(Blank, this) || IsLinkDestroyed;

            internal static Value<TValue> Fetch(Func<TValue> source, Func<TValue> destination, float duration, Action<TValue> update, Interpolator interpolator)
            {
                  #region V A L I D A T I O N
                  bool isValid = true;

                  void reject(string message)
                  {
                        Log.Warning($"Tween creation failed ({typeof(Value<TValue>).Name}<{typeof(TValue).Name}>): " + message);
                        isValid = false;
                  }
                  if (source is null) reject($"Missing '{nameof(source)}' function to capture the start value.");
                  if (destination is null) reject($"Missing '{nameof(destination)}' function to capture the end value.");
                  if (float.IsNaN(duration) || float.IsInfinity(duration) || duration <= 0F) reject($"Duration must be a finite number and greater than 0 (received {duration}).");
                  if (update is null) reject($"Missing '{nameof(update)}' callback to apply tweening.");
                  if (interpolator is null) reject($"Missing '{nameof(interpolator)}' action to compute interpolated values.");
                  #endregion

                  if (!isValid) return Blank;

                  Value<TValue> tween = pool.Get();
                  tween.bootstrap = () =>
                  {
                        tween.a = tween.current = source();
                        tween.b = destination();
                  };
                  tween.inverseDuration = 1F / duration;
                  tween.interpolator = interpolator;
                  tween.setter = update;
                  return tween;
            }

            protected override void Reset()
            {
                  base.Reset();

                  initializationPending = true;
                  reversePlayback = false;

                  elapsed = 0F;
                  a = b = current = default;
                  delayTime = 0F;
                  inverseDuration = 0F;

                  easeFunction = Linear;
            }

            protected override bool OnUpdate(float deltaTime)
            {
                  if (elapsed < 0F)
                  {
                        elapsed += deltaTime;
                        return false;
                  }

                  if (initializationPending)
                  {
                        if (TryInvoke(bootstrap, killOnException: true)) return false;
                        TryInvoke(onStart);

                        initializationPending = false;
                  }

                  float direction = reversePlayback ? -1F : 1F;
                  elapsed = Mathf.Clamp01(elapsed + deltaTime * direction * inverseDuration);

                  float easedRatio = easeFunction(elapsed);
                  TValue value = interpolator(a, b, easedRatio);

                  if (valueModifier != null)
                  {
                        try { value = valueModifier(value); }
                        catch (Exception ex)
                        {
                              this.Exception(ex);
                              Kill();
                              return false;
                        }
                  }

                  if (TryInvoke(setter, current = value, killOnException: true)) return false;
                  TryInvoke(onUpdate);

                  return elapsed == (reversePlayback ? 0F : 1F);
            }
            protected override void OnLoop(Loop.Type loopType)
            {
                  switch (loopType)
                  {
                        case Loop.Type.None: return;
                        case Loop.Type.Yoyo:
                              reversePlayback = !reversePlayback;
                              break;
                  }
                  elapsed = reversePlayback ? 1F : 0F;
            }

            protected override void Clear()
            {
                  base.Clear();
                  bootstrap = null;
                  easeFunction = null;
                  interpolator = null;
                  valueModifier = null;
                  setter = null;
            }
            protected override void OnRecycle() => pool.Release(this);

            public virtual void Replay(bool includeDelay, bool rebuild)
            {
                  playbackMode = PlaybackMode.Normal;
                  ResetPlayback(includeDelay);
                  if (rebuild) initializationPending = true;
                  Phase = Phase.Active;
            }
            protected override void OnReplay() => ResetPlayback(true);

            public override void Rewind(bool snap = true)
            {
                  if (initializationPending) return;
                  if (snap)
                  {
                        ResetPlayback(true);
                        TryInvoke(setter, current = a, true);
                  }
                  else reversePlayback = true;
                  base.Rewind(snap);
            }
            protected override void OnRewindComplete() => ResetPlayback(true);

            private void ResetPlayback(bool includeDelay)
            {
                  elapsed = includeDelay ? -delayTime : 0F;
                  loopsCompleted = 0;
                  reversePlayback = false;
            }

            #region F L U E N T   M E T H O D S
            public virtual Value<TValue> AddModifier(Modifier method) { if (method != null) valueModifier = valueModifier == null ? method : (value => method(valueModifier(value))); return this; }
            public virtual Value<TValue> SetDelay(float time) { delayTime = time; return this; }
            public virtual Value<TValue> SetEase(Type type) { easeFunction = TypeMap[type]; return this; }
            public virtual Value<TValue> SetEase(AnimationCurve curve) { easeFunction = curve.Evaluate; return this; }
            public virtual Value<TValue> SetEase(Method method) { easeFunction = method; return this; }
            public virtual Value<TValue> SetTarget(TValue value, bool rebaseStart = false) { if (rebaseStart) a = current; b = value; return this; }
            #endregion
      }
}