
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.UI.Core;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.DebugTools {

  class DebugInit {
    internal static GameObject GameGraphy = null;
    private static Window GlobalDebugWindow = null;
    private static RectTransform GameDebugStatsArea = null;
    private static RectTransform GameDebugFloatButton = null;
    private static int F12KeyListen = 0;

    public static void InitSystemDebug() {
      var SystemPackage = GamePackage.GetSystemPackage();
      var GameUIManager = GameManager.Instance.GetSystemService<GameUIManager>();

      //创建Graphy
      GameGraphy = GameObject.Instantiate(GameStaticResourcesPool.FindStaticPrefabs("PrefabGraphy"));
      GameGraphy.name = "GameGraphy";

      //创建窗口
      var DebugWindow = GameStaticResourcesPool.FindStaticPrefabs("DebugWindow");
      GlobalDebugWindow = GameUIManager.CreateWindow("Console", CloneUtils.CloneNewObjectWithParent(DebugWindow, GameManager.Instance.GameCanvas, "DebugWindow").transform as RectTransform, false, 9, -140, 800, 600);
      GlobalDebugWindow.CloseAsHide = true;
      GlobalDebugWindow.Icon = GameStaticResourcesPool.FindStaticAssets<Sprite>("IconDebug");
      GlobalDebugWindow.WindowType = WindowType.TopWindow;
      GlobalDebugWindow.gameObject.tag = "DebugWindow";
      GlobalDebugWindow.MinSize = new Vector2(300, 200);
      GlobalDebugWindow.Size = new Vector2(470, 350);

      //创建输出窗口
      GameDebugStatsArea = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugStats"), "GameDebugStats", true);
      
#if UNITY_ANDROID || UNITY_IOS
      //创建一个按扭方便手机上打开调试窗口
      GameDebugFloatButton = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugFloatButton"), "GameDebugFloatButton", true);
      GameDebugFloatButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
        if (GlobalDebugWindow.GetVisible()) GlobalDebugWindow.Hide();
        else GlobalDebugWindow.Show();
      });
#endif

      GameManager.Instance.GameDebugCommandServer.RegisterCommand("quit-dev", (keyword, fullCmd, argsCount, args) => {
        UnInitSystemDebug();
        GameManager.Instance.GameSettings.SetBool("DebugMode", false);
        GameUIManager.GlobalConfirmWindow("After exiting developer mode, you must restart the game to take effect", "Tip", () => {
          GameManager.Instance.RestartGame();
        }, () => {}, "Restart game");
        return true;
      }, 0, "quit-dev > 退出开发者模式");

      //F12 打开调试窗口
      F12KeyListen = GameUIManager.ListenKey(KeyCode.F12, (key, down) => {
        if(down) {       
          if (GlobalDebugWindow.GetVisible()) GlobalDebugWindow.Hide();
          else GlobalDebugWindow.Show();
        }
      });
    }
    public static void UnInitSystemDebug() {
      var GameUIManager = GameManager.Instance.GetSystemService<GameUIManager>();
      GameUIManager.DeleteKeyListen(F12KeyListen);

      if (GlobalDebugWindow != null) {
        GlobalDebugWindow.Close();
        GlobalDebugWindow = null;
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