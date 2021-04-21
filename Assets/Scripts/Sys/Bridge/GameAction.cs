using Ballance.LuaHelpers;
using Ballance2.Sys.Bridge.Handler;
using Ballance2.Sys.Package;
using Ballance2.Utils;
using System;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameAction.cs
 * 用途：
 * 全局操作类定义
 * 
 * 作者：
 * mengyu
 * 
 * 更改历史：
 * 2020-1-1 创建
 * 2021-1-17 imengyu 修改Package逻辑
 *
 */

namespace Ballance2.Sys.Bridge
{
    /// <summary>
    /// 全局操作
    /// </summary>
    [SLua.CustomLuaClass]
    [Serializable]
    [LuaApiDescription("全局操作")]
    public class GameAction
    {
        /// <summary>
        /// 创建全局操作
        /// </summary>
        /// <param name="name">操作名称</param>
        /// <param name="gameHandler">操作接收器</param>
        /// <param name="callTypeCheck">操作调用参数检查</param>
        [LuaApiDescription("创建全局操作")]
        [LuaApiParamDescription("name", "操作名称")]
        [LuaApiParamDescription("gameHandler", "操作接收器")]
        [LuaApiParamDescription("callTypeCheck", "操作调用参数检查")]
        public GameAction(GameActionStore store, GamePackage package, string name, GameHandler gameHandler, string[] callTypeCheck)
        {
            Store = store;
            Package = package;
            _Name = name;
            _GameHandler = gameHandler;
            _CallTypeCheck = callTypeCheck;
        }

        [SerializeField, SetProperty("Name")]
        private string _Name;
        [SerializeField, SetProperty("GameHandler")]
        private GameHandler _GameHandler;
        [SerializeField, SetProperty("CallTypeCheck")]
        private string[] _CallTypeCheck;

        /// <summary>
        /// 所属模块
        /// </summary>
        [LuaApiDescription("所属模块")]
        public GameActionStore Store { get; private set; }
        /// <summary>
        /// 所属模块
        /// </summary>
        [LuaApiDescription("所属模块")]
        public GamePackage Package { get; private set; }
        /// <summary>
        /// 操作名称
        /// </summary>
        [LuaApiDescription("操作名称")]
        public string Name { get { return _Name; } }
        /// <summary>
        /// 操作接收器
        /// </summary>
        [LuaApiDescription("操作接收器")]
        public GameHandler GameHandler { get { return _GameHandler; } }
        /// <summary>
        /// 操作类型检查
        /// </summary>
        [LuaApiDescription("操作类型检查")]
        public string[] CallTypeCheck { get { return _CallTypeCheck; } }

        /// <summary>
        /// 空操作
        /// </summary>
        [LuaApiDescription("空操作")]
        public static GameAction Empty { get; } = new GameAction(null, 
            GamePackage.GetSystemPackage(), 
            "internal.empty", null, null);

        [SLua.DoNotToLua]
        public void Dispose()
        {
            Store = null;
            Package = null;
            _CallTypeCheck = null;
            _GameHandler.Dispose();
            _GameHandler = null;
        }
    }

    /// <summary>
    /// 操作调用结果
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("操作调用结果")]
    public class GameActionCallResult
    {
        private GameActionCallResult() { }

        /// <summary>
        /// 创建操作调用结果
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="returnParams">返回的数据</param>
        /// <returns>操作调用结果</returns>
        [LuaApiDescription("创建操作调用结果")]
        [LuaApiParamDescription("success", "是否成功")]
        [LuaApiParamDescription("returnParams", "返回的数据")]
        public static GameActionCallResult CreateActionCallResult(bool success, object[] returnParams = null)
        {
            return new GameActionCallResult(success, returnParams);
        }
        /// <summary>
        /// 创建操作调用结果
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="returnParams">返回的数据</param>
        [LuaApiDescription("创建操作调用结果")]
        [LuaApiParamDescription("success", "是否成功")]
        [LuaApiParamDescription("returnParams", "返回的数据")]
        public GameActionCallResult(bool success, object[] returnParams)
        {
            Success = success;
            ReturnParams = LuaUtils.LuaTableArrayToObjectArray(returnParams);
        }

        /// <summary>
        /// 获取是否成功
        /// </summary>
        [LuaApiDescription("获取是否成功")]
        public bool Success { get; private set; }
        /// <summary>
        /// 获取操作返回的数据
        /// </summary>
        [LuaApiDescription("获取操作返回的数据")]
        public object[] ReturnParams { get; private set; }

        /// <summary>
        /// 预制成功的无其他参数的调用返回结果
        /// </summary>
        [LuaApiDescription("预制成功的无其他参数的调用返回结果")]
        public static GameActionCallResult SuccessResult = new GameActionCallResult(true, null);
        /// <summary>
        /// 预制失败的无其他参数的调用返回结果
        /// </summary>
        [LuaApiDescription("预制失败的无其他参数的调用返回结果")]
        public static GameActionCallResult FailResult = new GameActionCallResult(false, null);
    }
}
