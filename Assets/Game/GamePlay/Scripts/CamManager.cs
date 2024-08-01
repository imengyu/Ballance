using Ballance2.Base;
using Ballance2.Menu;
using Ballance2.Services;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 摄像机管理器，负责游戏中的摄像机运动控制。
  /// </summary>
  public class CamManager : MonoBehaviour
  {
    private const string TAG = "CamManager";

    public GameObject _CamOrient;
    public Transform _CamTarget;
    public Transform _CamOrientTransform;
    public Skybox _SkyBox;
    public AnimationCurve _CamRotateSpeedCurve;
    public AnimationCurve _CamUpSpeedCurve;
    public AnimationCurve _CamDownSpeedCurve;
    public Transform _CamPosFrame;
    public CamFollow _CamFollow;

    [SerializeField]
    private float _CameraRotateTime = 0.3f;
    [SerializeField]
    private float _CameraRotateUpTime = 0.8f;
    [SerializeField]
    private float _CameraRotateDownTime = 1.3f;
    [SerializeField]
    private float _CameraNormalZ = 17;
    [SerializeField]
    private float _CameraNormalY = 30;
    [SerializeField]
    private float _CameraSpaceY = 55;
    [SerializeField]
    private float _CameraSpaceZ = 8;
    [SerializeField]
    private float _CamRotateValueNow = 0;
    [SerializeField]
    private bool _CamIsRotateing = false;
    [SerializeField]
    private float _CamRotateTick = 0;
    [SerializeField]
    private bool _CamIsRotateingUp = false;
    [SerializeField]
    private float _CamRotateUpTick = 0;
    [SerializeField]
    private Vector3 _CamRotateUpStart = Vector3.zero;
    [SerializeField]
    private Vector3 _CamRotateUpTarget = Vector3.zero;
    [SerializeField]
    private Vector3 _CamOutSpeed = Vector3.zero;
    [SerializeField]
    private float _CamRotateStartDegree = 0;
    [SerializeField]
    private float _CamRotateTargetDegree = 0;

    #region 公共属性

    /// <summary>
    /// 获取球参照的摄像机旋转方向变换 [R]
    /// </summary>
    public Transform CamDirectionRef { get; private set; }
    /// <summary>
    /// 获取摄像机右侧向量 [R]
    /// </summary>
    public Vector3 CamRightVector { get; private set; } = Vector3.right;
    /// <summary>
    ///  获取摄像机左侧向量 [R]
    /// </summary>
    public Vector3 CamLeftVector { get; private set; } = Vector3.left;
    /// <summary>
    /// 获取摄像机向前向量 [R]
    /// </summary>
    public Vector3 CamForwerdVector  { get; private set; } = Vector3.forward;
    /// <summary>
    /// 获取摄像机向后向量 [R]
    /// </summary>
    public Vector3 CamBackVector  { get; private set; } = Vector3.back;
    /// <summary>
    /// 摄像机跟随速度 [RW]
    /// </summary>
    public float CamFollowSpeed  { get; set; } = 0.05f;
    /// <summary>
    ///  获取摄像机是否空格键升高了 [R]
    /// </summary>
    public bool CamIsSpaced { get; private set; } = false;
    /// <summary>
    /// 获取当前摄像机方向 [R] 设置请使用 RotateTo 方法
    /// </summary>
    public float CamRotateValue  { get; private set; } = 0;
    /// <summary>
    /// 获取摄像机跟随脚本 [R]
    /// </summary>
    public CamFollow CamFollow  { get { return _CamFollow;  } }
        
    #endregion

    #region 初始化

    private void Start() {
      InitCommand();
      InitEvents();
          
      _CamPosFrame.localPosition = new Vector3(0, _CameraNormalY, -_CameraNormalZ);
      transform.position = _CamPosFrame.position;
      transform.LookAt(_CamTarget);
      CamDirectionRef = _CamOrient.transform;
    }
    private void OnDestroy() {
      DestroyCommand();
      DestroyEvent();
    }
    private void Update() {
      //摄像机水平旋转
      if (_CamIsRotateing) {
        _CamRotateTick += Time.deltaTime;

        var v = _CamRotateSpeedCurve.Evaluate(_CamRotateTick / _CameraRotateTime);
        
        _CamRotateValueNow = _CamRotateStartDegree + v * _CamRotateTargetDegree;
        _CamOrientTransform.localEulerAngles = new Vector3(0, _CamRotateValueNow, 0);

        if (v >= 1) {
          _CamRotateValueNow = _CamRotateStartDegree + _CamRotateTargetDegree;
          _CamOrientTransform.localEulerAngles = new Vector3(0, _CamRotateValueNow, 0);
          _CamIsRotateing = false;
          ResetVector();
        }
      }
      //摄像机垂直向上
      if (_CamIsRotateingUp) {
        _CamRotateUpTick += Time.deltaTime;
        
        float v = 0;
        if (CamIsSpaced)
          v = _CamUpSpeedCurve.Evaluate(_CamRotateUpTick / _CameraRotateUpTime);
        else
          v = _CamDownSpeedCurve.Evaluate(_CamRotateUpTick / _CameraRotateDownTime);

        _CamPosFrame.localPosition = new Vector3(0, _CamRotateUpStart.y + v * _CamRotateUpTarget.y, _CamRotateUpStart.z + v * _CamRotateUpTarget.z);
        if (v >= 1)
          _CamIsRotateingUp = false;
      }
    }

    #endregion

    #region 事件

    /// <summary>
    /// 空格键升起摄像机状态变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventRotateUpStateChanged;
    /// <summary>
    /// 摄像机旋转方向变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventRotateDirectionChanged;
    /// <summary>
    /// 摄像机是否跟踪目标变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventCamFollowChanged;
    /// <summary>
    /// 摄像机对准目标变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventCamLookChanged;
    /// <summary>
    /// 摄像机跟踪目标变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventCamFollowTargetChanged;

    private void InitEvents() {
      var events = GameMediator.Instance.RegisterEventEmitter("CamManager");
      EventRotateUpStateChanged = events.RegisterEvent("RotateUpStateChanged");
      EventRotateDirectionChanged = events.RegisterEvent("RotateDirectionChanged");
      EventCamFollowChanged = events.RegisterEvent("CamFollowChanged");
      EventCamLookChanged = events.RegisterEvent("CamLookChanged");
      EventCamFollowTargetChanged = events.RegisterEvent("CamFollowTargetChanged");
    }
    private void DestroyEvent() {
      GameMediator.Instance?.UnRegisterEventEmitter("CamManager");
    }

    #endregion

    #region 指令

    private int _CommandId = 0;
    private void InitCommand() {
      _CommandId = GameManager.Instance.GameDebugCommandServer.RegisterCommand("cam", (eyword, fullCmd, argsCount, args) => {
        var type = args[0];
        if (type == "left")
          RotateLeft();
        else if (type == "right")
          RotateLeft();
        else if (type == "up")
          RotateUp(true);
        else if (type == "down")
          RotateUp(false);
        else if (type == "follow")
          SetCamFollow(true);
        else if (type == "no-follow")
          SetCamFollow(false);
        else if( type == "look")
          SetCamLook(true);
        else if (type == "no-look")
          SetCamLook(false);
        return true;
      }, 1, "cam <left/right/up/down> 摄像机管理器命令" +
              "  left      ▶ 向左旋转摄像机" + 
              "  right     ▶ 向右旋转摄像机" + 
              "  up        ▶ 空格键升起摄像机" + 
              "  down      ▶ 空格放开落下摄像机" + 
              "  follow    ▶ 开启摄像机跟随" + 
              "  no-follow ▶ 关闭摄像机跟随" + 
              "  look      ▶ 开启摄像机看球" + 
              "  no-look   ▶ 关闭摄像机看球"
      );
    }
    private void DestroyCommand() {
      GameManager.Instance?.GameDebugCommandServer.UnRegisterCommand(_CommandId);
    }
  
    #endregion

    #region 公共方法

    /// <summary>
    /// 摄像机面对向量重置
    /// </summary>
    /// <returns></returns>
    public void ResetVector()
    {
      //根据摄像机朝向重置几个球推动的方向向量
      var y = -_CamOrientTransform.localEulerAngles.y - 90;
      CamRightVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.right;
      CamLeftVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.left;
      CamForwerdVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.forward;
      CamBackVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.back;
    }
    private void _UpdateStateForDebugStats()
    {
      if (GameManager.DebugMode && GamePlayUIControl.Instance.DebugStatValues.ContainsKey("CamDirection")) {
        GamePlayUIControl.Instance.DebugStatValues["CamDirection"].Value = _CamOrientTransform.localEulerAngles.y.ToString("F2");
        GamePlayUIControl.Instance.DebugStatValues["CamState"].Value = $"IsSpaced: {CamIsSpaced} Follow: {CamFollow.Follow} Look: {CamFollow.Look}";
      }
    }

    /// <summary>
    /// 通过RestPoint占位符设置摄像机的方向和位置
    /// </summary>
    /// <param name="go">RestPoint占位符</param>
    public void SetPosAndDirByRestPoint(GameObject go) 
    {
      var rot = go.transform.eulerAngles.y;
      rot = rot % 360;
      if (rot < 0) rot += 360;
      else if (rot > 315) rot -= 360;

      _CamOrientTransform.localEulerAngles = new Vector3(0, rot - 90, 0);
      _CamTarget.position = go.transform.position;
      transform.position = _CamPosFrame.position;
      CamRotateValue = rot - 90;
      _CamRotateValueNow = rot - 90;
      ResetVector();
      _UpdateStateForDebugStats();
    }
    /// <summary>
    /// 空格键向上旋转
    /// </summary>
    /// <param name="enable">状态</param>
    public void RotateUp(bool enable)
    {
      if (CamIsSpaced != enable)
      {
        CamIsSpaced = enable;
        _CamRotateUpStart.y = _CamPosFrame.localPosition.y;
        _CamRotateUpStart.z = _CamPosFrame.localPosition.z;
        if (enable) 
        {
          _CamRotateUpTarget.y = _CameraSpaceY - _CamRotateUpStart.y;
          _CamRotateUpTarget.z = -_CameraSpaceZ - _CamRotateUpStart.z;
        }
        else
        {
          _CamRotateUpTarget.y = _CameraNormalY - _CamRotateUpStart.y;
          _CamRotateUpTarget.z = -_CameraNormalZ - _CamRotateUpStart.z;
        }
        _CamRotateUpTick = 0;
        _CamIsRotateingUp = true;
        EventRotateUpStateChanged.Emit(enable);
        _UpdateStateForDebugStats();
      }
    }
    /// <summary>
    /// 摄像机向右旋转
    /// </summary>
    public void RotateRight()
    {
      RotateDregree(-90);
    }
    /// <summary>
    /// 摄像机向左旋转
    /// </summary>
    public void RotateLeft()
    {
      RotateDregree(90);
    }
    /// <summary>
    /// 摄像机旋转指定度数
    /// </summary>
    /// <param name="deg">度数，正数往右，负数往左</param>
    public void RotateDregree(float deg)
    {
      CamRotateValue = CamRotateValue + deg;
      _CamRotateStartDegree = _CamRotateValueNow;
      _CamRotateTargetDegree = CamRotateValue - _CamRotateStartDegree;
      _CamRotateTick = 0;
      _CamIsRotateing = true;
      EventRotateDirectionChanged.Emit(_CamRotateTargetDegree);
      _UpdateStateForDebugStats();
    }
    /// <summary>
    /// 设置主摄像机天空盒材质
    /// </summary>
    /// <param name="mat"></param>
    public void SetSkyBox(Material mat)
    {
      _SkyBox.material = mat;
    }
    /// <summary>
    /// 指定摄像机是否开启跟随球
    /// </summary>
    /// <param name="enable"></param>
    public void SetCamFollow(bool enable)
    {
      CamFollow.Follow = enable;
      EventCamFollowChanged.Emit(enable);
      _UpdateStateForDebugStats();
    }
    /// <summary>
    /// 指定摄像机是否开启看着球
    /// </summary>
    /// <param name="enable"></param>
    public void SetCamLook(bool enable)
    {
      CamFollow.Look = enable;
      EventCamLookChanged.Emit(enable);
      _UpdateStateForDebugStats();
    }
    /// <summary>
    /// 指定当前摄像机跟踪的目标
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="noUpdatePos">禁止设置目标时是否禁止位置同步</param>
    public void SetTarget(Transform target, bool noUpdatePos)
    {
      if (noUpdatePos)
        CamFollow.SetTargetWithoutUpdatePos(target);
      else
        CamFollow.Target = target;
      EventCamFollowTargetChanged.Emit(target);
    }

    /// <summary>
    /// 获取当前摄像机跟踪的目标
    /// </summary>
    /// <value></value>
    public Transform Target {
      get { return CamFollow.Target; }
    }

    /// <summary>
    /// 禁用所有摄像机功能
    /// </summary>
    public void DisbleAll()
    {
      CamFollow.Follow = false;
      CamFollow.Look = false;
      CamFollow.Target = null;
      EventCamFollowChanged.Emit(false);
      EventCamLookChanged.Emit(false);
      EventCamFollowTargetChanged.Emit(null);
      _UpdateStateForDebugStats();
    }

    #endregion
  }
}