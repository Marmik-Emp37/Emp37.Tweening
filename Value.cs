using System;

using UnityEngine;
using UnityEngine.Pool;

namespace Emp37.Tweening
{
	using static Ease;

	public class Value<T> : Tween where T : struct
	{
		// D E L E G A T E S
		public delegate T Getter();
		public delegate void Setter(T value);
		public delegate T Interpolator(T a, T b, float ratio);
		public delegate T Modifier(T value);

		// F I E L D S
		private static readonly ObjectPool<Value<T>> pool = new(
			createFunc: () => new Value<T>(),
			actionOnGet: v => v.RestoreToDefault(),
			actionOnRelease: v => v.Clear(),
			collectionCheck: true,
			defaultCapacity: 64,
			maxSize: 4096);

		public static readonly Value<T> Blank = new Blank<T>();

		private Getter source, destination;
		private Setter update;
		private Method easeMethod;
		private Interpolator interpolator;
		private Modifier modifier;

		private T a, b, current;
		private float normalizedTime, inverseDuration;

		private sbyte direction;
		private bool isInitializationPending;

		private Delta timeMode;


		// P R O P E R T I E S
		internal override bool IsEmpty => ReferenceEquals(Blank, this);
		internal override bool CanMoveBack => normalizedTime > Mathf.Epsilon;
		internal override bool CanMoveForward => normalizedTime < 1F - Mathf.Epsilon;

		internal static Value<T> Fetch(Getter a, Getter b, Setter update, float duration, Interpolator lerp)
		{
			#region V A L I D A T I O N
			bool isValid = true;
			void reject(string message)
			{
				Log.Warning($"Tween creation failed ({typeof(Value<T>).Name}<{typeof(T).Name}>): " + message);
				isValid = false;
			}
			if (a is null) reject($"Missing '{nameof(a)}' function to capture the start value.");
			if (b is null) reject($"Missing '{nameof(b)}' function to capture the end value.");
			if (float.IsNaN(duration) || float.IsInfinity(duration) || duration <= 0F) reject($"Duration must be a finite number and greater than 0 (received {duration}).");
			if (update is null) reject($"Missing '{nameof(update)}' callback to apply tweening.");
			if (lerp is null) reject($"Missing '{nameof(lerp)}' action to compute interpolated values.");
			#endregion

			if (!isValid) return Blank;

			Value<T> tween = pool.Get();
			tween.source = a;
			tween.destination = b;
			tween.inverseDuration = 1F / duration;
			tween.interpolator = lerp;
			tween.update = update;
			return tween;
		}

		internal override void Update()
		{
			if (IsLinkDead)
			{
				Kill();
				return;
			}

			float deltaTime = timeMode is Delta.Scaled ? Time.deltaTime : Time.unscaledDeltaTime;

			if (elapsedDelay > 0F)
			{
				if ((elapsedDelay -= deltaTime) > 0F) return;

				deltaTime = Math.Abs(elapsedDelay);
				elapsedDelay = 0F;
			}

			if (isInitializationPending)
			{
				isInitializationPending = false;

				a = source();
				b = destination();
				current = a;

				callbacks.onStart();
			}

			int direction = this.direction * loop.Direction;

			float t = Mathf.Clamp01(normalizedTime + deltaTime * direction * inverseDuration);
			Apply(normalizedTime = t);
			bool complete = t is 1F || t is 0F;
			callbacks.onUpdate?.Invoke();
			if (!complete) return;

			if (isRetreating)
			{
				FinishRetreat();
				return;
			}

			if (loop.TryAdvance(direction > 0))
			{
				normalizedTime = (direction > 0 ^ loop.Mode == Loop.Type.Repeat) ? 1F : 0F;
				Apply(normalizedTime);

				callbacks.onCycleComplete();
				return;
			}

			phase = Phase.Completed;
			callbacks.onComplete();

			if (isAutoKill) Kill();
		}
		private void Apply(float ratio)
		{
			float easedRatio = easeMethod(ratio);
			T value = interpolator(a, b, easedRatio);
			if (modifier != null) value = modifier(value);
			update(current = value);
		}
		internal override void Pause()
		{
			if (phase == Phase.Active) phase = Phase.Paused;
		}
		internal override void Resume()
		{
			if (phase == Phase.Paused) phase = Phase.Active;
		}

		public override void PlayForward()
		{
			if (IsDead || !CanMoveForward) return;

			isRetreating = false;
			direction = 1;

			phase = Phase.Active;
		}
		public override void PlayBackward()
		{
			if (IsDead || !CanMoveBack) return;

			isRetreating = false;
			direction = -1;

			phase = Phase.Active;
		}
		public override void Replay(bool includeDelay = true, bool rebuild = false)
		{
			if (IsDead) return;

			direction = 1;
			if (rebuild) isInitializationPending = true;
			Reset(includeDelay);

			phase = Phase.Active;
		}
		public override void Retreat(bool snap = true)
		{
			if (isInitializationPending || IsDead || !CanMoveBack) return;
			if (snap)
			{
				FinishRetreat();
				return;
			}
			if (isRetreating) return;
			isRetreating = true;
			direction = (sbyte) -loop.Direction;
			phase = Phase.Active;
		}
		public override void Kill()
		{
			if (IsDead) return;

			phase = Phase.Dead;
			callbacks.onKill();
			Clear();

			if (isRecyclable) pool.Release(this);
		}

		protected override void RestoreToDefault()
		{
			base.RestoreToDefault();

			delay = elapsedDelay = 0F;
			timeMode = Delta.Scaled;
			direction = 1;
			isInitializationPending = true;

			a = b = current = default;
			normalizedTime = inverseDuration = 0F;
			easeMethod = Linear;
		}
		protected override void Clear()
		{
			base.Clear();

			source = destination = null;
			update = null; easeMethod = null; interpolator = null; modifier = null;
		}

		private void Reset(bool includeDelay = true)
		{
			elapsedDelay = includeDelay ? delay : 0F;
			loop.Reset();

			normalizedTime = 0F;
			if (!isInitializationPending) Apply(normalizedTime);
		}

		private void FinishRetreat()
		{
			isRetreating = false;
			direction = 1;
			Reset();

			// force pause
			phase = Phase.Paused;

			callbacks.onRetreat();
		}

		#region F L U E N T   M E T H O D S
		public virtual Value<T> SetTimeMode(Delta mode) { timeMode = mode; return this; }
		public virtual Value<T> SetModifier(Modifier method) { modifier = method; return this; }
		public virtual Value<T> SetEase(Type type) { easeMethod = TypeMap[type]; return this; }
		public virtual Value<T> SetEase(AnimationCurve curve) { easeMethod = curve.Evaluate; return this; }
		public virtual Value<T> SetEase(Method method) { easeMethod = method; return this; }
		public virtual Value<T> SetTarget(T value, bool rebase = false) { if (rebase) a = current; b = value; return this; }
		#endregion
	}
}