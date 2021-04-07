using UnityEngine;

namespace Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents
{
    public class GameLuaObjectEventCaller : MonoBehaviour
    {
        public virtual string[] GetSupportEvents() { return null; }
        public virtual void OnInitLua(GameLuaObjectHost host) { }
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
    }
}
