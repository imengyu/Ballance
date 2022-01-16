using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Ballance2.Utils;
using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Services.Debug;
using Ballance2.Base.Handler;
using Ballance2.Utils.ServiceUtils;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameMediator.cs
* 
* 用途：
* 游戏中介者管理器，用于游戏中央事件与操作的转发与处理。
* 中介者提供了3中交互方法，分别是：
* 事件：
*   全局事件：发送整体全局事件，让多个接受方接受事件。
*   单一事件：发送单向全局事件，让一个接受方接受事件。
* 操作：
*   发送操作至一个接受方，获取操作返回值。
* 共享数据：
*   提供了全局数据，可注册数据观察者，观察数据变化。或设置全局数据。

* 作者：
* mengyu
*/

namespace Ballance2.Services
{
  /// <summary>
  /// 游戏中介者
  /// </summary>
  [Serializable]
  [SLua.CustomLuaClass]
  public class GameMediator : GameService
  {
    private readonly string TAG = "GameMediator";
    private GameObject go = null;
    private GameMediatorDelayCaller DelayCaller = null;

    public GameMediator() : base("GameMediator")
    {

    }
    public override void Destroy()
    {
      if (go != null)
        UnityEngine.Object.Destroy(go);
      UnLoadAllEvents();
      UnLoadAllActions();
    }
    public override bool Initialize()
    {
      InitAllEvents();
      InitAllActions();
      
      DelayCaller = gameObject.AddComponent<GameMediatorDelayCaller>();
      DelayCaller.GameMediator = this;

      RegisterEventHandler(GamePackage.GetSystemPackage(),
          GameEventNames.EVENT_BASE_INIT_FINISHED, TAG, (evtName, param) =>
          {
            InitCommands();
            return false;
          });
      return true;
    }

    #region 全局事件控制器

    [SerializeField, SetProperty("Events")]
    private Dictionary<string, GameEvent> events = new Dictionary<string, GameEvent>();
    private Dictionary<string, GameHandler> singleEvents = new Dictionary<string, GameHandler>();

    public Dictionary<string, GameEvent> Events { get { return events; } }

    /// <summary>
    /// 注册单一事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    public bool RegisterSingleEvent(string evtName)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "RegisterSingleEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      if (!singleEvents.ContainsKey(evtName))
        singleEvents.Add(evtName, null);
      return true;
    }
    /// <summary>
    /// 取消注册单一事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    public bool UnRegisterSingleEvent(string evtName)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "UnRegisterSingleEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      if (singleEvents.ContainsKey(evtName))
      {
        singleEvents.Remove(evtName);
        return true;
      }
      else
      {
        GameErrorChecker.LastError = GameError.NotRegister;
        return false;
      }
    }
    /// <summary>
    /// 获取单一事件是否注册
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <returns>是否注册</returns>
    public bool IsSingleEventRegistered(string evtName)
    {
      return singleEvents.ContainsKey(evtName);
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    public GameEvent RegisterGlobalEvent(string evtName)
    {
      GameEvent gameEvent;

      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "RegisterGlobalEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return null;
      }
      if (IsGlobalEventRegistered(evtName, out gameEvent))
        return gameEvent;

      gameEvent = new GameEvent(evtName);
      events.Add(evtName, gameEvent);
      return gameEvent;
    }
    /// <summary>
    /// 取消注册事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    public bool UnRegisterGlobalEvent(string evtName)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "UnRegisterGlobalEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      if (IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
      {
        gameEvent.Dispose();
        events.Remove(evtName);
        return true;
      }
      else
      {
        GameErrorChecker.LastError = GameError.NotRegister;
        return false;
      }
    }
    /// <summary>
    /// 获取事件是否注册
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <returns>是否注册</returns>
    public bool IsGlobalEventRegistered(string evtName)
    {
      return events.ContainsKey(evtName);
    }
    /// <summary>
    /// 获取事件是否注册，如果已注册，则返回实例
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="e">返回的事件实例</param>
    /// <returns>是否注册</returns>
    public bool IsGlobalEventRegistered(string evtName, out GameEvent e)
    {
      if (events.TryGetValue(evtName, out e))
        return true;
      e = null;
      return false;
    }
    /// <summary>
    /// 获取事件实例
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <returns>返回的事件实例</returns>
    public GameEvent GetRegisteredGlobalEvent(string evtName)
    {
      GameEvent gameEvent = null;

      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "GetRegisteredGlobalEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return gameEvent;
      }

      events.TryGetValue(evtName, out gameEvent);
      return gameEvent;
    }

    /// <summary>
    /// 检测单一事件是否被接收者附加
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <returns>返回是否附加</returns>
    public bool CheckSingleEventAttatched(string evtName)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "NotifySingleEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      if (singleEvents.TryGetValue(evtName, out GameHandler handler))
      {
        return (handler != null);
      }
      else
      {
        Log.W(TAG, "事件 {0} 未注册", evtName);
        GameErrorChecker.LastError = GameError.NotRegister;
        return false;
      }
    }

    /// <summary>
    /// 延时通知单一事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="delayeSecond">延时时长，单位秒</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回是否成功</returns>
    public bool DelayedNotifySingleEvent(string evtName, float delayeSecond, params object[] pararms)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "NotifySingleEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      DelayCaller.AddDelayCallSingle(evtName, delayeSecond, pararms);
      return true;
    }
    /// <summary>
    /// 通知单一事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回是否成功</returns>
    public bool NotifySingleEvent(string evtName, params object[] pararms)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "NotifySingleEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      if (singleEvents.TryGetValue(evtName, out GameHandler handler))
      {
        if (handler != null)
          handler.CallEventHandler(evtName, pararms);
        return true;
      }
      else
      {
        Log.W(TAG, "事件 {0} 未注册", evtName);
        GameErrorChecker.LastError = GameError.NotRegister;
        return false;
      }
    }

    /// <summary>
    /// 延时执行事件分发
    /// </summary>
    /// <param name="gameEvent">事件实例</param>
    /// <param name="delayeSecond">延时时长，单位秒</param>
    /// <param name="handlerFilter">指定事件可以接收到的名字（这里可以用正则）</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回已经发送的接收器个数</returns>
    public bool DelayedDispatchGlobalEvent(string evtName, string handlerFilter, float delayeSecond, params object[] pararms)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "NotifySingleEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      if (!IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
      {
        Log.W(TAG, "事件 {0} 未注册", evtName);
        GameErrorChecker.LastError = GameError.NotRegister;
      }
      DelayCaller.AddDelayCallNormal(evtName, handlerFilter, delayeSecond, pararms);
      return true;
    }

    /// <summary>
    /// 执行事件分发
    /// </summary>
    /// <param name="gameEvent">事件实例</param>
    /// <param name="handlerFilter">指定事件可以接收到的名字（这里可以用正则）</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回已经发送的接收器个数</returns>
    public int DispatchGlobalEvent(GameEvent gameEvent, string handlerFilter, params object[] pararms)
    {
      int handledCount = 0;
      if (gameEvent == null)
      {
        Log.W(TAG, "DispatchGlobalEvent gameEvent 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return handledCount;
      }

      //事件分发
      GameHandler gameHandler;
      for (int i = gameEvent.EventHandlers.Count - 1; i >= 0; i--)
      {
        gameHandler = gameEvent.EventHandlers[i];
        if (gameHandler.Destroyed)
          gameEvent.EventHandlers.RemoveAt(i);
        //筛选Handler
        if (handlerFilter == "*" || Regex.IsMatch(gameHandler.Name, handlerFilter))
        {
          handledCount++;
          if (gameHandler.CallEventHandler(gameEvent.EventName, pararms))
          {
            Log.D(TAG, "Event {0} was interrupted by : {1}", gameEvent.EventName, gameHandler.Name);
            break;
          }
        }
      }

      return handledCount;
    }
    /// <summary>
    /// 执行事件分发
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="handlerFilter">指定事件可以接收到的名字（这里可以用正则）</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回已经发送的接收器个数</returns>
    public int DispatchGlobalEvent(string evtName, string handlerFilter, params object[] pararms)
    {
      int handledCount = 0;

      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "DispatchGlobalEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return 0;
      }
      if (IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
        return DispatchGlobalEvent(gameEvent, handlerFilter, pararms);
      else
        GameErrorChecker.LastError = GameError.NotRegister;
      return handledCount;
    }

    //卸载所有事件
    private void UnLoadAllEvents()
    {
      foreach (var gameEvent in events)
        gameEvent.Value.Dispose();
      events.Clear();
      foreach (var gameEvent in singleEvents)
        gameEvent.Value.Dispose();
      singleEvents.Clear();
    }
    private void InitAllEvents()
    {
      //注册内置事件
      RegisterGlobalEvent(GameEventNames.EVENT_BASE_INIT_FINISHED);
      RegisterGlobalEvent(GameEventNames.EVENT_BEFORE_GAME_QUIT);
      RegisterGlobalEvent(GameEventNames.EVENT_LOGIC_SECNSE_ENTER);
      RegisterGlobalEvent(GameEventNames.EVENT_LOGIC_SECNSE_QUIT);
    }

    /// <summary>
    /// 订阅全局单一事件
    /// </summary>
    /// <param name="package">所属包</param>
    /// <param name="evtName">事件名称</param>
    /// <param name="name">接收器名字</param>
    /// <param name="gameHandlerDelegate">回调</param>
    /// <returns>返回接收器实例，如果失败，则返回null，具体请查看LastError</returns>
    public GameHandler SubscribeSingleEvent(GamePackage package, string evtName, string name, GameEventHandlerDelegate gameHandlerDelegate)
    {
      if (string.IsNullOrEmpty(evtName) || string.IsNullOrEmpty(name) || gameHandlerDelegate == null)
      {
        Log.W(TAG, "参数缺失", evtName);
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return null;
      }
      if (!singleEvents.ContainsKey(evtName))
        RegisterSingleEvent(evtName);
      var oldHandler = singleEvents[evtName];
      if (oldHandler != null)
      {
        Log.W(TAG, "单一事件 {0} 已由 {1} 订阅", evtName, oldHandler.Name);
        GameErrorChecker.LastError = GameError.NotRegister;
        return null;
      }

      GameHandler gameHandler = GameHandler.CreateCsEventHandler(package, name, gameHandlerDelegate);
      singleEvents[evtName] = gameHandler;
      return gameHandler;
    }
    /// <summary>
    /// 取消订阅全局单一事件
    /// </summary>
    /// <param name="package">所属包</param>
    /// <param name="evtName">事件名称</param>
    /// <param name="name">接收器名字</param>
    /// <param name="gameHandler">注册的处理器实例</param>
    /// <returns>返回是否成功</returns>
    public bool UnsubscribeSingleEvent(GamePackage package, string evtName, GameHandler gameHandler)
    {
      if (string.IsNullOrEmpty(evtName) || gameHandler == null)
      {
        Log.W(TAG, "参数缺失", evtName);
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }

      if (!singleEvents.ContainsKey(evtName))
      {
        GameErrorChecker.LastError = GameError.NotRegister;
        return false;
      }
      var oldHandler = singleEvents[evtName];
      if (oldHandler != gameHandler)
      {
        Log.W(TAG, "单一事件 {0} 已由 {1} 订阅，必须为当前订阅者才能取消", evtName, oldHandler.Name);
        GameErrorChecker.LastError = GameError.AccessDenined;
        return false;
      }

      singleEvents[evtName] = null;
      return true;
    }

    /// <summary>
    /// 注册全局事件接收器（Delegate）
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="name">接收器名字</param>
    /// <param name="gameHandlerDelegate">回调</param>
    /// <returns>返回注册的处理器，可使用这个处理器取消注册对应事件</returns>
    public GameHandler RegisterEventHandler(GamePackage package, string evtName, string name, GameEventHandlerDelegate gameHandlerDelegate)
    {
      if (string.IsNullOrEmpty(evtName)
         || string.IsNullOrEmpty(name)
         || gameHandlerDelegate == null)
      {
        Log.W(TAG, "参数缺失", evtName);
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return null;
      }

      if (!IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
        gameEvent = RegisterGlobalEvent(evtName);

      GameHandler gameHandler = GameHandler.CreateCsEventHandler(package, name, gameHandlerDelegate);
      gameEvent.EventHandlers.Add(gameHandler);
      return gameHandler;
    }

    /// <summary>
    /// 取消注册全局事件接收器
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="handler">接收器类</param>
    public void UnRegisterEventHandler(string evtName, GameHandler handler)
    {
      if (string.IsNullOrEmpty(evtName)
          || handler == null)
      {
        Log.W(TAG, "参数缺失", evtName);
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return;
      }

      if (IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
        gameEvent.EventHandlers.Remove(handler);
      else
      {
        GameErrorChecker.LastError = GameError.NotRegister;
      }
    }

    #endregion

    #region 全局操作控制器

    [SerializeField, SetProperty("Actions")]
    private Dictionary<string, GameActionStore> actionStores = null;

    /// <summary>
    /// 注册全局共享数据存储池
    /// </summary>
    /// <param name="package">所属包</param>
    /// <param name="name">池名称</param>
    /// <returns>如果注册成功，返回池对象；如果已经注册，则返回已经注册的池对象</returns>
    public GameActionStore RegisterActionStore(GamePackage package, string name)
    {
      string keyName = package.PackageName + ":" + name;
      GameActionStore store;
      if (string.IsNullOrEmpty(name))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG, "RegisterGlobalDataStore name 参数未提供");
        return null;
      }
      if (actionStores.ContainsKey(keyName))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, "共享操作仓库 {0} 已经注册", keyName);
        store = actionStores[keyName];
        return store;
      }

      store = new GameActionStore(package, name);
      actionStores.Add(keyName, store);
      return store;
    }
    /// <summary>
    /// 获取全局共享数据存储池
    /// </summary>
    /// <param name="package">所属包</param>
    /// <param name="name">池名称</param>
    /// <returns></returns>
    public GameActionStore GetActionStore(GamePackage package, string name)
    {
      actionStores.TryGetValue(package.PackageName + ":" + name, out GameActionStore s);
      return s;
    }
    /// <summary>
    /// 释放已注册的全局共享数据存储池
    /// </summary>
    /// <param name="package">所属包</param>
    /// <param name="name">池名称</param>
    /// <returns></returns>
    public bool UnRegisterActionStore(GamePackage package, string name)
    {
      string keyName = package.PackageName + ":" + name;
      if (string.IsNullOrEmpty(name))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG,
            "UnRegisterActionStore name 参数未提供");
        return false;
      }
      if (!actionStores.ContainsKey(keyName))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG,
            "共享操作仓库 {0} 未注册", keyName);
        return false;
      }

      actionStores[keyName].Destroy();
      actionStores.Remove(keyName);
      return false;
    }
    /// <summary>
    /// 释放已注册的全局共享数据存储池
    /// </summary>
    /// <param name="name">池名称</param>
    /// <returns></returns>
    public bool UnRegisterActionStore(GameActionStore store)
    {
      if (!actionStores.ContainsKey(store.KeyName))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG,
            "actionStores {0} 未注册", store.KeyName);
        return false;
      }

      actionStores[store.KeyName].Destroy();
      return false;
    }

    //卸载所属模块的全部操作
    internal void UnloadAllPackageActionStore(GamePackage package)
    {
      List<string> keys = new List<string>(actionStores.Keys);
      GameActionStore store;
      foreach (string key in keys)
      {
        store = actionStores[key];
        if (store.Package == package)
          UnRegisterActionStore(store);
      }
    }

    /// <summary>
    /// 调用操作
    /// </summary>
    /// <param name="package">所属包</param>
    /// <param name="storeName">操作仓库名称</param>
    /// <param name="name">操作名称</param>
    /// <param name="param">调用参数</param>
    /// <returns></returns>
    public GameActionCallResult CallAction(GamePackage package, string storeName, string name, params object[] param)
    {
      string keyName = package.PackageName + ":" + storeName;
      if (!actionStores.ContainsKey(keyName))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG,
            "共享操作仓库 {0} 未注册", keyName);
        return GameActionCallResult.FailResult;
      }
      return CallAction(actionStores[keyName], name, param);
    }
    /// <summary>
    /// 调用操作
    /// </summary>
    /// <param name="store">操作仓库</param>
    /// <param name="name">操作名称</param>
    /// <param name="param">调用参数</param>
    /// <returns></returns>
    public GameActionCallResult CallAction(GameActionStore store, string name, params object[] param)
    {
      return store.CallAction(name, param);
    }
    /// <summary>
    /// 调用操作
    /// </summary>
    /// <param name="action">操作实体</param>
    /// <param name="param">调用参数</param>
    /// <returns></returns>
    public GameActionCallResult CallAction(GameAction action, params object[] param)
    {
      GameErrorChecker.LastError = GameError.None;
      GameActionCallResult result = GameActionCallResult.FailResult;

      if (action == null)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG, "CallAction action 参数为空");
        return result;
      }
      if (action.Name == GameAction.Empty.Name)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.Empty, TAG, "CallAction action 为空");
        return result;
      }
      if (action.CallTypeCheck != null && action.CallTypeCheck.Length > 0)
      {
        //参数类型检查
        int argCount = action.CallTypeCheck.Length;
        if (argCount > param.Length)
        {
          Log.W(TAG, "操作 {0} 至少需要 {1} 个参数", action.Name, argCount);
          return result;
        }
        string allowType, typeName;
        for (int i = 0; i < argCount; i++)
        {
          allowType = action.CallTypeCheck[i];
          if (param[i] == null)
          {
            if (allowType != "null" &&
               (!allowType.Contains("/") && !allowType.Contains("null")))
            {
              Log.W(TAG, "操作 {0} 参数 {1} 不能为null", action.Name, i);
              return result;
            }
          }
          else
          {
            typeName = param[i].GetType().Name;
            if (allowType != typeName &&
                (!allowType.Contains("/") && !allowType.Contains(typeName)))
            {
              Log.W(TAG, "操作 {0} 参数 {1} 类型必须是 {2}", action.Name, i, action.CallTypeCheck[i]);
              return result;
            }
          }
        }
      }

      //Log.Log(TAG, "CallAction {0} -> {1}", action.Name, StringUtils.ValueArrayToString(param));

      result = action.GameHandler.CallActionHandler(param);
      if (!result.Success)
        Log.W(TAG, "操作 {0} 执行失败 {1}", action.Name, GameErrorChecker.LastError);

      return result;
    }

    /// <summary>
    /// 内核的 ActinoStore
    /// </summary>
    public GameActionStore SystemActionStore { get; private set; }

    private void UnLoadAllActions()
    {
      if (actionStores != null)
      {
        foreach (var actionStore in actionStores)
          actionStore.Value.Destroy();
        actionStores.Clear();
        actionStores = null;
      }
    }
    private void InitAllActions()
    {
      actionStores = new Dictionary<string, GameActionStore>();

      //注册内置事件
      SystemActionStore = RegisterActionStore(GamePackage.GetSystemPackage(), SYSTEM_ACTION_STORE_NAME);
      SystemActionStore.RegisterAction(GamePackage.GetSystemPackage(), "QuitGame", "GameManager", (param) =>
      {
        GameManager.Instance.QuitGame();
        return GameActionCallResult.SuccessResult;
      }, null);
    }

    /// <summary>
    /// 内核的 ActinoStore 名称
    /// </summary>
    public const string SYSTEM_ACTION_STORE_NAME = "core";

    #endregion

    #region 调试命令

    private void InitCommands()
    {
      GameManager.Instance.GameDebugCommandServer.RegisterCommand("gm", (keyword, full, argsCount, args) =>
      {
        switch (args[0])
        {
          case "single_event":
            return HandleSingleEventCommand(args);
          case "event":
            return HandleEventCommand(args);
          case "action":
            return HandleActionCommand(args);
        }
        return false;
      }, 1, "gm <single_event/event/action/store>\n" +
              "  single_event\n" +
              "    all 显示所有单一事件\n" +
              "    notify <eventName:string> [params:any[]] 通知一个单一事件\n" +
              "  event\n" +
              "    all 显示所有全局事件\n" +
              "    dispatch <eventName:string> <handleFilter:string> [params:any[]] 进行全局事件分发。handleFilter为“*”时表示所有，为正则使用正则匹配。\n" +
              "  action\n" +
              "    stores 显示所有全局操作仓库\n" +
              "    list <storeName:string> 显示指定操作仓库的所有操作\n" +
              "    call <storeName:string> <actionName:string> [params:any[]] 调用指定操作\n");
    }
    private bool HandleSingleEventCommand(string[] args)
    {
      string act = "";
      if (!DebugUtils.CheckDebugParam(1, args, out act)) return false;
      switch (act)
      {
        case "all":
          {
            StringBuilder stringBuilder = new StringBuilder("Single events: ");
            stringBuilder.AppendLine(events.Count.ToString());
            foreach (var i in singleEvents)
              stringBuilder.AppendLine(string.Format("{0} => {1}", i.Key, i.Value == null ? "(null)" : i.Value.Name));
            Log.V(TAG, stringBuilder.ToString());
            return true;
          }
        case "notify":
          {
            string name = "";
            if (!DebugUtils.CheckDebugParam(2, args, out name)) return false;
            if (NotifySingleEvent(name, StringUtils.TryConvertStringArrayToValueArray(args, 3)))
              Log.V(TAG, "NotifySingleEvent success");
            else
              Log.V(TAG, "NotifySingleEvent failed");
            return true;
          }
      }
      return false;
    }
    private bool HandleEventCommand(string[] args)
    {
      string act = "";
      if (!DebugUtils.CheckDebugParam(1, args, out act)) return false;
      switch (act)
      {
        case "all":
          {
            StringBuilder stringBuilder = new StringBuilder("Events: ");
            stringBuilder.AppendLine(events.Count.ToString());
            foreach (var i in events)
            {
              stringBuilder.AppendLine(string.Format("{0} => Handlers: {1}", i.Key, i.Value.EventHandlers.Count));
              foreach (var h in i.Value.EventHandlers)
              {
                stringBuilder.Append("  ");
                stringBuilder.AppendLine(h.Name);
              }
            }
            Log.V(TAG, stringBuilder.ToString());
            return true;
          }
        case "dispatch":
          {
            string name = "";
            string filter = "";
            if (!DebugUtils.CheckDebugParam(2, args, out name)) return false;
            if (!DebugUtils.CheckDebugParam(3, args, out filter)) return false;

            int handlers = DispatchGlobalEvent(name, filter, StringUtils.TryConvertStringArrayToValueArray(args, 4));
            Log.V(TAG, "DispatchGlobalEvent finish > {0}", handlers);
            return true;
          }
      }
      return false;
    }
    private bool HandleActionCommand(string[] args)
    {
      string act = "";
      if (!DebugUtils.CheckDebugParam(1, args, out act)) return false;
      switch (act)
      {
        case "stores":
          {
            StringBuilder stringBuilder = new StringBuilder("ActionStores: ");
            stringBuilder.AppendLine(actionStores.Count.ToString());
            foreach (var i in actionStores)
            {
              stringBuilder.AppendLine(string.Format("{0} => {2} > Package: {1} Actions: {3}",
                  i.Key, i.Value.Package.PackageName, i.Value.Name, i.Value.Actions.Count));
            }
            Log.V(TAG, stringBuilder.ToString());
            return true;
          }
        case "list":
          {
            string name = "";
            if (!DebugUtils.CheckDebugParam(2, args, out name)) return false;

            if (!actionStores.TryGetValue(name, out var actionStore))
            {
              Log.E(TAG, "未找到指定 ActionStore {0}", name);
              return false;
            }

            StringBuilder stringBuilder = new StringBuilder(actionStore.Name);
            stringBuilder.Append("Actions: ");
            stringBuilder.AppendLine(actionStore.Actions.Count.ToString());
            foreach (var i in actionStore.Actions)
            {
              stringBuilder.AppendLine(string.Format("{0} => Handler: {1}", i.Value.Name, i.Value.GameHandler.Name));
            }
            Log.V(TAG, stringBuilder.ToString());
            return true;
          }
        case "call":
          {
            string storename = "";
            string name = "";
            if (!DebugUtils.CheckDebugParam(2, args, out storename)) return false;
            if (!DebugUtils.CheckDebugParam(3, args, out name)) return false;

            CallAction(GamePackage.GetSystemPackage(), storename, name, StringUtils.TryConvertStringArrayToValueArray(args, 4));
            return true;
          }
      }
      return false;
    }

    #endregion
  }
}
