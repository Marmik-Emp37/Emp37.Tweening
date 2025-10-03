using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Tweening
{
      public sealed class FT : MonoBehaviour
      {
            private static readonly List<IElement> elements = new(64);

            public static int ActiveTweens { get; private set; }
            public static int MaxTweens { get => elements.Capacity; set => elements.Capacity = value; }


            private void LateUpdate()
            {
                  for (int i = ActiveTweens - 1; i >= 0; i--) // iterate backwards so swap-removal doesn't skip elements
                  {
                        IElement element = elements[i];

                        if (element.Phase is Phase.Active) element.Update();
                        if (element.Phase is not (Phase.Complete or Phase.None)) continue;

                        int last = --ActiveTweens;
                        elements[i] = elements[last];
                        elements[last] = null;

                        if (ActiveTweens == 0)
                        {
                              enabled = false;
                              return; // short-circuit if multiple tweens finish at the same frame
                        }
                  }
            }

            public static void Play(IElement element)
            {
                  if (!Application.isPlaying || element.IsEmpty) return;
                  if (ActiveTweens == elements.Capacity)
                  {
                        Log.Warning($"{typeof(Factory).FullName}: Active tween limit ({MaxTweens}) reached. Increase {nameof(MaxTweens)} to allow more tweens.");
                        return;
                  }
                  elements[ActiveTweens++] = element;
                  element.Init();
                  //instance.enabled = true;
            }

            public static void Pause(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < ActiveTweens; i++) elements[i].Pause();
                  }
                  else
                  {
                        for (int i = 0; i < ActiveTweens; i++)
                        {
                              IElement e = elements[i];
                              if (e.Tag != null && e.Tag.Equals(tag, StringComparison.Ordinal)) e.Pause();
                        }
                  }
            }
            public static void Resume(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < ActiveTweens; i++) elements[i].Resume();
                  }
                  else
                  {
                        for (int i = 0; i < ActiveTweens; i++)
                        {
                              IElement e = elements[i];
                              if (e.Tag != null && e.Tag.Equals(tag, StringComparison.Ordinal)) e.Resume();
                        }
                  }
            }
            public static void Kill(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < ActiveTweens; i++) elements[i].Kill();
                  }
                  else
                  {
                        for (int i = 0; i < ActiveTweens; i++)
                        {
                              IElement e = elements[i];
                              if (e.Tag != null && e.Tag.Equals(tag, StringComparison.Ordinal)) e.Kill();
                        }
                  }
            }
      }
}