using Ballance2.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Progress.cs
* 
* 用途：
* 一个进度条组件组件。
*
* 作者：
* mengyu
*/
namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// 进度条组件
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu("Ballance/UI/Controls/Progress")]
  public class Progress : MonoBehaviour
  {
    [SerializeField, HideInInspector]
    private float val = 0;
    public bool lightByVal = false;

    public RectTransform fillArea;
    public RectTransform fillRect;
    public Image fillImage;

    public void UpdateVal()
    {
      UIAnchorPosUtils.SetUIRightTop(fillRect, ((1.0f - val) * fillArea.rect.size.x), 0);
      if (lightByVal)
        fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, val);
    }

    /// <summary>
    /// 进度条数值（0-1）
    /// </summary>
    /// <value></value>
    public float value
    {
      get { return val; }
      set
      {
        val = value;
        UpdateVal();
      }
    }
  }
}
