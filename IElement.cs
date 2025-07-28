namespace Emp37.Tweening
{
      public interface IElement
      {
            public bool IsEmpty { get; }
            public Phase Phase { get; }

            internal void Update();
            public void Pause();
            public void Resume();
            public void Kill();
      }
}