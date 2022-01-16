using Ballance2.Base;
using Ballance2.Base.Handler;
using Ballance2.Config;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Services.I18N;
using Ballance2.Services.LuaService.Lua;
using Ballance2.Services.LuaService.LuaWapper;
using Ballance2.Utils;
using SLua;
using System;
using System.Collections.Generic;
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
  [SLua.CustomLuaClass]
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
      var t = new Task<bool>(() => { return true; });
      t.Start();
      return t;
    }
    public virtual Task<bool> LoadPackage()
    {
      FixBundleShader();
      LoadI18NResource();

      var t = new Task<bool>(() => {
        //模块代码环境初始化
        if (PackageName != GamePackageManager.SYSTEM_PACKAGE_NAME && Type == GamePackageType.Module)
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

      if (requiredLuaClasses != null)
      {
        requiredLuaClasses.Clear();
        requiredLuaClasses = null;
      }
      if (requiredLuaFiles != null)
      {
        requiredLuaFiles.Clear();
        requiredLuaFiles = null;
      }
      //TODO: CLEAR
      luaObjects.Clear();

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

    private static GamePackage _CorePackage = null;
    private static GamePackage _SystemPackage = new GameSystemPackage();

    /// <summary>
    /// 获取系统的模块结构
    /// </summary>
    /// <returns></returns>
    public static void SetCorePackage(GamePackage pack) { 
      if(_CorePackage == null)
        _CorePackage = pack; 
      else 
        GameErrorChecker.SetLastErrorAndLog(GameError.AccessDenined, "GamePackage", "Not allow to chage GamePackage");
    }
    /// <summary>
    /// 获取系统的模块结构
    /// </summary>
    /// <returns></returns>
    public static GamePackage GetCorePackage() { 
      return _CorePackage; 
    }
    /// <summary>
    /// 获取系统的模块结构
    /// </summary>
    /// <returns></returns>
    public static GamePackage GetSystemPackage() { 
      return _SystemPackage; 
    }

    #endregion

    #region 常量定义

    public const int FLAG_CODE_BASE_LOADED = 0x000000001;
    public const int FLAG_CODE_LUA_PACK = 0x000000002;
    public const int FLAG_CODE_CS_PACK = 0x000000004;
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
    
    protected int flag = 0;

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

    /// <summary>
    /// 设置标志位
    /// </summary>
    /// <param name="flag"></param>
    public void SetFlag(int flag)  {

      if((this.flag & FLAG_PACK_NOT_UNLOADABLE) == FLAG_PACK_NOT_UNLOADABLE && (flag & FLAG_PACK_NOT_UNLOADABLE) != FLAG_PACK_NOT_UNLOADABLE) {
        Log.E(TAG, "Not allow set FLAG_PACK_NOT_UNLOADABLE flag for not unloadable packages.");
        flag &= FLAG_PACK_NOT_UNLOADABLE;
      }

      //internal
      if(this.flag == 0xF0 && PackageName == GamePackageManager.SYSTEM_PACKAGE_NAME)
        LoadPackageCodeBase();

      this.flag = flag;
    }
    /// <summary>
    /// 获取标志位
    /// </summary>
    /// <param name="flag"></param>
    public int GetFlag() { return flag; }

    /// <summary>
    /// Lua 虚拟机
    /// </summary>
    [DoNotToLua]
    public LuaState PackageLuaState => GameManager.Instance.GameMainLuaState;

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

      if (CodeType == GamePackageCodeType.Lua)
      {
        requiredLuaFiles = new Dictionary<string, object>();
        requiredLuaClasses = new Dictionary<string, LuaFunction>();

        object b = PackageLuaState.doString(@"IntneralLoadLuaPackage('" + PackageName + "','" + EntryCode + "')");
        if (b is bool && !((bool)b))
        {
          Log.E(TAG, "模块初始化返回了错误");
          GameErrorChecker.LastError = GameError.ExecutionFailed;
          return (bool)b;
        }

        flag &= FLAG_CODE_LUA_PACK;
        flag &= FLAG_CODE_BASE_LOADED;
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
        
        flag &= FLAG_CODE_CS_PACK;
        flag &= FLAG_CODE_BASE_LOADED;
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
      if (IsEntryCodeExecuted()) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "Run ExecutionCode failed, an not run twice");
        return false;
      }

      flag &= FLAG_CODE_ENTRY_CODE_RUN;
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
      if (IsUnloadCodeExecuted()) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "Run BeforeUnLoadCode failed, an not run twice");
        return false;
      }

      flag &= FLAG_CODE_UNLOD_CODE_RUN;
      if(PackageEntry.OnBeforeUnLoad != null)
        return PackageEntry.OnBeforeUnLoad.Invoke(this);
      return true;
    }

    #region LUA 文件导入

    /// <summary>
    /// 导入 Lua 类到当前模块虚拟机中。
    /// 注意，类函数以 “CreateClass:类名” 开头，
    /// 关于 Lua 类，请参考 Docs/LuaClass 。
    /// </summary>
    /// <param name="className">类名</param>
    /// <returns>类创建函数</returns>
    /// <exception cref="MissingReferenceException">
    /// 如果没有在当前模块包中找到类文件或是类创建函数 @* ，则抛出 MissingReferenceException 异常。
    /// </exception>
    /// <exception cref="Exception">
    /// 如果Lua执行失败，则抛出此异常。
    /// </exception>
    [LuaApiDescription("导入 Lua 类到当前模块虚拟机中", "类创建函数")]
    [LuaApiParamDescription("className", "类名")]
    public LuaFunction RequireLuaClass(string className)
    {
        LuaFunction classInit;
        if (requiredLuaClasses.TryGetValue(className, out classInit))
            return classInit;

        var CreateClass = (PackageLuaState["CreateClass"] as LuaTable);
        if(CreateClass == null)
            throw new MissingReferenceException("This shouldn't happen: CreateClass is null! ");

        classInit = CreateClass[className] as LuaFunction;
        if (classInit != null)
        {
            requiredLuaClasses.Add(className, classInit);
            return classInit;
        }

        byte[] lua = TryLoadLuaCodeAsset(className, out var realPath);
        if(lua.Length == 0)
            throw new MissingReferenceException(PackageName + " 无法导入 Lua class : " + className + " , 该文件为空");
        try
        {
            PackageLuaState.doBuffer(lua, realPath/*PackageName + ":" + className*/, out var v);
        }
        catch (Exception e)
        {
            Log.E(TAG, e.ToString());
            GameErrorChecker.LastError = GameError.ExecutionFailed;

            throw new Exception(PackageName + " 无法导入 Lua class : " + e.Message);
        }

        classInit = CreateClass[className] as LuaFunction;
        if (classInit == null)
        {
            throw new MissingReferenceException(PackageName + " 无法导入 Lua class : " +
                className + ", 未找到初始类函数: CreateClass:" + className);
        }

        requiredLuaClasses.Add(className, classInit);
        return classInit;
    }
    /// <summary>
    /// 导入Lua文件到当前模块虚拟机中
    /// </summary>
    /// <param name="fileName">LUA文件名</param>
    /// <returns>如果对应文件已导入，则返回true，否则返回false</returns>
    /// <exception cref="MissingReferenceException">
    /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass:* ，则抛出 MissingReferenceException 异常。
    /// </exception>
    /// <exception cref="Exception">
    /// 如果Lua执行失败，则抛出此异常。
    /// </exception>
    [LuaApiDescription("导入Lua文件到当前模块虚拟机中。不重复导入", "返回执行结果")]
    [LuaApiParamDescription("fileName", "LUA文件名")]
    public object RequireLuaFile(string fileName) { return RequireLuaFileInternal(this, fileName, true); }
    /// <summary>
    /// 导入Lua文件到当前模块虚拟机中，仅导入一次，不重复导入
    /// </summary>
    /// <param name="fileName">LUA文件名</param>
    /// <returns>如果对应文件已导入，则返回true，否则返回false</returns>
    /// <exception cref="MissingReferenceException">
    /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass:* ，则抛出 MissingReferenceException 异常。
    /// </exception>
    /// <exception cref="Exception">
    /// 如果Lua执行失败，则抛出此异常。
    /// </exception>
    [LuaApiDescription("导入Lua文件到当前模块虚拟机中，允许重复导入", "返回执行结果")]
    [LuaApiParamDescription("fileName", "LUA文件名")]
    public object RequireLuaFileNoOnce(string fileName) { return RequireLuaFileInternal(this, fileName, false); }
    /// <summary>
    /// 从其他模块导入Lua文件到当前模块虚拟机中
    /// </summary>
    /// <param name="fileName">LUA文件名</param>
    /// <returns>如果对应文件已导入，则返回true，否则返回false</returns>
    /// <exception cref="MissingReferenceException">
    /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass:* ，则抛出 MissingReferenceException 异常。
    /// </exception>
    /// <exception cref="Exception">
    /// 如果Lua执行失败，则抛出此异常。
    /// </exception>
    [LuaApiDescription("从其他模块导入Lua文件到当前模块虚拟机中。不重复导入", "返回执行结果")]
    [LuaApiParamDescription("fileName", "LUA文件名")]
    public object RequireLuaFile(GamePackage otherPack, string fileName) { return RequireLuaFileInternal(otherPack, fileName, true); }
    /// <summary>
    /// 从其他模块导入Lua文件到当前模块虚拟机中，仅导入一次，不重复导入
    /// </summary>
    /// <param name="fileName">LUA文件名</param>
    /// <returns>如果对应文件已导入，则返回true，否则返回false</returns>
    /// <exception cref="MissingReferenceException">
    /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass:* ，则抛出 MissingReferenceException 异常。
    /// </exception>
    /// <exception cref="Exception">
    /// 如果Lua执行失败，则抛出此异常。
    /// </exception>
    [LuaApiDescription("从其他模块导入Lua文件到当前模块虚拟机中，允许重复导入", "返回执行结果")]
    [LuaApiParamDescription("fileName", "LUA文件名")]
    public object RequireLuaFileNoOnce(GamePackage otherPack, string fileName) { return RequireLuaFileInternal(otherPack, fileName, false); }
    
    private Dictionary<string, object> requiredLuaFiles = null;
    private Dictionary<string, LuaFunction> requiredLuaClasses = null;

    private byte[] TryLoadLuaCodeAsset(string className, out string realPath) {

        var lua = GetCodeAsset(className);
        if (lua == null) 
            lua = GetCodeAsset(className + ".lua");
        if (lua == null) 
            lua = GetCodeAsset(className + ".luac");
        if (lua == null) 
            lua = GetCodeAsset("Scripts/" + className);
        if (lua == null) 
            lua = GetCodeAsset("Scripts/" + className + ".lua");
        if (lua == null) 
            lua = GetCodeAsset("Scripts/" + className + ".luac");
        if (lua == null)
            throw new MissingReferenceException(PackageName + " 无法导入 " + className + " , 未找到文件");
        realPath = lua.realPath;
        return lua.data;
    }
    private object RequireLuaFileInternal(GamePackage pack, string fileName, bool once)
    {
        object rs = null;
        byte[] lua = pack.TryLoadLuaCodeAsset(fileName, out var realPath);
        if (lua.Length == 0)
            throw new EmptyFileException(PackageName + " 无法导入 Lua : " + fileName + " , 该文件为空");
        try
        {
            //不重复导入
            if(once && requiredLuaFiles.TryGetValue(realPath, out var lastRet)) 
                return lastRet;

            if(PackageLuaState.doBuffer(lua, realPath, out var v))
                rs = v;
            else
                throw new Exception(PackageName + " 无法导入 Lua : 执行失败");

            //添加结果，用于下一次不重复导入
            if(requiredLuaFiles.ContainsKey(realPath))
                requiredLuaFiles[realPath] = rs;
            else
                requiredLuaFiles.Add(realPath, rs);
        }
        catch (Exception e)
        {
            Log.E(TAG, e.ToString());
            GameErrorChecker.LastError = GameError.ExecutionFailed;

            throw new Exception(PackageName + " 无法导入 Lua : " + e.Message);
        }

        return rs;
    }

    #endregion

    #region LUA 函数调用

    /// <summary>
    /// 获取当前 模块主代码 的指定函数
    /// </summary>
    /// <param name="funName">函数名</param>
    /// <returns>返回函数，未找到返回null</returns>
    [LuaApiDescription("获取当前 模块主代码 的指定函数", "返回函数，未找到返回null")]
    [LuaApiParamDescription("funName", "函数名")]
    public LuaFunction GetLuaFun(string funName)
    {
        if (PackageLuaState == null)
        {
            Log.E(TAG, "GetLuaFun Failed because package cannot run");
            GameErrorChecker.LastError = GameError.PackageCanNotRun;
            return null;
        }
        return PackageLuaState.getFunction(funName);
    }
    /// <summary>
    /// 调用模块主代码的lua无参函数
    /// </summary>
    /// <param name="funName">lua函数名称</param>
    [LuaApiDescription("调用模块主代码的lua无参函数")]
    [LuaApiParamDescription("funName", "lua函数名称")]
    public void CallLuaFun(string funName)
    {
        LuaFunction f = GetLuaFun(funName);
        if (f != null) f.call();
        else Log.E(TAG, "CallLuaFun Failed because function {0} not founnd", funName);
    }
    /// <summary>
    /// 尝试调用模块主代码的lua无参函数
    /// </summary>
    /// <param name="funName">lua函数名称</param>
    /// <returns>如果调用成功则返回true，否则返回false</returns>
    [LuaApiDescription("尝试调用模块主代码的lua无参函数", "如果调用成功则返回true，否则返回false")]
    [LuaApiParamDescription("funName", "lua函数名称")]
    public bool TryCallLuaFun(string funName)
    {
        LuaFunction f = GetLuaFun(funName);
        if (f != null) {
            f.call();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 调用模块主代码的lua函数
    /// </summary>
    /// <param name="funName">lua函数名称</param>
    /// <param name="pararms">参数</param>
    [LuaApiDescription("调用模块主代码的lua函数")]
    [LuaApiParamDescription("funName", "lua函数名称")]
    [LuaApiParamDescription("pararms", "参数")]
    public void CallLuaFun(string funName, params object[] pararms)
    {
        LuaFunction f = GetLuaFun(funName);
        if (f != null) f.call(pararms);
        else Log.E(TAG, "CallLuaFun Failed because function {0} not founnd", funName);
    }
    /// <summary>
    /// 调用指定的lua虚拟脚本中的lua无参函数
    /// </summary>
    /// <param name="luaObjectName">lua虚拟脚本名称</param>
    /// <param name="funName">lua函数名称</param>
    [LuaApiDescription("调用指定的lua虚拟脚本中的lua无参函数")]
    [LuaApiParamDescription("luaObjectName", "lua虚拟脚本名称")]
    [LuaApiParamDescription("funName", "lua函数名称")]
    public void CallLuaFun(string luaObjectName, string funName)
    {
        GameLuaObjectHost targetObject = null;
        if (FindLuaObject(luaObjectName, out targetObject))
            targetObject.CallLuaFun(funName);
        else Log.E(TAG, "CallLuaFun Failed because object {0} not founnd", luaObjectName);
    }
    /// <summary>
    /// 调用指定的lua虚拟脚本中的lua函数
    /// </summary>
    /// <param name="luaObjectName">lua虚拟脚本名称</param>
    /// <param name="funName">lua函数名称</param>
    /// <param name="pararms">参数</param>
    /// <returns>Lua函数返回的对象，如果调用该函数失败，则返回null</returns>
    [LuaApiDescription("调用指定的lua虚拟脚本中的lua函数", "Lua函数返回的对象，如果调用该函数失败，则返回null")]
    [LuaApiParamDescription("luaObjectName", "lua虚拟脚本名称")]
    [LuaApiParamDescription("funName", "lua函数名称")]
    [LuaApiParamDescription("pararms", "参数")]
    public object CallLuaFunWithParam(string luaObjectName, string funName, params object[] pararms)
    {
        GameLuaObjectHost targetObject = null;
        if (FindLuaObject(luaObjectName, out targetObject))
            return targetObject.CallLuaFunWithParam(funName, pararms);
        else 
            Log.E(TAG, "CallLuaFun Failed because object {0} not founnd", luaObjectName);
        return null;
    }

    #endregion

    #region LUA 组件

    //管理当前模块下的所有lua虚拟脚本，统一管理、释放
    private List<GameLuaObjectHost> luaObjects = new List<GameLuaObjectHost>();

    /// <summary>
    /// 注册lua虚拟脚本到物体上
    /// </summary>
    /// <param name="name">lua虚拟脚本的名称</param>
    /// <param name="gameObject">要附加的物体</param>
    /// <param name="className">目标代码类名</param>
    [LuaApiDescription("注册lua虚拟脚本到物体上")]
    [LuaApiParamDescription("name", "lua虚拟脚本的名称")]
    [LuaApiParamDescription("gameObject", "要附加的物体")]
    [LuaApiParamDescription("className", "目标代码类名")]
    public GameLuaObjectHost RegisterLuaObject(string name, GameObject gameObject, string className)
    {
        GameLuaObjectHost newGameLuaObjectHost = gameObject.AddComponent<GameLuaObjectHost>();
        newGameLuaObjectHost.Name = name;
        newGameLuaObjectHost.Package = this;
        newGameLuaObjectHost.LuaState = PackageLuaState;
        newGameLuaObjectHost.LuaClassName = className;
        luaObjects.Add(newGameLuaObjectHost);
        return newGameLuaObjectHost;
    }
    /// <summary>
    /// 查找lua虚拟脚本
    /// </summary>
    /// <param name="name">lua虚拟脚本的名称</param>
    /// <param name="gameLuaObjectHost">输出lua虚拟脚本</param>
    /// <returns>返回是否找到对应脚本</returns>
    [LuaApiDescription("查找lua虚拟脚本", "返回是否找到对应脚本")]
    [LuaApiParamDescription("name", "lua虚拟脚本的名称")]
    [LuaApiParamDescription("gameLuaObjectHost", "输出lua虚拟脚本")]
    public bool FindLuaObject(string name, out GameLuaObjectHost gameLuaObjectHost)
    {
        foreach (GameLuaObjectHost luaObjectHost in luaObjects)
        {
            if (luaObjectHost.Name == name)
            {
                gameLuaObjectHost = luaObjectHost;
                return true;
            }
        }
        gameLuaObjectHost = null;
        return false;
    }
    //清除已释放的lua虚拟脚本
    internal void RemoveLuaObject(GameLuaObjectHost o)
    {
        if (luaObjects != null)
            luaObjects.Remove(o);
    }
    internal void AddeLuaObject(GameLuaObjectHost o)
    {
        if (luaObjects != null && !luaObjects.Contains(o))
            luaObjects.Add(o);
    }

    #endregion

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
    /// 读取模块资源包中的代码资源
    /// </summary>
    /// <param name="pathorname">文件名称或路径</param>
    /// <returns>如果读取成功则返回代码内容，否则返回null</returns>
    public virtual CodeAsset GetCodeAsset(string pathorname)
    {
      TextAsset textAsset = GetTextAsset(pathorname);
      if (textAsset != null)
        return new CodeAsset(textAsset.bytes, pathorname, pathorname, pathorname);

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
    [SLua.CustomLuaClass]
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
      /// <summary>
      /// 调试器中显示的路径
      /// </summary>
      public string debugPath;

      public CodeAsset(byte[] data, string realPath, string relativePath, string debugPath) {
        this.data = data;
        this.realPath = realPath;
        this.relativePath = relativePath;
        this.debugPath = debugPath;
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
