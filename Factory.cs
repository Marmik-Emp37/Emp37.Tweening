using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Tweening
{
      public class Factory : MonoBehaviour
      {
            private static Factory instance;

            private static readonly List<IElement> tweens = new();


            private void Awake()
            {
                  if (instance != null && instance != this)
                  {
                        Destroy(gameObject);
                        return;
                  }
                  DontDestroyOnLoad(gameObject);
                  enabled = false;
            }
            private void LateUpdate()
            {
                  int count = tweens.Count;
                  for (int i = count - 1; i >= 0; i--)
                  {
                        IElement element = tweens[i];
                        element.Update();
                        if (element.Phase is Phase.Complete or Phase.None)
                        {
                              tweens.RemoveAt(i);
                        }
                  }
                  if (count == 0)
                  {
                        enabled = false;
                  }
            }
            private void OnDestroy()
            {
                  if (instance == this) instance = null;
            }

            public static Tween<T> Create<T>(Func<T> onInitialize, T target, float duration, Action<T> onValueChange, Tween<T>.Evaluator evaluate) where T : struct
            {
                  bool isValid = true;
                  if (duration <= 0F) { log($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (onInitialize == null) { log($"Missing required delegate: {nameof(onInitialize)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (onValueChange == null) { log($"Missing required delegate: {nameof(onValueChange)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }
                  return isValid ? new Tween<T>(onInitialize, target, duration, onValueChange, evaluate) : Tween<T>.Empty;

                  static void log(string message) => Debug.LogWarning($"{nameof(Tween<T>)} creation failed: {message}");
            }
            public static Tween<float> Create(Func<float> onInitialize, float target, float duration, Action<float> onValueChange) => Create(onInitialize, target, duration, onValueChange, Mathf.LerpUnclamped);
            public static Tween<Vector2> Create(Func<Vector2> onInitialize, Vector2 target, float duration, Action<Vector2> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector2.LerpUnclamped);
            public static Tween<Vector3> Create(Func<Vector3> onInitialize, Vector3 target, float duration, Action<Vector3> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector3.LerpUnclamped);
            public static Tween<Quaternion> Create(Func<Quaternion> onInitialize, Quaternion target, float duration, Action<Quaternion> onValueChange) => Create(onInitialize, target, duration, onValueChange, Quaternion.LerpUnclamped);
            public static Tween<Color> Create(Func<Color> onInitialize, Color target, float duration, Action<Color> onValueChange) => Create(onInitialize, target, duration, onValueChange, Color.LerpUnclamped);

            public static bool Add(IElement tween)
            {
                  if (!Application.isPlaying || tween.IsEmpty) return false;

                  if (instance == null) instance = new GameObject(nameof(Factory)).AddComponent<Factory>();

                  tweens.Add(tween);
                  instance.enabled = true;

                  return true;
            }
            public static void KillAll() => tweens.ForEach(item => item.Kill());
            public static void PauseAll() => tweens.ForEach(item => item.Pause());
            public static void ResumeAll() => tweens.ForEach(item => item.Resume());
      }
}