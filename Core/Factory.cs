using UnityEngine;

namespace Emp37.Tweening
{
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
                  enabled = false;
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