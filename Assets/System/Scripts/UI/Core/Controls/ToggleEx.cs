using Ballance2.Services.InputManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Toggle.cs
* 
* 用途：
* 一个开关组件，与原版的Chexkbox区分开来
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// 一个开关组件
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu("Ballance/UI/Controls/ToggleEx")]
  public class ToggleEx : Selectable, IPointerClickHandler
  {
    public RectTransform Drag;
    public RectTransform Background;
    public Image DragImage;
    public Sprite ActiveImage;
    public Sprite DeactiveImage;
    public Image CheckedButton;
    public Image UnCheckedButton;
    public UIText Text;

    public Toggle.ToggleEvent onValueChanged;
    public bool UseButtonStyle = false;

    protected override void Start()
    {
      UpdateOn();
    }

    [SerializeField, HideInInspector]
    private bool on = false;

    public void UpdateOn()
    {
      if (!UseButtonStyle)
      {
        DragImage.sprite = on ? ActiveImage : DeactiveImage;
        Drag.anchoredPosition = new Vector2(
            on ? 31.6f : 10.5f,
            Drag.anchoredPosition.y
        );
      }
      else
      {
        CheckedButton.sprite = on ? ActiveImage : DeactiveImage;
        UnCheckedButton.sprite = on ? DeactiveImage : ActiveImage;
      }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      isOn = !isOn;
    }

    public bool isOn
    {
      get { return on; }
      set
      {
        if (on != value)
        {
          on = value;
          if (onValueChanged != null)
            onValueChanged.Invoke(on);
          UpdateOn();
        }
      }
    }
  }
}
