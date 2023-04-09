using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_17 : ModulBase 
  {
    public PhysicsObject Modul17_Dreharme;

    public override void Active()
    {
      base.Active();
      Modul17_Dreharme.gameObject.SetActive(true);
      Modul17_Dreharme.Physicalize();
      Modul17_Dreharme.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("WoodOnlyHit");
    }

    public override void Deactive()
    {
      Modul17_Dreharme.UnPhysicalize(true);
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
      ObjectStateBackupUtils.RestoreObjectAndChilds(Modul17_Dreharme.gameObject);
    }

    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObject(Modul17_Dreharme.gameObject);
    }
  }
}