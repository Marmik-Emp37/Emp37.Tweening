using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Emp37.Tweening 
{
      using static Tween;

      public static class UIExtensions
      {
            public static Value<float> TweenFade(this CanvasGroup group, float target, float duration) => Value(group, () => group.alpha, target, duration, value => group.alpha = value);
            public static Value<Vector2> TweenMove(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => transform.anchoredPosition, () => relative ? transform.anchoredPosition + target : target, duration, value => transform.anchoredPosition = value);
            public static Value<Vector3> TweenMove(this RectTransform transform, Vector3 target, float duration, bool relative = false) => Value(transform, () => transform.anchoredPosition3D, () => relative ? transform.anchoredPosition3D + target : target, duration, value => transform.anchoredPosition3D = value);
            public static Value<Vector2> TweenSize(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(transform, () => transform.sizeDelta, () => relative ? transform.sizeDelta + target : target, duration, value => transform.sizeDelta = value);
            public static Value<float> TweenAlpha(this Graphic graphic, float target, float duration) => Value(graphic, () => graphic.color.a, target, duration, value => { var color = graphic.color; color.a = value; graphic.color = color; });
            public static Value<Color> TweenColor(this Graphic graphic, Color target, float duration) => Value(graphic, () => graphic.color, target, duration, value => graphic.color = value);
            public static Value<float> TweenFill(this Image image, float target, float duration) => Value(image, () => image.fillAmount, target, duration, value => image.fillAmount = value);
            public static Value<float> TweenText(this Text text, string target, float duration)
            {
                  if (string.IsNullOrEmpty(target))
                  {
                        Log.RejectTween("Cannot tween to null or empty text.");
                        return Value<float>.Empty;
                  }
                  return Value(text, () => 0, target.Length, duration, value => { int count = Mathf.Clamp(Mathf.FloorToInt(value), 0, target.Length); text.text = count == 0 ? string.Empty : target[..count]; });
            }        
            public static Value<float> TweenText(this TMP_Text text, string target, float duration)
            {
                  if (string.IsNullOrEmpty(target))
                  {
                        Log.RejectTween("Cannot tween to null or empty text.");
                        return Value<float>.Empty;
                  }
                  return Value(text, () => 0, target.Length, duration, value => { int count = Mathf.Clamp(Mathf.FloorToInt(value), 0, target.Length); text.text = count == 0 ? string.Empty : target[..count]; });
            }
            public static Value<float> TweenNumber(this Text text, float target, float duration, string format, bool relative = false) => Value(text, () => float.TryParse(text.text, out float value) ? value : 0F, () => relative ? (float.TryParse(text.text, out float value) ? value : 0F) + target : target, duration, value => text.text = value.ToString(format));
            public static Value<float> TweenNumber(this TMP_Text text, float target, float duration, string format, bool relative = false) => Value(text, () => float.TryParse(text.text, out float value) ? value : 0F, () => relative ? (float.TryParse(text.text, out float value) ? value : 0F) + target : target, duration, value => text.text = value.ToString(format));
      }
}