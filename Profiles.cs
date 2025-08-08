using UnityEngine;

namespace Emp37.Tweening
{
      public static class Profiles
      {
            private static readonly Keyframe zero = new(0F, 0F), one = new(1F, 1F), exit = new(1F, 0F);


            private static readonly Keyframe[] springKeys = {
                  zero, new(0.3F, 1.3F), new(0.6F, 0.8F), new(0.8F, 1.05F), one 
            };
            public static AnimationCurve Spring => new(springKeys);

            private static readonly Keyframe[] anticipateKeys = { 
                  zero, new(0.3F, -0.3F), one
            };
            public static AnimationCurve Anticipate => new(anticipateKeys);

            private static readonly Keyframe[] popKeys = {
                  zero, new(0.6F, 0.05F, 0.25F, 0.75F), new(0.85F, 0.9F, 1.25F, 1.25F), one
            };
            public static AnimationCurve Pop => new(popKeys);

            private static readonly Keyframe[] punchKeys = {
                  zero, new(0.1F, 1F), new(0.25F, -0.6F), new(0.5F, 0.4F), new(0.7F, -0.2F), exit
            };
            public static AnimationCurve Punch => new(punchKeys);

            private static readonly Keyframe[] shakeKeys = {
                  zero, new(0.1F, 0.5F), new(0.2F, -0.5F), new(0.3F, 0.4F), new(0.4F, -0.4F), new(0.5F, 0.3F), new(0.6F, -0.3F), new(0.7F, 0.2F), new(0.8F, -0.2F), new(0.9F, 0.1F), exit
            };
            public static AnimationCurve Shake => new(shakeKeys);

            private static readonly Keyframe[] snappyKeys = {
                  zero, new(0.3F, 1.05F, 0.75F, 0.75F), new(0.6F, 0.95F), one
            };
            public static AnimationCurve Snappy => new(snappyKeys);
      }
}