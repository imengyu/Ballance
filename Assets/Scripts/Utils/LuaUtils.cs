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
        /// <summary>
        /// 将参数LuaTable自动转为c#类型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 检查参数是不是LuaTable
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool CheckParamIsLuaTable(object[] param)
        {
            return param != null && param.Length == 1 && param[0] != null && param[0].GetType() == typeof(LuaTable);
        }
        /// <summary>
        /// 自动检查参数是不是LuaTable并自动转为c#类型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object[] AutoCheckParamIsLuaTableAndConver(object[] param)
        {
            if(CheckParamIsLuaTable(param))
                return LuaTableArrayToObjectArray(param);
            return param;
        }
        public static LuaTable ToLuaTableForCS(object param)
        {
            return param as LuaTable;
        }
        public static LuaFunction ToLuaFunctionForCS(object param)
        {
            return param as LuaFunction;
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
        [LuaApiDescription("字符串转为布尔值")]
        [LuaApiParamDescription("param", "字符串 \"true\" 或者 \"false\"")]
        public static bool StringToBool(string param)
        {
            return param == "true";
        }
        [LuaApiDescription("字符串转为KeyCode")]
        [LuaApiParamDescription("param", "字符串")]
        public static KeyCode StringToKeyCode(string param)
        {
           return (KeyCode)System.Enum.Parse(typeof(KeyCode), param);
        }

        [LuaApiDescription("Lua按位与函数")]
        public static int And(int a, int b)
        {
            return a & b;
        }       
        [LuaApiDescription("Lua按位或函数")]
        public static int Or(int a, int b)
        {
            return a | b;
        }  
        [LuaApiDescription("Lua按位异或函数")]
        public static int Xor(int a, int b)
        {
            return a ^ b;
        }  
        [LuaApiDescription("Lua按位非函数")]
        public static int Not(int a)
        {
            return ~a ;
        } 
        [LuaApiDescription("Lua按左移函数")]
        public static int LeftMove(int a, int b)
        {
            return a << b;
        }  
        [LuaApiDescription("Lua按右移函数")]
        public static int RightMove(int a, int b)
        {
            return a >> b;
        }  
    }
    
}
