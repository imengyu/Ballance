using System;
using Ballance2.Base.Handler;
using Ballance2.Package;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameAction.cs
 * 
 * 用途：
 * 全局操作类定义
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Base
{
  /// <summary>
  /// 全局操作
  /// </summary>
  [Serializable]
  public class GameAction
  {
    /// <summary>
    /// 创建全局操作
    /// </summary>
    /// <param name="name">操作名称</param>
    /// <param name="gameHandler">操作接收器</param>
    /// <param name="callTypeCheck">操作调用参数检查</param>
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
    public GameActionStore Store { get; private set; }
    /// <summary>
    /// 所属模块
    /// </summary>
    public GamePackage Package { get; private set; }
    /// <summary>
    /// 操作名称
    /// </summary>
    public string Name { get { return _Name; } }
    /// <summary>
    /// 操作接收器
    /// </summary>
    public GameHandler GameHandler { get { return _GameHandler; } }
    /// <summary>
    /// 操作类型检查
    /// </summary>
    public string[] CallTypeCheck { get { return _CallTypeCheck; } }

    /// <summary>
    /// 空操作
    /// </summary>
    public static GameAction Empty { get; } = new GameAction(null, GamePackage.GetSystemPackage(), "internal.empty", null, null);

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
  public class GameActionCallResult
  {
    private GameActionCallResult() { }

    /// <summary>
    /// 创建操作调用结果
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="returnParams">返回的数据</param>
    /// <returns>操作调用结果</returns>
    public static GameActionCallResult CreateActionCallResult(bool success, object[] returnParams = null)
    {
      return new GameActionCallResult(success, returnParams);
    }
    /// <summary>
    /// 创建操作调用结果
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="returnParams">返回的数据</param>
    public GameActionCallResult(bool success, object[] returnParams)
    {
      Success = success;
      ReturnParams = returnParams;
    }

    /// <summary>
    /// 获取是否成功
    /// </summary>
    public bool Success { get; private set; }
    /// <summary>
    /// 获取操作返回的数据
    /// </summary>
    public object[] ReturnParams { get; private set; }

    /// <summary>
    /// 预制成功的无其他参数的调用返回结果
    /// </summary>
    public static GameActionCallResult SuccessResult = new GameActionCallResult(true, null);
    /// <summary>
    /// 预制失败的无其他参数的调用返回结果
    /// </summary>
    public static GameActionCallResult FailResult = new GameActionCallResult(false, null);
  }

  /// <summary>
  /// 操作接收器内核回调
  /// </summary>
  /// <param name="pararms">参数</param>
  /// <returns>返回事件数据</returns>
  public delegate GameActionCallResult GameActionHandlerDelegate(params object[] pararms);
}
