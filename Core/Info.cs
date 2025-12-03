namespace Emp37.Tweening
{
      public readonly struct Info
      {
            public struct Property
            {
                  public string Label;
                  public object Value;

                  public Property(string label, object value)
                  {
                        Label = label;
                        Value = value;
                  }
            }

            public readonly string Title;
            public readonly float Ratio;
            public readonly Property[] Properties;

            public Info(string title, params Property[] properties) : this(title, 1F, properties) { }
            public Info(string title, float ratio, params Property[] properties)
            {
                  Title = title;
                  Ratio = ratio;
                  Properties = properties;
            }
      }
}