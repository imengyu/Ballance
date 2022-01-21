using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ballance2.Base;
using Ballance2.Config;
using Ballance2.Config.Settings;
using Ballance2.DebugTools;
using Ballance2.Entry;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services.Debug;
using Ballance2.Services.LuaService;
using Ballance2.Utils;
using SLua;
using UnityEngine;
using UnityEngine.Profiling;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* GameManager.cs
* 
* 用途：
* 游戏底层主管理器，提供多个关键全局组件使用，负责管理游戏底层管理与控制.
* 它主要提供了全局事件、主lua虚拟机、初始化与退出功能，虚拟场景功能，并且提供了
* 一些全局方便使用的工具函数。
*
* 作者：
* mengyu
*/

namespace Ballance2.Services
{
  /// <summary>
  /// 游戏管理器
  /// </summary>
  [SLua.CustomLuaClass]
  public class GameManager : GameService
  {
    public GameManager() : base("GameManager") {}

    #region 全局关键组件变量

    /// <summary>
    /// 实例
    /// </summary>
    [LuaApiDescription("实例")]
    public static GameManager Instance { get; private set; }
    /// <summary>
    /// GameMediator 实例
    /// </summary>
    [LuaApiDescription("GameMediator 实例")]
    public static GameMediator GameMediator { get; internal set; }
    /// <summary>
    /// 系统设置实例
    /// </summary>
    [LuaApiDescription("系统设置实例")]
    public GameSettingsActuator GameSettings { get; private set; }
    /// <summary>
    /// 基础摄像机
    /// </summary>
    [LuaApiDescription("基础摄像机")]
    public Camera GameBaseCamera { get; private set; }
    /// <summary>
    /// 根Canvas
    /// </summary>
    [LuaApiDescription("根Canvas")]
    public RectTransform GameCanvas { get; internal set; }
    /// <summary>
    /// 主虚拟机
    /// </summary>
    [LuaApiDescription("主虚拟机")]
    public LuaSvr GameMainEnv { get; internal set; }
    /// <summary>
    /// 游戏全局Lua虚拟机
    /// </summary>
    [LuaApiDescription("游戏全局Lua虚拟机")]
    public LuaSvr.MainState GameMainLuaState { get; private set; }
    /// <summary>
    /// 调试命令控制器
    /// </summary>
    /// <value></value>
    [LuaApiDescription("调试命令控制器")]
    public GameDebugCommandServer GameDebugCommandServer { get; private set; }
    /// <summary>
    /// 根GameActionStore
    /// </summary>
    [LuaApiDescription("根GameActionStore")]
    public GameActionStore GameActionStore { get; internal set; }

    public static bool DebugMode { get; private set; }

    private readonly string TAG = "GameManager";

    #endregion

    #region 初始化

    public override bool Initialize()
    {
      Log.D(TAG, "Initialize");

      GameEntry gameEntryInstance = GameEntry.Instance;
      GameBaseCamera = gameEntryInstance.GameBaseCamera;
      GameCanvas = gameEntryInstance.GameCanvas;
      DebugMode = gameEntryInstance.DebugMode;
      GameDebugCommandServer = new GameDebugCommandServer();

      InitBase(gameEntryInstance, () => StartCoroutine(InitAsysc()));
      return true;
    }

    /// <summary>
    /// 初始化基础设置、主lua虚拟机、其他事件
    /// </summary>
    /// <param name="gameEntryInstance">入口配置实例</param>
    /// <param name="finish">完成回调</param>
    private void InitBase(GameEntry gameEntryInstance, System.Action finish)
    {
      Instance = this;

      Application.wantsToQuit += Application_wantsToQuit;

      GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);

      InitDebugConfig(gameEntryInstance);
      InitCommands();
      InitVideoSettings();

      GameActionStore = GameMediator.RegisterActionStore(GameSystemPackage.GetSystemPackage(), "System");
      Profiler.BeginSample("BindLua");
      
      GameMainEnv = new LuaSvr();      
      GameMainEnv.init(null, () =>
      {
        GameMainLuaState = LuaSvr.mainState;
        GameMainLuaState.errorDelegate += (err) => GameErrorChecker.LuaStateErrReport(GameMainLuaState, err);

        Profiler.EndSample();
        GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED);
        GameMediator.SubscribeSingleEvent(GamePackage.GetSystemPackage(), "GameManagerWaitPackageManagerReady", TAG, (evtName, param) => {
          finish.Invoke();
          return false;
        });
        GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_UI_MANAGER_INIT_FINISHED, TAG, (evtName, param) => {
          //Init Debug
          if (DebugMode)
            DebugInit.InitSystemDebug();
          return false;
        });
      });
    }

    private bool sLoadUserPackages = true;
    private bool sEnablePackageLoadFilter = false;
    private string sCustomDebugName = "";
    private List<string> sLoadCustomPackages = new List<string>();

    /// <summary>
    /// 加载调试配置
    /// </summary>
    /// <param name="gameEntryInstance">入口配置实例</param>
    private void InitDebugConfig(GameEntry gameEntryInstance)
    {
      //根据GameEntry中调试信息赋值到当前类变量，供加载使用
      sLoadUserPackages = !DebugMode || gameEntryInstance.DebugLoadCustomPackages;
      sCustomDebugName = DebugMode && gameEntryInstance.DebugType == GameDebugType.CustomDebug ? gameEntryInstance.DebugCustomEntryEvent : "";
      sEnablePackageLoadFilter = DebugMode && gameEntryInstance.DebugType != GameDebugType.FullDebug && gameEntryInstance.DebugType != GameDebugType.NoDebug;
      if (DebugMode && gameEntryInstance.DebugType != GameDebugType.NoDebug)
      {
        foreach (var package in gameEntryInstance.DebugInitPackages)
        {
          if (package.Enable) sLoadCustomPackages.Add(package.PackageName);
        }
      }
    }
    
    /// <summary>
    /// 异步初始化主流程。
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitAsysc()
    {
      if(GameEntry.Instance.DebugEnableLuaDebugger)
      {
        Profiler.BeginSample("GameManagerStartDebugger");
        //初始化lua调试器
        GameMainLuaState.doString(@"
            local SystemPackage = Ballance2.Package.GamePackage.GetSystemPackage()
            SystemPackage:RequireLuaFile('debugger')
            InternalStart('" + GameEntry.Instance.DebugLuaDebugger + "')", "GameManagerStartDebugger");
        Profiler.EndSample();
      }

      //检测lua绑定状态
      object o = GameMainLuaState.doString(@"return Ballance2.Services.GameManager.EnvBindingCheckCallback()", "GameManagerSystemInit");
      if (o != null &&  (
              (o.GetType() == typeof(int) && (int)o == GameConst.GameBulidVersion)
              || (o.GetType() == typeof(double) && (double)o == GameConst.GameBulidVersion)
          ))
          Log.D(TAG, "Game Lua bind check ok.");
      else
      {
          Log.E(TAG, "Game Lua bind check failed, did you bind lua functions?");
          GameErrorChecker.LastError = GameError.EnvBindCheckFailed;
#if UNITY_EDITOR // 编辑器中
          GameErrorChecker.ThrowGameError(GameError.EnvBindCheckFailed, "Lua接口没有绑定。请点击“SLua”>“All”>“Make”生成 Lua 接口绑定。");
#else
          GameErrorChecker.ThrowGameError(GameError.LuaBindCheckFailed, "错误的发行配置，请检查。");  
#endif
          yield break;
      }

      SecurityUtils.FixLuaSecure(GameMainLuaState);

      //初始化宏定义
      LuaUtils.InitMacros(GameMainLuaState);

      GameErrorChecker.EnterStrictMode();

      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BASE_INIT_FINISHED, "*");
      
      //加载系统 packages
      yield return StartCoroutine(LoadSystemCore());

      //加载用户选择的模块
      if (sLoadUserPackages)
        yield return StartCoroutine(LoadUserPackages());

      //全部加载完毕之后通知所有模块初始化
      GetSystemService<GamePackageManager>().NotifyAllPackageRun("*");

      //通知初始化完成
      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED, "*", null);

      //如果调试配置中设置了CustomDebugName，则进入自定义调试场景，否则进入默认场景
      if (string.IsNullOrEmpty(sCustomDebugName))
      {
        //进入场景
        if (DebugMode && GameEntry.Instance.DebugSkipIntro) {
          if(!RequestEnterLogicScense("MenuLevel"))
            GameErrorChecker.ShowSystemErrorMessage("Enter firstScense failed");
        }
        else if (firstScense != "") {
          if(RequestEnterLogicScense(firstScense))
            GameErrorChecker.ShowSystemErrorMessage("Enter firstScense failed");
        }
      }
      else
      {
        Log.D(TAG, "Enter GameDebug.");
        if(RequestEnterLogicScense("GameDebug"))
          GameErrorChecker.ShowSystemErrorMessage("Enter GameDebug failed");
        GameMediator.DispatchGlobalEvent(sCustomDebugName, "*", null);
      }
    }
    /// <summary>
    /// 加载用户定义的模组
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadUserPackages()
    {
      var pm = GetSystemService<GamePackageManager>();
      var list = pm.LoadPackageRegisterInfo();

      //Save action
      GameActionStore.RegisterAction(GamePackage.GetSystemPackage(), "SavePackageRegisterInfo", "GameManager", (param) =>
      {
        pm.SavePackageRegisterInfo();
        return GameActionCallResult.SuccessResult;
      }, null);

      foreach (var info in list)
      {

        if (sEnablePackageLoadFilter && !sLoadCustomPackages.Contains(info.packageName)) continue;

        var task = pm.RegisterPackage((info.enableLoad ? "Enable:" : "") + info.package);
        yield return task;

        if (task.Result && info.enableLoad)
          yield return pm.LoadPackage(info.packageName);
      }
    }
    /// <summary>
    /// 加载系统定义的模块包
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadSystemCore()
    {
      var pm = GetSystemService<GamePackageManager>();
      var corePackageName = GamePackageManager.CORE_PACKAGE_NAME;
      var systemPackage = GamePackage.GetSystemPackage();

      #region 读取读取SystemInit文件

      XmlDocument systemInit = new XmlDocument();
      TextAsset systemInitAsset = Resources.Load<TextAsset>("SystemInit");
      if(systemInitAsset == null) {
        GameErrorChecker.ThrowGameError(GameError.FileNotFound, "SystemInit.xml missing! ");
        StopAllCoroutines();
        yield break;
      }
      systemInit.LoadXml(systemInitAsset.text);

      #endregion

      #region 系统配置

      XmlNode CorePackageName = systemInit.SelectSingleNode("System/SystemOptions/CorePackageName");
      if (CorePackageName != null && !string.IsNullOrWhiteSpace(CorePackageName.InnerText))
        corePackageName = CorePackageName.InnerText;
      XmlNode LogToFileEnabled = systemInit.SelectSingleNode("System/SystemOptions/LogToFileEnabled");
      if (LogToFileEnabled != null && LogToFileEnabled.InnerText.ToLower() == "true")
        Log.StartLogFile();

      #endregion

      #region 系统逻辑场景配置

      XmlNode SystemScenses = systemInit.SelectSingleNode("System/SystemScenses");
      if (SystemScenses != null)
      {
        foreach (XmlNode node in SystemScenses.ChildNodes)
        {
          if (node.NodeType == XmlNodeType.Element && node.Name == "Scense" && !StringUtils.isNullOrEmpty(node.InnerText))
            logicScenses.Add(node.InnerText);
        }
      }
      XmlNode FirstEnterScense = systemInit.SelectSingleNode("System/SystemOptions/FirstEnterScense");
      if (FirstEnterScense != null && !StringUtils.isNullOrEmpty(FirstEnterScense.InnerText)) 
        firstScense = FirstEnterScense.InnerText; 
      if (!logicScenses.Contains(firstScense))
      {
        GameErrorChecker.ThrowGameError(GameError.ConfigueNotRight, "FirstEnterScense 配置不正确");
        StopAllCoroutines();
        yield break;
      }

      #endregion

      #region 加载系统内核包

      {
        Profiler.BeginSample("ExecuteSystemCore");

        systemPackage.SetFlag(0xF0);
        systemPackage.SystemPackage = true;
        systemPackage.RunPackageExecutionCode();
        systemPackage.RequireLuaFile("SystemInternal.lua");
        systemPackage.RequireLuaFile("GameCoreLib/GameCoreLibInit.lua");

        Profiler.EndSample();

        yield return new WaitForSeconds(1f);

        Log.D(TAG, "ExecuteSystemCore ok");
      }

      {
        //加载游戏主内核包
        Task<bool> task = pm.LoadPackage(corePackageName);
        yield return new WaitUntil(() => task.IsCompleted);
        if (!task.Result)
        {
          GameErrorChecker.ThrowGameError(GameError.SystemPackageLoadFailed, "系统包 “" + corePackageName + "” 加载失败：" + GameErrorChecker.LastError + "\n您可尝试重新安装游戏");
          yield break;
        }

        Log.D(TAG, "Load system core ok");
      }

      {
        //检查系统包版本是否与内核版本一致
        var corePackage = pm.FindPackage(corePackageName);
        var ver = corePackage.PackageEntry.Version;
        if (ver <= 0)
        {
          GameErrorChecker.ThrowGameError(GameError.SystemPackageLoadFailed, "Invalid Core package (2)");
          yield break;
        }
        if (ver != GameConst.GameBulidVersion)
        {
          GameErrorChecker.ThrowGameError(GameError.SystemPackageLoadFailed, "主包版本与游戏内核版本不符（" + ver + "!=" + GameConst.GameBulidVersion + "）\n您可尝试重新安装游戏");
          yield break;
        }
      
        Log.D(TAG, "Check core version ok");
      }

      #endregion

      #region 加载SystemPackages中定义的包

      Profiler.BeginSample("GameManagerLoadSystemPackages");

      XmlNode nodeSystemPackages = systemInit.SelectSingleNode("System/SystemPackages");

      int loadedPackageCount = 0, failedPackageCount = 0;
      for (int loadStepNow = 0; loadStepNow < 2; loadStepNow++)
      {
        for (int i = 0; i < nodeSystemPackages.ChildNodes.Count; i++)
        {
          XmlNode nodePackage = nodeSystemPackages.ChildNodes[i];

          if (nodePackage.Name == "Package" && nodePackage.Attributes["name"] != null)
          {
            string packageName = nodePackage.Attributes["name"].InnerText;
            int minVer = 0;
            int loadStep = 0;
            bool mustLoad = false;
            foreach (XmlAttribute attribute in nodePackage.Attributes)
            {
              if (attribute.Name == "minVer")
                minVer = ConverUtils.StringToInt(attribute.Value, 0, "Package/minVer");
              else if (attribute.Name == "mustLoad")
                mustLoad = ConverUtils.StringToBoolean(attribute.Value, false, "Package/mustLoad");
              else if (attribute.Name == "loadStep")
                loadStep = ConverUtils.StringToInt(attribute.Value, 0, "Package/loadStep");
            }
            if (string.IsNullOrEmpty(packageName))
            {
              Log.W(TAG, "The Package node {0} name is empty!", i);
              continue;
            }
            if (loadStepNow != loadStep) continue;
            if (packageName == "core.debug" && !DebugMode) continue;
            if (sEnablePackageLoadFilter && !sLoadCustomPackages.Contains(packageName)) continue;

            //加载包
            Task<bool> task = pm.LoadPackage(packageName);
            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Result)
            {
              GamePackage package = pm.FindPackage(packageName);
              if (package == null)
              {
                StopAllCoroutines();
                GameErrorChecker.ThrowGameError(GameError.UnKnow, packageName + " not found!\n请尝试重启游戏");
                yield break;
              }
              if (package.PackageVersion < minVer)
              {
                StopAllCoroutines();
                GameErrorChecker.ThrowGameError(GameError.SystemPackageNotLoad,
                    string.Format("模块 {0} 版本过低：{1} 小于所需版本 {2}, 您可尝试重新安装游戏",
                    packageName, package.PackageVersion, minVer));
                yield break;
              }
              package.SystemPackage = true;
              loadedPackageCount++;
            }
            else
            {
              failedPackageCount++;
              if (mustLoad)
              {
                StopAllCoroutines();
                GameErrorChecker.ThrowGameError(GameError.SystemPackageNotLoad,
                    "系统定义的模块：" + packageName + " 未能加载成功\n错误：" +
                    GameErrorChecker.GetLastErrorMessage() + " (" + GameErrorChecker.LastError + ")\n请尝试重启游戏");
                yield break;
              }
            }
          }
        }
        Log.D(TAG, "Load " + loadedPackageCount + " packages, " + failedPackageCount + " failed.");

        //第一次加载基础包，等待其运行
        if (loadStepNow == 0)
        {
          yield return new WaitForSeconds(0.3f);

          pm.NotifyAllPackageRun("*");

          yield return new WaitForSeconds(0.2f);

          //进入Intro
          if (!DebugMode || !GameEntry.Instance.DebugSkipIntro)
          {
            RequestEnterLogicScense(firstScense);
            firstScense = "";
          }
        }
      }

      Profiler.EndSample();

      yield return new WaitForSeconds(0.2f);

      //全部加载完毕之后通知所有模块初始化
      pm.NotifyAllPackageRun("*");

      #endregion
    }

    #region 系统调试命令

    private void InitCommands()
    {
      var srv = GameManager.Instance.GameDebugCommandServer;
      srv.RegisterCommand("quit", (keyword, fullCmd, argsCount, args) =>
      {
        QuitGame();
        return false;
      }, 0, "quit > 退出游戏");
      srv.RegisterCommand("s", (keyword, fullCmd, argsCount, args) =>
      {
        var type = (string)args[0];
        switch (type)
        {
          case "set":
            {
              if (args.Length == 4)
              {
                string setName = "";
                string setType = "";
                string setVal = "";
                if (!DebugUtils.CheckDebugParam(1, args, out setName)) break;
                if (!DebugUtils.CheckDebugParam(2, args, out setType)) break;
                if (!DebugUtils.CheckDebugParam(3, args, out setVal)) break;

                switch (setType)
                {
                  case "bool":
                    GameSettings.SetBool(setName, bool.Parse(setVal));
                    return true;
                  case "string":
                    GameSettings.SetString(setName, setVal);
                    return true;
                  case "float":
                    GameSettings.SetFloat(setName, float.Parse(setVal));
                    return true;
                  case "int":
                    GameSettings.SetInt(setName, int.Parse(setVal));
                    return true;
                  default:
                    Log.E(TAG, "未知设置类型 {0}", setType);
                    break;
                }
              }
              else if (args.Length >= 5)
              {
                string setName = "";
                string setPackage = "";
                string setType = "";
                string setVal = "";
                if (!DebugUtils.CheckDebugParam(1, args, out setPackage)) break;
                if (!DebugUtils.CheckDebugParam(2, args, out setName)) break;
                if (!DebugUtils.CheckDebugParam(3, args, out setType)) break;
                if (!DebugUtils.CheckDebugParam(4, args, out setVal)) break;

                var settings = GameSettingsManager.GetSettings(setPackage);
                if (settings == null)
                {
                  Log.E(TAG, "未找到指定设置包 {0}", setPackage);
                  break;
                }

                switch (setType)
                {
                  case "bool":
                    settings.SetBool(setName, bool.Parse(setVal));
                    return true;
                  case "string":
                    settings.GetString(setName, setVal);
                    return true;
                  case "float":
                    settings.GetFloat(setName, float.Parse(setVal));
                    return true;
                  case "int":
                    settings.GetInt(setName, int.Parse(setVal));
                    return true;
                  default:
                    Log.E(TAG, "未知设置类型 {0}", setType);
                    break;
                }
              }
              break;
            }
          case "get":
            {
              if (args.Length == 3)
              {
                string setName = "";
                string setType = "";
                if (!DebugUtils.CheckDebugParam(1, args, out setName)) break;
                if (!DebugUtils.CheckDebugParam(2, args, out setType)) break;

                switch (setType)
                {
                  case "bool":
                    Log.V(TAG, GameSettings.GetBool(setName, false).ToString());
                    return true;
                  case "string":
                    Log.V(TAG, GameSettings.GetString(setName, ""));
                    return true;
                  case "float":
                    Log.V(TAG, GameSettings.GetFloat(setName, 0).ToString());
                    return true;
                  case "int":
                    Log.V(TAG, GameSettings.GetInt(setName, 0).ToString());
                    return true;
                  default:
                    Log.E(TAG, "未知设置类型 {0}", setType);
                    break;
                }
              }
              else if (args.Length >= 4)
              {
                string setName = "";
                string setPackage = "";
                string setType = "";
                if (!DebugUtils.CheckDebugParam(1, args, out setPackage)) break;
                if (!DebugUtils.CheckDebugParam(2, args, out setName)) break;
                if (!DebugUtils.CheckDebugParam(3, args, out setType)) break;

                var settings = GameSettingsManager.GetSettings(setPackage);
                if (settings == null)
                {
                  Log.E(TAG, "未找到指定设置包 {0}", setPackage);
                  break;
                }

                switch (setType)
                {
                  case "bool":
                    Log.V(TAG, settings.GetBool(setName, false).ToString());
                    return true;
                  case "string":
                    Log.V(TAG, settings.GetString(setName, ""));
                    return true;
                  case "float":
                    Log.V(TAG, settings.GetFloat(setName, 0).ToString());
                    return true;
                  case "int":
                    Log.V(TAG, settings.GetInt(setName, 0).ToString());
                    return true;
                  default:
                    Log.E(TAG, "未知设置类型 {0}", setType);
                    break;
                }
              }
              break;
            }
          case "reset":
            {
              GameSettingsManager.ResetDefaultSettings();
              return true;
            }
          case "list":
            {
              GameSettingsManager.ListActuators();
              return true;
            }
          case "notify":
            {
              string setPackage = "";
              string setGroup = "";
              if (!DebugUtils.CheckDebugParam(1, args, out setPackage)) break;
              if (!DebugUtils.CheckDebugParam(2, args, out setGroup)) break;

              var settings = GameSettingsManager.GetSettings(setPackage);
              if (settings == null)
              {
                Log.E(TAG, "未找到指定设置包 {0}", setPackage);
                break;
              }

              settings.NotifySettingsUpdate(setGroup);
              return true;
            }
        }
        return false;
      }, 0, "s <set/get/reset/list/notify> 系统设置命令\n" +
              "  set <packageName:string> <setKey:string> <bool/string/float/int> <newValue> ▶ 设置指定执行器的某个设置\n" +
              "  set <setKey:string> <bool/string/float/int> <newValue>                      ▶ 设置系统执行器的某个设置\n" +
              "  get <packageName:string> <setKey:string> <bool/string/float/int>            ▶ 获取指定执行器的某个设置值\n" +
              "  get <setKey:string> <bool/string/float/int>                                 ▶ 获取系统执行器的某个设置值\n" +
              "  reset <packageName:string>                                                  ▶ 重置所有设置为默认值\n" +
              "  notify <packageName:string> <group:string>                                  ▶ 通知指定组设置已更新\n" +
              "  list                                                                        ▶ 列举出所有子模块的设置执行器"
      );
      srv.RegisterCommand("r", (keyword, fullCmd, argsCount, args) =>
      {
        var type = (string)args[0];
        switch (type)
        {
          case "fps":
            {
              int fpsVal;
              if (DebugUtils.CheckIntDebugParam(1, args, out fpsVal, false, 0))
              {
                if (fpsVal <= 0 && fpsVal > 120) { Log.E(TAG, "错误的参数：{0}", args[0]); break; }
                Application.targetFrameRate = fpsVal;
              }
              Log.V(TAG, "TargetFrameRate is {0}", Application.targetFrameRate);
              return true;
            }
          case "resolution":
            {
              if (args.Length >= 3)
              {
                int width, height, mode;
                if (!DebugUtils.CheckIntDebugParam(1, args, out width)) break;
                if (!DebugUtils.CheckIntDebugParam(2, args, out height)) break;
                DebugUtils.CheckIntDebugParam(1, args, out mode, false, (int)Screen.fullScreenMode);

                Screen.SetResolution(width, height, (FullScreenMode)mode);
              }
              Log.V(TAG, "Screen resolution is {0}x{1} {2}", Screen.width, Screen.height, Screen.fullScreenMode);
              return true;
            }
          case "full":
            {
              int mode;
              if (!DebugUtils.CheckIntDebugParam(1, args, out mode)) break;
              Screen.SetResolution(Screen.width, Screen.height, (FullScreenMode)mode);
              return true;
            }
          case "vsync":
            {
              int mode;
              if (!DebugUtils.CheckIntDebugParam(1, args, out mode)) break;
              if (mode < 0 && mode > 2) { Log.E(TAG, "错误的参数：{0}", args[0]); break; }
              QualitySettings.vSyncCount = mode;
              return true;
            }
          case "device":
            {
              Log.V(TAG, SystemInfo.deviceName + " (" + SystemInfo.deviceModel + ")");
              Log.V(TAG, SystemInfo.operatingSystem + " (" + SystemInfo.processorCount + "/" + SystemInfo.processorType + "/" + SystemInfo.processorFrequency + ")");
              Log.V(TAG, SystemInfo.graphicsDeviceName + " " + SystemInfo.graphicsDeviceVendor + " /" +
                        FileUtils.GetBetterFileSize(SystemInfo.graphicsMemorySize) + " (" + SystemInfo.graphicsDeviceID + ")");
              break;
            }
        }
        return false;
      }, 0, "r <fps/resolution/full/device>\n" +
              "  fps [packageName:nmber:number(1-120)]                                ▶ 获取或者设置游戏的目标帧率\n" +
              "  resolution <width:nmber> <height:nmber> [fullScreenMode:number(0-3)] ▶ 设置游戏的分辨率或全屏\n" +
              "  vsync <fullScreenMode:number(0-2)>                                   ▶ 设置垂直同步,0: 关闭，1：同步1次，2：同步2次\n" +
              "  full <fullScreenMode::number(0-3)>                                   ▶ 设置游戏的全屏，0：ExclusiveFullScreen，1：FullScreenWindow，2：MaximizedWindow，3：Windowed\n" +
              "  device > 获取当前设备信息");
      srv.RegisterCommand("eval", (keyword, fullCmd, argsCount, args) =>
      {
        var cmd = fullCmd.Substring(2);
        if(!cmd.Contains("\n") && !cmd.StartsWith("return "))
            cmd = "return " + cmd;
        var ret = GameMainLuaState.doString(cmd, "GameManagerLuaConsole");
        Log.V(TAG, "doString return " + DebugUtils.PrintLuaVarAuto(ret, 10));
        return true;
      }, 1, "c <code:string> ▶ 运行 Lua 命令");
      srv.RegisterCommand("le", (keyword, fullCmd, argsCount, args) =>
      {
        Log.V(TAG, "LastError is {0}", GameErrorChecker.LastError.ToString());
        return true;
      }, 1, "le > 获取LastError");
    }

    #endregion

    #region 视频设置

    private Resolution[] resolutions = null;
    private int defaultResolution = 0;

    private void InitVideoSettings()
    {
      //发出屏幕大小更改事件
      GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_SCREEN_SIZE_CHANGED);

      resolutions = Screen.resolutions;

      for (int i = 0; i < resolutions.Length; i++)
        if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
        {
          defaultResolution = i;
          break;
        }

      //设置更新事件
      GameSettings.RegisterSettingsUpdateCallback("video", OnVideoSettingsUpdated);
      GameSettings.RequireSettingsLoad("video");
    }

    //视频设置更新器，更新视频设置至引擎
    private bool OnVideoSettingsUpdated(string groupName, int action)
    {
      int resolutionsSet = GameSettings.GetInt("video.resolution", defaultResolution);
      bool fullScreen = GameSettings.GetBool("video.fullScreen", Screen.fullScreen);
      int quality = GameSettings.GetInt("video.quality", QualitySettings.GetQualityLevel());
      int vSync = GameSettings.GetInt("video.vsync", QualitySettings.vSyncCount);

      Log.V(TAG, "OnVideoSettingsUpdated:\nresolutionsSet: {0}\nfullScreen: {1}" +
          "\nquality: {2}\nvSync : {3}", resolutionsSet, fullScreen, quality, vSync);

      Screen.SetResolution(resolutions[resolutionsSet].width, resolutions[resolutionsSet].height, fullScreen);
      QualitySettings.SetQualityLevel(quality, true);
      QualitySettings.vSyncCount = vSync;

      //发出屏幕大小更改事件
      GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_SCREEN_SIZE_CHANGED, "*",
          resolutions[resolutionsSet].width, resolutions[resolutionsSet].height);
      return true;
    }

    #endregion

    #endregion

    #region 退出和销毁

    /// <summary>
    /// 清空整个场景
    /// </summary>
    internal void ClearScense()
    {
      foreach (Camera c in Camera.allCameras)
        c.gameObject.SetActive(false);
      for (int i = 0, c = GameCanvas.transform.childCount; i < c; i++)
      {
        GameObject go = GameCanvas.transform.GetChild(i).gameObject;
        if (go.name == "GameUIWindow")
        {
          GameObject go1 = null;
          for (int j = 0, c1 = go.transform.childCount; j < c1; j++)
          {
            go1 = go.transform.GetChild(j).gameObject;
            if (go1.tag != "DebugWindow")
              go1.SetActive(false);
          }
        }
        else if (go.name != "DebugToolbar")
          go.SetActive(false);
      }
      for (int i = 0, c = transform.childCount; i < c; i++)
      {
        GameObject go = transform.GetChild(i).gameObject;
        if (go.name != "GameManager")
          go.SetActive(false);
      }
      GameBaseCamera.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 释放
    /// </summary>
    [DoNotToLua]
    public override void Destroy()
    {
      Log.D(TAG, "Destroy");

      Instance = null;

      if (GameMainEnv != null) {
        GameMainEnv = null;
      }
    }

    /// <summary>
    /// 开始退出游戏流程
    /// </summary>
    [LuaApiDescription("退出游戏")]
    public void QuitGame()
    {
      Log.D(TAG, "QuitGame");
      if (!gameIsQuitEmitByGameManager)
      {
        gameIsQuitEmitByGameManager = true;
        DoQuit();
      }
      GameSystem.IsRestart = false;
    }
    /// <summary>
    /// 重启游戏
    /// </summary>
    [LuaApiDescription("重启游戏")]
    public void RestartGame()
    {
      QuitGame();
      GameSystem.IsRestart = true;
    }

    private static bool gameIsQuitEmitByGameManager = false;

    private bool Application_wantsToQuit()
    {
      if (!gameIsQuitEmitByGameManager)
        DoQuit();
      return true;
    }
    private void DoQuit()
    {
      //退出时将清理一些关键数据，发送退出事件

      GameBaseCamera.clearFlags = CameraClearFlags.Color;
      GameBaseCamera.backgroundColor = Color.black;

      Application.wantsToQuit -= Application_wantsToQuit;

      var pm = GetSystemService<GamePackageManager>();
      pm.SavePackageRegisterInfo();

      ObjectStateBackupUtils.ClearAll();

      if (DebugMode)
        DebugInit.UnInitSystemDebug();

      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BEFORE_GAME_QUIT, "*", null);
      ReqGameQuit();
    }

    #endregion

    #region Update

    [CustomLuaClass]
    public delegate void VoidDelegate();

    private int nextGameQuitTick = -1;
    private class DelayItem
    {
      public float Time;
      public VoidDelegate Callback;
    }
    private List<DelayItem> delayItems = new List<DelayItem>();

    /// <summary>
    /// 请求游戏退出。
    /// </summary>
    private void ReqGameQuit()
    {
      nextGameQuitTick = 40;
    }
    protected override void Update()
    {
      if (nextGameQuitTick >= 0)
      {
        nextGameQuitTick--;
        if (nextGameQuitTick == 30)
          ClearScense();
        if (nextGameQuitTick == 0)
          GameSystem.Destroy();
      }
      if (delayItems.Count > 0)
      {
        for (int i = delayItems.Count - 1; i >= 0; i--)
        {
          var item = delayItems[i];
          item.Time -= Time.deltaTime;
          if (item.Time <= 0)
          {
            item.Callback.Invoke();
            delayItems.RemoveAt(i);
          }
        }
      }
    }

    #endregion

    #region 获取系统服务的包装

    /// <summary>
    /// 获取系统服务
    /// </summary>
    /// <typeparam name="T">继承于GameService的服务类型</typeparam>
    /// <returns>返回服务实例，如果没有找到，则返回null</returns>
    [DoNotToLua]
    public T GetSystemService<T>() where T : GameService
    {
      return (T)GameSystem.GetSystemService(typeof(T).Name);
    }
    /// <summary>
    /// 获取系统服务
    /// </summary>
    /// <param name="name">服务名称</param>
    /// <returns>返回服务实例，如果没有找到，则返回null</returns>
    [LuaApiDescription("获取系统服务", "返回服务实例，如果没有找到，则返回null")]
    [LuaApiParamDescription("name", "服务名称")]
    public static GameService GetSystemService(string name)
    {
      return GameSystem.GetSystemService(name);
    }

    #endregion

    #region 虚拟机执行方法



    #endregion

    #region 其他方法

    private Camera lastActiveCam = null;

    /// <summary>
    /// 设置基础摄像机状态
    /// </summary>
    /// <param name="visible">是否显示</param>
    [LuaApiDescription("设置基础摄像机状态")]
    [LuaApiParamDescription("visible", "是否显示")]
    public void SetGameBaseCameraVisible(bool visible)
    {
      if (visible)
      {
        foreach (var c in Camera.allCameras)
          if (c.gameObject.activeSelf && c.gameObject.tag == "MainCamera")
          {
            lastActiveCam = c;
            lastActiveCam.gameObject.SetActive(false);
            break;
          }
      }
      GameBaseCamera.gameObject.SetActive(visible);
      if (!visible && lastActiveCam != null)
      {
        lastActiveCam.gameObject.SetActive(true);
        lastActiveCam = null;
      }
    }

    /// <summary>
    /// Lua绑定检查回调
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("Lua绑定检查回调")]
    public static int EnvBindingCheckCallback()
    {
      return GameConst.GameBulidVersion;
    }

    // Prefab 预制体实例化相关方法。
    // 这里提供一些快速方法方便直接使用。这些方法与 CloneUtils 提供的方法功能一致。

    /// <summary>
    /// 实例化预制体，并设置名称
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="name">新对象名称</param>
    /// <returns>返回新对象</returns>
    [LuaApiDescription("实例化预制体", "返回新对象")]
    [LuaApiParamDescription("prefab", "预制体")]
    [LuaApiParamDescription("name", "新对象名称")]
    public GameObject InstancePrefab(GameObject prefab, string name)
    {
      return InstancePrefab(prefab, transform, name);
    }
    /// <summary>
    /// 实例化预制体 ，并设置父级与名称
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="parent">父级</param>
    /// <param name="name">父级</param>
    /// <returns>返回新对象</returns>
    [LuaApiDescription("实例化预制体", "返回新对象")]
    [LuaApiParamDescription("prefab", "预制体")]
    [LuaApiParamDescription("parent", "父级")]
    [LuaApiParamDescription("name", "新对象名称")]
    public GameObject InstancePrefab(GameObject prefab, Transform parent, string name)
    {
      return CloneUtils.CloneNewObjectWithParent(prefab, parent, name);
    }
    /// <summary>
    /// 新建一个新的 GameObject ，并设置父级与名称
    /// </summary>
    /// <param name="parent">父级</param>
    /// <param name="name">新对象名称</param>
    /// <returns>返回新对象</returns>
    [LuaApiDescription("新建一个新的 GameObject ", "返回新对象")]
    [LuaApiParamDescription("parent", "父级")]
    [LuaApiParamDescription("name", "新对象名称")]
    public GameObject InstanceNewGameObject(Transform parent, string name)
    {
      GameObject go = new GameObject(name);
      go.transform.SetParent(parent);
      return go;
    }
    /// <summary>
    /// 新建一个新的 GameObject 
    /// </summary>
    /// <param name="name">新对象名称</param>
    /// <returns>返回新对象</returns>
    [LuaApiDescription("新建一个新的 GameObject ", "返回新对象")]
    [LuaApiParamDescription("name", "新对象名称")]
    public GameObject InstanceNewGameObject(string name)
    {
      GameObject go = new GameObject(name);
      go.transform.SetParent(transform);
      return go;
    }

    // 此处提供了一些方法来允许Lua读写本地配置文件,操作或删除本地目录等。
    // 这里提供一些快速方法方便直接使用。等同于FileUtils中的相关函数。 

    /// <summary>
    /// 写入字符串至指定文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="append">是否追加写入文件，否则为覆盖写入</param>
    /// <param name="path">要写入的文件</param>
    [LuaApiDescription("写入字符串至指定文件")]
    [LuaApiParamDescription("path", "文件路径")]
    [LuaApiParamDescription("append", "是否追加写入文件，否则为覆盖写入")]
    [LuaApiParamDescription("data", "要写入的文件")]
    public void WriteFile(string path, bool append, string data) { FileUtils.WriteFile(path, append, data); }
    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>返回文件是否存在</returns>
    [LuaApiDescription("检查文件是否存在", "返回文件是否存在")]
    [LuaApiParamDescription("path", "文件路径")]
    public bool FileExists(string path) { return FileUtils.FileExists(path); }
    /// <summary>
    /// 检查目录是否存在
    /// </summary>
    /// <param name="path">目录路径</param>
    /// <returns>返回是否存在</returns>
    [LuaApiDescription("检查文件是否存在", "返回文件是否存在")]
    [LuaApiParamDescription("path", "文件路径")]
    public bool DirectoryExists(string path) { return FileUtils.DirectoryExists(path); }
    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="path">目录路径</param>
    [LuaApiDescription("创建目录")]
    [LuaApiParamDescription("path", "目录路径")]
    public void CreateDirectory(string path) { FileUtils.CreateDirectory(path); }
    /// <summary>
    /// 读取文件至字符串
    /// </summary>
    /// <param name="path">文件</param>
    /// <returns>返回文件男人</returns>
    [LuaApiDescription("读取文件至字符串", "返回文件路径")]
    [LuaApiParamDescription("path", "文件路径")]
    public string ReadFile(string path) { return FileUtils.ReadFile(path); }
    /// <summary>
    /// 删除指定的文件或目录
    /// </summary>
    /// <param name="path">文件</param>
    [LuaApiDescription("删除指定的文件或目录")]
    [LuaApiParamDescription("path", "文件")]
    public void RemoveFile(string path) { FileUtils.RemoveFile(path); }

    /// <summary>
    /// 延时
    /// </summary>
    /// <param name="sec">延时时长，秒</param>
    /// <param name="callback">回调</param>
    [LuaApiDescription("延时. Lua中不推荐使用这个函数，请使用LuaTimer")]
    public void Delay(float sec, VoidDelegate callback)
    {
      var d = new DelayItem();
      d.Time = sec;
      d.Callback = callback;
      delayItems.Add(d);
    }

    #endregion

    #region Logic Scense

    // 逻辑场景是类似于 Unity 场景的功能，但是它无需频繁加载卸载。
    // 切换逻辑场景实际上是在同一个 Unity 场景中操作，因此速度快。
    // 切换逻辑场景将发出 EVENT_LOGIC_SECNSE_QUIT 与 EVENT_LOGIC_SECNSE_ENTER 全局事件，
    // 子模块根据这两个事件来隐藏或显示自己的东西，从而达到切换场景的效果。

    private List<string> logicScenses = new List<string>();
    private int currentScense = -1;
    private string firstScense = "";

    /// <summary>
    /// 进入指定逻辑场景
    /// </summary>
    /// <param name="name">场景名字</param>
    /// <returns>返回是否成功</returns>
    [LuaApiDescription("进入指定逻辑场景", "返回是否成功")]
    [LuaApiParamDescription("name", "场景名字")]
    public bool RequestEnterLogicScense(string name)
    {
      int curIndex = logicScenses.IndexOf(name);
      if (curIndex < 0)
      {
        GameErrorChecker.LastError = GameError.NotRegister;
        Log.E(TAG, "Scense {0} not register! ", name);
        return false;
      }
      if (curIndex == currentScense)
        return true;

      Log.I(TAG, "Enter logic scense {0} ", name);
      if (currentScense >= 0)
        GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_LOGIC_SECNSE_QUIT, "*", logicScenses[currentScense]);
      currentScense = curIndex;
      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_LOGIC_SECNSE_ENTER, "*", logicScenses[currentScense]);
      return true;
    }
    /// <summary>
    /// 获取所有逻辑场景
    /// </summary>
    [LuaApiDescription("获取所有逻辑场景")]
    public string[] GetLogicScenses() { return logicScenses.ToArray(); }

    #endregion
  }
}