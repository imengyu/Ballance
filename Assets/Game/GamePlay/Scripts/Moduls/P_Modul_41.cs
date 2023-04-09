using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  /// <summary>
  /// 浮块机关
  /// </summary>
  public class P_Modul_41 : ModulBase 
  {
    public PhysicsObject P_Modul_41_Box;

    public P_Modul_41() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 60;
    }

    public override void Active()
    {
      base.Active();
      P_Modul_41_Box.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("WoodOnlyHit");
      P_Modul_41_Box.gameObject.SetActive(true);
      P_Modul_41_Box.Physicalize();
    }

    public override void Deactive()
    {
      P_Modul_41_Box.UnPhysicalize(true);
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
  }
}