using UnityEngine;

namespace Emp37.Tweening
{
      [AddComponentMenu(""), DisallowMultipleComponent]
      public sealed partial class Factory : MonoBehaviour
      {
            private static Factory instance;

            private Factory() { }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            private static void Initialize()
            {
                  if (instance != null)
                  {
                        DestroyImmediate(instance.gameObject);
                  }
                  instance = new GameObject(nameof(Factory)).AddComponent<Factory>();
                  DontDestroyOnLoad(instance);
            }

            private void Awake()
            {
                  enabled = false;
                  gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
            private void OnDestroy()
            {
                  if (instance == this)
                  {
                        KillTweens();
                        instance = null;
                  }
            }
      }
}