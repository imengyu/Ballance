using Ballance2.Services;
using Ballance2.Utils;
using BallancePhysics.Wapper;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_29 : ModulBase 
  {
    public PhysicsHinge _P_Modul_29_Platte05_HingeConstraint;
    public TiggerTester _P_Modul_29_Platte05_Tigger;
    public PhysicsObject[] _P_Modul_29_Plattes = new PhysicsObject[9];

    public P_Modul_29() {
      EnableBallRangeChecker = true;
      BallCheckeRange = 60;
    }

    private bool _BrigeBreaked = false;

    protected override void Start()
    {
      base.Start();
 
      //石球断开木桥
      _P_Modul_29_Platte05_Tigger.onTriggerEnter = (_, other) => OnBallEnterRange(other);
    }

    private void OnBallEnterRange(GameObject other) {
      if (!_BrigeBreaked && other.tag == "Ball" && other.name == "BallStone") {
        _BrigeBreaked = true;
        _P_Modul_29_Platte05_HingeConstraint.Destroy();
        GameSoundManager.Instance.PlayFastVoice("core.sounds:Misc_RopeTears.wav", GameSoundType.Normal);
      }
    }

    public override void Active()
    {
      base.Active();

      foreach (var Platte in _P_Modul_29_Plattes)
      {
        Platte.Physicalize();
        Platte.gameObject.SetActive(true);
      }
    }

    public override void Deactive()
    {
      foreach (var Platte in _P_Modul_29_Plattes)
      {
        Platte.UnPhysicalize(true);
        Platte.gameObject.SetActive(false);
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
      ObjectStateBackupUtils.RestoreObjectAndChilds(gameObject);
      _BrigeBreaked = false;
    }

    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObjectAndChilds(gameObject);
    }

    protected override void BallEnterRange()
    {
      if (!IsPreviewMode && IsActive)
        _P_Modul_29_Plattes[3].WakeUp();
    }
  }
}