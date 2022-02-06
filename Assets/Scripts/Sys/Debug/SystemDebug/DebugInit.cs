using UnityEngine;
using Ballance2.Sys;
using Ballance2.Sys.Bridge.Handler;
using Ballance2.Sys.UI;
using Ballance2.Sys.Package;
using Ballance2.Sys.Utils;
using Ballance2.Sys.Res;
using Ballance2;

public class DebugInit {

  private static Window GlobalDebugWindow = null;
  private static int F12KeyListen = 0;

  public static void InitSystemDebug() {

    var SystemPackage = GamePackage.GetSystemPackage();
    var GameUIManager = GameManager.Instance.GetSystemService<Ballance2.Sys.Services.GameUIManager>();

    //创建窗口
    var DebugWindow = GameStaticResourcesPool.FindStaticPrefabs("DebugWindow");
    GlobalDebugWindow = GameUIManager.CreateWindow("Console", CloneUtils.CloneNewObjectWithParent(DebugWindow, GameManager.Instance.GameCanvas, "DebugWindow").transform as RectTransform, false, 9, -140, 600, 400);
    GlobalDebugWindow.CloseAsHide = true;
    GlobalDebugWindow.WindowType = WindowType.TopWindow;
    GlobalDebugWindow.gameObject.tag = "DebugWindow";
    
    //F12 打开调试窗口
    F12KeyListen = GameUIManager.ListenKey(KeyCode.F12, (key, down) => {
      if(down) {       
        if (GlobalDebugWindow.GetVisible()) GlobalDebugWindow.Hide();
        else GlobalDebugWindow.Show();
      }
    });
  }

  public static void UnInitSystemDebug() {
    var GameUIManager = GameManager.Instance.GetSystemService<Ballance2.Sys.Services.GameUIManager>();
    GameUIManager.DeleteKeyListen(F12KeyListen);

    if (GlobalDebugWindow != null) UnityEngine.Object.Destroy(GlobalDebugWindow);
  }
}