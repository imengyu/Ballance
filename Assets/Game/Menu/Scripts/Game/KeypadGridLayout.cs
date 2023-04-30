using System;
using System.Collections.Generic;
using Ballance2.Services;
using Ballance2.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  [Serializable]
  public class KeypadGridLayoutItem {
    public Image Item;
    public Vector2 Position;
  }
  public enum KeypadGridLayoutAnchor {
    Left,
    Right,
  }
  public class KeypadGridLayout : MonoBehaviour {
    [SerializeField]
    public List<KeypadGridLayoutItem> Items = new List<KeypadGridLayoutItem>();
    [SerializeField]
    public KeypadGridLayoutAnchor Anchor = KeypadGridLayoutAnchor.Left;

    public void DoLayout(float size) {
      float maxX = 0;
      float maxY = 0;

      foreach (var item in Items)
      {
        var rectTransform = item.Item.rectTransform;
        switch(Anchor) {
          case KeypadGridLayoutAnchor.Left:
            UIAnchorPosUtils.SetUIPivot(rectTransform, UIPivot.TopLeft);
            UIAnchorPosUtils.SetUIAnchor(rectTransform, UIAnchor.Left, UIAnchor.Top);
            break;
          case KeypadGridLayoutAnchor.Right:
            UIAnchorPosUtils.SetUIPivot(rectTransform, UIPivot.TopRight);
            UIAnchorPosUtils.SetUIAnchor(rectTransform, UIAnchor.Right, UIAnchor.Top);
            break;
        }
        
        rectTransform.anchoredPosition = new Vector2(item.Position.x * size, -(item.Position.y * size));
        rectTransform.sizeDelta = new Vector2(size, size);

        if (item.Position.x > maxX)
          maxX = item.Position.x;
        if (item.Position.y > maxY)
          maxY = item.Position.y;
      }


      (transform as RectTransform).sizeDelta = new Vector2((maxX + 1) * size, (maxY + 1) * size);
    }
  }
}