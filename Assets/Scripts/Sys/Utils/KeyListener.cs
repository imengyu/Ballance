using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Sys.Utils
{
    [SLua.CustomLuaClass]
    /// <summary>
    /// 键 事件侦听器
    /// </summary>
    public class KeyListener : MonoBehaviour
    {
        [SLua.CustomLuaClass]
        /// <summary>
        /// 键盘按键事件回调
        /// </summary>
        /// <param name="downed">是否按下</param>
        public delegate void KeyDelegate(KeyCode key, bool downed);

        /// <summary>
        /// 从 指定 GameObject 创建键事件侦听器
        /// </summary>
        /// <param name="go">指定 GameObject</param>
        /// <returns></returns>
        static public KeyListener Get(GameObject go)
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
        private int updateInterval = 0;
        private bool isListenKey = true;

        /// <summary>
        /// 检测延迟
        /// </summary>
        public int UpdateInterval
        {
            set { updateInterval = value; }
            get { return updateInterval;  }
        }
        /// <summary>
        /// 是否开启监听
        /// </summary>
        public bool IsListenKey { get { return isListenKey; } set { isListenKey = value; } }

        /// <summary>
        /// 添加侦听器侦听键。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <param name="key2">键值2。</param>
        /// <param name="callBack">回调函数。</param>
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
        public void ClearKeyListen()
        {
            items.Clear();
        }

        private int updateTick = 0;
        private void Update()
        {
            if (isListenKey)
            {
                if (updateTick < updateInterval) updateTick++;
                else
                {
                    updateTick = 0;
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].has2key)
                        {
                            if (Input.GetKeyDown(items[i].key2) && !items[i].downed)
                            {
                                items[i].downed = true;
                                items[i].callBack(items[i].key2, true);
                            }
                            else if (Input.GetKeyUp(items[i].key2) && items[i].downed)
                            {
                                items[i].callBack(items[i].key2, false);
                                items[i].downed = false;
                            }
                        }
                        if (Input.GetKeyDown(items[i].key) && !items[i].downed)
                        {
                            items[i].downed = true;
                            items[i].callBack(items[i].key, true);
                        }
                        else if (Input.GetKeyUp(items[i].key) && items[i].downed)
                        {
                            items[i].callBack(items[i].key, false);
                            items[i].downed = false;
                        }
                    }
                }
            }
        }
    }
}
