using SLua;
using UnityEngine;

namespace Ballance2.System.Bridge.LuaWapper.GameLuaWapperEvents
{
    [CustomLuaClass]
    public class GameLuaObjectPhysicsEventCaller : GameLuaObjectEventCaller
    {
        private LuaTable self = null;

        private LuaCollisionDelegate onCollisionEnter = null;
        private LuaCollisionDelegate onCollisionExit = null;
        private LuaCollisionDelegate onCollisionStay = null;
        private LuaColliderDelegate onTriggerEnter = null;
        private LuaColliderDelegate onTriggerExit = null;
        private LuaColliderDelegate onTriggerStay = null;
        
    
        private string[] supportEvents = new string[]
        {
            "OnCollisionEnter",
            "OnCollisionExit",
            "OnCollisionStay",
            "OnTriggerEnter",
            "OnTriggerExit",
            "OnTriggerStay",
        };

        public override GameLuaObjectEventWarps GetEventType()
        {
            return GameLuaObjectEventWarps.Physics;
        }
        public override string[] GetSupportEvents()
        {
            return supportEvents;
        }
        public override void OnInitLua(GameLuaObjectHost host)
        {
            LuaFunction fun;
            self = host.LuaSelf;

            fun = self["OnCollisionEnter"] as LuaFunction;
            if (fun != null) onCollisionEnter = fun.cast<LuaCollisionDelegate>();

            fun = self["OnCollisionExit"] as LuaFunction;
            if (fun != null) onCollisionExit = fun.cast<LuaCollisionDelegate>();

            fun = self["OnCollisionStay"] as LuaFunction;
            if (fun != null) onCollisionStay = fun.cast<LuaCollisionDelegate>();


            fun = self["OnTriggerEnter"] as LuaFunction;
            if (fun != null) onTriggerEnter = fun.cast<LuaColliderDelegate>();

            fun = self["OnTriggerExit"] as LuaFunction;
            if (fun != null) onTriggerExit = fun.cast<LuaColliderDelegate>();

            fun = self["OnTriggerStay"] as LuaFunction;
            if (fun != null) onTriggerStay = fun.cast<LuaColliderDelegate>();
        }


        //Collision

        private void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnter != null) onCollisionEnter(self, collision);
        }
        private void OnCollisionExit(Collision collision)
        {
            if (onCollisionExit != null) onCollisionExit(self, collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            if (onCollisionStay != null) onCollisionStay(self, collision);
        }

        //Tigger

        private void OnTriggerEnter(Collider other)
        {
            if (onTriggerEnter != null) onTriggerEnter(self, other);
        }
        private void OnTriggerExit(Collider other)
        {
            if (onTriggerExit != null) onTriggerExit(self, other);
        }
        private void OnTriggerStay(Collider other)
        {
            if (onTriggerStay != null) onTriggerStay(self, other);
        }
    }
}
