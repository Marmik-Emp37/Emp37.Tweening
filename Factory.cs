using UnityEngine;

namespace Emp37.Tweening
{
      [AddComponentMenu(""), DisallowMultipleComponent]
      public sealed partial class Factory : MonoBehaviour
      {
            private static Factory instance = null!;

            private Factory() { }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            private static void Initialize() => instance = new GameObject('~' + nameof(Factory)) { hideFlags = HideFlags.DontSave }.AddComponent<Factory>();

            private void Awake()
            {
                  if (instance != null && instance != this)
                  {
                        DestroyImmediate(gameObject);
                  }
                  else
                  {
                        enabled = false;
                        DontDestroyOnLoad(this);
                  }
            }
            private void OnDestroy()
            {
                  if (instance == this)
                  {
                        Kill();
                        instance = null;
                  }
            }
      }
}