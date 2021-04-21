using UnityEngine;
using Ballance2.Sys.UI.Utils;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Toggle.cs
* 
* 用途：
* 一个开关组件，与原版的Chexkbox区分开来
* 
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-15 创建
*
*/

namespace Ballance2.Sys.UI {

    /// <summary>
    /// 一个开关组件
    /// </summary>
    [ExecuteInEditMode]
    [SLua.CustomLuaClass]
    [Ballance.LuaHelpers.LuaApiDescription("一个开关组件")]
    public class ToggleEx : MonoBehaviour
    {
        public RectTransform Drag;
        public RectTransform Background;
        public UnityEngine.UI.Image DragImage;
        public Sprite ActiveImage;
        public Sprite DeactiveImage;

        public UnityEngine.UI.Toggle.ToggleEvent onValueChanged;

        void Start()
        {
            EventTriggerListener.Get(Background.gameObject).onClick = (go) =>
            {
                isOn = !isOn;
            };
            UpdateOn();
        }

        [SerializeField, HideInInspector]
        private bool on = false;

        public void UpdateOn()
        {
            DragImage.sprite = on ? ActiveImage : DeactiveImage;
            Drag.anchoredPosition = new Vector2(
                on ? 31.6f : 10.5f,
                Drag.anchoredPosition.y
            );
        }

        public bool isOn
        {
            get { return on; }
            set
            {
                if(on != value)
                {
                    on = value;
                    if(!Application.isEditor)
                        onValueChanged.Invoke(on);
                    UpdateOn();
                }
            }
        }
    }
}
