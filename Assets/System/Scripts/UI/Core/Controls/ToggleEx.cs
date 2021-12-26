using Ballance2.Services.InputManager;
using UnityEngine;
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
  [JSExport]
  [AddComponentMenu("Ballance/UI/Controls/ToggleEx")]
  public class ToggleEx : MonoBehaviour
  {
    public RectTransform Drag;
    public RectTransform Background;
    public Image DragImage;
    public Sprite ActiveImage;
    public Sprite DeactiveImage;
    public Image CheckedButton;
    public Image UnCheckedButton;

    public Toggle.ToggleEvent onValueChanged;
    public bool UseButtonStyle = false;

    void Start()
    {
      if (Background != null)
        EventTriggerListener.Get(Background.gameObject).onClick = (go) => { isOn = !isOn; };
      if (CheckedButton != null)
      {
        var btn = UnCheckedButton.gameObject.GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(() => isOn = false);
        btn = CheckedButton.gameObject.GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(() => isOn = true);
      }
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

    public bool isOn
    {
      get { return on; }
      set
      {
        if (on != value)
        {
          on = value;
          if (!Application.isEditor && onValueChanged != null)
            onValueChanged.Invoke(on);
          UpdateOn();
        }
      }
    }
  }
}
