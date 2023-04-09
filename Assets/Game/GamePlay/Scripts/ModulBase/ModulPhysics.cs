using Ballance2.Utils;
using BallancePhysics.Wapper;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  public class ModulPhysics : ModulBase 
  {
    [Tooltip("机关对应物理物体，请在编辑器中设置此字段")]
    /// <summary>
    /// 机关对应物理物体
    /// </summary>
    public PhysicsObject PhysicsObject;
    [Tooltip("设置物理物体球碰撞声音组，可以在编辑器中设置此字段")]
    /// <summary>
    /// 物理物体球碰撞声音组
    /// </summary>
    public string PhysicsObjectCollIDName;

    protected override void Start()
    {
      base.Start();

      if (!IsPreviewMode) 
      {
        if (PhysicsObject == null) 
          PhysicsObject = gameObject.GetComponent<PhysicsObject>();
        
        if (!string.IsNullOrEmpty(PhysicsObjectCollIDName))
          PhysicsObject.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName(PhysicsObjectCollIDName);
      }
    }

    protected override void BallEnterRange()
    {
      base.BallEnterRange();
      if (!IsPreviewMode)
      {
        if (PhysicsObject != null && PhysicsObject.IsPhysicalized) 
          PhysicsObject.WakeUp();
      }
    }

    public override void Active()
    {
      base.Active();

      gameObject.SetActive(true);
      PhysicsObject.Physicalize();
    }
    public override void Deactive()
    {
      base.Deactive();

      PhysicsObject.UnPhysicalize(true);
      gameObject.SetActive(false);
    }

    public override void Reset(ModulBaseResetType type)
    {
      ObjectStateBackupUtils.RestoreObject(PhysicsObject.gameObject);
    }
    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObject(PhysicsObject.gameObject);
    }
    public override void ActiveForPreview()
    {
      gameObject.SetActive(true);
    }
    public override void DeactiveForPreview()
    {
      gameObject.SetActive(false);
    }
  }
}