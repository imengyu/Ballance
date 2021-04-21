using Ballance2.Sys.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*

* Copyright(c) 2021  mengyu
*
* 模块名：     
* UIDragControl.cs
* 
* 用途：
* UI 控件拖动脚本
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-1 创建
*
*/

namespace Ballance2.Sys.UI.Utils
{
    /// <summary>
    /// UI 控件拖动脚本
    /// </summary>
    public class UIDragControl : UIBehaviour
    {
        public RectTransform dragTransform;
        public Window window;

        private Vector2 mouseOffest = Vector2.zero;

        protected override void Awake()
        {
            base.Awake();

            if (dragTransform == null) dragTransform = GetComponent<RectTransform>();

            EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

            UnityAction<BaseEventData> pointerdownClick = new UnityAction<BaseEventData>(OnPointerDown);
            EventTrigger.Entry myclickDown = new EventTrigger.Entry();
            myclickDown.eventID = EventTriggerType.PointerDown;
            myclickDown.callback.AddListener(pointerdownClick);
            trigger.triggers.Add(myclickDown);

            UnityAction<BaseEventData> pointerupClick = new UnityAction<BaseEventData>(OnPointerUp);
            EventTrigger.Entry myclickUp = new EventTrigger.Entry();
            myclickUp.eventID = EventTriggerType.PointerUp;
            myclickUp.callback.AddListener(pointerupClick);
            trigger.triggers.Add(myclickUp);

            UnityAction<BaseEventData> pointerdragClick = new UnityAction<BaseEventData>(OnDrag);
            EventTrigger.Entry myclickDrag = new EventTrigger.Entry();
            myclickDrag.eventID = EventTriggerType.Drag;
            myclickDrag.callback.AddListener(pointerdragClick);
            trigger.triggers.Add(myclickDrag);
        }

        private GameUIManager UIManager;

        protected override void Start() {
            if (GameManager.Instance != null)
                UIManager = (GameUIManager)GameManager.Instance.GetSystemService("GameUIManager");
        }

        public void OnPointerDown(BaseEventData data)
        {
            dragTransform.SetAsLastSibling();
            mouseOffest = new Vector2(Input.mousePosition.x - dragTransform.position.x, Input.mousePosition.y - dragTransform.position.y);

            if(window != null && UIManager.GetCurrentActiveWindow() != window)
                UIManager.ActiveWindow(window);
        }
        public void OnPointerUp(BaseEventData data)
        {
            mouseOffest = Vector2.zero;
        }
        public void OnDrag(BaseEventData data)
        {
            Vector3  pos = new Vector3(Input.mousePosition.x - mouseOffest.x, Input.mousePosition.y - mouseOffest.y, 0);

            if (pos.x < 0) pos.x = 0;
            else if (pos.x > Screen.width - 30) pos.x = Screen.width - 30;

           if (pos.y < 30) pos.y = 30;
           else if(pos.y > Screen.height) pos.y = Screen.height ;

            dragTransform.position = pos;
        }
    }
}
