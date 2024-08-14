using System;
using System.Collections.Generic;
using Ballance2.Utils;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;
using static Ballance2.Game.LevelEditor.LevelDynamicFloorBlockMaker;
using static Ballance2.Game.LevelEditor.LevelDynamicControlPoint;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicFloorBlockEditor : MonoBehaviour 
  {
    public LevelDynamicFloorBlockComponent Floor;
    public LevelDynamicControlPoint ControlPoint1;
    public LevelDynamicControlPoint ControlPoint2;
    public LevelDynamicControlPoint ControlPoint3;
    public LevelDynamicControlPoint ControlPoint4;
    public LevelDynamicStraitRuler Ruler1;
    public LevelDynamicStraitRuler Ruler2;

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

      if (!enableEdit)
        return;
      
      switch (Floor.Type)
      {
        case LevelDynamicComponentType.Strait:
          ControlPoint2.DragMinValue = new Vector3(1, 0, 0);
          ControlPoint2.DragType = LevelDynamicControlPointDragType.Z;
          ControlPoint2.transform.localPosition = new Vector3(0, 0, ControlPoint2.transform.position.z);
          ControlPoint3.gameObject.SetActive(false);
          ControlPoint4.gameObject.SetActive(false);
          break;
        case LevelDynamicComponentType.Arc:
          ControlPoint2.DragType = LevelDynamicControlPointDragType.XZ;
          ControlPoint3.DragType = LevelDynamicControlPointDragType.X;
          ControlPoint2.DragMinValue = new Vector3(DragMinValueNoLimit.x, 0, 0);
          ControlPoint3.DragMinValue = new Vector3(1, 0, 0);
          ControlPoint2.transform.localPosition = new Vector3(ControlPoint2.transform.localPosition.x < 1 ? 10 : ControlPoint2.transform.localPosition.x, 0, ControlPoint2.transform.localPosition.z);
          ControlPoint3.transform.localPosition = new Vector3(ControlPoint3.transform.localPosition.x < 1 ? 10 : ControlPoint3.transform.localPosition.x, 0, 0);
          ControlPoint4.gameObject.SetActive(false);
          break;
        case LevelDynamicComponentType.Bizer:
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
      
      UpdateRuler();
    }

    private void Awake() 
    {
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