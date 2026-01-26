using System;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace Emp37.Tweening
{
      public static class Log
      {
            private const string UNITY_EDITOR = "UNITY_EDITOR";

            public static bool Enabled { get; set; } = true;

            [Conditional(UNITY_EDITOR)] internal static void Info(object message) { if (Enabled) Debug.Log(message); }
            [Conditional(UNITY_EDITOR)] internal static void Warning(object message) { if (Enabled) Debug.LogWarning(message); }
            [Conditional(UNITY_EDITOR)] internal static void Error(object message) { if (Enabled) Debug.LogError(message); }
            [Conditional(UNITY_EDITOR)] internal static void Exception(Exception exception) { if (Enabled) Debug.LogException(exception); }
            [Conditional(UNITY_EDITOR)] internal static void RejectTween(string message) => Warning($"Tween creation failed: {message}");
      }
}