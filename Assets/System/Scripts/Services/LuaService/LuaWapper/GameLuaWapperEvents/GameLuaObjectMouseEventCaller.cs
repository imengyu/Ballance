using SLua;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaObjectMouseEventCaller.cs
* 
* 用途：
* Lua 鼠标事件调用器。
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Services.LuaService.LuaWapper.GameLuaWapperEvents
{
    /// <summary>
    /// Lua 鼠标事件调用器
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("Lua 鼠标事件调用器")]
    public class GameLuaObjectMouseEventCaller : GameLuaObjectEventCaller
    {
        private LuaTable self = null;

        private LuaVoidDelegate luaOnMouseDown = null;
        private LuaVoidDelegate luaOnMouseDrag = null;
        private LuaVoidDelegate luaOnMouseEnter = null;
        private LuaVoidDelegate luaOnMouseExit = null;
        private LuaVoidDelegate luaOnMouseOver = null;
        private LuaVoidDelegate luaOnMouseUp = null;
        private LuaVoidDelegate luaOnMouseUpAsButton = null;

        private string[] supportEvents = new string[] {
            "OnMouseDown",
            "OnMouseDrag",
            "OnMouseEnter",
            "OnMouseExit",
            "OnMouseOver",
            "OnMouseUp",
            "OnMouseUpAsButton",
        };

        public override GameLuaObjectEventWarps GetEventType()
        {
            return GameLuaObjectEventWarps.Mouse;
        }
        public override string[] GetSupportEvents()
        {
            return supportEvents;
        }
        
        protected override void OnInitLua(GameLuaObjectHost host)
        {
            LuaFunction fun;
            self = host.LuaSelf;

            fun = self["OnMouseDown"] as LuaFunction;
            if (fun != null) luaOnMouseDown = fun.cast<LuaVoidDelegate>();

            fun = self["OnMouseDrag"] as LuaFunction;
            if (fun != null) luaOnMouseDrag = fun.cast<LuaVoidDelegate>();

            fun = self["OnMouseEnter"] as LuaFunction;
            if (fun != null) luaOnMouseEnter = fun.cast<LuaVoidDelegate>();

            fun = self["OnMouseExit"] as LuaFunction;
            if (fun != null) luaOnMouseExit = fun.cast<LuaVoidDelegate>();

            fun = self["OnMouseOver"] as LuaFunction;
            if (fun != null) luaOnMouseOver = fun.cast<LuaVoidDelegate>();

            fun = self["OnMouseUp"] as LuaFunction;
            if (fun != null) luaOnMouseUp = fun.cast<LuaVoidDelegate>();

            fun = self["OnMouseUpAsButton"] as LuaFunction;
            if (fun != null) luaOnMouseUpAsButton = fun.cast<LuaVoidDelegate>();
        }

        private void OnMouseDown()
        {
            if (luaOnMouseDown != null) luaOnMouseDown(self);
        }
        private void OnMouseDrag()
        {
            if (luaOnMouseDrag != null) luaOnMouseDrag(self);
        }
        private void OnMouseEnter()
        {
            if (luaOnMouseEnter != null) luaOnMouseEnter(self);
        }
        private void OnMouseExit()
        {
            if (luaOnMouseExit != null) luaOnMouseExit(self);
        }
        private void OnMouseOver()
        {
            if (luaOnMouseOver != null) luaOnMouseOver(self);
        }
        private void OnMouseUp()
        {
            if (luaOnMouseUp != null) luaOnMouseUp(self);
        }
        private void OnMouseUpAsButton()
        {
            if (luaOnMouseUpAsButton != null) luaOnMouseUpAsButton(self);
        }
    }
}
