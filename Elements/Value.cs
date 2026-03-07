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
            public delegate TValue Modifier(TValue value);

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
            private Action<TValue> update;
            private Method easeMethod;
            private Lerp interpolator;
            private Modifier modifier;

            private TValue a, b, current;
            private float normalizedTime, inverseDuration;

            // P R O P E R T I E S
            public override bool IsEmpty => ReferenceEquals(Blank, this);
            protected override bool CanMoveBack => normalizedTime > Mathf.Epsilon;
            protected override bool CanMoveForward => normalizedTime < 1F - Mathf.Epsilon;

            // C R E A T I O N
            internal static Value<TValue> Fetch(Func<TValue> a, Func<TValue> b, Action<TValue> update, float duration, Lerp lerp)
            {
                  #region V A L I D A T I O N
                  bool isValid = true;
                  void reject(string message)
                  {
                        Log.Warning($"Tween creation failed ({typeof(Value<TValue>).Name}<{typeof(TValue).Name}>): " + message);
                        isValid = false;
                  }
                  if (a is null) reject($"Missing '{nameof(a)}' function to capture the start value.");
                  if (b is null) reject($"Missing '{nameof(b)}' function to capture the end value.");
                  if (float.IsNaN(duration) || float.IsInfinity(duration) || duration <= 0F) reject($"Duration must be a finite number and greater than 0 (received {duration}).");
                  if (update is null) reject($"Missing '{nameof(update)}' callback to apply tweening.");
                  if (lerp is null) reject($"Missing '{nameof(lerp)}' action to compute interpolated values.");
                  #endregion

                  if (!isValid) return Blank;

                  Value<TValue> tween = pool.Get();
                  tween.source = a;
                  tween.destination = b;
                  tween.inverseDuration = 1F / duration;
                  tween.interpolator = lerp;
                  tween.update = update;
                  return tween;
            }

            // U P D A T E   L O O P
            /// <summary>
            /// Called each frame via the updateFunction delegate.
            /// </summary>
            private bool Tick(float delta)
            {
                  float blend = Mathf.Clamp01(normalizedTime + delta * inverseDuration);
                  Apply(normalizedTime = blend);
                  return blend is 1F || blend is 0F;
            }
            private void Apply(float ratio)
            {
                  float easedRatio = easeMethod(ratio);
                  TValue value = interpolator(a, b, easedRatio);
                  if (modifier != null) value = modifier(value);
                  update(current = value);
            }

            // L I F E C Y C L E
            protected override void OnInitialize()
            {
                  a = current = source();
                  b = destination();
            }
            protected override void RestoreToDefault()
            {
                  base.RestoreToDefault();

                  updateFunction = Tick;
                  a = b = current = default;
                  normalizedTime = inverseDuration = 0F;
                  easeMethod = Linear;
            }
            protected override void OnReset(bool snapToStart)
            {
                  normalizedTime = 0F;
                  if (snapToStart) Apply(normalizedTime);
            }
            protected override void OnLoop(LoopType type, float direction)
            {
                  normalizedTime = type switch
                  {
                        LoopType.Repeat => direction > 0F ? 0F : 1F,
                        LoopType.Yoyo => direction > 0F ? 1F : 0F,
                        _ => throw new InvalidOperationException($"OnLoop called with unexpected LoopType: {type}")
                  };
                  Apply(normalizedTime);
            }
            protected override void Clear()
            {
                  base.Clear();
                  source = destination = null;
                  update = null; easeMethod = null; interpolator = null; modifier = null;
            }
            protected override void OnRecycle() => pool.Release(this);

            #region F L U E N T   M E T H O D S
            public virtual Value<TValue> SetModifier(Modifier method) { modifier = method; return this; }
            public virtual Value<TValue> SetEase(Type type) { easeMethod = TypeMap[type]; return this; }
            public virtual Value<TValue> SetEase(AnimationCurve curve) { easeMethod = curve.Evaluate; return this; }
            public virtual Value<TValue> SetEase(Method method) { easeMethod = method; return this; }
            /// <summary>
            /// Retargets the tween's end value.
            /// <br>If <paramref name="rebase"/> is true, the current interpolated value becomes the new start, allowing smooth mid-tween redirects without a visible jump.</br>
            /// </summary>
            public virtual Value<TValue> SetTarget(TValue value, bool rebase = false) { if (rebase) a = current; b = value; return this; }
            #endregion
      }
}