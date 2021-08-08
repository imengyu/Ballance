using Ballance.LuaHelpers;
using SLua;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaObjectParticleEventCaller.cs
* 
* 用途：
* Lua Particle 事件调用器。
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
    /// Lua Particle 事件调用器
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("Lua Particle 事件调用器")]
    public class GameLuaObjectParticleEventCaller : GameLuaObjectEventCaller
    {
        private LuaTable self = null;

        private LuaGameObjectDelegate luaOnParticleCollision = null;
        private LuaVoidDelegate luaOnParticleSystemStopped = null;
        private LuaVoidDelegate luaOnParticleTrigger = null;

        private string[] supportEvents = new string[] {
            "OnParticleCollision",
            "OnParticleSystemStopped",
            "OnParticleTrigger",
        };

        public override GameLuaObjectEventWarps GetEventType()
        {
            return GameLuaObjectEventWarps.Particle;
        }
        public override string[] GetSupportEvents() {
            return supportEvents;
        }
        
        protected override void OnInitLua(GameLuaObjectHost host)
        {
            LuaFunction fun;
            self = host.LuaSelf;

            fun = self["OnParticleCollision"] as LuaFunction;
            if (fun != null) luaOnParticleCollision = fun.cast<LuaGameObjectDelegate>();

            fun = self["OnParticleSystemStopped"] as LuaFunction;
            if (fun != null) luaOnParticleSystemStopped = fun.cast<LuaVoidDelegate>();

            fun = self["OnParticleTrigger"] as LuaFunction;
            if (fun != null) luaOnParticleTrigger = fun.cast<LuaVoidDelegate>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (luaOnParticleCollision != null) luaOnParticleCollision(self, other);
        }
        private void OnParticleSystemStopped()
        {
            if (luaOnParticleSystemStopped != null) luaOnParticleSystemStopped(self);
        }
        private void OnParticleTrigger()
        {
            if (luaOnParticleTrigger != null) luaOnParticleTrigger(self);
        }
    }
}
