using System;

using UnityEngine;

namespace Emp37.Tweening
{
      internal sealed class ElementArray
      {
            private IElement[] elements;

            public int Count { get; private set; }
            public int Capacity
            {
                  get => elements.Length;
                  set
                  {
                        if (value == elements.Length) return;

                        int limit = Mathf.Max(value, Count);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                        if (limit != value)
                        {
                              Debug.LogWarning($"Requested capacity {value} is below active count ({Count}). Using {limit}.");
                        }
#endif
                        Array.Resize(ref elements, limit);
                  }
            }

            public ElementArray(int capacity) => elements = new IElement[Mathf.Max(1, capacity)];

            public bool Add(IElement item)
            {
                  if (Count == Capacity) return false;
                  elements[Count++] = item;
                  return true;
            }
            public void RemoveAt(int index)
            {
                  if (!InRange(index)) return;

                  int last = --Count;
                  elements[index] = elements[last];
                  elements[last] = null; // drop reference for GC
            }
            public void Clear()
            {
                  Array.Clear(elements, 0, Count); // bulk-null active range
                  Count = 0;
            }
            public void ForEach(Action<IElement> action)
            {
                  for (int i = 0; i < Count; i++)
                  {
                        action(elements[i]);
                  }
            }
            private bool InRange(int index) => (uint) index < (uint) Count;

            public IElement this[int index] => InRange(index) ? elements[index] : throw new IndexOutOfRangeException();
      }
}