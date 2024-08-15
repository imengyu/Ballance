using System;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicControlSnapListener : MonoBehaviour 
  {
    [HideInInspector]
    public LevelDynamicControlPoint Parent;

    public Action<LevelDynamicControlPoint> onStartSnap;
    public Action onQuitSnap;

    private LevelDynamicControlPoint currentEnterSnapPoint = null;

    private void OnTriggerEnter(Collider collider)
    {
      if (Parent.EnableSnap && !Parent.DisableSnapWhenChanged && collider.gameObject.layer == 13) //SnapPoint
      {
        var point = collider.gameObject.GetComponent<LevelDynamicControlPoint>();
        if (point != null && point.ParentId != Parent.ParentId && point.Snapable) //不能是同一个对象的吸附点
        {
          currentEnterSnapPoint = point;
          onStartSnap(point);
        }
      }
    }
    private void OnTriggerExit(Collider collider)
    {
      if (collider.gameObject.layer == 13)
      {
        var point = collider.gameObject.GetComponent<LevelDynamicControlPoint>();
        if (point != null && point == currentEnterSnapPoint)
        {
          currentEnterSnapPoint = null;
          onQuitSnap();
        }
      }
    }
  }
}