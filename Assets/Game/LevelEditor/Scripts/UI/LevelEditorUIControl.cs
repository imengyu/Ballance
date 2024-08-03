using System;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorUIControl : MonoBehaviour 
  {
    public RectTransform BottomBarTest;
    public RectTransform BottomBarEditor;
    public LevelEditorConfirmControl LevelEditorConfirmControl;

    private void Start() 
    {
      LevelEditorConfirmControl.onClose = (confirm, text) => {
        if (confirm) onConfirm?.Invoke(text);
        else onCancel?.Invoke();
      };
    }

    private Action<string> onConfirm = null;
    private Action onCancel = null;

    public void Confirm(
      string title, string content, 
      LevelEditorConfirmIcon icon = LevelEditorConfirmIcon.None, 
      string confirmText = "", string cancelText = "", 
      bool showInput = true,
      string inputFieldText = "", string inputFieldPlaceholder = "",
      Action<string> onConfirm = null, Action onCancel = null)
    {
      this.onConfirm = onConfirm;
      this.onCancel = onCancel;
      LevelEditorConfirmControl.gameObject.SetActive(true);
      LevelEditorConfirmControl.SetInfo( 
        title, content, icon, confirmText, 
        cancelText, showCancel: true,
        showInput: showInput,
        inputFieldText: inputFieldText,
        inputFieldPlaceholder: inputFieldPlaceholder
      );
    }
    public void Alert(
      string title, string content, 
      LevelEditorConfirmIcon icon = LevelEditorConfirmIcon.None,
      string confirmText = "", 
      Action onConfirm = null
    )
    {
      this.onConfirm = (e) => onConfirm();
      LevelEditorConfirmControl.gameObject.SetActive(true);
      LevelEditorConfirmControl.SetInfo( title, content, icon, confirmText, "", false);
    }
  
  

  }
}