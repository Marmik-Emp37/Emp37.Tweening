namespace Emp37.Tweening
{
      public enum Phase : byte
      {
            Idle, Active, Paused, Completed, Dead
      }
      public enum Delta : byte
      {
            Scaled, Unscaled
      }

      public enum LoopType : byte
      {
            None, Repeat, Yoyo
      }
}