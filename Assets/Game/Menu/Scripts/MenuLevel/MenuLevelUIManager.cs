using System;
using System.Collections.Generic;
using Ballance2.Config;
using Ballance2.Game;
using Ballance2.Game.GamePlay;
using Ballance2.Game.LevelBuilder;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class MenuLevelUIManager {
    public static void Create() {
      var package = GamePackage.GetSystemPackage();
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      var MessageCenter = MenuManager.MessageCenter;

      var PageMain = GameUIManager.RegisterPage("PageMain", "PageCommon");
      var PageAbout = GameUIManager.RegisterPage("PageAbout", "PageCommon");
      var PageHighscore = GameUIManager.RegisterPage("PageHighscore", "PageCommon");
      var PageStart = GameUIManager.RegisterPage("PageStart", "PageCommon");
      var PageStartOrginal = GameUIManager.RegisterPage("PageStartOrginal", "PageCommon");
      var PageStartLevelInfo = GameUIManager.RegisterPage("PageStartLevelInfo", "PageFull");
      var PageStartLevelSelect = GameUIManager.RegisterPage("PageStartLevelSelect", "PageFull");
      var PageStartEditor = GameUIManager.RegisterPage("PageStartEditor", "PageCommon");
      var PageLightZone = GameUIManager.RegisterPage("PageLightZone", "PageFull");
      var PageAboutCreators = GameUIManager.RegisterPage("PageAboutCreators", "PageCommon");
      var PageAboutProject = GameUIManager.RegisterPage("PageAboutProject", "PageCommon");
      var PageAboutLicense = GameUIManager.RegisterPage("PageAboutLicense", "PageCommon");
      var PageGlobalConfirm = GameUIManager.RegisterPage("PageGlobalConfirm", "PageCommonInGame");

      PageGlobalConfirm.CreateContent(package);
      PageGlobalConfirm.ForceShowDisplayAction = true;
      PageMain.CreateContent(package);
      PageMain.CanBack = false;
      PageAbout.CreateContent(package);
      PageHighscore.CreateContent(package);
      PageLightZone.CreateContent(package);
      PageAboutCreators.CreateContent(package);
      PageAboutProject.CreateContent(package);
      PageAboutLicense.CreateContent(package);
      PageStart.CreateContent(package);
      PageStartOrginal.CreateContent(package);
      PageStartLevelInfo.CreateContent(package);
      PageStartLevelSelect.CreateContent(package);
      PageStartEditor.CreateContent(package);
      PageStartOrginal.OnShow = (options) => {
        var ContentView = PageStartOrginal.Content.Find("ScrollViewVetical/Viewport/ContentView");
        //原版关卡未过关按钮
        for (var i = 2; i <= 12; i++) {
          var button = ContentView.Find("ButtonLevel" + i).GetComponent<Button>();
          var name = "Level";
          var prev = i - 1;
          if (prev < 10) 
            name = name + "0" + prev;
          else 
            name = name + prev;
          button.interactable = HighscoreManager.Instance.CheckLevelPassState(name);
        }
      };
      PageGlobalConfirm.CanBack = false;
      PageGlobalConfirm.OnShow = (options) => {
        PageGlobalConfirm.Content
          .GetComponent<PageGlobalConfirm>()
          .SetInfo((string)options["text"], (string)options["okText"], (string)options["cancelText"]);
      };
      PageLightZone.OnHide = () => {
        GameManager.GameMediator.NotifySingleEvent(MenuLevelCameraControl.EVENT_SWITCH_LIGHTZONE, false);
      };    

      //创建这个特殊logo特效
      var BallanceLogo3DObjects = GameManager.Instance.InstancePrefab(package.GetPrefabAsset("BallanceLogo3DObjects.prefab"), "BallanceLogo3DObjects");
      BallanceLogo3DObjects.SetActive(false);
      
      //GRAVITY 彩蛋

      //关于页面
      PageAbout.OnShow = (p) => {
        BallanceLogo3DObjects.SetActive(true);
        StartGRAVITYKey();
      };
      PageAbout.OnHide = () => {
        BallanceLogo3DObjects.SetActive(false);
        EndGRAVITYKey();
      };
      //制作者页面启动GRAVITY 彩蛋
      PageAboutCreators.OnShow = (p) => {
        StartGRAVITYKey();
      };
      PageAboutCreators.OnHide = () => {
        EndGRAVITYKey();
      };

      MessageCenter.SubscribeEvent("BtnOrginalClick", () => GameUIManager.GoPage("PageStartOrginal"));
      MessageCenter.SubscribeEvent("BtnCreativeClick", () => GameUIManager.GoPage("PageStartLevelSelect"));
      MessageCenter.SubscribeEvent("BtnLevelEditorClick", () => GameUIManager.GoPage("PageStartEditor"));
      MessageCenter.SubscribeEvent("BtnEditNewLevelClick", () =>
      {
        ShowEditorControllerTip(() =>
        {
          GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
          GameUIManager.MaskBlackFadeIn(1);
          GameTimer.Delay(1, () => {
            GameManager.GameMediator.NotifySingleEvent("CoreStartEditLevel", new object[] { null });
          });
        });
      });
      MessageCenter.SubscribeEvent("BtnEditExistsLevelClick", () =>
      {
        GameUIManager.GoPageWithOptions("PageStartLevelSelect", new Dictionary<string, object>()
        {
          { "page", 3 },
          { "isFromEditorPage", true }
        });
      });
      MessageCenter.SubscribeEvent("BtnStartClick", () => GameUIManager.GoPage("PageStart"));
      MessageCenter.SubscribeEvent("BtnCustomLevelClick", () =>  {
        GameManager.GameMediator.NotifySingleEvent("PageStartCustomLevelLoad");
        GameUIManager.GoPage("PageStartLevelSelect");
      });
      MessageCenter.SubscribeEvent("BtnAboutClick", () => GameUIManager.GoPage("PageAbout"));
      MessageCenter.SubscribeEvent("BtnSettingsClick", () => GameUIManager.GoPage("PageSettings"));
      MessageCenter.SubscribeEvent("BtnQuitClick", () => GameUIManager.GlobalConfirmWindow("I18N:core.ui.Menu.Main.QuitTitle" , () => {
        GameUIManager.CloseAllPage();
        GameManager.Instance.QuitGame();
      }));
      MessageCenter.SubscribeEvent("ButtonProjectClick", () => GameUIManager.GoPage("PageAboutProject"));
      MessageCenter.SubscribeEvent("BtnCreatorsClick", () => GameUIManager.GoPage("PageAboutCreators"));
      
      MessageCenter.SubscribeEvent("BtnStartLightZoneClick", () => {
        GameManager.GameMediator.NotifySingleEvent(MenuLevelCameraControl.EVENT_SWITCH_LIGHTZONE, true);
        GameUIManager.GoPage("PageLightZone");
      });
      MessageCenter.SubscribeEvent("ButtonOpenSourceLicenseClick", () => GameUIManager.GoPage("PageAboutLicense"));
      MessageCenter.SubscribeEvent("BtnGoBallanceBaClick", () => Application.OpenURL(ConstLinks.BallanceBa));
      MessageCenter.SubscribeEvent("BtnGoGithubClick", () => Application.OpenURL(ConstLinks.ProjectGithub));

      MessageCenter.SubscribeEvent("BtnHighscrollClick", () => {
        GameUIManager.GoPage("PageHighscore");
        HighscoreUIControl.Instance.LoadLevelData(0);
      });
      MessageCenter.SubscribeEvent("BtnHighscorePageLeftClick", () => {
        HighscoreUIControl.Instance.Prev();
      });
      MessageCenter.SubscribeEvent("BtnHighscorePageRightClick", () => {
        HighscoreUIControl.Instance.Next();
      });
      MessageCenter.SubscribeEvent("BtnAboutImengyuClick", () => {
        Application.OpenURL(ConstLinks.ImengyuHome);
      });

      MessageCenter.SubscribeEvent("BtnStartLev01Click", () => LoadInternalLevel("01"));
      MessageCenter.SubscribeEvent("BtnStartLev02Click", () => LoadInternalLevel("02"));
      MessageCenter.SubscribeEvent("BtnStartLev03Click", () => LoadInternalLevel("03"));
      MessageCenter.SubscribeEvent("BtnStartLev04Click", () => LoadInternalLevel("04"));
      MessageCenter.SubscribeEvent("BtnStartLev05Click", () => LoadInternalLevel("05"));
      MessageCenter.SubscribeEvent("BtnStartLev06Click", () => LoadInternalLevel("06"));
      MessageCenter.SubscribeEvent("BtnStartLev07Click", () => LoadInternalLevel("07"));
      MessageCenter.SubscribeEvent("BtnStartLev08Click", () => LoadInternalLevel("08"));
      MessageCenter.SubscribeEvent("BtnStartLev09Click", () => LoadInternalLevel("09"));
      MessageCenter.SubscribeEvent("BtnStartLev10Click", () => LoadInternalLevel("10"));
      MessageCenter.SubscribeEvent("BtnStartLev11Click", () => LoadInternalLevel("11"));
      MessageCenter.SubscribeEvent("BtnStartLev12Click", () => LoadInternalLevel("12"));
      MessageCenter.SubscribeEvent("ZoneSuDuClick", () => LoadInternalLevel("13"));
      MessageCenter.SubscribeEvent("ZoneLiLiangClick", () => LoadInternalLevel("14"));
      MessageCenter.SubscribeEvent("ZoneNenLiClick", () => LoadInternalLevel("15"));

      //点击版本按扭8次开启开发者模式
      var lastClickTime = 0.0f;
      var lastClickCount = 0;
      MessageCenter.SubscribeEvent("BtnVersionClick", () => {

        if (lastClickTime - Time.time > 5) 
          lastClickCount = 0;
        lastClickTime = Time.time;

        //增加
        lastClickCount++;
        if(lastClickCount >= 4) {
          if(lastClickCount >= 8) {
            GameManager.DebugMode = true;
            GameUIManager.GlobalToast(I18N.Tr("core.ui.Settings.Debug.YouAreInDebugMode"));
          } else {
            if (GameManager.DebugMode) {
              GameUIManager.GlobalToast(I18N.Tr("core.ui.Settings.Debug.YouAreInDebugMode"));
              return;
            } else
              GameUIManager.GlobalToast(I18N.TrF("core.ui.Settings.Debug.ClickNTimeToDebugMode", null, (8 -lastClickCount)));
          }
        }
      });

      //加载版本完整信息
      var ButtonVersionText = PageAbout.Content.transform.Find("ButtonVersion/Text").GetComponent<TMP_Text>();
      ButtonVersionText.text = I18N.TrF("core.ui.Settings.Debug.Version", "Version: {0}", Config.GameConst.GameVersion.ToString());
    }

    private static void LoadInternalLevel(string id) {
      var level = LevelManager.LevelManager.Instance.GetInternalLevel("Level" + id);
      //播放加载声音
      GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
      if (level == null)
        return;
      GameUIManager.Instance.MaskBlackFadeIn(1);
      GameTimer.Delay(1, () => GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", level));
    }
    public static void ShowEditorControllerTip(Action cb)
    {
      if (Gamepad.all.Count == 0)
      {
        cb();
        return;
      }
      GameUIManager.Instance.GlobalConfirmWindow(I18N.Tr("core.editor.help.ControllerTip"), () =>
      {
        cb();
      });
    }

    //GRAVITY 菜单控制
    private static List<InputAction> GRAVITYKeys = new List<InputAction>();
    private static int keysGRAVITYCurrentIndex = 0;

    private static void EndGRAVITYKey() {
      foreach(var action in GRAVITYKeys)
        action.Disable();
    }
    private static void StartGRAVITYKey() {

      Action<InputAction.CallbackContext> listenGRAVITYKey = (context) => {
        if (context.ReadValueAsButton()) {
          if (keysGRAVITYCurrentIndex >= 6) { 
            HighscoreManager.Instance.UnLockAllInternalLevel();
            GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_dong.wav", GameSoundType.UI);
            StartGRAVITYKey();
          }
          else {
            keysGRAVITYCurrentIndex++;
            GRAVITYKeys[keysGRAVITYCurrentIndex].Enable();
          }
        }
      };

      if (GRAVITYKeys.Count == 0)
      {
        var action = new InputAction("G", InputActionType.Button);
        action.AddBinding("<keyboard>/g");
        action.performed += listenGRAVITYKey;
        GRAVITYKeys.Add(action);
        action = new InputAction("R", InputActionType.Button);
        action.AddBinding("<keyboard>/r");
        action.performed += listenGRAVITYKey;
        GRAVITYKeys.Add(action);
        action = new InputAction("A", InputActionType.Button);
        action.AddBinding("<keyboard>/a");
        action.performed += listenGRAVITYKey;
        GRAVITYKeys.Add(action);
        action = new InputAction("V", InputActionType.Button);
        action.AddBinding("<keyboard>/v");
        action.performed += listenGRAVITYKey;
        GRAVITYKeys.Add(action);
        action = new InputAction("I", InputActionType.Button);
        action.AddBinding("<keyboard>/i");
        action.performed += listenGRAVITYKey;
        GRAVITYKeys.Add(action);
        action = new InputAction("T", InputActionType.Button);
        action.AddBinding("<keyboard>/t");
        action.performed += listenGRAVITYKey;
        GRAVITYKeys.Add(action);
        action = new InputAction("Y", InputActionType.Button);
        action.AddBinding("<keyboard>/y");
        action.performed += listenGRAVITYKey;
        GRAVITYKeys.Add(action);
      }

      foreach(var action in GRAVITYKeys)
        action.Disable();
      GRAVITYKeys[0].Enable();
      keysGRAVITYCurrentIndex = 0;
    }

    public static void Destroy()
    {
      if (GRAVITYKeys.Count > 0)
      {
        foreach (var action in GRAVITYKeys)
        {
          action.Disable();
          action.Dispose();
        }
        GRAVITYKeys.Clear();
      }
    }
  }
}