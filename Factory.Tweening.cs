using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public sealed partial class Factory : MonoBehaviour
      {
            private class Reservoir
            {
                  private int _capacity;
                  private IElement[] elements;

                  public int ActiveCount { get; private set; }
                  public int Capacity
                  {
                        get => _capacity;
                        set
                        {
                              Compact();

                              int limit = Mathf.Max(value, ActiveCount);
                              if (limit == _capacity) return;
                              if (limit == ActiveCount) Debug.LogWarning($"Requested capacity {value} is below the active tween count ({limit}). Using {limit} to prevent data loss.");

                              Array.Resize(ref elements, limit);
                              _capacity = limit;
                        }
                  }

                  public Reservoir(int capacity) => elements = new IElement[_capacity = Mathf.Max(1, capacity)];

                  public bool Add(IElement item)
                  {
                        bool value = ActiveCount < Capacity;
                        if (value) elements[ActiveCount++] = item;
                        return value;
                  }
                  public void Remove(int index)
                  {
                        if ((uint) index >= (uint) ActiveCount) return;

                        elements[index] = elements[--ActiveCount];
                        elements[ActiveCount] = null;
                  }
                  public void Clear()
                  {
                        Array.Clear(elements, 0, ActiveCount);
                        ActiveCount = 0;
                  }
                  public void Iterate(Action<IElement> action)
                  {
                        for (int i = ActiveCount - 1; i > -1; i--)
                        {
                              action(elements[i]);
                        }
                  }
                  public void Compact()
                  {
                        if (ActiveCount == 0) return;

                        int writeIndex = 0;
                        for (int readIndex = 0; readIndex < ActiveCount; readIndex++)
                        {
                              IElement element = elements[readIndex];
                              if (element.Phase != Phase.None && readIndex != writeIndex)
                              {
                                    elements[writeIndex++] = element;
                              }
                        }
                        for (int i = writeIndex; i < ActiveCount; i++)
                        {
                              elements[i] = null;
                        }
                        ActiveCount = writeIndex;
                  }

                  public IElement this[int index] => elements[index];
            }

            private static readonly Reservoir tweens = new(64);
            public static int MaxTweens { get => tweens.Capacity; set => tweens.Capacity = value; }
            public int AvailableTweens => MaxTweens - tweens.ActiveCount;

            private void LateUpdate()
            {
                  for (int i = tweens.ActiveCount - 1; i >= 0; i--)
                  {
                        var element = tweens[i];
                        element.Update();
                        if (element.Phase is Phase.Complete or Phase.None)
                        {
                              tweens.Remove(i);
                        }
                  }
                  if (tweens.ActiveCount == 0)
                  {
                        enabled = false;
                  }
            }

            public static void Add(IElement element)
            {
                  if (!Application.isPlaying || element.IsEmpty) return;

                  if (tweens.Add(element))
                  {
                        instance.enabled = true;
                  }
                  else
                  {
                        Debug.LogWarning($"[{typeof(Factory).FullName}] Active tween limit ({MaxTweens}) reached. Increase {nameof(MaxTweens)}.");
                  }
            }
            public static void Pause() => tweens.Iterate(e => e.Pause());
            public static void Resume() => tweens.Iterate(e => e.Resume());
            public static void Kill() => tweens.Iterate(e => e.Kill());
      }
}