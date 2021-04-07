using SLua;
using UnityEngine;

namespace Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents
{
    [CustomLuaClass]
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
        public override void OnInitLua(GameLuaObjectHost host)
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
