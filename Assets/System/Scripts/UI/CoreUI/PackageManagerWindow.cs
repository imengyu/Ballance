using System.Collections;
using System.Collections.Generic;
using Ballance2;
using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Services.Pool;
using Ballance2.UI.Core.Controls;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* PackageManagerWindow.cs
* 
* 用途：
* 模块管理器窗口的对话框逻辑。
*
* 作者：
* mengyu
*/

public class PackageManagerWindow : MonoBehaviour
{
  private const string TAG = "PackageManagerWindow";

  public PackageManagerContentWindow PackageManagerContentWindow;
  public RectTransform PackageManageListContent;
  public GameObject ItemPrefab;
  public Button ButtonSave;
  public Button ButtonBack;

  private GamePackageManager gamePackageManager;
  private GameUIManager gameUIManager;
  private GameObjectPool itemPrefabPool;

  void Start()
  {
    gamePackageManager = GameSystem.GetSystemService("GamePackageManager") as GamePackageManager;
    gameUIManager = GameSystem.GetSystemService("GameUIManager") as GameUIManager;
    itemPrefabPool = new GameObjectPool("itemPrefabPool", ItemPrefab, 8, 256, PackageManageListContent);
    ButtonSave.onClick.AddListener(Save);
    ButtonBack.onClick.AddListener(Back);
    PackageManagerContentWindow.OnGoPackage = SelectPackage;
    LoadList();
  }
  private void OnDestroy() {
    itemPrefabPool.Destroy();
  }

  private class PackageStateChange {
    public string name;
    public bool newState;
  }
  private List<PackageStateChange> packageStateChanges = new List<PackageStateChange>();

  private void SelectPackage(string name) {
    var pack = gamePackageManager.FindRegisteredPackage(name);
    if(pack != null)
      PackageManagerContentWindow.SetPackageInfo(pack);
    else
      gameUIManager.GlobalAlertWindow("没有找到模组 " + name + " ，它似乎没有在这里安装。", "提示", null);
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
      
      if(itemPrefabPool.ContainsObjectInParent(info.packageName))
        return;

      var package = info.package;
      var go = itemPrefabPool.NextAvailableObject();
      var Button = go.GetComponent<Button>();
      var ImageLogo = go.transform.Find("ImageLogo").GetComponent<Image>();
      var TextTitle = go.transform.Find("TextTitle").GetComponent<Text>();
      var TextIntrod = go.transform.Find("TextIntrod").GetComponent<Text>();
      var ToggleEnable = go.transform.Find("ToggleEnable").GetComponent<ToggleEx>();

      if(package.BaseInfo != null) {
        if(package.BaseInfo.LogoTexture != null)
          ImageLogo.sprite = package.BaseInfo.LogoTexture;
        else
          ImageLogo.sprite = GameStaticResourcesPool.FindStaticAssets<Sprite>("ModIconSuccess");
        TextTitle.text = package.BaseInfo.Name;
        TextIntrod.text = package.BaseInfo.Introduction;
      }

      Button.onClick.RemoveAllListeners();
      Button.onClick.AddListener(() => {
        PackageManagerContentWindow.SetPackageInfo(info.package);
      });

      if(package.IsNotUnLoadable()) {
        ToggleEnable.isOn = info.enableLoad;
        ToggleEnable.gameObject.SetActive(true);
        ToggleEnable.onValueChanged.RemoveAllListeners();
        ToggleEnable.onValueChanged.AddListener((on) => {
          AddPackageStateChange(info.packageName, on, info.enableLoad);
        });
      }
      else {
        ToggleEnable.onValueChanged.RemoveAllListeners();
        ToggleEnable.gameObject.SetActive(false);
      }
      go.name = info.packageName;
    }
  }
  private void RemovePackageFromList(string name) {
    itemPrefabPool.ReturnObjectFromParent(name);
  }

  private void LoadList() {
    //加载列表
    foreach(var pack in gamePackageManager.registeredPackages.Values) {
      AddPackageToList(pack);
      if(pack.packageName == GamePackageManager.CORE_PACKAGE_NAME)
        PackageManagerContentWindow.SetPackageInfo(pack.package);
    }

    //监听模块注册，移除事件
    GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_PACKAGE_REGISTERED, TAG, (evtName, param) => {
      var packageName = param[0] as string;
      if(gamePackageManager.registeredPackages.TryGetValue(packageName, out var pack))
        AddPackageToList(pack);
      return false;
    });
    GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_PACKAGE_UNREGISTERED, TAG, (evtName, param) => {
      RemovePackageFromList(param[0] as string);
      return false;
    });
  }

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
      gameUIManager.GlobalConfirmWindow(I18N.Tr("packageManager.Tip"), I18N.TrF("packageManager.ChangeTip", packageStateChanges.Count), () => {
        packageStateChanges.Clear();
        gameUIManager.BackPreviusPage();
      }, () => {
      }, I18N.Tr("packageManager.GiveUpSave"), I18N.Tr("packageManager.Back"));
    } else {
      gameUIManager.BackPreviusPage();
    }
  }
}
