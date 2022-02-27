using Ballance2.Base;
using Ballance2.Entry;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using BallancePhysics;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystemInit.cs
* 
* 用途：
* 框架上层初始化
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Services.Init
{

  /// <summary>
  /// 上层初始化器
  /// </summary>
  class GameSystemInit : MonoBehaviour
  {
    internal static GameSystem.SysHandler GetSysHandler()
    {
      return SysHandler;
    }
    internal static void FillStartParameters(GameEntry gameEntry)
    {
      gameEntryInstance = gameEntry;
    }

    private static GameEntry gameEntryInstance = null;
    private static GameManager gameManagerInstance = null;
    private static readonly string TAG = "GameSystemInit";

    /// <summary>
    /// 处理System的消息
    /// </summary>
    /// <param name="act"></param>
    private static void SysHandler(int act)
    {
      if (act == GameSystem.ACTION_INIT)
      {
        //初始化设置
        GameSettingsManager.Init();
        
        GameObject newGameManager = CloneUtils.CreateEmptyObjectWithParent(gameEntryInstance.transform);
        newGameManager.name = "GameSystemInit";
        newGameManager.AddComponent<GameSystemInit>();

        GamePackageManager.PreRegInternalPackage();

        //Init system services
        GameSystem.RegSystemService<GameMediator>();

        GameManager.GameMediator = (GameMediator)GameSystem.GetSystemService("GameMediator");
        GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_BASE_INIT_FINISHED, "DebuggerHandler", (evtName, param) => {
          GameSystem.StartRunDebugProvider();
          return false;
        });

        //Init base services
        GameSystem.RegSystemService<GameManager>();
        GameSystem.RegSystemService<GamePackageManager>();
        GameSystem.RegSystemService<GameUIManager>();
        GameSystem.RegSystemService<GameTimeMachine>();
        GameSystem.RegSystemService<GamePoolManager>();
        GameSystem.RegSystemService<GameSoundManager>();

  #if !UNITY_EDITOR
        //初始化物理引擎
        PhysicsApi.SecretKey = "666dccad4ae697b45aac145f18f49c5b";
        PhysicsSystemInit.DoInit();
  #endif
      }
      else if (act == GameSystem.ACTION_DESTROY)
      {
        if (gameManagerInstance != null)
        {
          Destroy(gameManagerInstance);
          gameManagerInstance = null;
        }
        
        GameManager.GameMediator = null;

        GamePackageManager.ReleaseInternalPackage();
        //释放其他组件
        I18NProvider.ClearAllLanguageResources();
        GameSettingsManager.Destroy();
        GameStaticResourcesPool.ReleaseAll();
        LuaService.Lua.LuaGlobalApi.Destroy();

#if !UNITY_EDITOR
        //释放物理引擎
        PhysicsSystemInit.DoDestroy();
#endif
      }
      else if (act == GameSystem.ACTION_FORCE_INT)
      {
        Log.D(TAG, "Force interrupt game");

        if (gameManagerInstance != null)
        {
          gameManagerInstance.ClearScense();
        }
      }
      else if (act == GameSystem.ACTION_PRE_INT)
      {
        //初始化设置
        GameSettingsManager.Init();
        //初始化I18N 和系统字符串资源
        I18NProvider.SetCurrentLanguage((SystemLanguage)GameSettingsManager.GetSettings("core").GetInt("language", (int)Application.systemLanguage));
        I18NProvider.LoadLanguageResources(Resources.Load<TextAsset>("StaticLangResource").text);
      }
    }
  }
}
