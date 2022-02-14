using System.Collections.Generic;
using Ballance2;
using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* PackageManagerContentUIControl.cs
* 
* 用途：
* 模块管理器窗口的对话框逻辑。
*
* 作者：
* mengyu
*/

public class PackageManagerUIControl : MonoBehaviour {
  
  private const string TAG = "PackageManagerUIControl";

  public ScrollView ScrollView;
  public Button ButtonSave;
  public Button ButtonCancel;
  public Button ButtonEnableAll;
  public Button ButtonDisableAll;
  public Text TextStatus;

  private GamePackageManager gamePackageManager;
  private GameUIManager gameUIManager;
  private GameUIPage page;

  private void Start() {
    gamePackageManager = GameSystem.GetSystemService("GamePackageManager") as GamePackageManager;
    gameUIManager = GameSystem.GetSystemService("GameUIManager") as GameUIManager;

    ScrollView.SetItemCountFunc(() => packageItems.Count);
    ScrollView.SetUpdateFunc(ItemUpdateFn);

    page = gameUIManager.FindPage("PagePackageManager");
    page.OnShow = (options) => {
      if(options.ContainsKey("LocatePackage")) 
        SelectPackage(options["LocatePackage"]);
      else
        SelectPackage(GamePackageManager.CORE_PACKAGE_NAME);
    };
    LoadList();
  }

  private class PackageStateChange {
    public string name;
    public bool newState;
  }
  private class PackageInfoItem {
    public string packageName;
    public bool enableLoad;
    public bool notUnLoadable;
    public string name;
    public string introduction;
    public Sprite logo;
  }
  private List<PackageStateChange> packageStateChanges = new List<PackageStateChange>();
  private List<PackageInfoItem> packageItems = new List<PackageInfoItem>();

  //条目更新函数
  private void ItemUpdateFn(int index, RectTransform item) {
    
    var data = packageItems[index];

    var Button = item.gameObject.GetComponent<Button>();
    var ImageLogo = item.Find("ImageLogo").GetComponent<Image>();
    var TextTitle = item.Find("TextTitle").GetComponent<Text>();
    var TextIntrod = item.Find("TextIntrod").GetComponent<Text>();
    var ToggleEnable = item.Find("ToggleEnable").GetComponent<ToggleEx>();

    Button.onClick.RemoveAllListeners();
    Button.onClick.AddListener(() => {
      Dictionary<string, string> options = new Dictionary<string, string>();
      options.Add("PackageName", data.packageName);
      gameUIManager.GoPageWithOptions("PagePackageManagerInfo", options);
    });

    if(data.notUnLoadable) {
      ToggleEnable.isOn = data.enableLoad;
      ToggleEnable.gameObject.SetActive(true);
      ToggleEnable.onValueChanged.RemoveAllListeners();
      ToggleEnable.onValueChanged.AddListener((on) => {
        AddPackageStateChange(data.packageName, on, data.enableLoad);
      });
    }
    else {
      ToggleEnable.onValueChanged.RemoveAllListeners();
      ToggleEnable.gameObject.SetActive(false);
    }
  }

  //更新状态文字
  private void UpdateStatusText() {
    TextStatus.text = I18N.TrF("core.ui.PackageManagerStatus", "", packageItems.Count, gamePackageManager.loadedPackages.Count);
  }

  //选择模块查看信息
  private void SelectPackage(string name) {
    var pack = gamePackageManager.FindRegisteredPackage(name);
    if(pack != null) {
      Dictionary<string, string> options = new Dictionary<string, string>();
      options.Add("PackageName", name);
      gameUIManager.GoPageWithOptions("PagePackageManagerInfo", options);
    }
    else
      gameUIManager.GlobalAlertWindow(I18N.TrF("core.ui.PackageManagerPackNotFound", name), I18N.Tr("core.ui.Tip"), null);
  }

  private void AddPackageStateChange(string name, bool state, bool intitalState) {
    var pack = packageStateChanges.Find((v) => v.name == name);
    if(pack == null) {
      if(state != intitalState) {
        pack = new PackageStateChange();
        pack.name = name;
        pack.newState = state;
        packageStateChanges.Add(pack); 
      }
    } else {
      if(state == intitalState)
        packageStateChanges.Remove(pack);
      else {
        pack.name = name;
        pack.newState = state;
      }
    }
  }
  private void AddPackageToList(GamePackageManager.GamePackageRegisterInfo info) {
    if(info.package != null) {
      
      if(packageItems.Find((i) => i.packageName == info.packageName) != null)
        return;

      var package = info.package;
      PackageInfoItem newItem = new PackageInfoItem();
      newItem.packageName = info.packageName;
      newItem.logo = package.BaseInfo.LogoTexture != null ? package.BaseInfo.LogoTexture : GameStaticResourcesPool.FindStaticAssets<Sprite>("ModIconSuccess");
      newItem.name = package.BaseInfo.Name;
      newItem.introduction = package.BaseInfo.Introduction;
      newItem.notUnLoadable = package.IsNotUnLoadable();

      packageItems.Add(newItem);
    }
  }
  private void RemovePackageFromList(string name) {
    var item = packageItems.Find((i) => i.packageName == name);
    if(item != null)
      packageItems.Remove(item);
  }

  //加载列表
  private void LoadList() {
    //加载列表
    foreach(var pack in gamePackageManager.registeredPackages.Values) 
      AddPackageToList(pack);
    ScrollView.UpdateData(false);
    UpdateStatusText();

    //监听模块注册，移除事件
    GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_PACKAGE_REGISTERED, TAG, (evtName, param) => {
      var packageName = param[0] as string;
      if(gamePackageManager.registeredPackages.TryGetValue(packageName, out var pack)) {
        AddPackageToList(pack);
        UpdateStatusText();
        ScrollView.UpdateData(false);
      }
      return false;
    });
    GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_PACKAGE_UNREGISTERED, TAG, (evtName, param) => {
      RemovePackageFromList(param[0] as string);
      UpdateStatusText();
      ScrollView.UpdateData(false);
      return false;
    });
  }

  //保存和返回
  private void Save() {
    //保存状态至模块管理器中
    foreach(PackageStateChange change in packageStateChanges) {
      if(gamePackageManager.registeredPackages.ContainsKey(change.name)) 
        gamePackageManager.registeredPackages[change.name].enableLoad = change.newState;
    }
    packageStateChanges.Clear();
    //保存状态
    gamePackageManager.SavePackageRegisterInfo();
    //重启游戏
    GameManager.Instance.RestartGame();
  }
  private void Back() {
    if(packageStateChanges.Count > 0) {
      gameUIManager.GlobalConfirmWindow(I18N.Tr("core.ui.PackageManagerTip"), I18N.TrF("core.ui.PackageManagerChangeTip", packageStateChanges.Count), () => {
        packageStateChanges.Clear();
        gameUIManager.BackPreviusPage();
      }, () => {
      }, I18N.Tr("core.ui.PackageManagerGiveUpSave"), I18N.Tr("core.ui.Back"));
    } else {
      gameUIManager.BackPreviusPage();
    }
  }

}