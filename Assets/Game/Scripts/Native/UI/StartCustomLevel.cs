using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Ballance2;
using Ballance2.Config.Settings;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Services.InputManager;
using Ballance2.Services.Pool;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartCustomLevel : MonoBehaviour
{
  public RectTransform PanelListContentView;
  public GameObject PanelListPrefab;
  public RectTransform PanelContent;
  public RectTransform PanelNoContent;
  public RectTransform PanelError;
  public Button ButtonBack;
  public Button ButtonStart;
  public Text TextRefresh;
  public Text TextErrorContent;
  public Image ImageLogo;
  public Text TextName;
  public Text TextAuthor;
  public Text TextVersion;
  public Text TextUrl;
  public Text TextIntroduction;
  public Text TextPreview;
  public Text TextDepends;
  public Image ImageDepends;
  public Sprite IconSuccess;
  public Sprite IconError;

  private GameUIManager gameUIManager;
  private GameSoundManager gameSoundManager;
  private GameObjectPool itemPrefabPool;

  private List<GameLevelInfo> loadedLevels = new List<GameLevelInfo>();
  private GameLevelInfo selectedItem = null;

  private class GameLevelInfo {
    public string name;
    public string error;
    public bool infoLoaded = false;
    public string author;
    public string version;
    public string introduction;
    public string url;
    public bool allowPreview = false;
    public List<GameLevelDependencies> requiredPackages = new List<GameLevelDependencies>();
    public bool dependsSuccess;
    public string dependsStatus;
    public Sprite logo;

    public GameLevelInfo(string name) {
      this.name = name;
    }

    public void Set(GameLevelInfoJSON o) {
      name = o.name;
      author = o.author;
      version = o.version;
      introduction = o.introduction;
      url = o.url;
      allowPreview = o.allowPreview;
      requiredPackages = new List<GameLevelDependencies>(o.requiredPackages);
    }
  }
  [Serializable]
  private class GameLevelInfoJSON {
    public string name;
    public string author;
    public string version;
    public string introduction;
    public string url;
    public bool allowPreview;
    public GameLevelDependencies[] requiredPackages;
  }
  [Serializable]
  public class GameLevelDependencies
  {
    public string name;
    public int minVersion;
  }

  private void Start() {
    gameUIManager = GameSystem.GetSystemService("GameUIManager") as GameUIManager;
    gameSoundManager = GameSystem.GetSystemService("GameSoundManager") as GameSoundManager;
    itemPrefabPool = new GameObjectPool("itemPrefabPool", PanelListPrefab, 32, 256, PanelListContentView);
    ButtonStart.onClick.AddListener(StartLevel);
    ButtonBack.onClick.AddListener(Back);
    EventTriggerListener.Get(TextRefresh.gameObject).onClick = (go) => LoadLevelList(true);
    EventTriggerListener.Get(TextPreview.gameObject).onClick = (go) => PreviewLevel();
    EventTriggerListener.Get(TextDepends.gameObject).onClick = (go) => ShowDependsInfo();

    GameManager.GameMediator.SubscribeSingleEvent(GamePackage.GetCorePackage(), "PageStartCustomLevelLoad", "A", (evtName, param) => {
      LoadLevelList(false);
      return false;
    });
  }
  private void OnDestroy() {
    itemPrefabPool.Destroy();
  }

  private void Back() {
    gameUIManager.BackPreviusPage();
  }
  private void StartLevel() {
    if(selectedItem != null) {
      gameSoundManager.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
      gameUIManager.MaskBlackFadeIn(1);
      GameManager.Instance.Delay(1000, () => GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", selectedItem.name));
    }
  }
  private void ShowDependsInfo() {
    gameUIManager.GlobalToast("TODO!");
  }  
  private void PreviewLevel() {
    gameUIManager.GlobalToast("TODO!");
  }

  private void AddLevelToList(GameLevelInfo info) {
    if(itemPrefabPool.ContainsObjectInParent(info.name))
      return;

    var go = itemPrefabPool.NextAvailableObject();
    var Button = go.GetComponent<Button>();
    var Text = go.transform.Find("Text").GetComponent<Text>();

    Text.text = info.name;
    Button.onClick.RemoveAllListeners();
    Button.onClick.AddListener(() => StartCoroutine(SelectLevel(info)));

    go.name = info.name;
  }
  private IEnumerator SelectLevel(GameLevelInfo info) {
    selectedItem = info;

    //加载信息
    if(!selectedItem.infoLoaded) {
      selectedItem.infoLoaded = true;

      TextErrorContent.text = I18N.Tr("core.ui.Loading");
      PanelError.gameObject.SetActive(true);
      PanelContent.gameObject.SetActive(false);
      PanelNoContent.gameObject.SetActive(false);

      string jsonString = "";
#if UNITY_EDITOR
      string realPackagePath = GamePathManager.DEBUG_LEVEL_FOLDER + "/" + name;
      //在编辑器中加载
      if (DebugSettings.Instance.PackageLoadWay == LoadResWay.InUnityEditorProject && Directory.Exists(realPackagePath))
        jsonString = File.ReadAllText(realPackagePath);
      else
#else
      if(true) 
#endif
      {
        string path = GamePathManager.GetLevelRealPath(info.name.ToLower(), false);
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
          if (request.responseCode == 404)
            info.error = I18N.Tr("core.ui.FileNotExist");
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

              
            var logo = assetBundle.LoadAsset<Texture2D>("Level.png");
            if(logo != null) {
              info.logo = Sprite.Create(logo,
                new Rect(Vector2.zero, new Vector2(logo.width, logo.height)),
                new Vector2(0.5f, 0.5f));
            }
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
      TextPreview.gameObject.SetActive(info.allowPreview);
      ImageDepends.sprite = info.dependsSuccess ? IconSuccess : IconError;
      TextDepends.text = info.dependsStatus;

      PanelContent.gameObject.SetActive(true);
    } else {
      TextErrorContent.text = info.error;
      PanelError.gameObject.SetActive(true);
      PanelContent.gameObject.SetActive(false);
      PanelNoContent.gameObject.SetActive(false);
    }
  }

  private void LoadLevelList(bool refresh) {
    if(refresh) {
      loadedLevels.Clear();
      itemPrefabPool.Clear();
    }
    if(loadedLevels.Count == 0) {
#if UNITY_EDITOR
      DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_LEVEL_FOLDER);
      DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
      for (int i = 0; i < dirs.Length; i++)
        if(dirs[i].Name != "MakerAssets") {
          var info = new GameLevelInfo(dirs[i].Name);
          loadedLevels.Add(info);
          AddLevelToList(info);
        }
#else
      DirectoryInfo direction = new DirectoryInfo(GamePathManager.GetLevelRealPath(""));
      DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
      for (int i = 0; i < dirs.Length; i++)
        if(!Regex.IsMatch(dirs[i].Name, "^level([0]{1}[0-9]{1})|([1]{1}[0-3]{1})$", RegexOptions.IgnoreCase)) { //排除自带关卡
          var info = new GameLevelInfo(dirs[i].Name);
          loadedLevels.Add(info);
          AddLevelToList(info);
        }
#endif
    }
  }
}
