using System.Collections.Generic;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameStaticResourcesPool.cs
* 
* 用途：
* 游戏静态资源池。
* 可在 GameEntry 中配置静态引用资源，打包后无需加载，可使用本工具类直接获取。
* 但太多静态引用资源会导致游戏启动变慢。
*
* 作者：
* mengyu
*/

namespace Ballance2.Res
{
  /// <summary>
  /// 游戏静态资源池
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("游戏静态资源池")]
  [LuaApiNotes(@"可在 GameEntry 中配置静态引用资源，打包后无需加载，可使用本工具类直接获取。但太多静态引用资源会导致游戏启动变慢。")]
  public class GameStaticResourcesPool
  {
    public static GameObject PrefabUIEmpty { get; private set; }
    public static GameObject PrefabEmpty { get; private set; }

    /// <summary>
    /// 在静态引入资源中查找指定名称的预制体资源
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <returns>如果找到指定预制体资源，则返回预制体实例，否则返回null</returns>
    [LuaApiDescription("在静态引入资源中查找指定名称的预制体资源", "如果找到指定预制体资源，则返回预制体实例，否则返回null")]
    [LuaApiParamDescription("name", "资源名称")]
    public static GameObject FindStaticPrefabs(string name)
    {
      if (GamePrefab == null)
        return null;
      foreach (GameObjectInfo gameObjectInfo in GamePrefab)
      {
        if (gameObjectInfo.Name == name)
          return gameObjectInfo.Object;
      }
      return null;
    }
    /// <summary>
    /// 在静态引入资源中查找指定名称的资源
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <returns如果找到指定资源，则返回资源实例，否则返回null></returns>
    public static T FindStaticAssets<T>(string name) where T : UnityEngine.Object
    {
      if (GameAssets == null)
        return null;
      foreach (GameAssetsInfo gameAssetsInfo in GameAssets)
      {
        if (gameAssetsInfo.Name == name)
          return (T)gameAssetsInfo.Object;
      }
      return null;
    }
    /// <summary>
    /// 在静态引入资源中查找指定名称的资源
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <returns>如果找到指定资源，则返回资源实例，否则返回null</returns>
    [LuaApiDescription("在静态引入资源中查找指定名称的资源", "如果找到指定资源，则返回资源实例，否则返回null")]
    [LuaApiParamDescription("name", "资源名称")]
    public static Object FindStaticAssets(string name)
    {
      if (GameAssets == null)
        return null;
      foreach (GameAssetsInfo gameAssetsInfo in GameAssets)
      {
        if (gameAssetsInfo.Name == name)
          return gameAssetsInfo.Object;
      }
      return null;
    }

    internal static void InitStaticPrefab(List<GameObjectInfo> gamePrefab, List<GameAssetsInfo> gameAssets)
    {
      GamePrefab = gamePrefab;
      GameAssets = gameAssets;

      PrefabEmpty = FindStaticPrefabs("PrefabEmpty");
      PrefabUIEmpty = FindStaticPrefabs("PrefabUIEmpty");
    }
    internal static void ReleaseAll()
    {
      PrefabUIEmpty = null;
      PrefabEmpty = null;
      GamePrefab = null;
      GameAssets = null;
    }

    /// <summary>
    /// 静态引入 Prefab
    /// </summary>
    [LuaApiDescription("静态引入 Prefab")]
    public static List<GameObjectInfo> GamePrefab { get; private set; }
    /// <summary>
    /// 静态引入资源
    /// </summary>
    [LuaApiDescription("静态引入资源")]
    public static List<GameAssetsInfo> GameAssets { get; private set; }
  }
}
