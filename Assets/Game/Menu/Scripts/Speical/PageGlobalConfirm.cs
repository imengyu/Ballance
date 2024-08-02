using System.Collections;
using System.Collections.Generic;
using Ballance2.Services;
using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

public class PageGlobalConfirm : MonoBehaviour
{
  public UIText Text;
  public UIText ButtonConfirmText;
  public UIText ButtonCancelText;

  public InputAction Cancel;
  public InputAction Confirm;

  private void Awake() 
  {
    Confirm.performed += (e) => ModalCallback(true);
    Cancel.performed += (e) => ModalCallback(false);
    Cancel.Disable();
    Confirm.Disable();
  }

  public void SetInfo(string title, string confirmText, string cancelText)
  {
    Text.text = title;
    ButtonConfirmText.text = string.IsNullOrEmpty(confirmText) ? "I18N:core.ui.Confirm" : confirmText;
    ButtonCancelText.text= string.IsNullOrEmpty(cancelText) ? "I18N:core.ui.Cancel" : cancelText;
    
    GameTimer.Delay(0.8f, () => {
      Cancel.Enable();
      Confirm.Enable();
    });
  }
  public void ModalCallback(bool confirm)
  {
    Cancel.Disable();
    Confirm.Disable();
    GameUIManager.Instance.GlobalConfirmWindowCallback(confirm);
    GameUIManager.Instance.BackPreviusPage();
  }
}
