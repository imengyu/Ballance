using Ballance2.LuaHelpers;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Package;
using Ballance2.Sys.Services;
using Ballance2.Utils;
using SLua;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
 */

namespace Ballance2.Sys.Bridge.LuaWapper
{
  /// <summary>
  /// 简易 Lua 脚本承载组件（感觉很像 Lua 的 MonoBehaviour）
  /// </summary>
  /// <remarks>
  /// ✧使用方法：
  ///     ★ 可以直接绑定此组件至你的 Prefab 上，填写 LuaClassName 与 LuaPackageName，
  ///     Instantiate Prefab 后GameLuaObjectHost会自动找到模块并加载 Lua 文件并执行。
  ///     如果找不到模块或Lua 文件，将会抛出错误。
  ///     ★ 也可以在 GameMod 中直接调用 RegisterLuaObject 注册一个 Lua 对象
  ///     ☆ 以上两种方法都可以在 GamePackage 中使用 FindLuaObject 找到你注册的 Lua 对象
  /// 
  /// ✧参数引入
  ///     可以在编辑器中设置 LuaInitialVars 添加你想引入的参数，承载组件会自动将的参数设置
  ///     到你的 Lua脚本 self 上，通过 self.参数名 就可以访问了。
  ///     
  /// ✧Unity消息
  ///     ★ 你可以在 Lua 类中添加 Start、Update、Awake、OnDestroy等等方法（也可以不写），
  ///     承载组件会自动调用这些方法，Lua使用起来就像在C#中使用MonoBehaviour一样, 无需你写其他代码。
  ///     ★ GameLuaObjectHost默认实现 Awake Start Update FixedUpdate LateUpdate 
  ///     GUI Destroy Enable Disable 这几个事件，如果需要更多事件，在编辑器中 “Lua 类 On * 事件接收器”
  ///     这个设置中添加你需要的事件类别，编辑器中下方会显示可用的事件。
  ///     
  /// ✧你可以使用这几个变量来访问当前对象上的固定变量：
  ///     self.transform 访问当前对象的 transform;
  ///     self.monoBehaviour 访问当前 GameLuaObjectHost;
  ///     self.gameObject 访问 gameObject;
  ///     self.store 访问 GlobalStore
  ///     self.actionStore 访问 ActionStore;
  ///
  /// 
  /// </remarks>
  [CustomLuaClass]
  [AddComponentMenu("Ballance/Lua/GameLuaObjectHost")]
  [LuaApiDescription("简易 Lua 脚本承载组件")]
  public class GameLuaObjectHost : MonoBehaviour
  {
    public const string TAG = "GameLuaObjectHost";

    /// <summary>
    /// Lua 对象名字，用于 FindLuaObject 查找
    /// </summary>
    [Tooltip("Lua 对象名字，用于 FindLuaObject 查找")]
    [LuaApiDescription("Lua 对象名字")]
    public string Name;
    /// <summary>
    /// 获取或设置 Lua的类名（eg MenuLevel）
    /// </summary>
    [Tooltip("设置 Lua的类名（eg MenuLevel）")]
    [LuaApiDescription("Lua的类名")]
    public string LuaClassName;
    /// <summary>
    /// 获取或设置 Lua类的文件名（eg MenuLevel.lua）
    /// </summary>
    [Tooltip("设置 Lua类的文件名（eg MenuLevel.lua）")]
    [LuaApiDescription("Lua类的文件名")]
    public string LuaFileName;
    /// <summary>
    /// 获取或设置 Lua 类所在的模块包名（该模块类型必须是 Module 并可运行）。设置后该对象会自动注册到 LuaObject 中
    /// </summary>
    [Tooltip("设置 Lua 类所在的模块包名（该模块类型必须是 Module 并可运行）。设置后该对象会自动注册到 LuaObject 中")]
    [LuaApiDescription("Lua 类所在的模块包名")]
    public string LuaPackageName;
    /// <summary>
    /// 设置 Lua 初始参数，用于方便地从 Unity 编辑器直接引入初始参数至 Lua，这些变量会设置到 Lua self 上，可直接获取。
    /// </summary>
    /// <remarks>
    /// 提示：这些参数仅用于Lua对象初始化时来传递参数使用的，如果你在Lua中修改了变量值，或是在其他脚本中访问修改，
    /// 其不会自动更新，你需要手动调用 UpdateVarFromLua UpdateVarToLua 来更新对应数据。
    /// </remarks>
    [Tooltip("设置 Lua 初始参数，用于方便地从 Unity 编辑器直接引入初始参数至 Lua，这些变量会设置到 Lua self 上，可直接获取。")]
    [LuaApiDescription("Lua 初始参数")]
    [SerializeField]
    public List<LuaVarObjectInfo> LuaInitialVars = new List<LuaVarObjectInfo>();
    [SerializeField]
    public List<LuaVarObjectInfo> LuaPublicVars = new List<LuaVarObjectInfo>();
    [DoNotToLua]
    [Tooltip("设置 Lua 脚本执行顺序，等于0时立即初始化。这个值越大，脚本越晚被执行。请设置为大于等于0的值。")]
    [SerializeField]
    public int ExecuteOrder = 0;
    [Tooltip("是否创建 GlobalStore，勾选后会创建此Lua脚本的共享数据仓库(仓库名字是 包名:Name)，可以使用 self.store 或 GameLuaObjectHost.Store 访问 ")]
    [LuaApiDescription("是否创建 GlobalStore")]
    [SerializeField]
    public bool CreateStore = false;
    [Tooltip("是否自动创建共享操作仓库，勾选后会创建此Lua脚本的操作仓库(仓库名字是 包名:Name)，可以使用 self.actionStore 或 GameLuaObjectHost.ActionStore 访问 ")]
    [LuaApiDescription("是否自动创建共享操作仓库")]
    [SerializeField]
    public bool CreateActionStore = false;
    [Tooltip("在调试环境中加载脚本，需要先添加LuaDebugMini调试环境至场景中")]
    [DoNotToLua]
    [SerializeField]
    public bool DebugLoadScript = false;
    [DoNotToLua]
    [SerializeField]
    public bool ManualInputScript = false;
    [SerializeField]
    [Tooltip("Update和LateUpdate函数调用的间隔，为0时则不限制，小于0时禁用，大于0时每指定的Tick调用一次")]
    [LuaApiDescription("Update和LateUpdate函数调用的间隔，为0时则不限制，小于0时禁用，大于0时每指定的Tick调用一次")]
    public int UpdateDelta = 0;
    [Tooltip("FixUpdate函数调用的间隔，为0时则不限制，小于0时禁用，大于0时每指定的Tick调用一次")]
    [LuaApiDescription("FixUpdate函数调用的间隔，为0时则不限制，小于0时禁用，大于0时每指定的Tick调用一次")]
    public int FixUpdateDelta = 0;

    /// <summary>
    /// 获取lua self
    /// </summary>
    [LuaApiDescription("获取lua self")]
    public LuaTable LuaSelf { get { return self; } }
    /// <summary>
    /// 获取当前虚拟机
    /// </summary>
    [LuaApiDescription("获取当前虚拟机")]
    public LuaState LuaState { get; set; }
    /// <summary>
    /// 获取对应模块包
    /// </summary>
    [LuaApiDescription("获取对应模块包")]
    public GamePackage Package { get; set; }
    /// <summary>
    /// 获取此Lua脚本的共享数据仓库
    /// </summary>
    [LuaApiDescription("获取此Lua脚本的共享数据仓库")]
    public Store Store { get; set; }
    /// <summary>
    /// 获取此Lua脚本的共享操作仓库
    /// </summary>
    [LuaApiDescription("获取此Lua脚本的共享操作仓库")]
    public GameActionStore ActionStore { get; set; }
    /// <summary>
    /// 获取该 Lua 脚本所属包包名
    /// </summary>
    [LuaApiDescription("获取该 Lua 脚本所属包包名")]
    public string PackageName { get { return _PackageName; } }

    private int _ExecuteOrder = 0;

    private GamePackageManager _GamePackageManager;
    private GamePackageManager GamePackageManager {
      get {
        if(_GamePackageManager == null && GameManager.Instance != null)
          _GamePackageManager = GameManager.Instance.GetSystemService<GamePackageManager>();
        return _GamePackageManager;
      }
    }
    private string _PackageName = null;

    private LuaTable self = null;
    private LuaVoidDelegate update = null;
    private LuaVoidDelegate fixedUpdate = null;
    private LuaVoidDelegate lateUpdate = null;
    private LuaStartDelegate start = null;
    private LuaVoidDelegate awake = null;
    private LuaVoidDelegate onDestroy = null;
    private LuaVoidDelegate onEnable = null;
    private LuaVoidDelegate onDisable = null;
    private LuaVoidDelegate reset = null;

    private bool luaInited = false;
    private bool awakeCalledBeforeInit = false;
    private bool startCalledBeforeInit = false;
    private bool startCalled = false;
    private bool awakeCalled = false;

    public System.Action LuaInitFinished;

    private int updateTick = 0;
    private int fixUpdateTick = 0;
    private int lateUpdateTick = 0;

    /// <summary>
    /// 立即创建类，如果类已创建，则返回已创建的类
    /// </summary>
    /// <returns>返回类的table，如果创建失败，则返回null</returns>
    [LuaApiDescription("立即创建类，如果类已创建，则返回已创建的类", "返回类的table，如果创建失败，则返回nil")]
    public LuaTable CreateClass()
    {
      if (self == null)
      {
        LuaFunction classInit = null;
        if (DebugLoadScript)
        {
#if UNITY_EDITOR
          //直接加载
          if (LuaDebugMini.Instance == null)
          {
            Log.E(TAG + ":" + Name, "Not found LuaDebugMini, please add it");
            return null;
          }
          var path = LuaFileName;
          if (!path.EndsWith(".lua")) path += ".lua";
          if (!File.Exists(path))
          {
            Log.E(TAG + ":" + Name, "Not found lua file: '" + path + "' ");
            return null;
          }

          LuaDebugMini.LuaState.doString(File.ReadAllText(path)); 
          var CreateClass = (LuaDebugMini.LuaState["CreateClass"] as LuaTable);
          if(CreateClass == null)
            throw new MissingReferenceException("This shouldn't happen: CreateClass is null! ");

          classInit = CreateClass["LuaClassName"] as LuaFunction;
#else
          Log.E(TAG + ":" + Name, "DebugLoadScript can only use in editor");
#endif
        }
        else if (Package == null && !InitPackage()) {
          Log.W(TAG + ":" + Name, "Package is null or not init: " + GameErrorChecker.LastError);
          GameErrorChecker.LastError = GameError.NotLoad;
          return null;
        } else
          classInit = Package.RequireLuaClass(LuaClassName);

        //Create class
        if (classInit == null)
        {
          Log.E(TAG + ":" + Name, "LuaObject {0} create error : class not found : {1}", Name, LuaClassName);
          GameErrorChecker.LastError = GameError.ClassNotFound;
          return null;
        }

        object o = classInit.call();
        if (o != null && o is LuaTable) self = o as LuaTable;
        else
        {
          Log.E(TAG + ":" + Name, "LuaObject {0} create error : table not return ", Name);
          GameErrorChecker.LastError = GameError.NotReturn;
          return null;
        }
      }

      return self;
    }
    /// <summary>
    /// 从已附加 GameLuaObjectHost 的 GameObject 上获取Lua类table
    /// </summary>
    /// <param name="go">已附加 GameLuaObjectHost 的 GameObject</param>
    /// <returns>如果找到，则返回类table，如果没找到则返回null</returns>
    [LuaApiDescription("从已附加 GameLuaObjectHost 的 GameObject 上获取Lua类table", "如果找到，则返回类table，如果没找到则返回nIl")]
    [LuaApiParamDescription("go", "已附加 GameLuaObjectHost 的 GameObject")]
    public static LuaTable GetLuaClassFromGameObject(GameObject go)
    {
      var cls = go.GetComponent<GameLuaObjectHost>();
      if (cls != null)
        return cls.CreateClass();
      return null;
    }


    private void DoInit()
    {
      if (!LuaInit())
      {
        enabled = false;
      }
      else
      {
        if (LuaInitFinished != null) LuaInitFinished.Invoke();
        if (awakeCalledBeforeInit && awake != null) CallAwake();
        if (startCalledBeforeInit && start != null && !startCalled)
        {
          startCalled = true;
          start(self, gameObject);
        }
      }
    }
    private void CallAwake() {
      if(!awakeCalled && awake != null) {
        awake(self);
        awakeCalled = true;
      }
    }

    private void Start()
    {
      if (!luaInited) startCalledBeforeInit = true;
      else if (start != null && !startCalled)
      {
        startCalled = true;
        start(self, gameObject);
      }
    }
    private void Awake()
    {
      _ExecuteOrder = ExecuteOrder;
      if (string.IsNullOrEmpty(Name)) Name = gameObject.name;
      if (ExecuteOrder == 0) DoInit();
      if (!luaInited) awakeCalledBeforeInit = true;
      else if (awake != null) CallAwake();
    }
    private void Update()
    {
      if (_ExecuteOrder >= 0)
      {
        _ExecuteOrder--;
        if (_ExecuteOrder <= 0)
          DoInit();
      }
      if (UpdateDelta >= 0)
      {
        if (updateTick > 0)
          updateTick--;
        else
        {
          updateTick = UpdateDelta;
          if (update != null) update(self);
        }
      }
    }
    private void FixedUpdate()
    {
      if (FixUpdateDelta >= 0)
      {
        if (fixUpdateTick > 0)
          fixUpdateTick--;
        else
        {
          fixUpdateTick = FixUpdateDelta;
          if (fixedUpdate != null) fixedUpdate(self);
        }
      }
    }
    private void LateUpdate()
    {
      if (UpdateDelta >= 0)
      {
        if (lateUpdateTick > 0)
          lateUpdateTick--;
        else
        {
          lateUpdateTick = UpdateDelta;
          if (lateUpdate != null) lateUpdate(self);
        }
      }
    }

    private void OnDestroy()
    {
      if (onDestroy != null) onDestroy(self);
      StopLuaEvents();
      self = null;
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
      if (CreateClass() == null)
        return false;
      InitLuaInternalVars();
      InitLuaVars(); //初始化引入参数
      InitLuaEvents();
      //调用其他Lua初始化脚本

      if (OnInitLua != null) OnInitLua.Invoke();
      return true;
    }
    private bool InitPackage()
    {
      if (GamePackageManager == null)
      {
        GameErrorChecker.LastError = GameError.SystemNotInit;
        return false;
      }
      if (Package == null)
      {
        if (string.IsNullOrEmpty(LuaPackageName))
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
          Store = GameManager.GameMediator.RegisterGlobalDataStore(_PackageName + ":" + Name);
        }
        if (CreateActionStore)
        {
          ActionStore = GameManager.GameMediator.RegisterActionStore(Package, Name);
        }

        LuaState = Package.PackageLuaState;
        if (LuaState == null)
        {
          Log.E(TAG + ":" + Name, "LuaObject {0} load error :  Mod can not run : {1}", Name, LuaPackageName);
          GameErrorChecker.LastError = GameError.PackageCanNotRun;
          return false;
        }
      }
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

      fun = self["FixedUpdate"] as LuaFunction;
      if (fun != null) fixedUpdate = fun.cast<LuaVoidDelegate>();

      fun = self["LateUpdate"] as LuaFunction;
      if (fun != null) lateUpdate = fun.cast<LuaVoidDelegate>();

      fun = self["OnEnable"] as LuaFunction;
      if (fun != null) onEnable = fun.cast<LuaVoidDelegate>();

      fun = self["OnDisable"] as LuaFunction;
      if (fun != null) onDisable = fun.cast<LuaVoidDelegate>();

      fun = self["OnDestroy"] as LuaFunction;
      if (fun != null) onDestroy = fun.cast<LuaVoidDelegate>();

      fun = self["Reset"] as LuaFunction;
      if (fun != null) reset = fun.cast<LuaVoidDelegate>();
    }
    private void InitLuaInternalVars()
    {
      LuaSelf["transform"] = transform;
      LuaSelf["monoBehaviour"] = this;
      LuaSelf["gameObject"] = gameObject;
      LuaSelf["store"] = Store;
      LuaSelf["actionStore"] = ActionStore;
      LuaSelf["package"] = Package;
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
      onDestroy = null;
    }

    public VoidDelegate OnInitLua;

    /// <summary>
    /// 更新 lua 脚本的所有 InitialVars 至 lua table上
    /// </summary>
    [LuaApiDescription("更新 lua 脚本的所有 InitialVars 至 lua table上")]
    public void UpdateAllVarToLua()
    {
      foreach (LuaVarObjectInfo objectInfo in LuaInitialVars)
        objectInfo.UpdateToLua(LuaSelf);
    }
    /// <summary>
    /// 更新所有  lua table上的 InitialVars 至当前脚本上
    /// </summary>
    [LuaApiDescription("更新所有  lua table上的 InitialVars 至当前脚本上")]
    public void UpdateAllVarFromLua()
    {
      foreach (LuaVarObjectInfo objectInfo in LuaInitialVars)
        objectInfo.UpdateFromLua(LuaSelf);
    }
    /// <summary>
    /// 更新指定的初始变量 LuaVarObjectInfo 至 lua 脚本上
    /// </summary>
    /// <param name="v">初始变量名称</param>
    [LuaApiDescription("更新指定的初始变量 LuaVarObjectInfo 至 lua 脚本上")]
    [LuaApiParamDescription("v", "初始变量名称")]
    public void UpdateVarToLua(LuaVarObjectInfo v)
    {
      if (!string.IsNullOrEmpty(v.Name))
        v.UpdateToLua(LuaSelf);
    }
    /// <summary>
    /// 从 lua 脚本上获取 lua 变量更新至 LuaVarObjectInfo 
    /// </summary>
    /// <param name="v">初始变量名称</param>
    [LuaApiDescription("从 lua 脚本上获取 lua 变量更新至 LuaVarObjectInfo ")]
    [LuaApiParamDescription("v", "初始变量名称")]
    public void UpdateVarFromLua(LuaVarObjectInfo v)
    {
      v.UpdateFromLua(LuaSelf);
    }
    /// <summary>
    /// 将指定名字的 lua 变量更新至 LuaVarObjectInfo 
    /// </summary>
    /// <param name="paramName">变量名称</param>
    /// <returns>如果没有找到变量，则返回false，否则返回true。</returns>
    [LuaApiDescription("将指定名字的 lua 变量更新至 LuaVarObjectInfo ", "如果没有找到变量，则返回false，否则返回true")]
    [LuaApiParamDescription("paramName", "变量名称")]
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
    [LuaApiDescription("获取当前 Lua 类(等于LuaSelf)")]
    public LuaTable GetLuaClass()
    {
      return self;
    }
    /// <summary>
    /// 获取当前 Object 的指定函数
    /// </summary>
    /// <param name="funName">函数名</param>
    /// <returns>返回函数，未找到返回null</returns>
    [LuaApiDescription("获取当前 Object 的指定函数", "返回函数，未找到返回null")]
    [LuaApiParamDescription("funName", "函数名")]
    public LuaFunction GetLuaFun(string funName)
    {
      LuaFunction f = null;
      if (self != null) f = self[funName] as LuaFunction;
      else if (LuaState != null) f = LuaState.getFunction(funName);
      return f;
    }
    /// <summary>
    /// 调用lua无参函数
    /// </summary>
    /// <param name="funName">lua函数名称</param>
    [LuaApiDescription("调用lua无参函数")]
    [LuaApiParamDescription("funName", "lua函数名称")]
    public void CallLuaFun(string funName)
    {
      LuaFunction f = GetLuaFun(funName);
      if (f != null)
        f.call(self);
    }
    /// <summary>
    /// 调用lua函数
    /// </summary>
    /// <param name="funName">lua函数名称</param>
    /// <param name="pararms">参数</param>
    /// <returns>Lua函数返回的对象，如果调用该函数失败，则返回null</returns>
    [LuaApiDescription("调用lua函数", "Lua函数返回的对象，如果调用该函数失败，则返回null")]
    [LuaApiParamDescription("funName", "lua函数名称")]
    [LuaApiParamDescription("pararms", "参数")]
    public object CallLuaFunWithParam(string funName, params object[] pararms)
    {
      LuaFunction f = GetLuaFun(funName);
      if (f != null) return f.call(self, pararms);
      return null;
    }
  }
}
