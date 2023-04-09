/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameSingletonManager.cs
 *
 * 用途：
 * 单例对象基类，用于管理一些单例.
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Base
{
  /// <summary>
  /// 单例对象基类
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class GameSingletonManager<T> where T : class
  {
    public static T Instance = null;

    public GameSingletonManager() {
      if (Instance != null)
        throw new System.Exception("The Singleton " + typeof(T).Name + " already exists!");
      Instance = this as T;
    }
  }
}