using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_34 : ModulBase 
  {
    public PhysicsObject P_Modul_34_Kiste;
    public PhysicsObject P_Modul_34_Schiebestein;

    public P_Modul_34() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 60;
    }

    protected override void Start()
    {
      base.Start();

      if (!IsPreviewMode) {
        P_Modul_34_Kiste.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("WoodOnlyHit");
        P_Modul_34_Schiebestein.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("Stone");
      }
    }

    public override void Active()
    {
      P_Modul_34_Kiste.gameObject.SetActive(true);
      P_Modul_34_Schiebestein.gameObject.SetActive(true);
      P_Modul_34_Kiste.Physicalize();
      P_Modul_34_Schiebestein.Physicalize();
      base.Active();
    }

    public override void Deactive()
    {
      P_Modul_34_Kiste.UnPhysicalize(true);
      P_Modul_34_Schiebestein.UnPhysicalize(true);
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
        P_Modul_34_Schiebestein.WakeUp();
    }
  }
}