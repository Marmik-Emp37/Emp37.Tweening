using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Emp37.Tweening
{
      using static Tweens;

      public static class UIExtensions
      {
            // C A N V A S   G R O U P
            public static Value<float> TweenFade(this CanvasGroup group, float target, float duration, bool relative = false) => Value(() => group.alpha, target, value => group.alpha = value, duration, relative);


            // R E C T   T R A N S F O R M
            public static Value<Vector2> TweenAnchoredPosition(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(() => transform.anchoredPosition, target, value => transform.anchoredPosition = value, duration, relative);
            public static Value<Vector3> TweenAnchoredPosition3D(this RectTransform transform, Vector3 target, float duration, bool relative = false) => Value(() => transform.anchoredPosition3D, target, value => transform.anchoredPosition3D = value, duration, relative);
            public static Value<Vector2> TweenSizeDelta(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(() => transform.sizeDelta, target, value => transform.sizeDelta = value, duration, relative);


            // G R A P H I C
            public static Value<float> TweenFade(this Graphic graphic, float target, float duration, bool relative = false) => Value(() => graphic.color.a, target, value => { var color = graphic.color; color.a = value; graphic.color = color; }, duration, relative);
            public static Value<Color> TweenColor(this Graphic graphic, Color target, float duration, bool relative = false) => Value(() => graphic.color, () => target, value => graphic.color = value, duration, relative);
            public static Value<float> TweenFillAmount(this Image image, float target, float duration, bool relative = false) => Value(() => image.fillAmount, target, value => image.fillAmount = value, duration, relative);


            // T E X T
            public static Value<float> TweenText(this TMP_Text text, string content, float duration)
            {
                  if (string.IsNullOrEmpty(content))
                  {
                        Log.Error("Cannot tween to null or empty content", text);
                        return Value<float>.Blank;
                  }
                  return ValueClamped(() => 0F, content.Length, value => { int count = Mathf.FloorToInt(value); text.text = count == 0 ? string.Empty : content[..count]; }, duration);
            }
            public static Value<float> TweenFontSize(this TMP_Text text, float target, float duration, bool relative = false) => Value(() => text.fontSize, target, value => text.fontSize = value, duration, relative);
            public static Value<float> TweenNumber(this TMP_Text text, float target, float duration, string format, bool relative = false) => Value(() => float.TryParse(text.text, out float value) ? value : 0F, target, value => text.text = value.ToString(format), duration, relative);
      }
}