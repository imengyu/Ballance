using Ballance2.Game.GamePlay;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class GamePlayUIManager {
    public static void Create() {
      var package = GamePackage.GetSystemPackage();
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      var MessageCenter = MenuManager.MessageCenter;

      var PageGamePause = GameUIManager.RegisterPage("PageGamePause", "PageCommonInGame");
      var PageGameFail = GameUIManager.RegisterPage("PageGameFail", "PageCommonInGame");
      var PageGameWin = GameUIManager.RegisterPage("PageGameWin", "PageCommonInGame");
      var PageEndScore = GameUIManager.RegisterPage("PageEndScore", "PageTransparentInGame");
      var PageHighscoreEntry = GameUIManager.RegisterPage("PageHighscoreEntry", "PageCommonInGame");
      var PageGamePreviewPause = GameUIManager.RegisterPage("PageGamePreviewPause", "PageCommonInGame");
      
      PageGamePause.CreateContent(package);
      PageGamePause.CreateContent(package);
      PageGamePause.CanBack = false;
      PageGameWin.CreateContent(package);
      PageGameWin.CanBack = false;
      PageEndScore.CreateContent(package);
      PageEndScore.CanBack = false;
      PageEndScore.DelayShowDisplayAction = true;
      PageHighscoreEntry.CreateContent(package);
      PageHighscoreEntry.CanBack = false;
      PageGameFail.CreateContent(package);
      PageGameFail.CanBack = false;
      PageGamePreviewPause.CanBack = false;
      PageGamePreviewPause.CreateContent(package);

      MessageCenter.SubscribeEvent("BtnGameHomeClick", () => GamePlayManager.Instance.QuitLevel());
      MessageCenter.SubscribeEvent("BtnNextLevellick", () => GamePlayManager.Instance.NextLevel());
      MessageCenter.SubscribeEvent("BtnGameRestartClick", () => GameUIManager.GlobalConfirmWindow("I18N:core.ui.MainMenuRestartLevelTitle", () => {
        GameUIManager.HideCurrentPage();
        GamePlayManager.Instance.RestartLevel();
      }, okText: "I18N:core.ui.RestartLevel"));
      MessageCenter.SubscribeEvent("BtnGameWinRestartClick", () => GameUIManager.GlobalConfirmWindow("I18N:core.ui.MainMenuRestartLevelTitle", () => {
        GameUIManager.HideCurrentPage();
        GamePlayManager.Instance.RestartLevel();
      }, okText: "I18N:core.ui.RestartLevel"));
      MessageCenter.SubscribeEvent("BtnGameQuitClick", () => GameUIManager.GlobalConfirmWindow("I18N:core.ui.QuitToMainMenuTitle", () => {
        GameUIManager.HideCurrentPage();
        GamePlayManager.Instance.QuitLevel();
      }));
      MessageCenter.SubscribeEvent("BtnGamePreviewQuitClick", () => GameUIManager.GlobalConfirmWindow("I18N:core.ui.QuitToMainMenuTitle", () => {
        GameUIManager.HideCurrentPage();
        GamePlayManager.Instance.QuitLevel();
      }));
      MessageCenter.SubscribeEvent("BtnPauseSettingsClick", () => GameUIManager.GoPage("PageSettingsInGame"));
      MessageCenter.SubscribeEvent("BtnResumeClick", () => GamePlayManager.Instance.ResumeLevel());
      MessageCenter.SubscribeEvent("BtnPreviewResumeClick", () => GamePreviewManager.Instance.ResumeLevel());

      //高分默认数据
      var HighscoreEntryName = PageHighscoreEntry.Content.Find("InputField").GetComponent<TMP_InputField>();
      HighscoreEntryName.text = PlayerPrefs.GetString("LastEnterHighscoreEntry", "NAME");

      //过关之后的下一关按扭
      var ButtonNext = PageGameWin.Content.Find("ButtonNext").gameObject;
      PageGameWin.OnShow = (p) => {
        ButtonNext.SetActive(GamePlayManager.Instance.NextLevelName != "");
      };

      MessageCenter.SubscribeEvent("BtnHighscrollEnterClick", () => {
        PlayerPrefs.SetString("LastEnterHighscoreEntry", HighscoreEntryName.text);
        WinScoreUIControl.Instance.SaveHighscore(HighscoreEntryName.text);

        //当前有新的高分，跳转到高分页，否则直接进入下一关菜单
        GameUIManager.GoPage("PageGameWin");
        if (WinScoreUIControl.Instance.ThisTimeHasNewHighscore) {
          GameTimer.Delay(0.2f, () => {
            GameUIManager.GoPage("PageHighscore");
            //高分页加载当前关卡数据
            HighscoreUIControl.Instance.LoadLevelData(GamePlayManager.Instance.CurrentLevelName);
          });
        }
      });

      KeypadUIManager.AddKeypad("BaseCenter", package.GetPrefabAsset("KeypadCenter.prefab"), package.GetSpriteAsset("keypad_c.png"), I18N.Tr("core.ui.SettingsControlKeypadBaseCenter"));
      KeypadUIManager.AddKeypad("BaseLeft", package.GetPrefabAsset("KeypadLeft.prefab"), package.GetSpriteAsset("keypad_l.png"), I18N.Tr("core.ui.SettingsControlKeypadBaseLeft"));
      KeypadUIManager.AddKeypad("BaseRight", package.GetPrefabAsset("KeypadRight.prefab"), package.GetSpriteAsset("keypad_r.png"), I18N.Tr("core.ui.SettingsControlKeypadBaseRight"));
      KeypadUIManager.AddKeypad("BaseJoyLeft", package.GetPrefabAsset("KeypadJoyLeft.prefab"), package.GetSpriteAsset("keypad_joy_l.png"), I18N.Tr("core.ui.SettingsControlKeypadBaseJoyLeft"));
      KeypadUIManager.AddKeypad("BaseJoyRight", package.GetPrefabAsset("KeypadJoyRight.prefab"), package.GetSpriteAsset("keypad_joy_r.png"), I18N.Tr("core.ui.SettingsControlKeypadBaseJoyRight"));
    }
  }
}