using System;
using UnityEngine;

namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// You should subclass this to provide fast access to any data you need to populate
  /// this item on demand.
  /// </summary>
  [RequireComponent(typeof(RectTransform))]
  [SLua.CustomLuaClass]
  public class RecyclingListViewItem : MonoBehaviour
  {

    private RecyclingListView parentList;
    public RecyclingListView ParentList
    {
      get => parentList;
    }

    private int currentRow;
    public int CurrentRow
    {
      get => currentRow;
    }

    private RectTransform rectTransform;
    public RectTransform RectTransform
    {
      get
      {
        if (rectTransform == null)
          rectTransform = GetComponent<RectTransform>();
        return rectTransform;
      }
    }

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
    }

    public void NotifyCurrentAssignment(RecyclingListView v, int row)
    {
      parentList = v;
      currentRow = row;
    }


  }
}
