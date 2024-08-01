using System;
using System.Collections.Generic;
using Ballance2.Game;
using Ballance2.Game.GamePlay;
using Ballance2.Game.LevelBuilder;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
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
      var PageLightZone = GameUIManager.RegisterPage("PageLightZone", "PageTransparent");
      var PageAboutCreators = GameUIManager.RegisterPage("PageAboutCreators", "PageCommon");
      var PageAboutProject = GameUIManager.RegisterPage("PageAboutProject", "PageCommon");
      var PageAboutLicense = GameUIManager.RegisterPage("PageAboutLicense", "PageCommon");
      var PageStartCustomLevel = GameUIManager.RegisterPage("PageStartCustomLevel", "PageFull");
      var PageGlobalConfirm = GameUIManager.RegisterPage("PageGlobalConfirm", "PageCommon");

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
      PageStartCustomLevel.CreateContent(package);
      PageStart.OnShow = (options) => {
        var ContentView = PageStart.Content.Find("ScrollViewVetical/Viewport/ContentView");
        for (var i = 2; i <= 12; i++) {
          var button = ContentView.Find("ButtonLevel" + i).GetComponent<Button>();
          var name = "level";
          if (i < 10) 
            name = name + "0" + i;
          else 
            name = name + i;
          button.interactable = HighscoreManager.Instance.CheckLevelPassState(name);
        }
        //非 windows 隐藏此按扭
        #if !UNITY_STANDALONE_WIN
          ContentView.Find("ButtonNMO").gameObject.SetActive(false);
        #else
        if (!FileUtils.DirectoryExists(Application.dataPath + "/VirtoolsLoader/"))
          ContentView.Find("ButtonNMO").gameObject.SetActive(false);
        #endif
      };
      PageGlobalConfirm.OnShow = (options) => {
        var ButtonConfirmText = PageGlobalConfirm.Content.Find("Panel/ButtonConfirm/Text").GetComponent<UIText>();
        var ButtonCancelText = PageGlobalConfirm.Content.Find("Panel/ButtonCancel/Text").GetComponent<UIText>();
        var Text = PageGlobalConfirm.Content.Find("Text").GetComponent<UIText>();
        if (ButtonConfirmText != null)
          ButtonConfirmText.text = options["okText"];
        if (ButtonCancelText != null)
          ButtonCancelText.text = options["cancelText"];
        if (Text != null)
          Text.text = options["text"];
      };

      MessageCenter.SubscribeEvent("BtnConfirmConfirmClick", () =>  {
        GameUIManager.GlobalConfirmWindowCallback(true);
        GameUIManager.BackPreviusPage();
      });
      MessageCenter.SubscribeEvent("BtnConfirmCancelClick", () => {
        GameUIManager.GlobalConfirmWindowCallback(false);
        GameUIManager.BackPreviusPage();
      });
      

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

      MessageCenter.SubscribeEvent("BtnStartClick", () => GameUIManager.GoPage("PageStart"));
      MessageCenter.SubscribeEvent("BtnCustomLevelClick", () =>  {
        GameManager.GameMediator.NotifySingleEvent("PageStartCustomLevelLoad");
        GameUIManager.GoPage("PageStartCustomLevel");
      });
      MessageCenter.SubscribeEvent("BtnCustomNMOLevelClick", () => {
        LevelLoaderNative.PickLevelFile(".nmo", (path, _) => {
          //播放加载声音
          GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
          GameUIManager.MaskBlackFadeIn(1);
          //加载
          GameTimer.Delay(1, () => { 
            GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", path);
          });
        });
      });
      MessageCenter.SubscribeEvent("BtnAboutClick", () => GameUIManager.GoPage("PageAbout"));
      MessageCenter.SubscribeEvent("BtnSettingsClick", () => GameUIManager.GoPage("PageSettings"));
      MessageCenter.SubscribeEvent("BtnQuitClick", () => GameUIManager.GlobalConfirmWindow("I18N:core.ui.MainMenuQuitTitle" , () => {
        GameUIManager.CloseAllPage();
        GameManager.Instance.QuitGame();
      }));
      MessageCenter.SubscribeEvent("ButtonProjectClick", () => GameUIManager.GoPage("PageAboutProject"));
      MessageCenter.SubscribeEvent("BtnCreatorsClick", () => GameUIManager.GoPage("PageAboutCreators"));
      
      MessageCenter.SubscribeEvent("BtnLightZoneBackClick", () => {
        GameManager.GameMediator.NotifySingleEvent(MenuLevelCameraControl.EVENT_SWITCH_LIGHTZONE, false);
        GameUIManager.BackPreviusPage();
      });
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
            GameUIManager.GlobalToast(I18N.Tr("core.ui.SettingsYouAreInDebugMode"));
          } else {
            if (GameManager.DebugMode) {
              GameUIManager.GlobalToast(I18N.Tr("core.ui.SettingsYouAreInDebugMode"));
              return;
            } else
              GameUIManager.GlobalToast(I18N.TrF("core.ui.SettingsClickNTimeToDebugMode", (8-lastClickCount)));
          }
        }
      });

      //加载版本完整信息
      var ButtonVersionText = PageAbout.Content.transform.Find("ButtonVersion/Text").GetComponent<TMP_Text>();
      ButtonVersionText.text = I18N.TrF("core.ui.SettingsVersion", "Version: {0}", Config.GameConst.GameVersion.ToString());
    }

    private static void LoadInternalLevel(string id) {
      //播放加载声音
      GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
      GameUIManager.Instance.MaskBlackFadeIn(1);
      GameTimer.Delay(1, () => GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", "level" + id));
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
  }
}