using System;
using Ballance2.LuaHelpers;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [RequireComponent(typeof(PhysicsObject))]
  public class PhysicsComponent : MonoBehaviour
  {
    [LuaApiDescription("获取组件句柄。如果当前约束还未创建，则返回 IntPtr.Zero ")]
    public IntPtr Handle { get; protected set; } = IntPtr.Zero;

    protected virtual void DoCreate() {
    }
    protected virtual void DoDestry(IntPtr ptr) {
      Handle = IntPtr.Zero;
    }
    protected virtual void Awake() {
      var obj = GetComponent<PhysicsObject>();
      if(obj.ComponentsCreated) 
        Create();
    }

    [LuaApiDescription("创建")]
    public void Create() {
      if(Handle == IntPtr.Zero) 
        DoCreate();
    }
    [LuaApiDescription("销毁")]
    public virtual void Destry() {
      if(Handle != IntPtr.Zero) {
        DoDestry(Handle);
        Handle = IntPtr.Zero;
      }
    }
  }
}