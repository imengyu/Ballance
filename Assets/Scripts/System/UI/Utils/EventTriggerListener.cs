using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Ballance2.System.UI.Utils
{
    [SLua.CustomLuaClass]
    /// <summary>
    /// UI 事件侦听器
    /// </summary>
    public class EventTriggerListener : EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);

        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;

        /// <summary>
        /// 从 指定 GameObject 创建事件侦听器
        /// </summary>
        /// <param name="go">指定 GameObject</param>
        /// <returns></returns>
        static public EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null) listener = go.AddComponent<EventTriggerListener>();
            return listener;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null) onClick(gameObject);
            else base.OnPointerClick(eventData);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject);
            else base.OnPointerDown(eventData);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
            else base.OnPointerEnter(eventData);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
            else base.OnPointerExit(eventData);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject);
            else base.OnPointerUp(eventData);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
            else base.OnSelect(eventData);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
            else base.OnUpdateSelected(eventData);
        }
    }
}