using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// 一个键盘按键选择组件
    /// </summary>
    [SLua.CustomLuaClass]
    [Ballance2.LuaHelpers.LuaApiDescription("一个键盘按键选择组件")]
    [AddComponentMenu("Ballance/UI/Controls/KeyChoose")]
    public class KeyChoose : MonoBehaviour
    {
        public Text Text;
        public Text TextValue;

        [SerializeField, SetProperty("value")]
        private KeyCode _value = KeyCode.None;

        public class KeyChooseEvent : UnityEvent<KeyCode>
        {
            public KeyChooseEvent() {}
        }

        public KeyChooseEvent onValueChanged = new KeyChooseEvent();

        /// <summary>
        /// 获取或设置按钮选中的键
        /// </summary>
        public KeyCode value
        {
            get { return _value; }
            set
            {
                _value = value;
                UpdateValue();
                if (onValueChanged != null)
                    onValueChanged.Invoke(value);
            }
        }
        public void UpdateValue()
        {
            TextValue.text = _value.ToString();
        }

        void Start()
        {
            UpdateValue();
        }
        void Update()
        {
            if(Input.anyKeyDown && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                value = Event.current.keyCode;
            }
        }
    }
}
