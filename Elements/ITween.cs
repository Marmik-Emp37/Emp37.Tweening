namespace Emp37.Tweening
{
      public interface ITween
      {
            public string Tag { get; set; }
            public Phase Phase { get; }
            public bool IsEmpty { get; }
            public TweenInfo Info { get; }

            internal void Init();
            internal void Update();

            public void Pause();
            public void Resume();
            public void Kill();
      }
}