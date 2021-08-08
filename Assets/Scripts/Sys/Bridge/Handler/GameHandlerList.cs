using System.Collections.Generic;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameHandlerList.cs
 * 用途：
 * GameHandler的一个List包装类。
 * 
 * 作者：
 * mengyu
 *
 */

namespace Ballance2.Sys.Bridge.Handler
{
    /// <summary>
    /// GameHandler的一个List包装类
    /// </summary>
    public class GameHandlerList : List<GameHandler>
    {
        public void CallEventHandler(string evtName, params object[] parm)
        {
            foreach(GameHandler h in this)
                h.CallEventHandler(evtName, parm);
        }
        public void Dispose()
        {
            foreach (GameHandler h in this)
                h.Dispose();
            this.Clear();
        }
    }
}
