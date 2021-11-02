using System;
using System.Collections;
using UnityEngine;

namespace PhysicsRT
{
  [DefaultExecutionOrder(300)]
  [RequireComponent(typeof(PhysicsBody))]
  [SLua.CustomLuaClass]
  public class PhysicsConstraint : MonoBehaviour
  {
    public int Id { get; protected set; }

    [SerializeField]
    public PhysicsBody ConnectedBody;

    [SerializeField]
    [HideInInspector]
    private bool m_Breakable = false;
    [SerializeField]
    [HideInInspector]
    private float m_Threshold = 10f;
    [SerializeField]
    [HideInInspector]
    private float m_MaximumAngularImpulse = 1000;
    [SerializeField]
    [HideInInspector]
    private float m_MaximumLinearImpulse = 1000;
    [SerializeField]
    private ConstraintPriority m_Priority = ConstraintPriority.Psi;

    protected virtual void Awake()
    {

    }
    protected virtual void OnDestroy()
    {
      Destroy();
    }
    protected virtual void OnEnable()
    {
      if (ptr != IntPtr.Zero)
        PhysicsApi.API.SetConstraintEnable(ptr, true);
    }
    protected virtual void OnDisable()
    {
      if (ptr != IntPtr.Zero)
        PhysicsApi.API.SetConstraintEnable(ptr, false);
    }

    /// <summary>
    /// 设置是否启用约束
    /// </summary>
    /// <param name="b"></param>
    public void SetEnabled(bool b)
    {
      if (ptr != IntPtr.Zero)
        PhysicsApi.API.SetConstraintEnable(ptr, b);
    }

    private IntPtr ptr = IntPtr.Zero;
    private PhysicsWorld CurrentPhysicsWorld = null;

    /// <summary>
    /// 获取或设置约束是否被破坏
    /// </summary>
    public bool Breakable { get => m_Breakable; set => m_Breakable = value; }
    /// <summary>
    /// 获取或设置约束破坏阈值
    /// </summary>
    public float Threshold { get => m_Threshold; set => m_Threshold = value; }
    /// <summary>
    /// 获取或设置约束破最大线性脉冲
    /// </summary>
    public float MaximumAngularImpulse { get => m_MaximumAngularImpulse; set => m_MaximumAngularImpulse = value; }
    /// <summary>
    /// 获取或设置约束破最大线性脉冲
    /// </summary>
    public float MaximumLinearImpulse { get => m_MaximumLinearImpulse; set => m_MaximumLinearImpulse = value; }
    /// <summary>
    /// 获取或设置约束的优先级
    /// </summary>
    public ConstraintPriority Priority { get => m_Priority; set => m_Priority = value; }

    public delegate void OnConstraintBreakingEvent(PhysicsConstraint constraint, float forceMagnitude, int removed);

    /// <summary>
    /// 约束被破坏时的事件
    /// </summary>
    public OnConstraintBreakingEvent onBreaking;
    /// <summary>
    /// 手动创建
    /// </summary>
    public virtual void Create() { }
    /// <summary>
    /// 手动销毁
    /// </summary>
    public virtual void Destroy()
    {
      if (CurrentPhysicsWorld == null || ptr == IntPtr.Zero)
        return;
      if (CurrentPhysicsWorld.RemoveConstraint(this))
        PhysicsApi.API.DestoryConstraints(ptr);
      ptr = IntPtr.Zero;
    }

    internal bool TryCreate()
    {
      if (!enabled)
        return false;
      if (ptr == IntPtr.Zero)
      {
        var thisBody = GetComponent<PhysicsBody>();
        if (thisBody.GetPtr() != IntPtr.Zero && (ConnectedBody == null || ConnectedBody.GetPtr() != IntPtr.Zero))
        {
          Create();
          return true;
        }
        else if (ConnectedBody != null && ConnectedBody.GetPtr() == IntPtr.Zero)
          ConnectedBody.AddPendingCreateConstant(this);
        else if (thisBody.GetPtr() == IntPtr.Zero)
          thisBody.AddPendingCreateConstant(this);
      }
      return false;
    }

    protected IntPtr CreatePre()
    {
      CurrentPhysicsWorld = PhysicsWorld.GetCurrentScensePhysicsWorld();
      if (CurrentPhysicsWorld == null)
      {
        Debug.LogWarning("Not found PhysicsWorld in this scense, please add it before use PhysicsBody.");
        return IntPtr.Zero;
      }
      return GetComponent<PhysicsBody>().GetPtr();
    }
    protected void CreateLastStep(IntPtr ptr)
    {
      this.ptr = ptr;
      Id = PhysicsApi.API.GetConstraintId(ptr);
      CurrentPhysicsWorld.AddConstraint(Id, this);
    }
    protected sConstraintBreakData GetConstraintBreakData()
    {
      sConstraintBreakData data = new sConstraintBreakData();
      data.breakable = m_Breakable;
      data.threshold = m_Threshold;
      data.maximumAngularImpulse = m_MaximumAngularImpulse;
      data.maximumLinearImpulse = m_MaximumLinearImpulse;
      return data;
    }

    public bool GetConstraintBroken()
    {
      if (ptr != IntPtr.Zero)
        return PhysicsApi.API.IsConstraintBroken(ptr);
      Debug.LogWarning("Constraint not created");
      return false;
    }
    public void SetConstraintBroken(bool broken, float force = 0)
    {
      if (ptr != IntPtr.Zero)
        PhysicsApi.API.SetConstraintBroken(ptr, m_Breakable, force);
    }
    private IntPtr GetPtr() { return ptr; }
  }
  [SLua.CustomLuaClass]
  public enum ConstraintPriority
  {
    Toi = 3,
    Psi = 1
  }
}