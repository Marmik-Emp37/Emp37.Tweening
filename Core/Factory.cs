using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Tweening
{
      [DefaultExecutionOrder(1), DisallowMultipleComponent, AddComponentMenu("")]
      public sealed class Factory : MonoBehaviour
      {
            private static Factory instance = null!;

            private static readonly List<Tween> activeTweens = new(128);


            private Factory()
            {
            }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            private static void Initialize()
            {
                  if (instance != null)
                  {
                        DestroyImmediate(instance.gameObject);
                        instance = null;
                  }
                  instance = new GameObject("~" + nameof(Factory)).AddComponent<Factory>();
                  DontDestroyOnLoad(instance);
            }

            private void LateUpdate()
            {
                  for (int i = activeTweens.Count - 1; i >= 0; i--)
                  {
                        Tween tween = activeTweens[i];

                        if (tween.phase == Phase.Active) tween.Update();
                        if (tween.phase != Phase.Dead) continue;

                        int last = activeTweens.Count - 1;
                        if (i != last) activeTweens[i] = activeTweens[last];
                        activeTweens.RemoveAt(last);

                        if (activeTweens.Count == 0)
                        {
                              enabled = false;
                              return;
                        }
                  }
            }
            private void OnDestroy()
            {
                  if (instance != this) return;
                  activeTweens.Clear();
                  instance = null;
            }

            public static void Register(Tween tween)
            {
                  if (instance == null || tween == null || tween.IsEmpty) return;
                  if (activeTweens.Count == activeTweens.Capacity)
                  {
                        Log.Info($"[{typeof(Factory).FullName}] Tween capacity ({activeTweens.Capacity}) reached. Factory is scaling up, check for leaks or unintended bursts.", instance);
                  }
                  activeTweens.Add(tween);
                  tween.Replay();

                  instance.enabled = true;
            }

            public static void Pause(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < activeTweens.Count; i++) activeTweens[i].Pause();
                  }
                  else
                  {
                        for (int i = 0; i < activeTweens.Count; i++)
                        {
                              Tween t = activeTweens[i];
                              if (tag.Equals(t.Tag, StringComparison.Ordinal)) t.Pause();
                        }
                  }
            }
            public static void Resume(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < activeTweens.Count; i++) activeTweens[i].Resume();
                  }
                  else
                  {
                        for (int i = 0; i < activeTweens.Count; i++)
                        {
                              Tween t = activeTweens[i];
                              if (tag.Equals(t.Tag, StringComparison.Ordinal)) t.Resume();
                        }
                  }
            }
            public static void Kill(string tag = null)
            {
                  if (string.IsNullOrWhiteSpace(tag))
                  {
                        for (int i = 0; i < activeTweens.Count; i++)
                        {
                              activeTweens[i].Kill();
                        }
                        activeTweens.Clear();
                        instance.enabled = false;
                  }
                  else
                  {
                        for (int i = 0; i < activeTweens.Count; i++)
                        {
                              Tween t = activeTweens[i];
                              if (tag.Equals(t.Tag, StringComparison.Ordinal)) t.Kill();
                        }
                  }
            }
      }
}