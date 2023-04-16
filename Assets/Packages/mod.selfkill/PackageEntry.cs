using Ballance2.Package;
using Ballance2.Menu;
using Ballance2.Services;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Services.I18N;
using UnityEngine;

namespace mod.selfkill {

  public class PackageEntry {
    private static KeyCode key = KeyCode.R;
    private static string TAG = "selfkill";

    public static GamePackageEntry Main() {
      GamePackageEntry entry = new GamePackageEntry();
      entry.OnLoad = (package) => {
        GameEventEmitterHandler eventHandler = null; 
        GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LEVEL_BUILDER_START, TAG, (evtName, param) => {
          var GamePlayManagerInstance = GamePlayManager.Instance;
          var KeyListenId = 0;
          eventHandler = GamePlayManagerInstance.EventBeforeStart.On((param) => {
            KeyListenId = GameUIManager.Instance.ListenKey(key, (key, downed) => {
              GamePlayManagerInstance.Fall();
            });
          });
          eventHandler = GamePlayManagerInstance.EventQuit.On((param) => {
            GameUIManager.Instance.DeleteKeyListen(KeyListenId);
          });
          return false;
        });
        GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LEVEL_BUILDER_UNLOAD_START, TAG, (evtName, param) => {
          eventHandler.Off();
          return false;
        });
        return true;
      };
      entry.OnLoadUI = (package) => {
        var setting = GameSettingsManager.GetSettings(package.PackageName);
        var settingsUI = PackageManagerUIControl.Instance.RegisterPackageSettingsUI(package);
        var contol = settingsUI.AddKeyChoose("ResetKey", I18N.Tr("mod.selfkill.ui.KeyTitle"), (val) => {
          key = (KeyCode)val;
          setting.SetInt("key", (int)key);
        });
        settingsUI.Page.OnShow += (o) => {
          key = (KeyCode)setting.GetInt("key", (int)key);
          contol.ValueBinderSupplierCallback(key);
        };
        return true;
      };
      entry.OnBeforeUnLoad = (package) => {
        return true;
      };
      entry.Version = 1;
      return entry;
    }
  }
}