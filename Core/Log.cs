using System;

using UnityEngine;

using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      public static class Log
      {
            public static bool Enabled { get; set; } = true;

            public static void Info(string message) { if (Enabled) Debug.Log(message); }
            public static void Warning(string message) { if (Enabled) Debug.LogWarning(message); }
            public static void Exception(Exception exception) { if (Enabled) Debug.LogException(exception); }

            public static void Info(string message, UObject context) { if (Enabled) Debug.Log(message, context); }
            public static void Warning(string message, UObject context) { if (Enabled) Debug.LogWarning(message, context); }
            public static void Exception(Exception exception, UObject context) { if (Enabled) Debug.LogException(exception, context); }

            internal static void RejectTween(string message) => Warning($"Tween creation failed: {message}");
            internal static string Summarize(IElement element, params string[] properties)
            {
                  return $"{element.GetType().Name} [Tag: {element.Tag ?? "None"} | Phase: {element.Phase} {(properties != null && properties.Length > 0 ? " | " + string.Join(" | ", properties) : string.Empty)}]";
            }
      }
}