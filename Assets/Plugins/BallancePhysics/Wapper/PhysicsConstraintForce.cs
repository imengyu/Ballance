using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsConstraintForce")]
  [RequireComponent(typeof(PhysicsObject))]
  public class PhysicsConstraintForce : MonoBehaviour
  {
    [Tooltip("设置恒力数值")]
    [SerializeField]
    private Vector3 m_ForcePosition;
    [Tooltip("设置恒力方向")]
    [SerializeField]
    private Vector3 m_ForceDirection;
    [Tooltip("施加在这个物体上的恒力位置参考")]
    [SerializeField]
    private Transform m_ForcePositionRef;    
    [Tooltip("施加在这个物体上的恒力方向参考")]
    [SerializeField]
    private Transform m_ForceDirectionRef;
    [Tooltip("力的大小")]
    [SerializeField]
    private float m_Force = 1.0f;

    private int _ForceId = 0;
    private PhysicsObject _PhysicsObject = null;
    private PhysicsObject PhysicsObject {
      get {
        if(_PhysicsObject == null)
          _PhysicsObject = GetComponent<PhysicsObject>();
        return _PhysicsObject;
      }
    }

    /// <summary>
    /// 设置恒力数值
    /// </summary>
    /// <value></value>
    public float Force { get => m_Force; set { m_Force = value; ChangeForceValue(); } }
    /// <summary>
    /// 设置恒力数值
    /// </summary>
    /// <value></value>
    public Vector3 ForcePosition { get => m_ForcePosition; set { m_ForcePosition = value; ChangeForceValue(); } }
    /// <summary>
    /// 设置恒力方向
    /// </summary>
    /// <value></value>
    public Vector3 ForceDirection { get => m_ForceDirection; set { m_ForceDirection = value; ChangeForceValue(); } }
    /// <summary>
    /// 施加在这个物体上的恒力位置参考
    /// </summary>
    /// <value></value>
    public Transform ForcePositionRef { get => m_ForcePositionRef; set { m_ForcePositionRef = value; ChangeForceValue(); } }
    /// <summary>
    /// 施加在这个物体上的恒力方向参考
    /// </summary>
    /// <value></value>
    public Transform ForceDirectionRef { get => m_ForceDirectionRef; set { m_ForceDirectionRef = value; ChangeForceValue(); } }

    private void ChangeForceValue() {
      DeleteForceData();
      AddForceData();
    }
    private void DeleteForceData() {
      if(_ForceId != 0) {
        PhysicsObject.DeleteConstantForce(_ForceId);
        _ForceId = 0;
      }
    }
    private void AddForceData() {
      if(_ForceId == 0) 
        _ForceId = PhysicsObject.AddConstantForceWithPositionAndRef(m_Force, m_ForceDirection, m_ForcePosition, m_ForceDirectionRef, m_ForcePositionRef);
    }

    private void OnEnable() {
      AddForceData();
    }
    private void OnDisable() {
      DeleteForceData();
    }
  }
}