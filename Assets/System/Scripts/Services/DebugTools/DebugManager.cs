using Ballance2.Game.GamePlay;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using UnityEngine;
using static InputSystemUtil;

/*
* Copyright(c) 2022  mengyu
*
* 模块名：     
* DebugManager.cs
* 
* 用途：
* 调试管理器，复制管理调试控制台，输出等等。
*
* 作者：
* mengyu
*/

namespace Ballance2.DebugTools
{
  class DebugManager : MonoBehaviour
  {
    public static DebugManager Instance { get; private set; }

    private static RectTransform GlobalDebugConsole = null;
    private static DebugConsole DebugConsole = null;
    private static RectTransform GameDebugStatsArea = null;
    private static RectTransform GameDebugFloatButton = null;
    private static BindInputActionButtonHolder F12KeyListen;

    public static void Init() 
    {
      var SystemPackage = GamePackage.GetSystemPackage();
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      
      //创建控制台
      GlobalDebugConsole = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugConsole"), "GameDebugConsole", true);
      DebugConsole = GlobalDebugConsole.GetComponent<DebugConsole>();
      //创建输出窗口
      GameDebugStatsArea = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugStats"), "GameDebugStats", true);
      //显示FPS
      var GameFpsStat = GameUIManager.Instance.UIRoot.transform.Find("GameFpsStat");
      GameFpsStat.gameObject.SetActive(true);

#if UNITY_ANDROID || UNITY_IOS
      //创建一个按扭方便手机上打开调试窗口
      GameDebugFloatButton = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugFloatButton"), "GameDebugFloatButton", true);
      GameDebugFloatButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
        SwitchConsoleVisible();
      });
#endif

      GlobalDebugConsole.gameObject.SetActive(false);

      GameManager.Instance.GameDebugCommandServer.RegisterCommand("quit-dev", (keyword, fullCmd, argsCount, args) => {
        Destroy();
        GameManager.Instance.GameSettings.SetBool("DebugMode", false);
        GameManager.Instance.RestartGame();
        return true;
      }, 0, "quit-dev > 退出开发者模式");

      //F12 打开调试窗口
      F12KeyListen = ControlManager.Instance.KeyBoardTestConsole.BindInputActionButton((down) => {
        if(down) SwitchConsoleVisible();
      });
    }

    internal static void SwitchConsoleVisible() {
      if (GlobalDebugConsole.gameObject.activeSelf) {
        GlobalDebugConsole.gameObject.SetActive(false);
        GameDebugStatsArea.gameObject.SetActive(true);
      } else {
        GlobalDebugConsole.gameObject.SetActive(true);
        GameDebugStatsArea.gameObject.SetActive(false);
        DebugConsole.OnConsoleShow();
      }
    }

    public static void Destroy() 
    {
      if (F12KeyListen != null)
      {
        F12KeyListen.Delete();
        F12KeyListen = null;
      }
      if (GlobalDebugConsole != null) {
        Destroy(GlobalDebugConsole.gameObject);
        GlobalDebugConsole = null;
      }
      if (GameDebugFloatButton != null) {
        Destroy(GameDebugFloatButton.gameObject);
        GameDebugFloatButton = null;
      }
      if (GameDebugStatsArea != null) {
        Destroy(GameDebugStatsArea.gameObject);
        GameDebugStatsArea = null;
      }
    }
  }
}
