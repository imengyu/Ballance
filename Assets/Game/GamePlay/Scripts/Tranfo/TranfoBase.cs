
using UnityEngine;

namespace Ballance2.Game.GamePlay.Tranfo
{
  /// <summary>
  /// 变球器接口
  /// </summary>
  public interface ITranfoBase
  {
    /// <summary>
    /// 获取当前变球器目标变球类型
    /// </summary>
    /// <returns></returns>
    string GetTargetBallType();
    /// <summary>
    /// 获取变球器动画控制器
    /// </summary>
    /// <returns></returns>
    ITranfoAminControl GetTranfoAminControl();
    /// <summary>
    /// 获取变球器颜色
    /// </summary>
    /// <returns></returns>
    Color GetTranfoColor();
    /// <summary>
    /// 获取 Transform
    /// </summary>
    Transform GetTransform();
    /// <summary>
    /// 变球完成，重置回调
    /// </summary>
    void ResetTranfo();
  }

  /// <summary>
  /// 基础变球器
  /// </summary>
  public class TranfoBase : ModulBase, ITranfoBase
  {
    public TiggerTester _Tigger;
    public Color _Color;
    public string _TargetBallType;

    public string GetTargetBallType() { return _TargetBallType; }
    public ITranfoAminControl GetTranfoAminControl() { return TranfoAminControl.Instance; }
    public Color GetTranfoColor() { return _Color; }
    public Transform GetTransform() { return transform; }

    public TranfoBase() {
      this.AutoActiveBaseGameObject = true;
    }

    protected override void Start()
    {
      base.Start();

      _Tigger.onTriggerEnter = (_, other) => {
        //球，并且球类型于目标类型不一致
        if (!_TranfoActived && other.tag == "Ball" && other.name != _TargetBallType)
        {
          //触发变球
          _TranfoActived = true;
          GamePlayManager.Instance.ActiveTranfo(this, _TargetBallType);
        }
      };
    }

    public void ResetTranfo() {
      this.Reset(ModulBaseResetType.SectorRestart);
    }

    protected bool _TranfoActived = false;

    public override void ActiveForPreview()
    {
      base.ActiveForPreview();
      gameObject.SetActive(true);
    }
    public override void DeactiveForPreview()
    {
      base.DeactiveForPreview();
      gameObject.SetActive(false);
    }
    public override void Reset(ModulBaseResetType type)
    {
      base.Reset(type);
      _TranfoActived = false;
    }
  }
}