using System.Collections;
using System.Text;
using Ballance2.Sys;
using Ballance2.Sys.Bridge;
using Ballance2.Sys.Package;
using Ballance2.Sys.Services;
using Ballance2.Sys.UI.Utils;
using Ballance2.Sys.Utils;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePackageManageWindow.cs
* 
* 用途：
* 模块管理器窗口主逻辑
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-16 创建
*
*/

public class GamePackageManageWindow : MonoBehaviour {
  
  private const string TAG = "GamePackageManageWindow";

  public Toggle CheckBoxFilterRegistered;
  public Toggle CheckBoxFilterLoaded;
  public Toggle CheckBoxFilterNotCap;
  public Toggle CheckBoxFilterLoadFailed;
  public RectTransform PackageManageListView;

  public Text TextPackageName;
  public Text TextPackageVersion;
  public Text TextPackageMoreInfos;
  public Text LinkVisitSite;
  public Text LinkVisitDoc;
  public Text LinkVisiAuthor;
  public Image ImagePackageLogo;

  public Text TextExplain;
  public Text TextPackageDependencies;

  public InputField InputFieldName;
  public InputField InputFieldVersion;
  public InputField InputFieldAuthor;
  public InputField InputFieldUpdateTime;

  public Sprite SpriteNotLoad;
  public Sprite SpriteLoadSucees;
  public Sprite SpriteLoadFaild;

  public GameObject PackageManageItem;

  public GamePackageManager GamePackageManager;
  public GameUIManager GameUIManager;

  private int confirmCurrentUnLoad = 0;
  private string currentUnLoadPackageName = "";
  private int confirmCurrentLoad = 0;
  private string currentLoadPackageName = "";

  private void Start() { 
    var GameMediator = GameManager.GameMediator;
    var SystemPackage = GamePackage.GetSystemPackage();
    GameUIManager = GameManager.Instance.GetSystemService<GameUIManager>();
    GamePackageManager = GameManager.Instance.GetSystemService<GamePackageManager>();

    GameMediator.RegisterEventHandler(SystemPackage, GameEventNames.EVENT_PACKAGE_LOAD_FAILED, TAG, (evtName, pararms) => {
      ChangePackageStateInList(pararms[0] as string);
      return false;
    });
    GameMediator.RegisterEventHandler(SystemPackage, GameEventNames.EVENT_PACKAGE_LOAD_SUCCESS, TAG, (evtName, pararms) => {
      ChangePackageStateInList(pararms[0] as string);
      return false;
    });
    GameMediator.RegisterEventHandler(SystemPackage, GameEventNames.EVENT_PACKAGE_REGISTERED, TAG, (evtName, pararms) => {
      AddPackageToList(pararms[0] as string);
      return false;
    });
    GameMediator.RegisterEventHandler(SystemPackage, GameEventNames.EVENT_PACKAGE_UNREGISTERED, TAG, (evtName, pararms) => {
      RemovePackageFromList(pararms[0] as string);
      return false;
    });
    GameMediator.RegisterEventHandler(SystemPackage, GameEventNames.EVENT_PACKAGE_UNLOAD, TAG, (evtName, pararms) => {
      ChangePackageStateInList(pararms[0] as string);
      return false;
    });

    GameMediator.RegisterEventHandler(SystemPackage, GameEventNames.EVENT_GLOBAL_ALERT_CLOSE, TAG, (evtName, pararms) => {
      int id = (int)pararms[0];
      bool confirm = (bool)pararms[1];
      if(confirm) {
        if(id == confirmCurrentUnLoad) {
          if(GamePackageManager.UnLoadPackage(currentUnLoadPackageName, true))
            GameUIManager.GlobalToast("卸载完成");
          return true;
        }
        if(id == confirmCurrentLoad) {
          StartCoroutine(LoadPackageManualCon());
          return true;
        }
      }
      return false;
    });
  
    EventTriggerListener.Get(LinkVisitSite.gameObject).onClick = (go) => {
      if(currentShowPackage != null)
        Application.OpenURL(currentShowPackage.BaseInfo.Link);
    };
    EventTriggerListener.Get(LinkVisitDoc.gameObject).onClick = (go) => {
      if(currentShowPackage != null)
        Application.OpenURL(currentShowPackage.BaseInfo.DocLink);
    };
    EventTriggerListener.Get(LinkVisiAuthor.gameObject).onClick = (go) => {
      if(currentShowPackage != null)
        Application.OpenURL(currentShowPackage.BaseInfo.AuthorLink);
    };
  
    if(GamePackageManager.registeredPackages.Count > 0) 
      foreach(var v in GamePackageManager.registeredPackages.Keys) 
        AddPackageToList(v);
  }
  private void OnDestroy() {}

  private bool showLoaded = true;
  private bool showNotCap = true;
  private bool showRegistered = true;
  private bool showLoadFailed = true;

  public void OnCheckLoadedCanged(bool b) {
    showLoaded = b;
    FlushListVisible();
  }
  public void OnCheckNotCapCanged(bool b) {
    showNotCap = b;
    FlushListVisible();
  }
  public void OnCheckRegisteredCanged(bool b) {
    showRegistered = b;
    FlushListVisible();
  }
  public void OnCheckLoadFailedCanged(bool b) {
    showLoadFailed = b;
    FlushListVisible();
  }

  private void FlushListVisible() {
    for(int i = 0; i < PackageManageListView.childCount; i++) {
      var child = PackageManageListView.GetChild(i);
      var pack = GamePackageManager.FindRegisteredPackage(child.gameObject.name);
      if(pack != null) FlushListItemVisible(child.gameObject, pack);
    }
  }
  private void FlushListItemVisible(GameObject go, GamePackage pack) {
    switch(pack._Status) {
      case GamePackageStatus.Loading:
      case GamePackageStatus.NotLoad: go.SetActive(showRegistered); break;
      case GamePackageStatus.LoadFailed: 
        if(pack.IsCompatible)
          go.SetActive(showLoadFailed); 
        else
          go.SetActive(showNotCap); 
        break;
      case GamePackageStatus.UnloadWaiting: 
      case GamePackageStatus.LoadSuccess: go.SetActive(showLoaded); break;
    }
  }
  
  private RectTransform FindPackageFromList(string packageName) {
    for(int i = 0; i < PackageManageListView.childCount; i++) {
      var child = PackageManageListView.GetChild(i);
      if(child.gameObject.name == packageName) 
        return child as RectTransform;
    }
    return null;
  }
  private GamePackage AddPackageToList(string packageName) {
    var Package = GamePackageManager.FindRegisteredPackage(packageName);
    if(Package != null) {
      var go = CloneUtils.CloneNewObjectWithParent(PackageManageItem, PackageManageListView, packageName);
      EventTriggerListener.Get(go).onClick = (go) => LoadPackageInfoToView(go.name);

      var ImageLogo = go.transform.Find("ImageLogo").GetComponent<Image>();
      var TextTitle = go.transform.Find("TextTitle").GetComponent<Text>();
      var TextPackageName = go.transform.Find("TextPackageName").GetComponent<Text>();
      var TextLoadStatus = go.transform.Find("TextLoadStatus").GetComponent<Text>();
      var CheckBoxEnableLoad = go.transform.Find("CheckBoxEnableLoad").GetComponent<Toggle>();
      var ButtonUnLoad = go.transform.Find("ButtonUnLoad").GetComponent<Button>();
      var ButtonLoad = go.transform.Find("ButtonLoad").GetComponent<Button>();
      var ImageStatus = go.transform.Find("ImageStatus").GetComponent<Image>();

      ImageLogo.sprite = Package.BaseInfo.LogoTexture;
      TextTitle.text = Package.BaseInfo.Name;
      TextPackageName.text = Package.PackageName + "(" + Package.Type + ")";
      ButtonUnLoad.onClick.AddListener(() => {
        currentUnLoadPackageName = packageName;
        confirmCurrentUnLoad = GameUIManager.GlobalConfirmWindow("确认卸载模块 " + packageName + " 吗？\n" +
          "卸载模块会立即将模块所有资源从内存中移除，这可能会影响到当前正在运行的任务。\n不建议卸载模块，此功能仅供调试使用。", "提示");
      });
      ButtonLoad.onClick.AddListener(() => {
        currentLoadPackageName = packageName;
        confirmCurrentLoad = GameUIManager.GlobalConfirmWindow("确认加载模块 " + packageName + " 吗？\n" +
          "不建议加载模块，因为模块在当前状态加载可能无法正常初始化，此功能仅供调试使用。\n推荐启用模块启动加载由游戏系统自动加载该模块。", "提示");
      });

      if(!Package.IsCompatible) {
        CheckBoxEnableLoad.interactable = false;
        CheckBoxEnableLoad.isOn = false;
        ButtonLoad.gameObject.SetActive(false);
        TextLoadStatus.text = "模块与当前版本游戏不兼容";
        ImageStatus.sprite = SpriteLoadFaild;
      } else {
        TextLoadStatus.text = "";
        CheckBoxEnableLoad.isOn = GamePackageManager.IsPackageEnableLoad(packageName);
        CheckBoxEnableLoad.onValueChanged.AddListener((v) => GamePackageManager.SetPackageEnableLoad(packageName, v));
      }
    
      switch(Package._Status) {
        case GamePackageStatus.Loading:
        case GamePackageStatus.NotLoad: 
          ButtonLoad.gameObject.SetActive(true);
          ButtonUnLoad.gameObject.SetActive(false);
          ImageStatus.sprite = SpriteNotLoad;
          break;
        case GamePackageStatus.LoadFailed: 
          ButtonLoad.gameObject.SetActive(true);
          ButtonUnLoad.gameObject.SetActive(false);
          ImageStatus.sprite = SpriteLoadFaild;
          TextLoadStatus.text = "<color=#f11>加载失败</color>：" + Package.LoadError;
          break;
        case GamePackageStatus.UnloadWaiting: 
        case GamePackageStatus.LoadSuccess:
          ButtonLoad.gameObject.SetActive(false);
          ButtonUnLoad.gameObject.SetActive(true);
          ImageStatus.sprite = SpriteLoadSucees;
          TextLoadStatus.text = "加载成功";
          break;
      }

      if(packageName == GamePackageManager.SYSTEM_PACKAGE_NAME || Package.SystemPackage)
        ButtonUnLoad.gameObject.SetActive(false);

      FlushListItemVisible(go, Package);
    }
    return Package;
  }
  private void RemovePackageFromList(string packageName) {
    var old = FindPackageFromList(packageName);
    if(old) Destroy(old.gameObject);
  }
  private void ChangePackageStateInList(string packageName) {
    var Package = GamePackageManager.FindRegisteredPackage(packageName); 
    var old = FindPackageFromList(packageName);
    if(old != null && Package != null) {
      var go = old.gameObject;
      var TextLoadStatus = go.transform.Find("TextLoadStatus").GetComponent<Text>();
      var CheckBoxEnableLoad = go.transform.Find("CheckBoxEnableLoad").GetComponent<Toggle>();
      var ImageStatus = go.transform.Find("ImageStatus").GetComponent<Image>();
      var ButtonUnLoad = go.transform.Find("ButtonUnLoad").GetComponent<Button>();
      var ButtonLoad = go.transform.Find("ButtonLoad").GetComponent<Button>();

      switch(Package._Status) {
        case GamePackageStatus.Loading:
        case GamePackageStatus.NotLoad: 
          ButtonLoad.gameObject.SetActive(true);
          ButtonUnLoad.gameObject.SetActive(false);
          ImageStatus.sprite = SpriteNotLoad;
          break;
        case GamePackageStatus.LoadFailed: 
          ButtonLoad.gameObject.SetActive(true);
          ButtonUnLoad.gameObject.SetActive(false);
          ImageStatus.sprite = SpriteLoadFaild;
          TextLoadStatus.text = "<color=#f11>加载失败</color>：" + Package.LoadError;
          break;
        case GamePackageStatus.UnloadWaiting: 
        case GamePackageStatus.LoadSuccess:
          ButtonLoad.gameObject.SetActive(false);
          ButtonUnLoad.gameObject.SetActive(true);
          ImageStatus.sprite = SpriteLoadSucees;
          TextLoadStatus.text = "加载成功";
          break;
      }
    }

  }

  private GamePackage currentShowPackage = null;

  private IEnumerator LoadPackageManualCon() {
    System.Threading.Tasks.Task<bool> task = GamePackageManager.LoadPackage(currentLoadPackageName);
    yield return task;

    if(task.Result) 
      GameUIManager.GlobalToast("模块 " + currentLoadPackageName + " 加载成功");
    else 
      GameUIManager.GlobalAlertWindow("模块 " + currentLoadPackageName + " 加载失败，错误详情请查看日志输出。", "加载提示");
  }
  private void LoadPackageInfoToView(string packageName) {
    var Package = GamePackageManager.FindRegisteredPackage(packageName);
    if(Package != null) {
      currentShowPackage = Package;
      TextPackageName.text = Package.BaseInfo.Name;
      TextPackageVersion.text = Package.BaseInfo.VersionName;
      TextPackageMoreInfos.text = Package.BaseInfo.Introduction;
      ImagePackageLogo.sprite = Package.BaseInfo.LogoTexture;
      InputFieldName.text = Package.PackageName;
      InputFieldVersion.text = Package.BaseInfo.VersionName + " (" + Package.PackageVersion + ")";
      InputFieldAuthor.text = Package.BaseInfo.Author;
      InputFieldUpdateTime.text = Package.UpdateTime.ToShortDateString();
      TextExplain.text = Package.BaseInfo.Description;
      LinkVisitSite.gameObject.SetActive(StringUtils.IsUrl(Package.BaseInfo.Link));
      LinkVisitDoc.gameObject.SetActive(StringUtils.IsUrl(Package.BaseInfo.DocLink));
      LinkVisiAuthor.gameObject.SetActive(StringUtils.IsUrl(Package.BaseInfo.AuthorLink));

      StringBuilder stringBuilder = new StringBuilder();
      foreach(GamePackageDependencies dependencies in Package.BaseInfo.Dependencies) {
        stringBuilder.Append(dependencies.Name);
        for(int i = 0, v = 50 - dependencies.Name.Length; i < v; i++)
          stringBuilder.Append(' ');
        stringBuilder.Append(dependencies.MinVersion);
      }
      TextPackageDependencies.text = stringBuilder.ToString();
    }
  }
}