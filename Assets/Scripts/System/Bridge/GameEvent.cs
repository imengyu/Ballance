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
 * 2020-1-1 创建
 *
 */

namespace Ballance2.System.Bridge
{
    [SLua.CustomLuaClass]
    [Serializable]
    /// <summary>
    /// 全局事件存储类
    /// </summary>
    public class GameEvent
    {
        /// <summary>
        /// 全局事件存储类的构造函数
        /// </summary>
        /// <param name="evtName">事件名称</param>
        public GameEvent(string evtName)
        {
            _EventName = evtName; 
            _EventHandlers = new List<GameHandler>();
        }

        /// <summary>
        /// 释放
        /// </summary>
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
        public string EventName { get { return _EventName; } }
        /// <summary>
        /// 获取事件接收器
        /// </summary>
        public List<GameHandler> EventHandlers { get { return _EventHandlers; } }
    }

    [SLua.CustomLuaClass]
    /// <summary>
    /// 游戏内部事件说明
    /// </summary>
    public static class GameEventNames
    {
        /// <summary>
        /// 全局（基础管理器）全部初始化完成时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        public const string EVENT_BASE_INIT_FINISHED = "e:base_init_finished";

        /// <summary>
        /// 基础管理器初始化完成时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】管理器名称
        /// 【1】管理器二级名称
        /// </remarks>
        public const string EVENT_BASE_MANAGER_INIT_FINISHED = "e:base_manager_init_finished";

        /// <summary>
        /// 全局对话框（Alert，Confirm）关闭时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对话框ID
        /// 【1】用户是否选择了 Confirm（对于 Alert 永远是false）
        /// </remarks>
        public const string EVENT_GLOBAL_ALERT_CLOSE = "e:ui:global_alert_close";

        /// <summary>
        /// 游戏即将退出时触发该事件
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        public const string EVENT_BEFORE_GAME_QUIT = "e:before_game_quit";

        /// <summary>
        /// 游戏底层加载完成，现在开始加载游戏内核（GameInit入口）。
        /// 不推荐在这里接管加载序列（GameInit需要加载游戏必须的一些模块）。
        /// 你可以通过 EVENT_GAME_INIT_TAKE_OVER_CONTROL 来接管游戏加载序列
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        public const string EVENT_GAME_INIT_ENTRY = "e:base_game_init";

        /// <summary>
        /// 游戏全部加载完成，监听此事件可以中断默认的游戏加载序列。
        /// 你可以跳过监听此事件来接管游戏底层加载。
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// [0] LuaVoidDelegate ：如果接管后希望跳回原加载序列，可调用此回调
        /// </remarks>
        public const string EVENT_GAME_INIT_TAKE_OVER_CONTROL = "e:base_game_init_take_over_control";

        /// <summary>
        /// 当离开场景时发生事件。
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// [0] string ：将要离开的场景
        /// </remarks>
        public const string EVENT_BEFORE_LEAVE_SCENSE = "e:BEFORE_LEAVE_SCENSE";

        /// <summary>
        /// 当进入一个场景时发生事件。
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// [0] string ：将要进入的场景
        /// </remarks>
        public const string EVENT_ENTER_SCENSE = "e:EVENT_ENTER_SCENSE";

        /// <summary>
        /// gameinit 完成
        /// </summary>
        /// <remarks>
        /// 事件参数：无
        /// </remarks>
        public const string EVENT_ENTER_MENULEVEL = "e:init:enter_menulevel";

        /// <summary>
        /// 模组加载成功
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模组包名
        /// 【1】对应模组对象
        /// </remarks>
        public const string EVENT_MOD_LOAD_SUCCESS = "e:mod:mod_load_success";

        /// <summary>
        /// 模组加载成功
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模组包名
        /// 【1】对应模组对象
        /// 【2】错误信息
        /// </remarks>
        public const string EVENT_MOD_LOAD_FAILED = "e:mod:mod_load_failed";

        /// <summary>
        /// 模组注册
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模组包名
        /// 【1】对应模组对象
        /// </remarks>
        public const string EVENT_MOD_REGISTERED = "e:mod:mod_registered";

        /// <summary>
        /// 模组卸载
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】对应模组包名
        /// 【1】对应模组对象
        /// </remarks>
        public const string EVENT_MOD_UNLOAD = "e:mod:mod_unload";

        /// <summary>
        /// 屏幕分辨率变化
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】Vector2 屏幕大小
        /// </remarks>
        public const string EVENT_SCREEN_SIZE_CHANGED = "e:core:screen_size_changed";

        /// <summary>
        /// 进入关卡加载器
        /// 在此事件中可以对关卡加载器进行扩展注册
        /// </summary>
        /// <remarks>
        /// 事件参数：
        /// 【0】ILevelLoader 关卡加载器实例
        /// </remarks>
        public const string EVENT_ENTER_LEVEL_LOADER = "e:core:enter_level_loader";
    }
}
