using System.Collections.Generic;
using Ballance.LuaHelpers;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* KeyListener.cs
* 
* 用途：
* 键盘按键事件事件侦听器
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Sys.Utils
{
    /// <summary>
    /// 键盘按键事件事件侦听器
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("键盘按键事件事件侦听器")]
    public class KeyListener : MonoBehaviour
    {    
        /// <summary>
        /// 键盘按键事件回调
        /// </summary>
        /// <param name="downed">是否按下</param>
        [SLua.CustomLuaClass]
        [LuaApiDescription("键盘按键事件回调")]
        public delegate void KeyDelegate(KeyCode key, bool downed);

        /// <summary>
        /// 从 指定 GameObject 创建键事件侦听器
        /// </summary>
        /// <param name="go">指定 GameObject</param>
        /// <returns>返回事件侦听器实例</returns>
        [LuaApiDescription("从 指定 GameObject 创建键事件侦听器", "返回事件侦听器实例")]
        [LuaApiParamDescription("go", "指定 GameObject")]
        public static KeyListener Get(GameObject go)
        {
            KeyListener listener = go.GetComponent<KeyListener>();
            if (listener == null) listener = go.AddComponent<KeyListener>();
            return listener;
        }

        private class KeyListenerItem
        {
            public KeyCode key2;
            public KeyCode key;
            public bool downed = false;
            public bool has2key = false;
            public KeyDelegate callBack;
        }

        private List<KeyListenerItem> items = new List<KeyListenerItem>();
        private bool isListenKey = true;

        /// <summary>
        /// 是否开启监听
        /// </summary>
        [LuaApiDescription("是否开启监听")]
        public bool IsListenKey { get { return isListenKey; } set { isListenKey = value; } }

        /// <summary>
        /// 添加侦听器侦听键。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <param name="key2">键值2。</param>
        /// <param name="callBack">回调函数。</param>
        [LuaApiDescription("添加侦听器侦听键")]
        [LuaApiParamDescription("key", "键值")]
        [LuaApiParamDescription("key2", "键值2")]
        [LuaApiParamDescription("callBack", "回调函数")]
        public void AddKeyListen(KeyCode key, KeyCode key2, KeyDelegate callBack)
        {
            KeyListener.KeyListenerItem item = new KeyListener.KeyListenerItem();
            item.callBack = callBack;
            item.key = key;
            item.key2 = key2;
            item.has2key = true;
            items.Add(item);
        }
        /// <summary>
        /// 添加侦听器侦听键。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <param name="callBack">回调函数。</param>
        [LuaApiDescription("添加侦听器侦听键。")]
        [LuaApiParamDescription("key", "键值")]
        [LuaApiParamDescription("callBack", "回调函数")]
        public void AddKeyListen(KeyCode key, KeyDelegate callBack)
        {
            KeyListener.KeyListenerItem item = new KeyListener.KeyListenerItem();
            item.callBack = callBack;
            item.key = key;
            items.Add(item);
        }
        /// <summary>
        /// 清空事件侦听器所有侦听键。
        /// </summary>
        [LuaApiDescription("清空事件侦听器所有侦听键。")]
        public void ClearKeyListen()
        {
            items.Clear();
        }

        private void Update()
        {
            if (isListenKey)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].has2key)
                    {
                        if (Input.GetKeyDown(items[i].key2) && !items[i].downed)
                        {
                            items[i].downed = true;
                            items[i].callBack(items[i].key2, true);
                        }
                        if (Input.GetKeyUp(items[i].key2) && items[i].downed)
                        {
                            items[i].downed = false;
                            items[i].callBack(items[i].key2, false);
                        }
                    }
                    if (Input.GetKeyDown(items[i].key) && !items[i].downed)
                    {
                        items[i].downed = true;
                        items[i].callBack(items[i].key, true);
                    }
                    if (Input.GetKeyUp(items[i].key) && items[i].downed)
                    {
                        items[i].downed = false;
                        items[i].callBack(items[i].key, false);
                    }
                }
            }
        }
    }
}
