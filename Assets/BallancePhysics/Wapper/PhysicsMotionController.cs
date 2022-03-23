using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsMotionController")]
  [DefaultExecutionOrder(40)]
  [DisallowMultipleComponent]
  [LuaApiDescription("通过尝试使物理对象跟随另一个三维实体来控制它")]
  [LuaApiNotes("!> **提示：这个类没有实现！** ")]
  /// <summary>
  /// Ballance PhysicsMotionController 组件
  /// </summary>
  public class PhysicsMotionController : MonoBehaviour
  {
    //!TODO: Maybe this component is not necessary
  }
}