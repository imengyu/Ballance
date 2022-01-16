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
  public class KeyListener : MonoBehaviour
  {
    /// <summary>
    /// 键盘按键事件回调
    /// </summary>
    /// <param name="downed">是否按下</param>
    public delegate void KeyDelegate(KeyCode key, bool downed);

    /// <summary>
    /// 从 指定 GameObject 创建键事件侦听器
    /// </summary>
    /// <param name="go">指定 GameObject</param>
    /// <returns>返回事件侦听器实例</returns> 
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

    private Dictionary<int, KeyListenerItem> items = new Dictionary<int, KeyListenerItem>();
    private Dictionary<int, bool> itemsDownStatus = new Dictionary<int, bool>();
    private bool isListenKey = true;
    private bool isKeyModfied = false;
    private int listenKeyId = 0;

    /// <summary>
    /// 是否开启监听
    /// </summary>
    public bool IsListenKey { get { return isListenKey; } set { isListenKey = value; } }

    /// <summary>
    /// 如果UI激活时是否禁用键盘事件
    /// </summary>
    public bool DisableWhenUIFocused = true;

    /// <summary>
    /// 添加侦听器侦听键。
    /// </summary>
    /// <param name="key">键值。</param>
    /// <param name="key2">键值2。</param>
    /// <param name="callBack">回调函数。</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听器</returns>
    public int AddKeyListen(KeyCode key, KeyCode key2, KeyDelegate callBack)
    {
      KeyListener.KeyListenerItem item = new KeyListener.KeyListenerItem();
      item.callBack = callBack;
      item.key = key;
      item.key2 = key2;
      item.has2key = true;
      items.Add(++listenKeyId, item);
      itemsDownStatus[listenKeyId] = false;
      isKeyModfied = true;
      return listenKeyId;
    }
    /// <summary>
    /// 添加侦听器侦听键。
    /// </summary>
    /// <param name="key">键值。</param>
    /// <param name="callBack">回调函数。</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听器</returns>
    public int AddKeyListen(KeyCode key, KeyDelegate callBack)
    {
      KeyListener.KeyListenerItem item = new KeyListener.KeyListenerItem();
      item.callBack = callBack;
      item.key = key;
      items.Add(++listenKeyId, item);
      itemsDownStatus[listenKeyId] = false;
      isKeyModfied = true;
      return listenKeyId;
    }
    /// <summary>
    /// 删除指定侦听器。
    /// </summary>
    /// <param name="id">AddKeyListen 返回的ID</param>
    public void DeleteKeyListen(int id)
    {
      itemsDownStatus.Remove(listenKeyId);
      items.Remove(id);
      isKeyModfied = true;
    }
    /// <summary>
    /// 清空事件侦听器所有侦听键。
    /// </summary>
    public void ClearKeyListen()
    {
      items.Clear();
      itemsDownStatus.Clear();
      isKeyModfied = true;
    }

    private void Update()
    {
      if (isListenKey)
      {
        //排除GUI激活
        if (DisableWhenUIFocused && (EventSystem.current.IsPointerOverGameObject() || GUIUtility.hotControl != 0))
          return;
          
        isKeyModfied = false;
        foreach (var v in items)
        {
          var item = v.Value;
          var itemDownStatus = itemsDownStatus[v.Key];
          if (item.has2key)
          {
            if (Input.GetKeyDown(item.key2) && !itemDownStatus)
            {
              itemDownStatus = true;
              item.callBack(item.key2, true);
              if (isKeyModfied) return;
            }
            if (Input.GetKeyUp(item.key2) && itemDownStatus)
            {
              itemDownStatus = false;
              item.callBack(item.key2, false);
              if (isKeyModfied) return;
            }
          }
          if (Input.GetKeyDown(item.key) && !itemDownStatus)
          {
            itemDownStatus = true;
            item.callBack(item.key, true);
            if (isKeyModfied) return;
          }
          if (Input.GetKeyUp(item.key) && itemDownStatus)
          {
            itemDownStatus = false;
            item.callBack(item.key, false);
            if (isKeyModfied) return;
          }
          itemsDownStatus[v.Key] = itemDownStatus;
        }
      }
    }
  }
}
