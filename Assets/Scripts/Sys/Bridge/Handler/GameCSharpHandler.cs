using Ballance2.Sys.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
 * 
 * 更改历史：
 * 2021-1-17 创建
 *
 */

namespace Ballance2.Sys.Bridge.Handler
{
    class GameCSharpHandler : GameHandler
    {
        public override GameActionCallResult CallActionHandler(params object[] pararms)
        {
            if (Destroyed)
                return null;
            if (actionHandlerDelegate != null)
                return actionHandlerDelegate.Invoke(pararms);
            return base.CallActionHandler(pararms);
        }
        public override bool CallEventHandler(string evtName, params object[] pararms)
        {
            if (Destroyed)
                return false;
            if (eventHandlerDelegate != null)
                return eventHandlerDelegate.Invoke(evtName, pararms);
            return base.CallEventHandler(evtName, pararms);
        }
        public override object CallCustomHandler(params object[] pararms)
        {
            if (Destroyed)
                return false;
            if (customHandlerDelegate != null)
                return customHandlerDelegate.Invoke(pararms);
            return base.CallCustomHandler(pararms);
        }

        public GameCSharpHandler(GameActionHandlerDelegate actionHandlerDelegate)
        {
            this.actionHandlerDelegate = actionHandlerDelegate;
        }
        public GameCSharpHandler(GameEventHandlerDelegate eventHandlerDelegate)
        {
            this.eventHandlerDelegate = eventHandlerDelegate;
        }
        public GameCSharpHandler(GameCustomHandlerDelegate customHandlerDelegate)
        {
            this.customHandlerDelegate = customHandlerDelegate;
        }

        private GameCustomHandlerDelegate customHandlerDelegate = null;
        private GameActionHandlerDelegate actionHandlerDelegate = null;
        private GameEventHandlerDelegate eventHandlerDelegate = null;

        public override void Dispose()
        {
            customHandlerDelegate = null;
            actionHandlerDelegate = null;
            eventHandlerDelegate = null;
            base.Dispose();
        }
    }
}
