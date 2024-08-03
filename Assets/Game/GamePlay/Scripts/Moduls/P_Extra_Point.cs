using Ballance2.Game.Utils;
using Ballance2.Services;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Moduls
{
  /// <summary>
  /// 额外分数组件
  /// </summary>
  public class P_Extra_Point : ModulBase 
  {
    public TiggerTester P_Extra_Point_Tigger;
    public GameObject P_Extra_Point_Floor;
    public GameObject[] P_Extra_Point_Ball = new GameObject[7];
    public SmoothFly[] P_Extra_Point_Target = new SmoothFly[7];
    public GameObject P_Extra_Point_Ball_Povit1;
    public GameObject P_Extra_Point_Ball_Povit2;
    public GameObject P_Extra_Point_Ball_Povit4;
    public GameObject P_Extra_Point_Ball_Povit5;
    public GameObject P_Extra_Point_Ball_Povit6;
    public GameObject P_Extra_Point_Fizz;
    public AudioSource P_Extra_Point_Sound;

    public bool _Rotate = false;
    public bool _FlyUp = false;
    public bool _FlyFollow = false;
    public float _RotDegree = 6f;
    public float _FlyUpTime = 0.4f;
    public float _FlyFollowTime = 0.3f;
    public float _FlyTargetUpTime = 0.3f;
    public float _FlyTargetFollowTime = 0.2f;

    private SmoothFly[] _P_Extra_Point_Ball_Fly = new SmoothFly[7];
    private GameManager.VoidDelegate[] _P_Extra_Point_Ball_Rest = new GameManager.VoidDelegate[7];
    private Vector3 _RotCenter;
    private Vector3 _RotAxis1;
    private Vector3 _RotAxis2;
    private Vector3 _RotAxis4;
    private Vector3 _RotAxis5;
    private Vector3 _RotAxis6;

    private bool _Actived = false;
    private bool _OnFloor = false;

    public P_Extra_Point() {
      AutoActiveBaseGameObject = false;
      EnableBallRangeChecker = true;
      BallCheckeRange = 200;
    }

    protected override void Start()
    {
      if (!IsPreviewMode) {
        //初始化小球
        for (int i = 1; i <= 6; i++)
          _P_Extra_Point_Ball_Fly[i] = P_Extra_Point_Ball[i].GetComponent<SmoothFly>();

        for(int i = 1; i <= 6; i++) {
          var fly = _P_Extra_Point_Ball_Fly[i];
          var hitAudio = transform.Find($"P_Extra_Point_Ball{i}/P_Extra_Point_Hit").GetComponent<AudioSource>();
          var hitFizz = transform.Find($"P_Extra_Point_Ball{i}/P_Extra_Point_Fizz").gameObject;
          var ballParticle = transform.Find($"P_Extra_Point_Ball{i}/P_Extra_Point_Ball").gameObject;
          var flowParticle = transform.Find($"P_Extra_Point_Ball{i}/P_Extra_Point_Flow").gameObject;
          var target = P_Extra_Point_Target[i];

          GameSoundManager.Instance.RegisterSoundPlayer(GameSoundType.Normal, hitAudio);
          _P_Extra_Point_Ball_Rest[i] = () => {
            ballParticle.SetActive(true);
            flowParticle.SetActive(true);
            hitFizz.SetActive(false);
            hitAudio.Stop();
          };
          
          fly.Type = SmoothFlyType.SmoothDamp;
          fly.TargetTransform = target.transform;
          fly.StopWhenArrival = true;
          fly.ArrivalDiatance = 2;
          fly.ArrivalCallback = (fly) => {
            if (!_FlyUp) {
              target.Fly = false; //停止追逐
              ballParticle.SetActive(false);
              flowParticle.SetActive(false);
              hitFizz.SetActive(true);
              hitAudio.Play();
              GamePlayManager.Instance.AddPoint(20); //小球是20分
            }
          };
        }

        P_Extra_Point_Tigger.onTriggerEnter = (_, otherBody) => {
          if (IsActive && ! _Actived && otherBody.tag == "Ball") {
            _Actived = true;
            StartFly();
            GamePlayManager.Instance.AddPoint(100); //大球是100分
          }
        };
      }
      //触发射线，检查当前下方是不是路面，如果是，则显示 Shadow
      RaycastHit hitinfo;
      if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hitinfo, 5) && hitinfo.collider != null) {
        var parentName = hitinfo.collider.gameObject.tag;
        if (parentName == "Phys_Floors" || (parentName == "Phys_FloorWoods" && parentName != "Phys_FloorRails"))
          _OnFloor = true;
        else
          _OnFloor = false;
      }
      else _OnFloor = false;

      _RotCenter = transform.position;
      _RotAxis1 = P_Extra_Point_Ball_Povit1.transform.up;
      _RotAxis2 = P_Extra_Point_Ball_Povit2.transform.up;
      _RotAxis4 = P_Extra_Point_Ball_Povit4.transform.up;
      _RotAxis5 = P_Extra_Point_Ball_Povit5.transform.up;
      _RotAxis6 = P_Extra_Point_Ball_Povit6.transform.up;
      P_Extra_Point_Ball_Povit1.SetActive(false);
      P_Extra_Point_Ball_Povit2.SetActive(false);
      P_Extra_Point_Ball_Povit4.SetActive(false);
      P_Extra_Point_Ball_Povit5.SetActive(false);
      P_Extra_Point_Ball_Povit6.SetActive(false);

      base.Start();
    }

    private void StartFly() {
      _Rotate = false;
      _FlyUp = true;

      P_Extra_Point_Floor.SetActive(false);
      P_Extra_Point_Ball[0].SetActive(false);
      P_Extra_Point_Fizz.SetActive(true);
      P_Extra_Point_Sound.Play();

      //目标平移到指定位置

      var upY = 30.0f;

      for(int i = 1; i <= 6; i++) {

        var ball = P_Extra_Point_Ball[i];
        var flyTarget = P_Extra_Point_Target[i];
        var fly = _P_Extra_Point_Ball_Fly[i];
        var posMult = new Vector3(1.2f, 1.0f, 1.2f);
        var pos = ball.transform.localPosition;
        var localPos = Vector3.Scale(pos, posMult);

        flyTarget.transform.localPosition = pos;
        flyTarget.CurrentVelocity = Vector3.zero;
        flyTarget.TargetTransform = null;
        flyTarget.TargetPos = transform.TransformPoint(localPos.x, upY, localPos.z);
        flyTarget.Fly = true;
        flyTarget.Time = _FlyTargetUpTime;

        fly.Time = _FlyUpTime;
        fly.CurrentVelocity = Vector3.zero;
        fly.StopWhenArrival = false;
        fly.Fly = true;

        upY = upY + 0.3f;
      }

      var fTime =  _FlyFollowTime;

      GameTimer.Delay(0.750f, () => {
        for(int i = 1; i <= 6; i++) {
          var fly = P_Extra_Point_Target[i];
          fly.TargetPos = Vector3.zero;
          fly.TargetTransform = GamePlayManager.Instance.CamManager.Target;
          fly.Time = _FlyTargetFollowTime;
          fly.CurrentVelocity = Vector3.zero;
        }
      });
      GameTimer.Delay(1, () => {
        P_Extra_Point_Fizz.SetActive(false);
        _FlyUp = false;
        for(int i = 1; i <= 6; i++) {
          var fly = _P_Extra_Point_Ball_Fly[i];
          fly.StopWhenArrival = true;
          fly.Fly = true;
          fly.Time = fTime;
          fTime = fTime - 0.03f;
        }
      });
    }

    private void Update() {
      //旋转小球
      if (_Rotate) {
        var rotate = _RotDegree * Time.deltaTime;
        P_Extra_Point_Ball[1].transform.RotateAround(_RotCenter, _RotAxis1, rotate);
        P_Extra_Point_Ball[2].transform.RotateAround(_RotCenter, _RotAxis2, rotate);;
        P_Extra_Point_Ball[3].transform.RotateAround(_RotCenter, _RotAxis1, -rotate);
        P_Extra_Point_Ball[4].transform.RotateAround(_RotCenter, _RotAxis4, rotate);
        P_Extra_Point_Ball[5].transform.RotateAround(_RotCenter, _RotAxis5, rotate);
        P_Extra_Point_Ball[6].transform.RotateAround(_RotCenter, _RotAxis6, rotate);
      } 
    }

    public override void Active()
    {
      base.Active();

      if (!_Actived) {
        gameObject.SetActive(true);
        P_Extra_Point_Floor.SetActive(_OnFloor);
        for(int i = 1; i <= 6; i++)
          P_Extra_Point_Ball[i].SetActive(true);
        _FlyUp = false;
        _FlyFollow = false;
        if (BallInRange)
          _Rotate = true;
      }
    }
    public override void Deactive()
    {
      base.Deactive();

      _Rotate = false;
      gameObject.SetActive(false);
    }
    public override void Reset(ModulBaseResetType type)
    {
      if (type == ModulBaseResetType.LevelRestart) {
        _Actived = false;
        P_Extra_Point_Ball[0].SetActive(true);
        for (int i = 1; i <= 6; i++) {
          _P_Extra_Point_Ball_Fly[i].Fly = false;
          ObjectStateBackupUtils.RestoreObject(P_Extra_Point_Ball[i]);
          _P_Extra_Point_Ball_Rest[i]();
        }
      }
    }
    public override void Backup()
    {
      for (int i = 1; i <= 6; i++) 
        ObjectStateBackupUtils.BackUpObject(P_Extra_Point_Ball[i]);
    }
    public override void ActiveForPreview()
    {
      this.Active();
    }
    public override void DeactiveForPreview()
    {
      this.Deactive();
    }
    protected override void BallEnterRange() {
      if (IsActive && ! _Actived && !_Rotate) //进入范围后才旋转
        _Rotate = true;
    }   
    protected override void BallLeaveRange() {
      if (IsActive && !_Actived && _Rotate) //离开范围后停止旋转
        _Rotate = false;
    }
  }
}