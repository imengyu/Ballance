using System.Collections;
using System.Collections.Generic;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Services.InputManager;
using Ballance2.Services.Pool;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
  自定义关卡管理菜单的控制脚本
*/
namespace Ballance2.Menu
{
  public class LevelInfoUI : MonoBehaviour
  {
    public ScrollRect ScrollView;
    public RectTransform PanelContent;
    public RectTransform PanelError;
    public Button ButtonBack;
    public Button ButtonStart;
    public Button ButtonStart2;
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
    public TMP_Text TextPath;
    public Image ImageDepends;
    public Image ImageBigImage;

    public Sprite NoPreviewImage;
    public Sprite NoLogoImage;
    public Sprite IconSuccess;
    public Sprite IconError;

    public GameObject PanelDependsListPrefab;
    public RectTransform PanelDependsListContentView;

    private GameUIManager gameUIManager;
    private GameSoundManager gameSoundManager;
    private GamePackageManager gamePackageManager;

    private GameObjectPool itemDependsPrefabPool;
    private LevelSelectUIManager.ListItem selectedItem = null;
    private bool dependesChanged = true;
    private bool isFromEditorPage = true;

    private void Start() {
      gameUIManager = GameSystem.GetSystemService<GameUIManager>();
      gameSoundManager = GameSystem.GetSystemService<GameSoundManager>();
      gamePackageManager = GameSystem.GetSystemService<GamePackageManager>();
      itemDependsPrefabPool = new GameObjectPool("itemDependsPrefabPool", PanelDependsListPrefab, 32, 128, PanelDependsListContentView);

      PanelContent.gameObject.SetActive(false);
      PanelError.gameObject.SetActive(false);
      ButtonStart.onClick.AddListener(StartLevel);
      ButtonStart2.onClick.AddListener(EditLevel);
      ButtonBack.onClick.AddListener(Back);
      ButtonPreview.onClick.AddListener(PreviewLevel);
      ButtonEdit.onClick.AddListener(EditLevel);

      var page = gameUIManager.FindPage("PageStartLevelInfo");
      page.OnShow = (param) =>
      {
        selectedItem = (LevelSelectUIManager.ListItem)param["item"];
        isFromEditorPage = (param.ContainsKey("isEditor") && (bool)param["isEditor"] == true);
        StartCoroutine(LoadLevelInfo(selectedItem));
      };
      page.OnHide = () =>
      {
        PanelContent.gameObject.SetActive(false);
        selectedItem = null;
      };
    }
    private void OnDestroy() {
      itemDependsPrefabPool.Destroy();
    }

    private IEnumerator LoadLevelInfo(LevelSelectUIManager.ListItem item)
    {
      if (!item.Level.IsLoaded)
        yield return new WaitUntil(() => item.Level.IsLoaded);
      if (!gameObject.activeSelf)
        yield break;
      if (item.IsNetwork)
      {
        //TODO: IsNetwork
        TextPath.text = "";
      }
      else
      {
        TextName.text = StringUtils.ReturnDefaultIsNullOrEmpty(item.Level.InfoJson.name, "Level");
        TextAuthor.text = TextName.text + " by " + StringUtils.ReturnDefaultIsNullOrEmpty(item.Level.InfoJson.author, I18N.Tr("core.ui.Menu.LevelSelect.NoAuthor"));
        TextVersion.text = item.Level.InfoJson.version;
        TextUrl.text = item.Level.InfoJson.url;
        TextUrl.gameObject.SetActive(string.IsNullOrEmpty(item.Level.InfoJson.url));
        TextIntroduction.text = StringUtils.ReturnDefaultIsNullOrEmpty(item.Level.InfoJson.introduction, I18N.Tr("core.ui.Menu.LevelSelect.NoDsec"));
        if (item.Level.Type == LevelManager.LevelRegistedType.Mine)
        {
          if (isFromEditorPage)
          {
            ButtonStart2.gameObject.SetActive(true);
            ButtonStart.gameObject.SetActive(false);
            ButtonPreview.gameObject.SetActive(false);
            ButtonEdit.gameObject.SetActive(false);
          }
          else
          {
            ButtonStart2.gameObject.SetActive(false);
            ButtonStart.gameObject.SetActive(true);
            ButtonPreview.gameObject.SetActive(true);
            ButtonEdit.gameObject.SetActive(true);
          }
        }
        else
        {
          ButtonStart2.gameObject.SetActive(false);
          ButtonStart.gameObject.SetActive(true);
          ButtonPreview.gameObject.SetActive(item.Level.InfoJson.allowPreview);
          ButtonEdit.gameObject.SetActive(false);
        }   
        ImageDepends.sprite = item.Level.DependsSuccess ? IconSuccess : IconError;
        ImageBigImage.sprite = item.Preview != null ? item.Preview : NoPreviewImage;
        ImageLogo.sprite = item.Icon != null ? item.Icon : NoLogoImage;
        TextDepends.text = item.Level.DependsStatus;
        TextPath.text = item.LocalPath;
      }

      PanelContent.gameObject.SetActive(true);
      ShowDependsInfo();

      yield return new WaitForSeconds(1f);

      ScrollView.verticalNormalizedPosition = 1;
    }
    private void Back() {
      gameUIManager.BackPreviusPage();
    }
    private void StartLevel() {
      if(selectedItem != null) {
        gameSoundManager.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
        gameUIManager.MaskBlackFadeIn(1);
        GameTimer.Delay(1, () => {
          GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", new object[] { selectedItem.Level });
        });
      }
    }  
    private void PreviewLevel() {
      if(selectedItem != null) {
        gameSoundManager.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
        gameUIManager.MaskBlackFadeIn(1);
        GameTimer.Delay(1, () => {
          GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", new object[]{ selectedItem.Level, true });
        });
      }
    }
    private void EditLevel() {
      if(selectedItem != null) {
        MenuLevelUIManager.ShowEditorControllerTip(() =>
        {
          gameSoundManager.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
          gameUIManager.MaskBlackFadeIn(1);
          GameTimer.Delay(1, () =>
          {
            GameManager.GameMediator.NotifySingleEvent("CoreStartEditLevel", new object[] { selectedItem.Level });
          });
        });
      }
    }
    private void ShowDependsInfo() {
      //显示关卡依赖信息
      if(dependesChanged) {
        dependesChanged = false;
        itemDependsPrefabPool.Clear();
        selectedItem.Level.RequiredPackages.ForEach((p) => {
          var go = itemDependsPrefabPool.NextAvailableObject();
          var text = go.transform.Find("Text").GetComponent<TMP_Text>();
          var button = go.transform.Find("Button").GetComponent<Button>();
          if(p.loaded == "true")
            text.text = "";
          else if(p.loaded == "vermis")
            text.text = "<color=#f00>" + p.name + " >= " + p.minVersion + "</color> (" + I18N.Tr("core.ui.Menu.LevelSelect.VersionMismatch") + ")";
          else
            text.text = "<color=#f00>" + p.name + " >= " + p.minVersion + "</color>";

          button.onClick.RemoveAllListeners();
          button.onClick.AddListener(() => {
            gameUIManager.GoPageWithOptions("PackageManageWindow", new Dictionary<string, object>{ { "LocatePackage", p.name } });
          });
        });
      }
    }   
  }
}