namespace Emp37.Tweening
{
      public readonly struct TweenInfo
      {
            public readonly string Title; 
            public readonly float Ratio;
            public readonly (string Label, object Value)[] Properties;

            public TweenInfo(string title, params (string Label, object Value)[] properties) : this(title, 1F, properties) { }
            public TweenInfo(string title, float ratio, params (string Label, object Value)[] properties)
            {
                  Title = title;
                  Ratio = ratio;
                  Properties = properties;
            }
      }
}