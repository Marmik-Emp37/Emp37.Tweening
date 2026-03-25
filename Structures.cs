using System;

namespace Emp37.Tweening
{
	internal struct Callbacks
	{
		internal static readonly Action none = static delegate { };
		internal static readonly Callbacks Default = new() { onStart = none, onUpdate = null, onRetreat = none, onCycleComplete = none, onComplete = none, onKill = none };

		internal Action onStart, onUpdate, onRetreat, onCycleComplete, onComplete, onKill;
	}


	public struct Loop
	{
		public enum Type { Repeat, Yoyo }

		internal static readonly Loop Default = new() { count = 0, completed = 0, direction = 1, };

		private int count;
		private int completed;
		private Type mode;
		private sbyte direction;

		internal readonly int Count => count;
		internal readonly int Completed => completed;
		internal readonly Type Mode => mode;
		internal readonly sbyte Direction => direction;
		internal readonly bool IsInfinite => count is -1;

		internal void Configure(int iterations, Type type)
		{
			count = Math.Max(-1, iterations);
			mode = type;
		}
		internal bool TryAdvance(bool forward)
		{
			if (count is 0) return false;
			if (!IsInfinite)
			{
				int next = completed + (forward ? 1 : -1);
				if (next < 0 || next > count) return false;
				completed = next;
			}
			if (mode is Type.Yoyo) direction = (sbyte) -direction;
			return true;
		}
		internal void Reset() { completed = 0; direction = 1; }
	}
}
