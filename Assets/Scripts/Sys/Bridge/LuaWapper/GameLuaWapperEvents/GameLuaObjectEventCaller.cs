using UnityEngine;


/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaObjectEventCaller.cs
* 
* 用途：
* Lua 事件调用器声明。
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
    [RequireComponent(typeof(GameLuaObjectHost))]
    public class GameLuaObjectEventCaller : MonoBehaviour
    {
        private void Awake() {
            var host = GetComponent<GameLuaObjectHost>();
            host.OnInitLua += () => {
                OnInitLua(host);
            };
        }
        protected virtual void OnInitLua(GameLuaObjectHost host) { }
        
        public virtual string[] GetSupportEvents() { return null; }
        public virtual GameLuaObjectEventWarps GetEventType() { return GameLuaObjectEventWarps.Unknow; }
    }

    public enum GameLuaObjectEventWarps
    {
        Unknow,
        Physics,
        Physics2D,
        Mouse,
        Animator,
        Particle,
        EventTrigger,
        Other,
        OnGUI,
    }
}
