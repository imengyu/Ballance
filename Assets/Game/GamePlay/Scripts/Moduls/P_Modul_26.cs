using Ballance2.Services;
using Ballance2.Utils;
using BallancePhysics.Wapper;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_26 : ModulBase 
  {
    public PhysicsObject P_Modul_26_Sack;
    public PhysicsObject P_Modul_26_Rope;
    public PhysicsConstraintForce P_Modul_26_Sack_Force;
    public float Force = 0.25f;
    public float SwitchTime = 1.400f;

    private int  _ForceTimer = 0;
    private bool _ForceState = true;

    public P_Modul_26() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 30;
    }

    public override void Active()
    {
      base.Active();
      P_Modul_26_Rope.gameObject.SetActive(true);
      P_Modul_26_Sack.gameObject.SetActive(true);
      P_Modul_26_Rope.Physicalize();
      P_Modul_26_Sack.Physicalize();
      P_Modul_26_Sack.EnableConstantForce = true;
      _ForceTimer = GameTimer.Timer(SwitchTime, () => {
        _ForceState = !_ForceState;//切换方向
        if (_ForceState)
          P_Modul_26_Sack_Force.Force = Force;
        else
          P_Modul_26_Sack_Force.Force = -Force;
      });
    }

    public override void Deactive()
    {
      P_Modul_26_Sack.EnableConstantForce = false;
      P_Modul_26_Rope.UnPhysicalize(true);
      P_Modul_26_Sack.UnPhysicalize(true);
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
      ObjectStateBackupUtils.RestoreObject(P_Modul_26_Rope.gameObject);
      ObjectStateBackupUtils.RestoreObject(P_Modul_26_Sack.gameObject);
    }

    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObject(P_Modul_26_Rope.gameObject);
      ObjectStateBackupUtils.BackUpObject(P_Modul_26_Sack.gameObject);
    }
  }
}