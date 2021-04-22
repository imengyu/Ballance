using Ballance.LuaHelpers;
using Ballance2.Sys.Bridge.Handler;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameEvent.cs
 * 用途：
 * 游戏全局事件的说明与存储类。
 * 
 * 作者：
 * mengyu
 * 
 * 更改历史：
 * 2020-8-7 创建
 * 2021-1-16 修改事件 mengyu
 *
 */

namespace Ballance2.Sys.Bridge
{
    [SLua.CustomLuaClass]
    [Serializable]
    [LuaApiDescription("全局事件存储类")]
    /// <summary>
    /// 全局事件存储类
    /// </summary>
    public class GameEvent
    {
        /// <summary>
        /// 全局事件存储类的构造函数
        /// </summary>
        /// <param name="evtName">事件名称</param>
        [LuaApiDescription("全局事件存储类的构造函数")]
        [LuaApiParamDescription("evtName", "事件名称")]
        public GameEvent(string evtName)
        {
            _EventName = evtName; 
            _EventHandlers = new List<GameHandler>();
        }

        /// <summary>
        /// 释放
        /// </summary>
        [LuaApiDescription("释放")]
        public void Dispose()
        {
            _EventHandlers.Clear();
            _EventHandlers = null;
        }

        [SerializeField, SetProperty("EventName")]
        private string _EventName;
        [SerializeField, SetProperty("EventHandlers")]
        private List<GameHandler> _EventHandlers;

        /// <summary>
        /// 获取事件名称
        /// </summary>
        [LuaApiDescription("获取事件名称")]
        public string EventName { get { return _EventName; } }
        /// <summary>
        /// 获取事件接收器
        /// </summary>
        [LuaApiDescription("获取事件接收器")]
        public List<GameHandler> EventHandlers { get { return _EventHandlers; } }
    }
 
    /// <summary>
    /// 游戏内部事件说明
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("游戏内部事件说明")]
    public static class GameEventNames
    {
        /// <summary>
        /// 全局（基础管理器）全部初始化完成时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        [LuaApiDescription("全局（基础管理器）全部初始化完成时触发该事件")]
        public const string EVENT_BASE_INIT_FINISHED = "e:base_init_finished";

        /// <summary>
        /// GameManager初始化完成时触发该事件，在这个事件后子模块接管控制流程，游戏主逻辑开始运行
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        [LuaApiDescription("GameManager初始化完成时触发该事件，在这个事件后子模块接管控制流程，游戏主逻辑开始运行")]
        public const string EVENT_GAME_MANAGER_INIT_FINISHED = "e:gamemgr_init_finished";

        /// <summary>
        /// 全局（UI管理器）全部初始化完成时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        [LuaApiDescription("全局（UI管理器）全部初始化完成时触发该事件")]
        public const string EVENT_UI_MANAGER_INIT_FINISHED = "e:ui_manager_init_finished";

        /// <summary>
        /// 全局对话框（Alert，Confirm）关闭时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对话框ID
        /// 【1】用户是否选择了 Confirm（对于 Alert 永远是false）
        /// </remarks>
        [LuaApiDescription("全局对话框（Alert，Confirm）关闭时触发该事件")]
        public const string EVENT_GLOBAL_ALERT_CLOSE = "e:ui:global_alert_close";

        /// <summary>
        /// 游戏即将退出时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        [LuaApiDescription("游戏即将退出时触发该事件")]
        public const string EVENT_BEFORE_GAME_QUIT = "e:before_game_quit";

        /// <summary>
        /// 模块加载成功事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模块包名
        /// 【1】对应模块对象
        /// </remarks>
        [LuaApiDescription("模块加载成功事件")]
        public const string EVENT_PACKAGE_LOAD_SUCCESS = "e:package:package_load_success";

        /// <summary>
        /// 模块加载失败事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模块包名
        /// 【1】对应模块对象
        /// 【2】错误信息
        /// </remarks>
        [LuaApiDescription("模块加载失败事件")]
        public const string EVENT_PACKAGE_LOAD_FAILED = "e:package:package_load_failed";

        /// <summary>
        /// 模块注册事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模块包名
        /// </remarks>
        [LuaApiDescription("模块注册事件")]
        public const string EVENT_PACKAGE_REGISTERED = "e:package:package_registered";
        /// <summary>
        /// 模块已注销事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模块包名
        /// </remarks>
        [LuaApiDescription("模块已注销事件")]
        public const string EVENT_PACKAGE_UNREGISTERED = "e:package:package_unregistered";

        /// <summary>
        /// 模块卸载事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模块包名
        /// 【1】对应模块对象
        /// </remarks>
        [LuaApiDescription("模块卸载事件")]
        public const string EVENT_PACKAGE_UNLOAD = "e:package:package_unload";

        /// <summary>
        /// 屏幕分辨率更改事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】新的屏幕宽度
        /// 【1】新的屏幕高度
        /// 【2】新的屏幕刷新率
        /// </remarks>
        [LuaApiDescription("屏幕分辨率更改事件")]
        public const string EVENT_SCREEN_SIZE_CHANGED = "e:screen:size_changed";
    }
}
