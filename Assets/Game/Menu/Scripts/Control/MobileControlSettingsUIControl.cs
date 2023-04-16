using System.Collections.Generic;
using Ballance2.Game.GamePlay;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Services.InputManager;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class MobileControlSettingsUIControl : MonoBehaviour {
    public ScrollView ScrollView;

    private class MobileControlItem {
      public string name;
      public Sprite image;
    }
    private List<MobileControlItem> items = new List<MobileControlItem>();

    private void Start() {
      var settings = GameSettingsManager.GetSettings("core");
      var page = GameUIManager.Instance.FindPage("PageSettingsControlsMobile");
      var controlKeypadSettting = "";

      ScrollView.updateFunc = (index, item) => {
        var data = items[index];
        var Listener = item.GetComponent<EventTriggerListener>();
        var ImageBg = item.Find("ImageBg");
        var ImageBgActive = item.Find("ImageBgActive");
        var image = item.Find("Image").GetComponent<Image>();

        if (controlKeypadSettting == data.name) {
          ImageBg.gameObject.SetActive(false);
          ImageBgActive.gameObject.SetActive(true);
        } else {
          ImageBg.gameObject.SetActive(true);
          ImageBgActive.gameObject.SetActive(false);
        }

        image.sprite = data.image;

        Listener.onClick = (o) => {
          controlKeypadSettting = data.name;
          settings.SetString("control.keypad", data.name);

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
        controlKeypadSettting = settings.GetString("control.keypad", "BaseLeft");
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
        });
      }
    }
  }
}