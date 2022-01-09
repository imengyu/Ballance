using Ballance2.Base;
using Ballance2.Base.Handler;
using Ballance2.Config;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

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
  [JSExport]
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
      PackageFilePath = filePath;
      return null;
    }
    public virtual Task<bool> LoadPackage()
    {
      FixBundleShader();
      LoadI18NResource();

      //模块代码环境初始化
      var t = new Task<bool>(() => {
        if (Type == GamePackageType.Module)
          return LoadPackageCodeBase();
        return true;
      });
      t.Start();
      return t;
    }
    public virtual void Destroy()
    {
      Log.D(TAG, "Destroy package {0}", PackageName);

      RunPackageBeforeUnLoadCode();

      //GameManager.DestroyManagersInMod(PackageName);
      GameManager.GameMediator.UnloadAllPackageActionStore(this);
      HandlerClear();
      ActionClear();

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

    private static GamePackage _SystemPackage = null;

    /// <summary>
    /// 获取系统的模块结构
    /// </summary>
    /// <returns></returns>
    public static void SetSystemPackage(GamePackage pack) { 
      if(_SystemPackage == null)
        _SystemPackage = pack; 
      else 
        GameErrorChecker.SetLastErrorAndLog(GameError.AccessDenined, "GamePackage", "Not allow to chage GamePackage");
    }
    /// <summary>
    /// 获取系统的模块结构
    /// </summary>
    /// <returns></returns>
    public static GamePackage GetSystemPackage() { 
      return _SystemPackage; 
    }

    #endregion

    #region 模块运行环境

    /// <summary>
    /// C# 程序集
    /// </summary>
    public Assembly CSharpAssembly { get; protected set; }
    /// <summary>
    /// 程序入口
    /// </summary>
    public GamePackageEntry PackageEntry = null;
    private bool entryCodeRun = false;
    private bool unloadCodeRun = false;

    /// <summary>
    /// 获取入口代码是否已经运行过
    /// </summary>
    /// <returns></returns>
    public bool IsEntryCodeExecuted() { return entryCodeRun; }

    /// <summary>
    /// 加载运行环境代码
    /// </summary>
    /// <returns></returns>
    protected bool LoadPackageCodeBase() {
      if (CodeType == GamePackageCodeType.JS)
      {
        var ret = GameManager.Instance.GameMainEnv.Eval<bool>("ballance.internal.SystemLoadPackage('" + EntryCode + "','" + PackageName +"')", "ballance-internal:///packageLoader.js?name=" + PackageName);
        if (!ret)
        {
          Log.E(TAG, "模块 PackageEntry 返回了错误");
          GameErrorChecker.LastError = GameError.ExecutionFailed;
          return false;
        }
        return true;
      }
      else if (CodeType == GamePackageCodeType.CSharp)
      {
        //加载C#程序集
        CSharpAssembly = LoadCodeCSharp(EntryCode);
        if (CSharpAssembly == null) {
          Log.E(TAG, "无法加载DLL：" + EntryCode);
          return false;
        }
        
        Type type = CSharpAssembly.GetType("Package");
        if (type == null)
        {
          Log.E(TAG, "未找到 Package ");
          GameErrorChecker.LastError = GameError.ClassNotFound;
          return false;
        }

        object CSharpPackageEntry = Activator.CreateInstance(type);
        MethodInfo methodInfo = type.GetMethod("PackageEntry");  //根据方法名获取MethodInfo对象
        if (type == null)
        {
          Log.E(TAG, "未找到 PackageEntry()");
          GameErrorChecker.LastError = GameError.FunctionNotFound;
          return false;
        }

        object b = methodInfo.Invoke(CSharpPackageEntry, new object[] { this });
        if (b is bool && !((bool)b))
        {
          Log.E(TAG, "模块 PackageEntry 返回了错误");
          GameErrorChecker.LastError = GameError.ExecutionFailed;
          return (bool)b;
        }
        return true;
      }
      else
      {
        Log.E(TAG, "当前模块是普通模块，但是 CodeType 却未配置成为任何一种可运行代码环境，这种情况下此模块将无法运行任何代码，请检查配置是否正确");
      }
      return false;
    }

    /// <summary>
    /// 运行模块初始化代码
    /// </summary>
    /// <returns></returns>
    public bool RunPackageExecutionCode()
    {
      if (Type != GamePackageType.Module)
      {
        GameErrorChecker.LastError = GameError.PackageCanNotRun;
        return false;
      }
      if(entryCodeRun) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "Run ExecutionCode failed, an not run twice");
        return false;
      }

      entryCodeRun = true;
      if(PackageEntry.OnLoad != null)
        return PackageEntry.OnLoad.Invoke(this);
      return true;
    }
    /// <summary>
    /// 运行模块卸载回调
    /// </summary>
    public bool RunPackageBeforeUnLoadCode()
    {
      if (Type != GamePackageType.Module)
      {
        GameErrorChecker.LastError = GameError.PackageCanNotRun;
        return false;
      }
      if (unloadCodeRun) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "Run BeforeUnLoadCode failed, an not run twice");
        return false;
      }

      unloadCodeRun = false;

      if(PackageEntry.OnBeforeUnLoad != null)
        return PackageEntry.OnBeforeUnLoad.Invoke(this);
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
      BaseInfo = new GamePackageBaseInfo(nodeBaseInfo);

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
      XmlNode nodeCodeType = nodePackage.SelectSingleNode("CodeType");
      if (nodeCodeType != null)
        CodeType = ConverUtils.StringToEnum(nodeCodeType.InnerText, GamePackageCodeType.None, "CodeType");
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
    /// <summary>
    /// 获取模块代码类型
    /// </summary>
    public GamePackageCodeType CodeType { get; protected set; } = GamePackageCodeType.None;

    internal GamePackageStatus _Status = GamePackageStatus.NotLoad;

    /// <summary>
    /// 获取模块状态
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

    /// <summary>
    /// 获取指定路径的代码是否存在。
    /// </summary>
    /// <param name="pathorname">代码路径</param>
    /// <returns>返回是否存在</returns>
    public virtual bool CheckCodeAssetExists(string pathorname) {
      return AssetBundle.Contains(pathorname);
    }
    /// <summary>
    /// 读取模块资源包中的代码资源
    /// </summary>
    /// <param name="pathorname">文件名称或路径</param>
    /// <returns>如果读取成功则返回代码内容，否则返回null</returns>
    public virtual CodeAsset GetCodeAsset(string pathorname)
    {
      TextAsset textAsset = GetTextAsset(pathorname);
      if (textAsset != null)
        return new CodeAsset(textAsset.bytes, pathorname, pathorname);

      GameErrorChecker.LastError = GameError.FileNotFound;
      return null;
    }
    /// <summary>
    /// 加载模块资源包中的c#代码资源
    /// </summary>
    /// <param name="pathorname">资源路径</param>
    /// <returns>如果加载成功则返回已加载的Assembly，否则将抛出异常，若当前环境并不支持加载，则返回null</returns>
    public virtual Assembly LoadCodeCSharp(string pathorname)
    {
      GameErrorChecker.SetLastErrorAndLog(GameError.NotSupportFileType, TAG, "当前模块不支持加载 CSharp 代码");
      return null;
    }

    /// <summary>
    /// 表示代码资源
    /// </summary>
    [JSExport]
    public class CodeAsset {
      /// <summary>
      /// 代码字符串
      /// </summary>
      public byte[] data;
      /// <summary>
      /// 获取当前代码的真实路径（一般用于调试）
      /// </summary>
      public string realPath;
      /// <summary>
      /// 代码文件的相对路径
      /// </summary>
      public string relativePath;

      public CodeAsset(byte[] data, string realPath, string relativePath) {
        this.data = data;
        this.realPath = realPath;
        this.relativePath = relativePath;
      }

      /// <summary>
      /// 获取代码字符串
      /// </summary>
      /// <returns></returns>
      public string GetCodeString() {
        return Encoding.UTF8.GetString(data);
      }
    }

    #endregion

    #region 模块操作

    /// <summary>
    /// 修复 模块透明材质 Shader
    /// </summary>
    private void FixBundleShader()
    {
#if UNITY_EDITOR //editor 模式下修复一下透明shader
      if (AssetBundle == null)
        return;

      var materials = AssetBundle.LoadAllAssets<Material>();
      var standardShader = Shader.Find("Standard");
      if (standardShader == null)
        return;

      int _SrcBlend = 0;
      int _DstBlend = 0;

      foreach (Material material in materials)
      {
        var shaderName = material.shader.name;
        if (shaderName == "Standard")
        {
          material.shader = standardShader;

          _SrcBlend = material.renderQueue == 0 ? 0 : material.GetInt("_SrcBlend");
          _DstBlend = material.renderQueue == 0 ? 0 : material.GetInt("_DstBlend");

          if (_SrcBlend == (int)UnityEngine.Rendering.BlendMode.SrcAlpha
              && _DstBlend == (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha)
          {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
          }
        }
      }
#endif
    }
    /// <summary>
    /// 加载模块的国际化语言资源
    /// </summary>
    private void LoadI18NResource()
    {
      var res = GetTextAsset("I18n.xml");
      if (res != null)
      {
        if (!I18NProvider.LoadLanguageResources(res.text))
          Log.E(TAG, "Failed to load I18n.xml for package " + PackageName);
      }
    }

    //自定义数据,方便LUA层操作

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

    #region 模块从属资源处理

    private HashSet<GameHandler> packageHandlers = new HashSet<GameHandler>();
    private HashSet<GameAction> packageActions = new HashSet<GameAction>();

    internal void HandlerReg(GameHandler handler)
    {
      packageHandlers.Add(handler);
    }
    internal void HandlerRemove(GameHandler handler)
    {
      packageHandlers.Remove(handler);
    }
    internal void ActionReg(GameAction action)
    {
      packageActions.Add(action);
    }
    internal void ActionRemove(GameAction action)
    {
      packageActions.Remove(action);
    }

    //释放所有从属于当前模块的GameHandler
    private void HandlerClear()
    {
      List<GameHandler> list = new List<GameHandler>(packageHandlers);
      foreach (GameHandler gameHandler in list)
        gameHandler.Dispose();
      list.Clear();
      packageHandlers.Clear();
    }
    private void ActionClear()
    {
      List<GameAction> list = new List<GameAction>(packageActions);
      foreach (GameAction action in list)
        action.Store.UnRegisterAction(action);
      list.Clear();
      packageActions.Clear();
    }

    #endregion

  }
}
