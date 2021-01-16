using Ballance2.Config;
using Ballance2.System.Bridge;
using Ballance2.System.Debug;
using Ballance2.System.Res;
using Ballance2.System.Services;
using Ballance2.System.Utils.Lua;
using Ballance2.Utils;
using SLua;
using System;
using System.Collections;
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
* 更改历史：
* 2021-1-14 创建
*
*/

namespace Ballance2.System.Package
{
    /// <summary>
    /// 模块包实例
    /// </summary>
    [SLua.CustomLuaClass]
    public class GamePackage
    {
        private readonly string TAG = "GamePackage";

        internal int DependencyRefCount = 0;
        internal bool UnLoadWhenDependencyRefNone = false;

        [DoNotToLua]
        public virtual async Task<bool> LoadInfo(string filePath)
        {
            PackageFilePath = filePath;
            return true;
        }
        [DoNotToLua]
        public virtual async Task<bool> LoadPackage()
        {
            FixBundleShader();

            //模组代码环境初始化
            if (Type == GamePackageType.Module)
            {
                if (CodeType == GamePackageCodeType.Lua)
                {
                    //启动LUA 虚拟机
                    await InitLuaState();
                    baseInited = true;
                }
                else if (CodeType == GamePackageCodeType.CSharp)
                {
                    //加载C#程序集
#if ENABLE_MONO || ENABLE_DOTNET
                    TextAsset dll = GetTextAsset(EntryCode);
                    if (dll != null)
                    {
                        try
                        {
                            CSharpAssembly = Assembly.Load(dll.bytes);
                            baseInited = true;
                        }
                        catch (Exception e)
                        {
                            Log.E(TAG, e.ToString());
                            Log.E(TAG, "模组包 {0} EntryCode {1} 加载失败 : {2}", 
                                PackageName, EntryCode, e.Message);
                        }
                    }
                    else Log.E(TAG, "模组包 EntryCode {0} 未找到", EntryCode);
#else
                    Log.E(TAG, "模组包 {0} 加载代码错误：当前使用 IL2CPP 模式，因此 C# DLL 不能加载", PackageName);
#endif
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

        #region 模块运行环境

        /// <summary>
        /// C# 程序集
        /// </summary>
        public Assembly CSharpAssembly { get; private set; }
        /// <summary>
        /// C# 程序入口
        /// </summary>
        public object CSharpPackageEntry { get; private set; }

        /// <summary>
        /// Lua 虚拟机
        /// </summary>
        public LuaState PackageLuaState { get; private set; }
        /// <summary>
        /// Lua 虚拟机
        /// </summary>
        public PackageLuaServer PackageLuaServer { get; private set; }

        //管理当前模块下的所有lua虚拟脚本，统一管理、释放
        private List<GameLuaObjectHost> luaObjects = new List<GameLuaObjectHost>();

        /// <summary>
        /// 注册lua虚拟脚本到物体上
        /// </summary>
        /// <param name="name">lua虚拟脚本的名称</param>
        /// <param name="gameObject">要附加的物体</param>
        /// <param name="className">目标代码类名</param>
        public void RegisterLuaObject(string name, GameObject gameObject, string className)
        {
            GameLuaObjectHost newGameLuaObjectHost = gameObject.AddComponent<GameLuaObjectHost>();
            newGameLuaObjectHost.Name = name;
            newGameLuaObjectHost.Package = this;
            newGameLuaObjectHost.LuaState = PackageLuaState;
            newGameLuaObjectHost.LuaClassName = className;
            newGameLuaObjectHost.LuaModName = PackageName;
            luaObjects.Add(newGameLuaObjectHost);
        }
        /// <summary>
        /// 查找lua虚拟脚本
        /// </summary>
        /// <param name="name">lua虚拟脚本的名称</param>
        /// <param name="gameLuaObjectHost">输出lua虚拟脚本</param>
        /// <returns>返回是否找到对应脚本</returns>
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
        /// 获取模组启动代码是否已经执行
        /// </summary>
        /// <returns></returns>
        public bool GetEntryCodeExecuted() { return mainLuaCodeLoaded; }

        private bool mainLuaCodeLoaded = false;
        private bool luaStateInited = false;
        private bool baseInited = false;

        //初始化LUA虚拟机
        private async Task<bool> InitLuaState()
        {
            mainLuaCodeLoaded = false;
            requiredLuaFiles = new List<string>();
            requiredLuaClasses = new Dictionary<string, LuaFunction>();

            if (!string.IsNullOrEmpty(ShareLuaState))
            {
                //如果指定了core，则代码载入到全局lua虚拟机中
                if (ShareLuaState == "core")
                {
                    PackageLuaState = GameManager.Instance.GameMainLuaState;
                    LuaStateInitFinished();
                    return false;
                }

                GamePackage m = GameManager.Instance.GetSystemService<GamePackageManager>("GamePackageManager")
                    .FindPackage(ShareLuaState);

                if (m != null && m.PackageLuaState != null && m.luaStateInited)
                {
                    PackageLuaState = m.PackageLuaState;
                    LuaStateInitFinished();
                    return false;
                }
                else Log.E(TAG, "package {0} can not be load because " +
                    "the target mod lua environment is not ready or not load. now use independent LuaServer",
                    ShareLuaState);
            }

            PackageLuaServer = new PackageLuaServer(PackageName);
            PackageLuaState = PackageLuaServer.getLuaState();
            PackageLuaServer.init((i) => { }, LuaStateInitFinished);

            await new WaitUntil(IsLuaStateInitFinished);

            return true;
        }

        /// <summary>
        /// 获取Lua虚拟机是否初始化完成
        /// </summary>
        /// <returns></returns>
        public bool IsLuaStateInitFinished()
        {
            return luaStateInited;
        }

        private void LuaStateInitFinished()
        {
            luaStateInited = true;
        }

        /// <summary>
        /// 运行模块初始化代码
        /// </summary>
        /// <returns></returns>
        public bool RunPackageExecutionCode()
        {
            if (!baseInited)
            {
                Log.E(TAG, "RunPackageExecutionCode failed, package not load");
                GameErrorChecker.LastError = GameError.NotLoad;
                return false;
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

                    TextAsset lua = GetTextAsset(EntryCode);
                    if (lua == null) 
                        Log.E(TAG, "Run package EntryCode failed, function {0} not found", EntryCode);
                    else if (!mainLuaCodeLoaded)
                    {
                        Log.D(TAG, "Run package EntryCode {0} ", EntryCode);

                        try
                        {
                            mainLuaCodeLoaded = true;
                            PackageLuaState.doString(lua.text, PackageName + ":Main");
                            requiredLuaFiles.Add(EntryCode);
                        }
                        catch (Exception e)
                        {
                            Log.E(TAG, "模组 {0} 运行启动代码失败! {1}", PackageName, e.Message);
                            Log.E(TAG, e.ToString());
                            GameErrorChecker.LastError = GameError.ExecutionFailed;
                            return false;
                        }

                        LuaFunction fPackageEntry = GetLuaFun("PackageEntry");
                        if (fPackageEntry != null)
                        {
                            object b = fPackageEntry.call(this);
                            if (b is bool && !((bool)b))
                            {
                                Log.E(TAG, "模组 {0} PackageEntry 返回了错误", PackageName);
                                GameErrorChecker.LastError = GameError.ExecutionFailed;
                                return (bool)b;
                            }
                            return false;
                        }
                        else
                        {
                            Log.E(TAG, "模组 {0} 未找到 PackageEntry ", PackageName);
                            GameErrorChecker.LastError = GameError.FunctionNotFound;
                        }
                    }
                    else
                    {
                        Log.E(TAG, "无法重复运行模组启动代码 {0} {1} ", EntryCode, PackageName);
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
                        Log.E(TAG, "模组 PackageEntry 返回了错误");
                        GameErrorChecker.LastError = GameError.ExecutionFailed;
                        return (bool)b;
                    }
                    return true;
                }
            }

            return false;
        }
        //运行模块卸载回调
        public void RunPackageBeforeUnLoadCode()
        {
            if (CodeType == GamePackageCodeType.Lua)
            {
                if (mainLuaCodeLoaded)
                    CallLuaFun("PackageBeforeUnLoad", this);
            }
            else if (CodeType == GamePackageCodeType.CSharp)
            {
                if (CSharpAssembly != null)
                {
                    Type type = CSharpAssembly.GetType("Package");
                    MethodInfo methodInfo = type.GetMethod("PackageBeforeUnLoad");
                    methodInfo.Invoke(CSharpPackageEntry, null);
                }
            }
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
                PackageLuaState.Dispose();
                PackageLuaState = null;
            }
        }

        private List<string> requiredLuaFiles = null;
        private Dictionary<string, LuaFunction> requiredLuaClasses = null;

        /// <summary>
        /// 导入 Lua 类到当前模组虚拟机中。
        /// 注意，类函数以 “class_类名” 开头，
        /// 关于 Lua 类，请参考 Docs/LuaClass 。
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns>类创建函数</returns>
        /// <exception cref="MissingReferenceException">
        /// 如果没有在当前模组包中找到类文件或是类创建函数 class_* ，则抛出 MissingReferenceException 异常。
        /// </exception>
        /// <exception cref="Exception">
        /// 如果Lua执行失败，则抛出此异常。
        /// </exception>
        public LuaFunction RequireLuaClass(string className)
        {
            LuaFunction classInit;
            if (requiredLuaClasses.TryGetValue(className, out classInit))
                return classInit;

            classInit = PackageLuaState.getFunction("class_" + className);
            if (classInit != null)
            {
                requiredLuaClasses.Add(className, classInit);
                return classInit;
            }

            TextAsset lua = GetTextAsset(className);
            if (lua == null) lua = GetTextAsset(className + ".lua.txt");
            if (lua == null) lua = GetTextAsset(className + ".txt");
            if (lua == null)
                throw new MissingReferenceException(PackageName + " 无法导入 Lua class : " + className + " ,未找到该文件");

            try
            {
                PackageLuaState.doString(lua.text, PackageName + ":" + className);
            }
            catch (Exception e)
            {
                Log.E(TAG, e.ToString());
                GameErrorChecker.LastError = GameError.ExecutionFailed;

                throw new Exception(PackageName + " 无法导入 Lua class : " + e.Message);
            }

            classInit = PackageLuaState.getFunction("class_" + className);
            if (classInit == null)
            {
                throw new MissingReferenceException(PackageName + " 无法导入 Lua class : " +
                    className + ", 未找到初始类函数: class_" + className);
            }

            requiredLuaClasses.Add(className, classInit);
            return classInit;
        }
        /// <summary>
        /// 导入Lua文件到当前模组虚拟机中
        /// </summary>
        /// <param name="fileName">LUA文件名</param>
        /// <exception cref="MissingReferenceException">
        /// 如果没有在当前模组包中找到类文件或是类创建函数 class_* ，则抛出 MissingReferenceException 异常。
        /// </exception>
        /// <exception cref="Exception">
        /// 如果Lua执行失败，则抛出此异常。
        /// </exception>
        public bool RequireLuaFile(string fileName)
        {
            if (requiredLuaFiles.Contains(fileName))
                return true;

            TextAsset lua = GetTextAsset(fileName);
            if (lua == null)
                lua = GetTextAsset(fileName + ".lua.txt");
            if (lua == null)
                throw new MissingReferenceException(PackageName + " 无法导入 Lua : " + fileName + " ,未找到该文件");

            try
            {
                PackageLuaState.doString(lua.text, PackageName + ":" + GamePathManager.GetFileNameWithoutExt(fileName));
                requiredLuaFiles.Add(fileName);
            }
            catch (Exception e)
            {
                Log.E(TAG, e.ToString());
                GameErrorChecker.LastError = GameError.ExecutionFailed;

                throw new Exception(PackageName + " 无法导入 Lua class : " + e.Message);
            }

            return true;
        }

        #region LUA 函数调用

        /// <summary>
        /// 获取当前 模块主代码 的指定函数
        /// </summary>
        /// <param name="funName">函数名</param>
        /// <returns>返回函数，未找到返回null</returns>
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
        public void CallLuaFun(string funName)
        {
            LuaFunction f = GetLuaFun(funName);
            if (f != null) f.call();
            else Log.E(TAG, "CallLuaFun Failed because function {0} not founnd", funName);
        }
        /// <summary>
        /// 调用模块主代码的lua函数
        /// </summary>
        /// <param name="funName">lua函数名称</param>
        /// <param name="pararms">参数</param>
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
        public void CallLuaFun(string luaObjectName, string funName, params object[] pararms)
        {
            GameLuaObjectHost targetObject = null;
            if (FindLuaObject(luaObjectName, out targetObject))
                targetObject.CallLuaFun(funName, pararms);
            else Log.E(TAG, "CallLuaFun Failed because object {0} not founnd", luaObjectName);
        }

        #endregion

        #endregion

        #region 模块信息

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
            PackageVersion = ConverUtils.StringToInt(attributeVersion.Value, 0, "Package/version");

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
                return false;
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
                Type = ConverUtils.StringToEnum(nodeCodeType.InnerText, GamePackageType.Asset, "Type");
            XmlNode nodeShareLuaState = nodePackage.SelectSingleNode("ShareLuaState");
            if (nodeShareLuaState != null)
                ShareLuaState = nodeShareLuaState.InnerText;
            

            return true;
        }

        /// <summary>
        /// 模块文件路径
        /// </summary>
        public string PackageFilePath { get; private set; }
        /// <summary>
        /// 模块包名
        /// </summary>
        public string PackageName { get; private set; }
        /// <summary>
        /// 模块版本号
        /// </summary>
        public int PackageVersion { get; private set; }
        /// <summary>
        /// 基础信息
        /// </summary>
        public GamePackageBaseInfo BaseInfo { get; private set; }

        /// <summary>
        /// 加载错误
        /// </summary>
        public string LoadError { get; protected set; } = "";

        /// <summary>
        /// PackageDef文档
        /// </summary>
        public XmlDocument PackageDef { get; protected set; }
        /// <summary>
        /// AssetBundle
        /// </summary>
        public AssetBundle AssetBundle { get; protected set; }

        /// <summary>
        /// 表示模块目标游戏内核版本
        /// </summary>
        public int TargetVersion { get; private set; } = GameConst.GameBulidVersion;
        /// <summary>
        /// 表示模块可以正常使用的最低游戏内核版本
        /// </summary>
        public int MinVersion { get; private set; } = GameConst.GameBulidVersion;

        /// <summary>
        /// 入口代码
        /// </summary>
        public string EntryCode { get; private set; }
        /// <summary>
        /// 模块类型
        /// </summary>
        public GamePackageType Type { get; private set; } = GamePackageType.Asset;
        /// <summary>
        /// 代码类型
        /// </summary>
        public GamePackageCodeType CodeType { get; private set; } = GamePackageCodeType.Lua;
        /// <summary>
        /// 共享Lua虚拟机
        /// </summary>
        public string ShareLuaState { get; private set; }

        #endregion

        #region 资源读取

        /// <summary>
        /// 读取 资源包 中的资源
        /// </summary>
        /// <param name="pathorname">资源路径</param>
        /// <returns></returns>
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
        /// 读取 资源包 中的文字资源
        /// </summary>
        /// <param name="pathorname">资源路径</param>
        /// <returns></returns>
        public virtual TextAsset GetTextAsset(string pathorname)
        {
            return GetAsset<TextAsset>(pathorname);
        }
        /// <summary>
        /// 读取 资源包 中的 Prefab 资源
        /// </summary>
        /// <param name="pathorname">资源路径</param>
        /// <returns></returns>
        public virtual GameObject GetPrefabAsset(string pathorname)
        {
            return GetAsset<GameObject>(pathorname);

        }
        public virtual Texture GetTextureAsset(string pathorname) { return GetAsset<Texture>(pathorname); }
        public virtual Texture2D GetTexture2DAsset(string pathorname) { return GetAsset<Texture2D>(pathorname); }
        public virtual Sprite GetSpriteAsset(string pathorname) { return GetAsset<Sprite>(pathorname); }
        public virtual Material GetMaterialAsset(string pathorname) { return GetAsset<Material>(pathorname); }
        public virtual PhysicMaterial GetPhysicMaterialAsset(string pathorname) { return GetAsset<PhysicMaterial>(pathorname); }

        #endregion

        #region 模组操作

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

        //自定义数据,方便LUA层操作

        private Dictionary<string, object> modCustomData = new Dictionary<string, object>();

        public object AddCustomProp(string name, object data)
        {
            if (modCustomData.ContainsKey(name))
            {
                modCustomData[name] = data;
                return data;
            }
            modCustomData.Add(name, data);
            return data;
        }
        public object GetCustomProp(string name)
        {
            if (modCustomData.ContainsKey(name))
                return modCustomData[name];
            return null;
        }
        public object SetCustomProp(string name, object data)
        {
            if (modCustomData.ContainsKey(name))
            {
                object old = modCustomData[name];
                modCustomData[name] = data;
                return old;
            }
            return null;
        }
        public bool RemoveCustomProp(string name)
        {
            if (modCustomData.ContainsKey(name))
            {
                modCustomData.Remove(name);
                return true;
            }
            return false;
        }

        #endregion
    }
}
