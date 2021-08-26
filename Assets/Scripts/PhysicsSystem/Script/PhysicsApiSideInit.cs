using Ballance2.Sys.Utils;
using UnityEngine;

namespace PhysicsRT
{
    public class PhysicsApiSideInit : MonoBehaviour {

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            var go = new GameObject("PhysicsSystem");
            go.AddComponent<PhysicsApiSideInit>();
        }

        private void Start() {
            PhysicsApi.InitFinish += () => {
#if UNITY_STANDALONE_WIN
                PhysicsApi.API.SetName("666dccad4ae697b45aac145f18f49c5b");
#elif UNITY_STANDALONE_OSX
                PhysicsApi.API.SetName("39b98d635a1f2982ddab472cc6292bf8");
#elif UNITY_STANDALONE_LINUX
                Debug.LogError("The physical engine does not support this platform");
#elif UNITY_ANDROID
                PhysicsApi.API.SetName("f34560c0d2fa8d7a3d0a796fe78ac546");
#elif UNITY_IPHONE
                PhysicsApi.API.SetName("db70f6336ab7924836d9cca01d7ed919");
#else
                Debug.LogError("The physical engine does not support this platform");
#endif
            };
        }
    }
}