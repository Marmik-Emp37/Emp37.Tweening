using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public static class Log
      {
            public static bool Enabled { get; set; } = true;

            public static void Info(string message) { if (Enabled) Debug.Log(message); }
            public static void Warning(string message) { if (Enabled) Debug.LogWarning(message); }
            public static void Exception(Exception exception) { if (Enabled) Debug.LogException(exception); }

            internal static string ElementInfo(IElement element, params string[] properties) => $"{element.GetType().Name} [Tag: {element.Tag ?? "None"} | Phase: {element.Phase}" + (properties != null && properties.Length > 0 ? " | " + string.Join(" | ", properties) : string.Empty) + "]";
            internal static void InvalidTween(string message) => Warning($"Tween creation failed: {message}");
      }
}