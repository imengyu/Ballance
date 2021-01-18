using Ballance2.System.Debug;
using Ballance2.System.Package;
using Ballance2.System.Services;
using Ballance2.Utils;
using SLua;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameLuaObjectHost.cs
 * 用途：
 * MonoBehaviour 的 Lua 包装类
 * 
 * 作者：
 * mengyu
 * 
 * 更改历史：
 * 2020-1-1 创建
 *
 */

namespace Ballance2.System.Bridge.LuaWapper
{
    /// <summary>
    /// 简易 Lua 脚本承载组件（感觉很像 Lua 的 MonoBehaviour）
    /// </summary>
    /// <remarks>
    /// ✧使用方法：
    ///     ★ 可以直接绑定此组件至你的 Prefab 上，填写 LuaClassName 与 LuaPackageName，
    ///     Instantiate Prefab 后GameLuaObjectHost会自动找到模块并加载 LUA 文件并执行。
    ///     如果找不到模块或LUA 文件，将会抛出错误。
    ///     ★ 也可以在 GameMod 中直接调用 RegisterLuaObject 注册一个 Lua 对象
    ///     ☆ 以上两种方法都可以在 GameMod 中使用 FindLuaObject 找到你注册的 Lua 对象
    /// 
    /// ✧参数引入
    ///     可以在编辑器中设置 LuaInitialVars 添加你想引入的参数，承载组件会自动将的参数设置
    ///     到你的 Lua脚本 self 上，通过 self.参数名 就可以访问了。
    ///     
    /// ✧Unity消息
    ///     ★ 你可以在 Lua 类中添加 Start、Update、Awake、OnDestroy等等方法（也可以不写），
    ///     承载组件会自动调用这些方法，Lua使用起来就像在C#中使用MonoBehaviour一样, 无需你写其他代码。
    ///     ★ GameLuaObjectHost默认实现 Awake Start Update FixedUpdate LateUpdate 
    ///     GUI Destory Enable Disable 这几个事件，如果需要更多事件，在编辑器中 “Lua 类 On * 事件接收器”
    ///     这个设置中添加你需要的事件类别，编辑器中下方会显示可用的事件。
    /// 
    /// </remarks>
    [CustomLuaClass]
    [AddComponentMenu("Ballance/Lua/GameLuaObjectHost")]
    public class GameLuaObjectHost : MonoBehaviour
    {
        public const string TAG = "GameLuaObjectHost";

        /// <summary>
        /// LUA 对象名字，用于 FindLuaObject 查找
        /// </summary>
        [Tooltip("LUA 对象名字，用于 FindLuaObject 查找")]
        public string Name;

        /// <summary>
        /// 获取或设置 Lua类的文件名（eg MenuLevel）
        /// </summary>
        [Tooltip("设置 Lua类的文件名（eg MenuLevel）")]
        public string LuaClassName;
        /// <summary>
        /// 获取或设置 Lua 类所在的模块包名（该模块类型必须是 Module 并可运行）。设置后该对象会自动注册到 LuaObject 中
        /// </summary>
        [Tooltip("设置 Lua 类所在的模块包名（该模块类型必须是 Module 并可运行）。设置后该对象会自动注册到 LuaObject 中")]
        public string LuaPackageName;
        /// <summary>
        /// 设置 LUA 初始参数，用于方便地从 Unity 编辑器直接引入初始参数至 Lua，这些变量会设置到 Lua self 上，可直接获取。
        /// </summary>
        /// <remarks>
        /// 提示：这些参数仅用于LUA对象初始化时来传递参数使用的，如果你在LUA中修改了变量值，或是在其他脚本中访问修改，
        /// 其不会自动更新，你需要手动调用 UpdateVarFromLua UpdateVarToLua 来更新对应数据。
        /// </remarks>
        [Tooltip("设置 LUA 初始参数，用于方便地从 Unity 编辑器直接引入初始参数至 Lua，这些变量会设置到 Lua self 上，可直接获取。")]
        [SerializeField]
        public List<LuaVarObjectInfo> LuaInitialVars = new List<LuaVarObjectInfo>();
        /// <summary>
        /// 设置 LUA 脚本执行顺序，这个值越大，脚本越晚被执行。(仅在加载时有效)
        /// </summary>
        [DoNotToLua]
        [Tooltip("设置 LUA 脚本执行顺序，这个值越大，脚本越晚被执行。")]
        [SerializeField]
        public int ExecuteOrder = 0;
        [Tooltip("是否创建 GlobalStore，勾选后会创建此Lua脚本的共享数据仓库(仓库名字是 包名:Name)，可以使用 self.store 或 GameLuaObjectHost.Store 访问 ")]
        [SerializeField]
        public bool CreateStore = false;
        [Tooltip("是否自动创建共享操作仓库，勾选后会创建此Lua脚本的操作仓库(仓库名字是 包名:Name)，可以使用 self.actionStore 或 GameLuaObjectHost.ActionStore 访问 ")]
        [SerializeField]
        public bool CreateActionStore = false;

        /// <summary>
        /// lua self
        /// </summary>
        public LuaTable LuaSelf { get { return self; } }
        /// <summary>
        /// 获取当前虚拟机
        /// </summary>
        public LuaState LuaState { get; set; }
        /// <summary>
        /// 获取对应 模组包
        /// </summary>
        public GamePackage Package { get; set; }
        /// <summary>
        /// 获取此Lua脚本的共享数据仓库
        /// </summary>
        public Store Store { get; set; }
        /// <summary>
        /// 获取此Lua脚本的共享操作仓库
        /// </summary>
        public GameActionStore ActionStore { get; set; }
        /// <summary>
        /// 获取该 Lua 脚本所属包包名
        /// </summary>
        public string PackageName { get { return _PackageName; } }

        private GamePackageManager GamePackageManager;
        private string _PackageName = null;

        private LuaTable self = null;
        private LuaVoidDelegate update = null;
        private LuaVoidDelegate fixedUpdate = null;
        private LuaVoidDelegate lateUpdate = null;
        private LuaStartDelegate start = null;
        private LuaVoidDelegate awake = null;
        private LuaVoidDelegate onGUI = null;
        private LuaVoidDelegate onDestory = null;
        private LuaVoidDelegate onEnable = null;
        private LuaVoidDelegate onDisable = null;
        private LuaVoidDelegate reset = null;

        private bool luaInited = false;
        private bool awakeCalledBeforeInit = false;
        private bool startCalledBeforeInit = false;

        private void DoInit()
        {
            GamePackageManager = GameManager.Instance.GetSystemService<GamePackageManager>("GamePackageManager");

            if (!LuaInit())
            {
                enabled = false;
                Log.E(TAG + ":" + Name, "LuaObject {0} disabled because load error", Name);
            }
            else
            {
                if (awakeCalledBeforeInit && awake != null) awake(self);
                if (startCalledBeforeInit && start != null) start(self, gameObject);      
            }
        }

        private void Start()
        {
            if (!luaInited) awakeCalledBeforeInit = true;
            if (start != null) start(self, gameObject);
        }
        private void Awake()
        {
            if (ExecuteOrder == 0) DoInit();
            if (!luaInited) awakeCalledBeforeInit = true;
            if (awake != null) awake(self);
        }
        private void Update()
        {
            if (ExecuteOrder >= 0)
            {
                ExecuteOrder--;
                if (ExecuteOrder == 0)
                    DoInit();
            }
            if (update != null) update(self);
        }
        private void FixedUpdate()
        {
            if (fixedUpdate != null) fixedUpdate(self);
        }
        private void LateUpdate()
        {
            if (lateUpdate != null) lateUpdate(self);
        }

        private void OnGUI()
        {
            if (onGUI != null) onGUI(self);
        }
        private void OnDestroy()
        {
            if (onDestory != null) onDestory(self);
            StopLuaEvents();
            if (Package != null) Package.RemoveLuaObject(this);
        }
        private void OnDisable()
        {
            if (onDisable != null) onDisable(self);
        }
        private void OnEnable()
        {
            if (onEnable != null) onEnable(self);
        }

        private void Reset()
        {
            if (reset != null) reset(self);
        }

        // Init and get
        // ===========================

        private bool LuaInit()
        {
            if(Package ==  null)
            {
                if(string.IsNullOrEmpty(LuaPackageName))
                {
                    Log.E(TAG + ":" + Name, "LuaObject {0} load error :  LuaPackageName not provide ", Name);
                    GameErrorChecker.LastError = GameError.ParamNotProvide;
                    return false;
                }

                Package = GamePackageManager.FindPackage(LuaPackageName); 

                if (Package == null)
                {
                    Log.E(TAG + ":" + Name, "LuaObject {0} load error :  LuaPackageName not found : {1}", Name, LuaPackageName);
                    GameErrorChecker.LastError = GameError.NotRegister;
                    return false;
                }

                Package.AddeLuaObject(this);

                _PackageName = Package.PackageName;

                if (CreateStore)
                {
                    Store = GameManager.GameMediator.RegisterGlobalDataStore(Name);
                }
                if (CreateActionStore)
                {
                    ActionStore = GameManager.GameMediator.RegisterActionStore(Package, Name);
                }

                LuaState = Package.PackageLuaState;
                if(LuaState == null)
                {
                    Log.E(TAG + ":" + Name, "LuaObject {0} load error :  Mod can not run : {1}", Name, LuaPackageName);
                    GameErrorChecker.LastError = GameError.PackageCanNotRun;
                    return false;
                }
            }

            LuaFunction classInit = Package.RequireLuaClass(LuaClassName);
            if (classInit == null)
            {
                Log.E(TAG + ":" + Name, "LuaObject {0} load error : class not found : {1}", Name, LuaClassName);
                GameErrorChecker.LastError = GameError.ClassNotFound;
                return false;
            }

            object o = classInit.call();
            if (o != null && o is LuaTable) self = o as LuaTable;
            else
            {
                Log.E(TAG + ":" + Name, "LuaObject {0} load error : table not return ", Name);
                GameErrorChecker.LastError = GameError.NotReturn;
                return false;
            }

            InitLuaInternalVars();
            InitLuaVars(); //初始化引入参数
            //调用其他LUA初始化脚本
            SendMessage("OnInitLua", gameObject, SendMessageOptions.DontRequireReceiver);
            InitLuaEvents();
            return true;
        }
        private void InitLuaEvents()
        {
            LuaFunction fun;

            fun = self["Start"] as LuaFunction;
            if (fun != null) start = fun.cast<LuaStartDelegate>();

            fun = self["Update"] as LuaFunction;
            if (fun != null) update = fun.cast<LuaVoidDelegate>();

            fun = self["Awake"] as LuaFunction;
            if (fun != null) awake = fun.cast<LuaVoidDelegate>();

            fun = self["OnGUI"] as LuaFunction;
            if (fun != null) onGUI = fun.cast<LuaVoidDelegate>();

            fun = self["FixedUpdate"] as LuaFunction;
            if (fun != null) fixedUpdate = fun.cast<LuaVoidDelegate>();

            fun = self["LateUpdate"] as LuaFunction;
            if (fun != null) lateUpdate = fun.cast<LuaVoidDelegate>();

            fun = self["onEnable"] as LuaFunction;
            if (fun != null) onEnable = fun.cast<LuaVoidDelegate>();

            fun = self["OnDisable"] as LuaFunction;
            if (fun != null) onDisable = fun.cast<LuaVoidDelegate>();

            fun = self["Reset"] as LuaFunction;
            if (fun != null) reset = fun.cast<LuaVoidDelegate>();
        }
        private void InitLuaInternalVars()
        {
            LuaSelf["transform"] = transform;
            LuaSelf["gameObject"] = gameObject;
            LuaSelf["store"] = Store;
            LuaSelf["actionStore"] = ActionStore;
        }
        private void InitLuaVars()
        {
            foreach (LuaVarObjectInfo v in LuaInitialVars)
                UpdateVarToLua(v);
        }
        private void StopLuaEvents()
        {
            update = null;
            start = null;
            awake = null;
            onGUI = null;
            onDestory = null;
        }

        /// <summary>
        /// 更新 LuaVarObjectInfo 至 lua 脚本上
        /// </summary>
        /// <param name="v"></param>
        public void UpdateVarToLua(LuaVarObjectInfo v)
        {
            if (!string.IsNullOrEmpty(v.Name))
                v.UpdateToLua(LuaSelf);
        }
        /// <summary>
        /// 从 lua 脚本上获取 lua 变量更新至 LuaVarObjectInfo 
        /// </summary>
        /// <param name="v"></param>
        public void UpdateVarFromLua(LuaVarObjectInfo v)
        {
            v.UpdateFromLua(LuaSelf);
        }
        /// <summary>
        /// 将指定名字的 lua 变量更新至 LuaVarObjectInfo 
        /// </summary>
        /// <param name="paramName">变量名称</param>
        /// <returns>如果没有找到变量，则返回false，否则返回true。</returns>
        public bool UpdateVarFromLua(string paramName)
        {
            foreach (LuaVarObjectInfo v in LuaInitialVars)
                if (v.Name == paramName)
                {
                    v.UpdateFromLua(LuaSelf);
                    return true;
                }
            return false;
        }

        /// <summary>
        /// 获取当前 Lua 类
        /// </summary>
        /// <returns></returns>
        public LuaTable GetLuaClass()
        {
            return self;
        }
        /// <summary>
        /// 获取当前 Object 的指定函数
        /// </summary>
        /// <param name="funName">函数名</param>
        /// <returns>返回函数，未找到返回null</returns>
        public LuaFunction GetLuaFun(string funName)
        {
            LuaFunction f;
            if (self != null) f = self[funName] as LuaFunction;
            else f = LuaState.getFunction(funName);
            return f;
        }
        /// <summary>
        /// 调用的lua无参函数
        /// </summary>
        /// <param name="funName">lua函数名称</param>
        public void CallLuaFun(string funName)
        {
            LuaFunction f = GetLuaFun(funName);
            if (f != null) f.call(self);
        }
        /// <summary>
        /// 调用的lua函数
        /// </summary>
        /// <param name="funName">lua函数名称</param>
        /// <param name="pararms">参数</param>
        public void CallLuaFun(string funName, params object[] pararms)
        {
            LuaFunction f = GetLuaFun(funName);
            if (f != null) f.call(self, pararms);
        }
    }
}
