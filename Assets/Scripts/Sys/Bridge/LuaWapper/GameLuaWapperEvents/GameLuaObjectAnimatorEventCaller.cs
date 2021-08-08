using Ballance.LuaHelpers;
using SLua;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaObjectAnimatorEventCaller.cs
* 
* 用途：
* Lua Animator 事件调用器。
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents
{
    /// <summary>
    /// Lua Animator 事件调用器
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("Lua Animator 事件调用器")]
    public class GameLuaObjectAnimatorEventCaller : GameLuaObjectEventCaller
    {
        private LuaTable self = null;

        private LuaIntDelegate luaOnAnimatorIK = null;
        private LuaVoidDelegate luaOnAnimatorMove = null;

        private string[] supportEvents = new string[]
        {
            "OnAnimatorIK",
            "OnAnimatorMove"
        };

        public override GameLuaObjectEventWarps GetEventType()
        {
            return GameLuaObjectEventWarps.Animator;
        }
        public override string[] GetSupportEvents()
        {
            return supportEvents;
        }
       
        protected override void OnInitLua(GameLuaObjectHost host)
        {
            LuaFunction fun;
            self = host.LuaSelf;

            fun = self[supportEvents[0]] as LuaFunction;
            if (fun != null) luaOnAnimatorIK = fun.cast<LuaIntDelegate>();

            fun = self[supportEvents[1]] as LuaFunction;
            if (fun != null) luaOnAnimatorMove = fun.cast<LuaVoidDelegate>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (luaOnAnimatorIK != null)
                luaOnAnimatorIK(self, layerIndex);
        }
        private void OnAnimatorMove()
        {
            if (luaOnAnimatorMove != null)
                luaOnAnimatorMove(self);
        }
    }
}
