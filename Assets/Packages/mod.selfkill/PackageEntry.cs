using Ballance2.Package;
using Ballance2.Menu;
using Ballance2.Services;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Services.I18N;
using UnityEngine;
using UnityEngine.InputSystem;

namespace mod.selfkill {

  public class PackageEntry {
    private static string TAG = "selfkill";

    public static GamePackageEntry Main() {

      //按键处理
      var deathKey = new InputAction("Death", InputActionType.Button);
      deathKey.AddBinding("<keyboard>/r");
      deathKey.AddBinding("<Gamepad>/buttonNorth");
      deathKey.Disable();
      deathKey.performed += (context) => {
        GamePlayManager.Instance.Fall();
      };

      //模组事件处理
      GamePackageEntry entry = new GamePackageEntry
      {
        OnLoad = (package) =>
        {
          GameEventEmitterHandler eventHandler = null;
          //进入关卡事件，进入关卡之后才能访问 GamePlayManager
          GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LEVEL_BUILDER_START, TAG, (evtName, param) =>
          {
            //监听游戏开始与结束事件，只有游戏中才能让我们的按键事件生效
            eventHandler = GamePlayManager.Instance.EventBeforeStart.On((param) => deathKey.Enable());
            eventHandler = GamePlayManager.Instance.EventQuit.On((param) => deathKey.Disable());
            return false;
          });
          //退出关卡事件
          GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LEVEL_BUILDER_UNLOAD_START, TAG, (evtName, param) =>
          {
            eventHandler.Off();
            return false;
          });
          return true;
        },
        OnLoadUI = (package) =>
        {
          //场景模组设置界面
          var setting = GameSettingsManager.GetSettings(package.PackageName);
          var settingsUI = PackageManagerUIControl.Instance.RegisterPackageSettingsUI(package);
          var key = settingsUI.AddKeyChoose("ResetKey", I18N.Tr("mod.selfkill.ui.KeyTitle"));
          var controller = settingsUI.AddKeyChoose("ResetKey", I18N.Tr("mod.selfkill.ui.ControllerTitle"));
          key.OnlyKeyboard = true;
          controller.OnlyGamepad = true;

          settingsUI.Page.OnShow += (o) =>
          {
            deathKey.LoadBindingOverridesFromJson(setting.GetString("key", ""));
            key.BindAction = deathKey;
            key.BindingIndex = 0;
            controller.BindAction = deathKey;
            controller.BindingIndex = 1;
          };
          settingsUI.Page.OnHide += () =>
          {
            setting.SetString("key", deathKey.SaveBindingOverridesAsJson());
          };
          return true;
        },
        OnBeforeUnLoad = (package) =>
        {
          return true;
        },
        Version = 1
      };
      return entry;
    }
  }
}