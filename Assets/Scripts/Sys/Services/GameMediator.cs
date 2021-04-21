using Ballance.LuaHelpers;
using Ballance2.Sys.Bridge;
using Ballance2.Sys.Bridge.Handler;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Package;
using Ballance2.Utils;
using SLua;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameMediator.cs
* 
* 用途：
* 游戏中介者管理器，用于游戏中央事件与操作的转发与处理
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-12 创建
*
*/

namespace Ballance2.Sys.Services
{
    /// <summary>
    /// 游戏中介者
    /// </summary>
    [CustomLuaClass]
    [Serializable]
    [LuaApiDescription("游戏中介者")]
    public class GameMediator : GameService
    {
        private readonly string TAG = "GameMediator";
        
        [DoNotToLua]
        public GameMediator() : base("GameMediator")
        {

        }
        [DoNotToLua]
        public override void Destroy()
        {
            UnLoadAllEvents();
            UnLoadAllActions();
            DestroyStore();
        }
        [DoNotToLua]
        public override bool Initialize()
        {
            InitAllEvents();
            InitAllActions();
            InitStore();
            return true;
        }

        #region 全局事件控制器

        [SerializeField, SetProperty("Events")]
        private Dictionary<string, GameEvent> events = null;
        private Dictionary<string, GameHandler> singleEvents = null;

        public Dictionary<string, GameEvent> Events { get { return events; } }

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
            if (singleEvents.ContainsKey(evtName)) {
                singleEvents.Remove(evtName);
                return true;
            }
            else
            {
                Log.W(TAG, "单一事件 {0} 未注册", evtName);
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
            {
                Log.W(TAG, "事件 {0} 已注册", evtName);
                GameErrorChecker.LastError = GameError.AlreadyRegistered;
                return gameEvent;
            }

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
                Log.W(TAG, "事件 {0} 未注册", evtName);
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
        /// 通知单一事件
        /// </summary>
        /// <param name="evtName">事件名称</param>
        /// <param name="pararms">事件参数</param>
        /// <returns>返回是否成功</returns>
        [LuaApiDescription("通知单一事件", "返回是否成功")]
        [LuaApiParamDescription("evtName", "事件名称")]
        [LuaApiParamDescription("pararms", "事件参数")]
        public bool NotifySingleEvent(string evtName, params object[] pararms) {
            if (string.IsNullOrEmpty(evtName))
            {
                Log.W(TAG, "NotifySingleEvent evtName 参数未提供");
                GameErrorChecker.LastError = GameError.ParamNotProvide;
                return false;
            }
            if (singleEvents.TryGetValue(evtName, out GameHandler handler)) {
                if(handler != null)
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
        /// 执行事件分发
        /// </summary>
        /// <param name="gameEvent">事件实例</param>
        /// <param name="handlerFilter">指定事件可以接收到的名字（这里可以用正则）</param>
        /// <param name="pararms">事件参数</param>
        /// <returns>返回已经发送的接收器个数</returns>
        [LuaApiDescription("执行事件分发", "返回已经发送的接收器个数")]
        [LuaApiParamDescription("gameEvent", "事件实例")]
        [LuaApiParamDescription("handlerFilter", "指定事件可以接收到的名字（这里可以用正则）")]
        [LuaApiParamDescription("pararms", "事件参数")]
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
        [LuaApiDescription("执行事件分发", "返回已经发送的接收器个数")]
        [LuaApiParamDescription("evtName", "事件名称")]
        [LuaApiParamDescription("handlerFilter", "指定事件可以接收到的名字（这里可以用正则）")]
        [LuaApiParamDescription("pararms", "事件参数")]
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
            {
                Log.W(TAG, "事件 {0} 未注册", evtName);
                GameErrorChecker.LastError = GameError.NotRegister;
            }
            return handledCount;
        }

        //卸载所有事件
        private void UnLoadAllEvents()
        {
            if (events != null)
            {
                foreach (var gameEvent in events)
                    gameEvent.Value.Dispose();
                events.Clear();
                events = null;
            }
        }
        private void InitAllEvents()
        {
            events = new Dictionary<string, GameEvent>();
            singleEvents = new Dictionary<string, GameHandler>();

            //注册内置事件
            RegisterGlobalEvent(GameEventNames.EVENT_BASE_INIT_FINISHED);
            RegisterGlobalEvent(GameEventNames.EVENT_BEFORE_GAME_QUIT);
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
            if(oldHandler != null) {
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
                Log.W(TAG, "单一事件 {0} 未注册", evtName);
                GameErrorChecker.LastError = GameError.NotRegister;
                return false;
            } 
            var oldHandler = singleEvents[evtName];
            if (oldHandler != gameHandler) {
                Log.W(TAG, "单一事件 {0} 已由 {1} 订阅，必须为当前订阅者才能取消", evtName, oldHandler.Name);
                GameErrorChecker.LastError = GameError.AccessDenined;
                return false;
            }

            singleEvents[evtName] = null;
            return true;
        }

        /// <summary>
        /// 注册全局事件接收器（Lua）
        /// </summary>
        /// <param name="evtName">事件名称</param>
        /// <param name="name">接收器名字</param>
        /// <param name="gameHandlerDelegate">回调</param>
        /// <returns>返回注册的处理器，可使用这个处理器取消注册对应事件</returns>
        [LuaApiDescription("注册全局事件接收器（Lua）", "返回注册的处理器，可使用这个处理器取消注册对应事件")]
        [LuaApiParamDescription("package", "所属包")]
        [LuaApiParamDescription("evtName", "事件名称")]
        [LuaApiParamDescription("name", "接收器名字")]
        [LuaApiParamDescription("luaFunction", "回调")]
        [LuaApiParamDescription("luaSelf", "lua self")]
        public GameHandler RegisterEventHandler(GamePackage package, string evtName, string name, LuaFunction luaFunction, LuaTable luaSelf)
        {
            if (string.IsNullOrEmpty(evtName)
               || string.IsNullOrEmpty(name)
               || luaFunction == null)
            {
                Log.W(TAG, "参数缺失", evtName);
                GameErrorChecker.LastError = GameError.ParamNotProvide;
                return null;
            }

            if (IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
            {
                GameHandler gameHandler = GameHandler.CreateLuaHandler(package, name, luaFunction, luaSelf);
                gameEvent.EventHandlers.Add(gameHandler);
                return gameHandler;
            }
            else
            {
                Log.W(TAG, "事件 {0} 未注册", evtName);
                GameErrorChecker.LastError = GameError.NotRegister;
            }
            return null;
        }
        /// <summary>
        /// 注册全局事件接收器（Delegate）
        /// </summary>
        /// <param name="evtName">事件名称</param>
        /// <param name="name">接收器名字</param>
        /// <param name="gameHandlerDelegate">回调</param>
        /// <returns>返回注册的处理器，可使用这个处理器取消注册对应事件</returns>
        [LuaApiDescription("注册全局事件接收器（Delegate）", "返回注册的处理器，可使用这个处理器取消注册对应事件")]
        [LuaApiParamDescription("package", "所属包")]
        [LuaApiParamDescription("evtName", "事件名称")]
        [LuaApiParamDescription("name", "接收器名字")]
        [LuaApiParamDescription("gameHandlerDelegate", "回调")]
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

            if (IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
            {
                GameHandler gameHandler = GameHandler.CreateCsEventHandler(package, name, gameHandlerDelegate);
                gameEvent.EventHandlers.Add(gameHandler);
                return gameHandler;
            }
            else
            {
                Log.W(TAG, "事件 {0} 未注册", evtName);
                GameErrorChecker.LastError = GameError.NotRegister;
            }
            return null;
        }
        /// <summary>
        /// 注册全局事件接收器
        /// </summary>
        /// <param name="evtName">事件名称</param>
        /// <param name="name">接收器名字</param>
        /// <param name="luaModulHandler">模块接收器函数标识符</param>
        /// <returns>返回注册的处理器，可使用这个处理器取消注册对应事件</returns>
        [LuaApiDescription("游戏中注册全局事件接收器介者", "返回注册的处理器，可使用这个处理器取消注册对应事件")]
        [LuaApiParamDescription("package", "所属包")]
        [LuaApiParamDescription("evtName", "事件名称")]
        [LuaApiParamDescription("name", "接收器名字")]
        [LuaApiParamDescription("luaModulHandler", "模块接收器函数标识符")]
        public GameHandler RegisterEventHandler(GamePackage package, string evtName, string name, string luaModulHandler)
        {
            if (string.IsNullOrEmpty(evtName)
                || string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(luaModulHandler))
            {
                Log.W(TAG, "参数缺失", evtName);
                GameErrorChecker.LastError = GameError.ParamNotProvide;
                return null;
            }

            if (IsGlobalEventRegistered(evtName, out GameEvent gameEvent))
            {
                GameHandler gameHandler = GameHandler.CreateLuaStaticHandler(package, name, luaModulHandler);
                gameEvent.EventHandlers.Add(gameHandler);
                return gameHandler;
            }
            else
            {
                Log.W(TAG, "事件 {0} 未注册", evtName);
                GameErrorChecker.LastError = GameError.NotRegister;
            }
            return null;
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
                Log.W(TAG, "事件 {0} 未注册", evtName);
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
        [LuaApiDescription("注册全局共享数据存储池", "如果注册成功，返回池对象；如果已经注册，则返回已经注册的池对象")]
        [LuaApiParamDescription("package", "所属包")]
        [LuaApiParamDescription("name", "池名称")]
        public GameActionStore RegisterActionStore(GamePackage package, string name)
        {
            string keyName = package.PackageName + ":" + name;
            GameActionStore store;
            if (string.IsNullOrEmpty(name))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG,
                    "RegisterGlobalDataStore name 参数未提供");
                return null;
            }
            if (actionStores.ContainsKey(keyName))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG,
                    "共享操作仓库 {0} 已经注册", keyName);
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
        [LuaApiDescription("获取全局共享数据存储池")]
        [LuaApiParamDescription("package", "所属包")]
        [LuaApiParamDescription("name", "池名称")]
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
        [LuaApiDescription("释放已注册的全局共享数据存储池")]
        [LuaApiParamDescription("package", "所属包")]
        [LuaApiParamDescription("name", "池名称")]
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
        [LuaApiDescription("释放已注册的全局共享数据存储池")]
        [LuaApiParamDescription("name", "池名称")]
        public bool UnRegisterActionStore(GameActionStore store)
        {
            if (!actionStores.ContainsKey(store.KeyName))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG,
                    "actionStores {0} 未注册", store.KeyName);
                return false;
            }

            actionStores[store.KeyName].Destroy();
            globalStore.Remove(store.KeyName);
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
        [LuaApiDescription("调用操作")]
        [LuaApiParamDescription("package", "所属包")]
        [LuaApiParamDescription("storeName", "操作仓库名称")]
        [LuaApiParamDescription("name", "服务名称")]
        [LuaApiParamDescription("param", "调用参数")]
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
        [LuaApiDescription("调用操作")]
        [LuaApiParamDescription("name", "服务名称")]
        [LuaApiParamDescription("store", "操作仓库")]
        [LuaApiParamDescription("param", "调用参数")]
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
        [LuaApiDescription("调用操作")]
        [LuaApiParamDescription("action", "操作实体")]
        [LuaApiParamDescription("param", "调用参数")]
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

            param = LuaUtils.LuaTableArrayToObjectArray(param);

            //Log.Log(TAG, "CallAction {0} -> {1}", action.Name, StringUtils.ValueArrayToString(param));

            result = action.GameHandler.CallActionHandler(param);
            if (!result.Success)
                Log.W(TAG, "操作 {0} 执行失败 {1}", action.Name, GameErrorChecker.LastError);

            return result;
        }

        /// <summary>
        /// 内核的 ActinoStore
        /// </summary>
        [LuaApiDescription("内核的 ActinoStore")]
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

        #region 全局共享数据共享池

        [SerializeField, SetProperty("GlobalStore")]
        private Dictionary<string, Store> globalStore;

        public Dictionary<string, Store> GlobalStore { get { return globalStore; } }

        private void InitStore()
        {
            globalStore = new Dictionary<string, Store>();
        }
        private void DestroyStore()
        {
            foreach (var v in globalStore)
                v.Value.Destroy();
            globalStore.Clear();
            globalStore = null;
        }

        /// <summary>
        /// 注册全局共享数据存储池
        /// </summary>
        /// <param name="name">池名称</param>
        /// <returns>如果注册成功，返回池对象；如果已经注册，则返回已经注册的池对象</returns>
        [LuaApiDescription("注册全局共享数据存储池", "如果注册成功，返回池对象；如果已经注册，则返回已经注册的池对象")]
        [LuaApiParamDescription("name", "池名称")]
        public Store RegisterGlobalDataStore(string name)
        {
            Store store;
            if (string.IsNullOrEmpty(name))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG,
                    "RegisterGlobalDataStore name 参数未提供");
                return null;
            }
            if (globalStore.ContainsKey(name))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG,
                    "数据共享存储池 {0} 已经注册", name);
                store = globalStore[name];
                return store;
            }

            store = new Store(name);
            globalStore.Add(name, store);
            return store;
        }
        /// <summary>
        /// 获取全局共享数据存储池
        /// </summary>
        /// <param name="name">池名称</param>
        /// <returns></returns>
        [LuaApiDescription("获取全局共享数据存储池")]
        [LuaApiParamDescription("name", "池名称")]
        public Store GetGlobalDataStore(string name)
        {
            globalStore.TryGetValue(name, out Store s);
            return s;
        }
        /// <summary>
        /// 释放已注册的全局共享数据存储池
        /// </summary>
        /// <param name="name">池名称</param>
        /// <returns></returns>
        [LuaApiDescription("释放已注册的全局共享数据存储池")]
        [LuaApiParamDescription("name", "池名称")]
        public bool UnRegisterGlobalDataStore(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG,
                    "UnRegisterGlobalDataStore name 参数未提供");
                return false;
            }

            if (!globalStore.ContainsKey(name))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG,
                    "数据共享存储池 {0} 未注册", name);
                return false;
            }

            globalStore[name].Destroy();
            globalStore.Remove(name);

            return false;
        }
        /// <summary>
        /// 释放已注册的全局共享数据存储池
        /// </summary>
        /// <param name="name">池名称</param>
        /// <returns></returns>
        [LuaApiDescription("释放已注册的全局共享数据存储池")]
        [LuaApiParamDescription("name", "池名称")]
        public bool UnRegisterGlobalDataStore(Store store)
        {
            if (!globalStore.ContainsKey(store.PoolName))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG,
                    "数据共享存储池 {0} 未注册", store.PoolName);
                return false;
            }

            globalStore.Remove(store.PoolName);
            store.Destroy();

            return false;
        }

        #endregion

    }
}
