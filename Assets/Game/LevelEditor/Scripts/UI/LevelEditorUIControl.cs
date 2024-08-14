using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorUIControl : MonoBehaviour 
  {
    public RectTransform BottomBarTest;
    public RectTransform BottomBarEditor;
    public LevelEditorConfirmControl LevelEditorConfirmControl;
    public LevelEditorContentSelection LevelEditorContentSelection;
    public LevelEditorTransformToolControl LevelEditorTransformToolControl;
    public LevelEditorObjectInfoControl LevelEditorObjectInfoControl;
    public GameObject TransformHandles;
    public RectTransform DialogTransformHelp;
    public RectTransform DialogTestHelp;
    public RectTransform DialogLevelInfo;
    public RectTransform DialogLoading;
    public RectTransform DialogPause;
    public RectTransform MouseTip;
    public UIText MouseTipText;
    public UIText DialogLoadingText;
    public UIText TextStatus;

    public List<Color> SectorColors;
    public List<LevelDynamicModel> SelectedObject = new List<LevelDynamicModel>();

    public Color GetSectorColor(int sector)
    {
      if (sector > SectorColors.Count)
        sector %= SectorColors.Count;
      return SectorColors[sector];
    }

    public UIButtonActiveState ButtonFloors;
    public UIButtonActiveState ButtonRails;
    public UIButtonActiveState ButtonModuls;
    public UIButtonActiveState ButtonOthers;

    public InputAction DeleteAction;

    private void Awake() 
    {
      ButtonFloors.onClick = () => ShowContentSelectUI(LevelDynamicModelCategory.Floors);
      ButtonRails.onClick = () => ShowContentSelectUI(LevelDynamicModelCategory.Rails);
      ButtonModuls.onClick = () => ShowContentSelectUI(LevelDynamicModelCategory.Moduls);
      ButtonOthers.onClick = () => ShowContentSelectUI(LevelDynamicModelCategory.Decoration);
      DeleteAction.performed += (e) => DeleteSelection();
      DeleteAction.Disable();
      LevelEditorConfirmControl.onClose = (confirm, text) => {
        if (confirm) onConfirm?.Invoke(text);
        else onCancel?.Invoke();
      };
      LevelEditorTransformToolControl.OnSelect = (objects) => {
        LevelEditorObjectInfoControl.SetSelectModel(objects);
        ReSetSelect(objects);
        HideLevelInfo();
        UpdateStatusText();
      };
    }
    private void ReSetSelect(LevelDynamicModel[] newSels)
    {
      foreach (var item in SelectedObject)
        item.SelectFlag = -1;
      foreach (var item in newSels)
      {
        item.SelectFlag = 1;
        if (!SelectedObject.Contains(item))
        {
          item.ConfigueRef.OnEditorSelected(newSels.Length == 1);
          SelectedObject.Add(item);
        }
      }
      for (int i = SelectedObject.Count - 1; i >= 0 ; i--)
      {
        if (SelectedObject[i].SelectFlag == -1)
          SelectedObject.RemoveAt(i);
      }
    }

    private Action<string> onConfirm = null;
    private Action onCancel = null;
    private LevelDynamicModelCategory currentShowTab = LevelDynamicModelCategory.UnSet;

    public void Init()
    {
      SetToolBarMode(ToolBarMode.Edit);
      if (GameManager.Instance.GameSettings.GetBool("showHelpAtStart", true))
        ShowTransformHelp();
    }

    public enum ToolBarMode
    {
      None,
      Edit,
      Test
    }
    public void SetToolBarMode(ToolBarMode mode)
    {
      switch (mode)
      {
        case ToolBarMode.None:
          BottomBarTest.gameObject.SetActive(false);
          BottomBarEditor.gameObject.SetActive(false);
          LevelEditorTransformToolControl.gameObject.SetActive(false);
          CloseContentSelectUI();
          HideLevelInfo();
          HideMouseTip();
          HideTransformHelp();
          DeleteAction.Disable();
          TransformHandles.SetActive(false);
          break;
        case ToolBarMode.Edit:
          BottomBarTest.gameObject.SetActive(false);
          BottomBarEditor.gameObject.SetActive(true);
          LevelEditorTransformToolControl.gameObject.SetActive(true);
          ShowContentSelectUI(LevelDynamicModelCategory.UnSet);
          DeleteAction.Enable();
          TransformHandles.SetActive(true);
          break;
        case ToolBarMode.Test:
          BottomBarTest.gameObject.SetActive(true);
          BottomBarEditor.gameObject.SetActive(false);
          LevelEditorTransformToolControl.gameObject.SetActive(false);
          LevelEditorContentSelection.gameObject.SetActive(false);
          ShowContentSelectUI(LevelDynamicModelCategory.UnSet);
          HideLevelInfo();
          HideMouseTip();
          HideTransformHelp();
          DeleteAction.Disable();
          TransformHandles.SetActive(false);
          break;
      }
    }

    public void SetShowHelpAtStart(bool v) {
      GameManager.Instance.GameSettings.SetBool("showHelpAtStart", v);
    }
    public void CloseContentSelectUI()
    {
      ButtonFloors.SetActive(false);
      ButtonRails.SetActive(false);
      ButtonModuls.SetActive(false);
      ButtonOthers.SetActive(false);
       LevelEditorContentSelection.Close();
    }
    public void ShowContentSelectUI(LevelDynamicModelCategory tab)
    {
      ButtonFloors.SetActive(false);
      ButtonRails.SetActive(false);
      ButtonModuls.SetActive(false);
      ButtonOthers.SetActive(false);
      if (currentShowTab == tab)
      {
        LevelEditorContentSelection.Close();
        currentShowTab = LevelDynamicModelCategory.UnSet;
        return;
      }
      currentShowTab = tab;
      switch (tab)
      {
        case LevelDynamicModelCategory.Floors: ButtonFloors.SetActive(true); break;
        case LevelDynamicModelCategory.Rails: ButtonRails.SetActive(true); break;
        case LevelDynamicModelCategory.Moduls: ButtonModuls.SetActive(true); break;
        case LevelDynamicModelCategory.Decoration: ButtonOthers.SetActive(true); break;
      }
      LevelEditorContentSelection.LoadCategory(tab);
    }

    public void ShowTransformHelp()
    {
      DialogTransformHelp.gameObject.SetActive(true);
    }
    public void HideTransformHelp()
    {
      DialogTransformHelp.gameObject.SetActive(false);
    }
    public void ShowTestHelp()
    {
      DialogTestHelp.gameObject.SetActive(true);
    }
    public void HideTestHelp()
    {
      DialogTestHelp.gameObject.SetActive(false);
    }
      
    public void ShowLevelInfo()
    {
      LevelEditorObjectInfoControl.gameObject.SetActive(false);
      DialogLevelInfo.gameObject.SetActive(true);
    }
    public void HideLevelInfo()
    {
      DialogLevelInfo.gameObject.SetActive(false);
    }
    public void SwitchLevelInfo()
    {
      if (!DialogLevelInfo.gameObject.activeSelf)
        ShowLevelInfo();
      else
        HideLevelInfo();
    }
    
    public void SetPauseTipShow(bool s)
    {
      DialogPause.gameObject.SetActive(s);
    }
      
    public void ClearSelectionWhenDelete()
    {
      LevelEditorTransformToolControl.ClearSelection();
    }
    public void DeleteSelection()
    {
      if (SelectedObject.Count > 1)
      {
        Confirm("I18N:core.ui.Tip", I18N.TrF("core.editor..messages.DeleteAsk", "", SelectedObject.Count), LevelEditorConfirmIcon.Warning, onConfirm: (_) => {
          LevelEditorTransformToolControl.DoDeleteSeletedObjects();
        });
      }
      else if (SelectedObject.Count == 1)
      {
        LevelEditorTransformToolControl.DoDeleteSeletedObjects();
      }
    }
    public void UpdateStatusText()
    {
      var level = LevelEditorManager.Instance?.LevelCurrent;
      if (level != null)
      {
        TextStatus.text = I18N.TrF("core.editor.StatusText", "", 
          level.LevelData.LevelModels.Count,
          level.GetSectorCount(),
          SelectedObject.Count
        );
      }
      else
      {
        TextStatus.text = "";
      }
    }

    public void Confirm(
      string title, string content, 
      LevelEditorConfirmIcon icon = LevelEditorConfirmIcon.None, 
      string confirmText = "", string cancelText = "", 
      bool showInput = false,
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
      this.onConfirm = (e) => onConfirm?.Invoke();
      LevelEditorConfirmControl.gameObject.SetActive(true);
      LevelEditorConfirmControl.SetInfo( title, content, icon, confirmText, "", false);
    }
  
    public void ShowLoading(string text = "")
    {
      DialogLoading.gameObject.SetActive(true);
      UpdateLoading(string.IsNullOrEmpty(text) ? "I18N:core.editor.Loading" : text);
    }
    public void UpdateLoading(string text)
    {
      DialogLoadingText.text = text;
    }
    public void HideLoading()
    {
      DialogLoading.gameObject.SetActive(false);
    }
    
    public void ShowMouseTip(string text)
    {
      MouseTip.gameObject.SetActive(true);
      UpdateMouseTip(text);
    }
    public void UpdateMouseTip(string text)
    {
      MouseTipText.text = text;
    }
    public void HideMouseTip()
    {
      MouseTip.gameObject.SetActive(false);
    }
  
  }
}