using UnityEngine;

namespace Emp37.Tweening
{
      public static class Logger
      {
            public static bool Enabled { get; set; } = true;

            public static void Info(string message) { if (Enabled) Debug.Log(message); }
            public static void Warning(string message) { if (Enabled) Debug.LogWarning(message); }
            public static void RejectTween(string message) => Warning($"Tween creation failed: {message}");
      }
}