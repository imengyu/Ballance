using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.UI.Utils
{
  [DisallowMultipleComponent]
  [AddComponentMenu("Ballance/UI/Tools/ResizeWithChild")]
  public class ResizeWithChild : UIBehaviour
  {
    public ResizeWithListener ResizeRef;
    public Rect Padding;
    public bool ResizeWidth = true;
    public bool ResizeHeight = true;

    protected override void Awake() 
    {
      ResizeRef.RectTransformChangeAction += (childTransform) => {
        var t = transform as RectTransform;
        t.sizeDelta = new Vector2(
          ResizeWidth ? Padding.xMin + childTransform.sizeDelta.x + Padding.xMax : t.sizeDelta.x ,
          ResizeHeight ? Padding.yMin + childTransform.sizeDelta.y + Padding.yMax : t.sizeDelta.y
        );
      };
    }
  }
}
