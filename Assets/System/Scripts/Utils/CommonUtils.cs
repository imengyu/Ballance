using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* CommonUtils.cs
* 
* 用途：
* 通用帮助类
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 通用帮助类
  /// </summary>
  [JSExport]
  public class CommonUtils
  {
    private static System.Random random = new System.Random();
    private static int idPool = 1000;
    private static int idPoolRm = 1000;
    private static int idPoolSq = 0;

    /// <summary>
    /// 生成随机ID
    /// </summary>
    /// <returns></returns>
    public static int GenRandomID()
    {
      return random.Next(128) * idPoolRm++ / random.Next(idPoolRm, idPoolRm + random.Next(16));
    }
    /// <summary>
    /// 生成自增长ID
    /// </summary>
    /// <returns></returns>
    public static int GenAutoIncrementID()
    {
      return idPoolSq++;
    }
    /// <summary>
    /// 生成不重复ID
    /// </summary>
    /// <returns></returns>
    public static int GenNonDuplicateID()
    {
      return idPool++;
    }

    /// <summary>
    /// 生成 min-max 的随机数
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns></returns>
    public static float RandomFloat(float min, float max)
    {
      return UnityEngine.Random.Range(min, max);
    }
    /// <summary>
    /// 生成 0-max 的随机数
    /// </summary>
    /// <param name="max">最大值</param>
    /// <returns></returns>
    public static float RandomFloat(float max)
    {
      return UnityEngine.Random.Range(0, max);
    }

    /// <summary>
    /// 检查数组是否为空
    /// </summary>
    /// <param name="arr">要检查的数组</param>
    /// <returns>如果数组为null或长度为0，则返回true，否则返回false</returns>
    public static bool IsArrayNullOrEmpty(object[] arr)
    {
      return (arr == null || arr.Length == 0);
    }
    /// <summary>
    /// 检查 Dictionary 是否为空
    /// </summary>
    /// <param name="arr">要检查的 Dictionary</param>
    /// <returns>如果Dictionary为null或长度为0，则返回true，否则返回false</returns>
    public static bool IsDictionaryNullOrEmpty(IDictionary arr)
    {
      return (arr == null || arr.Keys.Count == 0);
    }
    /// <summary>
    /// 生成相同的字符串数组
    /// </summary>
    /// <param name="val">字符串</param>
    /// <param name="count">数组长度</param>
    /// <returns></returns>
    public static string[] GenSameStringArray(string val, int count)
    {
      string[] arr = new string[count];
      for (int i = 0; i < count; i++)
        arr[i] = val;
      return arr;
    }
    /// <summary>
    /// 检查可变参数
    /// </summary>
    /// <param name="param">可变参数数组</param>
    /// <param name="index">要检查的参数索引</param>
    /// <param name="typeName">目标类型</param>
    /// <returns>检查类型一致则返回true，否则返回false</returns>
    public static bool CheckParam(object[] param, int index, string typeName)
    {
      if (param.Length <= index)
        return false;

      return param.GetType().Name == typeName;
    }

    /// <summary>
    /// 获取 Dictionary 里的string值数组（低性能！）
    /// </summary>
    /// <param name="keyValuePairs">Dictionary</param>
    /// <returns></returns>
    public static string[] GetStringArrayFromDictionary(Dictionary<string, string> keyValuePairs)
    {
      return GetArrayFromDictionary(keyValuePairs);
    }
    /// <summary>
    /// 获取 Dictionary 里的值数组（低性能！）
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="keyValuePairs">Dictionary</param>
    /// <returns></returns>
    public static T[] GetArrayFromDictionary<T>(Dictionary<string, T> keyValuePairs)
    {
      Dictionary<string, T>.ValueCollection ts = keyValuePairs.Values;
      return ts.ToList<T>().ToArray();
    }
    /// <summary>
    /// 更改颜色Alpha值，其他不变
    /// </summary>
    /// <param name="o">原颜色</param>
    /// <param name="a">Alpha值</param>
    /// <returns>新生成的颜色对象</returns>
    public static Color ChangeColorAlpha(Color o, float a)
    {
      return new Color(o.r, o.g, o.b, a);
    }

    /// <summary>
    /// 计算两个物体的距离
    /// </summary>
    /// <param name="o1">物体1</param>
    /// <param name="o2">物体2</param>
    /// <returns></returns>
    public static float DistanceBetweenTwoObjects(GameObject o1, GameObject o2)
    {
      return (o1.transform.position - o2.transform.position).sqrMagnitude;
    }
    /// <summary>
    /// 计算两个坐标的距离
    /// </summary>
    /// <param name="o1">坐标1</param>
    /// <param name="o2">坐标2</param>
    /// <returns></returns>
    public static float DistanceBetweenTwoPos(Vector3 o1, Vector3 o2)
    {
      return (o1 - o2).sqrMagnitude;
    }

    public static bool IsObjectNull(UnityEngine.Object o)
    {
      return o == null;
    }

    /// <summary>
    /// 判断 Vector3 是有效
    /// </summary>
    /// <param name="v">对象</param>
    /// <returns></returns>
    public static bool IsValid(Vector3 v)
    {
      return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z);
    }
    /// <summary>
    /// 判断 Vector2 是否有效
    /// </summary>
    /// <param name="v">对象</param>
    /// <returns></returns>
    public static bool IsValid(Vector2 v)
    {
      return !float.IsNaN(v.x) && !float.IsNaN(v.y);
    }
  }
}
