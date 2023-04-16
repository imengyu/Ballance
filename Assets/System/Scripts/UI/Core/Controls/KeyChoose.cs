using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Progress.cs
* 
* 用途：
* 一个键盘按键选择组件。
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// 一个键盘按键选择组件
  /// </summary>
  [AddComponentMenu("Ballance/UI/Controls/KeyChoose")]
  public class KeyChoose : MonoBehaviour
  {
    public I18NText Text;
    public Text TextValue;

    [SerializeField, SetProperty("value")]
    private KeyCode _value = KeyCode.None;

    public class KeyChooseEvent : UnityEvent<KeyCode>
    {
      public KeyChooseEvent() { }
    }

    public KeyChooseEvent onValueChanged = new KeyChooseEvent();

    /// <summary>
    /// 获取或设置按钮选中的键
    /// </summary>
    public KeyCode value
    {
      get { return _value; }
      set
      {
        _value = value;
        UpdateValue();
        if (onValueChanged != null)
          onValueChanged.Invoke(value);
      }
    }
    public void UpdateValue()
    {
      TextValue.text = _value.ToString();
    }

    private bool waitForKey = false;

    public void StartWaitKey() {
      waitForKey = true;
    }

    void Start()
    {
      UpdateValue();
    }
    void OnGUI()
    {
      if (Input.anyKeyDown && waitForKey)
      {
        Event e = Event.current;
        if (e.isKey) {
          value = e.keyCode;
          waitForKey = false;
        }
      }
    }
  }
}
