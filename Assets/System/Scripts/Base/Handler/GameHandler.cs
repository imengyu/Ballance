using Ballance2.Package;
using Ballance2.Services.Debug;

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

namespace Ballance2.Base.Handler
{
  /// <summary>
  /// 游戏通用接收器
  /// </summary>
  [SLua.CustomLuaClass]
  public class GameHandler
  {
    private static readonly string TAG = "GameHandler";

    protected GameHandler() { }

    /// <summary>
    /// 释放
    /// </summary>
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
    public virtual bool CallEventHandler(string evtName, params object[] pararms)
    {
      return false;
    }
    /// <summary>
    /// 调用操作接收器
    /// </summary>
    /// <param name="pararms">参数</param>
    /// <returns>返回是否中断剩余事件分发/返回Action是否成功</returns>
    public virtual GameActionCallResult CallActionHandler(params object[] pararms)
    {
      return null;
    }

    /// <summary>
    /// 所属模块
    /// </summary>
    public GamePackage BelongPackage { get; private set; }
    /// <summary>
    /// 接收器名称
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 获取是否被释放
    /// </summary>
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
