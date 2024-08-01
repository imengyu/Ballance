using System;
using System.Collections.Generic;
using Ballance2.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* KeyChoose.cs
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
    public UIText Text;
    public UIText TextValue;

    private InputAction bindAction; // Reference to an action to rebind.
    private void Start()
    {
      UpdateValue();
    }

    public InputAction BindAction 
    {
      get => bindAction;
      set {
        bindAction = value;
        UpdateValue();
      }
    }
    public int BindingIndex = 0;
    public string ActionName = "";
    public bool OnlyGamepad = false;
    public bool OnlyKeyboard = false;

    public void UpdateValue()
    {
      if (bindAction != null)
        TextValue.text = bindAction.GetBindingDisplayString(BindingIndex);
      else
        TextValue.text = "";
    }
    public void StartWaitKey() 
    {
      if (bindAction != null)
      {
        var uIManager = GameUIManager.Instance;
        uIManager.GoPageWithOptions("PageBindKey", new Dictionary<string, string>() {
          { "keyName", ActionName == "" ? Text.text : ActionName },
          { "keyCurrent", bindAction.GetBindingDisplayString(BindingIndex) },
        });

        var rebindOperation = bindAction.PerformInteractiveRebinding()
          .WithTargetBinding(BindingIndex)
          .WithCancelingThrough("*/{Cancel}");

        if (OnlyGamepad)
          rebindOperation = rebindOperation.WithControlsHavingToMatchPath("<Gamepad>");
        else if (OnlyKeyboard)
          rebindOperation = rebindOperation.WithControlsHavingToMatchPath("<Keyboard>");
          
        rebindOperation.OnComplete(_ => {
          UpdateValue();
          onSelect.Invoke(bindAction.GetBindingDisplayString(BindingIndex));
          uIManager.BackPreviusPage();
          rebindOperation.Dispose();
        })
          .Start();
      }
    }

    public Action<string> onSelect;
  }
}
