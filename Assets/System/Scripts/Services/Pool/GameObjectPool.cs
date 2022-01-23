
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Services.Pool
{
  [Serializable]
  public class PoolInfo
  {
    public string poolName;
    public GameObject prefab;
    public int poolSize;
    public bool fixedSize;
  }

  /// <summary>
  /// 游戏对象池
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("游戏对象池")]
  public class GameObjectPool
  {
    private string TAG {
      get {
        return "GameObjectPool:" + poolName;
      }
    } 

    private int maxSize;
    private int poolSize;
    private string poolName;
    private Transform poolRoot;
    private GameObject poolObjectPrefab;
    private Stack<GameObject> availableObjStack = new Stack<GameObject>();

    /// <summary>
    /// 释放
    /// </summary>
    [SLua.DoNotToLua]
    public void Destroy() {
      availableObjStack.Clear();
      poolObjectPrefab = null;
    }

    /// <summary>
    /// 创建游戏对象池
    /// </summary>
    /// <param name="poolName">池名称</param>
    /// <param name="poolObjectPrefab">池的预制体</param>
    /// <param name="initCount">初始大小</param>
    /// <param name="maxSize">最大大小</param>
    /// <param name="pool">池物体挂载的根</param>
    [SLua.DoNotToLua]
    public GameObjectPool(string poolName, GameObject poolObjectPrefab, int initCount, int maxSize, Transform pool)
    {
      this.poolName = poolName;
      this.poolSize = initCount;
      this.maxSize = maxSize;
      this.poolRoot = pool;
      this.poolObjectPrefab = poolObjectPrefab;

      //populate the pool
      for (int index = 0; index < initCount; index++)
      {
        AddObjectToPool(NewObjectInstance());
      }
    }

    //o(1)
    private void AddObjectToPool(GameObject go)
    {
      //add to pool
      go.SetActive(false);
      availableObjStack.Push(go);
      go.transform.SetParent(poolRoot, false);
    }
    private GameObject NewObjectInstance()
    {
      return GameObject.Instantiate(poolObjectPrefab) as GameObject;
    }
    
    /// <summary>
    /// 获取一个可用的物体
    /// </summary>
    /// <returns>游戏物体实例，如果没有可用物体，则返回空</returns>
    [LuaApiDescription("获取一个可用的物体", "游戏物体实例，如果没有可用物体，则返回空")]
    public GameObject NextAvailableObject()
    {
      GameObject go = null;
      if (availableObjStack.Count > 0)
      {
        go = availableObjStack.Pop();
      }
      else
      {
        Log.W(TAG, "No object available & cannot grow pool: " + poolName);
      }
      if(go != null)
        go.SetActive(true);
      return go;
    }

    /// <summary>
    /// 回退当前池所控根的指定名称子物体至池中
    /// </summary>
    /// <param name="objName">对象名称</param>
    [LuaApiDescription("回退当前池所控根的指定名称子物体至池中")]
    [LuaApiParamDescription("objName", "对象名称")]
    public void ReturnObjectFromParent(string objName)
    {
      var obj = this.poolRoot.Find(objName);
      if(obj != null)
        AddObjectToPool(obj.gameObject);
      else
        Log.E(TAG, string.Format("Not found the pool child object {0}.", objName));
    }
    /// <summary>
    /// 获取当前池所控根的是否存在指定名称子物体
    /// </summary>
    /// <param name="objName">对象名称</param>
    [LuaApiDescription("获取当前池所控根的是否存在指定名称子物体")]
    [LuaApiParamDescription("objName", "对象名称")]
    public bool ContainsObjectInParent(string objName)
    {
      return this.poolRoot.Find(objName) != null;
    }

    /// <summary>
    /// 回退物体至池中，注意，只能回退当前池所控根的子物体
    /// </summary>
    /// <param name="po">物体</param>
    [LuaApiDescription("回退物体至池中，注意，只能回退当前池所控根的子物体")]
    [LuaApiParamDescription("po", "物体")]
    public void ReturnObjectToPool(GameObject po)
    {
      if(po.transform.parent == this.poolRoot)
        AddObjectToPool(po);
      else
        Log.E(TAG, string.Format("Trying to add object to incorrect pool {0}, it parent {1} not this pool root.", poolName, po.transform.parent.name));
    }
    
    //o(1)
    /// <summary>
    /// 回退物体至池中
    /// </summary>
    /// <param name="pool">池的名称</param>
    /// <param name="po">物体</param>
    [LuaApiDescription("回退物体至池中")]
    [LuaApiParamDescription("pool", "池的名称")]
    [LuaApiParamDescription("po", "物体")]
    public void ReturnObjectToPool(string pool, GameObject po)
    {
      if (poolName.Equals(pool))
      {
        AddObjectToPool(po);
      }
      else
      {
        Log.E(TAG, string.Format("Trying to add object to incorrect pool {0} ", poolName));
      }
    }
  }
}