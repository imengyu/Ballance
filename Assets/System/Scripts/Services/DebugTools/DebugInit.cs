
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.UI.Core;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.DebugTools {

  class DebugInit {
    private static Window GlobalDebugWindow = null;
    private static RectTransform GameDebugStatsArea = null;
    private static int F12KeyListen = 0;

    public static void InitSystemDebug() {
      var SystemPackage = GamePackage.GetSystemPackage();
      var GameUIManager = GameManager.Instance.GetSystemService<GameUIManager>();

      //创建Graphy
      var GameGraphy = GameObject.Instantiate(GameStaticResourcesPool.FindStaticPrefabs("PrefabGraphy"));
      GameGraphy.name = "GameGraphy";

      //创建窗口
      var DebugWindow = GameStaticResourcesPool.FindStaticPrefabs("DebugWindow");
      GlobalDebugWindow = GameUIManager.CreateWindow("Console", CloneUtils.CloneNewObjectWithParent(DebugWindow, GameManager.Instance.GameCanvas, "DebugWindow").transform as RectTransform, false, 9, -140, 800, 600);
      GlobalDebugWindow.CloseAsHide = true;
      GlobalDebugWindow.WindowType = WindowType.TopWindow;
      GlobalDebugWindow.gameObject.tag = "DebugWindow";
      GlobalDebugWindow.MinSize = new Vector2(300, 200);

      //创建输出窗口
      GameDebugStatsArea = GameUIManager.InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("GameDebugStats"), "GameDebugStats", true);
      
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
        UnityEngine.Object.Destroy(GlobalDebugWindow);
        GlobalDebugWindow = null;
      }
      if (GameDebugStatsArea != null) {
        UnityEngine.Object.Destroy(GameDebugStatsArea);
        GameDebugStatsArea = null;
      }
    }
  }

}