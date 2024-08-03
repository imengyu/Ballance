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
  public class GameTimeMachine : GameService<GameTimeMachine>
  {
    private const string TAG = "GameTimeMachine";

    public GameTimeMachine() : base(TAG) {}

    public override bool Initialize()
    {
      base.Initialize();
      return true;
    }
    
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
    public class GameTimeMachineTimeTicket {

      private GameTimeMachine service = null;
      private int type;
      internal int sleepTick = 0;
      internal int errTickCount = 0;

      public GameTimeMachineTimeTicket(GameTimeMachine service, Action updateAction, int order, int interval, int type) {
        this.service = service;
        this.type = type;
        this.updateAction = updateAction;
        this.order = order;
        this.interval = interval;
      }
      
      /// <summary>
      /// 获取当前实例的更新函数。
      /// </summary>
      public Action updateAction { get; }
      /// <summary>
      /// 获取当前更新函数的更新顺序。顺序越小，越先被调用。
      /// </summary>
      public int order { get; }
      /// <summary>
      /// 获取或者设置当前更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。
      /// </summary>     
      public int interval;
      /// <summary>
      /// 获取指定当前更新实例是否启用。可以动态改变此值。
      /// </summary>     
      public bool enable { get; private set; } = true;

      /// <summary>
      /// 停用当前更新函数。
      /// </summary>      
      public void Disable() {
        enable = false;
      }
      /// <summary>
      /// 重新启用当前更新函数。
      /// </summary>      
      public void Enable() {
        errTickCount = 0;
        enable = true;
      }   
      /// <summary>
      /// 取消注册当前更新函数。
      /// </summary>      
      public void Unregister() {
        switch(type) {
          case 1: 
            service.updates.Remove(this);
            break;
          case 2: 
            service.lateUpdates.Remove(this);
            break;
          case 3: 
            service.fixUpdates.Remove(this);
            break;
        }
      }
    }

    private LinkedList<GameTimeMachineTimeTicket> updates = new LinkedList<GameTimeMachineTimeTicket>();
    private LinkedList<GameTimeMachineTimeTicket> lateUpdates = new LinkedList<GameTimeMachineTimeTicket>();
    private LinkedList<GameTimeMachineTimeTicket> fixUpdates = new LinkedList<GameTimeMachineTimeTicket>();

    private void InsertTimeMachineTimeTicketToList(GameTimeMachineTimeTicket ticket, LinkedList<GameTimeMachineTimeTicket> list) {
      //根据order直接插入到指定位置
      LinkedListNode<GameTimeMachineTimeTicket> item = list.First;
      while(item != null) {
        if(item.Value.order >= ticket.order) {
          list.AddBefore(item, ticket);
          return;
        }
        item = item.Next;
      }
      //末尾
      list.AddFirst(ticket);
    } 
    private void RemoveActionInList(Action updateAction, LinkedList<GameTimeMachineTimeTicket> list) {
      LinkedListNode<GameTimeMachineTimeTicket> item = list.First;
      while(item != null) {
        if(item.Value.updateAction == updateAction)
          list.Remove(item);
        item = item.Next;
      }
    }
    private void DoFlushUpdateList(string name, LinkedList<GameTimeMachineTimeTicket> list) {
      GameTimeMachineTimeTicket current = null;
      LinkedListNode<GameTimeMachineTimeTicket> item = list.First;
      while(item != null) {
        current = item.Value;
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
              Log.W(TAG, name + " TimeMachine encountered an exception more than {0} times in {1} order: {2} interval: {3}. exception: {4}", current.errTickCount, current.updateAction.ToString(), current.order, current.interval, e.ToString());
            else {
              current.Disable();
              Log.E(TAG, name + " TimeMachine encountered an exception more than 5 times in {0} order: {1} interval: {2}, and it was disabled. exception: {3}", current.updateAction.ToString(), current.order, current.interval, e.ToString());
            }
          }
        }
        item = item.Next;
      }
    }

    /// <summary>
    /// 注册 Update 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    /// <param name="order">函数的更新顺序。顺序越小，越先被调用。</param>
    /// <param name="interval">更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。</param>
    /// <returns>返回一个更新实例，使用此实例可以取消注册更新函数。</returns>
    public GameTimeMachineTimeTicket RegisterUpdate(Action updateAction, int order = 0, int interval = 0) {
      var ticket = new GameTimeMachineTimeTicket(this, updateAction, order, interval, 1);
      InsertTimeMachineTimeTicketToList(ticket, updates);
      return ticket;
    }
    /// <summary>
    /// 取消注册 Update 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
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
    public GameTimeMachineTimeTicket RegisterLateUpdate(Action updateAction, int order = 0, int interval = 0) {
      var ticket = new GameTimeMachineTimeTicket(this, updateAction, order, interval, 2);
      InsertTimeMachineTimeTicketToList(ticket, lateUpdates);
      return ticket;
    }
    /// <summary>
    /// 取消注册 LateUpdate 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
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
    public GameTimeMachineTimeTicket RegisterFixedUpdate(Action updateAction, int order = 0, int interval = 0) {
      var ticket = new GameTimeMachineTimeTicket(this, updateAction, order, interval, 3);
      InsertTimeMachineTimeTicketToList(ticket, fixUpdates);
      return ticket;
    }
    /// <summary>
    /// 取消注册 FixedUpdate 更新函数
    /// </summary>
    /// <param name="updateAction">更新函数。</param>
    public void UnRegisterFixedUpdate(Action updateAction) {
      RemoveActionInList(updateAction, fixUpdates);
    }

    public static int FixedUpdateTick = 0;
    public static int UpdateTick = 0;

    protected override void Update() {
      if(UpdateTick < 1024) UpdateTick++;
      else UpdateTick = 0;

      DoFlushUpdateList("Update", updates);
    }
    private void LateUpdate() {
      DoFlushUpdateList("LateUpdate", lateUpdates);
    }
    protected override void FixedUpdate() {
      if(FixedUpdateTick < 1024) FixedUpdateTick++;
      else FixedUpdateTick = 0;

      DoFlushUpdateList("FixedUpdate", fixUpdates);
    }

  }
}