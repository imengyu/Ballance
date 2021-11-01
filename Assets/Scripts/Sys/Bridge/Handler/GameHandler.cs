using Ballance2.LuaHelpers;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Package;
using SLua;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameHandler.cs
 *
 * 用途：
 * 用于统一各个层的数据和事件接收。
 * 为C#和LUA都提供了一个包装类，可以方便的接收事件或是回调。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Sys.Bridge.Handler
{
    /// <summary>
    /// 游戏通用接收器
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("游戏通用接收器")]
    public class GameHandler
    {
        private static readonly string TAG = "GameHandler";

        protected GameHandler() { }

        /// <summary>
        /// 释放
        /// </summary>
        [DoNotToLua]
        public virtual void Dispose()
        {
            if (BelongPackage != null)
                BelongPackage.HandlerRemove(this);
            Destroyed = true;
        }

        /// <summary>
        /// 调用自定义接收器
        /// </summary>
        /// <param name="evtName">事件名称</param>
        /// <param name="pararms">参数</param>
        /// <returns>返回自定义对象</returns>
        [LuaApiDescription("调用自定义接收器", "返回自定义对象")]
        [LuaApiParamDescription("evtName", "事件名称")]
        [LuaApiParamDescription("pararms", "参数")]
        public virtual object CallCustomHandler(params object[] pararms)
        {
            return null;
        }
        /// <summary>
        /// 调用接收器
        /// </summary>
        /// <param name="evtName">事件名称</param>
        /// <param name="pararms">参数</param>
        /// <returns>返回是否中断剩余事件分发/返回Action是否成功</returns>
        [LuaApiDescription("调用接收器", "返回是否中断剩余事件分发/返回Action是否成功")]
        [LuaApiParamDescription("evtName", "事件名称")]
        [LuaApiParamDescription("pararms", "参数")]
        public virtual bool CallEventHandler(string evtName, params object[] pararms)
        {
            return false;
        }
        /// <summary>
        /// 调用操作接收器
        /// </summary>
        /// <param name="pararms">参数</param>
        /// <returns>返回是否中断剩余事件分发/返回Action是否成功</returns>
        [LuaApiDescription("调用操作接收器", "返回是否中断剩余事件分发/返回Action是否成功")]
        [LuaApiParamDescription("pararms", "参数")]
        public virtual GameActionCallResult CallActionHandler(params object[] pararms)
        {
            return null;
        }

        /// <summary>
        /// 所属模块
        /// </summary>
        [LuaApiDescription("所属模块")]
        public GamePackage BelongPackage { get; private set; }
        /// <summary>
        /// 接收器名称
        /// </summary>
        [LuaApiDescription("接收器名称")]
        public string Name { get; private set; }
        /// <summary>
        /// 获取是否被释放
        /// </summary>
        [LuaApiDescription("获取是否被释放")]
        public bool Destroyed { get; private set; } = false;

        //自定义接收器
        public static GameHandler CreateCsActionHandler(GamePackage gamePackage, string name, GameActionHandlerDelegate actionHandler)
        {
            return CreateHandlerInternal(gamePackage, name, new GameCSharpHandler(actionHandler));
        }
        public static GameHandler CreateCsEventHandler(GamePackage gamePackage, string name, GameEventHandlerDelegate eventHandler)
        {
            return CreateHandlerInternal(gamePackage, name, new GameCSharpHandler(eventHandler));
        }
        public static GameHandler CreateCsCustomHandler(GamePackage gamePackage, string name, GameCustomHandlerDelegate handler)
        {
            return CreateHandlerInternal(gamePackage, name, new GameCSharpHandler(handler));
        }

        /// <summary>
        /// 创建 Lua 通用接收器
        /// </summary>
        /// <param name="gamePackage">所在包</param>
        /// <param name="name">接收器名称</param>
        /// <param name="luaFunction">Lua函数</param>
        /// <param name="self">Lua self</param>
        /// <returns>返回创建的 GameHandler，如果创建失败，则返回null</returns>
        [LuaApiDescription("创建 Lua 通用接收器", "返回创建的 GameHandler，如果创建失败，则返回null")]
        [LuaApiParamDescription("gamePackage", "所在包")]
        [LuaApiParamDescription("name", "接收器名称")]
        [LuaApiParamDescription("luaFunction", "Lua函数")]
        [LuaApiParamDescription("self", "Lua self")]
        public static GameHandler CreateLuaHandler(GamePackage gamePackage, string name, LuaFunction luaFunction, LuaTable self)
        {
            return CreateHandlerInternal(gamePackage, name, new GameLuaHandler(luaFunction, self));
        }
        /// <summary>
        /// 创建 Lua 静态或 GameLuaObjectHost 接收器
        /// </summary>
        /// <param name="gamePackage">所在包</param>
        /// <param name="name">接收器名称</param>
        /// <param name="luaModulHandler">接收器标识符字符串</param>
        /// <remarks>
        /// 接收器标识符字符串:
        ///    [格式] 对象名称:函数名称[:附带参数]
        ///    
        ///     [对象名称]   已注册的 GameLuaObjectHost 名称  或  Main (模组主代码)
        ///     [函数名称]
        ///     [附带参数]   可选：要传给接收器的附带参数，参数将放在结尾
        /// </remarks>
        /// <returns>返回创建的 GameHandler，如果创建失败，则返回null</returns>
        [LuaApiDescription("创建 Lua 静态或 GameLuaObjectHost 接收器", "返回创建的 GameHandler，如果创建失败，则返回null")]
        [LuaApiParamDescription("gamePackage", "所在包")]
        [LuaApiParamDescription("name", "接收器名称")]
        [LuaApiParamDescription("luaModulHandler", "接收器标识符字符串")]
        public static GameHandler CreateLuaStaticHandler(GamePackage gamePackage, string name, string luaModulHandler)
        {
            return CreateHandlerInternal(gamePackage, name, new GameLuaStaticHandler(luaModulHandler));
        }
        
        //创建
        private static GameHandler CreateHandlerInternal(GamePackage gamePackage, string name, GameHandler handler)
        {
            if (gamePackage == null)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG, "gamePackage 参数必须提供");
                return null;
            }
            if (string.IsNullOrEmpty(name))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG, "name 参数必须提供");
                return null;
            }

            handler.BelongPackage = gamePackage;
            handler.Name = name;
            gamePackage.HandlerReg(handler);
            return handler;
        }
    }
}
