using Ballance2.Utils;
using UnityEngine;
using UnityEngine.Animations;

namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 机关定义
  /// 您可以通过继承此类来定义您的机关。
  /// </summary>
  public class ModulBase : MonoBehaviour 
  {
    [Tooltip("设置假贴图投射影子")]
    /// <summary>
    /// 设置假贴图投射影子
    /// </summary>
    public GameObject FakeShadow = null;
    [Tooltip("设置占位符预制体，占位符用于在关卡编辑器中显示")]
    /// <summary>
    /// 设置占位符预制体，占位符用于在关卡编辑器中显示
    /// </summary>
    public GameObject PlaceHolderPrefab = null;
    /// <summary>
    /// 获取或者设置当前机关基类是否自动控制当前机关的激活与失活
    /// </summary>
    protected bool AutoActiveBaseGameObject = false;
    /// <summary>
    /// 获取玩家球是否在当前机关球区域检测范围内
    /// </summary>
    protected bool BallInRange = false;
    /// <summary>
    /// 是否开启球区域检测，开启后会定时检测，如果球进入指定范围，则发出 BallEnterRange 事件
    /// </summary>
    protected bool EnableBallRangeChecker = false;
    /// <summary>
    /// 球区域检测范围。创建之后也可以手动设置 modul.BallRangeChecker.Diatance 属性来设置
    /// </summary>
    protected float BallCheckeRange = 60;
    /// <summary>
    /// 获取当前机关是否在预览模式中加载
    /// </summary>
    [HideInInspector]
    public bool IsPreviewMode = false;
    /// <summary>
    /// 获取当前机关是否在测试模式中加载
    /// </summary>
    [HideInInspector]
    public bool IsDebugMode = false;
    /// <summary>
    /// 获取当前机关是否激活
    /// </summary>
    [HideInInspector]
    protected bool IsActive = false;

    protected TiggerTester BallRangeChecker = null;
    protected Projector ShadowInstance = null;

    protected virtual void Start() {
      //机关内置的球区域检测功能初始化
      if (this.EnableBallRangeChecker) {
        var collide = this.gameObject.AddComponent<SphereCollider>();
        collide.radius = this.BallCheckeRange;
        collide.isTrigger = true;
        this.BallRangeChecker = this.gameObject.AddComponent<TiggerTester>();
        this.BallRangeChecker.onTriggerEnter = (obj, other) => {
          if (!this.BallInRange && other.tag == "Ball") 
          {
            this.BallInRange = true;
            this.BallEnterRange();
          }
        };
        this.BallRangeChecker.onTriggerExit = (obj, other) => {
          if (this.BallInRange && other.tag == "Ball") {
            this.BallInRange = false;
            this.BallLeaveRange();
          }
        };
      }
      if (FakeShadow != null)
      {
        ShadowInstance = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<Projector>(FakeShadow, transform.parent, $"ShadowOf{name}");
        ShadowInstance.gameObject.SetActive(false);
        var parentConstraint = ShadowInstance.GetComponent<ParentConstraint>();
        var constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = transform;
        constraintSource.weight = 1;
        parentConstraint.constraintActive = true;
        parentConstraint.SetSource(0, constraintSource);
      }
    }

    /// <summary>
    /// 机关激活时发出此事件（进入当前小节）
    /// 此事件在Backup事后发出
    /// </summary>
    public virtual void Active() {
      if (AutoActiveBaseGameObject)
        gameObject.SetActive(true);
      if (ShadowInstance != null)
        ShadowInstance.gameObject.SetActive(true);
      IsActive = true;
    }
    /// <summary>
    /// 机关隐藏时发出此事件（当前小节结束）
    /// </summary>
    public virtual void Deactive() {
      IsActive = false;
      if (AutoActiveBaseGameObject)
        gameObject.SetActive(false);
      if (ShadowInstance != null)
        ShadowInstance.gameObject.SetActive(false);
    }
    /// <summary>
    /// 关卡卸载时发出此事件
    /// </summary>
    public virtual void UnLoad() {}
    /// <summary>
    /// 机关重置为初始状态时发出此事件（玩家失败，重新开始一节）。Reset在Deactive之后发出
    /// </summary>
    public virtual void Reset(ModulBaseResetType type) {}
    /// <summary>
    /// 机关就绪，可以保存状态时发出此事件（初次加载完成）
    /// </summary>
    public virtual void Backup() {}
    /// <summary>
    /// 球进入当前机关指定范围时发出此事件
    /// </summary>
    protected virtual void BallEnterRange() {}
    /// <summary>
    /// 球离开当前机关指定范围时发出此事件
    /// </summary>
    protected virtual void BallLeaveRange() {}
    
    /// <summary>
    /// 在预览模式中激活时发出此事件
    /// </summary>
    public virtual void ActiveForPreview() {}
    /// <summary>
    /// 在预览模式中隐藏时发出此事件
    /// </summary>
    public virtual void DeactiveForPreview() {}
    /// <summary>
    /// 在测试模式中执行测试指令时发出此事件
    /// </summary>
    public virtual void DebugTestCommand(int command) {}
  }

  /// <summary>
  /// 标识机关的重置类型
  /// </summary>
  public enum ModulBaseResetType {
    /// <summary>
    /// 玩家失败，重新开始一节
    /// </summary>
    SectorRestart,
    /// <summary>
    /// 关卡重新开始
    /// </summary>
    LevelRestart,
  }
}