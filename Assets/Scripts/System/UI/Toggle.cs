using UnityEngine;
using Ballance2.System.UI.Utils;

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

namespace Ballance2.System.UI {

    /// <summary>
    /// 一个开关组件
    /// </summary>
    [ExecuteInEditMode]
    public class Toggle : MonoBehaviour
    {
        public RectTransform Drag;
        public RectTransform Background;
        public UnityEngine.UI.Image DragImage;

        public UnityEngine.UI.Toggle.ToggleEvent onValueChanged;

        void Start()
        {
            EventTriggerListener.Get(Background.gameObject).onClick = (go) =>
            {
                isOn = !isOn;
            };
        }
        private void Update()
        {
            if (on != currentOn)
                updateOn();
        }

        [SerializeField, SetProperty("isOn")]
        private bool on = false;
        private bool currentOn = false;

        private void updateOn()
        {
            currentOn = on;
            DragImage.color = on ? Color.white : new Color(0.55f, 0.55f, 0.55f);
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
                    updateOn();
                }
            }
        }
    }
}
