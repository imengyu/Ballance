using System;
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
using Ballance2.Game;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services.Debug;
using Ballance2.Utils;
using UnityEngine;

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
  public class GameManager : GameService<GameManager>
  {
    public GameManager() : base("GameManager") {}

    #region 全局关键组件变量

    /// <summary>
    /// 获取中介者 GameMediator 实例
    /// </summary>
    public static GameMediator GameMediator { get; internal set; }
    /// <summary>
    /// 获取系统设置实例
    /// </summary>
    public GameSettingsActuator GameSettings { get; private set; }
    /// <summary>
    /// 获取基础摄像机
    /// </summary>
    public Camera GameBaseCamera { get; private set; }
    /// <summary>
    /// 获取根Canvas
    /// </summary>
    public RectTransform GameCanvas { get; internal set; }
    /// <summary>
    /// 获取调试命令控制器
    /// </summary>
    /// <value></value>
    public GameDebugCommandServer GameDebugCommandServer { get; private set; }
    /// <summary>
    /// 获取全局灯光实例
    /// </summary>
    public static Light GameLight { get; private set; }
    /// <summary>
    /// GameTimeMachine 的一个实例. 
    /// </summary>
    /// <value></value>
    public static GameTimeMachine GameTimeMachine { 
      get {
        if(_GameTimeMachine == null)
          _GameTimeMachine = GameSystem.GetSystemService<GameTimeMachine>();
        return _GameTimeMachine;
      }
    }

    private static GameTimeMachine _GameTimeMachine = null;
    private static bool _DebugMode = false;

    /// <summary>
    /// 获取或者设置当前是否处于开发者模式
    /// </summary>
    /// <remark>
    /// 设置调试模式后需要重启才能生效。
    /// </remark>
    /// <value></value>
    public static bool DebugMode { 
      get {
        return _DebugMode;
      }  
      set {
        if(_DebugMode != value) {
          _DebugMode = value;
          if(Instance != null && Instance.GameSettings != null )
            Instance.GameSettings.SetBool("DebugMode", value);
        }
      } 
    }

    private readonly string TAG = "GameManager";

    #endregion

    #region 初始化

    public override bool Initialize()
    {
      Log.D(TAG, "Initialize");

      GameEntry gameEntryInstance = GameEntry.Instance;
      GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      GameBaseCamera = gameEntryInstance.GameBaseCamera;
      GameCanvas = gameEntryInstance.GameCanvas;
      DebugMode = gameEntryInstance.DebugMode;
      GameDebugCommandServer = new GameDebugCommandServer();
      GameLight = GameObject.Find("GameLight").GetComponent<Light>();

      try {
        InitBase(gameEntryInstance, () => StartCoroutine(InitAsysc()));
      } catch(Exception e) {
        GameErrorChecker.ThrowGameError(GameError.ExecutionFailed, "系统初始化时发生异常，请反馈此BUG.\n" + e.ToString());
      }

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

      if (File.Exists(Application.dataPath + "/debugenable.txt"))
        GameManager.DebugMode = true;

      Application.wantsToQuit += Application_wantsToQuit;

      InitDebugConfig(gameEntryInstance);
      InitCommands();
      InitVideoSettings();
      
      GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED);
      GameMediator.SubscribeSingleEvent(GamePackage.GetSystemPackage(), "GameManagerWaitPackageManagerReady", TAG, (evtName, param) => {
        finish.Invoke();
        GameTimeMachine.Instance.RegisterFixedUpdate(() => Physics.Simulate(Time.fixedDeltaTime * 5), interval: 5);
        return false;
      });
      GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_UI_MANAGER_INIT_FINISHED, TAG, (evtName, param) => {
        //Init Debug
        if (DebugMode)
          DebugManager.Init();
        return false;
      });
    }

    private bool sLoadUserPackages = true;
    private bool sEnablePackageLoadFilter = false;
    private string sCustomDebugName = "";
    private List<string> sLoadCustomPackages = new List<string>();

    private XmlDocument systemInit = new XmlDocument();

    /// <summary>
    /// 加载调试配置
    /// </summary>
    /// <param name="gameEntryInstance">入口配置实例</param>
    private void InitDebugConfig(GameEntry gameEntryInstance)
    {
#if UNITY_EDITOR
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
#endif
    }
    
    /// <summary>
    /// 异步初始化主流程。
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitAsysc()
    {
      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BASE_INIT_FINISHED);
      
      //加载系统 packages
      yield return StartCoroutine(LoadSystemCore());

      yield return StartCoroutine(LoadSystemPackages());

      //加载用户选择的模块
      if (sLoadUserPackages)
        yield return StartCoroutine(LoadUserPackages());

      //全部加载完毕之后通知所有模块初始化
      GetSystemService<GamePackageManager>().NotifyAllPackageRun("*");

      //通知初始化完成
      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED);
      GameMediator.NotifySingleEvent("DoSendAllPackageUILoad");

      //如果调试配置中设置了CustomDebugName，则进入自定义调试场景，否则进入默认场景
      if (string.IsNullOrEmpty(sCustomDebugName))
      {
#if UNITY_EDITOR
        yield return new WaitForSeconds(0.5f);

        //进入场景
        if (DebugMode && GameEntry.Instance.DebugSkipIntro) {
          //进入场景
          if(!RequestEnterLogicScense("MenuLevel"))
            GameErrorChecker.ShowSystemErrorMessage("Enter firstScense failed");
          //隐藏初始加载中动画
          HideGlobalStartLoading();
        }
        else 
#endif
        if (GameSettings.GetBool("debugDisableIntro", false)) {
          //进入场景
          if(!RequestEnterLogicScense("MenuLevel"))
            GameErrorChecker.ShowSystemErrorMessage("Enter firstScense failed");
          //隐藏初始加载中动画
          HideGlobalStartLoading();
        }
        else if (firstScense != "") {
          //进入场景
          if(!RequestEnterLogicScense(firstScense))
            GameErrorChecker.ShowSystemErrorMessage("Enter firstScense failed");
          //隐藏初始加载中动画
          HideGlobalStartLoading();
        }
      }
      else
      {
        //进入场景
        Log.D(TAG, "Enter GameDebug.");
        if(!RequestEnterLogicScense("GameDebug"))
          GameErrorChecker.ShowSystemErrorMessage("Enter GameDebug failed");
        GameMediator.DispatchGlobalEvent(sCustomDebugName, GameEntry.Instance.DebugCustomEntryEventParamas);
        //隐藏初始加载中动画
        HideGlobalStartLoading();
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
      pm.ScanUserPackages(list);

      foreach (var info in list)
      {
        if (sEnablePackageLoadFilter && !sLoadCustomPackages.Contains(info.Value.packageName)) continue;

        var task = pm.RegisterPackage((info.Value.enableLoad ? "Enable:" : "") + info.Value.packageName);
        yield return task.AsIEnumerator();

        if (task.Result && info.Value.enableLoad) {
          var task2 = pm.LoadPackage(info.Value.packageName);
          yield return task2.AsIEnumerator();
        }
      }
    }
    /// <summary>
    /// 加载系统定义的模块包
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadSystemCore()
    {
      yield return 1;

      var pm = GetSystemService<GamePackageManager>();
      var systemPackage = GamePackage.GetSystemPackage();

      yield return 1;

      #region 读取读取SystemInit文件

      systemInit = new XmlDocument();
      TextAsset systemInitAsset = Resources.Load<TextAsset>("SystemInit");
      if(systemInitAsset == null) {
        GameErrorChecker.ThrowGameError(GameError.FileNotFound, "SystemInit.xml missing! ");
        StopAllCoroutines();
        yield break;
      }
      yield return 1;
      systemInit.LoadXml(systemInitAsset.text);

      #endregion

      yield return new WaitForSeconds(0.1f);

      #region 系统配置

      XmlNode LogToFileEnabled = systemInit.SelectSingleNode("System/SystemOptions/LogToFileEnabled");
      if (LogToFileEnabled != null && LogToFileEnabled.InnerText.ToLower() == "true")
        Log.StartLogFile();

      #endregion

      #region Load system core package

      Task<bool> task = systemPackage.LoadInfo("");
      yield return task.AsIEnumerator();
      if (!task.Result) 
      {
        GameErrorChecker.ThrowGameError(GameError.SystemPackageLoadFailed, "Failed to load system package info");
        StopAllCoroutines();
        yield break;
      }

      task = systemPackage.LoadPackage();
      yield return task.AsIEnumerator();

      if (!task.Result) 
      {
        GameErrorChecker.ThrowGameError(GameError.SystemPackageLoadFailed, "Failed to load system package");
        StopAllCoroutines();
        yield break;
      }

      if (!systemPackage.RunPackageExecutionCode()) 
      {
        GameErrorChecker.ThrowGameError(GameError.SystemPackageLoadFailed, "Failed to run system package");
        StopAllCoroutines();
        yield break;
      }

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
        GameErrorChecker.ThrowGameError(GameError.ConfigueNotRight, "FirstEnterScense Configue not right");
        StopAllCoroutines();
        yield break;
      }

      #endregion
    }
    /// <summary>
    /// 加载系统内核包
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadSystemPackages() {
      var pm = GetSystemService<GamePackageManager>();

      XmlNode nodeSystemPackages = systemInit.SelectSingleNode("System/SystemPackages");

      int loadedPackageCount = 0, failedPackageCount = 0;
      for (int loadStepNow = 0; loadStepNow < 2; loadStepNow++)
      {
        for (int i = 0; i < nodeSystemPackages.ChildNodes.Count; i++)
        {
          yield return new WaitForSeconds(0.02f);

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
            yield return task.AsIEnumerator();

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
              package.flag |= GamePackage.FLAG_PACK_SYSTEM_PACKAGE | GamePackage.FLAG_PACK_NOT_UNLOADABLE;
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
          pm.NotifyAllPackageRun("*");
          
          if (
#if UNITY_EDITOR
          string.IsNullOrEmpty(sCustomDebugName) && (!DebugMode || !GameEntry.Instance.DebugSkipIntro)) 
          //EDITOR 下才判断是否跳过intro DebugSkipIntro
#else
          !GameSettings.GetBool("debugDisableIntro", false))
#endif
          {
            //在基础包加载完成时就进入Intro
            RequestEnterLogicScense(firstScense);
            firstScense = "";
            HideGlobalStartLoading();
          }
        }
      }

      yield return new WaitForSeconds(0.1f);

      //全部加载完毕之后通知所有模块初始化
      pm.NotifyAllPackageRun("*");
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
      srv.RegisterCommand("restart-game", (keyword, fullCmd, argsCount, args) =>
      {
        RestartGame();
        return false;
      }, 0, "restart-game > 重启游戏");
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
      srv.RegisterCommand("lc", (keyword, fullCmd, argsCount, args) =>
      {
        var type = (string)args[0];
        switch (type)
        {
          case "show":
            {
              Log.V(TAG, "CurrentScense is {0} ({1})", logicScenses[currentScense], currentScense);
              return true;
            }
          case "show-all":
            {
              for (int i = 0; i < logicScenses.Count; i++)
                Log.V(TAG, "LogicScense[{0}] = {1}", i, logicScenses[i]);
              return true;
            }
          case "go":
            {
              if (args.Length >= 1)
              {
                if (!DebugUtils.CheckStringDebugParam(1, args)) break;

                string name = args[0];
                if(RequestEnterLogicScense(name))
                  Log.V(TAG, "EnterLogicScense " + name);
                else
                  Log.E(TAG, "EnterLogicScense " + name + " failed. LastError: " + GameErrorChecker.LastError.ToString());
              }
              return true;
            }
        }
        return false;
      }, 0, "lc <show/go>\n" +
              "  show             ▶ 获取当前所在虚拟场景\n" +
              "  go <name:string> ▶ 进入指定虚拟场景");
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
      GameSettings.RegisterSettingsUpdateCallback(SettingConstants.SettingsVideo, OnVideoSettingsUpdated);
      GameSettings.RequireSettingsLoad(SettingConstants.SettingsVideo);
    }

    //视频设置更新器，更新视频设置至引擎
    private bool OnVideoSettingsUpdated(string groupName, int action)
    {
      int resolutionsSet = GameSettings.GetInt(SettingConstants.SettingsVideoResolution, -1);
      bool fullScreen = GameSettings.GetBool(SettingConstants.SettingsVideoFullScreen, Screen.fullScreen);
      int quality = GameSettings.GetInt(SettingConstants.SettingsVideoQuality, -1);
      int vSync = GameSettings.GetInt(SettingConstants.SettingsVideoVsync, -1);

      Log.V(TAG, "OnVideoSettingsUpdated:\nresolutionsSet: {0}\nfullScreen: {1}\nquality: {2}\nvSync : {3}", resolutionsSet, fullScreen, quality, vSync);

      if(resolutionsSet >= 0) {
        if (resolutionsSet < resolutions.Length) {
          var resolution = resolutions[resolutionsSet];
          Screen.SetResolution(resolution.width, resolution.height, fullScreen);
          //发出屏幕大小更改事件
          GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_SCREEN_SIZE_CHANGED, resolution.width, resolution.height);
        }
      }
      if(fullScreen != Screen.fullScreen)
        Screen.fullScreen = fullScreen;
      if(quality >= 0) 
        QualitySettings.SetQualityLevel(quality, true);
      if(vSync >= 0) 
        QualitySettings.vSyncCount = vSync;
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
    public override void Destroy()
    {
      Log.D(TAG, "Destroy");
      Instance = null;
    }

    /// <summary>
    /// 请求开始退出游戏流程
    /// </summary>
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
    public void RestartGame()
    {
      QuitGame();
      GameSystem.IsRestart = true;
    }

    private static bool gameIsQuitEmitByGameManager = false;

    private bool Application_wantsToQuit()
    {
      if (!gameIsQuitEmitByGameManager) {
        DoQuit();
        return false;
      }
      return true;
    }
    private void DoQuit()
    {
      //退出时将清理一些关键数据，发送退出事件

      GameBaseCamera.clearFlags = CameraClearFlags.Color;
      GameBaseCamera.backgroundColor = Color.black;

      Application.wantsToQuit -= Application_wantsToQuit;

      delayItems.Clear();

      var pm = GetSystemService<GamePackageManager>();
      pm.SavePackageRegisterInfo();

      ObjectStateBackupUtils.ClearAll();

      if (DebugMode)
        DebugManager.Destroy();
      
      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BEFORE_GAME_QUIT);
      ReqGameQuit();
    }

    #endregion

    #region Update

    public delegate void VoidDelegate();

    private int nextGameQuitTick = -1;

    /// <summary>
    /// 请求游戏退出。
    /// </summary>
    private void ReqGameQuit()
    {
      nextGameQuitTick = 40;
    }
    protected override void Update()
    {
      //延时函数管理运行
      if (delayItems.Count > 0)
      {
        LinkedListNode<DelayItem> item = delayItems.First;
        while(item != null) {
          item.Value.Time -= Time.deltaTime;
          if (item.Value.Time <= 0)
          {
            delayItems.Remove(item);
            item.Value.Callback.Invoke();
            item = item.Next;
            continue;
          }
          item = item.Next;
        }
      }
      //定时器函数运行
      if (timerItems.Count > 0)
      {
        LinkedListNode<TimerItem> item = timerItems.First;
        while(item != null) {
          var timeItem = item.Value;
          timeItem.Time -= Time.deltaTime;
          if (timeItem.Time <= 0) {
            timeItem.Time = timeItem.LoopTime;
            item.Value.Callback.Invoke();
          }
          item = item.Next;
        }
      }
      //退出延时
      if (nextGameQuitTick >= 0)
      {
        nextGameQuitTick--;
        if (nextGameQuitTick == 30)
          ClearScense();
        if (nextGameQuitTick == 0) {
          nextGameQuitTick = -1;
          gameIsQuitEmitByGameManager = false;
          GameSystem.Destroy();
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
    public static T GetSystemService<T>() where T : GameService<T>
    {
      return GameSystem.GetSystemService<T>();
    }

    #endregion

    #region 其他方法

    private Camera lastActiveCam = null;

    /// <summary>
    /// 进行截图
    /// </summary>
    public string CaptureScreenshot() {
      
      //创建目录
      string saveDir = Application.persistentDataPath + "/Screenshot/";
      if(!Directory.Exists(saveDir))
        Directory.CreateDirectory(saveDir);

      //保存图片
      string savePath = saveDir + "Screenshot" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
      ScreenCapture.CaptureScreenshot(savePath);
      Log.D(TAG, "CaptureScreenshot to " + savePath);
      //提示
      Delay(1.0f, () => GetSystemService<GameUIManager>()
        .GlobalToast(I18N.I18N.TrF("global.CaptureScreenshotSuccess", "", savePath))
      );
      return savePath;
    }
    /// <summary>
    /// 设置基础摄像机状态
    /// </summary>
    /// <param name="visible">是否显示</param>
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
    /// 隐藏全局初始Loading动画
    /// </summary>
    public void HideGlobalStartLoading() { GameEntry.Instance.GameGlobalIngameLoading.SetActive(false); }
    /// <summary>
    /// 显示全局初始Loading动画
    /// </summary>
    public void ShowGlobalStartLoading() { GameEntry.Instance.GameGlobalIngameLoading.SetActive(true); }

    #region Prefab methods

    // Prefab 预制体实例化相关方法。
    // 这里提供一些快速方法方便直接使用。这些方法与 CloneUtils 提供的方法功能一致。

    /// <summary>
    /// 实例化预制体，并设置名称
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="name">新对象名称</param>
    /// <returns>返回新对象</returns>
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
    public GameObject InstanceNewGameObject(string name)
    {
      GameObject go = new GameObject(name);
      go.transform.SetParent(transform);
      return go;
    }

    #endregion

    #region FileUtils

    // 这里提供一些快速方法方便直接使用。等同于FileUtils中的相关函数。 

    /// <summary>
    /// 写入字符串至指定文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="append">是否追加写入文件，否则为覆盖写入</param>
    /// <param name="path">要写入的文件</param>
    public void WriteFile(string path, bool append, string data) { FileUtils.WriteFile(path, append, data); }
    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>返回文件是否存在</returns>
    public bool FileExists(string path) { return FileUtils.FileExists(path); }
    /// <summary>
    /// 检查目录是否存在
    /// </summary>
    /// <param name="path">目录路径</param>
    /// <returns>返回是否存在</returns>
    public bool DirectoryExists(string path) { return FileUtils.DirectoryExists(path); }
    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="path">目录路径</param>
    public void CreateDirectory(string path) { FileUtils.CreateDirectory(path); }
    /// <summary>
    /// 读取文件至字符串
    /// </summary>
    /// <param name="path">文件</param>
    /// <returns>返回文件男人</returns>
    public string ReadFile(string path) { return FileUtils.ReadFile(path); }
    /// <summary>
    /// 删除指定的文件或目录
    /// </summary>
    /// <param name="path">文件</param>
    public void RemoveFile(string path) { FileUtils.RemoveFile(path); }
    /// <summary>
    /// 删除指定的目录
    /// </summary>
    /// <param name="path">文件</param>
    public void RemoveDirectory(string path) { FileUtils.RemoveDirectory(path); }

    #endregion

    #region 定时器与延时

    private int timerId = -1;
    private class DelayItem
    {
      public int Id;
      public float Time;
      public VoidDelegate Callback;
    }
    private class TimerItem
    {
      public int Id;
      public float DelayTime;
      public float LoopTime;

      public float Time;
      public VoidDelegate Callback;
    }
    private LinkedList<DelayItem> delayItems = new LinkedList<DelayItem>();
    private LinkedList<TimerItem> timerItems = new LinkedList<TimerItem>();

    private int GetNextTimerId() { 
      if (timerId > 0xffffffe)
        timerId = 0;
      return timerId++;
    }

    /// <summary>
    /// 设置一个延时执行回调
    /// </summary>
    /// <param name="sec">延时时长，秒</param>
    /// <param name="callback">回调</param>
    public int Delay(float sec, VoidDelegate callback)
    {
      var d = new DelayItem();
      d.Time = sec;
      d.Id = GetNextTimerId();
      d.Callback = callback;
      delayItems.AddFirst(d);
      return d.Id;
    }

    /// <summary>
    /// 通过ID删除指定的延时执行回调
    /// </summary>
    /// <param name="timerId">Delay 函数返回的 ID </param>
    /// <returns>返回是否删除成功</returns>
    public bool DeleteDelay(int timerId)
    {
      LinkedListNode<DelayItem> item = delayItems.First;
      while(item != null) {
        if (item.Value.Id == timerId)
        {
          delayItems.Remove(item);
          return true;
        }
        item = item.Next;
      }
      return false;
    }

    /// <summary>
    /// 设置一个定时器
    /// </summary>
    /// <param name="loopSec">定时器循环时间</param>
    /// <param name="callback">回调</param>
    /// <returns>返回ID, 可以使用 DeleteTimer 来删除定时器</returns>
    public int Timer(float loopSec, VoidDelegate callback)
    {
      var d = new TimerItem();
      d.Id = GetNextTimerId();
      d.LoopTime = loopSec;
      d.DelayTime = loopSec;
      d.Callback = callback;
      timerItems.AddLast(d);
      return d.Id;
    }

    /// <summary>
    /// 设置一个定时器
    /// </summary>
    /// <param name="delaySec">定时器第一次启动延迟时间</param>
    /// <param name="loopSec">定时器循环时间</param>
    /// <param name="callback">回调</param>
    /// <returns>返回ID, 可以使用 DeleteTimer 来删除定时器</returns>
    public int Timer(float loopSec, float delaySec, VoidDelegate callback)
    {
      var d = new TimerItem();
      d.Id = GetNextTimerId();
      d.LoopTime = loopSec;
      d.DelayTime = delaySec;
      d.Callback = callback;
      timerItems.AddLast(d);
      return d.Id;
    }
       
    /// <summary>
    /// 通过ID删除指定的定时器
    /// </summary>
    /// <param name="timerId">Timer 函数返回的 ID </param>
    /// <returns>返回是否删除成功</returns>
    public bool DeleteTimer(int timerId)
    {
      LinkedListNode<TimerItem> item = timerItems.First;
      while(item != null) {
        if (item.Value.Id == timerId)
        {
          timerItems.Remove(item);
          return true;
        }
        item = item.Next;
      }
      return false;
    }
    
    #endregion

    #endregion

    #region Logic Scense

    // 逻辑场景是类似于 Unity 场景的功能，但是它无需频繁加载卸载。
    // 切换逻辑场景实际上是在同一个 Unity 场景中操作，因此速度快。
    // 切换逻辑场景将发出 EVENT_LOGIC_SECNSE_QUIT 与 EVENT_LOGIC_SECNSE_ENTER 全局事件，
    // 子模块根据这两个事件来隐藏或显示自己的东西，从而达到切换场景的效果。

    /*
    下面的示例演示了如何监听进入离开逻辑场景，并显示或隐藏自己的内容：
    ```csharp
    GameManager.GameMediator.RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, "Intro", (evtName, params) => {
      var scense = params[1];
      if (scense == "Intro") { 
        //进入场景时显示自己的东西
      }
      return false;
    });
    GameManager.GameMediator.RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, "Intro", (evtName, params) => {
      var scense = params[1];
      if (scense == "Intro") { 
        //离开场景时隐藏自己的东西
      }
      return false;
    });
    ```
    */

    private List<string> logicScenses = new List<string>();
    private int currentScense = -1;
    private string firstScense = "";

    /// <summary>
    /// 进入指定逻辑场景
    /// </summary>
    /// <param name="name">场景名字</param>
    /// <returns>返回是否成功</returns>
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
        GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_LOGIC_SECNSE_QUIT, logicScenses[currentScense]);
      currentScense = curIndex;
      GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_LOGIC_SECNSE_ENTER, logicScenses[currentScense]);
      return true;
    }
    /// <summary>
    /// 获取所有逻辑场景
    /// </summary>
    public string[] GetLogicScenses() { return logicScenses.ToArray(); }

    #endregion
  }
}