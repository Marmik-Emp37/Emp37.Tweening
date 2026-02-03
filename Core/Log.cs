using System;
using System.Diagnostics;

using UObject = UnityEngine.Object;
using static UnityEngine.Debug;

namespace Emp37.Tweening
{
      public static class Log
      {
            public static bool Enabled { get; set; } = true;


            private const string UNITY_EDITOR = "UNITY_EDITOR";
            [Conditional(UNITY_EDITOR)] public static void Info(object message, UObject context = null) { if (Enabled) Log(message, context); }
            [Conditional(UNITY_EDITOR)] public static void Warning(object message, UObject context = null) { if (Enabled) LogWarning(message, context); }
            [Conditional(UNITY_EDITOR)] public static void Error(object message, UObject context = null) { if (Enabled) LogError(message, context); }
            [Conditional(UNITY_EDITOR)] public static void Exception(this Tween tween, Exception exception) { if (Enabled) LogException(exception); }
      }
}