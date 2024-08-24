using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Services.I18N;
using System.Collections.Generic;

namespace mod.speedball {

  public class PackageEntry {
    const string TAG = "speedball";

    private static int ballSpeedChoice = 0;
    private static List<float> ballSpeedChoices = new List<float>() {
      1,
      1.13f,
      1.41f,
      1.73f,
      2,
      3,
    };

    public static GamePackageEntry Main() {
      GamePackageEntry entry = new GamePackageEntry();
      entry.OnLoad = (package) => {
        var setting = GameSettingsManager.GetSettings(package.PackageName);
        ballSpeedChoice = setting.GetInt("choice", 0);

        /*
        此处功能是监听GamePlayManager上的EventBeforeStart事件，在关卡开始之前设置倍速球设置
        */
        GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LEVEL_BUILDER_START, TAG, (evtName, param) => {
          GamePlayManager.Instance.BallManager.BallSpeedFactor = ballSpeedChoices[ballSpeedChoice];
          return false;
        });
        return true;
      };
      entry.OnLoadUI = (package) => {
        var setting = GameSettingsManager.GetSettings(package.PackageName);
        var settingsUI = Ballance2.Menu.PackageManagerUIControl.Instance.RegisterPackageSettingsUI(package);
        var contol = settingsUI.AddUpdown("Enable", I18N.Tr("mod.speedball.ui.EnableTitle"), (val) => {
          ballSpeedChoice = (int)(float)val;
          setting.SetInt("choice", ballSpeedChoice);
        });
        contol.Control.SetOptions(ballSpeedChoices.ConvertAll<string>((v) => v.ToString()));
        contol.Control.AppendText = I18N.Tr("mod.speedball.ui.BeiText");

        settingsUI.Page.OnShow += (o) => {
          ballSpeedChoice = setting.GetInt("choice", 0);
          contol.ValueBinderSupplierCallback(ballSpeedChoice);
        };
        return true;
      };
      entry.OnBeforeUnLoad = (package) => {
        return true;
      };
      entry.Version = 1; //返回版本号
      return entry;
    }
  }
}