using System;
using System.Text;
using System.Text.RegularExpressions;
using Ballance.LuaHelpers;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* StringUtils.cs
* 
* 用途：
* 字符串工具类
*
* 作者：
* mengyu
*
* 更改历史：
* 2018-8-11 创建
*
*/

namespace Ballance2.Utils
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("")]
    public static class StringUtils
    {
        /// <summary>
        /// 检测字符串是否为空
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        /// <returns></returns>
        [LuaApiDescription("检测字符串是否为空", "")]
        [LuaApiParamDescription("text", "要检测的字符串")]
        public static bool isNullOrEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }
        /// <summary>
        /// 检测字符串是否为空或空白
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        /// <returns></returns>
        [LuaApiDescription("检测字符串是否为空或空白", "")]
        [LuaApiParamDescription("text", "要检测的字符串")]
        public static bool IsNullOrWhiteSpace(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
        /// <summary>
        /// 检测字符串是否是URL
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        /// <returns></returns>
        [LuaApiDescription("检测字符串是否是URL", "")]
        [LuaApiParamDescription("text", "要检测的字符串")]
        public static bool IsUrl(string text)
        {
            return !isNullOrEmpty(text) && Regex.IsMatch(text, "(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
        }
        /// <summary>
        /// 检测字符串是否是整数
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        /// <returns></returns>
        [LuaApiDescription("检测字符串是否是整数", "")]
        [LuaApiParamDescription("text", "要检测的字符串")]
        public static bool IsNumber(string text)
        {
            return !isNullOrEmpty(text) && Regex.IsMatch(text, "^-?[1-9]\\d*$");
        }
        /// <summary>
        /// 检测字符串是否是浮点数
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        /// <returns></returns>
        [LuaApiDescription("检测字符串是否是浮点数", "")]
        [LuaApiParamDescription("text", "要检测的字符串")]
        public static bool IsFloatNumber(string text)
        {
            return !isNullOrEmpty(text) && Regex.IsMatch(text, "^(-?\\d+)(\\.\\d+)?");
        }
        /// <summary>
        /// 检测字符串是否是包名
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        /// <returns></returns>
        [LuaApiDescription("检测字符串是否是包名", "")]
        [LuaApiParamDescription("text", "要检测的字符串")]
        public static bool IsPackageName(string text)
        {
            if (isNullOrEmpty(text)) return false;
            return Regex.IsMatch(text, "^([a-zA-Z]+[.][a-zA-Z]+)[.]*.*");
        }
        /// <summary>
        /// 比较两个版本字符串的先后
        /// </summary>
        /// <param name="version1">版本1</param>
        /// <param name="version2">版本2</param>
        /// <returns>1 小于 2 返回 -1 ，大于返回 1，等于返回 0</returns>
        [LuaApiDescription("比较两个版本字符串的先后", "1 小于 2 返回 -1 ，大于返回 1，等于返回 0")]
        [LuaApiParamDescription("version1", "版本1")]
        [LuaApiParamDescription("version2", "版本2")]
        public static int CompareTwoVersion(string version1, string version2)
        {
            if (version1 == version2) return 0;
            long ver1 = 0, ver2 = 0;

            string[] vf = version1.Split('.');
            int v = 0;
            for (int i = 0; i < vf.Length; i++)
            {
                if(int.TryParse(vf[i], out v))
                    ver1 += v * (int)Math.Pow(10, i);
            }
            vf = version2.Split('.');
            for (int i = 0; i < vf.Length; i++)
            {
                if (int.TryParse(vf[i], out v))
                    ver2 += v * (int)Math.Pow(10, i);
            }

            if (ver1 == ver2) return 0;
            return ver1 < ver2 ? -1 : 1;
        }
        /// <summary>
        /// 替换字符串的 &lt;br&gt; 转为换行符
        /// </summary>
        /// <param name="str">要替换的字符串</param>
        /// <returns></returns>
        [LuaApiDescription("替换字符串的 <br> 转为换行符", "")]
        [LuaApiParamDescription("str", "要替换的字符串")]
        public static string ReplaceBrToLine(string str)
        {
            return str.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n"); ;
        }
        /// <summary>
        /// 颜色字符串转为 Color
        /// </summary>
        /// <param name="color">要转换的颜色字符串,支持Color中定义的颜色名称，或者是#ffffff格式或者是255,255,255格式的颜色数值字符串</param>
        /// <returns></returns>
        [LuaApiDescription("颜色字符串转为 Color", "")]
        [LuaApiParamDescription("color", "要转换的颜色字符串,支持Color中定义的颜色名称，或者是#ffffff格式或者是255,255,255格式的颜色数值字符串")]
        public static Color StringToColor(string color)
        {
            switch(color)
            {
                case "black": return Color.black;
                case "blue": return Color.blue;
                case "clear": return Color.clear;
                case "cyan": return Color.cyan;
                case "gray": return Color.gray;
                case "green": return Color.green;
                case "magenta": return Color.magenta;
                case "red": return Color.red;
                case "white": return Color.white;
                case "yellow": return Color.yellow;
                default:
                    Color nowColor;
                    if (ColorUtility.TryParseHtmlString(color, out nowColor))
                        return nowColor;
                    else if(color.Contains(","))
                    {
                        string[] s = color.Split(',');
                        if (s.Length >= 3)
                        {
                            int r, g, b, a = 255;
                            int.TryParse(s[0], out r);
                            int.TryParse(s[1], out g);
                            int.TryParse(s[2], out b);

                            if (s.Length >= 4) int.TryParse(s[2], out a);

                            nowColor = new Color((float)r / 255, (float)g / 255, (float)b / 255, (float)a / 255);
                            return nowColor;
                        }
                    }
                    break;
            }
            return Color.black;
        }
        /// <summary>
        /// 尝试把字符串数组转为参数数组
        /// </summary>
        /// <param name="arr">要转换的字符串数组</param>
        /// <returns></returns>
        [LuaApiDescription("尝试把字符串数组转为参数数组", "")]
        [LuaApiParamDescription("arr", "要转换的字符串数组")]
        public static object[] TryConvertStringArrayToValueArray(string[] arr)
        {
            return TryConvertStringArrayToValueArray(arr, 0);
        }

        /// <summary>
        /// 尝试把字符串数组转为参数数组
        /// </summary>
        /// <param name="arr">要转换的字符串数组</param>
        /// <param name="startIndex">数组转换起始索引</param>
        /// <returns></returns>
        [LuaApiDescription("尝试把字符串数组转为参数数组", "")]
        [LuaApiParamDescription("arr", "要转换的字符串数组")]
        [LuaApiParamDescription("startIndex", "数组转换起始索引")]
        public static object[] TryConvertStringArrayToValueArray(string[] arr, int startIndex)
        {
            object[] rs = new object[arr.Length];
            if(arr==null) return rs;

            for (int i = startIndex, ix = 0; i < arr.Length; i++, ix++)
                rs[ix] = TryConvertStringToValue(arr[i]);
            
            return rs;
        }
        
        /// <summary>
        /// 尝试转换字符串为参数
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <returns>如果转换失败则返回原字符串</returns>
        [LuaApiDescription("尝试转换字符串为参数", "如果转换失败则返回原字符串")]
        [LuaApiParamDescription("arr", "要转换的字符串")]
        public static object TryConvertStringToValue(string value)
        {
            object rs = null;
            if (IsFloatNumber(value))
            {
                float v = 0; 
                if (float.TryParse(value, out v)) rs = v; 
            }
            else if (IsNumber(value))
            {
                int v = 0;
                if (int.TryParse(value, out v)) rs = v;
            }
            else if (value.ToLower() == "true")
                rs = true;
            else if (value.ToLower() == "false")
                rs = false;
            else
                rs = value;
            
            return rs;
        }

        /// <summary>
        /// 尝试把参数数组数组转为字符串
        /// </summary>
        /// <param name="arr">要转换的参数数组</param>
        /// <returns></returns>
        [LuaApiDescription("尝试把参数数组数组转为字符串", "")]
        [LuaApiParamDescription("arr", "要转换的参数数组")]
        public static string ValueArrayToString(object[] arr)
        {
            StringBuilder sb = new StringBuilder();
            if (arr == null)
                sb.Append("null");
            else
            {
                foreach (object o in arr)
                {
                    sb.Append("\n");
                    if (o == null) sb.Append("null");
                    else sb.Append(o.ToString());
                }
            }
            return sb.ToString() ;
        }


        /// <summary>
        /// 比较Bytes
        /// </summary>
        /// <param name="inV">bytes数组1</param>
        /// <param name="outV">bytes数组2</param>
        /// <returns>返回两个Bytes是否相等</returns>
        [LuaApiDescription("比较Bytes", "返回两个Bytes是否相等")]
        [LuaApiParamDescription("inV", "bytes数组1")]
        [LuaApiParamDescription("outV", "bytes数组2")]
        public static bool TestBytesMatch(byte[] inV, byte[] outV)
        {
            bool rs = true;
            if (inV != null && outV != null)
            {
                if (inV.Length == outV.Length)
                {
                    for (int i = 0, c = outV.Length; i < c; i++)
                    {
                        if (inV[i] != outV[i])
                        {
                            rs = false;
                            break;
                        }
                    }
                }
            }
            return rs;
        }

        /// <summary>
        /// 修复UTF8的BOM头
        /// </summary>
        /// <param name="buffer">内容数组</param>
        /// <returns>返回处理完成的字符串</returns>
        [LuaApiDescription("修复UTF8的BOM头", "返回处理完成的字符串")]
        [LuaApiParamDescription("buffer", "内容数组")]
        public static string FixUtf8BOM(byte[] buffer)
        {
            string xmlStr;
            byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };
            if (buffer.Length > 3 && buffer[0] == bomBuffer[0]
                && buffer[1] == bomBuffer[1]
                && buffer[2] == bomBuffer[2])
            {
                xmlStr = Encoding.UTF8.GetString(buffer, 3, buffer.Length - 3);
            }
            else
            {
                xmlStr = Encoding.UTF8.GetString(buffer);
            }
            return xmlStr;
        }
    }
}
