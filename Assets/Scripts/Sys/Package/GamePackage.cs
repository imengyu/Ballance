using Ballance2.LuaHelpers;
using Ballance2.Config;
using Ballance2.Sys.Bridge;
using Ballance2.Sys.Bridge.Handler;
using Ballance2.Sys.Bridge.LuaWapper;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Language;
using Ballance2.Sys.Res;
using Ballance2.Sys.Services;
using Ballance2.Sys.Utils.Lua;
using Ballance2.Utils;
using SLua;
using System;
using System.Collections.Generic;
using System.Reflection;
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
* 游戏模块的声明以及模块功能提供
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Sys.Package
{
    /// <summary>
    /// 模块包实例
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("模块包实例")]
    public class GamePackage
    {
        /// <summary>
        /// 标签
        /// </summary>
        [LuaApiDescription("标签")]
        public string TAG { 
            get {
                return "GamePackage:" + PackageName;
            }
        }

        internal int DependencyRefCount = 0;
        internal bool UnLoadWhenDependencyRefNone = false;

        [DoNotToLua]
        public virtual Task<bool> LoadInfo(string filePath)
        {
            PackageFilePath = filePath;
            return null;
        }
        [DoNotToLua]
        public virtual async Task<bool> LoadPackage()
        {
            FixBundleShader();
            LoadI18NResource();

            //模块代码环境初始化
            if (PackageName != GamePackageManager.SYSTEM_PACKAGE_NAME && Type == GamePackageType.Module)
            {
                if (CodeType == GamePackageCodeType.Lua)
                {
                    //启动LUA 虚拟机
                    await InitLuaState();
                }
                else if (CodeType == GamePackageCodeType.CSharp)
                {
                    //加载C#程序集
                    CSharpAssembly = LoadCodeCSharp(EntryCode);
                    if (CSharpAssembly != null)
                        baseInited = true;
                }
                else {
                    Log.W(TAG, "当前模块是普通模块，但是 CodeType 却未配置成为任何一种可运行代码环境，这种情况下此模块将无法运行任何代码，请检查配置是否正确");
                }
            }

            return true;
        }
        [DoNotToLua]
        public virtual void Destroy()
        {
            Log.D(TAG, "Destroy package {0}", PackageName);

            baseInited = false;

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

            
            DestroyLuaState();

            //Lua释放
            if (luaObjects != null)
            {
                //释放所有LUA 虚拟机
                foreach (GameLuaObjectHost o in luaObjects)
                {
                    o.enabled = false;
                    o.StopAllCoroutines();
                    UnityEngine.Object.Destroy(o);
                }
                luaObjects.Clear();
                luaObjects = null;
            }

            if (CSharpAssembly != null)
                CSharpAssembly = null;
            if (CSharpPackageEntry != null)
                CSharpPackageEntry = null;
        }

        #region 系统包

        private static GamePackage _SystemPackage = new GameSystemPackage();

        /// <summary>
        /// 获取系统的模块结构
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("获取系统模块包")]
        public static GamePackage GetSystemPackage() { return _SystemPackage; }

        #endregion

        #region 模块运行环境

        /// <summary>
        /// C# 程序集
        /// </summary>
        [LuaApiDescription("C# 程序集")]
        public Assembly CSharpAssembly { get; protected set; }
        /// <summary>
        /// C# 程序入口
        /// </summary>
        [LuaApiDescription("C# 程序入口")]
        private object CSharpPackageEntry = null;
        /// <summary>
        /// LUA程序入口
        /// </summary>
        [LuaApiDescription("LUA程序入口")]
        private LuaTable LuaPackageEntry = null;

        /// <summary>
        /// 获取 Lua 虚拟机
        /// </summary>
        [LuaApiDescription("获取 Lua 虚拟机")]
        public virtual LuaState PackageLuaState { get; private set; }
        private PackageLuaServer PackageLuaServer = null;

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
        /// <summary>
        /// 获取模块启动代码是否已经执行
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("获取模块启动代码是否已经执行")]
        public bool IsEntryCodeExecuted() { return mainLuaCodeLoaded; }

        private bool mainLuaCodeLoaded = false;
        private bool luaStateInited = false;
        private bool luaStateIniting = false;
        private bool runExecutionCodeWhenLuaStateInit = false;
        protected bool baseInited = false;

        //初始化LUA虚拟机
        private async Task<bool> InitLuaState()
        {
            mainLuaCodeLoaded = false;
            requiredLuaFiles = new Dictionary<string, object>();
            requiredLuaClasses = new Dictionary<string, LuaFunction>();

            if (!string.IsNullOrEmpty(ShareLuaState))
            {
                //如果指定了core，则代码载入到全局lua虚拟机中
                if (ShareLuaState == "core")
                {
                    PackageLuaState = GameManager.Instance.GameMainLuaState;
                    SetLuaStateInitFinished();
                    return false;
                }

                GamePackage m = GameManager.Instance.GetSystemService<GamePackageManager>().FindPackage(ShareLuaState);

                if (m != null && m.PackageLuaState != null && m.luaStateInited)
                {
                    PackageLuaState = m.PackageLuaState;
                    SetLuaStateInitFinished();
                    return false;
                }
                else Log.E(TAG, "package {0} can not be load because " +
                    "the target mod lua environment is not ready or not load. now use independent LuaServer",
                    ShareLuaState);
            }

            PackageLuaServer = new PackageLuaServer(PackageName);
            PackageLuaState = PackageLuaServer.getLuaState();
            PackageLuaServer.init(null, SetLuaStateInitFinished);
            luaStateIniting = true;

            await new WaitUntil(IsLuaStateInitFinished);

            return true;
        }
        protected void SystemPackageSetInitFinished() { 
            baseInited = true;
            luaStateInited = true; 
            luaStateIniting = false;
            mainLuaCodeLoaded = false;
            requiredLuaFiles = new Dictionary<string, object>();
            requiredLuaClasses = new Dictionary<string, LuaFunction>();
            RunPackageExecutionCode();
        }
        private void SetLuaStateInitFinished() { 
            luaStateInited = true; 
            luaStateIniting = false;
            baseInited = true;
            if(runExecutionCodeWhenLuaStateInit) 
                RunPackageExecutionCode();
        }

        /// <summary>
        /// 获取Lua虚拟机是否初始化完成
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("获取Lua虚拟机是否初始化完成")]
        public bool IsLuaStateInitFinished()
        {
            return luaStateInited;
        }

        /// <summary>
        /// 运行模块初始化代码
        /// </summary>
        /// <returns></returns>
        [DoNotToLua]
        public bool RunPackageExecutionCode()
        {
            if (Type != GamePackageType.Module)
            {
                GameErrorChecker.LastError = GameError.PackageCanNotRun;
                return false;
            }
            if (!baseInited)
            {
                if(luaStateIniting) 
                    runExecutionCodeWhenLuaStateInit = true;
                else {
                    Log.E(TAG, "RunPackageExecutionCode failed, package not load, status {0}", Status);
                    GameErrorChecker.LastError = GameError.NotLoad;
                    return false;
                }
            }

            if (CodeType == GamePackageCodeType.Lua)
            {
                //运行Lua入口代码
                if (!string.IsNullOrWhiteSpace(EntryCode))
                {
                    if (!luaStateInited)
                    {
                        Log.E(TAG, "RunModExecutionCode failed, Lua state not init, mod maybe cannot run");
                        GameErrorChecker.LastError = GameError.PackageCanNotRun;
                        return false;
                    }

                    string lua = GetCodeLuaAsset(EntryCode, out var realpath);
                    if (lua == null) 
                        Log.E(TAG, "Run package EntryCode failed, function {0} not found", EntryCode);
                    else if(string.IsNullOrWhiteSpace(lua))
                        Log.E(TAG, "Run package EntryCode failed, EntryCode is null", EntryCode);
                    else if (!mainLuaCodeLoaded)
                    {
                        Log.D(TAG, "Run package EntryCode {0} ", EntryCode);
                        
                        try
                        {
                            LuaPackageEntry = PackageLuaState.doString(lua, realpath) as LuaTable;
                            if(LuaPackageEntry == null) {
                                Log.E(TAG, "模块 {0} 运行启动代码失败! 启动代码未返回指定结构体。\n请检查代码 ->\n{1}", 
                                    PackageName, 
                                    DebugUtils.PrintCodeWithLine(lua));
                                GameErrorChecker.LastError = GameError.ExecutionFailed;
                                return false;
                            } else {
                                mainLuaCodeLoaded = true;
                                requiredLuaFiles.Add(EntryCode, LuaPackageEntry);
                            }
                        }
                        catch (Exception e)
                        {
                            Log.E(TAG, "模块 {0} 运行启动代码失败! {1}\n请检查代码 ->\n{2}", 
                                PackageName, e.Message, 
                                DebugUtils.PrintCodeWithLine(lua));
                            Log.E(TAG, e.ToString());
                            GameErrorChecker.LastError = GameError.ExecutionFailed;
                            return false;
                        }

                        LuaFunction fPackageEntry = LuaPackageEntry["PackageEntry"] as LuaFunction;
                        if (fPackageEntry != null)
                        {
                            object b = fPackageEntry.call(this);
                            if (b is bool && !((bool)b))
                            {
                                Log.E(TAG, "模块 {0} PackageEntry 返回了错误", PackageName);
                                GameErrorChecker.LastError = GameError.ExecutionFailed;
                                return (bool)b;
                            }
                            return false;
                        }
                        else
                        {
                            Log.E(TAG, "模块 {0} 未找到 PackageEntry ", PackageName);
                            GameErrorChecker.LastError = GameError.FunctionNotFound;
                        }
                    }
                    else
                    {
                        Log.E(TAG, "无法重复运行模块启动代码 {0} {1} ", EntryCode, PackageName);
                        GameErrorChecker.LastError = GameError.AlreadyRegistered;
                    }
                }
            }
            else if (CodeType == GamePackageCodeType.CSharp)
            {
                //运行C#入口
                if (CSharpAssembly != null)
                {
                    Type type = CSharpAssembly.GetType("Package");
                    if (type == null)
                    {
                        Log.E(TAG, "未找到 Package ");
                        GameErrorChecker.LastError = GameError.ClassNotFound;
                        return false;
                    }

                    CSharpPackageEntry = Activator.CreateInstance(type);
                    MethodInfo methodInfo = type.GetMethod("PackageEntry");  //根据方法名获取MethodInfo对象
                    if (type == null)
                    {
                        Log.E(TAG, "未找到 PackageEntry()");
                        GameErrorChecker.LastError = GameError.FunctionNotFound;
                        return false;
                    }

                    object b = methodInfo.Invoke(CSharpPackageEntry, new object[] { this } );
                    if (b is bool && !((bool)b))
                    {
                        Log.E(TAG, "模块 PackageEntry 返回了错误");
                        GameErrorChecker.LastError = GameError.ExecutionFailed;
                        return (bool)b;
                    }
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 运行模块卸载回调
        /// </summary>
        [DoNotToLua]
        public bool RunPackageBeforeUnLoadCode()
        {
            if (Type != GamePackageType.Module)
            {
                GameErrorChecker.LastError = GameError.PackageCanNotRun;
                return false;
            }
            if (CodeType == GamePackageCodeType.Lua)
            {
                if (mainLuaCodeLoaded && !PackageLuaState.Destroyed)
                {
                    LuaFunction fPackageBeforeUnLoad = LuaPackageEntry["PackageBeforeUnLoad"] as LuaFunction;
                    if (fPackageBeforeUnLoad != null)
                        fPackageBeforeUnLoad.call(this);
                    return true;
                }
            }
            else if (CodeType == GamePackageCodeType.CSharp)
            {
                if (CSharpAssembly != null)
                {
                    Type type = CSharpAssembly.GetType("Package");
                    MethodInfo methodInfo = type.GetMethod("PackageBeforeUnLoad");
                    methodInfo.Invoke(CSharpPackageEntry, null);
                    return true;
                }
            }

            return false;
        }
        //释放Lua虚拟机
        private void DestroyLuaState()
        {
            if (requiredLuaFiles != null)
            {
                requiredLuaFiles.Clear();
                requiredLuaFiles = null;
            }
            if (requiredLuaClasses != null)
            {
                foreach (var v in requiredLuaClasses)
                    v.Value.Dispose();
                requiredLuaClasses.Clear();
                requiredLuaClasses = null;
            }
            if (PackageLuaState != null)
            {
                if(!PackageLuaState.Destroyed && PackageLuaState != LuaState.main)
                    PackageLuaState.Dispose();
                PackageLuaState = null;
            }
        }

        private Dictionary<string, object> requiredLuaFiles = null;
        private Dictionary<string, LuaFunction> requiredLuaClasses = null;

        private string TryLoadLuaCodeAsset(string className, out string realPath) {
            string lua = GetCodeLuaAsset(className, out realPath);

            if (lua == null) 
                lua = GetCodeLuaAsset(className + ".lua", out realPath);
                
            if(!className.EndsWith(".lua")) className += ".lua";

            if (lua == null) 
                lua = GetCodeLuaAsset("Scripts/" + className, out realPath);

            if(!className.EndsWith(".txt")) className += ".txt";
            if (lua == null) 
                lua = GetCodeLuaAsset(className + ".txt", out realPath);
            if (lua == null) 
                lua = GetCodeLuaAsset("Scripts/" + className + ".txt", out realPath);
            if (lua == null)
                throw new MissingReferenceException(PackageName + " 无法导入 Lua class : " + className + " , 未找到该文件");
            return lua;
        }

        /// <summary>
        /// 导入 Lua 类到当前模块虚拟机中。
        /// 注意，类函数以 “CreateClass_类名” 开头，
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

            classInit = PackageLuaState.getFunction("CreateClass_" + className);
            if (classInit != null)
            {
                requiredLuaClasses.Add(className, classInit);
                return classInit;
            }

            string lua = TryLoadLuaCodeAsset(className, out var realPath);
            if(string.IsNullOrWhiteSpace(lua))
                throw new MissingReferenceException(PackageName + " 无法导入 Lua class : " + className + " , 该文件为空");
            try
            {
                PackageLuaState.doString(lua, realPath/*PackageName + ":" + className*/);
            }
            catch (Exception e)
            {
                Log.E(TAG, e.ToString());
                GameErrorChecker.LastError = GameError.ExecutionFailed;

                throw new Exception(PackageName + " 无法导入 Lua class : " + e.Message);
            }

            classInit = PackageLuaState.getFunction("CreateClass_" + className);
            if (classInit == null)
            {
                throw new MissingReferenceException(PackageName + " 无法导入 Lua class : " +
                    className + ", 未找到初始类函数: CreateClass_" + className);
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
        /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass_* ，则抛出 MissingReferenceException 异常。
        /// </exception>
        /// <exception cref="Exception">
        /// 如果Lua执行失败，则抛出此异常。
        /// </exception>
        [LuaApiDescription("导入Lua文件到当前模块虚拟机中", "返回执行结果")]
        [LuaApiParamDescription("fileName", "LUA文件名")]
        public object RequireLuaFile(string fileName) { return RequireLuaFileInternal(this, fileName, false); }
        /// <summary>
        /// 导入Lua文件到当前模块虚拟机中，仅导入一次，不重复导入
        /// </summary>
        /// <param name="fileName">LUA文件名</param>
        /// <returns>如果对应文件已导入，则返回true，否则返回false</returns>
        /// <exception cref="MissingReferenceException">
        /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass_* ，则抛出 MissingReferenceException 异常。
        /// </exception>
        /// <exception cref="Exception">
        /// 如果Lua执行失败，则抛出此异常。
        /// </exception>
        [LuaApiDescription("导入Lua文件到当前模块虚拟机中，仅导入一次，不重复导入", "返回执行结果")]
        [LuaApiParamDescription("fileName", "LUA文件名")]
        public object RequireLuaFileOnce(string fileName) { return RequireLuaFileInternal(this, fileName, true); }
        /// <summary>
        /// 从其他模块导入Lua文件到当前模块虚拟机中
        /// </summary>
        /// <param name="fileName">LUA文件名</param>
        /// <returns>如果对应文件已导入，则返回true，否则返回false</returns>
        /// <exception cref="MissingReferenceException">
        /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass_* ，则抛出 MissingReferenceException 异常。
        /// </exception>
        /// <exception cref="Exception">
        /// 如果Lua执行失败，则抛出此异常。
        /// </exception>
        [LuaApiDescription("从其他模块导入Lua文件到当前模块虚拟机中", "返回执行结果")]
        [LuaApiParamDescription("fileName", "LUA文件名")]
        public object RequireLuaFile(GamePackage otherPack, string fileName) { return RequireLuaFileInternal(otherPack, fileName, false); }
        /// <summary>
        /// 从其他模块导入Lua文件到当前模块虚拟机中，仅导入一次，不重复导入
        /// </summary>
        /// <param name="fileName">LUA文件名</param>
        /// <returns>如果对应文件已导入，则返回true，否则返回false</returns>
        /// <exception cref="MissingReferenceException">
        /// 如果没有在当前模块包中找到类文件或是类创建函数 CreateClass_* ，则抛出 MissingReferenceException 异常。
        /// </exception>
        /// <exception cref="Exception">
        /// 如果Lua执行失败，则抛出此异常。
        /// </exception>
        [LuaApiDescription("从其他模块导入Lua文件到当前模块虚拟机中，仅导入一次，不重复导入", "返回执行结果")]
        [LuaApiParamDescription("fileName", "LUA文件名")]
        public object RequireLuaFileOnce(GamePackage otherPack, string fileName) { return RequireLuaFileInternal(otherPack, fileName, true); }

        private object RequireLuaFileInternal(GamePackage pack, string fileName, bool once)
        {
            object rs = null;
            string lua = pack.TryLoadLuaCodeAsset(fileName, out var realPath);
            if (string.IsNullOrWhiteSpace(lua))
                throw new MissingReferenceException(PackageName + " 无法导入 Lua : " + fileName + " , 该文件为空");
            try
            {
                //不重复导入
                if(once && requiredLuaFiles.TryGetValue(fileName, out var lastRet)) 
                    return lastRet;

                rs = PackageLuaState.doString(lua, realPath);
                requiredLuaFiles.Add(fileName, rs);
            }
            catch (Exception e)
            {
                Log.E(TAG, e.ToString());
                GameErrorChecker.LastError = GameError.ExecutionFailed;

                throw new Exception(PackageName + " 无法导入 Lua : " + e.Message);
            }

            return rs;
        }

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

        #endregion

        #region 模块信息

        private int VerConverter(string s) {
            if(s == "{internal.core.version}")
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
            } else {
                IsCompatible = true;
            }

            //参数
            XmlNode nodeEntryCode = nodePackage.SelectSingleNode("EntryCode");
            if (nodeEntryCode != null)
                EntryCode = nodeEntryCode.InnerText;
            XmlNode nodeCodeType = nodePackage.SelectSingleNode("CodeType");
            if (nodeCodeType != null) 
                CodeType = ConverUtils.StringToEnum(nodeCodeType.InnerText, GamePackageCodeType.Lua, "CodeType");
            XmlNode nodeType = nodePackage.SelectSingleNode("Type");
            if (nodeType != null)
                Type = ConverUtils.StringToEnum(nodeType.InnerText, GamePackageType.Asset, "Type");
            XmlNode nodeShareLuaState = nodePackage.SelectSingleNode("ShareLuaState");
            if (nodeShareLuaState != null)
                ShareLuaState = nodeShareLuaState.InnerText;
            

            return true;
        }

        /// <summary>
        /// 获取模块文件路径
        /// </summary>
        [LuaApiDescription("模块文件路径")]
        public string PackageFilePath { get; protected set; }
        /// <summary>
        /// 获取模块包名
        /// </summary>
        [LuaApiDescription("获取模块包名")]
        public string PackageName { get; protected set; }
        /// <summary>
        /// 获取模块版本号
        /// </summary>
        [LuaApiDescription("获取模块版本号")]
        public int PackageVersion { get; protected set; }
        /// <summary>
        /// 获取基础信息
        /// </summary>
        [LuaApiDescription("获取基础信息")]
        public GamePackageBaseInfo BaseInfo { get; protected set; }
        /// <summary>
        /// 获取模块更新时间
        /// </summary>
        [LuaApiDescription("获取模块更新时间")]
        public DateTime UpdateTime { get; protected set; }
        /// <summary>
        /// 获取获取是否是系统必须包
        /// </summary>
        [LuaApiDescription("获取获取是否是系统必须包")]
        public bool SystemPackage { get; internal set; }

        /// <summary>
        /// 获取模块加载错误
        /// </summary>
        [LuaApiDescription("获取模块加载错误")]
        public string LoadError { get; protected set; } = "";

        /// <summary>
        /// 获取模块PackageDef文档
        /// </summary>
        [LuaApiDescription("获取模块PackageDef文档")]
        public XmlDocument PackageDef { get; protected set; }
        /// <summary>
        /// 获取模块AssetBundle
        /// </summary>
        [LuaApiDescription("获取模块AssetBundle")]
        public AssetBundle AssetBundle { get; protected set; }

        /// <summary>
        /// 获取表示模块目标游戏内核版本
        /// </summary>
        [LuaApiDescription("获取表示模块目标游戏内核版本")]
        public int TargetVersion { get; protected set; } = GameConst.GameBulidVersion;
        /// <summary>
        /// 获取表示模块可以正常使用的最低游戏内核版本
        /// </summary>
        [LuaApiDescription("获取表示模块可以正常使用的最低游戏内核版本")]
        public int MinVersion { get; protected set; } = GameConst.GameBulidVersion;
        /// <summary>
        /// 获取模块是否兼容当前内核
        /// </summary>
        [LuaApiDescription("获取模块是否兼容当前内核")]
        public bool IsCompatible { get; protected set; }


        /// <summary>
        /// 获取模块入口代码
        /// </summary>
        [LuaApiDescription("获取模块入口代码")]
        public string EntryCode { get; protected set; }
        /// <summary>
        /// 获取模块类型
        /// </summary>
        [LuaApiDescription("获取模块类型")]
        public GamePackageType Type { get; protected set; } = GamePackageType.Asset;
        /// <summary>
        /// 获取模块代码类型
        /// </summary>
        [LuaApiDescription("获取模块代码类型")]
        public GamePackageCodeType CodeType { get; protected set; } = GamePackageCodeType.None;
        /// <summary>
        /// 获取模块共享Lua虚拟机
        /// </summary>
        [LuaApiDescription("获取模块共享Lua虚拟机")]
        public string ShareLuaState { get; protected set; }

        internal GamePackageStatus _Status = GamePackageStatus.NotLoad;

        /// <summary>
        /// 获取模块状态
        /// </summary>
        [LuaApiDescription("获取模块状态")]
        public GamePackageStatus Status { get { return _Status; } }
        
        /// <summary>
        /// 转为字符串显示
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("转为字符串显示")]
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
        [LuaApiDescription("读取模块资源包中的资源", "返回资源实例，如果未找到，则返回null")]
        [LuaApiParamDescription("pathorname", "资源路径")]
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
        [LuaApiDescription("读取模块资源包中的文字资源", "返回TextAsset实例，如果未找到，则返回null")]
        [LuaApiParamDescription("pathorname", "资源路径")]
        public virtual TextAsset GetTextAsset(string pathorname) { return GetAsset<TextAsset>(pathorname); }
        
        /// <summary>
        /// 读取模块资源包中的 Prefab 资源
        /// </summary>
        /// <param name="pathorname">资源路径</param>
        /// <returns>返回 GameObject 实例，如果未找到，则返回null</returns>
        [LuaApiDescription("读取模块资源包中的 Prefab 资源", "返回 GameObject 实例，如果未找到，则返回null")]
        [LuaApiParamDescription("pathorname", "资源路径")]
        public virtual GameObject GetPrefabAsset(string pathorname) { return GetAsset<GameObject>(pathorname); }
        public virtual Texture GetTextureAsset(string pathorname) { return GetAsset<Texture>(pathorname); }
        public virtual Texture2D GetTexture2DAsset(string pathorname) { return GetAsset<Texture2D>(pathorname); }
        public virtual Sprite GetSpriteAsset(string pathorname) { return GetAsset<Sprite>(pathorname); }
        public virtual Material GetMaterialAsset(string pathorname) { return GetAsset<Material>(pathorname); }
        public virtual PhysicMaterial GetPhysicMaterialAsset(string pathorname) { return GetAsset<PhysicMaterial>(pathorname); }
       
        /// <summary>
        /// 读取模块资源包中的Lua代码资源
        /// </summary>
        /// <param name="pathorname">文件名称或路径</param>
        /// <returns>如果读取成功则返回代码内容，否则返回null</returns>
        [LuaApiDescription("读取模块资源包中的Lua代码资源", "如果读取成功则返回代码内容，否则返回null")]
        [LuaApiParamDescription("pathorname", "文件名称或路径")]
        public virtual string GetCodeLuaAsset(string pathorname, out string realPath)
        {
            TextAsset textAsset = GetTextAsset(pathorname);
            if (textAsset != null) {
                realPath = pathorname;
                return textAsset.text;
            }

            GameErrorChecker.LastError = GameError.FileNotFound;
            realPath = "";
            return null;
        }
        
        /// <summary>
        /// 加载模块资源包中的c#代码资源
        /// </summary>
        /// <param name="pathorname">资源路径</param>
        /// <returns>如果加载成功则返回已加载的Assembly，否则将抛出异常，若当前环境并不支持加载，则返回null</returns>
        [LuaApiDescription("加载模块资源包中的c#代码资源", "如果加载成功则返回已加载的Assembly，否则将抛出异常，若当前环境并不支持加载，则返回null")]
        [LuaApiParamDescription("pathorname", "资源路径")]
        public virtual Assembly LoadCodeCSharp(string pathorname)
        {
            GameErrorChecker.SetLastErrorAndLog(GameError.NotSupportFileType, TAG, "当前模块不支持加载 CSharp 代码");
            return null;
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
        private void LoadI18NResource() {
            var res = GetTextAsset("I18n.xml");
            if(res != null) {
                if(!I18NProvider.LoadLanguageResources(res.text))
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
        [LuaApiDescription("添加自定义数据", "返回数据值")]
        [LuaApiParamDescription("name", "数据名称")]
        [LuaApiParamDescription("data", "数据值")]
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
        [LuaApiDescription("获取自定义数据", "返回数据值")]
        [LuaApiParamDescription("name", "数据名称")]
        [LuaApiParamDescription("data", "数据值")]
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
        [LuaApiDescription("设置自定义数据", "返回旧的数据值，如果之前没有该数据，则返回null")]
        [LuaApiParamDescription("name", "数据名称")]
        [LuaApiParamDescription("data", "数据值")]
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
        [LuaApiDescription("清除自定义数据", "返回是否成功")]
        [LuaApiParamDescription("name", "数据名称")]
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
