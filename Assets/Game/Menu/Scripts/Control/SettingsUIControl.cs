using Ballance2.Game;
using Ballance2.Game.GamePlay;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Controls;
using UnityEngine;

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

      PageSettingsControls.OnShow += (options) => {
        var control = ControlManager.Instance;
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

        control.LoadActionSettings(false);

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
        ButtonKeyChooseControlControllerForward.BindingIndex = 0;
        ButtonKeyChooseControlControllerBack.BindAction = control.ControllerActionMove;
        ButtonKeyChooseControlControllerBack.BindingIndex = 1;
        ButtonKeyChooseControlControllerLeft.BindAction = control.ControllerActionMove;
        ButtonKeyChooseControlControllerLeft.BindingIndex = 2;
        ButtonKeyChooseControlControllerRight.BindAction = control.ControllerActionMove;
        ButtonKeyChooseControlControllerRight.BindingIndex = 3;
        ButtonKeyChooseControlControllerDown.BindAction = control.ControllerActionFly;
        ButtonKeyChooseControlControllerDown.BindingIndex = 0;
        ButtonKeyChooseControlControllerUp.BindAction = control.ControllerActionFly;
        ButtonKeyChooseControlControllerUp.BindingIndex = 1;
        ButtonKeyChooseControlControllerOverlookCam.BindAction = control.ControllerActionOverlook;
        ButtonKeyChooseControlControllerRoateCamLeft.BindAction = control.ControllerActionRotateCamLeft;
        ButtonKeyChooseControlControllerRoateCamRight.BindAction = control.ControllerActionRotateCamRight;

        ButtonKeyChooseScreenShort.BindAction = control.ScreenShort;
        ButtonKeyChooseToggleFPS.BindAction = control.ToggleFPS;
        ButtonKeyChooseToggleView.BindAction = control.ToggleView;
      };
      PageBindKey.OnShow += (options) => {
        var keyName = options["keyName"];
        var keyCurrent = options["keyCurrent"];
        var Text = PageBindKey.Content.Find("Panel/Text").GetComponent<UIText>();
        Text.text = I18N.TrF("core.ui.ControllerBind", keyName, keyCurrent);
      };

      GameTimer.Delay(1.0f, () => {
        BindSettingsUI(MessageCenter);
      });
    }

    private class QualitiesItemData {
      public string text;
      public int level;
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
          resolutions = Screen.resolutions;
          var GrResolution = MessageCenter.GetComponentInstance("GrResolution").GetComponent<Updown>();
          var currentResolution = Screen.currentResolution;
          for (int i = 0; i < resolutions.Length; i++)
          {
            var resolution = resolutions[i];
            GrResolution.AddOption($"{resolution.width}x{resolution.height}@{resolution.refreshRateRatio.value}");
            if (
              updateGrResolution != null
              && currentResolution.width == resolution.width 
              && currentResolution.height == resolution.height
              && currentResolution.refreshRateRatio.value == resolution.refreshRateRatio.value
            ) 
              updateGrResolution.Invoke(i - 1);
          }
        }
        if (qualities == null) 
        {
          qualities = new QualitiesItemData[] {
            new QualitiesItemData { level = 0, text = I18N.Tr("core.ui.SettingsQualityVeryLow") },
            new QualitiesItemData { level = 1, text = I18N.Tr("core.ui.SettingsQualityLow") },
            new QualitiesItemData { level = 2, text = I18N.Tr("core.ui.SettingsQualityMedium") },
            new QualitiesItemData { level = 3, text = I18N.Tr("core.ui.SettingsQualityHigh") },
            new QualitiesItemData { level = 4, text = I18N.Tr("core.ui.SettingsQualityVeryHigh") },
            new QualitiesItemData { level = 5, text = I18N.Tr("core.ui.SettingsQualityUltra") },
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

      //语言
      MessageCenter.SubscribeEvent("BtnSettingsLanguageClick", () => { GameUIManager.GoPage("PageLanguage"); });
      MessageCenter.SubscribeEvent("BtnChineseSimplifiedClick", () => { ApplyLanguage(SystemLanguage.ChineseSimplified); });
      MessageCenter.SubscribeEvent("BtnChineseTraditionalClick", () => { ApplyLanguage(SystemLanguage.ChineseTraditional); });
      MessageCenter.SubscribeEvent("BtnEnglishClick", () => { ApplyLanguage(SystemLanguage.English); });

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

    private static void ApplyLanguage(SystemLanguage language) {
      GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME).SetInt("language", (int)language);
      if (I18NProvider.GetCurrentLanguage() != language)
        GameUIManager.Instance.GlobalConfirmWindow("I18N:core.ui.ChangeLanageNotice", () => GameManager.Instance.RestartGame(), okText: "I18N:core.ui.Restart");
      else
        GameUIManager.Instance.BackPreviusPage();
    }
  }
}