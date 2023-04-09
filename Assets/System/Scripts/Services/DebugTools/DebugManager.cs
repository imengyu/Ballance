using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using UnityEngine;

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

    internal static GameObject GameGraphy = null;
    private static RectTransform GlobalDebugConsole = null;
    private static DebugConsole DebugConsole = null;
    private static RectTransform GameDebugStatsArea = null;
    private static RectTransform GameDebugFloatButton = null;
    private static int F12KeyListen = 0;
    private static int ESCKeyListen = 0;

    public static void Init() 
    {
      var SystemPackage = GamePackage.GetSystemPackage();
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();

      //创建Graphy
      GameGraphy = GameObject.Instantiate(GameStaticResourcesPool.FindStaticPrefabs("PrefabGraphy"));
      GameGraphy.name = "GameGraphy";

      //创建控制台
      GlobalDebugConsole = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugConsole"), "GameDebugConsole", true);
      DebugConsole = GlobalDebugConsole.GetComponent<DebugConsole>();
      //创建输出窗口
      GameDebugStatsArea = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugStats"), "GameDebugStats", true);
      
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
        return true;
      }, 0, "quit-dev > 退出开发者模式");

      //F12 打开调试窗口
      F12KeyListen = GameUIManager.ListenKey(KeyCode.F12, (key, down) => {
        if(down) SwitchConsoleVisible();
      });
      //Esc 关闭调试窗口
      ESCKeyListen = GameUIManager.ListenKey(KeyCode.Escape, (key, down) => {
        if(down && GlobalDebugConsole.gameObject.activeSelf) 
          SwitchConsoleVisible();
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
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      GameUIManager.DeleteKeyListen(F12KeyListen);

      if (GlobalDebugConsole != null) {
        UnityEngine.Object.Destroy(GlobalDebugConsole.gameObject);
        GlobalDebugConsole = null;
      }
      if (GameDebugFloatButton != null) {
        UnityEngine.Object.Destroy(GameDebugFloatButton.gameObject);
        GameDebugFloatButton = null;
      }
      if (GameDebugStatsArea != null) {
        UnityEngine.Object.Destroy(GameDebugStatsArea.gameObject);
        GameDebugStatsArea = null;
      }
    }
  }
}
