using Ballance2.Package;
using Ballance2.Menu;
using Ballance2.Services;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Services.I18N;

namespace mod.invlife {

  public class PackageEntry {
    private static bool enable = false;
    private static string TAG = "invlife";

    public static GamePackageEntry Main() {
      GamePackageEntry entry = new GamePackageEntry();
      entry.OnLoad = (package) => {
        /*
        此处功能是监听GamePlayManager上的EventBeforeStart事件，在关卡开始之前设置生命为-1（无限）
        */ 
        GameEventEmitterHandler eventHandler = null; 
        GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LEVEL_BUILDER_START, TAG, (evtName, param) => {
          var GamePlayManagerInstance = GamePlayManager.Instance;
          eventHandler = GamePlayManagerInstance.EventBeforeStart.On((param) => {
            if (enable) {
              GamePlayManagerInstance.StartLife = -1;
              GamePlayManagerInstance.CurrentLife = -1;
              GamePlayUIControl.Instance.SetLifeBallCount(-1);
            }
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
        /*
        此处功能是创建一个设置UI界面，包含一个开关“Enable infinite life ball”，
        允许用户控制是否开启无限生命功能
        */
        var setting = GameSettingsManager.GetSettings(package.PackageName);
        var settingsUI = PackageManagerUIControl.Instance.RegisterPackageSettingsUI(package);
        //添加一个开关
        var contol = settingsUI.AddToggle("Enable", I18N.Tr("mod.invlife.ui.EnableTitle"), (val) => {
          //开关参数更改时保存设置
          enable = (bool)val;
          setting.SetBool("enable", enable);
        });
        //页面进入时加载设置
        settingsUI.Page.OnShow += (o) => {
          enable = setting.GetBool("enable");
          contol.ValueBinderSupplierCallback(enable);
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