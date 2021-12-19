using Ballance2.Utils;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameCSharpHandler.cs
 * 用途：
 * C#事件或是回调接收器。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Base.Handler
{
  class GameCSharpHandler : GameHandler
  {
    public override GameActionCallResult CallActionHandler(params object[] pararms)
    {
      if (Destroyed)
        return null;
      if (actionHandlerDelegate != null)
      {
        return actionHandlerDelegate.Invoke(pararms);
      }
      return base.CallActionHandler(pararms);
    }
    public override bool CallEventHandler(string evtName, params object[] pararms)
    {
      if (Destroyed)
        return false;
      if (eventHandlerDelegate != null)
      {
        return eventHandlerDelegate.Invoke(evtName, pararms);
      }
      return base.CallEventHandler(evtName, pararms);
    }
    public GameCSharpHandler(GameActionHandlerDelegate actionHandlerDelegate)
    {
      this.actionHandlerDelegate = actionHandlerDelegate;
    }
    public GameCSharpHandler(GameEventHandlerDelegate eventHandlerDelegate)
    {
      this.eventHandlerDelegate = eventHandlerDelegate;
    }

    private GameActionHandlerDelegate actionHandlerDelegate = null;
    private GameEventHandlerDelegate eventHandlerDelegate = null;

    public override void Dispose()
    {
      actionHandlerDelegate = null;
      eventHandlerDelegate = null;
      base.Dispose();
    }
  }
}
