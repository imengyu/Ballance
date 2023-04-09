using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  /// <summary>
  /// 双向推板机关
  /// </summary>
  public class P_Modul_19 : ModulBase 
  {
    public PhysicsObject P_Modul_19_Flaps;

    private bool WakeUpLock = false;

    public P_Modul_19() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 60;
      WakeUpLock = false;
    }

    public override void Active()
    {
      base.Active();
      WakeUpLock = false;
      P_Modul_19_Flaps.gameObject.SetActive(true);
      P_Modul_19_Flaps.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("Wood");
      P_Modul_19_Flaps.Physicalize();
    }

    public override void Deactive()
    {
      P_Modul_19_Flaps.UnPhysicalize(true);
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
      WakeUpLock = false;
      ObjectStateBackupUtils.RestoreObjectAndChilds(gameObject);
    }

    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObjectAndChilds(gameObject);
    }

    protected override void BallEnterRange()
    {
      if (!IsPreviewMode && !WakeUpLock && IsActive) {
        WakeUpLock = true;
        P_Modul_19_Flaps.WakeUp();
      }
    }
  }
}