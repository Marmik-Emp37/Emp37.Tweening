using System;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      public static class TweenExtensions
      {
            public static T SetDelay<T>(this T tween, float delay) where T : Tween { tween.setDelay(delay); return tween; }
            public static T SetTag<T>(this T tween, string tag) where T : Tween { tween.setTag(tag); return tween; }
            public static T SetTimeMode<T>(this T tween, Delta timeMode) where T : Tween { tween.setTimeMode(timeMode); return tween; }
            public static T SetAutoKill<T>(this T tween, bool value) where T : Tween { tween.setAutoKill(value); return tween; }
            public static T SetLink<T>(this T tween, UObject link) where T : Tween { tween.setLink(link); return tween; }
            public static T SetLoops<T>(this T tween, int count, LoopType mode) where T : Tween { tween.setLooping(count, mode); return tween; }
            public static T SetRecyclable<T>(this T tween, bool value) where T : Tween { tween.setRecyclable(value); return tween; }
            public static T Pause<T>(this T tween) where T : Tween { tween.Pause(); return tween; }
            public static T Resume<T>(this T tween) where T : Tween { tween.Resume(); return tween; }
            public static T OnStart<T>(this T tween, Action callback) where T : Tween { tween.setOnStart(callback); return tween; }
            public static T OnUpdate<T>(this T tween, Action callback) where T : Tween { tween.setOnUpdate(callback); return tween; }
            public static T OnRewind<T>(this T tween, Action callback) where T : Tween { tween.setOnRewind(callback); return tween; }
            public static T OnLoopComplete<T>(this T tween, Action callback) where T : Tween { tween.setOnLoopComplete(callback); return tween; }
            public static T OnComplete<T>(this T tween, Action callback) where T : Tween { tween.setOnComplete(callback); return tween; }
            public static T OnKill<T>(this T tween, Action callback) where T : Tween { tween.setOnKill(callback); return tween; }
      }
}