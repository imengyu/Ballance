using System.Collections.Generic;
using UnityEngine;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* ObjectStateBackupUtils.cs
* 
* 用途：
* 对象变换状态保存器。
* Virtools 中有一个叫做IC的功能，可以保存物体的初始状态，设置了IC，可以很
* 方便的恢复物体的初始状态，Ballance中很多模块需要IC的功能以重复恢复初始状态，
* 因此设计了此对象变换状态保存器。
* 目前可保存物体的旋转与位置信息，可选保存单个物体，或是对象和他的一级子对象，
* 可满足大部分使用需求。
*
* 作者：
* mengyu
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 对象变换状态保存器
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("对象变换状态保存器")]
  [LuaApiNotes(@"Virtools 中有一个叫做IC的功能，可以保存物体的初始状态，设置了IC，可以很方便的恢复物体的初始状态，Ballance中很多模块需要IC的功能以重复恢复初始状态，因此设计了此对象变换状态保存器。

目前可保存物体的旋转与位置信息，可选保存单个物体，或是对象和他的一级子对象，可满足大部分使用需求。
  ", @"例如，在初始化的时候使用 `BackUpObject` 保存当前物体状态：
```lua
BackUpObject(self.gameObject)
```
然后，在需要恢复当前物体状态时可以调用 `RestoreObject` 恢复：
```lua
RestoreObject(self.gameObject)
```
")]
  public static class ObjectStateBackupUtils
  {
    private struct ObjectStateBackup
    {
      public Vector3 Pos;
      public Quaternion Rot;
    }
    private static Dictionary<int, ObjectStateBackup> objectBackup = new Dictionary<int, ObjectStateBackup>();

    // 由 GameManager 调用。
    [LuaApiDescription("由 `GameManager` 调用。手动调用将清空所有信息。")]
    public static void ClearAll()
    {
      objectBackup.Clear();
    }

    /// <summary>
    /// 清除对象的备份
    /// </summary>
    /// <param name="gameObject">要操作的游戏对象</param>
    [LuaApiDescription("清除对象的备份")]
    [LuaApiParamDescription("gameObject", "要操作的游戏对象")]
    public static void ClearObjectBackUp(GameObject gameObject)
    {
      objectBackup.Remove(gameObject.GetInstanceID());
    }

    /// <summary>
    /// 备份对象的变换状态
    /// </summary>
    /// <param name="gameObject">要备份的游戏对象</param>
    [LuaApiDescription("备份对象的变换状态")]
    [LuaApiParamDescription("gameObject", "要备份的游戏对象")]
    public static void BackUpObject(GameObject gameObject)
    {
      var key = gameObject.GetInstanceID();
      if (!objectBackup.TryGetValue(key, out var st))
        st = new ObjectStateBackup();

      st.Pos = gameObject.transform.position;
      st.Rot = gameObject.transform.rotation;

      objectBackup[key] = st;
    }

    /// <summary>
    /// 备份对象和他的一级子对象的变换状态
    /// </summary>
    /// <param name="gameObject">要备份的游戏对象</param>
    [LuaApiDescription("备份对象和他的一级子对象的变换状态")]
    [LuaApiParamDescription("gameObject", "要备份的游戏对象")]
    public static void BackUpObjectAndChilds(GameObject gameObject)
    {
      BackUpObject(gameObject);
      for (int i = 0; i < gameObject.transform.childCount; i++)
        BackUpObject(gameObject.transform.GetChild(i).gameObject);
    }

    /// <summary>
    /// 从备份还原对象的变换状态
    /// </summary>
    /// <param name="gameObject">要还原的游戏对象</param>
    [LuaApiDescription("从备份还原对象的变换状态")]
    [LuaApiParamDescription("gameObject", "要还原的游戏对象")]
    public static void RestoreObject(GameObject gameObject)
    {
      var key = gameObject.GetInstanceID();
      if (objectBackup.TryGetValue(key, out var st))
      {
        gameObject.transform.position = st.Pos;
        gameObject.transform.rotation = st.Rot;
      }
    }

    /// <summary>
    /// 从备份还原对象和他的一级子对象的变换状态
    /// </summary>
    /// <param name="gameObject">要还原的游戏对象</param>
    [LuaApiDescription("从备份还原对象和他的一级子对象的变换状态")]
    [LuaApiParamDescription("gameObject", "要还原的游戏对象")]
    public static void RestoreObjectAndChilds(GameObject gameObject)
    {
      RestoreObject(gameObject);
      for (int i = 0; i < gameObject.transform.childCount; i++)
        RestoreObject(gameObject.transform.GetChild(i).gameObject);
    }

  }
}