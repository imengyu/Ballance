using Ballance2.LuaHelpers;
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
 */

namespace Ballance2.Sys.Bridge
{
    [CustomLuaClass]
    public delegate void VoidDelegate();
    [CustomLuaClass]
    public delegate bool BooleanDelegate();
    [CustomLuaClass]
    public delegate void GameObjectDelegate(GameObject go);

    /// <summary>
    /// 自定义接收器内核回调
    /// </summary>
    /// <param name="pararms">参数</param>
    /// <returns>返回自定义对象</returns>
    [CustomLuaClass]
    [LuaApiDescription("自定义接收器内核回调")]
    public delegate object GameCustomHandlerDelegate(params object[] pararms);
    /// <summary>
    /// 事件接收器内核回调
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="pararms">参数</param>
    /// <returns>返回是否中断其他事件的分发</returns>
    [CustomLuaClass]
    [LuaApiDescription("事件接收器内核回调")]
    public delegate bool GameEventHandlerDelegate(string evtName, params object[] pararms);
    /// <summary>
    /// 操作接收器内核回调
    /// </summary>
    /// <param name="pararms">参数</param>
    /// <returns>返回事件数据</returns>
    [CustomLuaClass]
    [LuaApiDescription("操作接收器内核回调")]
    public delegate GameActionCallResult GameActionHandlerDelegate(params object[] pararms);
    /// <summary>
    /// 调试命令回调
    /// </summary>
    /// <param name="keyword">命令单词</param>
    /// <param name="fullCmd">完整命令</param>
    /// <param name="args">命令参数</param>
    /// <returns>须返回命令是否执行成功</returns>
    [SLua.CustomLuaClass]
    [LuaApiDescription("调试命令回调")]
    public delegate bool CommandDelegate(string keyword, string fullCmd, int argsCount, string[] args);
    
}
