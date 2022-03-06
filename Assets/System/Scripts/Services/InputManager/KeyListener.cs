using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

namespace Ballance2.Services.InputManager
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
      public int id;
    }

    private LinkedList<KeyListenerItem> items = new LinkedList<KeyListenerItem>();
    private bool isListenKey = true;
    private int listenKeyId = 0;

    /// <summary>
    /// 是否开启监听
    /// </summary>
    [LuaApiDescription("是否开启监听")]
    public bool IsListenKey { get { return isListenKey; } set { isListenKey = value; } }

    /// <summary>
    /// 如果UI激活时是否禁用键盘事件
    /// </summary>
    [LuaApiDescription("如果UI激活时是否禁用键盘事件")]
    public bool DisableWhenUIFocused = true;

    /// <summary>
    /// 指定是否允许同时发出1个以上的键盘事件，否则同时只能发送一个键盘事件。以后注册的先发送
    /// </summary>
    [LuaApiDescription("指定是否允许同时发出1个以上的键盘事件，否则同时只能发送一个键盘事件。以后注册的先发送")]
    public bool AllowMultipleKey = false;

    /// <summary>
    /// 重新发送当前已按下的按键事件
    /// </summary>
    [LuaApiDescription("重新发送当前已按下的按键事件")]
    public void ReSendPressingKey() {
      LinkedListNode<KeyListenerItem> cur = items.Last;
      while(cur != null) {
        var item = cur.Value;
        if (item.has2key && item.downed)
          item.callBack(item.key2, true);
        else
          item.callBack(item.key2, false);
        if (item.downed)
          item.callBack(item.key, true);
        else
          item.callBack(item.key, false);
        cur = cur.Previous;
      }
    }

    /// <summary>
    /// 添加侦听器侦听键。
    /// </summary>
    /// <param name="key">键值。</param>
    /// <param name="key2">键值2。</param>
    /// <param name="callBack">回调函数。</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听器</returns>
    [LuaApiDescription("添加侦听器侦听键", "返回一个ID, 可使用 DeleteKeyListen 删除侦听器")]
    [LuaApiParamDescription("key", "键值")]
    [LuaApiParamDescription("key2", "键值2")]
    [LuaApiParamDescription("callBack", "回调函数")]
    public int AddKeyListen(KeyCode key, KeyCode key2, KeyDelegate callBack)
    {
      listenKeyId++;

      KeyListenerItem item = new KeyListenerItem();
      item.callBack = callBack;
      item.key = key;
      item.key2 = key2;
      item.has2key = key2 != KeyCode.None;
      item.id = listenKeyId;

      //逆序遍历链表。添加按键至相同按键位置
      LinkedListNode<KeyListenerItem> cur = items.Last;
      while(cur != null) {
        if(cur.Value.key == key) {
          items.AddAfter(cur, item);
          return listenKeyId;
        }
        cur = cur.Previous;
      }
      //没有找到相同按键，则添加到末尾
      items.AddLast(item);
      return listenKeyId;
    }
    /// <summary>
    /// 添加侦听器侦听键。
    /// </summary>
    /// <param name="key">键值。</param>
    /// <param name="callBack">回调函数。</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听器</returns>
    [LuaApiDescription("添加侦听器侦听键。", "返回一个ID, 可使用 DeleteKeyListen 删除侦听器")]
    [LuaApiParamDescription("key", "键值")]
    [LuaApiParamDescription("callBack", "回调函数")]
    public int AddKeyListen(KeyCode key, KeyDelegate callBack) { return AddKeyListen(key, KeyCode.None, callBack); }
    /// <summary>
    /// 删除指定侦听器。
    /// </summary>
    /// <param name="id">AddKeyListen 返回的ID</param>
    [LuaApiDescription("删除指定侦听器。")]
    [LuaApiParamDescription("id", "AddKeyListen 返回的ID")]
    public void DeleteKeyListen(int id)
    {
      //链表移除
      int count = 0;
      LinkedListNode<KeyListenerItem> cur = items.First;
      while(cur != null) {
        if(cur.Value.id == id) {
          items.Remove(cur);
          return;
        }
        cur = cur.Next;
        count++;

        if(count > items.Count)
          break;
      }
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
        //排除GUI激活
        if (DisableWhenUIFocused && (EventSystem.current.IsPointerOverGameObject() || GUIUtility.hotControl != 0))
          return;

        //逆序遍历链表。后添加的按键事件最先处理
        LinkedListNode<KeyListenerItem> cur = items.Last;
        KeyCode lastPressedKey = KeyCode.None;
        while(cur != null) {
          var item = cur.Value;

          if (item.has2key)
          {
            if (Input.GetKeyDown(item.key2) && !item.downed)
            {
              item.downed = true;
              item.callBack(item.key2, true);
            }
            if (Input.GetKeyUp(item.key2) && item.downed)
            {
              item.downed = false;
              item.callBack(item.key2, false);
            }
          }

          if (Input.GetKeyDown(item.key) && !item.downed)
          {
            if(!AllowMultipleKey && lastPressedKey == item.key) {
              //相同的按键，并且不允许发送相同按键，则不发送按键
              item.downed = false;
            } else {
              item.downed = true;
              item.callBack(item.key, true);

              if(!AllowMultipleKey)
                lastPressedKey = item.key;
            }
          }
          if (Input.GetKeyUp(item.key) && item.downed)
          {
            item.downed = false;
            item.callBack(item.key, false);
          }
          cur = cur.Previous;
        }
      }
    }
  }
}
