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
  public class KeyChoose : MonoBehaviour, ISelectHandler, IDeselectHandler
  {
    public UIText Text;
    public UIText TextValue;
    
    public InputAction ResetAction;

    private InputAction bindAction; // Reference to an action to rebind.

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
    public bool NoXYControl = true;

    private void Awake() {
      ResetAction.performed += (e) => {
        bindAction.RemoveAllBindingOverrides();
        UpdateValue();
      };
      ResetAction.Disable();
    }
    private void OnEnable() 
    {
      UpdateValue();
    }

    public void UpdateValue()
    {
      try {
        if (bindAction != null)
          TextValue.text = bindAction.GetBindingDisplayString(BindingIndex);
        else
          TextValue.text = "";
      } 
      catch (Exception e)
      {
        Debug.LogWarning("KeyChoose failed to get DisplayString: " + e.ToString());
        TextValue.text = "ERROR";
      }
    }
    public void StartWaitKey() 
    {
      if (bindAction != null)
      {
        var uIManager = GameUIManager.Instance;
        uIManager.GoPageWithOptions("PageBindKey", new Dictionary<string, object>() {
          { "keyName", ActionName == "" ? Text.text : ActionName },
          { "keyCurrent", bindAction.GetBindingDisplayString(BindingIndex) },
        });

        var rebindOperation = bindAction.PerformInteractiveRebinding()
          .WithTargetBinding(BindingIndex)
          .WithCancelingThrough("*/{Cancel}");

        if (NoXYControl)
          rebindOperation = rebindOperation
            .WithControlsExcluding("<Gamepad>/leftStick/x")
            .WithControlsExcluding("<Gamepad>/leftStick/y")
            .WithControlsExcluding("<Gamepad>/rightStick/x")
            .WithControlsExcluding("<Gamepad>/rightStick/y");
        if (OnlyGamepad)
          rebindOperation = rebindOperation.WithControlsHavingToMatchPath("<Gamepad>");
        else if (OnlyKeyboard)
          rebindOperation = rebindOperation.WithControlsHavingToMatchPath("<Keyboard>");
          
        rebindOperation.OnComplete(_ => {
          UpdateValue();
          onSelect?.Invoke(bindAction.GetBindingDisplayString(BindingIndex));
          uIManager.BackPreviusPage();
          rebindOperation.Dispose();
        })
          .Start();
      }
    }

    public void OnSelect(BaseEventData eventData)
    {
      ResetAction.Enable();
    }
    public void OnDeselect(BaseEventData eventData)
    {
      ResetAction.Disable();
    }

    public Action<string> onSelect;
  }
}
