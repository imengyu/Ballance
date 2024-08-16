using System;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicControlSnapListener : MonoBehaviour 
  {
    [HideInInspector]
    public LevelDynamicControlPoint Parent;
    [HideInInspector]
    public Collider Collider;

    public Action<LevelDynamicControlPoint> onStartSnap;
    public Action onQuitSnap;

    private void Awake()
    {
      Collider = GetComponent<Collider>();
    }

    private LevelDynamicControlPoint currentEnterSnapPoint = null;

    private void OnTriggerEnter(Collider collider)
    {
      if (Parent.EnableSnap && !Parent.DisableSnapWhenChanged && collider.gameObject.layer == 13) //SnapPoint
      {
        var point = collider.gameObject.GetComponent<LevelDynamicControlSnapListener>();
        if (point != null && point.Parent.ParentId != Parent.ParentId && point.Parent.Snapable) //不能是同一个对象的吸附点
        {
          currentEnterSnapPoint = point.Parent;
          onStartSnap(point.Parent);
        }
      }
    }
    private void OnTriggerExit(Collider collider)
    {
      if (collider.gameObject.layer == 13)
      {
        var point = collider.gameObject.GetComponent<LevelDynamicControlSnapListener>();
        if (point != null && point.Parent == currentEnterSnapPoint)
        {
          currentEnterSnapPoint = null;
          onQuitSnap();
        }
      }
    }
  }
}