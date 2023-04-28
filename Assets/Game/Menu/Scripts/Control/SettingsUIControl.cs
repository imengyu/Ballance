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
      var GameUIManager = Ballance2.Services.GameUIManager.Instance;
      var MessageCenter = MenuManager.MessageCenter;
      var package = GamePackage.GetSystemPackage();

      var PageSettings = GameUIManager.RegisterPage("PageSettings", "PageCommon");
      var PageSettingsInGame = GameUIManager.RegisterPage("PageSettingsInGame", "PageCommon");
      var PageSettingsAudio = GameUIManager.RegisterPage("PageSettingsAudio", "PageCommon");
      var PageSettingsControls = GameUIManager.RegisterPage("PageSettingsControls", "PageCommon");
      var PageSettingsControlsMobile = GameUIManager.RegisterPage("PageSettingsControlsMobile", "PageCommon");
      var PageSettingsGraphics = GameUIManager.RegisterPage("PageSettingsGraphics", "PageCommon");
      var PageLanguage = GameUIManager.RegisterPage("PageLanguage", "PageCommon");
      var PageApplyLangDialog = GameUIManager.RegisterPage("PageApplyLangDialog", "PageCommon");
      var PagePackageManager = GameUIManager.RegisterPage("PagePackageManager", "PageFull");
      var PagePackageManagerInfo = GameUIManager.RegisterPage("PagePackageManagerInfo", "PageFull");
      var PagePackageManagerTrust = GameUIManager.RegisterPage("PagePackageManagerTrust", "PageFull");

      PageSettings.CreateContent(package);
      PageSettingsAudio.CreateContent(package);
      PageSettingsControls.CreateContent(package);
      PageSettingsControlsMobile.CreateContent(package);
      PageSettingsGraphics.CreateContent(package);
      PageSettingsInGame.CreateContent(package);
      PageLanguage.CreateContent(package);
      PageApplyLangDialog.CreateContent(package);
      PagePackageManager.CreateContent(package);
      PagePackageManagerInfo.CreateContent(package);
      PagePackageManagerTrust.CreateContent(package);

      GameTimer.Delay(1.0f, () => {
        BindSettingsUI(MessageCenter);
      });
    }

    private class QualitiesItemData {
      public string text;
      public int level;
    }

    private static void BindSettingsUI(GameUIMessageCenter MessageCenter) {
      var GameSettings = GameSettingsManager.GetSettings("core");
      var GameUIManager = Ballance2.Services.GameUIManager.Instance;

      //设置数据绑定
      //声音
      var updateVoiceMain = MessageCenter.SubscribeValueBinder("VoiceMain", (val) => {
        GameSettings.SetFloat("voice.main", (float)val);
        GameSettings.NotifySettingsUpdate("voice");
      });
      var updateVoiceUI = MessageCenter.SubscribeValueBinder("VoiceUI", (val) => {
        GameSettings.SetFloat("voice.ui", (float)val);
        GameSettings.NotifySettingsUpdate("voice");
      });
      var updateVioceBackground = MessageCenter.SubscribeValueBinder("VioceBackground", (val) => {
        GameSettings.SetFloat("voice.background", (float)val);
        GameSettings.NotifySettingsUpdate("voice");
      });
      //视频
      var updateGrResolution = MessageCenter.SubscribeValueBinder("GrResolution", (val) => {
        GameSettings.SetInt("video.resolution", (int)(float)val);
      });
      var updateGrQuality = MessageCenter.SubscribeValueBinder("GrQuality", (val) => {
        GameSettings.SetInt("video.quality", (int)(float)val);
      });
      var updateGrFullScreen = MessageCenter.SubscribeValueBinder("GrFullScreen", (val) => {
        GameSettings.SetBool("video.fullScreen", (bool)val);
      });
      var updateGrCloud = MessageCenter.SubscribeValueBinder("GrCloud", (val) => {
        GameSettings.SetBool("video.cloud", (bool)val);
      });
      var updateGrVSync = MessageCenter.SubscribeValueBinder("GrVSync", (val) => {
        GameSettings.SetInt("video.vsync", (bool)val ? 1 : 0 );
      });
      //控制
      var updateControlKeyUp = MessageCenter.SubscribeValueBinder("ControlKeyUp", (val) => {
        GameSettings.SetInt("control.key.front", (int)val);
      });
      var updateControlKeyDown = MessageCenter.SubscribeValueBinder("ControlKeyDown", (val) => {
        GameSettings.SetInt("control.key.back", (int)val);
      });
      var updateControlKeyLeft = MessageCenter.SubscribeValueBinder("ControlKeyLeft", (val) => {
        GameSettings.SetInt("control.key.left", (int)val);
      });
      var updateControlKeyRight = MessageCenter.SubscribeValueBinder("ControlKeyRight", (val) => {
        GameSettings.SetInt("control.key.right", (int)val);
      });
      var updateControlKeyOverlookCam = MessageCenter.SubscribeValueBinder("ControlKeyOverlookCam", (val) => {
        GameSettings.SetInt("control.key.up_cam", (int)val);
      });
      var updateControlKeyRoateCam = MessageCenter.SubscribeValueBinder("ControlKeyRoateCam", (val) => {
        GameSettings.SetInt("control.key.roate", (int)val);
      });
      var updateControlReverse = MessageCenter.SubscribeValueBinder("ControlReverse", (val) => {
        GameSettings.SetBool("control.reverse", (bool)val);
      });
      var updateControlUISize = MessageCenter.SubscribeValueBinder("ControlUISize", (val) => {
        GameSettings.SetFloat("control.key.size", (float)val);
        GameSettings.NotifySettingsUpdate("control");
      });

      Resolution[] resolutions = null;
      QualitiesItemData[] qualities = null;

      //设置数据加载
      MessageCenter.SubscribeEvent("BtnSettingsGraphicsClick", () => {
        if (updateGrFullScreen != null)
          updateGrFullScreen.Invoke(GameSettings.GetBool("video.fullScreen", Screen.fullScreen));
        if (updateGrCloud != null)
          updateGrCloud.Invoke(GameSettings.GetBool("video.cloud", true));
        if (updateGrVSync != null)
          updateGrVSync.Invoke(GameSettings.GetInt("video.vsync", QualitySettings.vSyncCount) >= 1 ? true : false);
        
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
            GrResolution.AddOption($"{resolution.width}x{resolution.height}@{resolution.refreshRate}");
            if (
              updateGrResolution != null
              && currentResolution.width == resolution.width 
              && currentResolution.height == resolution.height
              && currentResolution.refreshRate == resolution.refreshRate
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
        if (updateControlKeyUp != null)
          updateControlKeyUp.Invoke(GameSettings.GetInt("control.key.front", (int)KeyCode.UpArrow));
        if (updateControlKeyDown != null)
          updateControlKeyDown.Invoke(GameSettings.GetInt("control.key.back", (int)KeyCode.DownArrow));
        if (updateControlKeyLeft != null)
          updateControlKeyLeft.Invoke(GameSettings.GetInt("control.key.left", (int)KeyCode.LeftArrow));
        if (updateControlKeyRight != null)
          updateControlKeyRight.Invoke(GameSettings.GetInt("control.key.right", (int)KeyCode.RightArrow));
        if (updateControlKeyOverlookCam != null)
          updateControlKeyOverlookCam.Invoke(GameSettings.GetInt("control.key.up_cam", (int)KeyCode.Space));
        if (updateControlKeyRoateCam != null)
          updateControlKeyRoateCam.Invoke(GameSettings.GetInt("control.key.roate", (int)KeyCode.LeftShift));
        if (updateControlReverse != null) 
          updateControlReverse.Invoke(GameSettings.GetBool("control.reverse", false));
        if (updateControlUISize != null) 
          updateControlUISize.Invoke(GameSettings.GetFloat("control.key.size", 80));
        
        #if UNITY_IOS || UNITY_ANDROID
          GameUIManager.GoPage("PageSettingsControlsMobile");
        #else
          GameUIManager.GoPage("PageSettingsControls") ;
        #endif
      });
      MessageCenter.SubscribeEvent("BtnSettingsAudioClick", () => {
        if (updateVioceBackground != null)
          updateVioceBackground.Invoke(GameSettings.GetFloat("voice.background", 20));
        if (updateVoiceUI != null)
          updateVoiceUI.Invoke(GameSettings.GetFloat("voice.ui", 80));
        if (updateVoiceMain != null)
          updateVoiceMain.Invoke(GameSettings.GetFloat("voice.main", 100));
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
        GameSettings.NotifySettingsUpdate("video");
        GameUIManager.BackPreviusPage();
      });
      MessageCenter.SubscribeEvent("BtnSettingsControlsBackClick", () => {
        GameSettings.NotifySettingsUpdate("control");
        GameUIManager.BackPreviusPage();
      });
      MessageCenter.SubscribeEvent("BtnSettingsAudioBackClick", () => {
        GameSettings.NotifySettingsUpdate("voice");
        GameUIManager.BackPreviusPage();
      });

      //重启游戏
      MessageCenter.SubscribeEvent("BtnRestartGameClick", () => { GameManager.Instance.RestartGame(); });
    }

    private static void ApplyLanguage(SystemLanguage language) {
      GameSettingsManager.GetSettings("core").SetInt("language", (int)language);
      if (I18NProvider.GetCurrentLanguage() != language)
        GameUIManager.Instance.GoPage("PageApplyLangDialog");
      else
        GameUIManager.Instance.BackPreviusPage();
    }
  }
}