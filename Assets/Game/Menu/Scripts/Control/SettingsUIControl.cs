using Ballance2.Game;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Controls;
using System;
using System.Text;
using UnityEngine;

namespace Ballance2.Menu
{
    public class SettingsUIControl
    {
        public static void Create()
        {
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

            PageSettingsControls.OnShow += (options) =>
            {
                if (!GameManager.DebugMode)
                {
                    PageSettingsControls.Content.Find("ScrollViewVetical/Viewport/ContentView/ButtonKeyChooseControlKeyUp").gameObject.SetActive(false);
                    PageSettingsControls.Content.Find("ScrollViewVetical/Viewport/ContentView/ButtonKeyChooseControlKeyDown").gameObject.SetActive(false);
                }
            };

            GameTimer.Delay(1.0f, () =>
            {
                BindSettingsUI(MessageCenter);
            });
        }

        private class QualitiesItemData
        {
            public string text;
            public int level;
        }

        private static void BindSettingsUI(GameUIMessageCenter MessageCenter)
        {
            var GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
            var GameUIManager = Ballance2.Services.GameUIManager.Instance;

            //设置数据绑定
            //声音
            var updateVoiceMain = MessageCenter.SubscribeValueBinder("VoiceMain", (val) =>
            {
                GameSettings.SetFloat(SettingConstants.SettingsVoiceMain, (float)val);
                GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
            });
            var updateVoiceUI = MessageCenter.SubscribeValueBinder("VoiceUI", (val) =>
            {
                GameSettings.SetFloat(SettingConstants.SettingsVoiceUI, (float)val);
                GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
            });
            var updateVioceBackground = MessageCenter.SubscribeValueBinder("VioceBackground", (val) =>
            {
                GameSettings.SetFloat(SettingConstants.SettingsVoiceBackground, (float)val);
                GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
            });
            //视频
            var updateGrResolution = MessageCenter.SubscribeValueBinder("GrResolution", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsVideoResolution, Convert.ToInt32(val));
            });
            var updateGrQuality = MessageCenter.SubscribeValueBinder("GrQuality", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsVideoQuality, Convert.ToInt32(val));
            });
            var updateGrFullScreen = MessageCenter.SubscribeValueBinder("GrFullScreen", (val) =>
            {
                GameSettings.SetBool(SettingConstants.SettingsVideoFullScreen, (bool)val);
            });
            var updateGrCloud = MessageCenter.SubscribeValueBinder("GrCloud", (val) =>
            {
                GameSettings.SetBool(SettingConstants.SettingsVideoCloud, (bool)val);
            });
            var updateGrVSync = MessageCenter.SubscribeValueBinder("GrVSync", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsVideoVsync, (bool)val ? 1 : 0);
            });
            //控制
            var updateControlKeyForward = MessageCenter.SubscribeValueBinder("ControlKeyForward", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyFront, (int)val);
            });
            var updateControlKeyBack = MessageCenter.SubscribeValueBinder("ControlKeyBack", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyBack, (int)val);
            });
            var updateControlKeyUp = MessageCenter.SubscribeValueBinder("ControlKeyUp", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyUp, (int)val);
            });
            var updateControlKeyDown = MessageCenter.SubscribeValueBinder("ControlKeyDown", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyDown, (int)val);
            });
            var updateControlKeyLeft = MessageCenter.SubscribeValueBinder("ControlKeyLeft", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyLeft, (int)val);
            });
            var updateControlKeyRight = MessageCenter.SubscribeValueBinder("ControlKeyRight", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyRight, (int)val);
            });
            var updateControlKeyOverlookCam = MessageCenter.SubscribeValueBinder("ControlKeyOverlookCam", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyUpCam, (int)val);
            });
            var updateControlKeyRoateCam = MessageCenter.SubscribeValueBinder("ControlKeyRoateCam", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyRoate, (int)val);
            });
            var updateControlKeyRoateCam2 = MessageCenter.SubscribeValueBinder("ControlKeyRoateCam2", (val) =>
            {
                GameSettings.SetInt(SettingConstants.SettingsControlKeyRoate2, (int)val);
            });
            var updateControlReverse = MessageCenter.SubscribeValueBinder("ControlReverse", (val) =>
            {
                GameSettings.SetBool(SettingConstants.SettingsControlKeyReverse, (bool)val);
            });
            var updateControlUISize = MessageCenter.SubscribeValueBinder("ControlUISize", (val) =>
            {
                GameSettings.SetFloat(SettingConstants.SettingsControlKeySize, (float)val);
                GameSettings.NotifySettingsUpdate(SettingConstants.SettingsControl);
            });

            Resolution[] resolutions = null;
            QualitiesItemData[] qualities = null;

            //设置数据加载
            MessageCenter.SubscribeEvent("BtnSettingsGraphicsClick", () =>
            {
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
                    var resolutionWidth = currentResolution.width;
                    var resolutionHeight = currentResolution.height;
                    switch (Screen.orientation)
                    {
                        case ScreenOrientation.LandscapeLeft:
                        case ScreenOrientation.LandscapeRight:
                            var tmpInt = resolutionWidth;
                            resolutionWidth = resolutionHeight;
                            resolutionHeight = tmpInt;
                            break;
                    }
                    for (int i = 0; i < resolutions.Length; i++)
                    {
                        var resolution = resolutions[i];
                        GrResolution.AddOption($"{resolution.width}x{resolution.height}@{resolution.refreshRate}");
                        if (
                          updateGrResolution != null
                          && resolutionWidth == resolution.width
                          && resolutionHeight == resolution.height
                          && currentResolution.refreshRate == resolution.refreshRate
                        )
                            updateGrResolution.Invoke(i);
                    }
                }
                if (qualities == null)
                {
                    qualities = new QualitiesItemData[]
                    {
                        new QualitiesItemData { level = 0, text = I18N.Tr("core.ui.SettingsQualityVeryLow") },
                        new QualitiesItemData { level = 1, text = I18N.Tr("core.ui.SettingsQualityLow") },
                        new QualitiesItemData { level = 2, text = I18N.Tr("core.ui.SettingsQualityMedium") },
                        new QualitiesItemData { level = 3, text = I18N.Tr("core.ui.SettingsQualityHigh") },
                        new QualitiesItemData { level = 4, text = I18N.Tr("core.ui.SettingsQualityVeryHigh") },
                        new QualitiesItemData { level = 5, text = I18N.Tr("core.ui.SettingsQualityUltra") },
                    };
                    var GrQuality = MessageCenter.GetComponentInstance("GrQuality").GetComponent<Updown>();
                    var qualitiyCurrent = QualitySettings.GetQualityLevel();
                    for (int i = 0; i < qualities.Length; i++)
                    {
                        var qualitiy = qualities[i];
                        GrQuality.AddOption(qualitiy.text);
                        if (qualitiyCurrent == qualitiy.level && updateGrQuality != null)
                            updateGrQuality.Invoke(i);
                    }
                }
            });
            MessageCenter.SubscribeEvent("BtnSettingsControlsClick", () =>
            {
                if (updateControlKeyForward != null) updateControlKeyForward.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyFront));
                if (updateControlKeyBack != null) updateControlKeyBack.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyBack));
                if (updateControlKeyUp != null) updateControlKeyUp.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyUp));
                if (updateControlKeyDown != null) updateControlKeyDown.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyDown));
                if (updateControlKeyLeft != null) updateControlKeyLeft.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyLeft));
                if (updateControlKeyRight != null) updateControlKeyRight.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyRight));
                if (updateControlKeyOverlookCam != null) updateControlKeyOverlookCam.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyUpCam));
                if (updateControlKeyRoateCam != null) updateControlKeyRoateCam2.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyRoate));
                if (updateControlKeyRoateCam2 != null) updateControlKeyRoateCam2.Invoke(GameSettings.GetInt(SettingConstants.SettingsControlKeyRoate2));
                if (updateControlReverse != null) updateControlReverse.Invoke(GameSettings.GetBool(SettingConstants.SettingsControlKeyReverse));
                if (updateControlUISize != null) updateControlUISize.Invoke(GameSettings.GetFloat(SettingConstants.SettingsControlKeySize));

#if UNITY_IOS || UNITY_ANDROID
                GameUIManager.GoPage("PageSettingsControlsMobile");
#else
                GameUIManager.GoPage("PageSettingsControls") ;
#endif
            });
            MessageCenter.SubscribeEvent("BtnSettingsAudioClick", () =>
            {
                if (updateVioceBackground != null) updateVioceBackground.Invoke(GameSettings.GetFloat(SettingConstants.SettingsVoiceBackground));
                if (updateVoiceUI != null) updateVoiceUI.Invoke(GameSettings.GetFloat(SettingConstants.SettingsVoiceUI));
                if (updateVoiceMain != null) updateVoiceMain.Invoke(GameSettings.GetFloat(SettingConstants.SettingsVoiceMain));
                GameUIManager.GoPage("PageSettingsAudio");
            });
            MessageCenter.SubscribeEvent("BtnSettingsPackageClick", () =>
            {
                GameUIManager.GoPage("PagePackageManager");
            });

            //语言
            MessageCenter.SubscribeEvent("BtnSettingsLanguageClick", () => { GameUIManager.GoPage("PageLanguage"); });
            MessageCenter.SubscribeEvent("BtnChineseSimplifiedClick", () => { ApplyLanguage(SystemLanguage.ChineseSimplified); });
            MessageCenter.SubscribeEvent("BtnChineseTraditionalClick", () => { ApplyLanguage(SystemLanguage.ChineseTraditional); });
            MessageCenter.SubscribeEvent("BtnEnglishClick", () => { ApplyLanguage(SystemLanguage.English); });

            MessageCenter.SubscribeEvent("BtnLangBackClick", () =>
            {
                GameUIManager.BackPreviusPage();
                GameUIManager.BackPreviusPage();
            });

            //设置保存
            MessageCenter.SubscribeEvent("BtnSettingsGraphicsBackClick", () =>
            {
                GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVideo);
                GameUIManager.BackPreviusPage();
            });
            MessageCenter.SubscribeEvent("BtnSettingsControlsBackClick", () =>
            {
                GameSettings.NotifySettingsUpdate(SettingConstants.SettingsControl);
                GameUIManager.BackPreviusPage();
            });
            MessageCenter.SubscribeEvent("BtnSettingsAudioBackClick", () =>
            {
                GameSettings.NotifySettingsUpdate(SettingConstants.SettingsVoice);
                GameUIManager.BackPreviusPage();
            });

            //重启游戏
            MessageCenter.SubscribeEvent("BtnRestartGameClick", () => { GameManager.Instance.RestartGame(); });
        }

        private static void ApplyLanguage(SystemLanguage language)
        {
            GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME).SetInt("language", (int)language);
            if (I18NProvider.GetCurrentLanguage() != language)
                GameUIManager.Instance.GoPage("PageApplyLangDialog");
            else
                GameUIManager.Instance.BackPreviusPage();
        }
    }
}