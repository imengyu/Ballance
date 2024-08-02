using Ballance2.Base;
using Ballance2;
using Ballance2.Package;
using Ballance2.Services;
using UnityEngine;

namespace Ballance2.Game {

  /// <summary>
  /// Intro 管理
  /// </summary>
  public class IntroManager : MonoBehaviour {

    const string TAG = "IntroManager";

    private static RectTransform IntroUI = null;

    //进入Intro场景
    private static void OnEnterIntro(GamePackage thisGamePackage) {

      Log.D(TAG, "Into intro ui");

      var GameUIManager = GameManager.GetSystemService<GameUIManager>();
      var GameSoundManager = GameManager.GetSystemService<GameSoundManager>();

      if (IntroUI == null) {
        IntroUI = GameUIManager.InitViewToCanvas(thisGamePackage.GetPrefabAsset("IntroUI"), "IntroUI", true);
        IntroUI.SetAsFirstSibling();  
      }

      GameUIManager.MaskBlackFadeIn(0.3f);

      //进入音乐
      GameSoundManager.PlayFastVoice("core.sounds.music:Music_Theme_4_1.wav", GameSoundType.Background);

      GameTimer.Delay(5, () => {
        //黑色渐变进入
        GameUIManager.MaskBlackFadeIn(1);
        GameTimer.Delay(1, () => {
          //进入菜单
          GameManager.Instance.RequestEnterLogicScense("MenuLevel");
        });
      });
    }
    private static void OnQuitIntro() {
      Log.D(TAG, "Quit intro ui");
      if (IntroUI != null) {
        IntroUI.gameObject.SetActive(false);
      }
    }

    public static void Init(GamePackage package) {
      //注册场景切换
      GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, "Intro", (evtName, param) => {
        var scense = param[0];
        if(scense.ToString() == "Intro") 
          OnEnterIntro(package);
        return false;
      }); 
      GameManager.GameMediator.RegisterEventHandler(package, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, "Intro", (evtName, param) => {
        var scense = param[0];
        if (scense.ToString() == "Intro") 
          OnQuitIntro();
        return false;
      });
    }

    public static void Destroy(GamePackage package) {
      if (IntroUI != null) {
        GameObject.Destroy(IntroUI.gameObject);
        IntroUI = null;
      }
    }
  }
}