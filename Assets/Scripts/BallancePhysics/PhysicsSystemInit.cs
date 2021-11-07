using System.Collections.Generic;
using UnityEngine;

namespace BallancePhysics
{
  public static class PhysicsSystemInit
  {
    public static void Init()
    {
#if !UNITY_EDITOR
      DoInit();
      Application.quitting += () =>
      {
        DoDestroy();
      };
#endif
    }

    public static void DoDestroy()
    {
      PhysicsApi.PhysicsApiDestroy();
    }
    public static void DoInit()
    {
      PhysicsApi.PhysicsApiInit();
    }
  }
}