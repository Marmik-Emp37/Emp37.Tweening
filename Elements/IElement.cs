namespace Emp37.Tweening
{
      public interface IElement
      {
            public Phase Phase { get; }
            public bool IsEmpty { get; }

            internal void Init();
            internal void Update();

            public void Pause();
            public void Resume();
            public void Kill();
      }
}