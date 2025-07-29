using UnityEngine;

namespace Emp37.Tweening
{
      public partial class Factory : MonoBehaviour
      {
            private static Factory instance;


            private void Awake()
            {
                  if (instance != null && instance != this)
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
                  instance = new GameObject(nameof(Factory)).AddComponent<Factory>();
            }
      }
}