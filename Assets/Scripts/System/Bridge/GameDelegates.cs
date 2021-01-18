using SLua;
using System.Xml;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameDelegates.cs
 * 用途：
 * 提供一些委托定义，用于向lua导出
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
    [CustomLuaClass]
    public delegate void VoidDelegate();
    [CustomLuaClass]
    public delegate bool BooleanDelegate();

    /// <summary>
    /// 自定义接收器内核回调
    /// </summary>
    /// <param name="pararms">参数</param>
    /// <returns>返回自定义对象</returns>
    [CustomLuaClass]
    public delegate object GameCustomHandlerDelegate(params object[] pararms);
    /// <summary>
    /// 事件接收器内核回调
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="pararms">参数</param>
    /// <returns>返回是否中断其他事件的分发</returns>
    [CustomLuaClass]
    public delegate bool GameEventHandlerDelegate(string evtName, params object[] pararms);
    /// <summary>
    /// 操作接收器内核回调
    /// </summary>
    /// <param name="pararms">参数</param>
    /// <returns>返回事件数据</returns>
    [CustomLuaClass]
    public delegate GameActionCallResult GameActionHandlerDelegate(params object[] pararms);
}
