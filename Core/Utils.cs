using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public static class Utils
      {
            public static void SafeInvoke(Action callback)
            {
                  try
                  {
                        callback?.Invoke();
                  }
                  catch (Exception ex)
                  {
                        Debug.LogException(ex);
                  }
            }
            public static void SafeInvoke<T>(Action<T> callback, T argument)
            {
                  try
                  {
                        callback?.Invoke(argument);
                  }
                  catch (Exception ex)
                  {
                        Debug.LogException(ex);
                  }
            }
      }
}