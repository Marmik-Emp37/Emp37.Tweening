using System;

using UnityEngine;

namespace Emp37.Tweening
{
      public partial class Factory
      {
            private class Reservoir
            {
                  private static readonly string logPrefix = $"[{typeof(Reservoir).FullName}]";

                  private int _capacity;
                  private IElement[] elements;

                  public int ActiveTweens { get; private set; }
                  public int Capacity
                  {
                        get => _capacity;
                        set
                        {
                              Compact();
                              int limit = Mathf.Max(value, ActiveTweens);
                              if (limit != _capacity)
                              {
                                    if (limit == ActiveTweens)
                                    {
                                          Debug.LogWarning($"{logPrefix} Requested capacity {value} is below the active tween count ({limit}). Using {limit} to prevent data loss.");
                                    }
                                    Array.Resize(ref elements, limit);
                                    _capacity = limit;
                              }
                        }
                  }

                  public Reservoir(int initialCapacity = 64)
                  {
                        _capacity = Mathf.Max(1, initialCapacity);
                        elements = new IElement[_capacity];
                  }

                  public bool Add(IElement item)
                  {
                        if (ActiveTweens >= _capacity)
                        {
                              Debug.LogWarning($"{logPrefix} Tween limit reached. Increase {nameof(Capacity)} if more concurrent tweens are needed.");
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

                        int writeIndex = 0;
                        for (int readIndex = 0; readIndex < ActiveTweens; readIndex++)
                        {
                              IElement element = elements[readIndex];
                              if (element.Phase != Phase.None && readIndex != writeIndex)
                              {
                                    elements[writeIndex++] = element;
                              }
                        }
                        for (int i = writeIndex; i < ActiveTweens; i++)
                        {
                              elements[i] = null;
                        }
                        ActiveTweens = writeIndex;
                  }

                  public IElement this[int index] => elements[index];
            }

            private static readonly Reservoir elements = new();
            public static int MaxTweens { get => elements.Capacity; set => elements.Capacity = value; }
            public int AvailableTweens => MaxTweens - elements.ActiveTweens;

            private void LateUpdate()
            {
                  for (int i = elements.ActiveTweens - 1; i >= 0; i--)
                  {
                        var element = elements[i];
                        element.Update();
                        if (element.Phase is Phase.Complete or Phase.None)
                        {
                              elements.Remove(i);
                        }
                  }
                  if (elements.ActiveTweens == 0)
                  {
                        enabled = false;
                  }
            }

            public static void Add(IElement element)
            {
                  if (Application.isPlaying && !element.IsEmpty && elements.Add(element))
                  {
                        instance.enabled = true;
                  }
            }
            public static void Pause() => elements.Iterate(e => e.Pause());
            public static void Resume() => elements.Iterate(e => e.Resume());
            public static void Kill() => elements.Iterate(e => e.Kill());
      }
}