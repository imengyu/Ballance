using System;
using System.Collections.Generic;
using Ballance2.Base.Handler;
using Ballance2.Services;
using UnityEngine;

/*
 * Copyright (c) 2022  mengyu
 * 
 * 模块名：     
 * GameEventEmitter.cs
 *
 * 用途：
 * 游戏事件发射器。
 * 
 * 作者：
 * imengyu
 */

namespace Ballance2.Base
{
  /// <summary>
  /// 游戏事件发射器
  /// </summary>
  [SLua.CustomLuaClass]
  [Serializable]
  [LuaApiDescription("游戏事件发射器")]
  public class GameEventEmitter
  {
    public GameEventEmitter(string name) { Name = name; }

    /// <summary>
    /// 当前事件发射器的名称
    /// </summary>
    /// <value></value>
    [LuaApiDescription("当前事件发射器的名称")]
    public string Name { get; }

    public Dictionary<string, GameEventEmitterStorage> _Events = new Dictionary<string, GameEventEmitterStorage>();
    
    /// <summary>
    /// 获取指定的事件
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <returns></returns>
    [LuaApiDescription("获取指定的事件")]
    [LuaApiParamDescription("name", "事件名称")]
    public GameEventEmitterStorage GetEvent(string name) {
      GameEventEmitterStorage result = null;
      _Events.TryGetValue(name, out result);
      return result;
    }
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <returns></returns>
    [LuaApiDescription("注册事件")]
    [LuaApiParamDescription("name", "事件名称")]
    public GameEventEmitterStorage RegisterEvent(string name) {
      GameEventEmitterStorage result = null;
      if(_Events.TryGetValue(name, out result))
        return result;
      result = new GameEventEmitterStorage(name);
      return result; 
    }
    /// <summary>
    /// 发射事件
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <param name="param">事件名称参数</param>
    [LuaApiDescription("发射事件")]
    [LuaApiParamDescription("name", "事件名称")]
    [LuaApiParamDescription("param", "事件名称参数")]
    public void EmitEvent(string name, object param) {
      if(_Events.TryGetValue(name, out var result))
        result.Emit(param);
    }
    /// <summary>
    /// 删除指定事件
    /// </summary>
    /// <param name="name">事件名称</param>
    [LuaApiDescription("删除指定事件")]
    [LuaApiParamDescription("name", "事件名称")]
    public void DeleteEvent(string name) {
      if(_Events.ContainsKey(name))
        _Events.Remove(name);
    }
  }

  /// <summary>
  /// 事件发射器的事件存储类
  /// </summary>
  [SLua.CustomLuaClass]
  [Serializable]
  [LuaApiDescription("事件发射器的事件存储类")]
  public class GameEventEmitterStorage
  {
    public GameEventEmitterStorage(string name) { Name = name; }

    /// <summary>
    /// 当前事件的名称
    /// </summary>
    /// <value></value>
    [LuaApiDescription("当前事件的名称")]
    public string Name { get; }

    /// <summary>
    /// 发射当前事件
    /// </summary>
    [LuaApiDescription("发射当前事件")]
    public void Emit(object obj) {
      GameEventEmitterHandler n = _Listeners;
      GameEventEmitterHandler next = null;
      while(n != null) {
        next = n.Next;
        n.Delegate.Invoke(obj);
        //只执行一次就删除
        if(n.Once) 
          n.Off();
        //下一个
        n = next;
      }
    }

    internal GameEventEmitterHandler _Listeners;

    /// <summary>
    /// 增加事件侦听
    /// </summary>
    /// <param name="fn"></param>
    [LuaApiDescription("增加事件侦听")]
    public GameEventEmitterHandler On(GameEventEmitterDelegate fn) { return AddListener(fn, null, false); }
    /// <summary>
    /// 增加事件侦听并且设置标签
    /// </summary>
    /// <param name="fn"></param>
    [LuaApiDescription("增加事件侦听并且设置标签")]
    public GameEventEmitterHandler OnWithTag(GameEventEmitterDelegate fn, string tag) { return AddListener(fn, tag, false); }
    /// <summary>
    /// 增加单次事件侦听
    /// </summary>
    /// <param name="fn"></param>
    [LuaApiDescription("增加单次事件侦听")]
    public GameEventEmitterHandler Once(GameEventEmitterDelegate fn) { return AddListener(fn, null, true); }

    private GameEventEmitterHandler AddListener(GameEventEmitterDelegate fn, string tag, bool once) {
      GameEventEmitterHandler result = new GameEventEmitterHandler(this);
      if(_Listeners != null) {
        result.Next = _Listeners;
        _Listeners.Prev = result;
      }

      _Listeners = result;
      result.Delegate = fn;
      result.Tag = tag;
      result.Once = once;
      return result;
    }

    /// <summary>
    /// 移除事件侦听
    /// </summary>
    /// <param name="fn"></param>
    [LuaApiDescription("移除事件侦听")]
    public void Off(GameEventEmitterDelegate fn) {
      GameEventEmitterHandler n = _Listeners;
      GameEventEmitterHandler next = null;
      while(n != null) {
        next = n.Next;
        if(n.Delegate == fn) 
          n.Off();
        n = next;
      }
    }
    /// <summary>
    /// 移除事件指定标签的侦听
    /// </summary>
    /// <param name="fn"></param>
    [LuaApiDescription("移除事件指定标签的侦听")]
    public void OffAllTag(string tag) {
      GameEventEmitterHandler n = _Listeners;
      GameEventEmitterHandler next = null;
      while(n != null) {
        next = n.Next;
        if(n.Tag == tag) 
          n.Off();
        n = next;
      }
    }
    /// <summary>
    /// 清空当前事件的所有事件侦听
    /// </summary>
    [LuaApiDescription("清空当前事件的所有事件侦听")]
    public void Clear() {
      GameEventEmitterHandler n = _Listeners;
      GameEventEmitterHandler next = null;
      while(n != null) {
        next = n.Next;
        n.Off();
        n = next;
      }
      _Listeners = null;
    }
  }
  [SLua.CustomLuaClass]
  [Serializable]
  [LuaApiDescription("事件发射器接收器回调")]
  public class GameEventEmitterHandler
  {
    /// <summary>
    /// 获取当前回调所属事件
    /// </summary>
    [LuaApiDescription("获取当前回调所属事件")]
    public GameEventEmitterStorage Storage { get; }

    [SLua.DoNotToLua]
    public GameEventEmitterHandler(GameEventEmitterStorage storage) {
      Storage = storage;
    }

    [SLua.DoNotToLua]
    public GameEventEmitterHandler Next = null;
    [SLua.DoNotToLua]
    public GameEventEmitterHandler Prev = null;
    [SLua.DoNotToLua]
    public GameEventEmitterDelegate Delegate;
    [SLua.DoNotToLua]
    public bool Once = false;
    [SLua.DoNotToLua]
    public string Tag = null;

    /// <summary>
    /// 获取当前监听回调是否已经移除
    /// </summary>
    [LuaApiDescription("获取当前监听回调是否已经移除")]
    public bool Deleted = false;

    /// <summary>
    /// 移除当前监听回调
    /// </summary>
    [LuaApiDescription("移除当前监听回调")]
    public void Off() {
      if(this == Storage._Listeners)//第一个
        Storage._Listeners = Next;
      if(Prev != null)
        Prev.Next = Next;
      if(Next != null)
        Next.Prev = Prev;
      Deleted = true;
    }
  }

  /// <summary>
  /// 事件接收器内核回调
  /// </summary>
  /// <param name="evtName">事件名称</param>
  /// <param name="pararms">参数</param>
  /// <returns>返回是否中断其他事件的分发</returns>
  [SLua.CustomLuaClass]
  [LuaApiDescription("事件接收器内核回调")]
  public delegate void GameEventEmitterDelegate(object obj);
}
