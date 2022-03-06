using Ballance2.Base;
using Ballance2.Config;
using Ballance2.Config.Settings;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services.Debug;
using Ballance2.UI.Core;
using Ballance2.Utils;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Profiling;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePackageManager.cs
* 
* 用途：
* 框架包管理器，用于管理模块包的注册、加载、卸载等流程。
* 一并提供了从模块包中读取资源的相关API。
*
* 作者：
* mengyu
*/

namespace Ballance2.Services
{
  /// <summary>
  /// 框架模块管理器
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("框架模块管理器")]
  public class GamePackageManager : GameService
  {
    private static readonly string TAG = "GamePackageManager";

    /// <summary>
    /// 系统模块的包名
    /// </summary>
    [LuaApiDescription("系统模块的包名")]
    public const string SYSTEM_PACKAGE_NAME = "system";
    /// <summary>
    /// 游戏主模块的包名
    /// </summary>
    [LuaApiDescription("游戏主模块的包名")]
    public const string CORE_PACKAGE_NAME = "core";

    public GamePackageManager() : base(TAG) { }

    /// <summary>
    /// 是否是无模块模式
    /// </summary>
    /// <value></value>
    [LuaApiDescription("是否是无模块模式")]
    public bool NoPackageMode { get; set; }

    [SLua.DoNotToLua]
    public override void Destroy()
    {
      UnLoadAllPackages();

      var systemPackage = GamePackage.GetSystemPackage();
      systemPackage.flag = (GamePackage.FLAG_PACK_NOT_UNLOADABLE | GamePackage.FLAG_PACK_SYSTEM_PACKAGE);
      systemPackage._Status = GamePackageStatus.NotLoad;
      registeredPackages.Remove(SYSTEM_PACKAGE_NAME);
      registeredPackages.Clear();
      packagesLoadStatus.Clear();
      loadedPackages.Clear();

      if(GameManager.GameMediator) {
        GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_FAILED);
        GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_SUCCESS);
        GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_REGISTERED);
        GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_UNLOAD);
      }
    }
    [SLua.DoNotToLua]
    public override bool Initialize()
    {
      var systemPackage = GamePackage.GetSystemPackage();

      GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_FAILED);
      GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_SUCCESS);
      GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_REGISTERED);
      GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_UNREGISTERED);
      GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_PACKAGE_UNLOAD);
      GameManager.GameMediator.RegisterEventHandler(systemPackage,
        GameEventNames.EVENT_UI_MANAGER_INIT_FINISHED, "GamePackageManagerHandler", (evtName, param) =>
        {
          InitPackageCommands();
          return false;
        });
      GameManager.GameMediator.DelayedNotifySingleEvent("GameManagerWaitPackageManagerReady", 0.5f);

      systemPackage._Status = GamePackageStatus.LoadSuccess;
      registeredPackages.Add(SYSTEM_PACKAGE_NAME, new GamePackageRegisterInfo(systemPackage));
      loadedPackages.Add(SYSTEM_PACKAGE_NAME, systemPackage);
      return true;
    }

    #region 模块包自动加载管理

    internal class GamePackageRegisterInfo
    {
      public GamePackageRegisterInfo(GamePackage package)
      {
        this.package = package;
        this.packageName = package.PackageName;
        this.enableLoad = true;
      }
      public GamePackageRegisterInfo(string packageName, bool enableLoad, bool trustPackage)
      {
        this.packageName = packageName;
        this.enableLoad = enableLoad;
        this.trustPackage = trustPackage;
      }

      public GamePackage package;

      public string packageName;
      public bool enableLoad;
      public bool trustPackage;
    }

    internal Dictionary<string, int> packagesLoadStatus = new Dictionary<string, int>();
    internal Dictionary<string, GamePackageRegisterInfo> registeredPackages = new Dictionary<string, GamePackageRegisterInfo>();
    internal Dictionary<string, GamePackage> loadedPackages = new Dictionary<string, GamePackage>();

    private XmlDocument packageEnableStatusListXml = new XmlDocument();

    /// <summary>
    /// 扫描模块
    /// </summary>
    internal void ScanUserPackages(Dictionary<string, GamePackageRegisterInfo> list) {
      var dirPath = GamePathManager.GetResRealPath("package", "", false, false);
      if(Directory.Exists(dirPath)) {
        //扫描文件夹中没有的包，添加到列表中
        DirectoryInfo direction = new DirectoryInfo(dirPath);
        FileInfo[] files = direction.GetFiles("*.ballance", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {
          var rel = files[i].FullName.Replace('\\', '/');
          var fileNameNoExt = Path.GetFileNameWithoutExtension(rel);
          if(StringUtils.IsPackageName(fileNameNoExt) && !list.ContainsKey(fileNameNoExt) && !IsPackageRegistered(fileNameNoExt)) {
            list.Add(fileNameNoExt, new GamePackageRegisterInfo(fileNameNoExt, false, true));
          }
        } 
      }
    }
    /// <summary>
    /// 读取模块包状态
    /// </summary>
    /// <returns></returns>
    internal Dictionary<string, GamePackageRegisterInfo> LoadPackageRegisterInfo()
    {
      string pathPackageStatus = Application.persistentDataPath + "/PackageStatus.xml";
      if (File.Exists(pathPackageStatus))
      {
        StreamReader sr = new StreamReader(pathPackageStatus, Encoding.UTF8);
        try
        {
          packageEnableStatusListXml.LoadXml(sr.ReadToEnd());
        }
        catch
        {
          packageEnableStatusListXml.LoadXml(ConstStrings.DEFAULT_PACKAGE_STATUS_XML);
        }
        sr.Close();
        sr.Dispose();
      }
      else//加载默认xml文档
        packageEnableStatusListXml.LoadXml(ConstStrings.DEFAULT_PACKAGE_STATUS_XML);

      Dictionary<string, GamePackageRegisterInfo> lastRegisteredPackages = new Dictionary<string, GamePackageRegisterInfo>();
      XmlNode nodeNoPackagePackagee = packageEnableStatusListXml.SelectSingleNode("NoPackagePackagee");
      if (nodeNoPackagePackagee != null)
        NoPackageMode = bool.Parse(nodeNoPackagePackagee.InnerText);
      XmlNode nodePackageList = packageEnableStatusListXml.SelectSingleNode("PackageList");
      if (nodePackageList != null)
      {
        foreach (XmlNode n in nodePackageList) {
          if(!lastRegisteredPackages.ContainsKey(n.InnerText))
            lastRegisteredPackages.Add(n.InnerText, new GamePackageRegisterInfo(
              n.InnerText, 
              n.Attributes["enabled"].Value == "True",
              n.Attributes["trusted"].Value == "True")
            );
        }
      }

      return lastRegisteredPackages;
    }
    /// <summary>
    /// 保存模块包状态
    /// </summary>
    internal void SavePackageRegisterInfo()
    {
      StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/PackageStatus.xml", false, Encoding.UTF8);

      XmlDocument xml = new XmlDocument();
      XmlNode nodePackageConfig = xml.CreateElement("PackageConfig");
      XmlNode nodePackageList = xml.CreateElement("PackageList");
      XmlNode nodeNoPackagePackagee = xml.CreateElement("NoPackagePackagee");

      xml.AppendChild(xml.CreateXmlDeclaration("1.0", "utf-8", null));
      xml.AppendChild(nodePackageConfig);
      nodePackageConfig.AppendChild(nodePackageList);
      nodePackageConfig.AppendChild(nodeNoPackagePackagee);

      nodeNoPackagePackagee.InnerText = NoPackageMode.ToString();
      foreach (var s in registeredPackages.Values)
      {
        XmlNode node = xml.CreateElement("Package");
        XmlAttribute attr = xml.CreateAttribute("enabled");
        attr.Value = s.enableLoad.ToString();
        attr = xml.CreateAttribute("trusted");
        attr.Value = s.trustPackage.ToString();
        node.InnerText = s.package.PackageName;
        nodePackageList.AppendChild(node);
      }

      //save
      packageEnableStatusListXml.Save(sw);
      sw.Close();
      sw.Dispose();

    }
    /// <summary>
    /// 设置模块包已信任状态
    /// </summary>
    /// <param name="packageName">包名</param>
    internal void SetPackageTrusted(string packageName)
    {
      if (registeredPackages.TryGetValue(packageName, out var outPackage))
        outPackage.trustPackage = true;
    }
    /// <summary>
    /// 设置模块包启用状态
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <param name="val">启用状态</param>
    internal void SetPackageEnableLoad(string packageName, bool val)
    {
      if (registeredPackages.TryGetValue(packageName, out var outPackage))
        outPackage.enableLoad = val;
    }
    
    #endregion

    #region 模块包管理API

    internal static void PreRegInternalPackage() {
#if UNITY_EDITOR
      var realPackagePath = ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ASSET_PATH;
      if (DebugSettings.Instance.PackageLoadWay == LoadResWay.InUnityEditorProject && Directory.Exists(realPackagePath)) {
        GamePackage.SetCorePackage(new GameEditorCorePackage(realPackagePath));
      } 
      else
#else
      if(true) 
#endif
      {
        GamePackage.SetCorePackage(new GameCorePackage());
      }
    }
    internal static void ReleaseInternalPackage() {
      GamePackage.SetCorePackage(null);
    }

    //模块包信任对话框

    private bool _isTrustPackageDialogFinished = false;
    private bool _trustPackageDialogResult = false;
    private GamePackage _trustPackageCurrent = null;
    
    internal bool GetTrustPackageDialogResult() { return _isTrustPackageDialogFinished; }
    internal bool IsTrustPackageDialogFinished() { return _trustPackageDialogResult; }
    internal void TrustPackageDialogFinish(bool result, bool delete) { 
      _trustPackageDialogResult = result;
      _isTrustPackageDialogFinished = true;
      if(!result && delete) {
        UnLoadPackage(_trustPackageCurrent.PackageName, true);
        //删除
        if(File.Exists(_trustPackageCurrent.PackageFilePath))
          File.Delete(_trustPackageCurrent.PackageFilePath);
      }
    }
    //检测模块是否信任加载
    internal bool IsTrustPackage(string packageName) {
      if (registeredPackages.TryGetValue(packageName, out var outPackage))
        return outPackage.trustPackage;
      return false;
    }
    internal void ShowTrustPackageDialog(GamePackage p) { 
      _isTrustPackageDialogFinished = false;
      _trustPackageDialogResult = false;
      _trustPackageCurrent = p;

      GameUIManager uIManager = GameSystem.GetSystemService("GameUIManager") as GameUIManager;
      Dictionary<string, string> options = new Dictionary<string, string>();
      options.Add("PackageName", p.PackageName);
      options.Add("PackageTitle", p.BaseInfo.Name);
      uIManager.GoPageWithOptions("PagePackageManagerTrust", options);
    }

    /// <summary>
    /// 注册模块
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <param name="load">是否立即加载</param>
    /// <returns>返回是否加载成功。要获得错误代码，请获取 <see cref="GameErrorChecker.LastError"/></returns>
    [LuaApiDescription("注册模块", "返回是否加载成功。要获得错误代码，请获取 GameErrorChecker.LastError")]
    [LuaApiParamDescription("packageName", "包名")]
    [LuaApiParamDescription("load", "是否立即加载")]
    public async Task<bool> RegisterPackage(string packageName)
    {
      bool forceEnablePackage = false;
      if (packageName.StartsWith("Enable:"))
      {
        packageName = packageName.Substring(7);
        forceEnablePackage = true;
      }

      if (packagesLoadStatus.ContainsKey(packageName))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.IsLoading, TAG, "Package {0} is loading", packageName);
        return false;
      }

      if (registeredPackages.ContainsKey(packageName))
      {
        Log.W(TAG, "Package {0} already registered!", packageName);
        return true;
      }

      string realPackagePath = null;
      GamePackage gamePackage = null;

      realPackagePath = GamePathManager.DEBUG_PACKAGE_FOLDER + "/" + packageName;
      if (packageName == CORE_PACKAGE_NAME)
      {
#if UNITY_EDITOR
        realPackagePath = ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ASSET_PATH;
        if (DebugSettings.Instance.PackageLoadWay == LoadResWay.InUnityEditorProject && Directory.Exists(realPackagePath)) {
          gamePackage = GamePackage.GetCorePackage();
        } else
#else
        if(true) 
#endif
        {
          gamePackage = GamePackage.GetCorePackage();
          realPackagePath = GamePathManager.GetResRealPath("core", "core.ballance");
        }
      }
#if UNITY_EDITOR
      //在编辑器中加载
      else if (DebugSettings.Instance.PackageLoadWay == LoadResWay.InUnityEditorProject && Directory.Exists(realPackagePath))
      {
        gamePackage = new GameEditorDebugPackage();
        Log.D(TAG, "Load package in editor : {0}", realPackagePath);
      }
      else
#else
      else if(true) 
#endif
      {
        //路径转换
        realPackagePath = GamePathManager.GetResRealPath("package", packageName + ".ballance");
        string realPackagePathInCore = GamePathManager.GetResRealPath("core", packageName + ".ballance");

        //如果路径在Core中存在，则使用Core加载
        if (PathUtils.Exists(realPackagePathInCore)) realPackagePath = realPackagePathInCore;
        else if (realPackagePath.Contains("jar:file://") || PathUtils.Exists(realPackagePath))
          realPackagePathInCore = "";
        else if (realPackagePathInCore.Contains("jar:file://") || PathUtils.Exists(realPackagePathInCore)) //如果路径在Packages中存在，则使用Packages加载
          realPackagePath = realPackagePathInCore;
        else {
          //不存在则抛出异常
          Log.E(TAG, "Package {0} register failed because file {1} not found", packageName, realPackagePath);
          return false;
        }
      }

      //设置正在加载
      packagesLoadStatus.Add(packageName, 1);

      Log.D(TAG, "Registing package {0}", packageName);

      if (gamePackage == null)
      {
        //判断文件类型
        if (realPackagePath.Contains("jar:file://") || FileUtils.TestFileIsZip(realPackagePath))
          gamePackage = new GameZipPackage();
        else if (FileUtils.TestFileIsAssetBundle(realPackagePath))
          gamePackage = new GameAssetBundlePackage();
        else
        {
          packagesLoadStatus.Remove(packageName);
          //文件格式不支持
          GameErrorChecker.LastError = GameError.NotSupportFileType;
          Log.E(TAG, "Package file type not support {0}", realPackagePath);
          return false;
        }
      }

      gamePackage._Status = GamePackageStatus.Registing;

      //加载信息
      if (await gamePackage.LoadInfo(realPackagePath))
      {

        packagesLoadStatus.Remove(packageName);
        if (!registeredPackages.ContainsKey(packageName))
        {
          var info = new GamePackageRegisterInfo(gamePackage);
          info.enableLoad = forceEnablePackage;
          registeredPackages.Add(packageName, info);
        } 
        else 
        {
          var info = registeredPackages[packageName];
          info.enableLoad = forceEnablePackage;
          info.package = gamePackage;
        }

        gamePackage._Status = GamePackageStatus.Registered;

        Log.D(TAG, "Package {0} registered", packageName);
        //通知事件
        GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_PACKAGE_REGISTERED, packageName);

        return true;
      }
      else
      {
        Log.E(TAG, "Package {0} failed LoadInfo", packageName);
      }

      packagesLoadStatus.Remove(packageName);
      return false;
    }
    /// <summary>
    /// 查找已注册的模块
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <returns>返回模块实例，如果未找到，则返回null</returns>
    [LuaApiDescription("查找已注册的模块", "返回模块实例，如果未找到，则返回null")]
    [LuaApiParamDescription("packageName", "包名")]
    public GamePackage FindRegisteredPackage(string packageName)
    {
      registeredPackages.TryGetValue(packageName, out var outPackage);
      return outPackage != null ? outPackage.package : null;
    }
    /// <summary>
    /// 检测模块是否用户选择了启用
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <returns></returns>
    [LuaApiDescription("检测模块是否用户选择了启用", "")]
    [LuaApiParamDescription("packageName", "包名")]
    public bool IsPackageEnableLoad(string packageName)
    {
      registeredPackages.TryGetValue(packageName, out var outPackage);
      return outPackage != null && outPackage.enableLoad;
    }
    /// <summary>
    /// 取消注册模块
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <param name="unLoadImmediately">是否立即卸载</param>
    /// <returns>返回是否成功</returns>
    [LuaApiDescription("取消注册模块", "返回是否成功")]
    [LuaApiParamDescription("packageName", "包名")]
    [LuaApiParamDescription("unLoadImmediately", "是否立即卸载")]
    public bool UnRegisterPackage(string packageName, bool unLoadImmediately)
    {
      bool success = false;

      GamePackage package = FindPackage(packageName);
      if (package != null)
      {
        if ((package.GetFlag() & GamePackage.FLAG_PACK_SYSTEM_PACKAGE) == GamePackage.FLAG_PACK_SYSTEM_PACKAGE) {
          GameErrorChecker.SetLastErrorAndLog(GameError.AccessDenined, TAG, "Can not unload system package " + packageName + " ! ");
          return false;
        }
        if ((package.GetFlag() & GamePackage.FLAG_PACK_NOT_UNLOADABLE) == GamePackage.FLAG_PACK_NOT_UNLOADABLE) {
          GameErrorChecker.SetLastErrorAndLog(GameError.AccessDenined, TAG, "Can not unload package " + packageName + " with flag FLAG_PACK_NOT_UNLOADABLE ! ");
          return false;
        }
      }

      if (registeredPackages.ContainsKey(packageName))
      {
        registeredPackages.Remove(packageName);
        success = true;
      }
      if (packagesLoadStatus.ContainsKey(packageName))
        packagesLoadStatus.Remove(packageName);

      if (IsPackageLoaded(packageName))
        UnLoadPackage(packageName, unLoadImmediately);

      Log.D(TAG, "Package {0} unregistered", packageName);
      //通知事件
      GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_PACKAGE_UNREGISTERED, packageName);
      return success;
    }

    /// <summary>
    /// 获取模块是否正在加载
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <returns></returns>
    [LuaApiDescription("获取模块是否正在加载", "")]
    [LuaApiParamDescription("packageName", "包名")]
    public bool IsPackageLoading(string packageName)
    {
      return packagesLoadStatus.ContainsKey(packageName) && packagesLoadStatus[packageName] == 2;
    }
    /// <summary>
    /// 获取模块是否正在注册
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <returns></returns>
    [LuaApiDescription("获取模块是否正在注册", "")]
    [LuaApiParamDescription("packageName", "包名")]
    public bool IsPackageRegistering(string packageName)
    {
      return packagesLoadStatus.ContainsKey(packageName) && packagesLoadStatus[packageName] == 1;
    }
    /// <summary>
    /// 获取模块是否注册
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <returns></returns>
    [LuaApiDescription("获取模块是否注册", "")]
    [LuaApiParamDescription("packageName", "包名")]
    public bool IsPackageRegistered(string packageName)
    {
      return registeredPackages.ContainsKey(packageName);
    }
    /// <summary>
    /// 获取模块是否已加载
    /// </summary>
    /// <param name="packageName">包名</param>
    /// <returns></returns>
    [LuaApiDescription("获取模块是否已加载", "")]
    [LuaApiParamDescription("packageName", "包名")]
    public bool IsPackageLoaded(string packageName)
    {
      return loadedPackages.ContainsKey(packageName);
    }

    /// <summary>
    /// 通知模块运行
    /// </summary>
    /// <param name="packageNameFilter">包名筛选，为“*”时表示所有包，为正则表达式时使用正则匹配包。</param>
    [LuaApiDescription("通知模块运行")]
    [LuaApiParamDescription("packageNameFilter", "包名筛选，为“*”时表示所有包，为正则表达式时使用正则匹配包。")]
    public void NotifyAllPackageRun(string packageNameFilter)
    {
      Profiler.BeginSample("NotifyAllPackageRun");

      foreach (GamePackage package in loadedPackages.Values)
      {
        if (package.Status == GamePackageStatus.LoadSuccess && !package.IsEntryCodeExecuted() &&
            (packageNameFilter == "*" || Regex.IsMatch(package.PackageName, packageNameFilter))) 
          package.RunPackageExecutionCode();
      }

      Profiler.EndSample();
    }

    /// <summary>
    /// 加载模块
    /// </summary>
    /// <param name="packageName">模块包名</param>
    /// <returns>返回加载是否成功</returns>
    [LuaApiDescription("加载模块", "返回加载是否成功")]
    [LuaApiParamDescription("packageName", "模块包名")]
    public async Task<bool> LoadPackage(string packageName)
    {
      if (IsPackageLoaded(packageName))
        return true;
      if (IsPackageLoading(packageName))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.IsLoading, TAG,
            "Package {0} is loading!", packageName);
        return false;
      }

      //注册包
      GamePackage package = FindRegisteredPackage(packageName);
      if (package == null)
      {
        if (!await RegisterPackage(packageName))
        {
          packagesLoadStatus.Remove(packageName);

          var msg = string.Format("Package {0} could not load, because RegisterPackage failed", packageName);
          //通知事件
          GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_FAILED, packageName, msg);
          Log.E(TAG, msg);
          return false;
        }

        package = FindRegisteredPackage(packageName);
      }

      packagesLoadStatus.Add(packageName, 2);
      package._Status = GamePackageStatus.Loading;

      Log.D(TAG, "Loading package {0}", packageName);

      //加载依赖
      List<GamePackageDependencies> dependencies = package.BaseInfo.Dependencies;
      if (dependencies.Count > 0)
      {
        GamePackageDependencies dependency;
        GamePackage dependencyPackage;

        //加载依赖
        for (int i = 0; i < dependencies.Count; i++)
        {
          dependency = dependencies[i];
          if (!IsPackageLoaded(dependency.Name))
          {
            bool loadSuccess = await LoadPackage(dependency.Name);
            if (!loadSuccess)
            {
              packagesLoadStatus.Remove(packageName);
              string err = string.Format("Package {0} load failed because a dependency {1}/{2} " +
                 "load failed",
                 packageName, dependency.Name, dependency.MinVersion);

              Log.E(TAG, err);
              //通知事件
              GameManager.GameMediator.DispatchGlobalEvent(
                  GameEventNames.EVENT_PACKAGE_LOAD_FAILED, packageName, err);

              package._Status = GamePackageStatus.LoadFailed;
              return false;
            }
          }
        }
        //检查依赖版本
        for (int i = 0; i < dependencies.Count; i++)
        {
          dependency = dependencies[i];

          if (IsPackageLoaded(dependency.Name))
          {
            dependencyPackage = loadedPackages[dependency.Name];
            if (dependencyPackage.PackageVersion < dependency.MinVersion)
            {
              string err = string.Format("Package {0} load failed because dependency {1} {2} " +
                  "less than required version {3}",
                  packageName, dependency.Name, dependencyPackage.PackageVersion,
                  dependency.MinVersion);

              packagesLoadStatus.Remove(packageName);
              package._Status = GamePackageStatus.LoadFailed;

              Log.E(TAG, err);
              //通知事件
              GameManager.GameMediator.DispatchGlobalEvent(
                  GameEventNames.EVENT_PACKAGE_LOAD_FAILED, packageName, err);

              return false;
            }
            //添加依赖计数
            dependencyPackage.DependencyRefCount++;
          }
        }
      }

      //加载
      if (!await package.LoadPackage())
      {
        package._Status = GamePackageStatus.LoadFailed;
        packagesLoadStatus.Remove(packageName);

        string err = string.Format("Package {0} load failed {1}", packageName, GameErrorChecker.GetLastErrorMessage());
        Log.E(TAG, err);

        //通知事件
        GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_FAILED, packageName, err);
        return false;
      }

      package._Status = GamePackageStatus.LoadSuccess;
      loadedPackages.Add(packageName, package);
      packagesLoadStatus.Remove(packageName);

      Log.D(TAG, "Package {0} loaded", packageName);

      //通知事件
      GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_PACKAGE_LOAD_SUCCESS, packageName, package);

      return true;
    }
    /// <summary>
    /// 卸载模块
    /// </summary>
    /// <param name="packageName">模块包名</param>
    /// <param name="unLoadImmediately">
    /// 是否立即卸载，如果为false，此模块
    /// 将等待至依赖它的模块全部卸载之后才会卸载
    /// </param>
    /// <returns>返回是否成功</returns>
    [LuaApiDescription("卸载模块", "返回加载是否成功")]
    [LuaApiParamDescription("packageName", "模块包名")]
    [LuaApiParamDescription("unLoadImmediately", "是否立即卸载，如果为false，此模块将等待至依赖它的模块全部卸载之后才会卸载")]
    public bool UnLoadPackage(string packageName, bool unLoadImmediately)
    {
      GamePackage package = FindPackage(packageName);
      if (package == null)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotLoad, TAG, "Can not unload package " + packageName + " because it isn't load! ");
        return false;
      }
      if ((package.GetFlag() & GamePackage.FLAG_PACK_SYSTEM_PACKAGE) == GamePackage.FLAG_PACK_SYSTEM_PACKAGE) {
        GameErrorChecker.SetLastErrorAndLog(GameError.AccessDenined, TAG, "Can not unload system package " + packageName + " ! ");
        return false;
      }
      if ((package.GetFlag() & GamePackage.FLAG_PACK_NOT_UNLOADABLE) == GamePackage.FLAG_PACK_NOT_UNLOADABLE) {
        GameErrorChecker.SetLastErrorAndLog(GameError.AccessDenined, TAG, "Can not unload package " + packageName + " with flag FLAG_PACK_NOT_UNLOADABLE ! ");
        return false;
      }
      return UnLoadPackageInternal(package, unLoadImmediately);
    }

    private bool UnLoadPackageInternal(GamePackage package, bool unLoadImmediately)
    {
      var packageName = package.PackageName;

      Log.D(TAG, "Unload package {0}", packageName);

      Profiler.BeginSample("UnLoadPackageInternal" + package.PackageName);

      if (package.UnLoadWhenDependencyRefNone)
        return true;

      //如果不是立即卸载并且依赖引用计数大于0
      if (!unLoadImmediately && package.DependencyRefCount > 0)
      {
        package.UnLoadWhenDependencyRefNone = true;
        package._Status = GamePackageStatus.UnloadWaiting;
        Log.D(TAG, "Set package {0} to unload waiting", packageName);
        return true;
      }

      //依赖计数处理，在这里当前模块卸载了，所以要将他们的依赖计数减一
      List<GamePackageDependencies> dependencies = package.BaseInfo.Dependencies;
      GamePackage dependencyPackage;
      for (int i = 0; i < dependencies.Count; i++)
      {
        if (IsPackageLoaded(dependencies[i].Name))
        {
          dependencyPackage = loadedPackages[dependencies[i].Name];
          dependencyPackage.DependencyRefCount--;
          if (dependencyPackage.DependencyRefCount <= 0
              && dependencyPackage.UnLoadWhenDependencyRefNone)
            UnLoadPackage(dependencyPackage.PackageName, true);
        }
      }

      Profiler.EndSample();

      //卸载

      //通知事件
      if(GameManager.GameMediator)
        GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_PACKAGE_UNLOAD, packageName, package);

      package._Status = GamePackageStatus.NotLoad;
      package.Destroy();
      packagesLoadStatus.Remove(packageName);
      loadedPackages.Remove(packageName);

      Log.D(TAG, "Package {0} unloaded", packageName);
      return true;
    }

    /// <summary>
    /// 查找已加载的模块
    /// </summary>
    /// <param name="packageName">模块包名</param>
    /// <returns>返回模块实例，如果未找到，则返回null</returns>
    [LuaApiDescription("查找已加载的模块", "返回模块实例，如果未找到，则返回null")]
    [LuaApiParamDescription("packageName", "模块包名")]
    public GamePackage FindPackage(string packageName)
    {
      loadedPackages.TryGetValue(packageName, out var outPackage);
      return outPackage;
    }

    /// <summary>
    /// 检查指定需求的模块是否加载
    /// </summary>
    /// <param name="packageName">模块包名</param>
    /// <param name="ver">模块所须最小版本</param>
    /// <returns>如果已加载并且版本符合</returns>
    [LuaApiDescription("检查指定需求的模块是否加载", "如果已加载并且版本符合")]
    [LuaApiParamDescription("packageName", "模块包名")]
    [LuaApiParamDescription("ver", "模块所须最小版本")]
    public bool CheckRequiredPackage(string packageName, int ver)
    {
      if (loadedPackages.TryGetValue(packageName, out var outPackage))
        return outPackage.PackageVersion >= ver;
      return false;
    }

    /// <summary>
    /// 卸载所有包
    /// </summary>
    private void UnLoadAllPackages()
    {
      List<string> packageNames = new List<string>(loadedPackages.Keys);
      foreach (string key in packageNames) {
        if (key != SYSTEM_PACKAGE_NAME) {
          GamePackage package = FindPackage(key);
          if (package != null)
            UnLoadPackageInternal(package, true);
        }
      }
      packageNames.Clear();
    }

    #endregion

    #region 模块管理窗口

    private void InitPackageCommands()
    {
      var srv = GameManager.Instance.GameDebugCommandServer;
      srv.RegisterCommand("pm", (keyword, fullCmd, argsCount, args) =>
      {
        var type = (string)args[0];
        switch (type)
        {
          case "load":
            {
              string packageName = "";
              if (!DebugUtils.CheckDebugParam(1, args, out packageName)) break;
              Task<bool> task = LoadPackage(args[1]);
              return true;
            }
          case "unload":
            {
              string packageName = "";
              bool unLoadImmediately;
              if (!DebugUtils.CheckDebugParam(1, args, out packageName)) break;
              DebugUtils.CheckBoolDebugParam(2, args, out unLoadImmediately, false, false);

              UnLoadPackage(args[1], unLoadImmediately);
              return true;
            }
          case "unload-all":
            {
              UnLoadAllPackages();
              return true;
            }
          case "info":
            {
              string packageName = "";
              if (!DebugUtils.CheckDebugParam(1, args, out packageName)) break;

              GamePackage p = FindPackage(packageName);
              if(p == null) {
                Log.E(TAG, "找不到模块：" + packageName);
                break;
              }

              Log.V(TAG, p.ToString());
              return true;
            }
          case "list-res":
            {
              string packageName = "";
              if (!DebugUtils.CheckDebugParam(1, args, out packageName)) break;

              GamePackage p = FindPackage(packageName);
              if(p == null) {
                Log.E(TAG, "找不到模块：" + packageName);
                break;
              }

              Log.V(TAG, p.ListResource());
              return true;
            }
          case "enable":
            {
              string packageName = "";
              if (!DebugUtils.CheckDebugParam(1, args, out packageName)) break;

              SetPackageEnableLoad(args[1], true);
              return true;
            }
          case "disable":
            {
              string packageName = "";
              if (!DebugUtils.CheckDebugParam(1, args, out packageName)) break;

              SetPackageEnableLoad(args[1], false);
              return true;
            }
          case "reg":
            {
              string packageName = "";
              if (!DebugUtils.CheckDebugParam(1, args, out packageName)) break;

              Task<bool> task = RegisterPackage(packageName);
              return true;
            }
          case "list-loaded":
            {
              foreach (var i in loadedPackages)
                Log.V(TAG, string.Format("{0} => {1}", i.Value.PackageName, i.Value.Status));
              return true;
            }
          case "list-reged":
            {
              foreach (var i in registeredPackages)
                Log.V(TAG, string.Format("{0} enableLoad: {1}", i.Value.packageName, i.Value.enableLoad));
              return true;
            }
          case "notify-run":
            {
              string packageNameFilter = "";
              if (!DebugUtils.CheckDebugParam(1, args, out packageNameFilter)) break;

              NotifyAllPackageRun(packageNameFilter);
              return true;
            }
        }
        return false;
      }, 1, "pm <reg/load/info/unload/list-loaded/list-reged/notify-run/wnd> [packageName] 模块管理器命令\n" +
              "  reg <packageName:string>                                ▶ 注册一个模块包\n" +
              "  load <packageName:string>                               ▶ 加载一个模块包\n" +
              "  info <packageName:string>                               ▶ 显示一个模块包的信息\n" +
              "  list-res <packageName:string>                           ▶ 显示一个模块包的所有资源列表\n" +
              "  unload <packageName:string> [unLoadImmediately:boolean] ▶ 卸载一个模块包, unLoadImmediately指定是否立即卸载，默认false\n" +
              "  enable <packageName:string>                             ▶ 启用模块包启动加载\n" +
              "  disable <packageName:string>                            ▶ 禁用模块包启动加载\n" +
              "  list-loaded                                             ▶ 列举出已加载的模块包\n" +
              "  list-regedad                                            ▶ 列举出已注册的模块包\n" +
              "  notify-run <packageNameFilter>                          ▶ 通知模块包运行，packageNameFilter为包名筛选，为“*”时表示所有包，为正则表达式时使用正则匹配包。\n" +
              "  wnd                                                     ▶ 显示模块管理器窗口");
    }

    #endregion

    #region 公用加载资源方法

    private GamePackage TryGetPackageByPath(string pathOrName, out string resName)
    {
      var lastIdx = pathOrName.IndexOf("__/");
      if (pathOrName.StartsWith("__") && lastIdx >= 0)
      {
        var packname = pathOrName.Substring(2, lastIdx - 2);
        GamePackage pack = null;
        switch(packname) {
          case "system":
            pack = GamePackage.GetSystemPackage();
            break;
          case "game":
          case "ballance":
          case "core":
            pack = GamePackage.GetCorePackage();
            break;
          default:
            pack = FindPackage(packname);
            break;
        }
        if (pack == null)
          throw new System.Exception("Package " + packname + " not found");
        resName = pathOrName.Substring(lastIdx + 3);
        return pack;
      }
      else
      {
        resName = pathOrName;
        return GamePackage.GetSystemPackage();
      }
    }
    
    /// <summary>
    /// 全局读取资源包中的代码资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回CodeAsset实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("全局读取资源包中的代码资源", "返回CodeAsset实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public GamePackage.CodeAsset GetCodeAsset(string pathorname, out GamePackage package)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      package = pack;
      return pack.GetCodeAsset(path);
    }  
    /// <summary>
    /// 全局读取资源包中的文字资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回TextAsset实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("读取模块资源包中的文字资源", "返回TextAsset实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public TextAsset GetTextAsset(string pathorname)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      return pack.GetTextAsset(path);
    }
    /// <summary>
    /// 全局读取模块资源包中的 Prefab 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 GameObject 实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("读取模块资源包中的 Prefab 资源", "返回资源实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public GameObject GetPrefabAsset(string pathorname)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      return pack.GetPrefabAsset(path);
    }
    /// <summary>
    /// 全局读取模块资源包中的 Texture 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Texture 实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("读取模块资源包中的 Texture 资源", "返回资源实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public Texture GetTextureAsset(string pathorname)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      return pack.GetTextureAsset(path);
    }
    /// <summary>
    /// 全局读取模块资源包中的 Texture2D 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Texture2D 实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("读取模块资源包中的 Texture2D 资源", "返回资源实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public Texture2D GetTexture2DAsset(string pathorname)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      return pack.GetTexture2DAsset(path);
    }
    /// <summary>
    /// 全局读取模块资源包中的 Sprite 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Sprite 实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("读取模块资源包中的 Sprite 资源", "返回资源实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public Sprite GetSpriteAsset(string pathorname)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      return pack.GetSpriteAsset(path);
    }
    /// <summary>
    /// 全局读取模块资源包中的 Material 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Material 实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("读取模块资源包中的 Material 资源", "返回资源实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public Material GetMaterialAsset(string pathorname)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      return pack.GetMaterialAsset(path);
    }
    /// <summary>
    /// 全局读取模块资源包中的 AudioClip 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 AudioClip 实例，如果未找到，则返回null</returns>
    /// <exception cref="RequireFailedException">
    /// 未找到指定的模块包。
    /// </exception>
    [LuaApiDescription("读取模块资源包中的 AudioClip 资源", "返回资源实例，如果未找到，则返回null")]
    [LuaApiParamDescription("pathorname", "资源路径")]
    public AudioClip GetAudioClipAsset(string pathorname)
    {
      var pack = TryGetPackageByPath(pathorname, out var path);
      return pack.GetAudioClipAsset(path);
    }

    #endregion
  }
}
