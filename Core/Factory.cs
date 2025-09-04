using UnityEngine;

namespace Emp37.Tweening
{
      /// <summary>
      /// Singleton factory that manages the lifecycle of all active tweens. Automatically created at runtime and persists across scene loads.
      /// </summary>
      /// <remarks>
      /// Uses Unity's LateUpdate to tick all active tweens, ensuring animations run after all other game logic has been processed each frame.
      /// <br>It aslo automatically enables/disables itself based on active tween count to minimize performance overhead when no tweens are running.</br>
      /// </remarks>
      [AddComponentMenu(""), DisallowMultipleComponent]
      public sealed partial class Factory : MonoBehaviour
      {
            private static Factory instance = null!;

            private Factory() { }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
            private static void Initialize()
            {
                  if (instance != null)
                  {
                        DestroyImmediate(instance.gameObject);
                        instance = null;
                  }
                  instance = new GameObject("~" + nameof(Factory)) { hideFlags = HideFlags.DontSave }.AddComponent<Factory>();
            }

            private void Awake()
            {
                  enabled = false; // only enable when tweens are active
                  DontDestroyOnLoad(this);
            }
            private void OnDestroy()
            {
                  if (instance != this) return;
                  OnFactoryDestroy();
                  instance = null;
            }

            static partial void OnFactoryDestroy();
      }
}