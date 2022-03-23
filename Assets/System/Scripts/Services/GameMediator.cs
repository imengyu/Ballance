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
using SLua;
using UnityEngine.Profiling;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameMediator.cs
* 
* 用途：
* 游戏中介者管理器，用于游戏中央事件的转发与处理。
* 中介者提供了事件交互方法：

*   全局事件：发送整体全局事件，让多个接受方接受事件。
*   单一事件：发送单向全局事件，让一个接受方接受事件。
*
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
  [LuaApiDescription("游戏中介者")]
  [LuaApiNotes(@"游戏中介者管理器，用于游戏中央事件的转发与处理。

中介者提供了事件交互方法：

* 全局事件：发送整体全局事件，让多个接受方接受事件。
* 单一事件：发送单向全局事件，让一个接受方接受事件。 
* 事件发射器：某个类发送一组事件，让许多接受方订阅事件。 

?> **提示：** 单一事件与全局数据无须手动使用 `RegisterGlobalEvent`/`RegisterSingleEvent` 注册，你可以直接执行相关方法，例如 `DispatchGlobalEvent` 等等，
如果事件没有注册，它会自动调用注册。

")]
  public class GameMediator : GameService
  {
    private readonly string TAG = "GameMediator";
    private GameObject go = null;
    private GameMediatorDelayCaller DelayCaller = null;

    public GameMediator() : base("GameMediator")
    {

    }
    [DoNotToLua]
    public override void Destroy()
    {
      if (go != null)
        UnityEngine.Object.Destroy(go);
      UnLoadAllEvents();
    }
    [DoNotToLua]
    public override bool Initialize()
    {
      InitAllEvents();
      
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
    private Dictionary<string, GameEventEmitter> eventEmitts = new Dictionary<string, GameEventEmitter>();
    private Dictionary<string, GameHandler> singleEvents = new Dictionary<string, GameHandler>();

    public Dictionary<string, GameEvent> Events { get { return events; } }

    #region 事件发射器

    /// <summary>
    /// 注册事件发射器
    /// </summary>
    /// <param name="name">事件发射器名称</param>
    /// <returns></returns>
    [LuaApiDescription("注册事件发射器")]
    [LuaApiParamDescription("name", "事件发射器名称")]
    public GameEventEmitter RegisterEventEmitter(string name) {
      GameEventEmitter result = null;
      if(eventEmitts.TryGetValue(name, out result))
        return result;
      result = new GameEventEmitter(name);
      return result; 
    }
    /// <summary>
    /// 取消注册事件发射器
    /// </summary>
    /// <param name="name">事件发射器名称</param>
    [LuaApiDescription("取消注册事件发射器")]
    [LuaApiParamDescription("name", "事件发射器名称")]
    public void UnRegisterEventEmitter(string name) {
      eventEmitts.Remove(name);
    }

    #endregion

    #region 单一事件

    /// <summary>
    /// 注册单一事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    [LuaApiDescription("注册单一事件")]
    [LuaApiParamDescription("evtName", "事件名称")]
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
    [LuaApiDescription("取消注册单一事件")]
    [LuaApiParamDescription("evtName", "事件名称")]
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
    [LuaApiDescription("获取单一事件是否注册", "是否注册")]
    [LuaApiParamDescription("evtName", "事件名称")]
    public bool IsSingleEventRegistered(string evtName)
    {
      return singleEvents.ContainsKey(evtName);
    }
    /// <summary>
    /// 检测单一事件是否被接收者附加
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <returns>返回是否附加</returns>
    [LuaApiDescription("检测单一事件是否被接收者附加", "返回是否附加")]
    [LuaApiParamDescription("evtName", "事件名称")]
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
    [LuaApiDescription("延时通知单一事件", "返回是否成功")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("delayeSecond", "延时时长，单位秒")]
    [LuaApiParamDescription("pararms", "事件参数")]
    public bool DelayedNotifySingleEvent(string evtName, float delayeSecond, params object[] pararms)
    {
      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "NotifySingleEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return false;
      }
      DelayCaller.AddDelayCallSingle(evtName, delayeSecond, LuaUtils.AutoCheckParamIsLuaTableAndConver(pararms));
      return true;
    }
    /// <summary>
    /// 通知单一事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回是否成功</returns>
    [LuaApiDescription("通知单一事件", "返回是否成功")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("pararms", "事件参数")]
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
        Profiler.BeginSample("NotifySingleEvent" + evtName);
        
        if (handler != null)
          handler.CallEventHandler(evtName, LuaUtils.AutoCheckParamIsLuaTableAndConver(pararms));

        Profiler.EndSample();
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
    /// 订阅全局单一事件
    /// </summary>
    /// <param name="package">所属包</param>
    /// <param name="evtName">事件名称</param>
    /// <param name="name">接收器名字</param>
    /// <param name="gameHandlerDelegate">回调</param>
    /// <returns>返回接收器实例，如果失败，则返回null，具体请查看LastError</returns>
    [LuaApiDescription("订阅全局单一事件", "返回接收器实例，如果失败，则返回null，具体请查看LastError")]
    [LuaApiParamDescription("package", "所属包")]
    [LuaApiParamDescription("name", "服务名称")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("name", "接收器名字")]
    [LuaApiParamDescription("gameHandlerDelegate", "回调")]
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
    [LuaApiDescription("取消订阅全局单一事件", "返回是否成功")]
    [LuaApiParamDescription("package", "所属包")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("name", "接收器名字")]
    [LuaApiParamDescription("gameHandler", "注册的处理器实例")]
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


    #endregion

    #region 全局事件

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="evtName">事件名称</param>
    [LuaApiDescription("注册事件")]
    [LuaApiParamDescription("evtName", "事件名称")]
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
    [LuaApiDescription("取消注册事件")]
    [LuaApiParamDescription("evtName", "事件名称")]
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
    [LuaApiDescription("获取事件是否注册", "是否注册")]
    [LuaApiParamDescription("evtName", "事件名称")]
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
    [LuaApiDescription("获取事件是否注册，如果已注册，则返回实例", "是否注册")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("e", "返回的事件实例")]
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
    [LuaApiDescription("获取事件实例", "返回的事件实例")]
    [LuaApiParamDescription("evtName", "事件名称")]
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
    /// 延时执行事件分发
    /// </summary>
    /// <param name="gameEvent">事件实例</param>
    /// <param name="delayeSecond">延时时长，单位秒</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回已经发送的接收器个数</returns>
    [LuaApiDescription("延时执行事件分发", "返回已经发送的接收器个数")]
    [LuaApiParamDescription("gameEvent", "事件实例")]
    [LuaApiParamDescription("delayeSecond", "延时时长，单位秒")]
    [LuaApiParamDescription("pararms", "事件参数")]
    public bool DelayedDispatchGlobalEvent(string evtName, float delayeSecond, params object[] pararms)
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
      DelayCaller.AddDelayCallNormal(evtName, delayeSecond, LuaUtils.AutoCheckParamIsLuaTableAndConver(pararms));
      return true;
    }

    /// <summary>
    /// 执行事件分发
    /// </summary>
    /// <param name="gameEvent">事件实例</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回已经发送的接收器个数</returns>
    [LuaApiDescription("执行事件分发", "返回已经发送的接收器个数")]
    [LuaApiParamDescription("gameEvent", "事件实例")]
    [LuaApiParamDescription("pararms", "事件参数")]
    public int DispatchGlobalEvent(GameEvent gameEvent, params object[] pararms)
    {
      Profiler.BeginSample("DispatchGlobalEvent_" + gameEvent.EventName);

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
        handledCount++;
        if (gameHandler.CallEventHandler(gameEvent.EventName, LuaUtils.AutoCheckParamIsLuaTableAndConver(pararms)))
        {
          Log.D(TAG, "Event {0} was interrupted by : {1}", gameEvent.EventName, gameHandler.Name);
          break;
        }
      }

      Profiler.EndSample();
      return handledCount;
    }
    /// <summary>
    /// 执行事件分发
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="pararms">事件参数</param>
    /// <returns>返回已经发送的接收器个数</returns>
    [LuaApiDescription("执行事件分发", "返回已经发送的接收器个数")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("pararms", "事件参数")]
    public int DispatchGlobalEvent(string evtName, params object[] pararms)
    {
      int handledCount = 0;

      if (string.IsNullOrEmpty(evtName))
      {
        Log.W(TAG, "DispatchGlobalEvent evtName 参数未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return 0;
      }
      if (IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
        return DispatchGlobalEvent(gameEvent, LuaUtils.AutoCheckParamIsLuaTableAndConver(pararms));
      else
        GameErrorChecker.LastError = GameError.NotRegister;
      return handledCount;
    }
    
    /// <summary>
    /// 注册全局事件接收器（Delegate）
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="name">接收器名字</param>
    /// <param name="gameHandlerDelegate">回调</param>
    /// <returns>返回注册的处理器，可使用这个处理器取消注册对应事件</returns>
    [LuaApiDescription("游戏中注册全局事件接收器介者", "返回注册的处理器，可使用这个处理器取消注册对应事件")]
    [LuaApiParamDescription("package", "所属包")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("name", "接收器名字")]
    [LuaApiParamDescription("luaModulHandler", "模块接收器函数标识符")]
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
    [LuaApiDescription("取消注册全局事件接收器")]
    [LuaApiParamDescription("evtName", "事件名称")]
    [LuaApiParamDescription("handler", "注册的处理器实例")]
    public void UnRegisterEventHandler(string evtName, GameHandler handler)
    {
      if (string.IsNullOrEmpty(evtName) || handler == null)
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

    //卸载所有事件
    private void UnLoadAllEvents()
    {
      foreach (var gameEvent in events)
        gameEvent.Value.Dispose();
      events.Clear();
      eventEmitts.Clear();
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
        }
        return false;
      }, 1, "gm <single_event/event/action/store>\n" +
              "  single_event\n" +
              "    all 显示所有单一事件\n" +
              "    notify <eventName:string> [params:any[]] 通知一个单一事件\n" +
              "  event\n" +
              "    all 显示所有全局事件\n" +
              "    dispatch <eventName:string> <handleFilter:string> [params:any[]] 进行全局事件分发。handleFilter为“*”时表示所有，为正则使用正则匹配。\n");
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

    #endregion
  }
}
