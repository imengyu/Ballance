using SLua;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaObjecOnGUICaller.cs
* 
* 用途：
* Lua OnGUI 函数调用器。
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Services.LuaService.LuaWapper.GameLuaWapperEvents
{
    /// <summary>
    /// Lua Animator 事件调用器
    /// </summary>
    [CustomLuaClass]
    [LuaApiNoDoc]
    [LuaApiDescription("Lua OnGUI 函数调用器")]
    public class GameLuaObjecOnGUICaller : GameLuaObjectEventCaller
    {
        private LuaTable self = null;

        private LuaVoidDelegate onGUI = null;

        private string[] supportEvents = new string[]
        {
            "OnGUI",
        };

        public override GameLuaObjectEventWarps GetEventType()
        {
            return GameLuaObjectEventWarps.OnGUI;
        }
        public override string[] GetSupportEvents()
        {
            return supportEvents;
        }
       
        protected override void OnInitLua(GameLuaObjectHost host)
        {
            LuaFunction fun;
            self = host.LuaSelf;

            fun = self["OnGUI"] as LuaFunction;
            if (fun != null) onGUI = fun.cast<LuaVoidDelegate>();
        }

        private void OnGUI()
        {
            if (onGUI != null) onGUI(self);
        }
    }
}
