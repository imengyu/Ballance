/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameSingletonBehavior.cs
 *
 * 用途：
 * 单例对象基类，用于管理一些单例.
 * 
 * 作者：
 * mengyu
 */
using UnityEngine;

namespace Ballance2.Base
{
  /// <summary>
  /// 单例对象基类
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class GameSingletonBehavior<T> : MonoBehaviour where T : class
  {
    public static T Instance;

    public GameSingletonBehavior() 
    {
/*#if UNITY_EDITOR
      if (Instance != null)
        Debug.LogWarning("You re try to create two GameSingletonBehavior instance, this may be a mistake.");
#endif*/
      Instance = this as T;
    }

    protected virtual void OnDestroy() {
      Instance = null;
    }
  }
}