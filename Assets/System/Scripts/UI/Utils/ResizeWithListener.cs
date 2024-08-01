using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.UI.Utils
{
  [AddComponentMenu("Ballance/UI/Tools/ResizeWithListener")]
  public class ResizeWithListener : UIBehaviour
  {
    public event Action<RectTransform> RectTransformChangeAction;
    
    private bool locking = false;

    protected override void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
      if (locking)
        return;
      locking = true;
      RectTransformChangeAction?.Invoke(transform as RectTransform);
      locking = false;
    }
  }
}
