using Ballance.LuaHelpers;
using SLua;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaObjectPhysics2DEventCaller.cs
* 
* 用途：
* Lua 2D物理 事件调用器。
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
    /// Lua 2D物理 事件调用器
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("Lua 2D物理 事件调用器")]
    public class GameLuaObjectPhysics2DEventCaller : GameLuaObjectEventCaller
    {
        private LuaTable self = null;

        private LuaCollision2DDelegate onCollisionEnter2D = null;
        private LuaCollision2DDelegate onCollisionExit2D = null;
        private LuaCollision2DDelegate onCollisionStay2D = null;
        private LuaCollider2DDelegate onTriggerEnter2D = null;
        private LuaCollider2DDelegate onTriggerExit2D = null;
        private LuaCollider2DDelegate onTriggerStay2D = null;

        private string[] supportEvents = new string[]
        {
            "OnCollisionEnter2D",
            "OnCollisionExit2D",
            "OnCollisionStay2D",
            "OnTriggerEnter2D",
            "OnTriggerExit2D",
            "OnTriggerStay2D",
        };

        public override GameLuaObjectEventWarps GetEventType()
        {
            return GameLuaObjectEventWarps.Physics2D;
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

            fun = self["OnCollisionEnter2D"] as LuaFunction;
            if (fun != null) onCollisionEnter2D = fun.cast<LuaCollision2DDelegate>();

            fun = self["OnCollisionExit2D"] as LuaFunction;
            if (fun != null) onCollisionExit2D = fun.cast<LuaCollision2DDelegate>();

            fun = self["onCollisionStay2D"] as LuaFunction;
            if (fun != null) onCollisionStay2D = fun.cast<LuaCollision2DDelegate>();


            fun = self["OnTriggerEnter2D"] as LuaFunction;
            if (fun != null) onTriggerEnter2D = fun.cast<LuaCollider2DDelegate>();

            fun = self["OnTriggerExit2D"] as LuaFunction;
            if (fun != null) onTriggerExit2D = fun.cast<LuaCollider2DDelegate>();

            fun = self["OnTriggerStay2D"] as LuaFunction;
            if (fun != null) onTriggerStay2D = fun.cast<LuaCollider2DDelegate>();
        }

        //Collision
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (onCollisionEnter2D != null) onCollisionEnter2D(self, collision);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (onCollisionExit2D != null) onCollisionExit2D(self, collision);
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (onCollisionStay2D != null) onCollisionStay2D(self, collision);
        }

        //Tigger

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (onTriggerEnter2D != null) onTriggerEnter2D(self, other);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (onTriggerExit2D != null) onTriggerExit2D(self, other);
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (onTriggerStay2D != null) onTriggerStay2D(self, other);
        }
    }
}
