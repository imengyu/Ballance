using Ballance.LuaHelpers;
using SLua;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* LuaUtils.cs
* 
* 用途：
* Lua 工具类
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
    /// Lua 工具类
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("Lua 工具类")]
    public class LuaUtils
    {
        public static object[] LuaTableArrayToObjectArray(object[] param)
        {
            if (param == null)
                return null;
            if (param.Length == 1 && param[0] is LuaTable)
            {
                LuaTable arr = param[0] as LuaTable;
                object[] arrFixed = new object[arr.length()];
                for (int i = 0, c = arrFixed.Length; i < c; i++)
                    arrFixed[i] = arr[i + 1];
                return arrFixed;
            }
            else
                return param;
        }
        public static bool CheckParamIsLuaTable(object[] param)
        {
            return param != null && param.Length == 1 && param[0] != null && param[0].GetType() == typeof(LuaTable);
        }
        /// <summary>
        /// 布尔值转为字符串
        /// </summary>
        /// <param name="param">布尔值</param>
        /// <returns>字符串 "true" 或者 "false"</returns>
        [LuaApiDescription("布尔值转为字符串", "字符串 \"true\" 或者 \"false\"")]
        [LuaApiParamDescription("param", "布尔值")]
        public static string BooleanToString(bool param)
        {
            return param ? "true" : "false";
        }

        public static bool StringToBool(string param)
        {
            return param == "true";
        }

        public static KeyCode StringToKeyCode(string param)
        {
           return (KeyCode)System.Enum.Parse(typeof(KeyCode), param);
        }

        public static LuaTable ToLuaTableForCS(object param)
        {
            return param as LuaTable;
        }
        public static LuaFunction ToLuaFunctionForCS(object param)
        {
            return param as LuaFunction;
        }

    }
    
}
