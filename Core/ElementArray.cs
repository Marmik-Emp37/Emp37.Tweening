using System;

using UnityEngine;

namespace Emp37.Tweening
{
      internal sealed class ElementArray
      {
            private int _capacity;
            private IElement[] elements;

            public int Count { get; private set; }
            public int Capacity
            {
                  get => _capacity;
                  set
                  {
                        int limit = Mathf.Max(value, Count);
                        if (limit == _capacity) return;
                        if (limit < Count) Debug.LogWarning($"Requested capacity {value} is below the active tween count ({limit}). Using {limit} to prevent data loss.");
                        Array.Resize(ref elements, limit);
                        _capacity = limit;
                  }
            }

            public ElementArray(int capacity) => elements = new IElement[_capacity = Mathf.Max(1, capacity)];

            public bool Add(IElement item)
            {
                  bool value = Count < Capacity;
                  if (value) elements[Count++] = item;
                  return value;
            }
            public void RemoveAt(int index)
            {
                  if (!InRange(index)) return;
                  elements[index] = elements[--Count];
                  elements[Count] = null;
            }
            public void Clear()
            {
                  Array.Clear(elements, 0, Count);
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