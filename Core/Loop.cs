namespace Emp37.Tweening
{
      public readonly struct Loop
      {
            public enum Type : byte
            {
                  None, Repeat, Yoyo
            }

            public readonly int Count;
            public readonly Type Mode;

            public static readonly Loop Default = new(0, Type.None);

            public Loop(int count, Type mode) { Count = count; Mode = mode; }
      }
}