using System.Collections.Generic;

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

            public static readonly IReadOnlyDictionary<Type, Function> TypeMap = new Dictionary<Type, Function>
            {
                  { Type.Linear, Linear },
                  { Type.InSine, InSine },
                  { Type.OutSine, OutSine },
                  { Type.InOutSine, InOutSine },
                  { Type.InCubic, InCubic },
                  { Type.OutCubic, OutCubic },
                  { Type.InOutCubic, InOutCubic },
                  { Type.InQuint, InQuint },
                  { Type.OutQuint, OutQuint },
                  { Type.InOutQuint, InOutQuint },
                  { Type.InCirc, InCirc },
                  { Type.OutCirc, OutCirc },
                  { Type.InOutCirc, InOutCirc },
                  { Type.InQuad, InQuad },
                  { Type.OutQuad, OutQuad },
                  { Type.InOutQuad, InOutQuad },
                  { Type.InQuart, InQuart },
                  { Type.OutQuart, OutQuart },
                  { Type.InOutQuart, InOutQuart },
                  { Type.InExpo, InExpo },
                  { Type.OutExpo, OutExpo },
                  { Type.InOutExpo, InOutExpo },
                  { Type.InBack, InBack },
                  { Type.OutBack, OutBack },
                  { Type.InOutBack, InOutBack },
                  { Type.InElastic, InElastic },
                  { Type.OutElastic, OutElastic },
                  { Type.InOutElastic, InOutElastic },
                  { Type.InBounce, InBounce },
                  { Type.OutBounce, OutBounce },
                  { Type.InOutBounce, InOutBounce }
            };

            #region P E N N E R ' S   E A S I N G   N O T A T I O N S
            public delegate float Function(float t);

            /// <summary>
            /// Default overshoot amount for Back easing.
            /// </summary>
            public const float S = 1.70158F;
            /// <summary>
            /// Adjusted overshoot for smoother InOutBack transition.
            /// </summary>
            public const float C2 = S * 1.525F;
            /// <summary>
            /// Amplified overshoot used in InBack and OutBack formulas.
            /// </summary>
            public const float C3 = S + 1F;
            /// <summary>
            /// Angular frequency for InElastic and OutElastic easing functions (controls number of oscillations).
            /// </summary>
            public const float C4 = 2F * PI / 3F;
            /// <summary>
            /// Angular frequency for InOutElastic easing (higher value creates faster oscillation).
            /// </summary>
            public const float C5 = 2F * PI / 4.5F;
            /// <summary>
            /// Bounce scaling factor (controls bounce height and shape).
            /// </summary>
            public const float N1 = 7.5625F;
            /// <summary>
            /// Bounce phase division (segments the bounce into time intervals for OutBounce).
            /// </summary>
            public const float D1 = 2.75F;
            #endregion

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
                  t -= 1F;
                  return t * t * t * t + 1F;
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

                  private static readonly Keyframe[] anticipateKeys = { Zero, new(0.3F, -0.3F), One };
                  private static readonly Keyframe[] popKeys = { Zero, new(0.6F, 0.05F, 0.25F, 0.75F), new(0.85F, 0.9F, 1.25F, 1.25F), One };
                  private static readonly Keyframe[] punchKeys = { Zero, new(0.1F, 1F), new(0.25F, -0.6F), new(0.5F, 0.4F), new(0.7F, -0.2F), Exit };
                  private static readonly Keyframe[] shakeKeys = { Zero, new(0.1F, 0.5F), new(0.2F, -0.5F), new(0.3F, 0.4F), new(0.4F, -0.4F), new(0.5F, 0.3F), new(0.6F, -0.3F), new(0.7F, 0.2F), new(0.8F, -0.2F), new(0.9F, 0.1F), Exit };
                  private static readonly Keyframe[] snappyKeys = { Zero, new(0.3F, 1.05F, 0.75F, 0.75F), new(0.6F, 0.95F), One };
                  private static readonly Keyframe[] springKeys = { Zero, new(0.3F, 1.3F), new(0.6F, 0.8F), new(0.8F, 1.05F), One };

                  public static AnimationCurve Anticipate => new(anticipateKeys);
                  public static AnimationCurve Pop => new(popKeys);
                  public static AnimationCurve Punch => new(punchKeys);
                  public static AnimationCurve Shake => new(shakeKeys);
                  public static AnimationCurve Snappy => new(snappyKeys);
                  public static AnimationCurve Spring => new(springKeys);
            }
      }
}