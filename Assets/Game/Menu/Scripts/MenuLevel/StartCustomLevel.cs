using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Ballance2;
using Ballance2.Config.Settings;
using Ballance2.Game.GamePlay;
using Ballance2.Game.LevelBuilder;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Services.InputManager;
using Ballance2.Services.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/**
  自定义关卡管理菜单的控制脚本
*/
namespace Ballance2.Menu
{
  public class StartCustomLevel : MonoBehaviour
  {
    public RectTransform PanelListContentView;
    public GameObject PanelListPrefab;
    public RectTransform PanelContent;
    public RectTransform PanelNoContent;
    public RectTransform PanelError;
    public RectTransform PanelDepends;
    public RectTransform PanelNoLevel;
    public Button ButtonBack;
    public Button ButtonStart;
    public Button ButtonRefresh;
    public Button ButtonPreview;
    public Button ButtonEdit;
    public TMP_Text TextErrorContent;
    public Image ImageLogo;
    public TMP_Text TextName;
    public TMP_Text TextAuthor;
    public TMP_Text TextVersion;
    public TMP_Text TextUrl;
    public TMP_Text TextIntroduction;
    public TMP_Text TextDepends;
    public Image ImageDepends;
    public Image ImageBigImage;
    public Sprite NoPreviewImage;
    public Sprite IconSuccess;
    public Sprite IconError;

    public TMP_Text TextPanelDependsTitle;
    public GameObject PanelDependsListPrefab;
    public RectTransform PanelDependsListContentView;
    public Button ButtonDependsBack;
    public Button ButtonEnableAllDepends;

    private GameUIManager gameUIManager;
    private GameSoundManager gameSoundManager;
    private GamePackageManager gamePackageManager;

    private GameObjectPool itemPrefabPool;
    private GameObjectPool itemDependsPrefabPool;

    private static List<GameLevelInfo> loadedLevels = new List<GameLevelInfo>();
    private GameLevelInfo selectedItem = null;
    private bool dependesChanged = true;

    private class GameLevelInfo {
      public string name;
      public string error;
      public bool infoLoaded = false;
      public string author;
      public string version;
      public string introduction;
      public string url;
      public bool allowPreview = false;
      public bool allowEdit = false;
      public List<GameLevelDependencies> requiredPackages = new List<GameLevelDependencies>();
      public bool dependsSuccess;
      public string dependsStatus;
      public Sprite logo;
      public Sprite image;

      private GamePackageManager pm = null;

      public GameLevelInfo(string name, GamePackageManager pm) {
        this.name = name;
        this.pm = pm;
      }

      public void Set(GameLevelInfoJSON o) {
        name = o.name;
        author = o.author;
        version = o.version;
        introduction = o.introduction;
        url = o.url;
        allowPreview = o.allowPreview;
        allowEdit = o.allowPreview;
        requiredPackages = new List<GameLevelDependencies>(o.requiredPackages);

        if(requiredPackages.Count == 0) {
          //没有依赖
          dependsSuccess = true;
          dependsStatus = I18N.Tr("core.ui.LevelNoDepends");
        } else {
          //加载当前关卡的依赖状态
          int successCount = 0;
          for(var i = 0; i < requiredPackages.Count; i++) {
            var p = requiredPackages[i];
            var package = pm.FindRegisteredPackage(p.name);
            if(package == null)
              p.loaded = "not";
            else {
              if(package.PackageVersion < p.minVersion)
                p.loaded = "vermis";
              else {
                successCount ++;
                p.loaded = "true";
              }
            }
          }
          //设置状态文字
          if(successCount == requiredPackages.Count) {
            dependsSuccess = true;
            dependsStatus = I18N.TrF("core.ui.LevelHasDepends", "", successCount);
          } else {
            dependsSuccess = false;
            dependsStatus = I18N.TrF("core.ui.LevelHasBadDepends", "", requiredPackages.Count , requiredPackages.Count - successCount);
          }
        }
      }
    }
    [Serializable]
    private class GameLevelInfoJSON {
      public string name;
      public string author;
      public string version;
      public string introduction;
      public string url;
      public bool allowPreview = false;
      public bool allowEdit= false;
      public GameLevelDependencies[] requiredPackages;
    }

    private void Start() {
      gameUIManager = GameSystem.GetSystemService<GameUIManager>();
      gameSoundManager = GameSystem.GetSystemService<GameSoundManager>();
      gamePackageManager = GameSystem.GetSystemService<GamePackageManager>();

      itemPrefabPool = new GameObjectPool("itemPrefabPool", PanelListPrefab, 32, 256, PanelListContentView);
      itemDependsPrefabPool = new GameObjectPool("itemDependsPrefabPool", PanelDependsListPrefab, 32, 128, PanelDependsListContentView);

      PanelContent.gameObject.SetActive(false);
      PanelNoContent.gameObject.SetActive(true);
      PanelError.gameObject.SetActive(false);
      PanelDepends.gameObject.SetActive(false);
      PanelNoLevel.gameObject.SetActive(false);
      ButtonStart.onClick.AddListener(StartLevel);
      ButtonBack.onClick.AddListener(Back);
      ButtonDependsBack.onClick.AddListener(HideDependsInfo);
      ButtonEnableAllDepends.onClick.AddListener(EnableAllDepends);
      ButtonRefresh.onClick.AddListener(() => LoadLevelList(true));
      ButtonPreview.onClick.AddListener(() => PreviewLevel());
      ButtonEdit.onClick.AddListener(() => EditLevel());
      EventTriggerListener.Get(TextDepends.gameObject).onClick = (go) => ShowDependsInfo();

      GameManager.GameMediator.SubscribeSingleEvent(GamePackage.GetSystemPackage(), "PageStartCustomLevelLoad", "A", (evtName, param) => {
        LoadLevelList(false);
        return false;
      });
    }
    private void OnDestroy() {
      loadedLevels.Clear();
      itemPrefabPool.Destroy();
    }

    private void Back() {
      gameUIManager.BackPreviusPage();
    }
    private void StartLevel() {
      if(selectedItem != null) {
        gameSoundManager.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
        gameUIManager.MaskBlackFadeIn(1);
        GameTimer.Delay(1, () => {
          Log.D("StartCustomLevel", "StartLevel " + selectedItem.name);
          GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", new object[]{ selectedItem.name });
        });
      }
    }  
    private void PreviewLevel() {
      if(selectedItem != null) {
        gameSoundManager.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
        gameUIManager.MaskBlackFadeIn(1);
        GameTimer.Delay(1, () => {
          Log.D("StartCustomLevel", "PreviewLevel " + selectedItem.name);
          GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", new object[]{ selectedItem.name, true });
        });
      }
    }
    private void EditLevel() {
      if(selectedItem != null) {
        //TODO:
      }
    }
    private void ShowDependsInfo() {
      //显示关卡依赖信息
      PanelDepends.gameObject.SetActive(true);
      if(dependesChanged) {
        dependesChanged = false;
        itemDependsPrefabPool.Clear();

        TextPanelDependsTitle.text = I18N.TrF("core.ui.LevelAllDependsTile", "", selectedItem.name, selectedItem.requiredPackages.Count);
        selectedItem.requiredPackages.ForEach((p) => {
          var go = itemDependsPrefabPool.NextAvailableObject();
          var text = go.transform.Find("Text").GetComponent<TMP_Text>();
          var button = go.transform.Find("Button").GetComponent<Button>();
          if(p.loaded == "true")
            text.text = "";
          else if(p.loaded == "vermis")
            text.text = "<color=#f00>" + p.name + " >= " + p.minVersion + "</color> (" + I18N.Tr("core.ui.VersionMismatch") + ")";
          else
            text.text = "<color=#f00>" + p.name + " >= " + p.minVersion + "</color>";

          button.onClick.RemoveAllListeners();
          button.onClick.AddListener(() => {
            gameUIManager.GoPageWithOptions("PackageManageWindow", new Dictionary<string, string>
            {
              { "LocatePackage", p.name }
            });
          });
        });
      }
    }   
    private void EnableAllDepends() {
      //启用所有依赖
      gameUIManager.GlobalToast(I18N.Tr("core.ui.LevelEnableAllDependsTodo"));
    }
    private void HideDependsInfo() {
      PanelDepends.gameObject.SetActive(false);
    } 

    private void AddLevelToList(GameLevelInfo info) {
      if(itemPrefabPool.ContainsObjectInParent(info.name))
        return;

      var go = itemPrefabPool.NextAvailableObject();
      var Button = go.GetComponent<Button>();
      var Text = go.transform.Find("Text").GetComponent<TMP_Text>();

      Text.text = info.name;
      Button.onClick.RemoveAllListeners();
      Button.onClick.AddListener(() => StartCoroutine(SelectLevel(info)));

      go.name = info.name;
    }
    private IEnumerator SelectLevel(GameLevelInfo info) {
      if(selectedItem != info) {
        selectedItem = info;
        dependesChanged = true;

        //加载信息
        if(!selectedItem.infoLoaded) {
          selectedItem.infoLoaded = true;

          TextErrorContent.text = I18N.Tr("core.ui.Loading");
          PanelError.gameObject.SetActive(true);
          PanelContent.gameObject.SetActive(false);
          PanelNoContent.gameObject.SetActive(false);

          string jsonString = "";
          #if UNITY_EDITOR
          string realPackagePath = GamePathManager.DEBUG_LEVEL_FOLDER + "/" + info.name;
          //在编辑器中加载
          if (DebugSettings.Instance.PackageLoadWay == LoadResWay.InUnityEditorProject && Directory.Exists(realPackagePath)) {
            jsonString = File.ReadAllText(realPackagePath + "/Level.json");
            info.Set(JsonUtility.FromJson<GameLevelInfoJSON>(jsonString));

            var logo = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(realPackagePath + "/LevelLogo.png");
            if(logo != null) {
              info.logo = Sprite.Create(logo,
                new Rect(Vector2.zero, new Vector2(logo.width, logo.height)),
                new Vector2(0.5f, 0.5f));
            }
            var image = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(realPackagePath + "/LevelPreview.png");
            if(image != null) {
              info.image = Sprite.Create(image,
                new Rect(Vector2.zero, new Vector2(image.width, image.height)),
                new Vector2(1f, 1f));
            }
          }
          else
          #else
          if(true) 
          #endif
          {
            #if UNITY_EDITOR
            Log.D("StartCustomLevel", "realPackagePath not exists: " + realPackagePath);
            #endif

            string path = GamePathManager.GetLevelRealPath(info.name.ToLower(), false);
            UnityWebRequest request = UnityWebRequest.Get(path);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
              if (request.responseCode == 404)
                info.error = path + '\n' + I18N.Tr("core.ui.FileNotExist");
              else if (request.responseCode == 403)
                info.error = "No permission to read file";
              else
                info.error = "Http error: " + request.responseCode;
            } else {
              AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(request.downloadHandler.data);
              yield return assetBundleCreateRequest;
              var assetBundle = assetBundleCreateRequest.assetBundle;

              if (assetBundle == null)
                info.error = "Wrong level, failed to load AssetBundle";
              else {

                var json = assetBundle.LoadAsset<TextAsset>("Level.json");
                if (json != null) {
                  jsonString = json.text;
                  info.Set(JsonUtility.FromJson<GameLevelInfoJSON>(jsonString));
                }
                else 
                  info.error = "Wrong level, no Level.json";

                var logo = assetBundle.LoadAsset<Texture2D>("LevelLogo.png");
                if(logo != null) {
                  info.logo = Sprite.Create(logo,
                    new Rect(Vector2.zero, new Vector2(logo.width, logo.height)),
                    new Vector2(0.5f, 0.5f));
                }
                var image = assetBundle.LoadAsset<Texture2D>("LevelPreview.png");
                if(image != null) {
                  info.image = Sprite.Create(image,
                    new Rect(Vector2.zero, new Vector2(image.width, image.height)),
                    new Vector2(0.5f, 0.5f));
                }

                assetBundle.UnloadAsync(false);
              }
            }
          }
        }

        //设置到UI上
        if(string.IsNullOrEmpty(info.error)) {
          PanelError.gameObject.SetActive(false);
          PanelNoContent.gameObject.SetActive(false);

          TextName.text = info.name;
          TextAuthor.text = info.author;
          TextName.text = info.name;
          TextVersion.text = info.version;
          TextUrl.text = info.url;
          TextUrl.gameObject.SetActive(string.IsNullOrEmpty(info.url));
          TextIntroduction.text = info.introduction;
          ButtonPreview.gameObject.SetActive(info.allowPreview);
          ButtonEdit.gameObject.SetActive(info.allowPreview && info.allowEdit);
          ImageDepends.sprite = info.dependsSuccess ? IconSuccess : IconError;
          ImageBigImage.sprite = info.image != null ? info.image : NoPreviewImage;
          ImageLogo.sprite = info.logo;
          TextDepends.text = info.dependsStatus;

          PanelContent.gameObject.SetActive(true);
        } else {
          TextErrorContent.text = info.error;
          PanelError.gameObject.SetActive(true);
          PanelContent.gameObject.SetActive(false);
          PanelNoContent.gameObject.SetActive(false);
        }
      }
    }
    private void ClearSelectLevel() {
      selectedItem = null;
      PanelError.gameObject.SetActive(false);
      PanelContent.gameObject.SetActive(false);
      PanelNoContent.gameObject.SetActive(false);
    }

    private void LoadLevelList(bool refresh) {

      if(refresh) {
        loadedLevels.Clear();
        itemPrefabPool.Clear();
        ClearSelectLevel();
      }
      if(loadedLevels.Count == 0) {
  #if UNITY_EDITOR
        DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_LEVEL_FOLDER);
        DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < dirs.Length; i++)
          if(dirs[i].Name != "MakerAssets") {
            var info = new GameLevelInfo(dirs[i].Name, gamePackageManager);
            loadedLevels.Add(info);
            AddLevelToList(info);
          }
          
  #else
        string dir = GamePathManager.GetLevelRealPath("", false);
        if(Directory.Exists(dir)) {
          DirectoryInfo direction = new DirectoryInfo(dir);
          FileInfo[] files = direction.GetFiles("*.ballance", SearchOption.TopDirectoryOnly);
          Log.D("StartCustomLevel", "Scan Level dir \"" + dir + "\" found " + files.Length + " level fileds");
          for (int i = 0; i < files.Length; i++) {
            string name = Path.GetFileNameWithoutExtension(files[i].Name);
            if(Regex.IsMatch(name, "^level\\d{2}$")) {
              //调用HighscoreManager，检查过关状态
              if(!HighscoreManager.Instance.CheckLevelPassState(name))
                continue;
            }
            var info = new GameLevelInfo(name, gamePackageManager);
            loadedLevels.Add(info);
            AddLevelToList(info);
          }
        } else {
          Log.W("StartCustomLevel", "Level dir " + dir + " not exists!");
        }
  #endif
        PanelNoLevel.gameObject.SetActive(loadedLevels.Count == 0);
      }
    }
  }
}