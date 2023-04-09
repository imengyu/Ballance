/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameEventHandler.cs
 *
 * 用途：
 * 事件接收器实例，用于取消注册事件接收器。
 * 
 * 作者：
 * mengyu
 */
using Ballance2.Package;
using static Ballance2.Services.GameManager;

namespace Ballance2.Base
{
  /// <summary>
  /// 事件接收器实例
  /// </summary>
  public class GameEventHandler {

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

    /// <summary>
    /// 取消注册当前事件接收器
    /// </summary>
    public void UnRegister() {
      if (unregisterCallback != null) {
        unregisterCallback(this);
        Destroyed = true;
        unregisterCallback = null;
      }
    }

    private UnregisterCallbackDelegate unregisterCallback = null;
    public delegate void UnregisterCallbackDelegate(GameEventHandler evt);
    internal GameEventHandler(GamePackage package, UnregisterCallbackDelegate unregisterCallback, string name, GameEventHandlerDelegate gameHandlerDelegate) {
      this.BelongPackage = package;
      this.Name = name;
      this.unregisterCallback = unregisterCallback;
      this.eventHandlerDelegate = gameHandlerDelegate;
    }

    /// <summary>
    /// 手动调用回调
    /// </summary>
    /// <param name="evtName">事件名称</param>
    /// <param name="pararms">参数</param>
    /// <returns></returns>
    public bool CallEventHandler(string evtName, params object[] pararms)
    {
      if (Destroyed)
        return false;
      return eventHandlerDelegate.Invoke(evtName, pararms);
    }

    private GameEventHandlerDelegate eventHandlerDelegate = null;
  }
}