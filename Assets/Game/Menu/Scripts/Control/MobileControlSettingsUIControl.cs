using System.Collections.Generic;
using Ballance2.Game;
using Ballance2.Game.GamePlay;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Services.InputManager;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class MobileControlSettingsUIControl : MonoBehaviour {
    public ScrollView ScrollView;

    private class MobileControlItem {
      public string name;
      public string explain;
      public Sprite image;
    }
    private List<MobileControlItem> items = new List<MobileControlItem>();

    private void Start() {
      var settings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      var page = GameUIManager.Instance.FindPage("PageSettingsControls");
      var controlKeypadSettting = "";

      ScrollView.updateFunc = (index, item) => {
        var data = items[index];
        var Listener = item.GetComponent<EventTriggerListener>();
        var ImageBg = item.Find("ImageBg");
        var text = item.Find("Text").GetComponent<TMP_Text>();
        var ImageBgActive = item.Find("ImageBgActive");
        var image = item.Find("Image").GetComponent<Image>();

        if (controlKeypadSettting == data.name) {
          ImageBg.gameObject.SetActive(false);
          ImageBgActive.gameObject.SetActive(true);
        } else {
          ImageBg.gameObject.SetActive(true);
          ImageBgActive.gameObject.SetActive(false);
        }

        text.text = data.explain;
        image.sprite = data.image;

        Listener.onClick = (o) => {
          controlKeypadSettting = data.name;
          settings.SetString(SettingConstants.SettingsControlKeypad, data.name);

          //更新列表
          ScrollView.UpdateData(false);

          //如果游戏正在运行中，则动态更换键盘
          if (GamePlayUIControl.Instance != null)
            GamePlayUIControl.Instance.ReBuildMobileKeyPad();
        };
      };
      ScrollView.itemCountFunc = () => {
        return items.Count;
      };

      page.OnShow = (o) => {
        controlKeypadSettting = settings.GetString("control.keypad", "BaseCenter");
        LoadList();
        ScrollView.UpdateData(false);
      };
    }
    private void LoadList() {
      items.Clear();
      foreach (var item in KeypadUIManager.GetAllKeypad())
      {
        items.Add(new MobileControlItem() {
          name = item.Key,
          image = item.Value.image,
          explain = item.Value.explain,
        });
      }
    }
  }
}