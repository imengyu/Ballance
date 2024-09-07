using Ballance2.Game;
using Ballance2.Game.GamePlay;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class SettingsUIControl {
    public static void Create() {
      var GameUIManager = Services.GameUIManager.Instance;
      var MessageCenter = MenuManager.MessageCenter;
      var package = GamePackage.GetSystemPackage();

      var PageSettings = GameUIManager.RegisterPage("PageSettings", "PageCommon");
      var PageSettingsInGame = GameUIManager.RegisterPage("PageSettingsInGame", "PageCommon");
      var PageSettingsAudio = GameUIManager.RegisterPage("PageSettingsAudio", "PageCommon");
      var PageSettingsControls = GameUIManager.RegisterPage("PageSettingsControls", "PageCommon");
      var PageSettingsGraphics = GameUIManager.RegisterPage("PageSettingsGraphics", "PageCommon");
      var PageLanguage = GameUIManager.RegisterPage("PageLanguage", "PageCommon");
      var PagePackageManager = GameUIManager.RegisterPage("PagePackageManager", "PageFull");
      var PagePackageManagerInfo = GameUIManager.RegisterPage("PagePackageManagerInfo", "PageFull");
      var PagePackageManagerTrust = GameUIManager.RegisterPage("PagePackageManagerTrust", "PageFull");
      var PageBindKey = GameUIManager.RegisterPage("PageBindKey", "PageCommon");

      PageSettings.CreateContent(package);
      PageSettingsAudio.CreateContent(package);
      PageSettingsControls.CreateContent(package);
      PageSettingsGraphics.CreateContent(package);
      PageSettingsInGame.CreateContent(package);
      PageLanguage.CreateContent(package);
      PagePackageManager.CreateContent(package);
      PagePackageManagerInfo.CreateContent(package);
      PagePackageManagerTrust.CreateContent(package);
      PageBindKey.CreateContent(package);
      PageBindKey.ForceShowDisplayAction = true;

      PageBindKey.OnHide = () => {
        ControlManager.Instance.SaveActionSettings();
      };
      PageBindKey.OnShow += (options) => {
        var keyName = options["keyName"];
        var keyCurrent = options["keyCurrent"];
        var Text = PageBindKey.Content.Find("Panel/Text").GetComponent<UIText>();
        Text.text = I18N.TrF("core.ui.Settings.Control.ControllerBind", "", keyName, keyCurrent);
      };
      PageLanguage.OnShow += (options) => {
        LoadAdditionalLanguagesToUI(PageLanguage);
      };

      GameTimer.Delay(1.0f, () => {
        BindControllerSettingsUI(PageSettingsControls);
        BindSettingsUI(MessageCenter);
      });
    }

    private class QualitiesItemData {
      public string text;
      public int level;
    }

    private static void BindControllerSettingsUI(GameUIPage PageSettingsControls)
    {
      var control = ControlManager.Instance;
      control.LoadActionSettings(false);

      var PanelKeyboard = PageSettingsControls.Content.Find("ScrollViewVetical/Viewport/ContentView/PanelKeyboard");
      var PanelController = PageSettingsControls.Content.Find("ScrollViewVetical/Viewport/ContentView/PanelController");
      var PanelOthers = PageSettingsControls.Content.Find("ScrollViewVetical/Viewport/ContentView/PanelOthers");
      var ButtonKeyChooseControlKeyForward = PanelKeyboard.Find("ButtonKeyChooseControlKeyForward").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyBack = PanelKeyboard.Find("ButtonKeyChooseControlKeyBack").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyUp = PanelKeyboard.Find("ButtonKeyChooseControlKeyUp").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyDown = PanelKeyboard.Find("ButtonKeyChooseControlKeyDown").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyLeft = PanelKeyboard.Find("ButtonKeyChooseControlKeyLeft").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyRight = PanelKeyboard.Find("ButtonKeyChooseControlKeyRight").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyOverlookCam = PanelKeyboard.Find("ButtonKeyChooseControlKeyOverlookCam").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyRoateCam = PanelKeyboard.Find("ButtonKeyChooseControlKeyRoateCam").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlKeyRoateCam2 = PanelKeyboard.Find("ButtonKeyChooseControlKeyRoateCam2").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerForward = PanelController.Find("ButtonKeyChooseControlControllerForward").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerBack = PanelController.Find("ButtonKeyChooseControlControllerBack").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerUp = PanelController.Find("ButtonKeyChooseControlControllerUp").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerDown = PanelController.Find("ButtonKeyChooseControlControllerDown").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerLeft = PanelController.Find("ButtonKeyChooseControlControllerLeft").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerRight = PanelController.Find("ButtonKeyChooseControlControllerRight").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerOverlookCam = PanelController.Find("ButtonKeyChooseControlControllerOverlookCam").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerRoateCamLeft = PanelController.Find("ButtonKeyChooseControlControllerRoateCamLeft").GetComponent<KeyChoose>();
      var ButtonKeyChooseControlControllerRoateCamRight = PanelController.Find("ButtonKeyChooseControlControllerRoateCamRight").GetComponent<KeyChoose>();
      var ButtonKeyChooseScreenShort = PanelOthers.Find("ButtonKeyChooseScreenShort").GetComponent<KeyChoose>();
      var ButtonKeyChooseToggleFPS = PanelOthers.Find("ButtonKeyChooseToggleFPS").GetComponent<KeyChoose>();
      var ButtonKeyChooseToggleView = PanelOthers.Find("ButtonKeyChooseToggleView").GetComponent<KeyChoose>();

      ButtonKeyChooseControlKeyForward.BindAction = control.KeyBoardActionForward;
      ButtonKeyChooseControlKeyBack.BindAction = control.KeyBoardActionBack;
      ButtonKeyChooseControlKeyUp.BindAction = control.KeyBoardActionUp;
      ButtonKeyChooseControlKeyDown.BindAction = control.KeyBoardActionDown;
      ButtonKeyChooseControlKeyLeft.BindAction = control.KeyBoardActionLeft;
      ButtonKeyChooseControlKeyRight.BindAction = control.KeyBoardActionRight;
      ButtonKeyChooseControlKeyOverlookCam.BindAction = control.KeyBoardActionOverlook;
      ButtonKeyChooseControlKeyRoateCam.BindAction = control.KeyBoardActionRotateCam;
      ButtonKeyChooseControlKeyRoateCam.BindingIndex = 0;
      ButtonKeyChooseControlKeyRoateCam2.BindAction = control.KeyBoardActionRotateCam;
      ButtonKeyChooseControlKeyRoateCam2.BindingIndex = 1;

      ButtonKeyChooseControlControllerForward.BindAction = control.ControllerActionMove;
      ButtonKeyChooseControlControllerForward.BindingIndex = 1;
      ButtonKeyChooseControlControllerBack.BindAction = control.ControllerActionMove;
      ButtonKeyChooseControlControllerBack.BindingIndex = 2;
      ButtonKeyChooseControlControllerLeft.BindAction = control.ControllerActionMove;
      ButtonKeyChooseControlControllerLeft.BindingIndex = 3;
      ButtonKeyChooseControlControllerRight.BindAction = control.ControllerActionMove;
      ButtonKeyChooseControlControllerRight.BindingIndex = 4;
      ButtonKeyChooseControlControllerDown.BindAction = control.ControllerActionFly;
      ButtonKeyChooseControlControllerDown.BindingIndex = 1;
      ButtonKeyChooseControlControllerUp.BindAction = control.ControllerActionFly;
      ButtonKeyChooseControlControllerUp.BindingIndex = 2;
      ButtonKeyChooseControlControllerOverlookCam.BindAction = control.ControllerActionOverlook;
      ButtonKeyChooseControlControllerRoateCamLeft.BindAction = control.ControllerActionRotateCamLeft;
      ButtonKeyChooseControlControllerRoateCamRight.BindAction = control.ControllerActionRotateCamRight;

      ButtonKeyChooseScreenShort.BindAction = control.ScreenShort;
      ButtonKeyChooseToggleFPS.BindAction = control.ToggleFPS;
      ButtonKeyChooseToggleView.BindAction = control.ToggleView;

      I18NProvider.RegisterSettingStringReplacer("control", (key) =>
      {
        switch(key)
        {
          case "RotateView":
            if (CommonUtils.HasGamePad)
              return I18N.TrF("core.ui.GameUI.TutorialText.KeyAnd", null, 
                control.ControllerActionRotateCamLeft.GetBindingDisplayString(),
                control.ControllerActionRotateCamRight.GetBindingDisplayString()
              );
            return I18N.TrF("core.ui.GameUI.TutorialText.KeyBoth", null,
              control.KeyBoardActionRotateCam.GetBindingDisplayString(),
              I18N.TrF("core.ui.GameUI.TutorialText.KeyOr", null,
                control.KeyBoardActionLeft.GetBindingDisplayString(),
                control.KeyBoardActionRight.GetBindingDisplayString()
              )
            );
          case "Overlook":
            if (CommonUtils.HasGamePad)
              return control.ControllerActionOverlook.GetBindingDisplayString();
            return control.KeyBoardActionOverlook.GetBindingDisplayString();
          case "Move":
            if (!CommonUtils.HasGamePad)
              return I18N.TrF("core.ui.GameUI.TutorialText.KeyOr4", null,
               control.KeyBoardActionForward.GetBindingDisplayString(),
               control.KeyBoardActionBack.GetBindingDisplayString(),
               control.KeyBoardActionLeft.GetBindingDisplayString(),
               control.KeyBoardActionRight.GetBindingDisplayString()
             );
            return control.ControllerActionMove.GetBindingDisplayString();
          case "Back":

            break;
        }
        return "";
      });
    }
    private static void BindSettingsUI(GameUIMessageCenter MessageCenter) {
      var GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      var GameUIManager = Services.GameUIManager.Instance;

      //设置数据绑定
      //声音
      var updateVoiceMain = MessageCenter.SubscribeValueBinder("VoiceMain", (val) => {
        GameSettings.SetFloat(SettingConstants.SettingsVoiceMain, (float)val);
        GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
      });
      var updateVoiceUI = MessageCenter.SubscribeValueBinder("VoiceUI", (val) => {
        GameSettings.SetFloat(SettingConstants.SettingsVoiceUI, (float)val);
        GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
      });
      var updateVioceBackground = MessageCenter.SubscribeValueBinder("VioceBackground", (val) => {
        GameSettings.SetFloat(SettingConstants.SettingsVoiceBackground, (float)val);
        GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
      });
      //视频
      var updateGrResolution = MessageCenter.SubscribeValueBinder("GrResolution", (val) => {
        GameSettings.SetInt(SettingConstants.SettingsVideoResolution, (int)(float)val);
      });
      var updateGrQuality = MessageCenter.SubscribeValueBinder("GrQuality", (val) => {
        GameSettings.SetInt(SettingConstants.SettingsVideoQuality, (int)(float)val);
      });
      var updateGrFullScreen = MessageCenter.SubscribeValueBinder("GrFullScreen", (val) => {
        GameSettings.SetBool(SettingConstants.SettingsVideoFullScreen, (bool)val);
      });
      var updateGrCloud = MessageCenter.SubscribeValueBinder("GrCloud", (val) => {
        GameSettings.SetBool(SettingConstants.SettingsVideoCloud, (bool)val);
      });
      var updateGrVSync = MessageCenter.SubscribeValueBinder("GrVSync", (val) => {
        GameSettings.SetInt(SettingConstants.SettingsVideoVsync, (bool)val ? 1 : 0 );
      });
      //控制
      var updateControlReverse = MessageCenter.SubscribeValueBinder("ControlReverse", (val) => {
        GameSettings.SetBool(SettingConstants.SettingsControlKeyReverse, (bool)val);
      });
      var updateControlUISize = MessageCenter.SubscribeValueBinder("ControlUISize", (val) => {
        GameSettings.SetFloat(SettingConstants.SettingsControlKeySize, (float)val);
        GameSettings.NotifySettingsUpdate(SettingConstants.SettingsControl);
      });

      Resolution[] resolutions = null;
      QualitiesItemData[] qualities = null;

      //设置数据加载
      MessageCenter.SubscribeEvent("BtnSettingsGraphicsClick", () => {
        if (updateGrFullScreen != null) updateGrFullScreen.Invoke(GameSettings.GetBool(SettingConstants.SettingsVideoFullScreen, Screen.fullScreen));
        if (updateGrCloud != null) updateGrCloud.Invoke(GameSettings.GetBool(SettingConstants.SettingsVideoCloud));
        if (updateGrVSync != null) updateGrVSync.Invoke(GameSettings.GetInt(SettingConstants.SettingsVideoVsync, QualitySettings.vSyncCount) >= 1 ? true : false);
        
        GameUIManager.GoPage("PageSettingsGraphics");

        if (resolutions == null) 
        {
          //加载分辨率设置
          var isMathed = false;
          resolutions = Screen.resolutions;
          var GrResolution = MessageCenter.GetComponentInstance("GrResolution").GetComponent<Updown>();
          var currentResolution = Screen.currentResolution;
          for (int i = 0; i < resolutions.Length; i++)
          {
            var resolution = resolutions[i];
            GrResolution.AddOption($"{resolution.width}x{resolution.height}@{resolution.refreshRateRatio.value.ToString("F2")}");
            if (
              updateGrResolution != null
              && currentResolution.width == resolution.width
              && currentResolution.height == resolution.height
              && currentResolution.refreshRateRatio.value == resolution.refreshRateRatio.value
            )
            {
              isMathed = true;
              updateGrResolution.Invoke(i - 1);
            }
          }
          GrResolution.AddOption("Default");
          if (!isMathed)
          {
            updateGrResolution.Invoke(resolutions.Length);
          }
        }
        if (qualities == null) 
        {
          qualities = new QualitiesItemData[] {
            new QualitiesItemData { level = 0, text = I18N.Tr("core.ui.Settings.Quality.VeryLow") },
            new QualitiesItemData { level = 1, text = I18N.Tr("core.ui.Settings.Quality.Low") },
            new QualitiesItemData { level = 2, text = I18N.Tr("core.ui.Settings.Quality.Medium") },
            new QualitiesItemData { level = 3, text = I18N.Tr("core.ui.Settings.Quality.High") },
            new QualitiesItemData { level = 4, text = I18N.Tr("core.ui.Settings.Quality.VeryHigh") },
            new QualitiesItemData { level = 5, text = I18N.Tr("core.ui.Settings.Quality.Ultra") },
          };
          var GrQuality = MessageCenter.GetComponentInstance("GrQuality").GetComponent<Updown>();
          var qualitiyCurrent = QualitySettings.GetQualityLevel();
          for (int i = 0; i < qualities.Length; i++) {
            var qualitiy = qualities[i];
            GrQuality.AddOption(qualitiy.text);
            if (qualitiyCurrent == qualitiy.level && updateGrQuality != null) 
              updateGrQuality.Invoke(i - 1);
          }
        }
      });
      MessageCenter.SubscribeEvent("BtnSettingsControlsClick", () => {
        if (updateControlReverse != null) updateControlReverse.Invoke(GameSettings.GetBool(SettingConstants.SettingsControlKeyReverse));
        if (updateControlUISize != null) updateControlUISize.Invoke(GameSettings.GetFloat(SettingConstants.SettingsControlKeySize));
        GameUIManager.GoPage("PageSettingsControls");
      });
      MessageCenter.SubscribeEvent("BtnSettingsAudioClick", () => {
        if (updateVioceBackground != null) updateVioceBackground.Invoke(GameSettings.GetFloat(SettingConstants.SettingsVoiceBackground));
        if (updateVoiceUI != null) updateVoiceUI.Invoke(GameSettings.GetFloat(SettingConstants.SettingsVoiceUI));
        if (updateVoiceMain != null) updateVoiceMain.Invoke(GameSettings.GetFloat(SettingConstants.SettingsVoiceMain));
        GameUIManager.GoPage("PageSettingsAudio");
      });
      MessageCenter.SubscribeEvent("BtnSettingsPackageClick", () => {
        GameUIManager.GoPage("PagePackageManager");
      });
      MessageCenter.SubscribeEvent("BtnSettingsResetClick", () => GameUIManager.GlobalConfirmWindow("I18N:core.ui.Settings.Control.ResetAsk" , () => {
        GameSettingsManager.ResetDefaultSettings();
        ControlManager.Instance.LoadActionSettings(true);
        GameUIManager.BackPreviusPage();
      }));

      //语言
      MessageCenter.SubscribeEvent("BtnSettingsLanguageClick", () => { GameUIManager.GoPage("PageLanguage"); });
      MessageCenter.SubscribeEvent("BtnChineseSimplifiedClick", () => { 
        ApplyLanguage(SystemLanguage.ChineseSimplified); 
      });
      MessageCenter.SubscribeEvent("BtnHelpTranslateClick", () => {
        Application.OpenURL(ConstLinks.HelpTranslate);
      });

      MessageCenter.SubscribeEvent("BtnLangBackClick", () => {
        GameUIManager.BackPreviusPage();
        GameUIManager.BackPreviusPage();
      });

      //设置保存
      MessageCenter.SubscribeEvent("BtnSettingsGraphicsBackClick", () => {
        GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVideo);
        GameUIManager.BackPreviusPage();
      });
      MessageCenter.SubscribeEvent("BtnSettingsControlsBackClick", () => {
        var control = ControlManager.Instance;
        control.SaveActionSettings();
        GameSettings.NotifySettingsUpdate(SettingConstants.SettingsControl);
        GameUIManager.BackPreviusPage();
      });
      MessageCenter.SubscribeEvent("BtnSettingsAudioBackClick", () => {
        GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
        GameUIManager.BackPreviusPage();
      });

      //重启游戏
      MessageCenter.SubscribeEvent("BtnRestartGameClick", () => { GameManager.Instance.RestartGame(); });
    }

    private static void LoadAdditionalLanguagesToUI(GameUIPage PageLanguage)
    {
      var Prefab = PageLanguage.Content.Find("ScrollViewVetical/Viewport/ContentView/BtnPrefab").gameObject;
      var AdditionalLanguages = PageLanguage.Content.Find("ScrollViewVetical/Viewport/ContentView/AdditionalLanguages");

      foreach(var lang in I18NProvider.AdditionalLanguageFiles)
      {
        var langKey = lang.Key;
        var btn = CloneUtils.CloneNewObjectWithParent(Prefab, AdditionalLanguages);
        btn.transform.Find("Text").gameObject.GetComponent<TMP_Text>().text = lang.Value;
        btn.SetActive(true);
        btn.GetComponent<Button>().onClick.AddListener(() => {
          ApplyLanguage(langKey);
        });
      }
    }
    private static void ApplyLanguage(SystemLanguage language) {
      GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME).SetInt("language", (int)language);
      if (I18NProvider.GetCurrentLanguage() != language)
        GameUIManager.Instance.GlobalConfirmWindow("I18N:core.ui.Menu.Main.ChangeLanageNotice", () => GameManager.Instance.RestartGame(), okText: "I18N:core.ui.Restart");
      else
        GameUIManager.Instance.BackPreviusPage();
    }
  }
}