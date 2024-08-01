using System;
using System.Collections.Generic;
using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSystemUtil
{
  private static readonly HashSet<string> _reportedCompositeParts = new HashSet<string>();

  public static int GetPrimaryBindingForDevice(this InputAction action, InputDevice device, string[] outControlPaths)
  {
    // use global buffer to avoid re-allocations
    _reportedCompositeParts.Clear();

    var numControlPaths = 0;
    foreach (var binding in action.bindings)
    {
      // make sure output buffer fits more
      if (numControlPaths >= outControlPaths.Length) break;

      // ignore composite bindings, as their device layout and control path are not meaningful
      if (!binding.isComposite)
      {
        // the binding's name corresponds to its composite part, if any
        var compositePart = binding.name;
        if (!_reportedCompositeParts.Contains(compositePart))
        {
          // we don't care about the display string, we just want the layout and control path
          // FIXME: this probably produces unnecessary garbage
          binding.ToDisplayString(out var bindingDeviceLayout, out var bindingControlPath);

          // test if the given device supports the binding's device layout
          if (InputSystem.IsFirstLayoutBasedOnSecond(device.layout, bindingDeviceLayout))
          {
            // report binding
            outControlPaths[numControlPaths++] = bindingControlPath;

            // for any composite part, we only want to report first binding
            _reportedCompositeParts.Add(compositePart);
          }
        }
      }
    }

    // we reported this many control paths
    return numControlPaths;
  }
  
  public static InputControl GetPrimaryControlForDevice(this InputAction action, InputDevice device)
  {
    foreach (var control in action.controls)
    {
      if (control.device.layout == device.layout)
        return control;
    }
    return null;
  }

  public interface BindInputActionHolder
  {
    public void Delete();
    public void ResendIfPressing();
  }
  public class BindInputActionButtonHolder : BindInputActionHolder
  {
    private Action<bool> callback;
    private InputAction action;
    private bool isPressed = false;
    private bool cancelRelease = false;

    public BindInputActionButtonHolder(InputAction action, Action<bool> callback, bool cancelRelease)
    {
      this.action = action;
      this.callback = callback;
      this.cancelRelease = cancelRelease;
      action.performed += OnPerformed;
      action.canceled += OnCanceled;
    }

    private void OnPerformed(InputAction.CallbackContext context)
    {
      if (!isPressed)
      {
        if (context.ReadValueAsButton())
        {
          isPressed = true;
          callback.Invoke(true);
        }
      } 
      else if (!cancelRelease)
      {
        if (!context.ReadValueAsButton())
        {
          isPressed = false;
          callback.Invoke(false);
        }
      }
    }
    private void OnCanceled(InputAction.CallbackContext context)
    {
      if (cancelRelease)
      {
        isPressed = false;
        callback.Invoke(false);
      }
    }

    public bool IsPressed => isPressed;

    public void ResendIfPressing()
    {
      if (isPressed)
        callback.Invoke(true);
    }
    public void Delete()
    {
      action.performed -= OnPerformed;
      action.canceled -= OnCanceled;
    }
  }
  public class BindInputActionSimpleHolder : BindInputActionHolder
  {
    private Action<InputAction.CallbackContext> callback;
    private Action<InputAction.CallbackContext> cancelCallback;
    private InputAction action;

    public BindInputActionSimpleHolder(InputAction action, Action<InputAction.CallbackContext> callback, Action<InputAction.CallbackContext> cancelCallback)
    {
      this.action = action;
      this.callback = callback;
      this.cancelCallback = cancelCallback;
      action.performed += callback;
      action.canceled += cancelCallback;
    }

    public void ResendIfPressing()
    {
      
    }
    public void Delete()
    {
      action.performed -= callback;
      action.canceled -= cancelCallback;
    }
  }
  
  public static BindInputActionSimpleHolder BindInputAction(this InputAction action, Action<InputAction.CallbackContext> callback, Action<InputAction.CallbackContext> cancelCallback)
  {
    return new BindInputActionSimpleHolder(action, callback, cancelCallback);
  }
  public static BindInputActionButtonHolder BindInputActionButton(this InputAction action, Action<bool> callback, bool cancelRelease = true)
  {
    return new BindInputActionButtonHolder(action, callback, cancelRelease);
  }
}