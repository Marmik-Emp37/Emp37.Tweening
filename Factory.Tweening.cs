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
                        for (int i = 0; i < ActiveTweens; i++)
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

            private static readonly Reservoir elements = new();
            public static int MaxTweens { get => elements.Capacity; set => elements.Capacity = value; }
            public int AvailableTweens => MaxTweens - elements.ActiveTweens;

            private void LateUpdate()
            {
                  for (int i = elements.ActiveTweens - 1; i >= 0; i--)
                  {
                        IElement element = elements[i];
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
                  if (!Application.isPlaying || element.IsEmpty) return;
                  if (instance == null)
                  {
                        Initialize();
                  }
                  if (elements.Add(element)) instance.enabled = true;
            }
            public static void PauseTweens() => elements.Iterate(item => item.Pause());
            public static void ResumeTweens() => elements.Iterate(item => item.Resume());
            public static void KillTweens() => elements.Iterate(element => element.Kill());
      }
}