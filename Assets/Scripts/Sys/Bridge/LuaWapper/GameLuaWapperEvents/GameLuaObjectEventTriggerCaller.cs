using SLua;
using UnityEngine.EventSystems;

/*
 * Copyright (c) 2021  mengyu
 * 
 * 模块名：     
 * GameLuaObjectEventTriggerCaller.cs
 * 用途：
 * Lua的 EventTrigger 包装。
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
    [CustomLuaClass]
    public class GameLuaObjectEventTriggerCaller : GameLuaObjectEventCaller, 
        IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, 
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, 
        IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, 
        IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, 
        ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
    {
        private LuaTable self = null;

        private string[] supportEvents = new string[]
        {
            "IPointerEnterHandler",
            "IPointerExitHandler",
            "IPointerDownHandler",
            "IPointerUpHandler",
            "IPointerClickHandler",
            "IInitializePotentialDragHandler",
            "IBeginDragHandler",
            "IDragHandler",
            "IEndDragHandler",
            "IDropHandler",
            "IScrollHandler",
            "IUpdateSelectedHandler",
            "ISelectHandler",
            "IDeselectHandler",
            "IMoveHandler",
            "ICancelHandler",
            "ISubmitHandler",
            "IEventSystemHandler"
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

            fun = self["OnBeginDrag"] as LuaFunction; if (fun != null) luaOnBeginDrag = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnCancel"] as LuaFunction; if (fun != null) luaOnCancel = fun.cast<LuaBaseEventDataDelegate>();
            fun = self["OnDeselect"] as LuaFunction; if (fun != null) luaOnDeselect = fun.cast<LuaBaseEventDataDelegate>(); 
            fun = self["OnDrag"] as LuaFunction; if (fun != null) luaOnDrag = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnDrop"] as LuaFunction; if (fun != null) luaOnDrop = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnEndDrag"] as LuaFunction; if (fun != null) luaOnEndDrag = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnInitializePotentialDrag"] as LuaFunction; if (fun != null) luaOnInitializePotentialDrag = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnMove"] as LuaFunction; if (fun != null) luaOnMove = fun.cast<LuaAxisEventDataDelegate>();
            fun = self["OnPointerClick"] as LuaFunction; if (fun != null) luaOnPointerClick = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnPointerDown"] as LuaFunction; if (fun != null) luaOnPointerDown = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnPointerEnter"] as LuaFunction; if (fun != null) luaOnPointerEnter = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnPointerExit"] as LuaFunction; if (fun != null) luaOnPointerExit = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnPointerUp"] as LuaFunction; if (fun != null) luaOnPointerUp = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnScroll"] as LuaFunction; if (fun != null) luaOnScroll = fun.cast<LuaPointerEventDataDelegate>();
            fun = self["OnSelect"] as LuaFunction; if (fun != null) luaOnSelect = fun.cast<LuaBaseEventDataDelegate>();
            fun = self["OnSubmit"] as LuaFunction; if (fun != null) luaOnSubmit = fun.cast<LuaBaseEventDataDelegate>();
            fun = self["OnUpdateSelected"] as LuaFunction; if (fun != null) luaOnUpdateSelected = fun.cast<LuaBaseEventDataDelegate>();
        }

        private LuaPointerEventDataDelegate luaOnBeginDrag = null;
        private LuaBaseEventDataDelegate luaOnCancel = null;
        private LuaBaseEventDataDelegate luaOnDeselect = null;
        private LuaPointerEventDataDelegate luaOnDrag = null;
        private LuaPointerEventDataDelegate luaOnDrop = null;
        private LuaPointerEventDataDelegate luaOnEndDrag = null;
        private LuaPointerEventDataDelegate luaOnInitializePotentialDrag = null;
        private LuaAxisEventDataDelegate luaOnMove = null;
        private LuaPointerEventDataDelegate luaOnPointerClick = null;
        private LuaPointerEventDataDelegate luaOnPointerDown = null;
        private LuaPointerEventDataDelegate luaOnPointerExit = null;
        private LuaPointerEventDataDelegate luaOnPointerEnter = null;
        private LuaPointerEventDataDelegate luaOnPointerUp = null;
        private LuaPointerEventDataDelegate luaOnScroll = null;
        private LuaBaseEventDataDelegate luaOnSelect = null;
        private LuaBaseEventDataDelegate luaOnSubmit = null;
        private LuaBaseEventDataDelegate luaOnUpdateSelected = null;

        public void OnBeginDrag(PointerEventData eventData) { luaOnBeginDrag?.Invoke(self, eventData); }
        public void OnCancel(BaseEventData eventData) { luaOnCancel?.Invoke(self, eventData); }
        public void OnDeselect(BaseEventData eventData) { luaOnDeselect?.Invoke(self, eventData); }
        public void OnDrag(PointerEventData eventData) { luaOnDrag?.Invoke(self, eventData); }
        public void OnDrop(PointerEventData eventData) { luaOnDrop?.Invoke(self, eventData); }
        public void OnEndDrag(PointerEventData eventData) { luaOnEndDrag?.Invoke(self, eventData); }
        public void OnInitializePotentialDrag(PointerEventData eventData) { luaOnInitializePotentialDrag?.Invoke(self, eventData); }
        public void OnMove(AxisEventData eventData) { luaOnMove?.Invoke(self, eventData); }
        public void OnPointerClick(PointerEventData eventData) { luaOnPointerClick?.Invoke(self, eventData); }
        public void OnPointerDown(PointerEventData eventData) { luaOnPointerDown?.Invoke(self, eventData); }
        public void OnPointerEnter(PointerEventData eventData) { luaOnPointerEnter?.Invoke(self, eventData); }
        public void OnPointerExit(PointerEventData eventData) { luaOnPointerExit?.Invoke(self, eventData); }
        public void OnPointerUp(PointerEventData eventData) { luaOnPointerUp?.Invoke(self, eventData); }
        public void OnScroll(PointerEventData eventData) { luaOnScroll?.Invoke(self, eventData); }
        public void OnSelect(BaseEventData eventData) { luaOnSelect?.Invoke(self, eventData); }
        public void OnSubmit(BaseEventData eventData) { luaOnSubmit?.Invoke(self, eventData); }
        public void OnUpdateSelected(BaseEventData eventData) { luaOnUpdateSelected?.Invoke(self, eventData); }
    }
}
