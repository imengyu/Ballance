using Ballance2.Base;
using Ballance2.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ballance2.Game.GamePlay
{
  public class ControlManager : GameSingletonBehavior<ControlManager>
  {
    public InputAction KeyBoardActionForward;
    public InputAction KeyBoardActionBack;
    public InputAction KeyBoardActionUp;
    public InputAction KeyBoardActionDown;
    public InputAction KeyBoardActionLeft;
    public InputAction KeyBoardActionRight;
    public InputAction KeyBoardActionOverlook;
    public InputAction KeyBoardActionRotateCam;

    public InputAction ControllerActionMove;
    public InputAction ControllerActionFly;
    public InputAction ControllerActionOverlook;
    public InputAction ControllerActionRotateCamLeft;
    public InputAction ControllerActionRotateCamRight;

    public InputAction ScreenShort;
    public InputAction ToggleView;
    public InputAction ToggleFPS;

    public InputAction KeyBoardTestBallWood;
    public InputAction KeyBoardTestBallStone;
    public InputAction KeyBoardTestBallPaper;
    public InputAction KeyBoardTestReset;
    public InputAction KeyBoardTestConsole;

    [HideInInspector]
    public GameEventEmitterStorage EventControlEnabled;
    [HideInInspector]
    public GameEventEmitterStorage EventControlDisabled;

    private void Start() 
    {
      ScreenShort.Enable();
      ToggleView.Enable();
      ToggleFPS.Enable();

      DisableControl();
    }

    public void LoadActionSettings(bool resetAll)
    {
      KeyBoardActionForward.RemoveAllBindingOverrides();
      KeyBoardActionBack.RemoveAllBindingOverrides();
      KeyBoardActionUp.RemoveAllBindingOverrides();
      KeyBoardActionDown.RemoveAllBindingOverrides();
      KeyBoardActionLeft.RemoveAllBindingOverrides();
      KeyBoardActionRight.RemoveAllBindingOverrides();
      KeyBoardActionOverlook.RemoveAllBindingOverrides();
      KeyBoardActionRotateCam.RemoveAllBindingOverrides();

      ControllerActionMove.RemoveAllBindingOverrides();
      ControllerActionFly.RemoveAllBindingOverrides();
      ControllerActionOverlook.RemoveAllBindingOverrides();
      ControllerActionRotateCamLeft.RemoveAllBindingOverrides();
      ControllerActionRotateCamRight.RemoveAllBindingOverrides();

      ScreenShort.RemoveAllBindingOverrides();
      ToggleView.RemoveAllBindingOverrides();
      ToggleFPS.RemoveAllBindingOverrides();
  
      if (!resetAll)
      {
        var settings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
        KeyBoardActionForward.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyFront, ""));
        KeyBoardActionBack.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyBack, ""));
        KeyBoardActionUp.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyUp, ""));
        KeyBoardActionDown.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyDown, ""));
        KeyBoardActionLeft.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyLeft, ""));
        KeyBoardActionRight.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyRight, ""));
        KeyBoardActionOverlook.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyUpCam, ""));
        KeyBoardActionRotateCam.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyRoate, ""));
        ControllerActionMove.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControllerActionMove, ""));
        ControllerActionFly.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControllerActionFly, ""));
        ControllerActionOverlook.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControllerActionOverlook, ""));
        ControllerActionRotateCamLeft.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControllerActionRotateCamLeft, ""));
        ControllerActionRotateCamRight.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControllerActionRotateCamRight, ""));
        ScreenShort.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyScreenShort, ""));
        ToggleView.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyToggleView, ""));
        ToggleFPS.LoadBindingOverridesFromJson(settings.GetString(SettingConstants.SettingsControlKeyToggleFPS, ""));
      }
    }    
    public void SaveActionSettings()
    {
      var settings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      settings.SetString(SettingConstants.SettingsControlKeyFront, KeyBoardActionForward.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyBack, KeyBoardActionBack.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyUp, KeyBoardActionUp.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyDown, KeyBoardActionDown.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyLeft, KeyBoardActionLeft.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyRight, KeyBoardActionRight.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyUpCam, KeyBoardActionOverlook.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyRoate, KeyBoardActionRotateCam.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControllerActionMove, ControllerActionMove.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControllerActionFly, ControllerActionFly.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControllerActionOverlook, ControllerActionOverlook.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControllerActionRotateCamLeft, ControllerActionRotateCamLeft.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControllerActionRotateCamRight, ControllerActionRotateCamRight.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyScreenShort, ScreenShort.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyToggleView, ToggleView.SaveBindingOverridesAsJson());
      settings.SetString(SettingConstants.SettingsControlKeyToggleFPS, ToggleFPS.SaveBindingOverridesAsJson());
      settings.NotifySettingsUpdate(SettingConstants.SettingsControl);
    }

    public void EnableControl()
    {
      KeyBoardTestBallWood.Enable();
      KeyBoardTestBallStone.Enable();
      KeyBoardTestBallPaper.Enable();
      KeyBoardTestReset.Enable();

      KeyBoardActionForward.Enable();
      KeyBoardActionBack.Enable();
      KeyBoardActionUp.Enable();
      KeyBoardActionDown.Enable();
      KeyBoardActionLeft.Enable();
      KeyBoardActionRight.Enable();
      KeyBoardActionOverlook.Enable();
      KeyBoardActionRotateCam.Enable();
      ControllerActionFly.Enable();
      ControllerActionMove.Enable();
      ControllerActionOverlook.Enable();
      ControllerActionRotateCamLeft.Enable();
      ControllerActionRotateCamRight.Enable();

      EventControlEnabled.Emit();
    }
    public void DisableControl()
    {
      KeyBoardTestBallWood.Disable();
      KeyBoardTestBallStone.Disable();
      KeyBoardTestBallPaper.Disable();
      KeyBoardTestReset.Disable();

      KeyBoardActionForward.Disable();
      KeyBoardActionBack.Disable();
      KeyBoardActionUp.Disable();
      KeyBoardActionDown.Disable();
      KeyBoardActionLeft.Disable();
      KeyBoardActionRight.Disable();
      KeyBoardActionOverlook.Disable();
      KeyBoardActionRotateCam.Disable();
      ControllerActionFly.Disable();
      ControllerActionMove.Disable();
      ControllerActionOverlook.Disable();
      ControllerActionRotateCamLeft.Disable();
      ControllerActionRotateCamRight.Disable();

      EventControlDisabled.Emit();
    }
  }
}