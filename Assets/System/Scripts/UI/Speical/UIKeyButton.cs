using System.Text;
using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using static Ballance2.UI.UIKeyButtons;

namespace Ballance2.UI 
{ 
  [AddComponentMenu("Ballance/UI/Controls/KeyButton")]
  public class UIKeyButton : Button 
  {
    private OnScreenButton onScreenButton = null;
    [SerializeField]
    private Image controllerIcon;
    [SerializeField]
    private RectTransform keyboardBox;
    [SerializeField]
    private UIText keyboardText;
    [SerializeField]
    private UIText actionText;

    private string GetKeyName(string name)
    {
      var arr = name.Split('/');
      var sb = new StringBuilder();
      for (var i = 1; i < arr.Length; i++)
      {
        sb.Append(arr[i]);
        if (i != arr.Length - 1) sb.Append("/");
      }
      return sb.ToString();
    }
    
    public void SetKeyImageWithControlPath(string controlPath, string key) 
    {
      if (controlPath.StartsWith("<Keyboard>/")) {
        controllerIcon.gameObject.SetActive(false);
        keyboardBox.gameObject.SetActive(true);
        keyboardText.text = key;
        return;
      } 

      if (controlPath.StartsWith("<Gamepad>/")) {
        var current = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        if (current != null) {
          controllerIcon.gameObject.SetActive(true);
          keyboardBox.gameObject.SetActive(false);

          var controlType = "";
          if (
            current.name.StartsWith("XInput") 
            || current.name.StartsWith("XBox") 
          ) 
            controlType = "Xbox";
          else if (current.name.StartsWith("Dual")) 
            controlType = "Dual";

          controllerIcon.sprite = ControllerKeyIconMap.Instance.GetControllerKeyIcon(
            controlType,
            GetKeyName(controlPath)
          );
          return;
        }
      } 
        
      controllerIcon.gameObject.SetActive(false);
      keyboardBox.gameObject.SetActive(false);
    } 
    public void SetActionName(string actionName) 
    {
      actionText.text = actionName;
    }
    public void SetKeyImageWithStaticImage(StaticDisplayAction staticImages) 
    {
      var controlType = "Keyboard";
      var current = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
      if (current != null)
      {
        if (
          current.name.StartsWith("XInput") 
          || current.name.StartsWith("XBox") 
        ) 
          controlType = "Xbox";
        else if (current.name.StartsWith("Dual")) 
          controlType = "Dual";

      }
      if (controlType == "Keyboard") {
        controllerIcon.gameObject.SetActive(false);
        keyboardBox.gameObject.SetActive(true);
        keyboardText.text = staticImages.Icons.Find((p) => p.Type == StaticDisplayActionType.Keyboard).Key;
        return;
      } 
      if (controlType != "") {
        StaticDisplayActionType type = StaticDisplayActionType.Keyboard;
        switch (controlType)
        {
          case "Xbox": type = StaticDisplayActionType.XBox; break;
          case "Dual": type = StaticDisplayActionType.Dual; break;
        }
        controllerIcon.gameObject.SetActive(true);
        keyboardBox.gameObject.SetActive(false);
        controllerIcon.sprite = staticImages.Icons.Find((p) => p.Type == type).Icon;
        return;
      }
        
      controllerIcon.gameObject.SetActive(false);
      keyboardBox.gameObject.SetActive(false);
    }

    public void EnableOnScreenButton()
    {
      if (onScreenButton != null)
        onScreenButton.enabled = true;
    }
    public void DisableOnScreenButton()
    {
      if(onScreenButton != null) 
        onScreenButton.enabled = false;
    }
    public void SetOnScreenButton(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        interactable = false;
        if (onScreenButton != null) {
          Destroy(onScreenButton);
          onScreenButton = null;
        }
      }
      else 
      {
        interactable = true;
        if (onScreenButton == null)
          onScreenButton = gameObject.AddComponent<OnScreenButton>();
        onScreenButton.controlPath = path;
      }
    }

  }
}