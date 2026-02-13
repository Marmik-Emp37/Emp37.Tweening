using System;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      public static class TweenExtensions
      {
            public static T SetTag<T>(this T tween, string tag) where T : Tween
            {
                  tween.tag = tag;
                  return tween;
            }
            public static T SetTimeMode<T>(this T tween, Delta timeMode) where T : Tween
            {
                  tween.timeMode = timeMode;
                  return tween;
            }
            public static T SetAutoKill<T>(this T tween, bool value) where T : Tween
            {
                  tween.options = value ? tween.options | Options.AutoKill : tween.options & ~Options.AutoKill;
                  return tween;
            }
            public static T SetLink<T>(this T tween, UObject link) where T : Tween
            {
                  tween.options = link != null ? tween.options | Options.Link : tween.options & ~Options.Link;
                  tween.linkedTarget = link;
                  return tween;
            }
            public static T SetLoops<T>(this T tween, int count, Loop.Type mode) where T : Tween
            {
                  tween.loop = new(count, mode);
                  return tween;
            }
            public static T SetRecyclable<T>(this T tween, bool value) where T : Tween
            {
                  tween.options = value ? tween.options | Options.Recycle : tween.options & ~Options.Recycle;
                  return tween;
            }
            public static T Pause<T>(this T tween) where T : Tween
            {
                  tween.Pause();
                  return tween;
            }
            public static T Resume<T>(this T tween) where T : Tween
            {
                  tween.Resume();
                  return tween;
            }

            public static T OnStart<T>(this T tween, Action callback) where T : Tween
            {
                  tween.onStart = callback;
                  return tween;
            }
            public static T OnUpdate<T>(this T tween, Action callback) where T : Tween
            {
                  tween.onUpdate = callback;
                  return tween;
            }
            public static T OnStepComplete<T>(this T tween, Action callback) where T : Tween
            {
                  tween.onStepComplete = callback;
                  return tween;
            }
            public static T OnComplete<T>(this T tween, Action callback) where T : Tween
            {
                  tween.onComplete = callback;
                  return tween;
            }
            public static T OnKill<T>(this T tween, Action callback) where T : Tween
            {
                  tween.onKill = callback;
                  return tween;
            }
      }
}