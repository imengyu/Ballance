using BallancePhysics.Wapper;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Moduls
{
  public class P_Modul_18 : ModulBase 
  {
    public TiggerTester P_Modul_18_Kollisionsquader;
    public Animator P_Modul_18_Rotor;
    public ParticleSystem P_Modul_18_Particle;
    public ParticleSystem P_Modul_18_Particle_Small;
    public AudioSource P_Modul_18_Sound;
    public float P_Modul_18_Force;

    private PhysicsObject _CurrentInRangeBall;
    private PhysicsConstantForceData CurrentBallForce;

    public P_Modul_18() {
      AutoActiveBaseGameObject = false;
      CurrentBallForce = null;
      EnableBallRangeChecker = true;
      BallCheckeRange = 80;
    }

    protected override void Start()
    {
      base.Start();

      if (!IsPreviewMode) {
        //球进入时给一个方向向上的恒力
        P_Modul_18_Kollisionsquader.onTriggerEnter = (_, other) => {
          if (_CurrentInRangeBall == null && other.tag == "Ball") {
            _CurrentInRangeBall = GamePlayManager.Instance.BallManager.CurrentBall._Rigidbody;
            CurrentBallForce = _CurrentInRangeBall.AddConstantForceLocalCenter(P_Modul_18_Force, transform.TransformVector(Vector3.up));
          }
        };
        //球离开时去除恒力
        P_Modul_18_Kollisionsquader.onTriggerExit = (_, other) => {
          if (_CurrentInRangeBall != null && other.tag == "Ball") {
            if (CurrentBallForce != null) {
              CurrentBallForce.Delete();
              CurrentBallForce = null;
            }
            _CurrentInRangeBall = null;
          }
        };
      }
    }

    public override void Active()
    {
      base.Active();
      P_Modul_18_Sound.Play();
      P_Modul_18_Rotor.Play("P_Modul_18_Rotor_Start_Animation");
      P_Modul_18_Kollisionsquader.gameObject.SetActive(true);
      if (BallInRange) {
        P_Modul_18_Particle.gameObject.SetActive(true);
        P_Modul_18_Particle_Small.gameObject.SetActive(true);
      }
    }

    public override void Deactive()
    {
      base.Deactive();
      _CurrentInRangeBall = null;
      P_Modul_18_Sound.Stop();
      P_Modul_18_Rotor.Play("P_Modul_18_Rotor_Stop_Animation");
      P_Modul_18_Kollisionsquader.gameObject.SetActive(false);
      P_Modul_18_Particle.gameObject.SetActive(false);
      P_Modul_18_Particle_Small.gameObject.SetActive(false) ;
    }

    public override void ActiveForPreview()
    {
      gameObject.SetActive(true);
      P_Modul_18_Particle.gameObject.SetActive(true);
      P_Modul_18_Particle_Small.gameObject.SetActive(true);
      P_Modul_18_Sound.Play();
      P_Modul_18_Rotor.Play("P_Modul_18_Rotor_Start_Animation");
    }

    public override void DeactiveForPreview()
    {
      P_Modul_18_Particle.gameObject.SetActive(false);
      P_Modul_18_Particle_Small.gameObject.SetActive(false);
      P_Modul_18_Sound.Stop();
      gameObject.SetActive(false);
    }

    public override void Reset(ModulBaseResetType type)
    {
      _CurrentInRangeBall = null;
    }

    protected override void BallEnterRange()
    {
      if (IsActive) {
        P_Modul_18_Particle.gameObject.SetActive(true);
        P_Modul_18_Particle_Small.gameObject.SetActive(true);
      }
    }
    
    protected override void BallLeaveRange()
    {
      if (IsActive) {
        P_Modul_18_Particle.gameObject.SetActive(false);
        P_Modul_18_Particle_Small.gameObject.SetActive(false);
      }
    }
  }
}