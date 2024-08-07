using System.Collections.Generic;
using Ballance2.Base;
using Ballance2.Game.GamePlay.Balls;
using Ballance2.Game.Utils;
using Ballance2.Menu;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Utils;
using BallancePhysics.Wapper;
using UnityEngine;
using UnityEngine.Animations;

namespace Ballance2.Game.GamePlay 
{
  /// <summary>
  /// 球管理器，负责管理球的注册、运动控制、特殊效果等等。
  /// </summary>
  public class BallManager : MonoBehaviour 
  {
    private const string TAG = "BallManager";

    public BallLightningSphere _BallLightningSphere;
    public GameObject _BallWood;
    public GameObject _BallStone;
    public GameObject _BallPaper;
    public GameObject _BallSmoke;
    public ParentConstraint _BallShadowProjector;

    public BallPiecesControllManager BallPiecesControllManager;

    /// <summary>
    /// 获取当前的球名称 [R]
    /// </summary>
    public string CurrentBallName { get; private set; }
    /// <summary>
    /// 获取当前的球 [R]
    /// </summary>
    public Ball CurrentBall { get; private set; }
    /// <summary>
    /// 获取或者设置当前球的推动方向 [RW]
    /// </summary>
    public BallPushType PushType = BallPushType.None; 
    /// <summary>
    /// 获取或者设置当前用户是否可以控制球 [RW]
    /// </summary>
    public bool CanControll = false; 
    /// <summary>
    /// 获取或者设置当前用户是否可以控制摄像机 [RW]
    /// </summary>
    public bool CanControllCamera = false;  
    /// <summary>
    /// 获取当前用户是否按下Shift键 [R]
    /// </summary>
    public bool ShiftPressed = false; 
    /// <summary>
    /// 获取上升按键状态 [RW]
    /// </summary>
    public bool KeyStateUp = false;  
    /// <summary>
    /// 获取下降按键状态 [RW]
    /// </summary>
    public bool KeyStateDown = false;   
    /// <summary>
    /// 获取前进按键状态 [RW]
    /// </summary>
    public bool KeyStateForward = false;   
    /// <summary>
    /// 获取后退按键状态 [RW]
    /// </summary>
    public bool KeyStateBack = false;   
    /// <summary>
    /// 获取左按键状态 [RW]
    /// </summary>
    public bool KeyStateLeft = false;   
    /// <summary>
    /// 获取右按键状态 [RW]
    /// </summary>
    public bool KeyStateRight = false;   
    /// <summary>
    /// 球速度倍数，默认1 [RW]
    /// </summary>
    public float BallSpeedFactor = 1; 
    /// <summary>
    /// 获取当前球的位置变换实例 [R]
    /// </summary>
    public Transform PosFrame { 
      get {
        return GamePlayManager.Instance.CamManager._CamOrientTransform;
      }
    }

    private void Awake() {
      _AddCommands();
      _InitEvents();
      _InitSettings();
      _InitKeys();

      //注册内置球
      RegisterBall("BallWood", _BallWood);
      RegisterBall("BallStone", _BallStone);
      RegisterBall("BallPaper", _BallPaper);
    }
    private void OnDestroy() {
      _DeleteCommands();
      _DeleteEvents();
      _UnInitSettings();
      _UnInitKeys();
      _CollectRegisterBalls();
    }

    #region 设置初始化

    private int settingsCallbackId = 0;
    private bool reverseRotation = false;
    private bool _LeftPressed = false;
    private bool _RightPressed = false;
    private void _InitSettings() {
      //初始化控制设置
      var GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      settingsCallbackId = GameSettings.RegisterSettingsUpdateCallback(SettingConstants.SettingsControl, (groupName, action) => {
        _OnControlSettingsChanged();
        return false;
      });
    }
    private void _UnInitSettings() {
      var GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      GameSettings?.UnRegisterSettingsUpdateCallback(settingsCallbackId);
    }
    private void _OnControlSettingsChanged() {
      //当设置更改时或加载时，更新设置到当前变量
      var GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      reverseRotation = GameSettings.GetBool(SettingConstants.SettingsControlKeyReverse);
    }
    
    #endregion

    #region 键盘

    private List<InputSystemUtil.BindInputActionHolder> bindInputActionHolders = new List<InputSystemUtil.BindInputActionHolder>();
    private void _InitKeys() {
      //初始化控制器
      var controller = ControlManager.Instance;

      bindInputActionHolders.Add(controller.ControllerActionMove.BindInputAction(
        (context) => {
          var val = context.ReadValue<Vector2>();
          SetBallPushValue(val.x, val.y);
        }, (context) => {
          SetBallPushValue(0, 0);
        })
      );
      bindInputActionHolders.Add(controller.ControllerActionFly.BindInputAction(
        (context) => {
          if (GameManager.DebugMode)
          {
            var val = context.ReadValue<float>();
            KeyStateUp = val > 0.3f;
            KeyStateDown = val < 0.3f;
            FlushBallPush();
          }
        }, (context) => {
          if (GameManager.DebugMode)
          {
            KeyStateUp = false;
            KeyStateDown = false;
            FlushBallPush();
          }
        })
      );
      bindInputActionHolders.Add(controller.ControllerActionOverlook.BindInputActionButton((v) => _CamOverlook(v)));
      bindInputActionHolders.Add(controller.ControllerActionRotateCamLeft.BindInputActionButton((v) => {
        if (v) _CamRotateLeft();
      }, false));
      bindInputActionHolders.Add(controller.ControllerActionRotateCamRight.BindInputActionButton((v) => {
        if (v) _CamRotateRight();
      }, false));
      
      bindInputActionHolders.Add(controller.KeyBoardActionForward.BindInputActionButton((v) => _UpArrow_Key(v)));
      bindInputActionHolders.Add(controller.KeyBoardActionBack.BindInputActionButton((v) => _DownArrow_Key(v)));
      bindInputActionHolders.Add(controller.KeyBoardActionLeft.BindInputActionButton((v) => _LeftArrow_Key(v)));
      bindInputActionHolders.Add(controller.KeyBoardActionRight.BindInputActionButton((v) => _RightArrow_Key(v)));
      bindInputActionHolders.Add(controller.KeyBoardActionOverlook.BindInputActionButton((v) => _CamOverlook(v)));
      bindInputActionHolders.Add(controller.KeyBoardActionDown.BindInputActionButton((v) => _Up_Key(v)));
      bindInputActionHolders.Add(controller.KeyBoardActionUp.BindInputActionButton((v) => _Down_Key(v)));
      bindInputActionHolders.Add(controller.KeyBoardActionRotateCam.BindInputActionButton((v) => _Shift_Key(v)));

      bindInputActionHolders.Add(controller.KeyBoardTestBallWood.BindInputActionButton((v) => {
        if (v && GameManager.DebugMode) {
          SetNextRecoverPosToNowPos();
          SetCurrentBall("BallWood", BallControlStatus.Control);
        }
      }));
      bindInputActionHolders.Add(controller.KeyBoardTestBallStone.BindInputActionButton((v) => {
        if (v && GameManager.DebugMode) {
          SetNextRecoverPosToNowPos();
          SetCurrentBall("BallStone", BallControlStatus.Control);
        }
      }));
      bindInputActionHolders.Add(controller.KeyBoardTestBallPaper.BindInputActionButton((v) => {
        if (v && GameManager.DebugMode) {
          SetNextRecoverPosToNowPos();
          SetCurrentBall("BallPaper", BallControlStatus.Control);
        }
      }));
      bindInputActionHolders.Add(controller.KeyBoardTestReset.BindInputActionButton((v) => {
        if (v && GameManager.DebugMode) {
          SetControllingStatus(BallControlStatus.NoControl);
          SetNextRecoverPos(Vector3.zero);
        }
      }));
    }
    private void _UnInitKeys() 
    {
      var controller = ControlManager.Instance;
      if (controller != null)
      {
        foreach(var v in bindInputActionHolders)
          v.Delete();
        bindInputActionHolders.Clear();

        controller.DisableControl();
      }
    }
    private void _ReSendPressingKey() 
    {
      foreach(var v in bindInputActionHolders)
        v.ResendIfPressing();
    }

    #endregion

    #region 指令

    private int _CommandId = 0;
    
    private void _AddCommands() 
    {
      //注册控制台指令
      _CommandId = GameManager.Instance.GameDebugCommandServer.RegisterCommand("balls", (keyword, fullCmd, argsCount, args) => {
        var type = args[1];
        if (type == "play-lighting")
          PlayLighting(nextRecoverPos, true, true, null);
        else if (type == "set-recover-pos")
        {
          if (!DebugUtils.CheckIntDebugParam(1, args, out var nx, true, 0))
            return false;
          if (!DebugUtils.CheckIntDebugParam(2, args, out var ny, true, 0))
            return false;
          if (!DebugUtils.CheckIntDebugParam(3, args, out var nz, true, 0))
            return false;

          SetNextRecoverPos(new Vector3(nx, ny, nz));
          Log.D(TAG, "NextRecoverPos now is : {0}", new Vector3(nx, ny, nz));
        }
        else if (type == "set-ball")
        {
          if (!DebugUtils.CheckDebugParam(1, args, out var n, true, ""))
            return false;

          SetCurrentBall(n);
          Log.D(TAG, "Set ball type to : {0}", n);
        }
        else if (type == "set-control-status")
        {
          if (!DebugUtils.CheckIntDebugParam(1, args, out var n, true, 1))
            return false;

          SetControllingStatus((BallControlStatus)n);
          Log.D(TAG, "Set control status to : {0}", n);
        }
        return true;
      }, 0, "balls <next/set/reset/reset-all> 球管理器命令" +
          "  set-recover-pos <x:number> <y:number> <z:number> ▶ 设置下次球激活位置" +
          "  set-ball <ballName:string>                       ▶ 设置当前激活球" +
          "  set-control-status <status:number>               ▶ 设置当前控制模式 status: 0 无控制 1 正常控制 2 释放模式 3 锁定模式 4 释放模式2" +
          "  play-lighting                                    ▶ 播放出生动画");
    }
    private void _DeleteCommands() 
    {
      GameManager.Instance?.GameDebugCommandServer.UnRegisterCommand(_CommandId);
    }

    #endregion

    #region 事件

    /// <summary>
    /// 新球注册事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventBallRegistered;
    /// <summary>
    /// 球删除注册事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventBallUnRegister;
    /// <summary>
    /// 当前球变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventCurrentBallChanged;
    /// <summary>
    /// 球的下一个出生位置变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventNextRecoverPosChanged;
    /// <summary>
    /// 球控制状态变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventControllingStatusChanged;
    /// <summary>
    /// 播放烟雾事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventPlaySmoke;
    /// <summary>
    /// 播放闪电事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventPlayLighting;
    /// <summary>
    /// 刷新球推动力状态事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventFlushBallPush;
    /// <summary>
    /// 球推动力数值手动更新事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventSetBallPushValue;
    /// <summary>
    /// 清除球推动力事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventRemoveAllBallPush;

    private void _InitEvents()  
    {
      var events = GameManager.GameMediator.RegisterEventEmitter("BallManager");
      this.EventBallRegistered = events.RegisterEvent("BallRegistered") ;
      this.EventBallUnRegister = events.RegisterEvent("BallUnRegister");
      this.EventCurrentBallChanged = events.RegisterEvent("CurrentBallChanged");
      this.EventNextRecoverPosChanged = events.RegisterEvent("NextRecoverPosChanged");
      this.EventControllingStatusChanged = events.RegisterEvent("ControllingStatusChanged");
      this.EventPlaySmoke = events.RegisterEvent("PlaySmoke");
      this.EventPlayLighting = events.RegisterEvent("PlayLighting");
      this.EventFlushBallPush = events.RegisterEvent("FlushBallPush");
      this.EventSetBallPushValue = events.RegisterEvent("SetBallPushValue");
      this.EventRemoveAllBallPush = events.RegisterEvent("RemoveAllBallPush");
    }
    private void _DeleteEvents() 
    {
      GameManager.GameMediator?.UnRegisterEventEmitter("BallManager");
    }

    #endregion

    #region 球注册

    private Dictionary<string, BallRegStorage> registerBalls = new Dictionary<string, BallRegStorage>();

    private void _CollectRegisterBalls() {
      //清除碎片的定时器
      foreach(var value in registerBalls.Values) {
        if (value.ball._PiecesData != null) 
        {
          var data = value.ball._PiecesData;
          if (data.delayHideTimerID > 0)
          {
            GameTimer.DeleteDelay(data.delayHideTimerID);
            data.delayHideTimerID = 0;
          }
          if (data.fadeOutTimerID > 0)
          {
            GameTimer.DeleteDelay(data.fadeOutTimerID);
            data.fadeOutTimerID = 0;
          }
        }
      }
      //清除
      registerBalls.Clear();
    }

    /// <summary>
    /// 注册球
    /// </summary>
    /// <param name="name">球名称</param>
    /// <param name="gameObject">球游戏对象，必须已经添加到场景中</param>
    public void RegisterBall(string name, GameObject gameObject)
    {
      //检查是否注册
      if(GetRegisterBall(name) != null) 
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, "Ball {0} already registered", name);
        return;
      }

      //添加刚体组件
      PhysicsObject body = gameObject.AddComponent<PhysicsObject>();
      //查找物理参数
      if(!GamePhysBall.Data.TryGetValue(name, out var physicsData))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotFound, TAG, "Not fuoud GamePhysBall data for Ball {0} , please add it before call RegisterBall", name);
        return;
      }
      body.DoNotAutoCreateAtAwake = true;
      //设置物理参数
      body.Fixed = false;
      body.Mass = physicsData.Mass;
      if (physicsData.BallRadius > 0)
      {
        body.UseBall = true;
        body.BallRadius = physicsData.BallRadius;
      }
      else
      {
        body.UseBall = false;
      }
      body.Friction = physicsData.Friction;
      body.Elasticity = physicsData.Elasticity;
      body.RotSpeedDamping = physicsData.RotDamp;
      body.LinearSpeedDamping = physicsData.LinearDamp;
      body.Layer = physicsData.Layer;
      body.EnableCollision = true;
      body.AutoControlActive = false;
      body.AutoMassCenter = false;
      body.UseExistsSurface = true;
      body.CustomDePhysicsFall = true;
      body.OnFallCallback = (body) => {
        if (!GamePlayManager.Instance.Fall()) {
          body.UnPhysicalize(true);
          body.gameObject.SetActive(false);
        }
      };

      //设置恒力
      body.EnableConstantForce = true;

      //添加速度计
      var speedMeter = gameObject.GetComponent<SpeedMeter>();
      if(speedMeter == null) {
        speedMeter = gameObject.AddComponent<SpeedMeter>();
        speedMeter.Enabled = true;
      }

      Ball ball = gameObject.GetComponent<Ball>();
      if(ball == null) 
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.ClassNotFound, TAG, "Not found Ball class on {0} !", name);
        return;
      }

      var pieces = ball.GetPieces();
      //设置球相关物理参数
      ball._PiecesPhysicsData = physicsData.PiecesPhysicsData;
      //设置推动物理参数
      ball._PiecesMinForce = physicsData.PiecesMinForce;
      ball._PiecesMaxForce = physicsData.PiecesMaxForce;
      ball._Force = physicsData.Force * physicsData.ForceMultiplier;
      ball._UpForce = physicsData.UpForce * 1.5f;
      ball._DownForce = physicsData.DownForce;
      if(pieces != null)
        ObjectStateBackupUtils.BackUpObjectAndChilds(pieces); //备份碎片的状态

      //还需要设置一个Unity的碰撞器，用于死亡区的检测
      var collder = gameObject.AddComponent<SphereCollider>();
      collder.radius = physicsData.TiggerBallRadius;
      var rigidbody = gameObject.AddComponent<Rigidbody>() ;
      rigidbody.isKinematic = true;
      rigidbody.useGravity = false;

      //设置名称
      if(gameObject.name != name) 
        gameObject.name = name;

      EventBallRegistered.Emit(ball, body);

      //添加信息
      registerBalls[name] = new BallRegStorage() {
        name = name,
        ball = ball,
        rigidbody = body,
        speedMeter = speedMeter,
      };
    }
    /// <summary>
    /// 取消注册球
    /// </summary>
    /// <param name="name">球名称</param>
    public bool UnRegisterBall(string name)
    {
      if(registerBalls.TryGetValue(name, out var ball)) 
      {
        if (ball == currentActiveBall)
          _DeactiveCurrentBall();

        registerBalls.Remove(name);
        EventBallUnRegister.Emit(name);
        return true;
      } 
      GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, "Ball {0} not register", name);
      return false;
    }
    /// <summary>
    /// 获取注册了的球
    /// </summary>
    /// <param name="name">球名称</param>
    public BallRegStorage GetRegisterBall(string name)
    {
      if(registerBalls.TryGetValue(name, out var ball))
        return ball;
      return null;
    }
    /// <summary>
    /// 获取所有注册的球
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, BallRegStorage>.ValueCollection GetRegisteredBalls()
    {
      return registerBalls.Values;
    }

    #endregion
    
    private BallRegStorage currentBall = null;
    private BallRegStorage currentActiveBall = null;
    private BallControlStatus controllingStatus = BallControlStatus.NoControl;
    private Vector3 nextRecoverPos = Vector3.zero;

    /// <summary>
    /// 获取当前激活的球
    /// </summary>
    public BallRegStorage GetCurrentBall() { return currentBall; }
    /// <summary>
    /// 设置当前正在控制的球
    /// </summary>
    /// <param name="name">球名称，不可为空</param>
    /// <param name="status">同时设置新的控制状态, 如果为空，则保持之前的控制状态</param>
    public void SetCurrentBall(string name, BallControlStatus status = BallControlStatus.KeepOldStatus)
    {
      if(string.IsNullOrEmpty(name)) {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, "You must provide a name for the ball");
        return;
      }
      var ball = GetRegisterBall(name);
      if(ball == null) {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, "Ball {0} not register", name);
        return;
      }
      if(currentBall != ball) {
        _DeactiveCurrentBall();
        currentBall = ball;
        CurrentBall = ball.ball;
        CurrentBallName = ball.name;
        EventCurrentBallChanged.Emit(name);
        SetControllingStatus(status);
      }
    }
    /// <summary>
    /// 设置禁用当前正在控制的球
    /// </summary>
    public void SetNoCurrentBall()
    {
      EventCurrentBallChanged.Emit("");
      _DeactiveCurrentBall();
    }
    /// <summary>
    /// 设置当前球的控制的状态
    /// </summary>
    /// <param name="status">新的状态值,为 KeepOldStatus 时不改变状态只刷新</param>
    public void SetControllingStatus(BallControlStatus status = BallControlStatus.KeepOldStatus)
    {
      if(status != BallControlStatus.KeepOldStatus) { 
        controllingStatus = status;
        _FlushCurrentBallAllStatus();
      }
      else if(status != controllingStatus) {
        _FlushCurrentBallAllStatus();
      }
    }
    /// <summary>
    /// 获取当前球的控制的状态
    /// </summary>
    public BallControlStatus GetControllingStatus()
    {
      return controllingStatus;
    }
    /// <summary>
    /// 设置下一次球出生位置
    /// </summary>
    /// <param name="pos">出生位置</param>
    public void SetNextRecoverPos(Vector3 pos)
    {
      EventNextRecoverPosChanged.Emit(pos);
      nextRecoverPos = pos;
    }
    /// <summary>
    /// 设置下一次球出生位置为当前球位置
    /// </summary>
    public void SetNextRecoverPosToNowPos()
    {
      var current = currentActiveBall;
      if (current != null)
        SetNextRecoverPos(current.ball.transform.position);
    }
    /// <summary>
    /// 重置指定球的碎片
    /// </summary>
    /// <param name="typeName">球名称，不可为空</param>
    public void ResetPeices(string typeName)
    {
      var ball = GetRegisterBall(typeName);
      if (ball != null)
        ball.ball.ResetPieces();
      else
        Log.W(TAG, "Ball type {0} not found", typeName);
    }
    /// <summary>
    /// 在下次激活位置（NextRecoverPos）抛出指定球的碎片
    /// </summary>
    /// <param name="typeName">球名称，不可为空</param>
    public void ThrowPeices(string typeName)
    {
      ThrowPeices(typeName, nextRecoverPos);
    }
    /// <summary>
    /// 抛出指定球的碎片
    /// </summary>
    /// <param name="typeName">球名称，不可为空</param>
    /// <param name="pos">抛出位置</param>
    public void ThrowPeices(string typeName, Vector3 pos)
    {
      var ball = GetRegisterBall(typeName);
      if (ball != null)
        ball.ball.ThrowPieces(pos);
      else
        Log.W(TAG, "Ball type {0} not found", typeName);
    }
    /// <summary>
    /// 恢复摄像机相关移动
    /// </summary>
    public void StartCamMove()
    {
      if (GamePlayManager.Instance.CamManager.Target == null)
        GamePlayManager.Instance.CamManager.SetTarget(currentBall.ball.transform, true);
    }
    /// <summary>
    /// 停止摄像机相关移动
    /// </summary>
    public void StopCamMove()
    {
      if (GamePlayManager.Instance.CamManager.Target == null)
        GamePlayManager.Instance.CamManager.SetTarget(null, false);
    }
    /// <summary>
    /// 重置所有碎片状态
    /// </summary>
    public void ResetAllPeices() {
      foreach (var item in registerBalls)
        item.Value.ball.ResetPieces();
    }

    #region 球状态工作方法

    private int debugFlushInfoTimer = 0;

    private void _SetStateDebugStatus(string stat) {
      if (GameManager.DebugMode && GamePlayUIControl.Instance.DebugStatValues.ContainsKey("CurrentStatus")) {
        GamePlayUIControl.Instance.DebugStatValues["CurrentStatus"].Value = stat;
      }
    }
    private void _FlushCurrentBallAllStatus() 
    {
      var CamManager = GamePlayManager.Instance.CamManager;

      if (controllingStatus == BallControlStatus.NoControl)
        ControlManager.Instance.DisableControl();
      else
        ControlManager.Instance.EnableControl();

      switch (controllingStatus) {
        case BallControlStatus.NoControl: {
          _SetCanControl(false);
          CanControllCamera = false;
          _DeactiveCurrentBall();
          CamManager.DisbleAll();
          _SetStateDebugStatus("NoControl");
          break;
        }
        case BallControlStatus.Control: {
          _SetCanControl(true);
          CanControllCamera = true;
          _ActiveCurrentBall();
          _PhysicsOrDePhysicsCurrentBall(true);
          CamManager.SetCamLook(true);
          CamManager.SetCamFollow(true);
          _SetStateDebugStatus("Control");
          break;
        }
        case BallControlStatus.LockMode: {
          _SetCanControl(false);
          CanControllCamera = true;
          _ActiveCurrentBall();
          _PhysicsOrDePhysicsCurrentBall(false);
          CamManager.SetCamLook(true);
          CamManager.SetCamFollow(true);
          _SetStateDebugStatus("LockMode");
          break;
        }
        case BallControlStatus.UnleashingMode: {
          _SetCanControl(false);
          CanControllCamera = true;
          _ActiveCurrentBall();
          _PhysicsOrDePhysicsCurrentBall(true);
          CamManager.SetCamLook(true);
          CamManager.SetCamFollow(false);
          _SetStateDebugStatus("UnleashingMode");
          break;
        }
        case BallControlStatus.FreeMode: {
          _SetCanControl(false);
          CanControllCamera = true;
          _ActiveCurrentBall();
          _PhysicsOrDePhysicsCurrentBall(true);
          CamManager.SetCamLook(true);
          CamManager.SetCamFollow(true);;
          _SetStateDebugStatus("FreeMode");
          break;
        }
        case BallControlStatus.LockLookMode: {
          _SetCanControl(false);
          CanControllCamera = false;
          _ActiveCurrentBall();
          _PhysicsOrDePhysicsCurrentBall(false);
          CamManager.SetCamLook(true);
          CamManager.SetCamFollow(false);
          _SetStateDebugStatus("LockLookMode");
          break;
        }
        default: {
          Log.W(TAG, "Bad ball control state {0}", controllingStatus);
          break;
        }
      }
      EventControllingStatusChanged.Emit(null);
    }
    private void _SetCanControl(bool can) { CanControll = can; }
    /// <summary>
    /// 取消激活当前的球
    /// </summary>
    private void _DeactiveCurrentBall() 
    {
      var current = currentActiveBall;
      if (current != null) {
        //隐藏阴影
        _BallShadowProjector.constraintActive = false;
        if (_BallShadowProjector.gameObject.activeSelf)
          _BallShadowProjector.gameObject.SetActive(false);
        //清除力
        RemoveAllBallPush();
        //取消激活
        if (current.rigidbody.IsPhysicalized)
          _PhysicsOrDePhysicsCurrentBall(false);
        current.ball.gameObject.SetActive(false);
        //清空摄像机跟随对象
        GamePlayManager.Instance.CamManager.SetTarget(null, false);
        currentActiveBall = null;

        if (GameManager.DebugMode)
          GamePlayUIControl.Instance.DebugStatValues["CurrentBall"].Value = "None";
      }
      if (GameManager.DebugMode) { 
        //删除定时器
        if (debugFlushInfoTimer > 0) {
          GameTimer.DeleteTimer(debugFlushInfoTimer);
          debugFlushInfoTimer = 0;
        }
      }
    }
    private void _ActiveCurrentBall() 
    {
      var current = currentActiveBall;
      if (current == null && currentBall != null) {
        current = currentBall;
        var currentTransform = current.ball.transform;
        currentActiveBall = currentBall;
        //设置阴影位置与父级约束    
        var constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = currentTransform;
        constraintSource.weight = 1;
        _BallShadowProjector.SetSource(0, constraintSource);
        _BallShadowProjector.constraintActive = true;
        //显示阴影
        if (!_BallShadowProjector.gameObject.activeSelf)
          _BallShadowProjector.gameObject.SetActive(true);
        //设置位置
        currentTransform.position = nextRecoverPos;
        //设置摄像机跟随对象
        GamePlayManager.Instance.CamManager.SetTarget(currentTransform, false);
        //清除力
        RemoveAllBallPush();
        //调试信息
        if (GameManager.DebugMode) { 
          var _DebugStatValues = GamePlayUIControl.Instance.DebugStatValues;
          var Position = _DebugStatValues["Position"];
          var Rotation = _DebugStatValues["Rotation"];
          var Velocity = _DebugStatValues["Velocity"];
          var PushValue = _DebugStatValues["PushValue"];
          var PhysicsTime = _DebugStatValues["PhysicsTime"];
          var PhysicsObjects = _DebugStatValues["PhysicsObjects"];
          var PhysicsState = _DebugStatValues["PhysicsState"];
          var GamePhysicsWorld = GamePlayManager.Instance.GamePhysicsWorld;
          _DebugStatValues["CurrentBall"].Value = current.name ;
          
          //删除定时器
          if (debugFlushInfoTimer > 0) {
            GameTimer.DeleteTimer(debugFlushInfoTimer);
            debugFlushInfoTimer = 0;
          }
          //每秒更新球位置调试显示数据
          debugFlushInfoTimer = GameTimer.Timer(0.5f, () => {

            if (!GameManager.Instance.GameSettings.GetBool("debugDisableBallInfo", false)) {
              //球位置
              Position.SetVector3Value(currentTransform.position);
              Rotation.SetVector3Value(currentTransform.eulerAngles);
            }
            if (!GameManager.Instance.GameSettings.GetBool("debugDisablePhysicsInfo", false)) {
              //物理时间
              PhysicsTime.Value = string.Format("{0:F2} ms", GamePhysicsWorld.PhysicsTime * 1000);
              //物理对象信息
              PhysicsObjects.Value = string.Format("All/Active/Update {0}/{1}/{2}\nFixed/FallCollect/Push {3}/{4}/{5}\nPhysicsForceFactor {6}", 
                GamePhysicsWorld.PhysicsBodies, 
                GamePhysicsWorld.PhysicsActiveBodies, 
                GamePhysicsWorld.PhysicsUpdateBodies,
                GamePhysicsWorld.PhysicsFixedBodies,
                GamePhysicsWorld.PhysicsFallCollectBodies, 
                GamePhysicsWorld.PhysicsConstantPushBodies,
                GamePhysicsWorld.PhysicsForceFactor
              );
            }
            if (!GameManager.Instance.GameSettings.GetBool("debugDisableBallInfo", false)) {
              //球的速度和推动数据
              if (current.rigidbody.IsPhysicalized) {
                PhysicsState.Value = string.Format("{0:F2} ContractState: {1}\n{2}", 
                  current.speedMeter.NowAbsoluteSpeed, 
                  current.rigidbody.ContractCacheString,
                  current.ball._SoundConfig._SoundManagerDebugString.ToString()
                );

                Velocity.SetVector3Value(current.rigidbody.SpeedVector);
                PushValue.Value = string.Format("{0:F2}, {1:F2}, {2:F2})", 
                  current.pushForceX.Force, 
                  current.pushForceY.Force,
                  current.pushForceZ.Force
                );
              }
              else
              {
                PhysicsState.Value = "";
                PushValue.Value = "";
                Velocity.Value = "";
              }
            }
          });
        }
      }
    }
    private void _PhysicsOrDePhysicsCurrentBall(bool physics) 
    {
      var CamManager = GamePlayManager.Instance.CamManager;
      var current = currentActiveBall;
      if (current != null) {
        //激活
        var physicsed = current.rigidbody.IsPhysicalized;
        current.ball.gameObject.SetActive(true);
        if (physics && !physicsed) {
          current.rigidbody.Physicalize() ;
          current.rigidbody.ClearConstantForce();
          current.rigidbody.WakeUp();
          //添加球的xyz推动恒力
          current.pushForceX = current.rigidbody.AddConstantForceWithPositionAndRef(0, Vector3.right, Vector3.zero, CamManager.CamDirectionRef, current.ball.transform);
          current.pushForceZ = current.rigidbody.AddConstantForceWithPositionAndRef(0, Vector3.forward, Vector3.zero, CamManager.CamDirectionRef, current.ball.transform);
          current.pushForceY = current.rigidbody.AddConstantForceWithPositionAndRef(0, Vector3.up, Vector3.zero, CamManager.CamDirectionRef, current.ball.transform);
          //激活
          current.ball.Active();
          //启动球的声音
          GamePlayManager.Instance.BallSoundManager.AddSoundableBall(current.rigidbody, current.ball._SoundConfig);
          //取消推动
          RemoveAllBallPush();
          //需要重新发送按键状态，因为可能在变球时还是按住按键，而此时球已经切换了，球的恒力需要重新设置
          _ReSendPressingKey();
        }
        if (!physics) {
          //取消推动
          RemoveAllBallPush();
          //清除正在播放的声音
          GamePlayManager.Instance.BallSoundManager.RemoveSoundableBall(current.rigidbody, current.ball._SoundConfig);
          GamePlayManager.Instance.BallSoundManager.StopSoundableBallAllSound(current.rigidbody, current.ball._SoundConfig);
          //移除球的推动恒力
          if (current.pushForceX != null) {
            current.pushForceX.Delete();
            current.pushForceX = null;
          }
          if (current.pushForceZ != null) {
            current.pushForceZ.Delete();
            current.pushForceZ = null;
          }
          if (current.pushForceY != null) {
            current.pushForceY.Delete();
            current.pushForceY = null;
          }
          //取消激活
          current.ball.Deactive();
          if (physicsed)
            current.rigidbody.UnPhysicalize(true);
        }
      }
    }
    internal void SetCanControlCameraWhenStart() {
      CanControllCamera = true;
      GamePlayManager.Instance.CamManager.SetCamFollow(true);
    }

    #endregion

    #region 球附加工具方法

    /// <summary>
    /// 播放球出生时的烟雾
    /// </summary>
    /// <param name="pos">放置位置</param>
    public void PlaySmoke(Vector3 pos)
    {
      _BallSmoke.transform.position = pos;
      _BallSmoke.SetActive(true);
      EventPlaySmoke.Emit(null);
    }
    /// <summary>
    /// 播放球出生时的闪电效果
    /// </summary>
    /// <param name="pos">放置位置</param>
    /// <param name="smallToBig">是否由小到大</param>
    /// <param name="lightAnim">是否同时播放灯光效果</param>
    /// <param name="callback">播放完成后，会调用这个完成回调</param>
    public void PlayLighting(Vector3 pos, bool smallToBig, bool lightAnim, GameManager.VoidDelegate callback) 
    {
      EventPlayLighting.Emit(null);
      _BallLightningSphere.PlayLighting(pos, smallToBig, callback, lightAnim);
    }
    /// <summary>
    /// 获取当前是否正在运行球出生闪电效果
    /// </summary>
    public bool IsLighting() { return _BallLightningSphere.IsLighting(); }
    /// <summary>
    /// 快速将球锁定并移动至目标位置
    /// </summary>
    /// <param name="pos">目标位置</param>
    /// <param name="time">时间 (秒)</param>
    /// <param name="callback">移动完成后会调用此回调</param>
    public void FastMoveTo(Vector3 pos, float time, SmoothFly.CallbackDelegate callback)
    {
      //锁定
      SetControllingStatus(BallControlStatus.LockMode);

      if (currentActiveBall != null) {  
        var ball = currentActiveBall.ball;
        SmoothFly mover = ball.gameObject.GetComponent<SmoothFly>();
        if (mover == null)
          mover = ball.gameObject.AddComponent<SmoothFly>();

        mover.TargetPos = pos;
        mover.Time = time;
        mover.ArrivalDiatance = 0.02f;
        mover.StopWhenArrival = true;
        mover.ArrivalCallback = callback;
        mover.Fly = true;
      }
    }

    #endregion

    #region 键盘事件处理

    private void _UpArrow_Key(bool down)
    {
      KeyStateForward = down;
      FlushBallPush();
    }
    private void _DownArrow_Key(bool down)
    {
      KeyStateBack = down;
      FlushBallPush();
    }
    private void _RightArrow_Key(bool down)
    {
      _RightPressed = down;
      if (down) {
        //旋转摄像机
        if (CanControllCamera && ShiftPressed) {
          if(reverseRotation)
            GamePlayManager.Instance.CamManager.RotateLeft();
          else
            GamePlayManager.Instance.CamManager.RotateRight();
          //按下shift时不许推动
          KeyStateRight = false;
        }
        else
          KeyStateRight = true;
        FlushBallPush();
      }
      else
      {
        KeyStateRight = false;
        FlushBallPush();
      }
    }
    private void _LeftArrow_Key(bool down)
    {
      _LeftPressed = down;
      if (down) {
        ///旋转摄像机
        if (CanControllCamera && ShiftPressed) 
        {
          if(reverseRotation)
            GamePlayManager.Instance.CamManager.RotateRight();
          else
            GamePlayManager.Instance.CamManager.RotateLeft();
          //按下shift时不许推动
          KeyStateLeft = false;
        }
        else
          KeyStateLeft = true;
        FlushBallPush();
      }
      else
      {
        KeyStateLeft = false;
        FlushBallPush();
      }
    }
    private void _Down_Key(bool down)
    {
      if (GameManager.DebugMode) {
        KeyStateDown = down;
        FlushBallPush();
      }
    }
    private void _Up_Key(bool down)
    {
      if (GameManager.DebugMode) {
        KeyStateUp = down;
        FlushBallPush();
      }
    }
    private void _Shift_Key(bool down)
    {
      ShiftPressed = down;
      if (_LeftPressed) 
      {
        //旋转摄像机
        if (down && CanControllCamera) {
          //禁用左推动
          KeyStateLeft = false;
          _CamRotateLeft();
        }
        else
        {
          //抬起时重新恢复左推动
          KeyStateLeft = true;
        }
        FlushBallPush();
      }
      else if (_RightPressed) 
      {
        //旋转摄像机
        if (down && CanControllCamera) 
        {
          _CamRotateRight();
          //禁用右推动
          KeyStateRight = false;
        }
        else
        {
          //抬起时重新恢复右推动
          KeyStateRight = true;
        }
        FlushBallPush();
      }
    }

    private void _CamOverlook(bool enable)
    {
      if (CanControllCamera)
        GamePlayManager.Instance.CamManager.RotateUp(enable) ;
    }
    private void _CamRotateLeft()
    {
      if(reverseRotation)
        GamePlayManager.Instance.CamManager.RotateRight();
      else
        GamePlayManager.Instance.CamManager.RotateLeft();
    }
    private void _CamRotateRight()
    {
      if(reverseRotation)
        GamePlayManager.Instance.CamManager.RotateLeft();
      else
        GamePlayManager.Instance.CamManager.RotateRight();
    }

    #endregion

    #region 推动方法

    /// <summary>
    /// 刷新球推动方向按键
    /// </summary>
    public void FlushBallPush()
    {
      if (CurrentBall == null || currentActiveBall == null || !CanControll)
        return;
      
      var currentBall = currentActiveBall;
      var force = CurrentBall._Force * BallSpeedFactor;

      //前进后退
      if (KeyStateForward && KeyStateBack)
        currentBall.pushForceZ.Force = 0;
      else if (KeyStateForward)
        currentBall.pushForceZ.Force = force;
      else if (KeyStateBack)
        currentBall.pushForceZ.Force = -force;
      else
        currentBall.pushForceZ.Force = 0;
      //左右
      if (KeyStateLeft && KeyStateRight)
        currentBall.pushForceX.Force = 0;
      else if (KeyStateLeft)
        currentBall.pushForceX.Force = -force;
      else if (KeyStateRight)
        currentBall.pushForceX.Force = force;
      else
        currentBall.pushForceX.Force = 0;
      ///上下
      if (KeyStateUp && KeyStateDown)
        currentBall.pushForceY.Force = 0;
      else if (KeyStateUp)
        currentBall.pushForceY.Force = currentBall.ball._UpForce;
      else if (KeyStateDown)
        currentBall.pushForceY.Force = -currentBall.ball._DownForce;
      else
        currentBall.pushForceY.Force = 0;

      EventFlushBallPush.Emit(null);
    }

    /// <summary>
    /// 设置球推动方向数值，此函数可以用于摇杆控制
    /// </summary>
    /// <param name="x">X轴推动力百分比（[-1,1]）</param>
    /// <param name="y">Y轴推动力百分比（[-1,1]）</param>
    public void SetBallPushValue(float x, float y)
    {
      if (CurrentBall == null || currentActiveBall == null || !CanControll)
        return;
      var currentBall = currentActiveBall;
      var force = CurrentBall._Force;
      EventSetBallPushValue.Emit(x, y);
      currentBall.pushForceX.Force = x * force;
      currentBall.pushForceZ.Force = y * force;
    }
    /// <summary>
    /// 去除当前球所有推动方向
    /// </summary>
    public void RemoveAllBallPush()
    {
      if (CurrentBall == null || currentActiveBall == null || !CanControll)
        return;
      var currentBall = currentActiveBall;
      EventRemoveAllBallPush.Emit(null);

      if (currentBall.pushForceX != null) currentBall.pushForceX.Force = 0;
      if (currentBall.pushForceY != null) currentBall.pushForceY.Force = 0;
      if (currentBall.pushForceZ != null) currentBall.pushForceZ.Force = 0;
    }

    #endregion
  }

  /// <summary>
  /// 指定球的控制状态
  /// </summary>
  public enum BallPushType { 
    /// <summary>
    /// 无推动
    /// </summary>
    None = 0,
    /// <summary>
    /// 前
    /// </summary>
    Forward = 0x2,
    /// <summary>
    /// 后
    /// </summary>
    Back = 0x4,
    /// <summary>
    /// 左
    /// </summary>
    Left = 0x8,
    /// <summary>
    /// 右
    /// </summary>
    Right = 0x10,
    /// <summary>
    /// 上升
    /// </summary>
    Up = 0x20,
    /// <summary>
    /// 下降
    /// </summary>
    Down = 0x40,
  }
  /// <summary>
  /// 指定球的控制状态
  /// </summary>
  public enum BallControlStatus 
  {
    /// <summary>
    /// 用于参数，表示维持之前的状态
    /// </summary>
    KeepOldStatus = -1,
    /// <summary>
    /// 没有控制（无控制、无物理效果，无摄像机跟随）
    /// </summary>
    NoControl = 0,
    /// <summary>
    /// 正常控制（可控制、物理效果，摄像机跟随）
    /// </summary>
    Control = 1,
    /// <summary>
    /// 释放模式（例如球坠落，球仍然有物理效果，但无法控制，摄像机不跟随，但看着球）
    /// </summary>
    UnleashingMode = 2,
    /// <summary>
    /// 锁定模式（例如变换球时，无物理效果，无法控制，但摄像机跟随）
    /// </summary>
    LockMode = 3,
    /// <summary>
    /// 释放模式2（球仍然有物理效果，但无法控制，摄像机跟随看着球）
    /// </summary>
    FreeMode = 4,
    /// <summary>
    /// 无物理效果，无法控制，但摄像机不跟随，但看着球
    /// </summary>
    LockLookMode = 5,
  }
  /// <summary>
  /// 球注册信息存储结构
  /// </summary>
  public class BallRegStorage {
    /// <summary>
    /// 球名称
    /// </summary>
    public string name = "";
    /// <summary>
    /// 编辑器预览图片
    /// </summary>
    public Sprite Preview = null;
    /// <summary>
    /// 球类实例
    /// </summary>
    public Ball ball = null;
    /// <summary>
    /// 球的刚体实例, 仅当前球激活时才有此实例
    /// </summary>
    public PhysicsObject rigidbody = null;
    /// <summary>
    /// 速度计
    /// </summary>
    public SpeedMeter speedMeter = null;
    /// <summary>
    /// 球的X轴推动力, 仅当前球激活时才有此实例
    /// </summary>
    public PhysicsConstantForceData pushForceX = null;
    /// <summary>
    /// 球的Y轴推动力, 仅当前球激活时才有此实例
    /// </summary>
    public PhysicsConstantForceData pushForceY = null;
    /// <summary>
    /// 球的Z轴推动力, 仅当前球激活时才有此实例
    /// </summary>
    public PhysicsConstantForceData pushForceZ = null;
  }
}