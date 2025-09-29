using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public static class Log
      {
            public static bool Enabled { get; set; } = true;

            public static void Info(object message) { if (Enabled) Debug.Log(message); }
            public static void Warning(object message) { if (Enabled) Debug.LogWarning(message); }
            public static void Error(object message) { if (Enabled) Debug.LogError(message); }
            public static void Exception(Exception exception) { if (Enabled) Debug.LogException(exception); }

            internal static void RejectTween(string message) => Warning($"Tween creation failed: {message}");
            internal static string Summarize(this ITween tween, string extraInfo) => $"{tween.GetType().Name}: [Tag: {tween.Tag ?? "None"} | Phase: {tween.Phase}]\n[{extraInfo}]";
      }
}