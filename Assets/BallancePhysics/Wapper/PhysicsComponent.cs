using System;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  
  [RequireComponent(typeof(PhysicsObject))]
  
  public class PhysicsComponent : MonoBehaviour
  {
    /// <summary>
    /// 获取组件句柄。如果当前约束还未创建，则返回 IntPtr.Zero 
    /// </summary>
    /// <value></value>
    public IntPtr Handle { get; protected set; } = IntPtr.Zero;

    protected virtual void DoCreate() {
    }
    protected virtual void DoDestroy(IntPtr ptr) {
      Handle = IntPtr.Zero;
    }
    protected virtual void Awake() {
      var obj = GetComponent<PhysicsObject>();
      if(obj.ComponentsCreated) 
        Create();
    }

    /// <summary>
    /// 手动创建当前组件
    /// </summary>
    
    public void Create() {
      if(Handle == IntPtr.Zero) 
        DoCreate();
    }
    /// <summary>
    /// 手动销毁当前组件
    /// </summary>
    
    public void Destroy() {
      if(Handle != IntPtr.Zero) {
        DoDestroy(Handle);
        Handle = IntPtr.Zero;
      }
    }
  }
}