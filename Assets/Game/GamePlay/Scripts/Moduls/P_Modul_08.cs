using Ballance2.Services;
using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_08 : ModulBase 
  {
    public PhysicsObject P_Modul_08_Schaukel;
    public PhysicsConstraintForce P_Modul_08_Schaukel_Force;
    public float Force = 1.1f;
    public float SwitchTime = 0.5f;
    public float DelayTime = 0.5f;

    private bool _EnableForce = false;
    private int _ForceTimer = 0;
    private int _ForceState = 0; //0/2 none 1 left 3 right

    public override void Active()
    {
      base.Active();
      P_Modul_08_Schaukel.gameObject.SetActive(true);
      P_Modul_08_Schaukel.CollisionID = GamePlayManager.Instance.BallSoundManager.GetSoundCollIDByName("Wood");
      P_Modul_08_Schaukel.Physicalize();
      P_Modul_08_Schaukel.EnableConstantForce = true;
      _EnableForce = true;
      _ForceTimer = GameTimer.Timer(SwitchTime, DelayTime, () => {
        _ForceState++;
        if (_ForceState > 3) 
          _ForceState = 0;
        //切换方向
        if (_ForceState == 0 || _ForceState == 2)
          P_Modul_08_Schaukel_Force.Force = 0;
        else if (_EnableForce && _ForceState == 1) {
          P_Modul_08_Schaukel_Force.Force = Force;
          P_Modul_08_Schaukel.WakeUp();
        } else if (_EnableForce && _ForceState == 3)
          P_Modul_08_Schaukel_Force.Force = -Force;
      });
    }

    public override void Deactive()
    {
      _EnableForce = false;
      P_Modul_08_Schaukel.EnableConstantForce = false;
      P_Modul_08_Schaukel.UnPhysicalize(true);
      if (_ForceTimer > 0) {
        GameTimer.DeleteTimer(_ForceTimer);
        _ForceTimer = 0;
      }
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
      ObjectStateBackupUtils.RestoreObjectAndChilds(P_Modul_08_Schaukel.gameObject);
    }

    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObjectAndChilds(P_Modul_08_Schaukel.gameObject);
    }
  }
}