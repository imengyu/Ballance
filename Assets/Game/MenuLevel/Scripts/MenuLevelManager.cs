using Ballance2.Base;
using Ballance2;
using Ballance2.Package;
using Ballance2.Services;
using UnityEngine;

namespace Ballance2.Game {

  /// <summary>
  /// MenuLevel 管理
  /// </summary>
  public class MenuLevelManager : MonoBehaviour {

    const string TAG = "MenuLevelManager";

    private static GameObject GameMenuLevel = null;

    //进入场景
    private static void OnEnterMenuLevel(GamePackage thisGamePackage) {

      Log.D(TAG, "Into MenuLevel");

      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      GameManager.Instance.SetGameBaseCameraVisible(false);

      if (GameMenuLevel == null)
        GameMenuLevel = GameManager.Instance.InstancePrefab(thisGamePackage.GetPrefabAsset("GameMenuLevel.prefab"), "GameMenuLevel");

      if (!GameMenuLevel.activeSelf)
        GameMenuLevel.SetActive(true);

      GameUIManager.GoPage("PageMain");
      GameUIManager.MaskBlackFadeOut(1);
    }
    private static void OnQuitMenuLevel() {
      Log.D(TAG, "Quit MenuLevel");
      
      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      GameUIManager.CloseAllPage();

      if (GameMenuLevel != null) 
        GameMenuLevel.SetActive(false);
      
      GameManager.Instance.SetGameBaseCameraVisible(true);
    }

    private static GameEventHandler enterEvent = null;
    private static GameEventHandler quitEvent = null;

    public static void Init(GamePackage package) {
      //注册场景切换
      enterEvent = GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, TAG, (evtName, param) => {
        var scense = param[0];
        if(scense.ToString() == "MenuLevel") 
          OnEnterMenuLevel(package);
        return false;
      }); 
      quitEvent = GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, TAG, (evtName, param) => {
        var scense = param[0];
        if (scense.ToString() == "MenuLevel") 
          OnQuitMenuLevel();
        return false;
      });
    }
    public static void Destroy(GamePackage package) {
      if (GameMenuLevel != null) {
        GameObject.Destroy(GameMenuLevel);
        GameMenuLevel = null;
      }    
      if (enterEvent != null) {
        enterEvent.UnRegister();
        enterEvent = null;
      }
      if (quitEvent != null) {
        quitEvent.UnRegister();
        quitEvent = null;
      }
    }
  }
}