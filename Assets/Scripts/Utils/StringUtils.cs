using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Ballance2.Utils
{
    [SLua.CustomLuaClass]
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool isNullOrEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }
        /// <summary>
        /// 字符串是否为空或空白
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
        /// <summary>
        /// 字符串是否是URL
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsUrl(string text)
        {
            return Regex.IsMatch(text, "(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
        }
        /// <summary>
        /// 字符串是否是整数
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string text)
        {
            return Regex.IsMatch(text, "^-?[1-9]\\d*$");
        }
        /// <summary>
        /// 字符串是否是浮点数
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsFloatNumber(string text)
        {
            return Regex.IsMatch(text, "^(-?\\d+)(\\.\\d+)?");
        }
        /// <summary>
        /// 字符串是否是包名
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static bool IsPackageName(string text)
        {
            if (isNullOrEmpty(text)) return false;
            return Regex.IsMatch(text, "^([a-zA-Z]+[.][a-zA-Z]+)[.]*.*");
        }
        /// <summary>
        /// 比较两个版本先后，1 小于 2 返回 -1 ，大于返回 1，等于返回 0
        /// </summary>
        /// <param name="version1">版本1</param>
        /// <param name="version2">版本2</param>
        /// <returns></returns>
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
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ReplaceBrToLine(string str)
        {
            return str.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n"); ;
        }
        /// <summary>
        /// 颜色字符串转为 Color
        /// </summary>
        /// <param name="color">颜色字符串</param>
        /// <returns></returns>
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
                            int r = 0, g = 0, b = 0, a = 255;
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
        /// <param name="arr">字符串数组</param>
        /// <returns></returns>
        public static object[] TryConvertStringArrayToValueArray(string[] arr)
        {
            object[] rs = new object[arr.Length];
            if(arr==null) return rs;

            for (int i = 0; i < arr.Length; i++)
            {
                string s = arr[i];
                if (IsFloatNumber(s))
                {
                    if (s.EndsWith("f")) { float v = 0; if (float.TryParse(s, out v)) rs[i] = v; }
                    else { double v = 0; if (double.TryParse(s, out v)) rs[i] = v; }
                }
                else if (IsNumber(s))
                {
                    int v = 0;
                    if (int.TryParse(s, out v)) rs[i] = v;
                }
                else if (s.ToLower() == "true")
                    rs[i] = true;
                else if (s.ToLower() == "false")
                    rs[i] = false;
                else
                    rs[i] = s;
            }
            return rs;
        }

        /// <summary>
        /// 尝试把参数数组数组转为字符串
        /// </summary>
        /// <param name="arr">参数数组</param>
        /// <returns></returns>
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
    }
}
