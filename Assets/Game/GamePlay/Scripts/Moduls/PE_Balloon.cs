using Ballance2.Services;
using Ballance2.Utils;
using BallancePhysics.Wapper;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Moduls
{
  /// <summary>
  /// 结尾气球组件
  /// </summary>
  public class PE_Balloon : ModulBase 
  {
    public PhysicsObject PE_Balloon_BoxSlide;
    public PhysicsConstraintForce PE_Balloon_BoxSlideForce;
    public PhysicsConstraintForce PE_Balloon_PlatformForce;
    public PhysicsObject PE_Balloon_Platform;
    public PhysicsObject PE_Balloon_Ballon04;
    public PhysicsObject PE_Balloon_Ballon03;
    public PhysicsObject PE_Balloon_Ballon02;
    public PhysicsObject PE_Balloon_Ballon01;
    public PhysicsObject PE_Balloon_Ballon_Seil04;
    public PhysicsObject PE_Balloon_Ballon_Seil03;
    public PhysicsObject PE_Balloon_Ballon_Seil02;
    public PhysicsObject PE_Balloon_Ballon_Seil01;
    public PhysicsHinge PE_Balloon_Platform_HingeJoint;
    public PhysicsObject PE_Balloon_Platte00;
    public PhysicsObject PE_Balloon_Platte01;
    public PhysicsObject PE_Balloon_Platte02;
    public PhysicsObject PE_Balloon_Platte03;
    public PhysicsObject PE_Balloon_Platte04;
    public PhysicsObject PE_Balloon_Platte05;
    public PhysicsObject PE_Balloon_Platte06;
    public PhysicsObject PE_Balloon_Platte07;
    public PhysicsObject PE_Balloon_Platte08;
    public GameObject PE_Balloon_BallRefPos;
    public TiggerTester PE_Balloon_BallTigger;
    public bool EnableMusic = true;

    private bool BallTiggerActived = false;
    private bool _MusicActived = false;
    private int _ControlSpeedTimer = 0;
    private int _PlayMusicDelayTimer = 0;
    private GamePlayManager GamePlayManagerInstance;
    private MusicManager MusicManager;

    protected override void Start()
    {
      base.Start();

      GamePlayManagerInstance = GamePlayManager.Instance;
      MusicManager = GamePlayManagerInstance.MusicManager;

      if (!IsPreviewMode) {

        //接近PE_Balloon时禁用背景音乐
        var musicDisabledByPE_Balloon = false;
        var distanceChecker = PE_Balloon_Platform.GetComponent<DistanceChecker>();
        distanceChecker.Object2 = GamePlayManagerInstance.BallManager.PosFrame;
        distanceChecker.OnEnterRange = (go) => {
          if (_MusicActived) {
            musicDisabledByPE_Balloon = true;
            MusicManager.DisableBackgroundMusic();
          }
          if (IsActive)
            PE_Balloon_Platform.WakeUp();
        };
        distanceChecker.OnLeaveRange = (go) => {
          if (_MusicActived && musicDisabledByPE_Balloon && !GamePlayManagerInstance.CurrentLevelPass) {
            musicDisabledByPE_Balloon = false;
            MusicManager.EnableBackgroundMusic();
          } 
        };
        distanceChecker.CheckEnabled = true;

        //PE_Balloon 过关触发器
        PE_Balloon_BallTigger.onTriggerEnter = (body, other) => {
          if (IsActive && other && other.gameObject.tag == "Ball" && !BallTiggerActived) {

            Log.D("PE_Balloon", "Break bridge!") ;

            BallTiggerActived = true;
            _MusicActived = false;

            PE_Balloon_Platform_HingeJoint.Destroy(); //断开与桥的连接
            PE_Balloon_Platform.WakeUp();
            PE_Balloon_PlatformForce.enabled = false;
            PE_Balloon_BoxSlideForce.enabled = false;

            if (_ControlSpeedTimer > 0) {
              GameTimer.DeleteDelay(_ControlSpeedTimer);
              _ControlSpeedTimer = 0;
            }
            //速度放慢一些
            _ControlSpeedTimer = GameTimer.Delay(2.001f, () => {
              PE_Balloon_BoxSlideForce.enabled = true;
              PE_Balloon_BoxSlideForce.Force = 0.15f;

              //速度再放慢一些
              _ControlSpeedTimer = GameTimer.Delay(1.300f, () => {
                PE_Balloon_BoxSlideForce.Force = 0.1f;
                
                //速度再放慢一些
                _ControlSpeedTimer = GameTimer.Delay(40.000f, () => {
                  PE_Balloon_BoxSlideForce.Force = 0f;
                  _ControlSpeedTimer = 0;
                });
              });
            });

            if (!IsDebugMode)
              GamePlayManagerInstance.Pass(); //通知管理器关卡已结束
          }
        };

        _MusicActived = false;

        var iWoodOnlyHit = GamePlayManagerInstance.BallSoundManager.GetSoundCollIDByName("WoodOnlyHit");
        PE_Balloon_Platte00.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte01.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte02.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte03.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte04.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte05.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte06.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte07.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platte08.CollisionID = iWoodOnlyHit;
        PE_Balloon_Platform.CollisionID = GamePlayManagerInstance.BallSoundManager.GetSoundCollIDByName("Wood");
      }
    }

    public override void Active()
    {
      base.Active();
      gameObject.SetActive(true);

      BallTiggerActived = false;
      PE_Balloon_Platform.gameObject.SetActive(true);
      PE_Balloon_BoxSlide.gameObject.SetActive(true);
      PE_Balloon_Ballon04.gameObject.SetActive(true);
      PE_Balloon_Ballon03.gameObject.SetActive(true);
      PE_Balloon_Ballon02.gameObject.SetActive(true);
      PE_Balloon_Ballon01.gameObject.SetActive(true);
      PE_Balloon_Ballon_Seil04.gameObject.SetActive(true);
      PE_Balloon_Ballon_Seil03.gameObject.SetActive(true);
      PE_Balloon_Ballon_Seil02.gameObject.SetActive(true);
      PE_Balloon_Ballon_Seil01.gameObject.SetActive(true);
      PE_Balloon_Platte00.gameObject.SetActive(true);
      PE_Balloon_Platte01.gameObject.SetActive(true);
      PE_Balloon_Platte02.gameObject.SetActive(true);
      PE_Balloon_Platte03.gameObject.SetActive(true);
      PE_Balloon_Platte04.gameObject.SetActive(true);
      PE_Balloon_Platte05.gameObject.SetActive(true);
      PE_Balloon_Platte06.gameObject.SetActive(true);
      PE_Balloon_Platte07.gameObject.SetActive(true);
      PE_Balloon_Platte08.gameObject.SetActive(true);
      PE_Balloon_Platform.Physicalize();
      PE_Balloon_PlatformForce.enabled = true;
      PE_Balloon_PlatformForce.Force = 0.37f;
      PE_Balloon_BoxSlide.Physicalize();
      PE_Balloon_BoxSlideForce.enabled = true;
      PE_Balloon_BoxSlideForce.Force = 0.31f;
      PE_Balloon_Ballon04.Physicalize();
      PE_Balloon_Ballon03.Physicalize();
      PE_Balloon_Ballon02.Physicalize();
      PE_Balloon_Ballon01.Physicalize();
      PE_Balloon_Ballon_Seil04.Physicalize();
      PE_Balloon_Ballon_Seil03.Physicalize();
      PE_Balloon_Ballon_Seil02.Physicalize();
      PE_Balloon_Ballon_Seil01.Physicalize();
      PE_Balloon_Platte00.Physicalize();
      PE_Balloon_Platte01.Physicalize();
      PE_Balloon_Platte02.Physicalize();
      PE_Balloon_Platte03.Physicalize();
      PE_Balloon_Platte04.Physicalize();
      PE_Balloon_Platte05.Physicalize();
      PE_Balloon_Platte06.Physicalize();
      PE_Balloon_Platte07.Physicalize();
      PE_Balloon_Platte08.Physicalize();

      if (IsDebugMode) {
        Log.D("PE_Balloon", "Active!");
        Log.D("PE_Balloon", "State. " + _MusicActived + " CurrentSector. " + GamePlayManagerInstance.CurrentSector);
      }

      if (!_MusicActived && GamePlayManagerInstance.CurrentSector != 1) {
         
        if (IsDebugMode)
          Log.D("PE_Balloon", "_MusicActived !");

        _MusicActived = true;
        //播放最后一小节的音乐
        var _SoundLastSector = GamePlayManagerInstance.GetSoundLastSector();
        //设置为2d
        _SoundLastSector.spatialBlend = 0;
        if (EnableMusic)
        {
          _SoundLastSector.Play();
          MusicManager.DisableInSec(70);    
        }

        if (_PlayMusicDelayTimer > 0)
          GameTimer.DeleteDelay(_PlayMusicDelayTimer);
        _PlayMusicDelayTimer = GameTimer.Delay(5, () => {
          //该音乐播放5秒后淡出
          GameUIManager.Instance.UIFadeManager.AddAudioFadeOut(_SoundLastSector, 5);
          _PlayMusicDelayTimer = GameTimer.Delay(5, () => {
            _PlayMusicDelayTimer = 0;
            //播放一次完成之后设置为3d声音100m范围衰减，位置为当前飞船位置
            _SoundLastSector.gameObject.transform.position = transform.position;
            _SoundLastSector.spatialBlend = 1;
            _SoundLastSector.volume = 1;

          });
        });
      }
    }
    public override void Deactive()
    {
      PE_Balloon_BoxSlide.UnPhysicalize(true);
      PE_Balloon_Ballon04.UnPhysicalize(true);
      PE_Balloon_Ballon03.UnPhysicalize(true);
      PE_Balloon_Ballon02.UnPhysicalize(true);
      PE_Balloon_Ballon01.UnPhysicalize(true);
      PE_Balloon_Ballon_Seil04.UnPhysicalize(true);
      PE_Balloon_Ballon_Seil03.UnPhysicalize(true);
      PE_Balloon_Ballon_Seil02.UnPhysicalize(true);
      PE_Balloon_Ballon_Seil01.UnPhysicalize(true);
      PE_Balloon_Platte00.UnPhysicalize(true);
      PE_Balloon_Platte01.UnPhysicalize(true);
      PE_Balloon_Platte02.UnPhysicalize(true);
      PE_Balloon_Platte03.UnPhysicalize(true);
      PE_Balloon_Platte04.UnPhysicalize(true);
      PE_Balloon_Platte05.UnPhysicalize(true);
      PE_Balloon_Platte06.UnPhysicalize(true);
      PE_Balloon_Platte07.UnPhysicalize(true);
      PE_Balloon_Platte08.UnPhysicalize(true);
      PE_Balloon_Platform.UnPhysicalize(true);

      var soundLastStore = GamePlayManagerInstance.GetSoundLastSector();
      if (soundLastStore != null)
        soundLastStore.Stop();
      if (_PlayMusicDelayTimer > 0) {
        GameTimer.DeleteDelay(_PlayMusicDelayTimer);
        _PlayMusicDelayTimer = 0;
      }
      if (_ControlSpeedTimer > 0) {
        GameTimer.DeleteDelay(_ControlSpeedTimer);
        _ControlSpeedTimer = 0;
      }

      gameObject.SetActive(false);
      base.Deactive();
    }
    public override void Reset(ModulBaseResetType type)
    {
      GamePlayManagerInstance.GetSoundLastSector().Stop();
      ObjectStateBackupUtils.RestoreObjectAndChilds(gameObject);
      if (type == ModulBaseResetType.LevelRestart) 
        _MusicActived = false;

      if (_PlayMusicDelayTimer > 0) {
        GameTimer.DeleteDelay(_PlayMusicDelayTimer);
        _PlayMusicDelayTimer = 0;
      }
      if (_ControlSpeedTimer > 0) {
        GameTimer.DeleteDelay(_ControlSpeedTimer);
        _ControlSpeedTimer = 0;
      }
    }
    public override void Backup()
    {
      ObjectStateBackupUtils.BackUpObjectAndChilds(gameObject);
    }
    
    public override void DebugTestCommand(int command)
    {
      if (command == 1) {
        PE_Balloon_Platform_HingeJoint.Destroy(); //断开与桥的连接
        PE_Balloon_Platform.WakeUp();
        Log.D("PE_Balloon", "Break bridge!");
      } else if (command == 2) {
        Log.D("PE_Balloon", "Test Pass!");
        GamePlayManagerInstance.Pass(); //通知管理器关卡已结束
      }
    }    
    
    public override void ActiveForPreview()
    {
      gameObject.SetActive(true);
    }
    public override void DeactiveForPreview()
    {
      gameObject.SetActive(false);
    }

  }
}