using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public partial class Factory
      {
            private class Reservoir
            {
                  private static readonly string name = $"[{typeof(Reservoir).FullName}]";

                  private int limit;
                  private IElement[] elements;

                  public int ActiveTweens { get; private set; }
                  public int Capacity
                  {
                        get => limit;
                        set
                        {
                              Compact();
                              limit = Mathf.Max(value, ActiveTweens);
                              if (limit == ActiveTweens)
                              {
                                    Debug.LogWarning($"{name} Requested capacity {value} is below the active tween count ({limit}). Using {limit} to prevent data loss.");
                              }
                              Array.Resize(ref elements, limit);
                        }
                  }

                  public Reservoir()
                  {
                        limit = 64;
                        elements = new IElement[limit];
                        ActiveTweens = 0;
                  }

                  public bool Add(IElement item)
                  {
                        if (ActiveTweens >= limit)
                        {
                              Debug.LogWarning($"{name} Tween limit reached. Increase {nameof(Capacity)} if more concurrent tweens are needed.");
                              return false;
                        }
                        elements[ActiveTweens++] = item;
                        return true;
                  }
                  public void Remove(int index)
                  {
                        if ((uint) index >= (uint) ActiveTweens) return;

                        elements[index] = elements[--ActiveTweens];
                        elements[ActiveTweens] = null;
                  }
                  public void Clear()
                  {
                        Array.Clear(elements, 0, ActiveTweens);
                        ActiveTweens = 0;
                  }
                  public void Iterate(Action<IElement> action)
                  {
                        for (int i = ActiveTweens - 1; i > -1; i--)
                        {
                              action(elements[i]);
                        }
                  }
                  public void Compact()
                  {
                        if (ActiveTweens == 0) return;

                        int count = 0;
                        for (int i = count; i < ActiveTweens; i++)
                        {
                              IElement element = elements[i];
                              if (element.Phase != Phase.None && i != count)
                              {
                                    elements[count++] = element;
                              }
                        }
                        for (int i = count; i < ActiveTweens; i++)
                        {
                              elements[i] = null;
                        }
                        ActiveTweens = count;
                  }

                  public IElement this[int index] => elements[index];
            }

            private static readonly Reservoir tweens = new();
            public static int MaxTweens { get => tweens.Capacity; set => tweens.Capacity = value; }
            public int AvailableTweens => MaxTweens - tweens.ActiveTweens;

            private void LateUpdate()
            {
                  for (int i = tweens.ActiveTweens - 1; i >= 0; i--)
                  {
                        IElement element = tweens[i];
                        element.Update();
                        if (element.Phase is Phase.Complete or Phase.None)
                        {
                              tweens.Remove(i);
                        }
                  }
                  if (tweens.ActiveTweens == 0)
                  {
                        enabled = false;
                  }
            }

            public static void Add(IElement tween)
            {
                  if (!Application.isPlaying || tween.IsEmpty) return;
                  if (instance == null)
                  {
                        Initialize();
                  }
                  if (tweens.Add(tween)) instance.enabled = true;
            }
            public static void PauseTweens() => tweens.Iterate(item => item.Pause());
            public static void ResumeTweens() => tweens.Iterate(item => item.Resume());
            public static void KillTweens() => tweens.Iterate(element => element.Kill());

            public static Handle<T> Create<T>(Func<T> onInitialize, T target, float duration, Action<T> onValueChange, Handle<T>.Evaluator evaluate) where T : struct
            {
                  bool isValid = true;

                  if (duration <= 0F) { warn($"Invalid duration: {duration}. Duration must be greater than 0 seconds to perform a tween."); isValid = false; }
                  if (onInitialize == null) { warn($"Missing required delegate: {nameof(onInitialize)}. This delegate provides the starting value for the tween and must not be null."); isValid = false; }
                  if (onValueChange == null) { warn($"Missing required delegate: {nameof(onValueChange)}. This delegate applies the interpolated value each frame and must not be null."); isValid = false; }

                  return isValid ? new Handle<T>(onInitialize, target, duration, onValueChange, evaluate) : Handle<T>.Empty;

                  static void warn(string message) => Debug.LogWarning($"{nameof(Handle<T>)} creation failed: {message}");
            }
            public static Handle<float> Create(Func<float> onInitialize, float target, float duration, Action<float> onValueChange) => Create(onInitialize, target, duration, onValueChange, Mathf.LerpUnclamped);
            public static Handle<Vector2> Create(Func<Vector2> onInitialize, Vector2 target, float duration, Action<Vector2> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector2.LerpUnclamped);
            public static Handle<Vector3> Create(Func<Vector3> onInitialize, Vector3 target, float duration, Action<Vector3> onValueChange) => Create(onInitialize, target, duration, onValueChange, Vector3.LerpUnclamped);
            public static Handle<Quaternion> Create(Func<Quaternion> onInitialize, Quaternion target, float duration, Action<Quaternion> onValueChange) => Create(onInitialize, target, duration, onValueChange, Quaternion.LerpUnclamped);
            public static Handle<Color> Create(Func<Color> onInitialize, Color target, float duration, Action<Color> onValueChange) => Create(onInitialize, target, duration, onValueChange, Color.LerpUnclamped);
      }
}