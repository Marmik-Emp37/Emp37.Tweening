using UnityEngine;

using static System.MathF;

namespace Emp37.Tweening
{
      public static class Ease
      {
            public enum Type
            {
                  Linear,
                  InSine,
                  OutSine,
                  InOutSine,
                  InCubic,
                  OutCubic,
                  InOutCubic,
                  InQuint,
                  OutQuint,
                  InOutQuint,
                  InCirc,
                  OutCirc,
                  InOutCirc,
                  InElastic,
                  OutElastic,
                  InOutElastic,
                  InQuad,
                  OutQuad,
                  InOutQuad,
                  InQuart,
                  OutQuart,
                  InOutQuart,
                  InExpo,
                  OutExpo,
                  InOutExpo,
                  InBack,
                  OutBack,
                  InOutBack,
                  InBounce,
                  OutBounce,
                  InOutBounce,
            }


            public delegate float Method(float t);


            // penner's Easing Notations
            public const float S = 1.70158F; // default overshoot for Back easing
            public const float C2 = S * 1.525F; // adjusted overshoot for smoother InOutBack transition
            public const float C3 = S + 1F; // amplified overshoot used in InBack and OutBack
            public const float C4 = 2F * PI / 3F; // angular frequency for InElastic and OutElastic (controls number of oscillations)
            public const float C5 = 2F * PI / 4.5F; // angular frequency for InOutElastic (higher value creates faster oscillation)
            public const float N1 = 7.5625F; // bounce scaling factor (controls bounce height and shape)
            public const float D1 = 2.75F; // bounce phase division (segments the bounce into time intervals for OutBounce).

            public static float Linear(float t) => t;
            public static float InSine(float t) => 1F - Cos(t * PI * 0.5F);
            public static float OutSine(float t) => Sin(t * PI * 0.5F);
            public static float InOutSine(float t) => (1F - Cos(PI * t)) * 0.5F;
            public static float InCubic(float t) => t * t * t;
            public static float OutCubic(float t)
            {
                  t -= 1F;
                  return t * t * t + 1F;
            }
            public static float InOutCubic(float t) => t < 0.5F ? 4F * t * t * t : 1F - Pow(-2F * t + 2F, 3F) * 0.5F;
            public static float InQuint(float t) => t * t * t * t * t;
            public static float OutQuint(float t)
            {
                  t -= 1F;
                  return t * t * t * t * t + 1F;
            }
            public static float InOutQuint(float t) => t < 0.5F ? 16F * t * t * t * t * t : 1F - Pow(-2F * t + 2F, 5F) * 0.5F;
            public static float InCirc(float t) => 1F - Sqrt(1F - t * t);
            public static float OutCirc(float t)
            {
                  t -= 1F;
                  return Sqrt(1F - t * t);
            }
            public static float InOutCirc(float t)
            {
                  if (t < 0.5F) return 0.5F * (1F - Sqrt(1F - 4F * t * t));
                  t = -2F * t + 2F;
                  return 0.5F * (Sqrt(1F - t * t) + 1F);
            }
            public static float InElastic(float t)
            {
                  if (t == 0F) return 0F; if (t == 1F) return 1F;
                  return -Pow(2F, 10F * t - 10F) * Sin((10F * t - 10.75F) * C4);
            }
            public static float OutElastic(float t)
            {
                  if (t == 0F) return 0F; if (t == 1F) return 1F;
                  return Pow(2F, -10F * t) * Sin((10F * t - 0.75F) * C4) + 1F;
            }
            public static float InOutElastic(float t)
            {
                  if (t == 0F) return 0F; if (t == 1F) return 1F;
                  if (t < 0.5F)
                  {
                        t = 20F * t - 10F;
                        return -0.5F * Pow(2F, t) * Sin((t - 1.125F) * C5);
                  }
                  else
                  {
                        t = 20F * t - 10F;
                        return 0.5F * Pow(2F, -t) * Sin((t - 1.125F) * C5) + 1F;
                  }
            }
            public static float InQuad(float t) => t * t;
            public static float OutQuad(float t) => t * (2F - t);
            public static float InOutQuad(float t) => t < 0.5F ? 2F * t * t : 1F - Pow(-2F * t + 2F, 2F) * 0.5F;
            public static float InQuart(float t) => t * t * t * t;
            public static float OutQuart(float t)
            {
                  t = 1F - t;
                  return 1F - t * t * t * t;
            }
            public static float InOutQuart(float t) => t < 0.5F ? (8F * t * t * t * t) : 1F - (Pow((-2F * t) + 2F, 4F) * 0.5F);
            public static float InExpo(float t) => t == 0F ? 0F : Pow(2F, 10F * (t - 1F));
            public static float OutExpo(float t) => t == 1F ? 1F : 1F - Pow(2F, -10F * t);
            public static float InOutExpo(float t)
            {
                  if (t == 0F) return 0F; if (t == 1F) return 1F;
                  return t < 0.5F ? Pow(2F, (20F * t) - 10F) * 0.5F : (2F - Pow(2F, -20F * t + 10F)) * 0.5F;
            }
            public static float InBack(float t)
            {
                  return C3 * t * t * t - S * t * t;
            }
            public static float OutBack(float t)
            {
                  t -= 1F;
                  return 1F + C3 * t * t * t + S * t * t;
            }
            public static float InOutBack(float t)
            {
                  if (t < 0.5F)
                  {
                        t *= 2F;
                        return 0.5F * (t * t * ((C2 + 1F) * t - C2));
                  }
                  else
                  {
                        t = 2F * t - 2F;
                        return 0.5F * (t * t * ((C2 + 1F) * t + C2) + 2F);
                  }
            }
            public static float InBounce(float t) => 1F - OutBounce(1F - t);
            public static float OutBounce(float t)
            {
                  if (t < 1F / D1) return N1 * t * t;
                  if (t < 2F / D1) return N1 * (t -= 1.5F / D1) * t + 0.75F;
                  if (t < 2.5F / D1) return N1 * (t -= 2.25F / D1) * t + 0.9375F;
                  return N1 * (t -= 2.625F / D1) * t + 0.984375F;
            }
            public static float InOutBounce(float t) => t < 0.5F ? (1F - OutBounce(1F - 2F * t)) * 0.5F : (1F + OutBounce(2F * t - 1F)) * 0.5F;


            public static class Curves
            {
                  private static readonly Keyframe Zero = new(0F, 0F), One = new(1F, 1F), Exit = new(1F, 0F);

                  private static readonly AnimationCurve
                        _anticipate = new(Zero, new(0.3F, -0.3F), One),
                        _pop = new(Zero, new(0.6F, 0.05F, 0.25F, 0.75F), new(0.85F, 0.9F, 1.25F, 1.25F), One),
                        _punch = new(Zero, new(0.1F, 1F), new(0.25F, -0.6F), new(0.5F, 0.4F), new(0.7F, -0.2F), Exit),
                        _shake = new(Zero, new(0.1F, 0.5F), new(0.2F, -0.5F), new(0.3F, 0.4F), new(0.4F, -0.4F), new(0.5F, 0.3F), new(0.6F, -0.3F), new(0.7F, 0.2F), new(0.8F, -0.2F), new(0.9F, 0.1F), Exit),
                        _snappy = new(Zero, new(0.3F, 1.05F, 0.75F, 0.75F), new(0.6F, 0.95F), One),
                        _spring = new(Zero, new(0.3F, 1.3F), new(0.6F, 0.8F), new(0.8F, 1.05F), One);

                  public static AnimationCurve Anticipate => _anticipate;
                  public static AnimationCurve Pop => _pop;
                  public static AnimationCurve Punch => _punch;
                  public static AnimationCurve Shake => _shake;
                  public static AnimationCurve Snappy => _snappy;
                  public static AnimationCurve Spring => _spring;
            }
      }
}