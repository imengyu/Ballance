using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_25 : ModulBase 
  {
    public PhysicsObject P_Modul_25_Bridge;
    public PhysicsObject P_Modul_25_Bridge_Stopper_Left;
    public PhysicsObject P_Modul_25_Bridge_Stopper_Right;
    public PhysicsObject P_Modul_25_Hinge_Col_Left;
    public PhysicsObject P_Modul_25_Hinge_Col_Right;

    public P_Modul_25() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 60;
    }

    public override void Active()
    {
      base.Active();
      P_Modul_25_Bridge.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("WoodOnlyHit");
      P_Modul_25_Bridge.gameObject.SetActive(true);
      P_Modul_25_Bridge_Stopper_Left.gameObject.SetActive(true);
      P_Modul_25_Bridge_Stopper_Right.gameObject.SetActive(true);
      P_Modul_25_Hinge_Col_Left.gameObject.SetActive(true);
      P_Modul_25_Hinge_Col_Right.gameObject.SetActive(true);
      P_Modul_25_Bridge.Physicalize();
      P_Modul_25_Bridge_Stopper_Left.Physicalize();
      P_Modul_25_Bridge_Stopper_Right.Physicalize();
      P_Modul_25_Hinge_Col_Left.Physicalize();
    }

    public override void Deactive()
    {
      P_Modul_25_Bridge.UnPhysicalize(true);
      P_Modul_25_Hinge_Col_Left.UnPhysicalize(true);
      P_Modul_25_Hinge_Col_Right.UnPhysicalize(true);
      P_Modul_25_Bridge_Stopper_Left.UnPhysicalize(true);
      P_Modul_25_Bridge_Stopper_Right.UnPhysicalize(true);
      base.Deactive();
    }

    public override void ActiveForPreview()
    {
      gameObject.SetActive(true);
    }

    public override void DeactiveForPreview()
    {
      gameObject.SetActive(false);
    }

    public override void Reset(ModulBaseResetType type)
    {
      ObjectStateBackupUtils.RestoreObjectAndChilds(gameObject);
    }

    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObjectAndChilds(gameObject);
    }

    protected override void BallEnterRange()
    {
      if (!IsPreviewMode && IsActive)
        P_Modul_25_Bridge.WakeUp();
    }
  }
}