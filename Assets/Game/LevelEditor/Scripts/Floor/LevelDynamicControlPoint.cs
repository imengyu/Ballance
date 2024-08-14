using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicControlPoint : MonoBehaviour 
  {
    public static Vector3 DragMinValueNoLimit = new Vector3(-10000, -10000, -10000);
    public static Vector3 DragMaxValueNoLimit = new Vector3(10000, 10000, 10000);

    public Material ActiveMateral;
    public Material NormalMateral;
    public MeshRenderer meshRenderer;
    public GameObject Inner;
    [SerializeField]
    private LevelDynamicControlPointDragType dragType = LevelDynamicControlPointDragType.None;
    private bool isHovered = false;
    private LockAxes lockAxes;
    private Vector3 pos;
    private int tick = 0;

    private void Awake() {
      if (meshRenderer == null)
        meshRenderer = GetComponent<MeshRenderer>();
      lockAxes = GetComponent<LockAxes>();
      pos = transform.localPosition;
    }
    private void Update() {
      if (tick < 5)
      tick++;
      else
      {
        tick = 0;
        if (pos != transform.localPosition)
        {
          pos = transform.localPosition;
          onMoved?.Invoke();
        }
      }
    }

    public bool IsConnected {
      get {
        return false;
      }
    }
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

    public void UpdateDragValues()
    {
      switch (dragType)
      {
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