using Ballance.LuaHelpers;
using SLua;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaObjectOtherEventCaller.cs
* 
* 用途：
* Lua 其他不常用事件调用器
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-22 创建
*
*/

namespace Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents
{
    /// <summary>
    /// Lua 其他不常用事件调用器
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("Lua 其他不常用事件调用器")]
    public class GameLuaObjectOtherEventCaller : GameLuaObjectEventCaller
    {
        private LuaTable self = null;

        private LuaBoolDelegate luaOnApplicationFocus = null;
        private LuaBoolDelegate luaOnApplicationPause = null;
        private LuaVoidDelegate luaOnApplicationQuit = null;
        private LuaVoidDelegate luaOnValidate = null;
        private LuaVoidDelegate luaOnDrawGizmos = null;
        private LuaVoidDelegate luaOnDrawGizmosSelected = null;
        private LuaVoidDelegate luaOnBecameInvisible = null;
        private LuaVoidDelegate luaOnBecameVisible = null;

        private string[] supportEvents = new string[]
        {
             "OnApplicationFocus",
             "OnApplicationPause",
             "OnApplicationQuit",
             "OnValidate",
             "OnDrawGizmos",
             "OnDrawGizmosSelected",
             "OnBecameInvisible",
             "OnBecameVisible",
        };

        public override GameLuaObjectEventWarps GetEventType()
        {
            return GameLuaObjectEventWarps.Other;
        }
        public override string[] GetSupportEvents()
        {
            return supportEvents;
        }
        [DoNotToLua]
        public override void OnInitLua(GameLuaObjectHost host)
        {
            LuaFunction fun;
            self = host.LuaSelf;

            fun = self["OnApplicationFocus"] as LuaFunction;
            if (fun != null) luaOnApplicationFocus = fun.cast<LuaBoolDelegate>();

            fun = self["OnApplicationPause"] as LuaFunction;
            if (fun != null) luaOnApplicationPause = fun.cast<LuaBoolDelegate>();

            fun = self["OnApplicationQuit"] as LuaFunction;
            if (fun != null) luaOnApplicationQuit = fun.cast<LuaVoidDelegate>();

            fun = self["OnValidate"] as LuaFunction;
            if (fun != null) luaOnValidate = fun.cast<LuaVoidDelegate>();

            fun = self["OnDrawGizmos"] as LuaFunction;
            if (fun != null) luaOnDrawGizmos = fun.cast<LuaVoidDelegate>();

            fun = self["OnDrawGizmosSelected"] as LuaFunction;
            if (fun != null) luaOnDrawGizmosSelected = fun.cast<LuaVoidDelegate>();

            fun = self["OnBecameInvisible"] as LuaFunction;
            if (fun != null) luaOnBecameInvisible = fun.cast<LuaVoidDelegate>();

            fun = self["OnBecameVisible"] as LuaFunction;
            if (fun != null) luaOnBecameVisible = fun.cast<LuaVoidDelegate>();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (luaOnApplicationFocus != null)
                luaOnApplicationFocus(self, focus);
        }
        private void OnApplicationPause(bool pause)
        {
            if (luaOnApplicationPause != null)
                luaOnApplicationPause(self, pause);
        }
        private void OnApplicationQuit()
        {
            if (luaOnApplicationQuit != null)
                luaOnApplicationQuit(self);
        }
        private void OnValidate()
        {
            if (luaOnValidate != null)
                luaOnValidate(self);
        }
        private void OnDrawGizmos()
        {
            if (luaOnDrawGizmos != null)
                luaOnDrawGizmos(self);
        }
        private void OnDrawGizmosSelected()
        {
            if (luaOnDrawGizmosSelected != null)
                luaOnDrawGizmosSelected(self);
        }
        private void OnBecameInvisible()
        {
            if (luaOnBecameInvisible != null)
                luaOnBecameInvisible(self);
        }
        private void OnBecameVisible()
        {
            if (luaOnBecameVisible != null)
                luaOnBecameVisible(self);
        }
    }
}
