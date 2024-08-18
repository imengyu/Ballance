using Ballance2.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicFloorStaticEditor : MonoBehaviour 
  {
    [HideInInspector]
    public LevelDynamicFloorStaticComponent Floor;
    public LevelDynamicControlPoint SnapPointpPrefab;
    public List<LevelDynamicControlPoint> SnapPoints = new List<LevelDynamicControlPoint>();

    public void UpdateSnapEnable(bool snapable)
    {
      foreach (var item in SnapPoints)
        item.EnableSnap = snapable;
    }
    public void InitControllers()
    {
      foreach (var item in SnapPoints)
        Destroy(item);
      SnapPoints.Clear();

      var id = ++LevelDynamicControlSnap.IdPool;
      var i = 0;

      foreach (var item in Floor.SnapPoints)
      {
        if (item != null)
        {
          item.gameObject.SetActive(false);
          var newPoint = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelDynamicControlPoint>(SnapPointpPrefab.gameObject, transform, $"SnapPoint{i}");
          newPoint.transform.localPosition = item.localPosition;
          newPoint.Inner.transform.localEulerAngles = item.localEulerAngles;
          newPoint.SnapId = i;
          newPoint.ParentId = id;
          newPoint.Parent = Floor;
          newPoint.GetParentSnapPortWidth = i % 2 == 0 ? () => Mathf.Floor(Floor.Width / Floor.CompSize) : () => Mathf.Floor(Floor.Length / Floor.CompSize);
          SnapPoints.Add(newPoint);
        }
        i++;
      }

    }
    public void UpdateControllers()
    { 
      var enableEdit = Floor.EnableEdit;
      foreach (var item in SnapPoints)
      {
        item.gameObject.SetActive(enableEdit);
        item.SnapParent = Floor.transform.parent.gameObject;
      }
    }
  }
  [Serializable]
  public class LevelDynamicFloorSnapPointDefine
  {
    public Vector3 Pos;
    public Transform Ref;
    public LevelDynamicFloorSnapPointType Type;
  }
  public enum LevelDynamicFloorSnapPointType
  {
    N,
    S,
    W,
    E,
  }
}