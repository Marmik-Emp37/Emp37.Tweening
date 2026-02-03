using System.Collections.Generic;
using static System.MathF;

using UnityEngine;

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

            public static readonly IReadOnlyDictionary<Type, Method> TypeMap = new Dictionary<Type, Method>
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
                  { Type.InOutBounce, InOutBounce },
            };


            public delegate float Method(float t);

            #region P E N N E R ' S   E A S I N G   N O T A T I O N S
            /// <summary>
            /// Represents the default overshoot value used in Back easing functions.
            /// </summary>
            /// <remarks>
            /// This constant is typically used to control the amount by which the Back easing function exceeds its target value before settling. 
            /// Adjusting this value changes the intensity of the overshoot effect.
            /// </remarks>
            public const float S = 1.70158F;

            /// <summary>
            /// Represents the adjusted overshoot constant used for smoother InOutBack easing transitions.
            /// </summary>
            /// <remarks>
            /// This constant is typically used in easing functions to control the overshoot behavior during the InOutBack transition, resulting in a smoother animation curve.
            /// </remarks>
            public const float C2 = S * 1.525F;

            /// <summary>
            /// Represents the amplified overshoot constant used in the InBack and OutBack easing functions.
            /// </summary>
            /// <remarks>
            /// This constant is typically used to control the degree of overshoot in back easing calculations.
            /// Adjusting its value affects how far the animation exceeds its target before settling.
            /// </remarks>
            public const float C3 = S + 1F;

            /// <summary>
            /// Represents the angular frequency used for InElastic and OutElastic easing functions, which controls the number of oscillations during the transition.
            /// </summary>
            /// <remarks>
            /// This constant is typically used to adjust the elasticity effect in animation curves.
            /// Modifying its value changes how many times the animation oscillates before settling.
            /// </remarks>
            public const float C4 = 2F * PI / 3F;

            /// <summary>
            /// Represents the angular frequency constant used for the InOutElastic easing function.
            /// </summary>
            /// <remarks>
            /// A higher value results in faster oscillation when applying the InOutElastic easing.
            /// This constant is typically used to control the frequency of elastic animations.
            /// </remarks>
            public const float C5 = 2F * PI / 4.5F;

            /// <summary>
            /// Represents the bounce scaling factor used to control the height and shape of bounce animations.
            /// </summary>
            public const float N1 = 7.5625F;

            /// <summary>
            /// Represents the constant used to divide the bounce phase into time intervals for the OutBounce easing function.
            /// </summary>
            /// <remarks>
            /// This value is typically used in animation calculations to segment the bounce effect into distinct intervals, enabling accurate timing and progression of the bounce phase.
            /// </remarks>
            public const float D1 = 2.75F;
            #endregion

            #region E A S I N G   M E T H O D S
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
            #endregion

            public static class Curves
            {
                  private static readonly Keyframe Zero = new(0F, 0F), One = new(1F, 1F), Exit = new(1F, 0F);

                  /// <summary>
                  /// Starts by slightly moving backward before accelerating forward to the target value.
                  /// Creates a sense of anticipation before the main motion.
                  /// </summary>
                  public static AnimationCurve Anticipate => new(Zero, new(0.3F, -0.3F), One);

                  /// <summary>
                  /// Quickly scales up with a slight overshoot and settles back to 1.
                  /// Great for UI pop-in or emphasis effects.
                  /// </summary>
                  public static AnimationCurve Pop => new(Zero, new(0.6F, 0.05F, 0.25F, 0.75F), new(0.85F, 0.9F, 1.25F, 1.25F), One);

                  /// <summary>
                  /// Sharp initial impact followed by diminishing oscillations that settle to zero.
                  /// Ideal for impact, hit, or recoil effects.
                  /// </summary>
                  public static AnimationCurve Punch => new(Zero, new(0.1F, 1F), new(0.25F, -0.6F), new(0.5F, 0.4F), new(0.7F, -0.2F), Exit);

                  /// <summary>
                  /// Rapid alternating oscillations that decrease over time and end at zero.
                  /// Suitable for shake or vibration effects.
                  /// </summary>
                  public static AnimationCurve Shake => new(Zero, new(0.1F, 0.5F), new(0.2F, -0.5F), new(0.3F, 0.4F), new(0.4F, -0.4F), new(0.5F, 0.3F), new(0.6F, -0.3F), new(0.7F, 0.2F), new(0.8F, -0.2F), new(0.9F, 0.1F), Exit);

                  /// <summary>
                  /// Fast, responsive motion with a small overshoot and quick settle.
                  /// Produces a crisp and snappy transition.
                  /// </summary>
                  public static AnimationCurve Snappy => new(Zero, new(0.3F, 1.05F, 0.75F, 0.75F), new(0.6F, 0.95F), One);

                  /// <summary>
                  /// Smooth spring-like motion with noticeable overshoot and soft settling.
                  /// Emulates elastic or bouncy movement.
                  /// </summary>
                  public static AnimationCurve Spring => new(Zero, new(0.3F, 1.3F), new(0.6F, 0.8F), new(0.8F, 1.05F), One);
            }
      }
}