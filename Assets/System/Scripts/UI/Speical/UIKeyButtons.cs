using System;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Ballance2.Utils;

namespace Ballance2.UI 
{ 
  public class UIKeyButtons : MonoBehaviour 
  {
    [Serializable]
    public class DisplayActionMap
    {
      public string MapName;
      [SerializeField]
      [Reorderable]
      public List<DisplayAction> DisplayKeys;
    }    
    [Serializable]
    public class DisplayAction 
    {
      public string DisplayName;
      public string Key;
    }
    [Serializable]
    public class StaticDisplayAction 
    {
      public string DisplayName;
      public List<StaticDisplayIcons> Icons;
    }
    [Serializable]
    public class StaticDisplayIcons
    {
      public Sprite Icon;
      public string Key;
      public StaticDisplayActionType Type;
    }
    public enum StaticDisplayActionType
    {
      Keyboard,
      XBox,
      Dual
    }

    [SerializeField]
    private RectTransform keyboardBox;
    [SerializeField]
    private GameObject keyButtonPrefab;
    [SerializeField]
    [Reorderable]
    private List<DisplayActionMap> actions = new List<DisplayActionMap>();
    [SerializeField]
    [Reorderable]
    private List<StaticDisplayAction> staticDisplayActions = new List<StaticDisplayAction>();

    private void SetKeyButtons() 
    {
      keyboardBox.DestroyAllChildren();
      
      if (actions.Count > 0)
      {
        var inputSystemUIInputModule = EventSystem.current.gameObject.GetComponent<InputSystemUIInputModule>();
        var actionsAsset = inputSystemUIInputModule.actionsAsset;

        foreach (var map in actions)
        {     
          var InputActions = actionsAsset.FindActionMap(map.MapName);
          foreach (var action in InputActions)
          {
            var displayAction = map.DisplayKeys.Find((a) => a.Key == action.name);
            if (displayAction != null)
            {
              var controlPath = GetCurrentControlPath(action);
              if (controlPath != null)
              {
                var keyButton = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIKeyButton>(keyButtonPrefab, keyboardBox.transform);
                keyButton.SetKeyImageWithControlPath(controlPath.Item1, controlPath.Item2);
                keyButton.SetActionName(displayAction.DisplayName);
                keyButton.SetOnScreenButton(controlPath.Item1);
              }
            }
          }
        }

        dirty = 5;
      }
      if (staticDisplayActions.Count > 0)
      {
        foreach(var action in staticDisplayActions)
        {
          var keyButton = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIKeyButton>(keyButtonPrefab, keyboardBox.transform);
          keyButton.SetKeyImageWithStaticImage(action);
          keyButton.SetActionName(action.DisplayName);
          keyButtons.Add(keyButton);
        }
      }
    }
    private Tuple<string, string> GetCurrentControlPath(InputAction action) 
    {
      if (Gamepad.current != null) {
        var control = action.GetPrimaryControlForDevice(Gamepad.current);
        if (control == null)
          return null;
        return new Tuple<string, string>("<Gamepad>/" + control.name, control.shortDisplayName ?? control.name);
      }
      if (Keyboard.current != null)
      {
        var control = action.GetPrimaryControlForDevice(Keyboard.current);
        if (control == null)
          return null;
        return new Tuple<string, string>("<Keyboard>/" + control.name, control.shortDisplayName ?? control.name);
      }
      return null;
    }

    private int dirty = 0;
    private List<UIKeyButton> keyButtons = new List<UIKeyButton>();

    private void Update() 
    {
      if (dirty >= 0)
        dirty--;
      if (dirty == 0)
        LayoutRebuilder.ForceRebuildLayoutImmediate(keyboardBox);
    }

    public void EnableAllDisplayActions()
    {
      foreach (var action in keyButtons)
        action.EnableOnScreenButton();
    }
    public void DeleteAllDisplayActions()
    {
      foreach (var action in keyButtons)
        action.DisableOnScreenButton();
      keyboardBox.transform.DestroyAllChildren();
      keyButtons.Clear();
    }
    public void AddDisplayActions(InputAction action, string DisplayName, Action actualAction = null) 
    {
      var controlPath = GetCurrentControlPath(action);
      if (controlPath != null)
      {
        var keyButton = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIKeyButton>(keyButtonPrefab, keyboardBox.transform);
        keyButton.SetKeyImageWithControlPath(controlPath.Item1, controlPath.Item2);
        keyButton.SetActionName(DisplayName);
        if (actualAction != null)
          keyButton.onClick.AddListener(() => actualAction.Invoke());
        else
          keyButton.SetOnScreenButton(controlPath.Item1);
        keyButtons.Add(keyButton);
        dirty = 5;
      }
    }
    public void AddStaticDisplayActions(StaticDisplayAction action) 
    {
      var keyButton = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIKeyButton>(keyButtonPrefab, keyboardBox.transform);
      keyButton.SetKeyImageWithStaticImage(action);
      keyButton.SetActionName(action.DisplayName);
      keyButton.interactable = false;
      keyButtons.Add(keyButton);
      dirty = 5;
    }

    public List<DisplayActionMap> Actions 
    {
      get { return actions; }
      set {
        actions = value;
        SetKeyButtons();
      }
    }
  }
}