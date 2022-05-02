using Ballance2.Utils;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Entry;
using System.Collections.Generic;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystem.cs
* 
* 用途：
* 游戏的基础系统与入口管理。
* 此管理器用来管理基础系统初始化和一些基础服务。
* 与GameManager不同，GameManager管理的是上层的服务，而此服务管理的是基础服务。
*
* 作者：
* mengyu
*
*/

/*

此类用于为整个框架的初始化提供一个包装，具体的初始化代码抽离出来放在 
Assets\System\Scripts\Services\Init\GameSystemInit.cs 中。

此类还负责提供注册必要的系统服务功能，获取系统服务功能（同GameManager.GetSystemService）。

索引器通过调用 GameSystem.Init / GameSystem.PreInit 来启动框架。

*/

namespace Ballance2
{
  /// <summary>
  /// 基础系统
  /// </summary>
  public static class GameSystem
  {
    private const string TAG = "GameSystem";

    #region 系统入口

    /// <summary>
    /// 系统接管器回调
    /// </summary>
    /// <param name="act">当前操作</param>
    public delegate void SysHandler(int act);

    private static SysHandler sysHandler = null;

    public const int ACTION_INIT = 1;
    public const int ACTION_DESTROY = 2;
    public const int ACTION_FORCE_INT = 3;
    public const int ACTION_PRE_INT = 4;

    /// <summary>
    /// 注册系统接管器
    /// </summary>
    /// <param name="handler">系统接管器</param>
    public static void RegSysHandler(SysHandler handler)
    {
      if (sysHandler != null)
      {
        Log.E("GameSystemInit", "SysHandler already set ");
        GameErrorChecker.LastError = GameError.AccessDenined;
        return;
      }
      sysHandler = handler;
    }
    /// <summary>
    /// 退出程序
    /// </summary>
    public static void QuitPlayer()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    #endregion

    #region 系统服务

    //系统服务容器
    private static Dictionary<string, GameService> systemService = new Dictionary<string, GameService>();

    /// <summary>
    /// 注册系统服务
    /// </summary>
    /// <param name="name">服务名称</param>
    /// <param name="classObject">服务对象</param>
    /// <returns></returns>
    public static bool RegSystemService<T>() where T : GameService
    {
      GameObject newManager = CloneUtils.CreateEmptyObject("NewManagerObject");
      T manager = newManager.AddComponent<T>();
      newManager.name = manager.Name;

      if (systemService.ContainsKey(manager.Name))
      {
        GameErrorChecker.LastError = GameError.AlreadyRegistered;
        return false;
      }

      //init
      if (!manager.Initialize())
      {
        Log.E(TAG, "Service {0} init failed ! {1}({2})", manager.Name, GameErrorChecker.LastError, GameErrorChecker.GetLastErrorMessage());
        return false;
      }

      systemService.Add(manager.Name, manager);

      return true;
    }
    /// <summary>
    /// 取消注册系统服务
    /// </summary>
    /// <param name="name">服务</param>
    /// <returns></returns>
    public static bool UnRegSystemService(string name)
    {
      if (!systemService.ContainsKey(name))
      {
        GameErrorChecker.LastError = GameError.NotRegister;
        return false;
      }

      //释放
      GameService gameService = systemService[name];
      gameService.Destroy();

      return systemService.Remove(name);
    }
    /// <summary>
    /// 获取系统服务
    /// </summary>
    /// <param name="name">服务名称</param>
    /// <returns></returns>
    public static GameService GetSystemService(string name)
    {
      if (!systemService.TryGetValue(name, out GameService o))
        GameErrorChecker.LastError = GameError.ClassNotFound;
      return o;
    }
    /// <summary>
    /// 获取系统服务
    /// </summary>
    /// <param name="name">服务名称</param>
    /// <returns></returns>
    public static T GetSystemService<T>() where T : GameService
    {
      return (T)GameSystem.GetSystemService(typeof(T).Name);
    }
    

    #endregion

    #region 调试提供

    /// <summary>
    /// 系统调试提供者
    /// </summary>
    public interface SysDebugProvider
    {
      bool StartDebug();
    }
    public delegate SysDebugProvider SysDebugProviderCheck();

    private static SysDebugProvider sysDebugProvider = null;
    private static SysDebugProviderCheck sysDebugProviderCheck = null;

    /// <summary>
    /// 注册系统级调试提供者
    /// </summary>
    public static void RegSysDebugProvider(SysDebugProviderCheck providerCheck)
    {
      sysDebugProviderCheck = providerCheck;
    }
    /// <summary>
    /// 启动系统级调试
    /// </summary>
    public static void StartRunDebugProvider()
    {
      if (sysDebugProviderCheck != null)
      {
        sysDebugProvider = sysDebugProviderCheck.Invoke();
        if (sysDebugProvider != null)
          sysDebugProvider.StartDebug();
      }
    }

    #endregion

    #region 初始化

    private static bool sysInit = false;

    /// <summary>
    /// 载入静态资源入口，必须调用，否则系统无法加载静态资源。
    /// </summary>
    /// <param name="gameEntry"></param>
    internal static void FillResEntry(GameStaticResEntry gameEntry) { gameStaticResEntryInstance = gameEntry; }

    private static GameStaticResEntry gameStaticResEntryInstance = null;

    /// <summary>
    /// 设置当前是否是重启模式。为true时Destroy完成后会重新启动。
    /// </summary>
    public static bool IsRestart = false;

    /// <summary>
    /// 预初始化。仅初始化I18N与设置。
    /// </summary>
    public static void PreInit()
    {
      if(!IsRestart)
        GameErrorChecker.Init();

      //Call game init
      if (sysHandler == null)
      {
        Log.D(TAG, "Not found SysHandler, did you call RegSysHandler first?");
        GameErrorChecker.ThrowGameError(GameError.ConfigueNotRight, null);
        return;
      }        
      //Call init
      sysHandler(ACTION_PRE_INT);
    }
    /// <summary>
    /// 初始化主入口
    /// </summary> 
    public static void Init()
    {
      if (!sysInit)
      {
        sysInit = true;

        try {
          //Init system
          Ballance2.Utils.UnityLogCatcher.Init();
          GameStaticResourcesPool.InitStaticPrefab(gameStaticResEntryInstance.GamePrefab, gameStaticResEntryInstance.GameAssets);

          //Call game init
          if (sysHandler == null)
          {
            Log.D(TAG, "Not found SysHandler, did you call RegSysHandler first?");
            GameErrorChecker.ThrowGameError(GameError.ConfigueNotRight, null);
            return;
          }        
          //Call init
          sysHandler(ACTION_INIT);

          Log.D(TAG, "System init ok");
        } catch(System.Exception e)  {
          GameErrorChecker.ThrowGameError(GameError.ExecutionFailed, "System init failed: \n" + e.ToString());
        }
      }
      else
      {
        Log.D(TAG, "System already init ok");
      }
    }
    /// <summary>
    /// 消毁
    /// </summary>
    public static void Destroy()
    {
      if (sysInit)
      {
        sysInit = false;

        Log.D(TAG, "System destroy");

        sysHandler?.Invoke(ACTION_DESTROY);

        //Destroy system service
        List<string> serviceNames = new List<string>(systemService.Keys);
        for (int i = serviceNames.Count - 1; i >= 0; i--)
        {
          try
          {
            systemService[serviceNames[i]].Destroy();
          }
          catch (System.Exception e)
          {
            UnityEngine.Debug.LogError("Exception when destroy service " + serviceNames[i] + "," + e.ToString());
          }
        }
        serviceNames.Clear();
        systemService.Clear();

        Ballance2.Utils.UnityLogCatcher.Destroy();
        GameErrorChecker.Destroy();

        if (IsRestart)
        {
          //Restart game
          #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
          System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
          #endif
        }
        QuitPlayer();
      }
    }
    /// <summary>
    /// 强制停止游戏
    /// </summary>
    public static void ForceInterruptGame()
    {
      sysHandler?.Invoke(ACTION_FORCE_INT);
    }

    #endregion
  }
}
