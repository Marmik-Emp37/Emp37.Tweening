using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Emp37.Tweening
{
      using static Tweens;

      public static class UIExtensions
      {
            // C A N V A S   G R O U P
            public static Value<float> TweenFade(this CanvasGroup group, float target, float duration) => Value(() => group.alpha, () => target, duration, value => group.alpha = value);


            // R E C T   T R A N S F O R M
            public static Value<Vector2> TweenAnchoredPosition(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(() => transform.anchoredPosition, () => relative ? transform.anchoredPosition + target : target, duration, value => transform.anchoredPosition = value);
            public static Value<Vector3> TweenAnchoredPosition3D(this RectTransform transform, Vector3 target, float duration, bool relative = false) => Value(() => transform.anchoredPosition3D, () => relative ? transform.anchoredPosition3D + target : target, duration, value => transform.anchoredPosition3D = value);
            public static Value<Vector2> TweenSizeDelta(this RectTransform transform, Vector2 target, float duration, bool relative = false) => Value(() => transform.sizeDelta, () => relative ? transform.sizeDelta + target : target, duration, value => transform.sizeDelta = value);


            // G R A P H I C
            public static Value<float> TweenFade(this Graphic graphic, float target, float duration) => Value(() => graphic.color.a, () => target, duration, value => { var color = graphic.color; color.a = value; graphic.color = color; });
            public static Value<Color> TweenColor(this Graphic graphic, Color target, float duration) => Value(() => graphic.color, () => target, duration, value => graphic.color = value);
            public static Value<float> TweenFillAmount(this Image image, float target, float duration) => Value(() => image.fillAmount, () => target, duration, value => image.fillAmount = value);


            // T E X T
            public static Value<float> TweenText(this TMP_Text text, string content, float duration)
            {
                  if (string.IsNullOrEmpty(content))
                  {
                        Log.Error("Cannot tween to null or empty content", text);
                        return Value<float>.Blank;
                  }
                  return ValueClamped(() => 0F, () => content.Length, duration, value => { int count = Mathf.FloorToInt(value); text.text = count == 0 ? string.Empty : content[..count]; });
            }
            public static Value<float> TweenFontSize(this TMP_Text text, float target, float duration, bool relative = false) => Value(() => text.fontSize, () => relative ? text.fontSize + target : target, duration, value => text.fontSize = value);
            public static Value<float> TweenNumber(this TMP_Text text, float target, float duration, string format, bool relative = false) => Value(() => float.TryParse(text.text, out float value) ? value : 0F, () => relative ? (float.TryParse(text.text, out float value) ? value : 0F) + target : target, duration, value => text.text = value.ToString(format));
      }
}