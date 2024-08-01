using System.Collections.Generic;
using System.IO;
using Ballance2;
using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Services.I18N;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Controls;
using Ballance2.UI.Core.ValueBinder;
using TMPro;
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


namespace Ballance2.Menu
{
  public class PackageManagerUIControl : GameSingletonBehavior<PackageManagerUIControl> {
    
    private const string TAG = "PackageManagerUIControl";
    private const string PagePackageSettingUIPagePrefixName = "PackageManagerUIControl:";
    private const string PagePackageSettingUIPageDefaultContent = "PagePackageSettingUI.prefab";

    public ScrollView ScrollView;
    public Button ButtonSave;
    public Button ButtonCancel;
    public Button ButtonEnableAll;
    public Button ButtonDisableAll;
    public TMP_Text TextStatus;

    public GameObject PrefabButton;
    public GameObject PrefabKeyChoose;
    public GameObject PrefabToogle;
    public GameObject PrefabUpdown;
    public GameObject PrefabInput;
    public GameObject PrefabSlider;
    public GameObject PrefabSubTitle;
    public GameObject PrefabText;

    private GamePackageManager gamePackageManager;
    private GameUIManager gameUIManager;
    private GameUIPage page;
    private bool firstShow = true;

    private void Start() {
      gamePackageManager = GameSystem.GetSystemService<GamePackageManager>();
      gameUIManager = GameSystem.GetSystemService<GameUIManager>();

      ScrollView.SetItemCountFunc(() => packageItems.Count);
      ScrollView.SetUpdateFunc(ItemUpdateFn);
      ScrollView.defaultItemSize = new Vector2((transform as RectTransform).rect.width - 250, 80);

      ButtonEnableAll.onClick.AddListener(() => EnableAll());
      ButtonDisableAll.onClick.AddListener(() => DisableAll());
      ButtonCancel.onClick.AddListener(() => Back());
      ButtonSave.onClick.AddListener(() => Save());

      page = gameUIManager.FindPage("PagePackageManager");
      page.OnShow = (options) => {
        if(firstShow) {
          LoadList();
          firstShow = false;
        }
        if(options.ContainsKey("LocatePackage")) 
          SelectPackage(options["LocatePackage"]);
      };
    }

    private class PackageStateChange {
      public string name;
      public bool newState;
      public PackageStateChange(string name, bool newState) {
        this.name = name;
        this.newState = newState;
      }
    }
    private class PackageInfoItem {
      public string packageName;
      public bool enableLoad;
      public bool notUnLoadable;
      public bool systemPackage;
      public bool hasSettingsUI;
      public string name;
      public string introduction;
      public Sprite logo;
    }
    private Dictionary<string, PackageStateChange> packageStateChanges = new Dictionary<string, PackageStateChange>();
    private List<PackageInfoItem> packageItems = new List<PackageInfoItem>();

    //条目更新函数
    private void ItemUpdateFn(int index, RectTransform item) {
      
      var data = packageItems[index];

      var Button = item.gameObject.GetComponent<Button>();
      var ImageLogo = item.Find("ImageLogo").GetComponent<Image>();
      var TextTitle = item.Find("TextTitle").GetComponent<UIText>();
      var TextIntrod = item.Find("TextIntrod").GetComponent<UIText>();
      var TextNotUnloadableText = item.Find("TextNotUnloadableText").GetComponent<UIText>();
      var ToggleEnable = item.Find("ToggleEnable").GetComponent<ToggleEx>();
      var ButtonUnload = item.Find("ButtonUnload").GetComponent<Button>();
      var ButtonSettings = item.Find("ButtonSettings").GetComponent<Button>();

      Button.onClick.RemoveAllListeners();
      Button.onClick.AddListener(() => {
        Dictionary<string, string> options = new Dictionary<string, string>();
        options.Add("PackageName", data.packageName);
        gameUIManager.GoPageWithOptions("PagePackageManagerInfo", options);
      });

      TextTitle.text = data.name;
      TextIntrod.text = data.introduction;
      ImageLogo.sprite = data.logo;

      //跳转设置按扭
      if(data.hasSettingsUI) {
        ButtonSettings.gameObject.SetActive(true);
        ButtonSettings.onClick.RemoveAllListeners();
        ButtonSettings.onClick.AddListener(() => {
          gameUIManager.GoPage(PagePackageSettingUIPagePrefixName + data.packageName);
        });
      } else {
        ButtonSettings.gameObject.SetActive(false);
      }

      if(!data.systemPackage) {
        TextNotUnloadableText.gameObject.SetActive(false);

        //卸载按扭
        if(data.notUnLoadable)
          ButtonUnload.gameObject.SetActive(false);
        else {
          ButtonUnload.gameObject.SetActive(true);
          ButtonUnload.onClick.RemoveAllListeners();
          ButtonUnload.onClick.AddListener(() => {
            UnloadPackage(data.packageName);
          });
        }

        if(packageStateChanges.TryGetValue(name, out var pack))
          ToggleEnable.isOn = pack.newState;//缓存中的状态
        else
          ToggleEnable.isOn = data.enableLoad;

        ToggleEnable.gameObject.SetActive(true);
        ToggleEnable.onValueChanged.RemoveAllListeners();
        ToggleEnable.onValueChanged.AddListener((on) => {
          AddPackageStateChange(data.packageName, on, data.enableLoad);
        });
      }
      else {
        ButtonUnload.gameObject.SetActive(false);
        TextNotUnloadableText.gameObject.SetActive(true);
        TextNotUnloadableText.text = I18N.Tr("core.ui.PackageSystemPackage");
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
        gameUIManager.GoPageWithOptions("PagePackageSettings", options);
      }
    }

    private void AddPackageStateChange(string name, bool state, bool intitalState) {
      if(!packageStateChanges.TryGetValue(name, out var pack)) {
        if(state != intitalState) {
          pack = new PackageStateChange(name, state);
          packageStateChanges.Add(name, pack); 
        }
      } else {
        if(state == intitalState)
          packageStateChanges.Remove(name);
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
        newItem.notUnLoadable = package.IsNotUnLoadable() || package.IsSystemPackage();
        newItem.systemPackage = package.IsSystemPackage();
        newItem.hasSettingsUI = packageSettingsUIs.ContainsKey(info.packageName);
        newItem.enableLoad = info.enableLoad;

        packageItems.Add(newItem);
      }
    }
    private void RemovePackageFromList(string name) {
      var item = packageItems.Find((i) => i.packageName == name);
      if(item != null)
        packageItems.Remove(item);
    }

    private void UnloadPackage(string name) {
      gameUIManager.GlobalConfirmWindow(I18N.Tr("core.ui.PackageUnaloadTip")  + '\n' + I18N.TrF("core.ui.PackageWantUnaload", packageStateChanges.Count), () => {
        packageStateChanges.Clear();
        gameUIManager.BackPreviusPage();
      }, () => {
        if(gamePackageManager.registeredPackages.TryGetValue(name, out var pack)) {
          if(gamePackageManager.UnLoadPackage(name, true)) {
            if(File.Exists(pack.package.PackageFilePath))
              File.Delete(pack.package.PackageFilePath);
          }
        }
      });
    }
    private void EnableAll() {
      for (var i = 0; i < packageItems.Count; i++)
      {
        var item = packageItems[i];
        if(!item.systemPackage) {
          if(packageStateChanges.ContainsKey(item.name))
            packageStateChanges[item.name].newState = true;
          else
            packageStateChanges.Add(item.name, new PackageStateChange(item.name, true));
        }
      }
      ScrollView.UpdateData(false);
    }
    private void DisableAll() {
      for (var i = 0; i < packageItems.Count; i++)
      {
        var item = packageItems[i];
        if(!item.systemPackage) {
          if(packageStateChanges.ContainsKey(item.name))
            packageStateChanges[item.name].newState = false;
          else
            packageStateChanges.Add(item.name, new PackageStateChange(item.name, false));
        }
      }
      ScrollView.UpdateData(false);
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
          if(ScrollView.gameObject.activeSelf)
            ScrollView.UpdateData(false);
        }
        return false;
      });
      GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_PACKAGE_UNREGISTERED, TAG, (evtName, param) => {
        RemovePackageFromList(param[0] as string);
        UpdateStatusText();
        if(ScrollView.gameObject.activeSelf)
          ScrollView.UpdateData(false);
        return false;
      });
    }

    //保存和返回
    private void Save() { 
      //移除状态一致的条目
      List<string> sameNames = new List<string>();
      foreach(var pack in packageItems) {
        if(packageStateChanges.ContainsKey(pack.name) && packageStateChanges[pack.name].newState == pack.enableLoad) 
          sameNames.Add(pack.name);
      }
      foreach(var name in sameNames)
        packageStateChanges.Remove(name);
      sameNames.Clear();

      if(packageStateChanges.Count > 0) {
        //保存状态至模块管理器中
        foreach(var change in packageStateChanges) {
          if(gamePackageManager.registeredPackages.ContainsKey(change.Value.name)) 
            gamePackageManager.registeredPackages[change.Value.name].enableLoad = change.Value.newState;
        }
        packageStateChanges.Clear();
        //保存状态
        gamePackageManager.SavePackageRegisterInfo();
        //重启游戏
        GameManager.Instance.RestartGame();
      } else {
        gameUIManager.BackPreviusPage();
      }
    }
    private void Back() {
      if(packageStateChanges.Count > 0) {
        gameUIManager.GlobalConfirmWindow(I18N.Tr("core.ui.PackageManagerTip")  + '\n' + I18N.TrF("core.ui.PackageManagerChangeTip", packageStateChanges.Count), () => {
          packageStateChanges.Clear();
          gameUIManager.BackPreviusPage();
        }, () => {
        }, I18N.Tr("core.ui.PackageManagerGiveUpSave"), I18N.Tr("core.ui.Back"));
      } else {
        gameUIManager.BackPreviusPage();
      }
    }
  
    //模块设置UI控制器
    public class PackageSettingsUIControlInfo<T> {
      public PackageSettingsUIControlInfo(GameUIControlValueBinderSupplierCallback valueBinderSupplierCallback, T control) {
        ValueBinderSupplierCallback = valueBinderSupplierCallback;
        Control = control;
      }
      public GameUIControlValueBinderSupplierCallback ValueBinderSupplierCallback { private set; get; }
      public T Control { private set; get; }
    }
    public class PackageSettingsUI {
      private GamePackage package;
      private PackageManagerUIControl control;
      private GameUIMessageCenter MessageCenter;
      private RectTransform PageContent;
      private bool isCustomControl;

      /// <summary>
      /// 获取当前设置界面所属UI页
      /// </summary>
      /// <value></value>
      public GameUIPage Page { get; private set; }

      internal PackageSettingsUI(GamePackage package, PackageManagerUIControl control, string customPagePrefab, string customPageTemplatePrefab) {
        this.control = control;
        this.package = package;
        isCustomControl = customPagePrefab != null;
        MessageCenter = MenuManager.MessageCenter;
        Page = GameUIManager.Instance.RegisterPage(
          PagePackageSettingUIPagePrefixName + package.PackageName, 
          customPageTemplatePrefab == null ? "PageCommon" : customPageTemplatePrefab
        );
        if (customPagePrefab != null) 
          Page.CreateContent(package, customPagePrefab);
        else {
          Page.CreateContent(GamePackage.GetSystemPackage(), PagePackageSettingUIPageDefaultContent);
          PageContent = Page.Content.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
          Page.Content.Find("Title").GetComponent<TMP_Text>().text = I18N.TrF("core.ui.PackageManagerSettingUITitle", "{0}", package.BaseInfo.Name);
        }
      } 
      internal void Delete() {
        GameUIManager.Instance.UnRegisterPage(Page.PageName);
      } 

      private bool CheckIsCustomControl() {
        if (isCustomControl) {
          GameErrorChecker.SetLastErrorAndLog(GameError.NotImplemented, TAG, "PackageSettingsUI fail: can not use auto make ui when set customPagePrefab!");
          return true;
        }
        return false;
      }

      /// <summary>
      /// 向设置UI添加一个按扭条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">按扭文字</param>
      /// <param name="onClick">按扭点击回调</param>
      /// <returns></returns>
      public PackageSettingsUIControlInfo<Button> AddButton(string name, string title, GameManager.VoidDelegate onClick) {
        if (CheckIsCustomControl())
          return null;
        var item = GameManager.Instance.InstancePrefab(control.PrefabButton, PageContent, name);
        var cls = item.GetComponent<Button>();
        cls.onClick.AddListener(() => onClick());
        item.transform.SetAsFirstSibling();
        var text = item.transform.Find("Text").GetComponent<UIText>();
        if (text != null)
          text.text = title;
        return new PackageSettingsUIControlInfo<Button>(null, cls);
      }
      /// <summary>
      /// 向设置UI添加一个副标题条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">副标题文字</param>
      /// <returns></returns>
      public PackageSettingsUIControlInfo<UIText> AddSubTitle(string name, string title) {
        if (CheckIsCustomControl())
          return null;

        var item = GameManager.Instance.InstancePrefab(control.PrefabSubTitle, PageContent, name);
        var cls = item.GetComponent<UIText>();
        item.transform.SetAsFirstSibling();
        cls.text = title;
        return new PackageSettingsUIControlInfo<UIText>(null, cls);
      }
      /// <summary>
      /// 向设置UI添加一个文字条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">文字内容</param>
      /// <returns></returns>
      public PackageSettingsUIControlInfo<UIText> AddText(string name, string title) {
        if (CheckIsCustomControl())
          return null;

        var item = GameManager.Instance.InstancePrefab(control.PrefabText, PageContent, name);
        var cls = item.GetComponent<UIText>();
        cls.text = title;
        item.transform.SetAsFirstSibling();
        return new PackageSettingsUIControlInfo<UIText>(null, cls);
      }
      /// <summary>
      /// 向设置UI添加一个按键选择器条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">标题文字</param>
      /// <param name="valueUpdateCallback">参数更改回调</param>
      /// <returns></returns>
      public KeyChoose AddKeyChoose(string name, string title) {
        if (CheckIsCustomControl())
          return null;

        var item = GameManager.Instance.InstancePrefab(control.PrefabKeyChoose, PageContent, name);
        var cls = item.GetComponent<KeyChoose>();
        cls.Text.text = title;
        item.transform.SetAsFirstSibling();
        return cls;
      }
      /// <summary>
      /// 向设置UI添加一个选择器条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">标题文字</param>
      /// <param name="valueUpdateCallback">参数更改回调</param>
      /// <returns></returns>
      public PackageSettingsUIControlInfo<Updown> AddUpdown(string name, string title, GameUIControlValueBinderUserUpdateCallback valueUpdateCallback) {
        if (CheckIsCustomControl())
          return null;

        var item = GameManager.Instance.InstancePrefab(control.PrefabUpdown, PageContent, name);
        var cls = item.GetComponent<Updown>();
        var valueBinder = item.AddComponent<UpdownValueBinder>();
        item.transform.SetAsFirstSibling();
        valueBinder.MessageCenter = MessageCenter;
        valueBinder.Name = name; 
        cls.TextTitle.text = title;
        return new PackageSettingsUIControlInfo<Updown>(MessageCenter.SubscribeValueBinder(valueBinder, valueUpdateCallback), cls);
      }
      /// <summary>
      /// 向设置UI添加一个开关条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">标题文字</param>
      /// <param name="valueUpdateCallback">参数更改回调</param>
      /// <returns></returns>
      public PackageSettingsUIControlInfo<ToggleEx> AddToggle(string name, string title, GameUIControlValueBinderUserUpdateCallback valueUpdateCallback) {
        if (CheckIsCustomControl())
          return null;

        var item = GameManager.Instance.InstancePrefab(control.PrefabToogle, PageContent, name);
        var cls = item.GetComponent<ToggleEx>();
        var valueBinder = item.AddComponent<ToggleExValueBinder>();
        item.transform.SetAsFirstSibling();
        valueBinder.MessageCenter = MessageCenter;
        valueBinder.Name = name; 
        cls.Text.text = title;
        return new PackageSettingsUIControlInfo<ToggleEx>(MessageCenter.SubscribeValueBinder(valueBinder, valueUpdateCallback), cls);
      }
      /// <summary>
      /// 向设置UI添加一个输入框条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">标题文字</param>
      /// <param name="valueUpdateCallback">参数更改回调</param>
      /// <returns></returns>
      public PackageSettingsUIControlInfo<TMP_InputField> AddInput(string name, GameUIControlValueBinderUserUpdateCallback valueUpdateCallback) {
        if (CheckIsCustomControl())
          return null;

        var item = GameManager.Instance.InstancePrefab(control.PrefabInput, PageContent, name);
        var cls = item.GetComponent<TMP_InputField>();
        var valueBinder = item.AddComponent<InputFieldValueBinder>();
        item.transform.SetAsFirstSibling();
        valueBinder.MessageCenter = MessageCenter;
        valueBinder.Name = name; 
        return new PackageSettingsUIControlInfo<TMP_InputField>(MessageCenter.SubscribeValueBinder(valueBinder, valueUpdateCallback), cls);
      }
      /// <summary>
      /// 向设置UI添加一个滑块条目
      /// </summary>
      /// <param name="name">条目名称</param>
      /// <param name="title">标题文字</param>
      /// <param name="valueUpdateCallback">参数更改回调</param>
      /// <returns></returns>
      public PackageSettingsUIControlInfo<Slider> AddSlider(string name, GameUIControlValueBinderUserUpdateCallback valueUpdateCallback) {
        if (CheckIsCustomControl())
          return null;

        var item = GameManager.Instance.InstancePrefab(control.PrefabInput, PageContent, name);
        var cls = item.GetComponent<Slider>();
        var valueBinder = item.AddComponent<SliderValueBinder>();
        item.transform.SetAsFirstSibling();
        valueBinder.MessageCenter = MessageCenter;
        valueBinder.Name = name; 
        return new PackageSettingsUIControlInfo<Slider>(MessageCenter.SubscribeValueBinder(valueBinder, valueUpdateCallback), cls);
      }
    }
    private Dictionary<string, PackageSettingsUI> packageSettingsUIs = new Dictionary<string, PackageSettingsUI>();

    /// <summary>
    /// 注册模块的设置界面
    /// </summary>
    /// <param name="package">模块</param>
    /// <param name="customPagePrefab">自定义页面内容预制体名称，如果不为空，则会在当前模组包中寻找对应页面预制体。使用这个选项后无法再使用 PackageSettingsUI 的相关自动生成控件方法。</param>
    /// <param name="customPageTemplatePrefab">自定义页面模板，如果为空，泽使用默认的 PageCommon 模板</param>
    /// <returns>返回设置页面控制器，你可以通过控制器自定义模块的设置页面</returns>
    public PackageSettingsUI RegisterPackageSettingsUI(GamePackage package, string customPagePrefab = null, string customPageTemplatePrefab = null) {
      if (packageSettingsUIs.ContainsKey(package.PackageName)) 
        return packageSettingsUIs[package.PackageName];
      var ui = new PackageSettingsUI(package, this, customPagePrefab, customPageTemplatePrefab);
      packageSettingsUIs.Add(package.PackageName, ui);
      var packageInfo = packageItems.Find((k) => k.packageName == package.PackageName);
      if (packageInfo != null)
        packageInfo.hasSettingsUI = true;
      return ui;
    }
    /// <summary>
    /// 删除模块的设置界面
    /// </summary>
    /// <param name="package">模块</param>
    public bool DeletePackageSettingsUI(GamePackage package) {
      if (!packageSettingsUIs.ContainsKey(package.PackageName)) {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, "DeletePackageSettingsUI " + package.PackageName + "  fail: not register");
        return false;
      }
      var ui = packageSettingsUIs[package.PackageName];
      ui.Delete();
      packageSettingsUIs.Remove(package.PackageName);
      return true;
    }
  }
}