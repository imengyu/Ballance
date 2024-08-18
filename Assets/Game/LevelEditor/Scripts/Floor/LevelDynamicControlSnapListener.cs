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
    public Action<LevelDynamicControlPoint> onPointEnter;
    public Action<LevelDynamicControlPoint> onPointLeave;

    private void Awake()
    {
      Collider = GetComponent<Collider>();
    }

    private LevelDynamicControlPoint currentEnterSnapPoint = null;

    private void OnTriggerEnter(Collider collider)
    {
      if (collider.gameObject.layer == 13)//SnapPoint
      {
        var point = collider.gameObject.GetComponent<LevelDynamicControlSnapListener>();
        if (point != null && point.Parent.ParentId != Parent.ParentId && point.Parent.Snapable)//不能是同一个对象的吸附点
        {
          if (LevelDynamicControlSnap.CheckSnapListener() && Parent.EnableSnap)
          {
            currentEnterSnapPoint = point.Parent;
            onStartSnap(point.Parent);
          }
          onPointEnter(point.Parent);
        }
      }
    }
    private void OnTriggerExit(Collider collider)
    {
      if (collider.gameObject.layer == 13)
      {
        var point = collider.gameObject.GetComponent<LevelDynamicControlSnapListener>();
        if (point != null && point.Parent.ParentId != Parent.ParentId)
        {
          onPointLeave(point.Parent);
          if (point.Parent == currentEnterSnapPoint)
          {
            currentEnterSnapPoint = null;
            onQuitSnap();
          }
        }
      }
    }
  }
}