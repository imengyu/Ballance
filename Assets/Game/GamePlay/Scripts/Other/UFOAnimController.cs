using System.Collections;
using Ballance2.Base;
using Ballance2.Game.Utils;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Other
{
  /// <summary>
  /// 游戏结束时的UFO动画控制器。
  /// </summary>
  public class UFOAnimController : GameSingletonBehavior<UFOAnimController> {

    public Animator PE_UFO_Arm_Inner_01;
    public Animator PE_UFO_Arm_Inner_02;
    public Animator PE_UFO_Arm_Inner_03;
    public Animator PE_UFO_Arm_Inner_04;
    public GameObject PE_UFO_Flash;
    public AudioSource PE_UFO_Sound;
    public AudioSource PE_UFO_CatchSound;
    public SmoothFly PE_UFO_Fly;
    public GameObject PE_UFO_Body;

    private Transform _LastBallParent;
    private Balls.Ball _LastSetBall;

    private void Start() {
      PE_UFO_Body.SetActive(false);
    } 

    public void StartSeq() {
      PE_UFO_Body.SetActive(true) ;
      PE_UFO_Sound.Play();
      PE_UFO_CatchSound.Stop();
      PE_UFO_Flash.SetActive(false);
      PE_UFO_Arm_Inner_01.Play("DefaultState");
      PE_UFO_Arm_Inner_02.Play("DefaultState");
      PE_UFO_Arm_Inner_03.Play("DefaultState");
      PE_UFO_Arm_Inner_04.Play("DefaultState");

      StartCoroutine(StartSeqInternal());
    }
    
    private IEnumerator StartSeqInternal() {
      var GamePlayManagerInstance = GamePlayManager.Instance;
      var PE_Balloon_BallRefPos = GamePlayManagerInstance.SectorManager.CurrentLevelEndBalloon.PE_Balloon_BallRefPos;
      var fly = PE_UFO_Fly;
      var index = 0;

      foreach (var value in UFOPositions.Data) {
        fly.Time = value.flyTime;
        fly.TargetPos = PE_Balloon_BallRefPos.transform.TransformPoint(value.pos);
        fly.Fly = true;

        yield return new WaitForSeconds(value.waitTime);

        if (value.startBall) {
          PE_UFO_Arm_Inner_01.Play("PE_UFO_Arm_Animation");
          PE_UFO_Arm_Inner_02.Play("PE_UFO_Arm_Animation");
          PE_UFO_Arm_Inner_03.Play("PE_UFO_Arm_Animation");
          PE_UFO_Arm_Inner_04.Play("PE_UFO_Arm_Animation");
          PE_UFO_CatchSound.Play();
        }
        if (value.catchBall) {
          GamePlayManagerInstance.BallManager.SetControllingStatus(BallControlStatus.LockLookMode); //去除物理化，禁用控制
          var currentBall = GamePlayManagerInstance.BallManager.CurrentBall;
          if (currentBall != null) {
            var transform = currentBall.gameObject.transform;
            _LastBallParent = transform.parent;
            _LastSetBall = currentBall;
            transform.SetParent(PE_UFO_Fly.transform);
            transform.localPosition = Vector3.zero;
          }
        }
        if (index == UFOPositions.Data.Count - 2) {
          //最后一个位置就隐藏UFO和球了
          PE_UFO_Body.SetActive(false);
          if (_LastSetBall != null)  
            _LastSetBall.gameObject.SetActive(false);
        } else if (index == UFOPositions.Data.Count - 1) {
          PE_UFO_Flash.SetActive(true); //闪光
        }
        index++;
      }

      PE_UFO_Fly.Fly = false;

      //恢复球的父级
      if (_LastBallParent != null) {
        _LastSetBall.transform.SetParent(_LastBallParent);
        _LastBallParent = null;
      }

      //UFO动画完成,现在控制权交回GamePlayManager
      GamePlayManagerInstance.UfoAnimFinish();
    }
  }
}
