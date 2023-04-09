using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_03 : ModulBase 
  {
    public PhysicsObject P_Modul_03_Floor;
    public PhysicsObject P_Modul_03_Gate;
    public PhysicsObject[] P_Modul_03_Wall = new PhysicsObject[6];

    public P_Modul_03() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 30;
    }

    public override void Active()
    {
      base.Active();
      P_Modul_03_Floor.gameObject.SetActive(true);
      P_Modul_03_Gate.gameObject.SetActive(true);
      P_Modul_03_Floor.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("Wood");
      P_Modul_03_Floor.Physicalize();
      P_Modul_03_Gate.Physicalize();
 
      foreach(var wall in P_Modul_03_Wall) {
        wall.gameObject.SetActive(true);
        wall.Physicalize();
      }

    }

    public override void Deactive()
    {
      P_Modul_03_Floor.UnPhysicalize();
      P_Modul_03_Gate.UnPhysicalize();
      foreach(var wall in P_Modul_03_Wall)
        wall.UnPhysicalize();
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
        P_Modul_03_Floor.WakeUp();
    }
  }
}