using System;
using Ballance2.Utils;
using Battlehub.RTCommon;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  [DefaultExecutionOrder(10)]
  public class LevelDynamicControlPoint : MonoBehaviour 
  {
    public static Vector3 DragMinValueNoLimit = new Vector3(-10000, -10000, -10000);
    public static Vector3 DragMaxValueNoLimit = new Vector3(10000, 10000, 10000);

    public object Parent = null;
    public Func<float> GetParentSnapPortWidth = () => 0;
    [HideInInspector]
    public int ParentId = 0;
    public int SnapId = 0; //用于标记吸附点的顺序，规定S0 N1 W2 E3
    [HideInInspector]
    public GameObject SnapParent = null;
    public bool Snapable = false;
    public bool EnableSnap
    {
      get => enableSnap;
      set
      {
        enableSnap = value;
        if (snapListener != null)
          snapListener.Collider.isTrigger = !enableSnap;
      }
    }
    public Material ActiveMateral;
    public Material NormalMateral;
    public MeshRenderer meshRenderer;
    public GameObject Inner;
    public Func<Vector3, Vector3> DragValueFixer = null;

    private bool enableSnap = false;
    [SerializeField]
    private LevelDynamicControlSnapListener snapListener;
    [SerializeField]
    private LevelDynamicControlPointDragType dragType = LevelDynamicControlPointDragType.None;
    private bool isHovered = false;
    private LockAxes lockAxes;
    private Vector3 pos;
    private int tick = 0;
    private int resetTick = 0;
    private bool dirty = false;
    private LevelDynamicControlPoint currentSnapOtherPoint = null;
    private LevelDynamicControlPoint currentConnectedOtherPoint = null;

    private void Awake() {
      if (meshRenderer == null)
        meshRenderer = GetComponent<MeshRenderer>();
      lockAxes = GetComponent<LockAxes>();
      pos = transform.localPosition;
      if (snapListener != null)
      {
        snapListener.Parent = this;
        snapListener.onStartSnap = OnStartSnap;
        snapListener.onQuitSnap = OnQuitSnap;
        snapListener.onPointEnter = OnOtherSnapPointEnter;
        snapListener.onPointLeave = OnOtherSnapPointLeave;
      }
    }
    private void Update()
    {
      if (pos != transform.localPosition)
      {
        pos = new Vector3(
          CommonUtils.LimitNumber(transform.localPosition.x, DragMinValue.x, DragMaxValue.x),
          CommonUtils.LimitNumber(transform.localPosition.y, DragMinValue.y, DragMaxValue.y),
          CommonUtils.LimitNumber(transform.localPosition.z, DragMinValue.z, DragMaxValue.z)
         );
        if (DragModValue.x > 0)
          pos.x -= pos.x % DragModValue.x;
        if (DragModValue.y > 0)
          pos.y -= pos.y % DragModValue.y;
        if (DragModValue.z > 0)
          pos.z -= pos.z % DragModValue.z;
        if (DragValueFixer != null)
          pos = DragValueFixer(pos);

        transform.localPosition = pos;
        dirty = true;
      }

      if (tick < 5)
        tick++;
      else
      {
        //位置变换
        tick = 0;

        if (dirty)
        {
          dirty = false;
          onMoved?.Invoke();
        }
      }
      //吸附状态重置
      if (resetTick > 0)
      {
        resetTick--;
        if (resetTick <= 0)
          LevelDynamicControlSnap.ResetSnapCheck();
      }

      //吸附
      if (currentSnapOtherPoint != null)
      {
        if (LevelDynamicControlSnap.CheckSnap(this) && EnableSnap)
        {
          //本点的旋转永远是与另外一个点相对
          var targeRot = currentSnapOtherPoint.SnapParent.transform.eulerAngles + currentSnapOtherPoint.Inner.transform.localEulerAngles - Inner.transform.localEulerAngles;
          SnapParent.transform.eulerAngles = targeRot;
          SnapParent.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

          var targetPos = currentSnapOtherPoint.SnapParent.transform.TransformPoint(currentSnapOtherPoint.transform.localPosition) //目标吸附点的世界位置
            + (SnapParent.transform.position - SnapParent.transform.TransformPoint(transform.localPosition)); //当前点的相对原点位置
          //如果距离已经大于10，则停止
          if (Vector3.Distance(targetPos, SnapParent.transform.position) > 20)
          { 
            OnQuitSnap();
            return;
          }
          SnapParent.transform.position = targetPos;
        }
        else
        {
          OnQuitSnap();
        }
      }
    }

    public void NoNextPositionChangeEdit()
    {
      pos = transform.localPosition;
    }

    private void OnOtherSnapPointEnter(LevelDynamicControlPoint otherPoint)
    {
      if (currentConnectedOtherPoint == null)
      {
        currentConnectedOtherPoint = otherPoint;
        currentConnectedOtherPoint.onConnectedChanged?.Invoke(this);
        currentConnectedOtherPoint.currentConnectedOtherPoint = this;
        onConnectedChanged?.Invoke(currentConnectedOtherPoint);
      }
    }
    private void OnOtherSnapPointLeave(LevelDynamicControlPoint otherPoint)
    {
      if (currentConnectedOtherPoint == otherPoint)
      {
        currentConnectedOtherPoint.onConnectedChanged?.Invoke(null);
        currentConnectedOtherPoint.currentConnectedOtherPoint = null;
        currentConnectedOtherPoint = null;
        onConnectedChanged?.Invoke(null);
      }
    }
    private void OnStartSnap(LevelDynamicControlPoint otherPoint)
    {
      LevelDynamicControlSnap.Instance.ActiveSnapPoint = this;
      currentSnapOtherPoint = otherPoint;
    }
    private void OnQuitSnap()
    {
      resetTick = 30;
      currentSnapOtherPoint = null;
    }
    public LevelDynamicControlPoint ConnectedOtherPoint  { get => currentConnectedOtherPoint; }
    public bool IsConnected { get => currentConnectedOtherPoint != null; }
    public bool IsHovered {
      get => isHovered;
      set {
        isHovered = value;
        meshRenderer.material = isHovered ? ActiveMateral : NormalMateral;
      }
    }
    public LevelDynamicControlPointDragType DragType { 
      get => dragType; 
      set { 
        dragType = value; 
        UpdateDragValues();
      }
    }
    public Vector3 DragMinValue { get; set; } = DragMinValueNoLimit;
    public Vector3 DragMaxValue { get; set; } = DragMaxValueNoLimit;
    public Vector3 DragModValue { get; set; } = Vector3.zero;

    public void UpdateDragValues()
    {
      switch (dragType)
      {
        case LevelDynamicControlPointDragType.None:
          lockAxes.PositionX = true;
          lockAxes.PositionY = true;
          lockAxes.PositionZ = true;
          break;
        case LevelDynamicControlPointDragType.X:
          lockAxes.PositionX = false;
          lockAxes.PositionY = true;
          lockAxes.PositionZ = true;
          break;
        case LevelDynamicControlPointDragType.Y:
          lockAxes.PositionX = true;
          lockAxes.PositionY = false;
          lockAxes.PositionZ = true;
          break;
        case LevelDynamicControlPointDragType.Z:
          lockAxes.PositionX = true;
          lockAxes.PositionY = true;
          lockAxes.PositionZ = false;
          break;
        case LevelDynamicControlPointDragType.XZ:
          lockAxes.PositionX = false;
          lockAxes.PositionY = true;
          lockAxes.PositionZ = false;
          break;
        case LevelDynamicControlPointDragType.YZ:
          lockAxes.PositionX = true;
          lockAxes.PositionY = false;
          lockAxes.PositionZ = false;
          break;
        case LevelDynamicControlPointDragType.All:
          lockAxes.PositionX = false;
          lockAxes.PositionY = false;
          lockAxes.PositionZ = false;
          break;
      }
      
    }

    public Action onMoved;
    public Action<LevelDynamicControlPoint> onConnectedChanged;
  } 
  
  public enum LevelDynamicControlPointDragType
  {
    None,
    X,
    Z,
    Y,
    XZ,
    YZ,
    All,
  }
}