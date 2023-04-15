using Ballance2.Package;
using Ballance2.Menu;
using Ballance2.Services;
using Ballance2.Base;
using Ballance2.Game.GamePlay;

namespace com.ballance.mod.invlife {

  public class PackageEntry {
    private static bool enable = false;
    private static string TAG = "invlife";

    public static GamePackageEntry Main() {
      GamePackageEntry entry = new GamePackageEntry();
      entry.OnLoad = (package) => {
        /*
        此处功能是监听GamePlayManager上的EventBeforeStart事件，在关卡开始之前设置生命为-1（无限）
        */
        GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LEVEL_BUILDER_START, TAG, (evtName, param) => {
          var GamePlayManagerInstance = GamePlayManager.Instance;
          GamePlayManagerInstance.EventBeforeStart.On((param) => {
            if (enable) {
              GamePlayManagerInstance.StartLife = -1;
              GamePlayManagerInstance.CurrentLife = -1;
              GamePlayUIControl.Instance.SetLifeBallCount(-1);
            }
          });
          return false;
        });
        return true;
      };
      entry.OnLoadUI = (package) => {
        /*
        此处功能是创建一个设置UI界面，包含一个开关“Enable infinite life ball”，
        允许用户控制是否开启无限生命功能
        */
        var setting = GameSettingsManager.GetSettings(package.PackageName);
        var settingsUI = PackageManagerUIControl.Instance.RegisterPackageSettingsUI(package);
        var contol = settingsUI.AddToggle("Enable", "Enable infinite life ball", (val) => {
          enable = (bool)val;
          setting.SetBool("enable", enable);
        });

        enable = setting.GetBool("enable");
        contol.ValueBinderSupplierCallback(enable);
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