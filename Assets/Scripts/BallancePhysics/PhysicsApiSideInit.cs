using Ballance2.Sys.Utils;
using UnityEngine;

namespace BallancePhysics
{
  public class PhysicsApiSideInit : MonoBehaviour
  {

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
      var go = new GameObject("PhysicsSystem");
      go.AddComponent<PhysicsApiSideInit>();
    }

    private void Start()
    {
      PhysicsApi.InitFinish += () =>
      {
#if UNITY_STANDALONE_WIN
        PhysicsApi.API.set_name(IntPtr.Zero, "666dccad4ae697b45aac145f18f49c5b");
#elif UNITY_STANDALONE_OSX
        PhysicsApi.API.set_name(IntPtr.Zero, "39b98d635a1f2982ddab472cc6292bf8");
#elif UNITY_STANDALONE_LINUX
        PhysicsApi.API.set_name(IntPtr.Zero, "1c2cbe3bde70231732c1ae01653047a4");
#elif UNITY_ANDROID
        PhysicsApi.API.set_name(IntPtr.Zero, "f34560c0d2fa8d7a3d0a796fe78ac546");
#elif UNITY_IPHONE
        PhysicsApi.API.set_name(IntPtr.Zero, "db70f6336ab7924836d9cca01d7ed919");
#else
        Debug.LogError("The physical engine does not support this platform");
#endif
        Destroy(this);
      };
    }
  }
}