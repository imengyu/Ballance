using Ballance.LuaHelpers;
using Ballance.Sys.Debug;
using Ballance2.Config;
using Ballance2.Config.Settings;
using Ballance2.Sys.Bridge;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Entry;
using Ballance2.Sys.Package;
using Ballance2.Sys.Res;
using Ballance2.Sys.Services;
using Ballance2.Utils;
using SLua;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* GameManager.cs
* 
* 用途：
* 游戏底层主管理器，提供多个关键全局组件使用，负责管理游戏底层管理与控制
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-15 创建
* 2021-4-13 mengyu 添加了退出方法
*
*/

namespace Ballance2.Sys
{
    /// <summary>
    /// 游戏底层主管理器，提供多个关键全局组件使用，负责管理游戏底层管理与控制
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("游戏底层主管理器，提供多个关键全局组件使用，负责管理游戏底层管理与控制")]
    public partial class GameManager : MonoBehaviour
    {
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
        /// 游戏全局Lua虚拟机
        /// </summary>
        [LuaApiDescription("游戏全局Lua虚拟机")]
        public LuaSvr.MainState GameMainLuaState { get; private set; }
        /// <summary>
        /// 系统设置实例
        /// </summary>
        [LuaApiDescription("系统设置实例")]
        public GameSettingsActuator GameSettings { get; private set; }
        /// <summary>
        /// 游戏全局Lua虚拟机
        /// </summary>
        [LuaApiDescription("游戏全局Lua虚拟机")]
        public LuaSvr GameMainLuaSvr { get; private set; }
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
        /// 调试命令控制器
        /// </summary>
        /// <value></value>
        [LuaApiDescription("调试命令控制器")]
        public GameDebugCommandServer GameDebugCommandServer { get; private set; }
        /// <summary>
        /// 游戏内核Store
        /// </summary>
        [LuaApiDescription("游戏内核Store")]
        public Store GameStore { get; private set; }
        /// <summary>
        /// 游戏内核ActionStore
        /// </summary>
        [LuaApiDescription("游戏内核ActionStore")]
        public GameActionStore GameActionStore { get; private set; }

        private readonly string TAG = "GameManager";

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        internal void Initialize(GameEntry gameEntryInstance)
        {
            Log.D(TAG, "Initialize");

            Instance = this;

            Application.wantsToQuit += Application_wantsToQuit;

            GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
            GameBaseCamera = gameEntryInstance.GameBaseCamera;
            GameCanvas = gameEntryInstance.GameCanvas;
            GameDebugCommandServer = new GameDebugCommandServer();

            LoadBaseSettings();
            InitCommands();
            InitVideoSettings();

            GameStore = GameMediator.RegisterGlobalDataStore("core");
            GameActionStore = GameMediator.RegisterActionStore(GameSystemPackage.GetSystemPackage(), "System");
            GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED);

            GameMainLuaSvr = new LuaSvr();
            GameMainLuaSvr.init(null, () =>
            {
                GameMainLuaState = LuaSvr.mainState;
                GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BASE_INIT_FINISHED, "*");

                //Run init
                StartCoroutine(InitAsysc());
            });
        }

        private bool debugMode = false;
        
        private IEnumerator InitAsysc()
        {
            //检测lua绑定状态
            object o = GameMainLuaState.doString("return Ballance2.Sys.GameManager.LuaBindingCallback()");
            if (o != null &&  (
                    (o.GetType() == typeof(int) && (int)o == GameConst.GameBulidVersion)
                    || (o.GetType() == typeof(double) && (double)o == GameConst.GameBulidVersion)
                ))
                Log.D(TAG, "Game Lua bind check ok.");
            else
            {
                Log.E(TAG, "Game Lua bind check failed, did you bind lua functions?");
                GameErrorChecker.LastError = GameError.LuaBindCheckFailed;
#if UNITY_EDITOR // 编辑器中
                GameErrorChecker.ThrowGameError(GameError.LuaBindCheckFailed,
                    "Lua接口没有绑定。请点击“SLua”>“All”>“Make”生成 Lua 接口绑定。");
#else
                GameErrorChecker.ThrowGameError(GameError.LuaBindCheckFailed, "错误的发行配置，请检查。");  
#endif
                yield break;
            }

            //加载系统 packages
            yield return StartCoroutine(LoadSystemInit());

            //加载用户选择的模块
            yield return StartCoroutine(LoadUserPackages());

            //通知初始化完成
            GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED, "*", null);
        }
        private IEnumerator LoadUserPackages() {
            var pm = GetSystemService<GamePackageManager>();
            var list = pm.LoadPackageRegisterInfo();

            //Save action
            GameActionStore.RegisterAction(GamePackage.GetSystemPackage(), "SavePackageRegisterInfo", "GameManager", (param) => {
                pm.SavePackageRegisterInfo();
                return GameActionCallResult.SuccessResult;
            }, null);

            foreach(var info in list) {

                var task = pm.RegisterPackage((info.enableLoad ? "Enable:" : "") + info.package);
                yield return task;

                if(task.Result && info.enableLoad) 
                    yield return pm.LoadPackage(info.packageName);
            }
   
            yield return new WaitForSeconds(1);

            //全部加载完毕之后通知所有模块初始化
            pm.NotifyAllPackageRun("*");
        }
        private IEnumerator LoadSystemInit()
        {
            //读取读取SystemInit文件
            XmlDocument systemInit = new XmlDocument();
#if UNITY_EDITOR
            if (DebugSettings.Instance.SystemInitLoadWay == LoadResWay.InUnityEditorProject
                && File.Exists("Assets/Packages/system_SystemInit.xml"))
            {
                Log.D(TAG, "Load SystemInit in editor.");
                systemInit.Load("Assets/Packages/system_SystemInit.xml");
            }
            else
#else
            if(true) 
#endif
            {
                string url = GamePathManager.GetResRealPath("systeminit", "");
                UnityWebRequest request = UnityWebRequest.Get(url);
                yield return request.SendWebRequest();
                if (!string.IsNullOrEmpty(request.error))
                {
                    //加载失败
                    StopAllCoroutines();
                    if (request.responseCode == 404)
                        GameErrorChecker.ThrowGameError(GameError.FileNotFound, "未找到 SystemInit\n您可尝试重新安装游戏");
                    else
                        GameErrorChecker.ThrowGameError(GameError.FileNotFound, "读取 SystemInit 失败：" + request.responseCode + "\n您可尝试重新安装游戏");
                    yield break;
                }
                systemInit.LoadXml(request.downloadHandler.text);
            }

            
            var pm = GetSystemService<GamePackageManager>();

            XmlNode nodeDebugMode = systemInit.SelectSingleNode("System/DebugMode");
            if(nodeDebugMode!=null&&nodeDebugMode.InnerText.ToLower() == "true") {
                GameSettings.SetBool("DebugMode", true);
                debugMode = true;
            }

            XmlNode LogToFileEnabled = systemInit.SelectSingleNode("System/LogToFileEnabled");
            if(nodeDebugMode!=null && nodeDebugMode.InnerText.ToLower() == "true") {
            }

            //加载SystemPackages中定义的包
            XmlNode nodeSystemPackages = systemInit.SelectSingleNode("System/SystemPackages");

            for(int loadStepNow = 0; loadStepNow < 2; loadStepNow++) {
                for(int i = 0; i < nodeSystemPackages.ChildNodes.Count; i++)
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
                        if(string.IsNullOrEmpty(packageName))
                        {
                            Log.W(TAG, "The Package node {0} name is empty!", i);
                            continue;
                        }
                        if(loadStepNow != loadStep) continue;
                        if(packageName == "core.debug" && !debugMode) continue;

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
                        }
                        else
                        {
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

                //第一次加载基础包，等待其运行
                if(loadStepNow == 0) {
                    yield return new WaitForSeconds(1);
                    pm.NotifyAllPackageRun("*");
                    yield return new WaitForSeconds(2);
                }
            }

            yield return new WaitForSeconds(2);
            //全部加载完毕之后通知所有模块初始化
            pm.NotifyAllPackageRun("*");
        }
        private void LoadBaseSettings() {
#if UNITY_EDITOR
            debugMode = true;
#else
            if(GameSettings.GetBool("DebugMode", false)) debugMode = true;
            else GameObject.Find("GameDebugBeginStats").SetActive(false);
#endif
        }
        
        #region 系统调试命令
        
        private void InitCommands() {
            var srv = GameManager.Instance.GameDebugCommandServer;
            srv.RegisterCommand("quit", (keyword, fullCmd, args) => {
                QuitGame();
                return false;
            }, 0, "quit 退出游戏");
            srv.RegisterCommand("s", (keyword, fullCmd, args) =>
            {
                var type = (string)args[0];
                switch(type) {
                    case "set": {
                        if(args.Length == 4) {
                            string setName = ""; 
                            string setType = "";
                            string setVal = "";
                            if(!DebugUtils.CheckDebugParam(1, args, out setName)) break;
                            if(!DebugUtils.CheckDebugParam(2, args, out setType)) break;
                            if(!DebugUtils.CheckDebugParam(3, args, out setVal)) break;

                            switch(setType) {
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
                        } else if(args.Length >= 5) {
                            string setName = ""; 
                            string setPackage = "";
                            string setType = "";
                            string setVal = "";
                            if(!DebugUtils.CheckDebugParam(1, args, out setPackage)) break;
                            if(!DebugUtils.CheckDebugParam(2, args, out setName)) break;
                            if(!DebugUtils.CheckDebugParam(3, args, out setType)) break;
                            if(!DebugUtils.CheckDebugParam(4, args, out setVal)) break;

                            var settings = GameSettingsManager.GetSettings(setPackage);
                            if(settings == null) {
                                Log.E(TAG, "未找到指定设置包 {0}", setPackage);
                                break;
                            }

                            switch(setType) {
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
                    case "get": {
                        if(args.Length == 3) {
                            string setName = ""; 
                            string setType = "";
                            if(!DebugUtils.CheckDebugParam(1, args, out setName)) break;
                            if(!DebugUtils.CheckDebugParam(2, args, out setType)) break;

                            switch(setType) {
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
                        } else if(args.Length >= 4) {
                            string setName = ""; 
                            string setPackage = "";
                            string setType = "";
                            if(!DebugUtils.CheckDebugParam(1, args, out setPackage)) break;
                            if(!DebugUtils.CheckDebugParam(2, args, out setName)) break;
                            if(!DebugUtils.CheckDebugParam(3, args, out setType)) break;

                            var settings = GameSettingsManager.GetSettings(setPackage);
                            if(settings == null) {
                                Log.E(TAG, "未找到指定设置包 {0}", setPackage);
                                break;
                            }

                            switch(setType) {
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
                    case "reset": {
                        GameSettingsManager.ResetDefaultSettings();
                        return true;
                    }
                    case "list": {
                        GameSettingsManager.ListActuators();
                        return true;
                    }
                    case "notify": {
                        string setPackage = "";
                        string setGroup = "";
                        if(!DebugUtils.CheckDebugParam(1, args, out setPackage)) break;
                        if(!DebugUtils.CheckDebugParam(2, args, out setGroup)) break;

                        var settings = GameSettingsManager.GetSettings(setPackage);
                        if(settings == null) {
                            Log.E(TAG, "未找到指定设置包 {0}", setPackage);
                            break;
                        }

                        settings.NotifySettingsUpdate(setGroup);
                        return true;
                    }
                }
                return false;
            }, 0, "s <set/get/reset/list/notify> 系统设置命令\n" +
                    "  set <packageName:string> <setKey:string> <bool/string/float/int> <newValue> 设置指定执行器的某个设置\n" +
                    "  set <setKey:string> <bool/string/float/int> <newValue> 设置系统执行器的某个设置\n" +
                    "  get <packageName:string> <setKey:string> <bool/string/float/int> 获取指定执行器的某个设置值\n" +
                    "  get <setKey:string> <bool/string/float/int> 获取系统执行器的某个设置值\n" +
                    "  reset <packageName:string> 重置所有设置为默认值\n" +
                    "  notify <packageName:string> <group:string> 通知指定组设置已更新\n" +
                    "  list 列举出所有子模块的设置执行器"
            );
            srv.RegisterCommand("r", (keyword, fullCmd, args) =>
            {
                var type = (string)args[0];
                switch(type) {
                    case "fps": {
                        int fpsVal;
                        if(DebugUtils.CheckIntDebugParam(1, args, out fpsVal, false, 0)) {
                            if (fpsVal <= 0 && fpsVal > 120) { Log.E(TAG, "错误的参数：{0}", args[0]); break; }
                            Application.targetFrameRate = fpsVal;
                        } 
                        Log.V(TAG, "TargetFrameRate is {0}", Application.targetFrameRate);
                        return true;
                    }
                    case "resolution": {
                        if(args.Length >= 3) {
                            int width, height, mode;
                            if(!DebugUtils.CheckIntDebugParam(1, args, out width)) break;
                            if(!DebugUtils.CheckIntDebugParam(2, args, out height)) break;
                            DebugUtils.CheckIntDebugParam(1, args, out mode, false, (int)Screen.fullScreenMode);

                            Screen.SetResolution(width, height, (FullScreenMode)mode);
                        }
                        Log.V(TAG, "Screen resolution is {0}x{1} {2}", Screen.width, Screen.height, Screen.fullScreenMode);
                        return true;
                    }
                    case "full": {
                        int mode;
                        if(!DebugUtils.CheckIntDebugParam(1, args, out mode)) break;
                        Screen.SetResolution(Screen.width, Screen.height, (FullScreenMode)mode);
                        return true;
                    }
                    case "vsync": {
                        int mode;
                        if(!DebugUtils.CheckIntDebugParam(1, args, out mode)) break;
                        if (mode < 0 && mode > 2) { Log.E(TAG, "错误的参数：{0}", args[0]); break; }
                        QualitySettings.vSyncCount = mode;
                        return true;
                    }
                    case "device": {
                        Log.V(TAG, SystemInfo.deviceName + " (" + SystemInfo.deviceModel + ")");
                        Log.V(TAG, SystemInfo.operatingSystem + " (" + SystemInfo.processorCount + "/" + SystemInfo.processorType + "/" + SystemInfo.processorFrequency + ")");
                        Log.V(TAG, SystemInfo.graphicsDeviceName + " " + SystemInfo.graphicsDeviceVendor + " /" + 
                            FileUtils.GetBetterFileSize(SystemInfo.graphicsMemorySize) + " (" + SystemInfo.graphicsDeviceID + ")" );
                        break;
                    }
                }
                return false;
            }, 0, "r <fps/resolution/full/device>\n" + 
                    "  fps [packageName:nmber:number(1-120)] 获取或者设置游戏的目标帧率\n" +
                    "  resolution <width:nmber> <height:nmber> [fullScreenMode:number(0-3)] 设置游戏的分辨率或全屏\n" +
                    "  vsync <fullScreenMode:number(0-2)> 设置垂直同步,0: 关闭，1：同步1次，2：同步2次\n" +
                    "  full <fullScreenMode::number(0-3)> 设置游戏的全屏，0：ExclusiveFullScreen，1：FullScreenWindow，2：MaximizedWindow，3：Windowed\n" +
                    "  device 获取当前设备信息");
            srv.RegisterCommand("c", (keyword, fullCmd, args) =>
            {
                GameMainLuaState.doString(fullCmd.Substring(2));
                return true;
            }, 1, "c [any] 运行 Lua 命令。此命令将会在全局Lua虚拟机中运行");
            srv.RegisterCommand("le", (keyword, fullCmd, args) =>
            {
                Log.V(TAG, "LastError is {0}", GameErrorChecker.LastError.ToString());
                return true;
            }, 1, "le 获取LastError");
        }
       
        #endregion

        #region 视频设置

        private Resolution[] resolutions = null;
        private int defaultResolution = 0;

        private void InitVideoSettings()
        {
            //屏幕大小事件
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

        private bool OnVideoSettingsUpdated(string groupName, int action)
        {
            int resolutionsSet = GameSettings.GetInt("video.resolution", defaultResolution);
            bool fullScreen = GameSettings.GetBool("video.fullScreen", Screen.fullScreen);
            int quality = GameSettings.GetInt("video.quality", QualitySettings.GetQualityLevel());
            int vSync = GameSettings.GetInt("video.vsync", QualitySettings.vSyncCount);

            Log.V(TAG, "OnVideoSettingsUpdated:\nresolutionsSet: {0}\nfullScreen: {1}" +
                "\nquality: {2}\nvSync : {3}", resolutionsSet, fullScreen, quality, vSync);

            Screen.SetResolution(resolutions[resolutionsSet].width, resolutions[resolutionsSet].height, true);
            Screen.fullScreen = fullScreen;
            Screen.fullScreenMode = fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            QualitySettings.SetQualityLevel(quality, true);
            QualitySettings.vSyncCount = vSync;
            
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
                        if(go1.tag != "DebugWindow")
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
        internal void Destroy()
        {
            Log.D(TAG, "Destroy");
            if (GameMainLuaState != null) GameMainLuaState = null;
        }

        /// <summary>
        /// 开始退出游戏流程
        /// </summary>
        public void QuitGame()
        {
            Log.D(TAG, "quit");
            if(!gameIsQuitEmitByGameManager) {
                gameIsQuitEmitByGameManager = true;
                DoQuit();
            }
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
            GameBaseCamera.clearFlags = CameraClearFlags.Color;
            GameBaseCamera.backgroundColor = Color.black;

            Application.wantsToQuit -= Application_wantsToQuit;
            
            var pm = GetSystemService<GamePackageManager>();
            pm.SavePackageRegisterInfo();

            GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BEFORE_GAME_QUIT, "*", null);
            ReqGameQuit();
        }

        #endregion

        #region Update

        private int nextGameQuitTick = -1;

        /// <summary>
        /// 请求游戏退出
        /// </summary>
        private void ReqGameQuit()
        {
            nextGameQuitTick = 80;
        }
        private void Update() {
            if (nextGameQuitTick >= 0)
            {
                nextGameQuitTick--;
                if (nextGameQuitTick == 50)
                    ClearScense();
                if (nextGameQuitTick == 30)
                    GameSystem.Destroy();
                if (nextGameQuitTick <= 10) 
                    GameSystem.QuitPlayer();
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
        public GameService GetSystemService(string name)
        {
            return GameSystem.GetSystemService(name);
        }
        
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
            if(visible) {
                foreach(var c in Camera.allCameras)
                    if(c.gameObject.activeSelf && c.gameObject.tag == "MainCamera") {
                        lastActiveCam = c;
                        lastActiveCam.gameObject.SetActive(false);
                        break;
                    }
            }
            GameBaseCamera.gameObject.SetActive(visible);
            if(!visible && lastActiveCam != null) {
                lastActiveCam.gameObject.SetActive(true);
                lastActiveCam = null;
            }
        }

        /// <summary>
        /// Lua绑定检查回调
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("Lua绑定检查回调")]
        public static int LuaBindingCallback()
        {
            return GameConst.GameBulidVersion;
        }

        #endregion
    }
}
