using System.Collections.Generic;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIControlValueBinder.cs
* 
* 用途：
* UI控件数据绑定器。该绑定器用在需要绑定的UI控件上。
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core
{
  /// <summary>
  /// UI控件数据绑定器。该绑定器用在需要绑定的UI控件上。
  /// </summary>
  [SLua.CustomLuaClass]
  [AddComponentMenu("Ballance/UI/ValueBinder/Custom")]
  [RequireComponent(typeof(RectTransform))]
  [LuaApiDescription("UI控件数据绑定器。该绑定器用在需要绑定的UI控件上")]
  public class GameUIControlValueBinder : MonoBehaviour
  {
    /// <summary>
    /// 指定对应UI消息中心名字
    /// </summary>
    [Tooltip("指定对应UI消息中心名字")]
    [LuaApiDescription("指定对应UI消息中心名字")]
    public string MessageCenterName = null;

    /// <summary>
    /// 指定绑定器的名称，可在UI消息中心使用该名称查找
    /// </summary>
    [Tooltip("指定绑定器的名称，可在UI消息中心使用该名称查找")]
    [LuaApiDescription("指定绑定器的名称，可在UI消息中心使用该名称查找")]
    public string Name = "";

    /// <summary>
    /// 指定当前绑定器是否在Lua中处理，需在UI消息中心设置Lua处理函数
    /// </summary>
    [Tooltip("指定当前绑定器是否在Lua中处理，需在UI消息中心设置Lua处理函数")]
    [LuaApiDescription("指定当前绑定器是否在Lua中处理，需在UI消息中心设置Lua处理函数")]
    public bool SolveInLua = false;

    public List<GameUIControlValueBinderUserUpdateCallback> UserUpdateCallbacks = new List<GameUIControlValueBinderUserUpdateCallback>();
    public GameUIControlValueBinderSupplierCallback BinderSupplierCallback = null;
    public GameUIMessageCenter MessageCenter = null;

    private void Start()
    {
      MessageCenter = GameUIMessageCenter.FindGameUIMessageCenter(MessageCenterName);
      if (MessageCenter != null)
        MessageCenter.RegisterValueBinder(this);
      if (SolveInLua)
        MessageCenter.CallLuaBinderBegin(this);
      else
        BinderBegin();
    }
    private void Awake()
    {
      MessageCenter = GameUIMessageCenter.FindGameUIMessageCenter(MessageCenterName);
    }
    private void OnDestroy()
    {
      if (MessageCenter != null)
        MessageCenter.UnRegisterValueBinder(this);
    }

    protected virtual void BinderBegin() { }

    /// <summary>
    /// 通知UI更新事件
    /// </summary>
    /// <param name="newval">新的数值</param>
    [LuaApiDescription("通知UI更新事件")]
    [LuaApiParamDescription("newval", "新的数值")]
    public void NotifyUserUpdate(object newval)
    {
      UserUpdateCallbacks.ForEach((a) => a.Invoke(newval));
    }
  }

  [SLua.CustomLuaClass]
  public delegate bool GameUIControlValueBinderSupplierCallback(object value);
  [SLua.CustomLuaClass]
  public delegate void GameUIControlValueBinderUserUpdateCallback(object value);
}
