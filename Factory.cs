using UnityEngine;

namespace Emp37.Tweening
{
      public sealed partial class Factory : MonoBehaviour
      {
            private static Factory instance;

            private Factory() { }

            private void Awake()
            {
                  if (instance == null) instance = this;
                  else if (instance != this)
                  {
                        Destroy(gameObject);
                        return;
                  }
                  DontDestroyOnLoad(gameObject);
                  enabled = false;
            }
            private void OnDestroy()
            {
                  if (instance == this)
                  {
                        KillTweens();
                        instance = null;
                  }
            }

            private static void Initialize()
            {
                  instance = FindAnyObjectByType<Factory>();
                  if (instance == null)
                  {
                        instance = new GameObject(nameof(Factory)).AddComponent<Factory>();
                  }
            }
      }
}