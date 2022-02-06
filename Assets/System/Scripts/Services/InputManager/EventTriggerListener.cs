using SLua;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* EventTriggerListener.cs
* 
* 用途：
* UI 事件侦听器。不再使用。推荐使用UGUI的事件进行绑定。
*
* 作者：
* mengyu
*/

namespace Ballance2.Services.InputManager
{
  public delegate void GameObjectDelegate(GameObject go);
  
  /// <summary>
  /// UI 事件侦听器
  /// </summary>
  [CustomLuaClass]
  [LuaApiDescription("UI 事件侦听器")]
  public class EventTriggerListener : EventTrigger
  {
    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    [LuaApiDescription("鼠标点击事件")]
    public GameObjectDelegate onClick;
    /// <summary>
    /// 鼠标按下事件
    /// </summary>
    [LuaApiDescription("鼠标按下事件")]
    public GameObjectDelegate onDown;
    /// <summary>
    /// 鼠标进入事件
    /// </summary>
    [LuaApiDescription("鼠标进入事件")]
    public GameObjectDelegate onEnter;
    /// <summary>
    /// 鼠标离开事件
    /// </summary>
    public GameObjectDelegate onExit;
    /// <summary>
    /// 鼠标放开事件
    /// </summary>
    [LuaApiDescription("鼠标离开事件")]
    public GameObjectDelegate onUp;
    public GameObjectDelegate onSelect;
    public GameObjectDelegate onUpdateSelect;

    /// <summary>
    /// 从 指定 GameObject 创建事件侦听器
    /// </summary>
    /// <param name="go">指定 GameObject</param>
    /// <returns>返回事件侦听器实例</returns>
    [LuaApiDescription("从 指定 GameObject 创建事件侦听器", "返回事件侦听器实例")]
    [LuaApiParamDescription("go", "指定 GameObject")]
    static public EventTriggerListener Get(GameObject go)
    {
      EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
      if (listener == null) listener = go.AddComponent<EventTriggerListener>();
      return listener;
    }

    [DoNotToLua]
    public override void OnPointerClick(PointerEventData eventData)
    {
      if (onClick != null) onClick(gameObject);
      else base.OnPointerClick(eventData);
    }
    [DoNotToLua]
    public override void OnPointerDown(PointerEventData eventData)
    {
      if (onDown != null) onDown(gameObject);
      else base.OnPointerDown(eventData);
    }
    [DoNotToLua]
    public override void OnPointerEnter(PointerEventData eventData)
    {
      if (onEnter != null) onEnter(gameObject);
      else base.OnPointerEnter(eventData);
    }
    [DoNotToLua]
    public override void OnPointerExit(PointerEventData eventData)
    {
      if (onExit != null) onExit(gameObject);
      else base.OnPointerExit(eventData);
    }
    [DoNotToLua]
    public override void OnPointerUp(PointerEventData eventData)
    {
      if (onUp != null) onUp(gameObject);
      else base.OnPointerUp(eventData);
    }
    [DoNotToLua]
    public override void OnSelect(BaseEventData eventData)
    {
      if (onSelect != null) onSelect(gameObject);
      else base.OnSelect(eventData);
    }
    [DoNotToLua]
    public override void OnUpdateSelected(BaseEventData eventData)
    {
      if (onUpdateSelect != null) onUpdateSelect(gameObject);
      else base.OnUpdateSelected(eventData);
    }
  }
}