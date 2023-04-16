using Ballance2.Config;
using Ballance2.Services.Debug;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Profiling;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePackage.cs
* 
* 用途：
* 游戏模块的声明以及模块功能提供。
* 负责：模块运行环境初始化卸载、资源读取相关。
*
* 作者：
* mengyu
*/

namespace Ballance2.Package
{
  /// <summary>
  /// 模块包实例
  /// </summary>
  public class GamePackage
  {
    /// <summary>
    /// 标签
    /// </summary>
    public string TAG
    {
      get { return "GamePackage:" + PackageName; }
    }

    internal int DependencyRefCount = 0;
    internal bool UnLoadWhenDependencyRefNone = false;

    public virtual Task<bool> LoadInfo(string filePath)
    {
      if (filePath != null)
        PackageFilePath = filePath;
      var t = new Task<bool>(() => { return true; });
      t.Start();
      return t;
    }
    public virtual async Task<bool> LoadPackage()
    {
      LoadI18NResource();

      //模块代码环境初始化
      if (Type == GamePackageType.Module)
        return LoadPackageCodeBase();
      var t = new Task<bool>(() => {
        return true; 
      });
      t.Start();
      return await t;
    }
    public virtual void Destroy()
    {
      Log.D(TAG, "Destroy package {0}", PackageName);

      if(!IsUnloadCodeExecuted())
        RunPackageBeforeUnLoadCode();

      //释放AssetBundle
      if (AssetBundle != null)
      {
        AssetBundle.Unload(true);
        AssetBundle = null;
      }

      if (CSharpAssembly != null)
        CSharpAssembly = null;
      if (PackageEntry != null)
        PackageEntry = null;
    }

    #region 系统包

#if UNITY_EDITOR
    private static GamePackage _SystemPackage = new GameEditorSystemPackage();
#else
    private static GamePackage _SystemPackage = new GameSystemPackage();
#endif

    /// <summary>
    /// 获取系统核心的模块包，包名是 system 。
    /// </summary>
    /// <returns></returns>
    public static GamePackage GetSystemPackage() { 
      return _SystemPackage; 
    }

    #endregion

    #region 常量定义

    public const int FLAG_CODE_BASE_LOADED = 0x000000001;
    public const int FLAG_CODE_ENTRY_CODE_RUN = 0x000000008;
    public const int FLAG_CODE_UNLOD_CODE_RUN = 0x000000010;
    public const int FLAG_PACK_NOT_UNLOADABLE = 0x000000020;
    public const int FLAG_PACK_SYSTEM_PACKAGE = 0x000000040;

    #endregion

    #region 模块运行环境

    /// <summary>
    /// C# 程序集
    /// </summary>
    public Assembly CSharpAssembly { get; protected set; }
    /// <summary>
    /// 程序入口
    /// </summary>
    public GamePackageEntry PackageEntry = new GamePackageEntry();
    
    internal int flag = 0;

    /// <summary>
    /// 获取是否可以卸载
    /// </summary>
    /// <returns></returns>
    public bool IsNotUnLoadable() { return (flag & FLAG_PACK_NOT_UNLOADABLE) == FLAG_PACK_NOT_UNLOADABLE; }
    /// <summary>
    /// 获取是否是系统包
    /// </summary>
    /// <returns></returns>
    public bool IsSystemPackage() { return (flag & FLAG_PACK_SYSTEM_PACKAGE) == FLAG_PACK_SYSTEM_PACKAGE; }
    /// <summary>
    /// 获取入口代码是否已经运行过
    /// </summary>
    /// <returns></returns>
    public bool IsEntryCodeExecuted() { return (flag & FLAG_CODE_ENTRY_CODE_RUN) == FLAG_CODE_ENTRY_CODE_RUN; }
    /// <summary>
    /// 获取出口代码是否已经运行过
    /// </summary>
    /// <returns></returns>
    public bool IsUnloadCodeExecuted() { return (flag & FLAG_CODE_UNLOD_CODE_RUN) == FLAG_CODE_UNLOD_CODE_RUN; }

    protected virtual Assembly LoadCodeCSharp(string pathorname) {
#if UNITY_EDITOR
      return Assembly.GetAssembly(typeof(GameSystem));
#else
      throw new NotImplementedException();
#endif
    }

    /// <summary>
    /// 设置当前模块的标志位
    /// </summary>
    /// <param name="flag">标志位，（GamePackage.FLAG_*）</param>
    public void SetFlag(int flag)  {

      if((this.flag & FLAG_PACK_NOT_UNLOADABLE) == FLAG_PACK_NOT_UNLOADABLE && (flag & FLAG_PACK_NOT_UNLOADABLE) != FLAG_PACK_NOT_UNLOADABLE) {
        Log.E(TAG, "Not allow set FLAG_PACK_NOT_UNLOADABLE flag for not unloadable packages.");
        flag |= FLAG_PACK_NOT_UNLOADABLE;
      }
      if((this.flag & FLAG_PACK_SYSTEM_PACKAGE) == FLAG_PACK_SYSTEM_PACKAGE && (flag & FLAG_PACK_NOT_UNLOADABLE) != FLAG_PACK_NOT_UNLOADABLE) {
        Log.E(TAG, "Not allow set FLAG_PACK_NOT_UNLOADABLE flag for not system packages.");
        flag |= FLAG_PACK_NOT_UNLOADABLE;
      }
      this.flag = flag;
    }
    /// <summary>
    /// 获取当前模块的标志位
    /// </summary>
    public int GetFlag() { return flag; }

    /// <summary>
    /// 加载运行环境代码
    /// </summary>
    /// <returns></returns>
    protected bool LoadPackageCodeBase() {
      //判断是否初始化
      if((FLAG_CODE_BASE_LOADED & flag) == FLAG_CODE_BASE_LOADED) {
        Log.E(TAG, "不能重复初始化");
        return false;
      }
      if (IsSystemPackage())
        return true;

      Type type = null;

      #if UNITY_EDITOR

      CSharpAssembly = Assembly.GetAssembly(typeof(GamePackage));
      type = CSharpAssembly.GetType(PackageName + ".PackageEntry");

      #else

      //加载C#程序集
      CSharpAssembly = LoadCodeCSharp(PackageName + ".dll");
      if (CSharpAssembly == null) {
        Log.E(TAG, "无法加载DLL：" + PackageName + ".dll");
        return false;
      }
      type = CSharpAssembly.GetType("PackageEntry");

      #endif
      
      if (type == null)
      {
        Log.W(TAG, "未找到 PackageEntry ");
        GameErrorChecker.LastError = GameError.ClassNotFound;
        return false;
      }
      else
      {
        try {
          object CSharpPackageEntry = Activator.CreateInstance(type);
          MethodInfo methodInfo = type.GetMethod("Main");  //根据方法名获取MethodInfo对象
          if (type == null)
          {
            Log.W(TAG, "未找到 PackageEntry.Main()");
            GameErrorChecker.LastError = GameError.FunctionNotFound;
          } 
          else  
          {
            object b = methodInfo.Invoke(null, new object[0]);
            if (b.GetType() == typeof(GamePackageEntry)) 
            {
              PackageEntry = b as GamePackageEntry;
            } 
            else
            {
              Log.W(TAG, "模块 PackageEntry.Main 返回了未知类型");
              return false;
            }
          }
        } catch (System.Exception e) {
          Log.W(TAG, "模块 PackageEntry.Main 执行失败: " + e.ToString());
          return false;
        }
      }
      flag |= FLAG_CODE_BASE_LOADED;
      return true;
    }    

    /// <summary>
    /// 运行模块初始化代码，模块的 初始化代码 只能运行一次，不能重复运行。
    /// </summary>
    /// <returns>返回是否成功</returns>
    public bool RunPackageExecutionCode()
    {
      if (Type != GamePackageType.Module)
      {
        GameErrorChecker.LastError = GameError.PackageCanNotRun;
        return false;
      }
      if (IsEntryCodeExecuted()) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "Run ExecutionCode failed, an not run twice");
        return false;
      }

      flag |= FLAG_CODE_ENTRY_CODE_RUN;
      flag ^= FLAG_CODE_UNLOD_CODE_RUN;

      if(PackageEntry.OnLoad != null) {
        
        Profiler.BeginSample(TAG + "PackageEntry.OnLoad");

        bool result = PackageEntry.OnLoad.Invoke(this);

        Profiler.EndSample();

        return result;
      }
      return true;
    }
    /// <summary>
    /// 运行模块卸载回调，模块的 卸载回调 只能运行一次，不能重复运行。
    /// </summary>
    /// <returns>返回是否成功</returns>
    public bool RunPackageBeforeUnLoadCode()
    {
      if (Type != GamePackageType.Module)
      {
        GameErrorChecker.LastError = GameError.PackageCanNotRun;
        return false;
      }
      if (IsUnloadCodeExecuted()) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "Run BeforeUnLoadCode failed, an not run twice");
        return false;
      }

      flag |= FLAG_CODE_UNLOD_CODE_RUN;
      flag ^= FLAG_CODE_ENTRY_CODE_RUN;

      if(PackageEntry.OnBeforeUnLoad != null) {
        
        Profiler.BeginSample(TAG + "PackageEntry.OnBeforeUnLoad");
        
        bool result = PackageEntry.OnBeforeUnLoad.Invoke(this);

        Profiler.EndSample();

        return result;
      }
      return true;
    }

    #endregion

    #region 模块信息

    private int VerConverter(string s)
    {
      if (s == "{internal.core.version}")
        return GameConst.GameBulidVersion;
      return ConverUtils.StringToInt(s, 0, "Package/version");
    }

    protected bool ReadInfo(XmlDocument xml)
    {
      XmlNode nodePackage = xml.SelectSingleNode("Package");
      XmlAttribute attributeName = nodePackage.Attributes["name"];
      XmlAttribute attributeVersion = nodePackage.Attributes["version"];
      XmlNode nodeBaseInfo = nodePackage.SelectSingleNode("BaseInfo");

      if (attributeName == null)
      {
        LoadError = "PackageDef.xml 配置存在错误 : name 丢失";
        GameErrorChecker.SetLastErrorAndLog(GameError.MissingAttribute, TAG, "Package attribute name is null");
        return false;
      }
      if (attributeVersion == null)
      {
        LoadError = "PackageDef.xml 配置存在错误 : version 丢失";
        GameErrorChecker.SetLastErrorAndLog(GameError.MissingAttribute, TAG, "Package attribute version is null");
        return false;
      }
      if (nodeBaseInfo == null)
      {
        LoadError = "PackageDef.xml 配置存在错误 : BaseInfo 丢失";
        GameErrorChecker.SetLastErrorAndLog(GameError.MissingAttribute, TAG, "Package node BaseInfo is null");
        return false;
      }

      //Version and PackageName
      PackageName = attributeName.Value;
      PackageVersion = VerConverter(attributeVersion.Value);

      //BaseInfo
      BaseInfo = new GamePackageBaseInfo(nodeBaseInfo, this);

      //Compatibility
      XmlNode nodeCompatibility = nodePackage.SelectSingleNode("Compatibility");
      if (nodeCompatibility != null)
        for (int i = 0; i < nodeCompatibility.Attributes.Count; i++)
        {
          switch (nodeCompatibility.ChildNodes[i].Name)
          {
            case "TargetVersion":
              TargetVersion = ConverUtils.StringToInt(nodeCompatibility.ChildNodes[i].InnerText,
                  GameConst.GameBulidVersion, "Compatibility/TargetVersion");
              break;
            case "MinVersion":
              MinVersion = ConverUtils.StringToInt(nodeCompatibility.ChildNodes[i].InnerText,
                  GameConst.GameBulidVersion, "Compatibility/MinVersion");
              break;
          }
        }

      //兼容性检查
      if (MinVersion > GameConst.GameBulidVersion)
      {
        Log.E(TAG, "MinVersion {0} greater than game version {1}", MinVersion, GameConst.GameBulidVersion);
        LoadError = "模块版本与当前游戏不兼容，模块所需版本 >=" + MinVersion;
        GameErrorChecker.LastError = GameError.PackageIncompatible;
        IsCompatible = false;
        return false;
      }
      else
      {
        IsCompatible = true;
      }

      //参数
      XmlNode nodeEntryCode = nodePackage.SelectSingleNode("EntryCode");
      if (nodeEntryCode != null)
        EntryCode = nodeEntryCode.InnerText;
      XmlNode nodeType = nodePackage.SelectSingleNode("Type");
      if (nodeType != null)
        Type = ConverUtils.StringToEnum(nodeType.InnerText, GamePackageType.Asset, "Type");

      return true;
    }

    /// <summary>
    /// 获取模块文件路径
    /// </summary>
    public string PackageFilePath { get; protected set; }
    /// <summary>
    /// 获取模块包名
    /// </summary>
    public string PackageName { get; protected set; }
    /// <summary>
    /// 获取模块版本号
    /// </summary>
    public int PackageVersion { get; protected set; }
    /// <summary>
    /// 获取基础信息
    /// </summary>
    public GamePackageBaseInfo BaseInfo { get; protected set; }
    /// <summary>
    /// 获取模块更新时间
    /// </summary>
    public DateTime UpdateTime { get; protected set; }
    /// <summary>
    /// 获取获取是否是系统必须包
    /// </summary>
    public bool SystemPackage { get; internal set; }

    /// <summary>
    /// 获取模块加载错误
    /// </summary>
    public string LoadError { get; protected set; } = "";
    /// <summary>
    /// 获取模块PackageDef文档
    /// </summary>
    public XmlDocument PackageDef { get; protected set; }
    /// <summary>
    /// 获取模块AssetBundle
    /// </summary>
    public AssetBundle AssetBundle { get; protected set; }
    /// <summary>
    /// 获取表示模块目标游戏内核版本
    /// </summary>
    public int TargetVersion { get; protected set; } = GameConst.GameBulidVersion;
    /// <summary>
    /// 获取表示模块可以正常使用的最低游戏内核版本
    /// </summary>
    public int MinVersion { get; protected set; } = GameConst.GameBulidVersion;
    /// <summary>
    /// 获取模块是否兼容当前内核
    /// </summary>
    public bool IsCompatible { get; protected set; }

    /// <summary>
    /// 获取模块入口代码
    /// </summary>
    public string EntryCode { get; protected set; }
    /// <summary>
    /// 获取模块类型
    /// </summary>
    public GamePackageType Type { get; protected set; } = GamePackageType.Asset;

    internal GamePackageStatus _Status = GamePackageStatus.NotLoad;

    /// <summary>
    /// 获取模块加载状态
    /// </summary>
    public GamePackageStatus Status { get { return _Status; } }
    /// <summary>
    /// 转为字符串显示
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return "Package: " + PackageName + "(" + PackageVersion + ") => " + _Status;
    }
    /// <summary>
    /// 展示所有资源
    /// </summary>
    /// <returns></returns>
    public virtual string ListResource()
    {
      StringBuilder sb = new StringBuilder();
      if(AssetBundle != null) {
        var list = AssetBundle.GetAllAssetNames();
        sb.Append("[AssetBundle " + AssetBundle.name + " assets count: " + list.Length + " ]");
        for (int i = 0; i < list.Length; i++)
          sb.AppendLine(list[i]);
      } else {
        sb.Append("[AssetBundle is null]");
      }
      return sb.ToString();
    }

    #endregion

    #region 资源读取

    /// <summary>
    /// 读取模块资源包中的资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回资源实例，如果未找到，则返回null</returns>
    public virtual T GetAsset<T>(string pathorname) where T : UnityEngine.Object
    {
      if (AssetBundle == null)
      {
        GameErrorChecker.LastError = GameError.NotLoad;
        return null;
      }

      return AssetBundle.LoadAsset<T>(pathorname);
    }

    /// <summary>
    /// 读取模块资源包中的文字资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回TextAsset实例，如果未找到，则返回null</returns>
    public virtual TextAsset GetTextAsset(string pathorname) { return GetAsset<TextAsset>(pathorname); }

    /// <summary>
    /// 读取模块资源包中的 Prefab 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 GameObject 实例，如果未找到，则返回null</returns>
    public virtual GameObject GetPrefabAsset(string pathorname) { return GetAsset<GameObject>(pathorname); }
    /// <summary>
    /// 读取模块资源包中的 Texture 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Texture 实例，如果未找到，则返回null</returns>
    public virtual Texture GetTextureAsset(string pathorname) { return GetAsset<Texture>(pathorname); }
    /// <summary>
    /// 读取模块资源包中的 Texture2D 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Texture2D 实例，如果未找到，则返回null</returns>
    public virtual Texture2D GetTexture2DAsset(string pathorname) { return GetAsset<Texture2D>(pathorname); }
    /// <summary>
    /// 读取模块资源包中的 Sprite 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Sprite 实例，如果未找到，则返回null</returns>
    public virtual Sprite GetSpriteAsset(string pathorname) { return GetAsset<Sprite>(pathorname); }
    /// <summary>
    /// 读取模块资源包中的 Material 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 Material 实例，如果未找到，则返回null</returns>
    public virtual Material GetMaterialAsset(string pathorname) { return GetAsset<Material>(pathorname); }
    /// <summary>
    /// 读取模块资源包中的 PhysicMaterial 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 PhysicMaterial 实例，如果未找到，则返回null</returns>
    public virtual PhysicMaterial GetPhysicMaterialAsset(string pathorname) { return GetAsset<PhysicMaterial>(pathorname); }
    /// <summary>
    /// 读取模块资源包中的 AudioClip 资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>返回 AudioClip 实例，如果未找到，则返回null</returns>
    public virtual AudioClip GetAudioClipAsset(string pathorname) { return GetAsset<AudioClip>(pathorname); }

    #endregion

    #region 模块操作

    private Dictionary<string, string> preI18NResource = new Dictionary<string, string>();

    /// <summary>
    /// 在当前模块中预加载的国际化语言资源寻找字符串
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>返回国际化字符串，如果未找到，则返回null</returns>
    public string GetPackageI18NResourceInPre(string key) {
      if(preI18NResource.TryGetValue(key, out var s))
        return s;
      return null;
    }
    /// <summary>
    /// 预加载国际化语言资源
    /// </summary>
    protected void PreLoadI18NResource(string resString) {
      if (resString != null)
        preI18NResource = I18NProvider.PreLoadLanguageResources(resString);
      else {
        var res = GetTextAsset("PackageLanguageResPre.xml");
        if (res != null)
          preI18NResource = I18NProvider.PreLoadLanguageResources(res.text);
      }
    }
    /// <summary>
    /// 加载模块的国际化语言资源
    /// </summary>
    private void LoadI18NResource()
    {
      var res = GetTextAsset("PackageLanguageRes.xml");
      if (res != null)
      {
        if (!I18NProvider.LoadLanguageResources(res.text))
          Log.E(TAG, "Failed to load PackageLanguageRes.xml for package " + PackageName);
      }
    }

    //自定义数据,方便操作

    private Dictionary<string, object> packageCustomData = new Dictionary<string, object>();

    /// <summary>
    /// 添加自定义数据
    /// </summary>
    /// <param name="name">数据名称</param>
    /// <param name="data">数据值</param>
    /// <returns>返回数据值</returns>
    public object AddCustomProp(string name, object data)
    {
      if (packageCustomData.ContainsKey(name))
      {
        packageCustomData[name] = data;
        return data;
      }
      packageCustomData.Add(name, data);
      return data;
    }
    /// <summary>
    /// 获取自定义数据
    /// </summary>
    /// <param name="name">数据名称</param>
    /// <returns>返回数据值</returns>
    public object GetCustomProp(string name)
    {
      if (packageCustomData.ContainsKey(name))
        return packageCustomData[name];
      return null;
    }
    /// <summary>
    /// 设置自定义数据
    /// </summary>
    /// <param name="name">数据名称</param>
    /// <param name="data"></param>
    /// <returns>返回旧的数据值，如果之前没有该数据，则返回null</returns>
    public object SetCustomProp(string name, object data)
    {
      if (packageCustomData.ContainsKey(name))
      {
        object old = packageCustomData[name];
        packageCustomData[name] = data;
        return old;
      }
      return null;
    }
    /// <summary>
    /// 清除自定义数据
    /// </summary>
    /// <param name="name">数据名称</param>
    /// <returns>返回是否成功</returns>
    public bool RemoveCustomProp(string name)
    {
      if (packageCustomData.ContainsKey(name))
      {
        packageCustomData.Remove(name);
        return true;
      }
      return false;
    }

    #endregion
  }
}
