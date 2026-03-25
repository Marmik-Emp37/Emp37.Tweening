using System;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
	public abstract class Tween
	{
		// F I E L D S
		private string tag;
		private UObject linkedTarget;
		private protected Callbacks callbacks;

		private protected float delay, elapsedDelay;

		private protected Loop loop;
		private protected Phase phase;

		private protected bool isLinked, isRetreating, isAutoKill, isRecyclable;

		// P R O P E R T I E S
		public string Tag => tag;
		public Phase Phase => phase;

		internal abstract bool IsEmpty { get; }
		internal abstract bool CanMoveForward { get; }
		internal abstract bool CanMoveBack { get; }

		protected bool IsLinkDead => isLinked && linkedTarget == null;
		protected bool IsDead => phase is Phase.Dead;


		internal abstract void Update();

		internal abstract void Pause();
		internal abstract void Resume();

		public abstract void PlayForward();
		public abstract void PlayBackward();
		public abstract void Replay(bool includeDelay = true, bool rebuild = false);
		public abstract void Retreat(bool snap = true);
		public abstract void Kill();

		// L I F E C Y C L E
		protected virtual void RestoreToDefault()
		{
			callbacks = Callbacks.Default;
			loop = Loop.Default;
			phase = Phase.Idle;
			isLinked = isRetreating = false;
			isAutoKill = isRecyclable = true;
		}
		protected virtual void Clear()
		{
			tag = null;
			linkedTarget = null;
			callbacks = Callbacks.Default;
		}


#pragma warning disable IDE1006
		internal void setDelay(float value) => delay = elapsedDelay = Math.Max(0F, value);
		internal void setTag(string value) => tag = value;
		internal void setLooping(int iterations, Loop.Type type) => loop.Configure(iterations, type);
		internal void setLink(UObject link) { if (isLinked = link != null) linkedTarget = link; }
		internal void setAutoKill(bool value) => isAutoKill = value;
		internal void setRecyclable(bool value) => isRecyclable = value;
		internal void setOnStart(Action callback) => callbacks.onStart = callback ?? Callbacks.none;
		internal void setOnUpdate(Action callback) => callbacks.onUpdate = callback;
		internal void setOnRetreat(Action callback) => callbacks.onRetreat = callback ?? Callbacks.none;
		internal void setOnCycleComplete(Action callback) => callbacks.onCycleComplete = callback ?? Callbacks.none;
		internal void setOnComplete(Action callback) => callbacks.onComplete = callback ?? Callbacks.none;
		internal void setOnKill(Action callback) => callbacks.onKill = callback ?? Callbacks.none;
#pragma warning restore IDE1006
	}
}