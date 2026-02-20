using System;

using UnityEngine;
using UnityEngine.Pool;

namespace Emp37.Tweening
{
      using static Ease;

      public class Value<TValue> : Tween where TValue : struct
      {
            // D E L E G A T E S
            public delegate TValue Lerp(TValue a, TValue b, float ratio);

            // S T A T I C
            private static readonly ObjectPool<Value<TValue>> pool = new(
                  createFunc: () => new Value<TValue>(),
                  actionOnGet: v => v.RestoreToDefault(),
                  collectionCheck: true,
                  defaultCapacity: 64,
                  maxSize: 4096);

            public static readonly Value<TValue> Blank = new Blank<TValue>();

            // F I E L D S
            private Func<TValue> source, destination;
            private Action<TValue> applyValue;
            private Method easeFunction;
            private Lerp lerpFunction;
            private Func<TValue, TValue> valueModifier;

            private TValue a, b, current;
            private float normalizedTime, inverseDuration, direction;

            // P R O P E R T I E S
            public override bool IsEmpty => ReferenceEquals(Blank, this) || IsLinkDead;

            // C R E A T I O N
            internal static Value<TValue> Fetch(Func<TValue> source, Func<TValue> destination, float duration, Action<TValue> update, Lerp lerpFunction)
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
                  if (lerpFunction is null) reject($"Missing '{nameof(lerpFunction)}' action to compute interpolated values.");
                  #endregion

                  if (!isValid) return Blank;

                  Value<TValue> tween = pool.Get();
                  tween.source = source;
                  tween.destination = destination;
                  tween.inverseDuration = 1F / duration;
                  tween.lerpFunction = lerpFunction;
                  tween.applyValue = update;
                  return tween;
            }

            // U P D A T E   L O O P
            private bool Update(float deltaTime)
            {
                  float t = normalizedTime + deltaTime * direction * inverseDuration;
                  if (t >= 1F)
                  {
                        Apply(normalizedTime = 1F);
                        return direction == 1F;
                  }
                  if (t <= 0F)
                  {
                        Apply(normalizedTime = 0F);
                        return direction == -1F;
                  }
                  normalizedTime = t;
                  Apply(t);
                  return false;
            }
            private void Apply(float ratio)
            {
                  float easedRatio = easeFunction(ratio);
                  TValue value = lerpFunction(a, b, easedRatio);

                  Func<TValue, TValue> modifier = valueModifier;
                  if (modifier != null) value = modifier(value);

                  applyValue(current = value);
            }

            // L I F E C Y C L E
            protected override void RestoreToDefault()
            {
                  base.RestoreToDefault();

                  updateFunc = Update;
                  a = b = current = default;
                  normalizedTime = inverseDuration = 0F;
                  direction = 1F;
                  easeFunction = Linear;
            }
            protected override void OnReset()
            {
                  normalizedTime = 0F;
                  direction = 1F;
            }
            protected override void Clear()
            {
                  base.Clear();

                  source = destination = null;
                  applyValue = null; easeFunction = null; lerpFunction = null; valueModifier = null;
            }

            protected override void OnInitialize()
            {
                  a = current = source();
                  b = destination();
            }
            protected override void OnLoopComplete(LoopType loopType)
            {
                  switch (loopType)
                  {
                        case LoopType.None: return;
                        case LoopType.Yoyo:
                              direction = -direction;
                              break;
                  }
                  normalizedTime = direction < 0f ? 1F : 0F;
            }
            protected override void OnPlaybackChange(PlaybackMode mode)
            {
                  direction = mode switch
                  {
                        PlaybackMode.Backward or PlaybackMode.Rewind => -1F,
                        _ => +1F
                  };
            }
            protected override void OnRewind(bool snap)
            {
                  if (snap) Apply(0F);
            }
            protected override void OnRecycle() => pool.Release(this);

            #region F L U E N T   M E T H O D S
            public virtual Value<TValue> SetModifier(Func<TValue, TValue> method) { valueModifier = method; return this; }
            public virtual Value<TValue> SetEase(Type type) { easeFunction = TypeMap[type]; return this; }
            public virtual Value<TValue> SetEase(AnimationCurve curve) { easeFunction = curve.Evaluate; return this; }
            public virtual Value<TValue> SetEase(Method method) { easeFunction = method; return this; }
            public virtual Value<TValue> SetTarget(TValue value, bool rebaseStart = false) { if (rebaseStart) a = current; b = value; return this; }
            #endregion
      }
}