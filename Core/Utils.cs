using System;

using UnityEngine;

namespace Emp37.Tweening
{
      internal static class Utils
      {
            internal static void SafeInvoke(Action callback)
            {
                  try
                  {
                        callback?.Invoke();
                  }
                  catch (Exception ex)
                  {
                        Log.Exception(ex);
                  }
            }
            internal static void SafeInvoke<T>(Action<T> callback, T argument)
            {
                  try
                  {
                        callback?.Invoke(argument);
                  }
                  catch (Exception ex)
                  {
                        Log.Exception(ex);
                  }
            }
            internal static T SafeInvoke<T>(Func<T> callback)
            {
                  try
                  {
                        return callback();
                  }
                  catch (Exception ex)
                  {
                        Log.Exception(ex);
                        return default;
                  }
            }

            internal static string Info(IElement element, params string[] properties) => $"{element.GetType().Name} [Tag: {element.Tag ?? "None"} | Phase: {element.Phase}" + (properties != null && properties.Length > 0 ? " | " + string.Join(" | ", properties) : string.Empty) + "]";
      }
}