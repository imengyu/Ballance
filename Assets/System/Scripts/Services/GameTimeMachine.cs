/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameTimeMachine.cs
* 
* 用途：
* 更新状态管理器。
* TimeMachine提供了另一种方式，实现类似MonoBehaviour中的Update之类的方法的功能。以便于我们在某些场合更方便的编写时间驱动的代码。
*
* 作者：
* mengyu
*/

using System;
using System.Collections.Generic;

namespace Ballance2.Services
{
  /// <summary>
  /// 时间更新状态管理器
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("更新状态管理器")]
  public class GameTimeMachine : GameService
  {
    private const string TAG = "GameTimeMachine";

    public GameTimeMachine() : base(TAG) {}

    [SLua.DoNotToLua]
    public override bool Initialize()
    {
      base.Initialize();
      return true;
    }
    [SLua.DoNotToLua]
    public override void Destroy()
    { 
      updates.Clear();
      lateUpdates.Clear();
      fixUpdates.Clear();
      base.Destroy();
    }

    /// <summary>
    /// 注册时的更新实例，使用此实例可以取消注册更新函数。
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("注册时的更新实例，使用此实例可以取消注册更新函数。")]
    public class GameTimeMachineTimeTicket {

      private GameTimeMachine service = null;
      internal int sleepTick = 0;
      internal int errTickCount = 0;

      public GameTimeMachineTimeTicket(GameTimeMachine service, Action updateAction, int order, int interval) {
        this.service = service;
        this.updateAction = updateAction;
        this.order = order;
        this.interval = interval;
      }
      
      /// <summary>
      /// 更新函数。
      /// </summary>
      public Action updateAction { get; }
      /// <summary>
      /// 当前更新函数的更新顺序。顺序越小，越先被调用。
      /// </summary>
      public int order { get; }
      /// <summary>
      /// 当前更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。
      /// </summary>
      public int interval;
      /// <summary>
      /// 指定当前更新实例是否启用。可以动态改变此值。
      /// </summary>
      public bool enable { get; private set; } = true;

      /// <summary>
      /// 停用当前更新函数。
      /// </summary>
      [LuaApiDescription("停用当前更新函数。")]
      public void Disable() {
        enable = false;
      }
      /// <summary>
      /// 重新启用当前更新函数。
      /// </summary>
      [LuaApiDescription("重新启用当前更新函数。")]
      public void Enable() {
        errTickCount = 0;
        enable = true;
      }   
      /// <summary>
      /// 取消注册当前更新函数。
      /// </summary>
      [LuaApiDescription("取消注册当前更新函数。")]
      public void Unregister() {
        int ind = service.updates.IndexOf(this);
        if(ind >= 0)
          service.updates.RemoveAt(ind);
      }
    }

    private List<GameTimeMachineTimeTicket> updates = new List<GameTimeMachineTimeTicket>();
    private List<GameTimeMachineTimeTicket> lateUpdates = new List<GameTimeMachineTimeTicket>();
    private List<GameTimeMachineTimeTicket> fixUpdates = new List<GameTimeMachineTimeTicket>();

    private void InsertTimeMachineTimeTicketToList(GameTimeMachineTimeTicket ticket, List<GameTimeMachineTimeTicket> list) {
      //根据order直接插入到指定位置
      GameTimeMachineTimeTicket current = null;
      for(int i = 0; i < list.Count; i++) {
        current = list[i];
        if(ticket.order < current.order) {
          list.Insert(i, ticket);
          return;
        }
      }
      //末尾
      list.Add(ticket);
    } 
    private void RemoveActionInList(Action updateAction, List<GameTimeMachineTimeTicket> list) {
      for(int i = list.Count - 1; i >= 0; i--) {
        if(list[i].updateAction == updateAction)
          list.RemoveAt(i);
      }
    }
    private void DoFlushUpdateList(string name, List<GameTimeMachineTimeTicket> list) {
      GameTimeMachineTimeTicket current = null;
      for(int i = 0; i < list.Count; i++) {
        current = list[i];
        if(current.enable) {
          //睡眠
          if(current.interval > 0) {
            if(current.sleepTick > 0) {
              current.sleepTick--;
              continue;
            }
            current.sleepTick = current.interval;
          }

          //执行
          try {
            current.updateAction.Invoke();
            if(current.errTickCount > 0)
              current.errTickCount --;
          } catch(Exception e) {
            current.errTickCount++;
            if(current.errTickCount < 5) 
              Log.W(TAG, name + " TimeMachine encountered an exception more than {0} times in {1} order: {2} interval: {3}. exception: {4}", current.errTickCount, i, current.order, current.interval, e.ToString());
            else {
              current.Disable();
              Log.E(TAG, name + " TimeMachine encountered an exception more than 5 times in {0} order: {1} interval: {2}, and it was disabled. exception: {3}", i, current.order, current.interval, e.ToString());
            }
          }
        }
      }
    }

    /// <summary>
    /// 注册 Update 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    /// <param name="order">函数的更新顺序。顺序越小，越先被调用。</param>
    /// <param name="interval">更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。</param>
    /// <returns>返回一个更新实例，使用此实例可以取消注册更新函数。</returns>
    [LuaApiDescription("注册 Update 更新函数", "返回一个更新实例，使用此实例可以取消注册更新函数。")]
    [LuaApiParamDescription("updateAction", "更新函数。")]
    [LuaApiParamDescription("order", "函数的更新顺序。顺序越小，越先被调用。")]
    [LuaApiParamDescription("interval", "更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。")]
    public GameTimeMachineTimeTicket RegisterUpdate(Action updateAction, int order = 0, int interval = 1) {
      var ticket = new GameTimeMachineTimeTicket(this, updateAction, order, interval);
      InsertTimeMachineTimeTicketToList(ticket, updates);
      return ticket;
    }
    /// <summary>
    /// 取消注册 Update 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    [LuaApiDescription("取消注册 Update 更新函数")]
    [LuaApiParamDescription("updateAction", "更新函数。")]
    public void UnRegisterUpdate(Action updateAction) {
      RemoveActionInList(updateAction, updates);
    }

    /// <summary>
    /// 注册 LateUpdate 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    /// <param name="order">函数的更新顺序。顺序越小，越先被调用。</param>
    /// <param name="interval">更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。</param>
    /// <returns>返回一个更新实例，使用此实例可以取消注册更新函数。</returns>
    [LuaApiDescription("注册 LateUpdate 更新函数", "返回一个更新实例，使用此实例可以取消注册更新函数。")]
    [LuaApiParamDescription("updateAction", "更新函数。")]
    [LuaApiParamDescription("order", "函数的更新顺序。顺序越小，越先被调用。")]
    [LuaApiParamDescription("interval", "更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。")]
    public GameTimeMachineTimeTicket RegisterLateUpdate(Action updateAction, int order = 0, int interval = 1) {
      var ticket = new GameTimeMachineTimeTicket(this, updateAction, order, interval);
      InsertTimeMachineTimeTicketToList(ticket, lateUpdates);
      return ticket;
    }
    /// <summary>
    /// 取消注册 LateUpdate 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    [LuaApiDescription("取消注册 LateUpdate 更新函数")]
    [LuaApiParamDescription("updateAction", "更新函数。")]
    public void UnRegisterLateUpdate(Action updateAction) {
      RemoveActionInList(updateAction, lateUpdates);
    }

    /// <summary>
    /// 注册 FixedUpdate 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    /// <param name="order">函数的更新顺序。顺序越小，越先被调用。</param>
    /// <param name="interval">更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。</param>
    /// <returns>返回一个更新实例，使用此实例可以取消注册更新函数。</returns>    
    [LuaApiDescription("注册 FixedUpdate 更新函数", "返回一个更新实例，使用此实例可以取消注册更新函数。")]
    [LuaApiParamDescription("updateAction", "更新函数。")]
    [LuaApiParamDescription("order", "函数的更新顺序。顺序越小，越先被调用。")]
    [LuaApiParamDescription("interval", "更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。")]
    public GameTimeMachineTimeTicket RegisterFixedUpdate(Action updateAction, int order = 0, int interval = 1) {
      var ticket = new GameTimeMachineTimeTicket(this, updateAction, order, interval);
      InsertTimeMachineTimeTicketToList(ticket, fixUpdates);
      return ticket;
    }
    /// <summary>
    /// 取消注册 FixedUpdate 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    [LuaApiDescription("取消注册 FixedUpdate 更新函数")]
    [LuaApiParamDescription("updateAction", "更新函数。")]
    public void UnRegisterFixedUpdate(Action updateAction) {
      RemoveActionInList(updateAction, fixUpdates);
    }

    protected override void Update() {
      DoFlushUpdateList("Update", updates);
    }
    private void LateUpdate() {
      DoFlushUpdateList("LateUpdate", lateUpdates);
    }
    private void FixedUpdate() {
      DoFlushUpdateList("FixedUpdate", fixUpdates);
    }

  }
}