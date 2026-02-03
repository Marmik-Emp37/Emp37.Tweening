using System;

namespace Emp37.Tweening
{
      public enum Phase : byte
      {
            None, Active, Paused, Completed
      }
      public enum Delta : byte
      {
            Scaled, Unscaled
      }
      [Flags]
      public enum Options : uint
      {
            Link = 1 << 0, AutoKill = 1 << 1, Recycle = 1 << 2
      }
}