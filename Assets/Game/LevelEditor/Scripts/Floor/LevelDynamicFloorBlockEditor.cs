using UnityEngine;
using static Ballance2.Game.LevelEditor.LevelDynamicControlPoint;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicFloorBlockEditor : MonoBehaviour 
  {
    public static int IdPool = 0;

    public LevelDynamicFloorBlockComponent Floor;
    public LevelDynamicControlPoint ControlPoint1;
    public LevelDynamicControlPoint ControlPoint2;
    public LevelDynamicControlPoint ControlPoint3;
    public LevelDynamicControlPoint ControlPoint4;
    public LevelDynamicStraitRuler Ruler1;
    public LevelDynamicStraitRuler Ruler2;

    public void UpdateSnapEnable(bool snapable)
    {
      ControlPoint1.EnableSnap = snapable;
      ControlPoint2.EnableSnap = snapable;
      ControlPoint3.EnableSnap = snapable;
      ControlPoint4.EnableSnap = snapable;
    }
    public void UpdateRuler()
    {
      switch(Floor.Type)
      {
        case LevelDynamicComponentType.Strait: {
          Ruler1.gameObject.SetActive(true);
          Ruler1.SetText($"{Floor.straitLength} m");
          Ruler1.FitInTowPoint(ControlPoint2.transform.position, ControlPoint1.transform.position);
          Ruler2.gameObject.SetActive(false);
          ControlPoint2.Inner.transform.localEulerAngles = Vector3.zero;
          break;
        }
        case LevelDynamicComponentType.Arc: {
          var radius = ControlPoint3.transform.localPosition.x - ControlPoint1.transform.localPosition.x;
          Ruler1.gameObject.SetActive(true);
          Ruler1.SetText($"Radius: {radius.ToString("F2")} m");
          Ruler1.FitInTowPoint(ControlPoint3.transform.position, ControlPoint1.transform.position);
          Ruler2.gameObject.SetActive(true);
          Ruler2.SetText($"Angle: {Floor.arcDeg.ToString("F2")} deg");
          Ruler2.FitInTowPoint(ControlPoint2.transform.position, ControlPoint3.transform.position);
          ControlPoint2.Inner.transform.localEulerAngles = new Vector3(0, Floor.arcDeg, 0);
          break;
        }
        case LevelDynamicComponentType.Bizer:
          Ruler1.gameObject.SetActive(true);
          Ruler1.SetText("");
          Ruler1.FitInTowPoint(ControlPoint3.transform.position, ControlPoint1.transform.position);
          Ruler2.gameObject.SetActive(true);
          Ruler2.SetText("");
          Ruler2.FitInTowPoint(ControlPoint2.transform.position, ControlPoint4.transform.position);
          ControlPoint2.Inner.transform.localEulerAngles = new Vector3(0, Floor.bizerEndDeg, 0);
          break;
      }
    }
    public void UpdateControllers()
    { 
      var enableEdit = Floor.EnableEdit;
      ControlPoint1.gameObject.SetActive(enableEdit);
      ControlPoint2.gameObject.SetActive(enableEdit);
      ControlPoint3.gameObject.SetActive(enableEdit);
      ControlPoint4.gameObject.SetActive(enableEdit);
      Ruler1.gameObject.SetActive(enableEdit);
      Ruler2.gameObject.SetActive(enableEdit);

      ControlPoint1.SnapParent = Floor.transform.parent.gameObject;
      ControlPoint2.SnapParent = Floor.transform.parent.gameObject;
      ControlPoint3.SnapParent = Floor.transform.parent.gameObject;
      ControlPoint4.SnapParent = Floor.transform.parent.gameObject;

      if (!enableEdit)
        return;

      ControlPoint1.DragType = LevelDynamicControlPointDragType.None;
      switch (Floor.Type)
      {
        case LevelDynamicComponentType.Strait:
          ControlPoint2.DragMinValue = new Vector3(0, 0, 1);
          ControlPoint2.DragModValue = new Vector3(0, 0, Floor.CompSize);
          ControlPoint2.DragType = LevelDynamicControlPointDragType.Z;
          ControlPoint2.transform.localPosition = new Vector3(0, 0, ControlPoint2.transform.localPosition.z);
          ControlPoint3.gameObject.SetActive(false);
          ControlPoint4.gameObject.SetActive(false);
          break;
        case LevelDynamicComponentType.Arc:
          ControlPoint2.DragModValue = Vector3.zero;
          switch (Floor.ArcDirection)
          {
            case LevelDynamicComponentArcType.X:
              ControlPoint2.DragType = LevelDynamicControlPointDragType.XZ;
              ControlPoint3.DragType = LevelDynamicControlPointDragType.X;
              ControlPoint2.DragMinValue = new Vector3(DragMinValueNoLimit.x, 0, 0);
              ControlPoint3.DragMinValue = new Vector3(1, 0, 0);
              ControlPoint2.transform.localPosition = new Vector3((ControlPoint2.transform.localPosition.x < 1 ? 10 : ControlPoint2.transform.localPosition.x), 0, ControlPoint2.transform.localPosition.z);
              ControlPoint3.transform.localPosition = new Vector3((ControlPoint3.transform.localPosition.x < 1 ? 10 : ControlPoint3.transform.localPosition.x), 0, 0);
              break;
            case LevelDynamicComponentArcType.Y:
              ControlPoint2.DragType = LevelDynamicControlPointDragType.YZ;
              ControlPoint3.DragType = LevelDynamicControlPointDragType.Y;
              ControlPoint2.DragMinValue = new Vector3(0, DragMinValueNoLimit.y, 0);
              ControlPoint3.DragMinValue = new Vector3(0, 1, 0);
              ControlPoint2.transform.localPosition = new Vector3(0, (ControlPoint2.transform.localPosition.y < 1 ? 10 : ControlPoint2.transform.localPosition.y), ControlPoint2.transform.localPosition.z);
              ControlPoint3.transform.localPosition = new Vector3(0, (ControlPoint3.transform.localPosition.y < 1 ? 10 : ControlPoint3.transform.localPosition.y), 0);
              break;
          }
          ControlPoint4.gameObject.SetActive(false);
          break;
        case LevelDynamicComponentType.Bizer:
          ControlPoint2.DragModValue = Vector3.zero;
          ControlPoint2.DragType = LevelDynamicControlPointDragType.All;
          ControlPoint3.DragType = LevelDynamicControlPointDragType.All;
          ControlPoint4.DragType = LevelDynamicControlPointDragType.All;
          ControlPoint3.transform.localPosition = new Vector3(10, 0, 0);
          ControlPoint4.transform.localPosition = new Vector3(-10, 0, 0);
          ControlPoint2.DragMinValue = DragMinValueNoLimit;
          ControlPoint3.DragMinValue = new Vector3(DragMinValueNoLimit.x, DragMinValueNoLimit.y, 1);
          ControlPoint4.DragMaxValue = new Vector3(DragMaxValueNoLimit.x, DragMaxValueNoLimit.y, 1);
          break;
      }

      ControlPoint1.UpdateDragValues();
      ControlPoint2.UpdateDragValues();
      ControlPoint3.UpdateDragValues();
      ControlPoint4.UpdateDragValues();
      UpdateRuler();
    }
    public void ApplyValueToControllers()
    {
      ControlPoint1.transform.localPosition = Floor.ControlPoint1;
      ControlPoint2.transform.localPosition = Floor.ControlPoint2;
      ControlPoint3.transform.localPosition = Floor.ControlPoint3;
      ControlPoint4.transform.localPosition = Floor.ControlPoint4;
    }

    private void Awake() 
    {
      var id = ++IdPool;
      ControlPoint1.ParentId = id;
      ControlPoint2.ParentId = id;
      ControlPoint3.ParentId = id;
      ControlPoint4.ParentId = id;
      ControlPoint4.onMoved = () => {
        if (Floor.EnableEdit)
        {
          Floor.ControlPoint4 = ControlPoint4.transform.localPosition;
          Floor.UpdateShape();
        }
      };
      ControlPoint3.onMoved = () => {
        if (Floor.EnableEdit)
        {
          Floor.ControlPoint3 = ControlPoint3.transform.localPosition;
          Floor.UpdateShape();
        }
      };
      ControlPoint2.onMoved = () => {
        if (Floor.EnableEdit)
        {
          Floor.ControlPoint2 = ControlPoint2.transform.localPosition;
          Floor.UpdateShape();
        }
      };
      ControlPoint1.onMoved = () => {
        if (Floor.EnableEdit)
        {
          Floor.ControlPoint1 = ControlPoint1.transform.localPosition;
          Floor.UpdateShape();
        }
      };
    }
  }
}