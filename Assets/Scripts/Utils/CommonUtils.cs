using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ballance.LuaHelpers;
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
* 
* 
*
*/

namespace Ballance2.Utils
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("通用帮助类")]
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
        [LuaApiDescription("生成随机ID", "")]
        [LuaApiParamDescription("", "")]
        public static int GenRandomID()
        {
            return random.Next(128) * idPoolRm++ / random.Next(idPoolRm, idPoolRm + random.Next(16));
        }
        /// <summary>
        /// 生成自增长ID
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("生成自增长ID", "")]
        [LuaApiParamDescription("", "")]
        public static int GenAutoIncrementID()
        {
            return idPoolSq++;
        }
        /// <summary>
        /// 生成不重复ID
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("生成不重复ID", "")]
        public static int GenNonDuplicateID()
        {
            return idPool++;
        }
        /// <summary>
        /// 检查数组是否为空
        /// </summary>
        /// <param name="arr">要检查的数组</param>
        /// <returns>如果数组为null或长度为0，则返回true，否则返回false</returns>
        [LuaApiDescription("检查数组是否为空", "如果数组为null或长度为0，则返回true，否则返回false")]
        [LuaApiParamDescription("arr", "要检查的数组")]
        public static bool IsArrayNullOrEmpty(object [] arr)
        {
            return (arr == null || arr.Length == 0); 
        }
        /// <summary>
        /// 检查 Dictionary 是否为空
        /// </summary>
        /// <param name="arr">要检查的 Dictionary</param>
        /// <returns>如果Dictionary为null或长度为0，则返回true，否则返回false</returns>
        [LuaApiDescription("检查 Dictionary 是否为空", "如果Dictionary为null或长度为0，则返回true，否则返回false")]
        [LuaApiParamDescription("arr", "要检查的 Dictionary")]
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
        [LuaApiDescription("生成相同的字符串数组", "")]
        [LuaApiParamDescription("val", "字符串")]
        [LuaApiParamDescription("count", "数组长度")]
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
        [LuaApiDescription("检查可变参数", "检查类型一致则返回true，否则返回false")]
        [LuaApiParamDescription("param", "可变参数数组")]
        [LuaApiParamDescription("index", "要检查的参数索引")]
        [LuaApiParamDescription("typeName", "目标类型")]
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
        [LuaApiDescription("获取 Dictionary 里的string值数组（低性能！）", "")]
        [LuaApiParamDescription("keyValuePairs", "Dictionary")]
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
        [LuaApiDescription("获取 Dictionary 里的值数组（低性能！）", "")]
        [LuaApiParamDescription("keyValuePairs", "Dictionary")]
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
        [LuaApiDescription("更改颜色Alpha值，其他不变", "新生成的颜色对象")]
        [LuaApiParamDescription("o", "原颜色")]
        [LuaApiParamDescription("a", "Alpha值")]
        public static Color ChangeColorAlpha(Color o, float a)
        {
            return new Color(o.r, o.g, o.b, a);
        }
    }
}
