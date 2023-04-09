using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  /// <summary>
  /// 栅栏机关
  /// </summary>
  public class P_Modul_01 : ModulBase 
  {
    public PhysicsObject P_Modul_01_Rinne;
    public PhysicsObject P_Modul_01_Filter;
    public PhysicsObject P_Modul_01_Pusher;

    protected override void Start()
    {
      if (!IsPreviewMode) 
        P_Modul_01_Pusher.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("WoodOnlyHit");
      base.Start();
    }

    public override void Active()
    {
      gameObject.SetActive(true);
      P_Modul_01_Rinne.gameObject.SetActive(true);
      P_Modul_01_Filter.gameObject.SetActive(true);
      P_Modul_01_Pusher.gameObject.SetActive(true);
      P_Modul_01_Rinne.Physicalize();
      P_Modul_01_Filter.Physicalize();
      P_Modul_01_Pusher.Physicalize();
    }
    public override void Deactive()
    {
      P_Modul_01_Rinne.UnPhysicalize(true);
      P_Modul_01_Filter.UnPhysicalize(true);
      P_Modul_01_Pusher.UnPhysicalize(true);
      gameObject.SetActive(false);
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