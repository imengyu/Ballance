using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorConfirmControl : MonoBehaviour 
  {
    public Button ButtonConfirm;
    public Button ButtonCancel;
    public UIText ButtonConfirmText;
    public UIText ButtonCancelText;
    public UIText TitleText;
    public UIText ContentText;
    public TMP_InputField InputField;
    public Image Icon;

    public Sprite IconNone;
    public Sprite IconInfo;
    public Sprite IconWarning;
    public Sprite IconError;

    public Action<bool, string> onClose;

    private void Start() 
    {
      ButtonConfirm.onClick.AddListener(() => {
        onClose.Invoke(true, InputField.text);
        gameObject.SetActive(false);
      });
      ButtonCancel.onClick.AddListener(() => {
        onClose.Invoke(false, InputField.text);
        gameObject.SetActive(false);
      });
    }
    public void SetInfo(
      string title, string content, 
      LevelEditorConfirmIcon icon = LevelEditorConfirmIcon.None, 
      string confirmText = "", string cancelText = "", 
      bool showCancel = true, bool showInput = false,
      string inputFieldText = "", string inputFieldPlaceholder = ""
    )
    {
      ButtonConfirmText.text = string.IsNullOrEmpty(confirmText) ? "I18N:core.ui.Ok" : confirmText;
      ButtonCancelText.text = string.IsNullOrEmpty(cancelText) ? "I18N:core.ui.Cancel" : cancelText;
      TitleText.text = title;
      ContentText.text = content;
      ButtonCancel.gameObject.SetActive(showCancel);
      InputField.gameObject.SetActive(showInput);
      InputField.text = inputFieldText;
      (InputField.placeholder as TMP_Text).text = inputFieldPlaceholder;
      switch(icon)
      {
        case LevelEditorConfirmIcon.None: 
          Icon.sprite = IconNone;
          break;
        case LevelEditorConfirmIcon.Info: 
          Icon.sprite = IconInfo;
          break;
        case LevelEditorConfirmIcon.Warning: 
          Icon.sprite = IconWarning;
          break;
        case LevelEditorConfirmIcon.Error: 
          Icon.sprite = IconError;
          break;
      }
    }
  }
  public enum LevelEditorConfirmIcon
  {
    None,
    Info,
    Warning,
    Error,
  }
}