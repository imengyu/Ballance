using System;
using System.Collections.Generic;
using UnityEngine;
using Ballance2.LuaHelpers;
using BallancePhysics.Api;

namespace BallancePhysics.Wapper
{
  [AddComponentMenu("BallancePhysics/PhysicsMotionController")]
  [DefaultExecutionOrder(40)]
  [DisallowMultipleComponent]
  [SLua.CustomLuaClass]
  [LuaApiDescription("Ballance PhysicsMotionController 组件")]
  public class PhysicsMotionController : MonoBehaviour
  {
    //!TODO: Maybe this component is not necessary
  }
}