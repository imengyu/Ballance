/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePoolManager.cs
* 
* 用途：
* 对象池管理器。
*
* 作者：
* mengyu
*/

using System;
using System.Collections.Generic;
using Ballance2.Services.Debug;
using Ballance2.Services.Pool;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ballance2.Services
{
  /// <summary>
  /// 对象池管理器，分普通类对象池+资源游戏对象池
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("对象池管理器，分普通类对象池+资源游戏对象池")]
  public class GamePoolManager : GameService
  {
    private const string TAG = "GamePoolManager";

    public GamePoolManager() : base(TAG) { }

    [SLua.DoNotToLua]
    public override bool Initialize()
    {
      base.Initialize();
      return true;
    }
    [SLua.DoNotToLua]
    public override void Destroy()
    { 
      m_ObjectPools.Clear();
      foreach(var o in m_GameObjectPools)
        o.Value.Destroy();
      m_GameObjectPools.Clear();
      base.Destroy();
    }

    private Transform m_PoolRootObject = null;
    private Dictionary<string, object> m_ObjectPools = new Dictionary<string, object>();
    private Dictionary<string, GameObjectPool> m_GameObjectPools = new Dictionary<string, GameObjectPool>();

    private Transform PoolRootObject
    {
      get
      {
        if (m_PoolRootObject == null)
        {
          var objectPool = CloneUtils.CreateEmptyObject("GameObjectPool");
          objectPool.transform.SetParent(transform);
          objectPool.transform.localScale = Vector3.one;
          objectPool.transform.localPosition = Vector3.zero;
          m_PoolRootObject = objectPool.transform;
        }
        return m_PoolRootObject;
      }
    }

    /// <summary>
    /// 创建游戏对象池
    /// </summary>
    /// <param name="poolName">池名称</param>
    /// <param name="poolObjectPrefab">池对象的预制体</param>
    /// <param name="initCount">初始大小</param>
    /// <param name="maxSize">最大大小</param>
    /// <param name="pool">池物体挂载的根</param>
    /// <returns>返回游戏资源池</returns>
    [LuaApiDescription("创建游戏对象池", "返回游戏资源池")]
    [LuaApiParamDescription("poolName", "池名称")]
    [LuaApiParamDescription("poolObjectPrefab", "池对象的预制体")]
    [LuaApiParamDescription("initCount", "初始大小")]
    [LuaApiParamDescription("maxSize", "最大大小")]
    [LuaApiParamDescription("pool", "池物体挂载的根")]
    public GameObjectPool CreatePool(string poolName, int initSize, int maxSize, GameObject prefab)
    {
      var pool = new GameObjectPool(poolName, prefab, initSize, maxSize, PoolRootObject);
      m_GameObjectPools[poolName] = pool;
      return pool;
    }
    /// <summary>
    /// 获取指定名称游戏对象池
    /// </summary>
    /// <param name="poolName">池名称</param>
    /// <returns>返回游戏资源池，如果未找到，则返回null</returns>
    [LuaApiDescription("获取指定名称游戏对象池", "返回游戏资源池，如果未找到，则返回null")]
    [LuaApiParamDescription("poolName", "池名称")]
    public GameObjectPool GetPool(string poolName)
    {
      if (m_GameObjectPools.ContainsKey(poolName))
      {
        return m_GameObjectPools[poolName];
      }
      GameErrorChecker.LastError = GameError.NotFound;
      return null;
    }
    /// <summary>
    /// 在指定名称游戏对象池中获取可用对象
    /// </summary>
    /// <param name="poolName">池名称</param>
    /// <returns>返回游戏资源，如果未找到，或者无可用实例，则返回null</returns>
    [LuaApiDescription("在指定名称游戏对象池中获取可用对象", "返回游戏资源，如果未找到，或者无可用实例，则返回null")]
    [LuaApiParamDescription("poolName", "池名称")]
    public GameObject Get(string poolName)
    {
      GameObject result = null;
      if (m_GameObjectPools.ContainsKey(poolName))
      {
        GameObjectPool pool = m_GameObjectPools[poolName];
        result = pool.NextAvailableObject();
        if (result == null)
        {
          Log.W(TAG, "No object available in pool. Consider setting fixedSize to false.: " + poolName);
          GameErrorChecker.LastError = GameError.NotAvailable;
        }
      }
      else
      {
        Log.E(TAG, "Invalid pool name specified: " + poolName);
        GameErrorChecker.LastError = GameError.NotFound;
      }
      return result;
    }
    /// <summary>
    /// 在指定名称游戏对象池中回退对象
    /// </summary>
    /// <param name="poolName">池名称</param>
    /// <param name="go">对象</param>
    [LuaApiDescription("在指定名称游戏对象池中回退对象", "")]
    [LuaApiParamDescription("poolName", "池名称")]
    [LuaApiParamDescription("go", "对象")]
    public void Release(string poolName, GameObject go)
    {
      if (m_GameObjectPools.ContainsKey(poolName))
      {
        GameObjectPool pool = m_GameObjectPools[poolName];
        pool.ReturnObjectToPool(poolName, go);
      }
      else
      {
        Log.W(TAG, "No pool available with name: " + poolName);
        GameErrorChecker.LastError = GameError.NotFound;
      }
    }

    //自定义池相关方法
    ///-----------------------------------------------------------------------------------------------

    /// <summary>
    /// 创建指定类型的资源池。
    /// </summary>
    /// <typeparam name="T">指定类型</typeparam>
    /// <param name="actionOnGet">获取回调</param>
    /// <param name="actionOnRelease">释放回调</param>
    [SLua.DoNotToLua]
    public ObjectPool<T> CreatePool<T>(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease) where T : class
    {
      var type = typeof(T);
      var pool = new ObjectPool<T>(actionOnGet, actionOnRelease);
      m_ObjectPools[type.Name] = pool;
      return pool;
    }
    /// <summary>
    /// 获取指定类型的资源池。
    /// </summary>
    /// <typeparam name="T">指定类型</typeparam>
    [SLua.DoNotToLua]
    public ObjectPool<T> GetPool<T>() where T : class
    {
      var type = typeof(T);
      ObjectPool<T> pool = null;
      if (m_ObjectPools.ContainsKey(type.Name))
      {
        pool = m_ObjectPools[type.Name] as ObjectPool<T>;
      }
      return pool;
    }
    /// <summary>
    /// 在指定类型的资源池中获取可用资源。
    /// </summary>
    /// <typeparam name="T">指定类型</typeparam>
    [SLua.DoNotToLua]
    public T Get<T>() where T : class
    {
      var pool = GetPool<T>();
      if (pool != null)
      {
        return pool.Get();
      }
      return default(T);
    }
    /// <summary>
    /// 释放指定类型的资源池中的指定对象。
    /// </summary>
    /// <param name="obj">指定对象</param>
    /// <typeparam name="T">指定类型</typeparam>
    [SLua.DoNotToLua]
    public void Release<T>(T obj) where T : class
    {
      var pool = GetPool<T>();
      if (pool != null)
      {
        pool.Release(obj);
      }
    }

  }
}