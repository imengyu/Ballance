using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ballance2.Utils
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    [SLua.CustomLuaClass]
    public class CommonUtils
    {
        private static Random random = new Random();
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
        /// 检查数组是否为空
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static bool IsArrayNullOrEmpty(object [] arr)
        {
            return (arr == null || arr.Length == 0); 
        }
        /// <summary>
        /// 检查 Dictionary 是否为空
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static bool IsDictionaryNullOrEmpty(IDictionary arr)
        {
            return (arr == null || arr.Keys.Count == 0);
        }
        /// <summary>
        /// 生成相同的字符串数组
        /// </summary>
        /// <param name="val">字符串</param>
        /// <param name="count">长度</param>
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
        /// <returns></returns>
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
    }
}
