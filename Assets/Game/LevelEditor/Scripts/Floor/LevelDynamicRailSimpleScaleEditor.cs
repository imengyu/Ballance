using UnityEngine;
using static Ballance2.Game.LevelEditor.LevelDynamicControlPoint;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicRailSimpleScaleEditor : MonoBehaviour 
  {
    [HideInInspector]
    public LevelDynamicRailSimpleScaleComponent Rail;
    public LevelDynamicControlPoint ControlPoint1;
    public LevelDynamicControlPoint ControlPoint2;
    public LevelDynamicControlPoint ControlPoint3;

    public void UpdateControllers()
    { 
      var enableEdit = Rail.EnableEdit;
      ControlPoint1.gameObject.SetActive(enableEdit && Rail.ScaleXEnable);
      ControlPoint2.gameObject.SetActive(enableEdit && Rail.ScaleYEnable);
      ControlPoint3.gameObject.SetActive(enableEdit && Rail.ScaleZEnable);

      if (!enableEdit)
        return;

      ControlPoint1.DragMinValue = new Vector3(1, 0, 0);
      ControlPoint1.DragType = LevelDynamicControlPointDragType.X;
      ControlPoint1.transform.localPosition = new Vector3(ControlPoint1.transform.localPosition.x, 0, 0);
      ControlPoint2.DragMinValue = new Vector3(0, 1, 0);
      ControlPoint2.DragType = LevelDynamicControlPointDragType.Y;
      ControlPoint2.transform.localPosition = new Vector3(0, ControlPoint2.transform.localPosition.y, 0);
      ControlPoint3.DragMinValue = new Vector3(0, 0, 1);
      ControlPoint3.DragType = LevelDynamicControlPointDragType.Z;
      ControlPoint3.transform.localPosition = new Vector3(0, 0, ControlPoint3.transform.localPosition.z);

      ControlPoint1.Parent = Rail;
      ControlPoint2.Parent = Rail;
      ControlPoint3.Parent = Rail;

      ControlPoint1.UpdateDragValues();
      ControlPoint2.UpdateDragValues();
      ControlPoint3.UpdateDragValues();
    }
    public void ApplyValueToControllers()
    {
      ControlPoint1.transform.localPosition = Rail.ControlPoint1;
      ControlPoint2.transform.localPosition = Rail.ControlPoint2;
      ControlPoint3.transform.localPosition = Rail.ControlPoint3;
      ControlPoint1.NoNextPositionChangeEdit();
      ControlPoint2.NoNextPositionChangeEdit();
      ControlPoint3.NoNextPositionChangeEdit();
    }

    private void Awake() 
    {
      var id = ++LevelDynamicControlSnap.IdPool;
      ControlPoint1.ParentId = id;
      ControlPoint2.ParentId = id;
      ControlPoint3.ParentId = id;
      ControlPoint3.onMoved = () => {
        if (Rail.EnableEdit)
        {
          Rail.ControlPoint3 = ControlPoint3.transform.localPosition;
          Rail.UpdateShape();
        }
      };
      ControlPoint2.onMoved = () => {
        if (Rail.EnableEdit)
        {
          Rail.ControlPoint2 = ControlPoint2.transform.localPosition;
          Rail.UpdateShape();
        }
      };
      ControlPoint1.onMoved = () => {
        if (Rail.EnableEdit)
        {
          Rail.ControlPoint1 = ControlPoint1.transform.localPosition;
          Rail.UpdateShape();
        }
      };
    }
  }
}