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
          
      this._CamPosFrame.localPosition = new Vector3(0, this._CameraNormalY, -this._CameraNormalZ);
      this.transform.position = this._CamPosFrame.position;
      this.transform.LookAt(this._CamTarget);
      this.CamDirectionRef = this._CamOrient.transform;
    }
    private void OnDestroy() {
      DestroyCommand();
      DestroyEvent();
    }
    private void Update() {
      //摄像机水平旋转
      if (this._CamIsRotateing) {
        this._CamRotateTick += Time.deltaTime;

        var v = this._CamRotateSpeedCurve.Evaluate(this._CamRotateTick / this._CameraRotateTime);
        
        this._CamRotateValueNow = this._CamRotateStartDegree + v * this._CamRotateTargetDegree;
        this._CamOrientTransform.localEulerAngles = new Vector3(0, this._CamRotateValueNow, 0);

        if (v >= 1) {
          this._CamRotateValueNow = this._CamRotateStartDegree + this._CamRotateTargetDegree;
          this._CamOrientTransform.localEulerAngles = new Vector3(0, this._CamRotateValueNow, 0);
          this._CamIsRotateing = false;
          this.ResetVector();
        }
      }
      //摄像机垂直向上
      if (this._CamIsRotateingUp) {
        this._CamRotateUpTick += Time.deltaTime;
        
        float v = 0;
        if (this.CamIsSpaced)
          v = this._CamUpSpeedCurve.Evaluate(this._CamRotateUpTick / this._CameraRotateUpTime);
        else
          v = this._CamDownSpeedCurve.Evaluate(this._CamRotateUpTick / this._CameraRotateDownTime);

        this._CamPosFrame.localPosition = new Vector3(0, this._CamRotateUpStart.y + v * this._CamRotateUpTarget.y, this._CamRotateUpStart.z + v * this._CamRotateUpTarget.z);
        if (v >= 1)
          this._CamIsRotateingUp = false;
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
      this.EventRotateUpStateChanged = events.RegisterEvent("RotateUpStateChanged");
      this.EventRotateDirectionChanged = events.RegisterEvent("RotateDirectionChanged");
      this.EventCamFollowChanged = events.RegisterEvent("CamFollowChanged");
      this.EventCamLookChanged = events.RegisterEvent("CamLookChanged");
      this.EventCamFollowTargetChanged = events.RegisterEvent("CamFollowTargetChanged");
    }
    private void DestroyEvent() {
      GameMediator.Instance.UnRegisterEventEmitter("CamManager");
    }

    #endregion

    #region 指令

    private int _CommandId = 0;
    private void InitCommand() {
      this._CommandId = GameManager.Instance.GameDebugCommandServer.RegisterCommand("cam", (eyword, fullCmd, argsCount, args) => {
        var type = args[0];
        if (type == "left")
          this.RotateLeft();
        else if (type == "right")
          this.RotateLeft();
        else if (type == "up")
          this.RotateUp(true);
        else if (type == "down")
          this.RotateUp(false);
        else if (type == "follow")
          this.SetCamFollow(true);
        else if (type == "no-follow")
          this.SetCamFollow(false);
        else if( type == "look")
          this.SetCamLook(true);
        else if (type == "no-look")
          this.SetCamLook(false);
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
      GameManager.Instance.GameDebugCommandServer.UnRegisterCommand(this._CommandId);
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
      var y = -this._CamOrientTransform.localEulerAngles.y - 90;
      this.CamRightVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.right;
      this.CamLeftVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.left;
      this.CamForwerdVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.forward;
      this.CamBackVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.back;
    }
    private void _UpdateStateForDebugStats()
    {
      if (GameManager.DebugMode && GamePlayUIControl.Instance.DebugStatValues.ContainsKey("CamDirection")) {
        GamePlayUIControl.Instance.DebugStatValues["CamDirection"].Value = this._CamOrientTransform.localEulerAngles.y.ToString("F2");
        GamePlayUIControl.Instance.DebugStatValues["CamState"].Value = $"IsSpaced: {this.CamIsSpaced} Follow: {this.CamFollow.Follow} Look: {this.CamFollow.Look}";
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

      this._CamOrientTransform.localEulerAngles = new Vector3(0, rot - 90, 0);
      this._CamTarget.position = go.transform.position;
      this.transform.position = this._CamPosFrame.position;
      this.CamRotateValue = rot - 90;
      this._CamRotateValueNow = rot - 90;
      this.ResetVector();
      this._UpdateStateForDebugStats();
    }
    /// <summary>
    /// 空格键向上旋转
    /// </summary>
    /// <param name="enable">状态</param>
    public void RotateUp(bool enable)
    {
      this.CamIsSpaced = enable;
      this._CamRotateUpStart.y = this._CamPosFrame.localPosition.y;
      this._CamRotateUpStart.z = this._CamPosFrame.localPosition.z;
      if (enable) 
      {
        this._CamRotateUpTarget.y = this._CameraSpaceY - this._CamRotateUpStart.y;
        this._CamRotateUpTarget.z = -this._CameraSpaceZ - this._CamRotateUpStart.z;
      }
      else
      {
        this._CamRotateUpTarget.y = this._CameraNormalY - this._CamRotateUpStart.y;
        this._CamRotateUpTarget.z = -this._CameraNormalZ - this._CamRotateUpStart.z;
      }
      this._CamRotateUpTick = 0;
      this._CamIsRotateingUp = true;
      this.EventRotateUpStateChanged.Emit(enable);
      this._UpdateStateForDebugStats();
    }
    /// <summary>
    /// 摄像机向右旋转
    /// </summary>
    public void RotateRight()
    {
      this.RotateDregree(-90);
    }
    /// <summary>
    /// 摄像机向左旋转
    /// </summary>
    public void RotateLeft()
    {
      this.RotateDregree(90);
    }
    /// <summary>
    /// 摄像机旋转指定度数
    /// </summary>
    /// <param name="deg">度数，正数往右，负数往左</param>
    public void RotateDregree(float deg)
    {
      this.CamRotateValue = this.CamRotateValue + deg;
      this._CamRotateStartDegree = this._CamRotateValueNow;
      this._CamRotateTargetDegree = this.CamRotateValue - this._CamRotateStartDegree;
      this._CamRotateTick = 0;
      this._CamIsRotateing = true;
      this.EventRotateDirectionChanged.Emit(this._CamRotateTargetDegree);
      this._UpdateStateForDebugStats();
    }
    /// <summary>
    /// 设置主摄像机天空盒材质
    /// </summary>
    /// <param name="mat"></param>
    public void SetSkyBox(Material mat)
    {
      this._SkyBox.material = mat;
    }
    /// <summary>
    /// 指定摄像机是否开启跟随球
    /// </summary>
    /// <param name="enable"></param>
    public void SetCamFollow(bool enable)
    {
      this.CamFollow.Follow = enable;
      this.EventCamFollowChanged.Emit(enable);
      this._UpdateStateForDebugStats();
    }
    /// <summary>
    /// 指定摄像机是否开启看着球
    /// </summary>
    /// <param name="enable"></param>
    public void SetCamLook(bool enable)
    {
      this.CamFollow.Look = enable;
      this.EventCamLookChanged.Emit(enable);
      this._UpdateStateForDebugStats();
    }
    /// <summary>
    /// 指定当前摄像机跟踪的目标
    /// </summary>
    /// <param name="target">目标</param>
    /// <param name="noUpdatePos">禁止设置目标时是否禁止位置同步</param>
    public void SetTarget(Transform target, bool noUpdatePos)
    {
      if (noUpdatePos)
        this.CamFollow.SetTargetWithoutUpdatePos(target);
      else
        this.CamFollow.Target = target;
      this.EventCamFollowTargetChanged.Emit(target);
    }

    /// <summary>
    /// 获取当前摄像机跟踪的目标
    /// </summary>
    /// <value></value>
    public Transform Target {
      get { return this.CamFollow.Target; }
    }

    /// <summary>
    /// 禁用所有摄像机功能
    /// </summary>
    public void DisbleAll()
    {
      this.CamFollow.Follow = false;
      this.CamFollow.Look = false;
      this.CamFollow.Target = null;
      this.EventCamFollowChanged.Emit(false);
      this.EventCamLookChanged.Emit(false);
      this.EventCamFollowTargetChanged.Emit(null);
      this._UpdateStateForDebugStats();
    }

    #endregion
  }
}