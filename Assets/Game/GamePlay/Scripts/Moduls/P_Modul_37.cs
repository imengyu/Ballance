using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_37 : ModulBase 
  {
    public PhysicsObject P_Modul_37_Bridge;

    public P_Modul_37() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 60;
    }

    public override void Active()
    {
      base.Active();
      P_Modul_37_Bridge.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("Wood");
      P_Modul_37_Bridge.gameObject.SetActive(true);
      P_Modul_37_Bridge.Physicalize();
    }

    public override void Deactive()
    {
      P_Modul_37_Bridge.UnPhysicalize(true);
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
        P_Modul_37_Bridge.WakeUp();
    }
  }
}