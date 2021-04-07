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
    public class ToggleEx : MonoBehaviour
    {
        public RectTransform Drag;
        public RectTransform Background;
        public UnityEngine.UI.Image DragImage;
        public Color ActiveColor = new Color(1.0f, 0.7f, 0.0f);
        public Color DeactiveColor = new Color(0.55f, 0.55f, 0.55f);

        public UnityEngine.UI.Toggle.ToggleEvent onValueChanged;

        void Start()
        {
            EventTriggerListener.Get(Background.gameObject).onClick = (go) =>
            {
                isOn = !isOn;
            };
        }

        [SerializeField, HideInInspector]
        private bool on = false;

        public void UpdateOn()
        {
            DragImage.color = on ? ActiveColor : DeactiveColor;
            Drag.anchoredPosition = new Vector2(
                on ? 10.5f : 31.6f,
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
                    onValueChanged.Invoke(on);
                    UpdateOn();
                }
            }
        }
    }
}
