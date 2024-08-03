using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 字符串工具类
  /// </summary>
  /// <remarks>
  /// 提供了许多字符串处理工具函数。
  /// </remarks>
  public static class StringUtils
  {
    /// <summary>
    /// 检测字符串是否为空
    /// </summary>
    /// <param name="text">要检测的字符串</param>
    /// <returns>如果字符串是否为空或者 null，则返回 true，否则返回 false</returns>
    public static bool isNullOrEmpty(string text)
    {
      return string.IsNullOrEmpty(text);
    }
    /// <summary>
    /// 检测字符串是否为空或空白
    /// </summary>
    /// <param name="text">要检测的字符串</param>
    /// <returns>如果字符串是否为空、空白，或者 null，则返回 true，否则返回 false</returns>
    public static bool IsNullOrWhiteSpace(string text)
    {
      return string.IsNullOrWhiteSpace(text);
    }
    /// <summary>
    /// 检测字符串是否是URL
    /// </summary>
    /// <param name="text">要检测的字符串</param>
    /// <returns>如果是一个有效的 URL, 则返回 true，否则返回 false</returns>
    public static bool IsUrl(string text)
    {
      return !isNullOrEmpty(text) && Regex.IsMatch(text, "(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
    }
    /// <summary>
    /// 检测字符串是否是整数
    /// </summary>
    /// <param name="text">要检测的字符串</param>
    /// <returns>检测字符串是否是整数", "如果是一个有效的整数字符串, 则返回 true，否则返回 false</returns>
    public static bool IsNumber(string text)
    {
      return !isNullOrEmpty(text) && Regex.IsMatch(text, "^-?[1-9]\\d*$");
    }
    /// <summary>
    /// 检测字符串是否是浮点数
    /// </summary>
    /// <param name="text">要检测的字符串</param>
    /// <returns>如果是一个有效的浮点数字符串, 则返回 true，否则返回 false</returns>
    public static bool IsFloatNumber(string text)
    {
      return !isNullOrEmpty(text) && Regex.IsMatch(text, "^(-?\\d+)(\\.\\d+)?");
    }
    /// <summary>
    /// 检测字符串是否是包名
    /// </summary>
    /// <param name="text">要检测的字符串</param>
    /// <returns>如果是一个有效的包名字符串, 则返回 true，否则返回 false</returns>
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
    public static int CompareTwoVersion(string version1, string version2)
    {
      if (version1 == version2) return 0;
      long ver1 = 0, ver2 = 0;

      string[] vf = version1.Split('.');
      int v = 0;
      for (int i = 0; i < vf.Length; i++)
      {
        if (int.TryParse(vf[i], out v))
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
    /// <returns>经过处理的字符串</returns>
    public static string ReplaceBrToLine(string str)
    {
      return str.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n"); ;
    }
    /// <summary>
    /// 颜色字符串转为 Color
    /// 
    /// <br/>转换的颜色字符串支持 Color 中定义的颜色名称，如下：
    /// <br/>* black ： Color.black
    /// <br/>* blue ： Color.blue
    /// <br/>* clear ： Color.clear
    /// <br/>* cyan ： Color.cyan
    /// <br/>* gray ： Color.gray
    /// <br/>* green ： Color.green
    /// <br/>* magenta ： Color.magenta
    /// <br/>* red ： Color.red
    /// <br/>* white ： Color.white
    /// <br/>* yellow ： Color.yellow
    /// <br/>或者是十六进制颜色字符串，例如 `#ffffff` 格式，或者是 `255,255,255` 格式的颜色数值字符串。
    /// </summary>
    /// <param name="color">要转换的颜色字符串</param>
    /// <returns>返回转换的颜色，如果转换失败，默认返回 Color.black</returns>
    public static Color StringToColor(string color)
    {
      switch (color)
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
          else if (color.Contains(","))
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
    /// <remarks>
    /// 此函数只能转换基本数据类型：float int bool string。
    /// </remarks>
    /// <param name="arr">要转换的字符串数组</param>
    /// <returns>转换的参数数组</returns>
    public static object[] TryConvertStringArrayToValueArray(string[] arr)
    {
      return TryConvertStringArrayToValueArray(arr, 0);
    }
    /// <summary>
    /// 尝试把字符串数组转为参数数组
    /// </summary>
    /// <remarks>
    /// 此函数只能转换基本数据类型：float int bool string。
    /// </remarks>
    /// <param name="arr">要转换的字符串数组</param>
    /// <param name="startIndex">数组转换起始索引</param>
    /// <returns>转换的参数数组</returns>
    public static object[] TryConvertStringArrayToValueArray(string[] arr, int startIndex)
    {
      object[] rs = new object[arr.Length];
      if (arr == null) return rs;

      for (int i = startIndex, ix = 0; i < arr.Length; i++, ix++)
        rs[ix] = TryConvertStringToValue(arr[i]);

      return rs;
    }

    /// <summary>
    /// 尝试转换字符串为参数
    /// </summary>
    /// <remarks>
    /// 此函数只能转换基本数据类型：float int bool string。
    /// </remarks>
    /// <param name="value">要转换的字符串</param>
    /// <returns>如果转换失败则返回原字符串</returns>
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
    /// 尝试把参数数组转为字符串
    /// </summary>
    /// <param name="arr">要转换的参数</param>
    /// <returns>转换的字符串</returns>
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
      return sb.ToString();
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
    /// <summary>
    /// 比较Bytes (指定长度)
    /// </summary>
    /// <param name="inV">bytes数组1</param>
    /// <param name="outV">bytes数组2</param>
    /// <returns>返回两个Bytes是否相等</returns>
    public static bool TestBytesMatchN(byte[] inV, byte[] outV, int len)
    {
      bool rs = true;
      if (inV != null && outV != null)
      {
        for (int i = 0, c = outV.Length; i < c && i < len; i++)
        {
          if (inV[i] != outV[i])
          {
            rs = false;
            break;
          }
        }
      }
      return rs;
    }

    /// <summary>
    /// 修复UTF8的BOM头
    /// </summary>
    /// <param name="buffer">内容数组</param>
    /// <returns>返回处理完成的数组</returns>
    public static byte[] FixUtf8BOM(byte[] buffer)
    {
      byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };
      if (
        buffer.Length > 3
        && TestBytesMatchN(buffer, bomBuffer, 3)
      )
        return buffer.Skip(3).ToArray();
      return buffer;
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    /// <param name="buffer">字符串</param>
    /// <returns>返回MD5值</returns>
    public static string MD5String(string input)
    {
      byte[] inputBytes = Encoding.UTF8.GetBytes(input);
      return MD5(inputBytes);
    }
    /// <summary>
    /// 计算字节数组的MD5值
    /// </summary>
    /// <param name="inputBytes">字节数组</param>
    /// <returns>返回MD5</returns>
    public static string MD5(byte[] inputBytes)
    {
      // Step 1, calculate MD5 hash from input
      MD5 md5 = System.Security.Cryptography.MD5.Create();
      byte[] hashBytes = md5.ComputeHash(inputBytes);

      // Step 2, convert byte array to hex string
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < hashBytes.Length; i++)
      {
        sb.Append(hashBytes[i].ToString("X2"));
      }
      return sb.ToString();
    }

    /// <summary>
    /// Encoding.ASCII.GetString 函数包装
    /// </summary>
    /// <param name="inputBytes">字节数组</param>
    /// <returns>返回解码的字符串</returns>
    public static string GetASCIIBytes(byte[] inputBytes)
    {
      return Encoding.ASCII.GetString(inputBytes);
    }
    /// <summary>
    /// Encoding.UTF8.GetString 函数包装
    /// </summary>
    /// <param name="inputBytes">字节数组</param>
    /// <returns>返回解码的字符串</returns>
    public static string GetUtf8Bytes(byte[] inputBytes)
    {
      return Encoding.UTF8.GetString(inputBytes);
    }
    /// <summary>
    /// Encoding.Unicode.GetString 函数包装
    /// </summary>
    /// <param name="inputBytes">字节数组</param>
    /// <returns>返回解码的字符串</returns>
    public static string GetUnicodeBytes(byte[] inputBytes)
    {
      return Encoding.Unicode.GetString(inputBytes);
    }
    /// <summary>
    /// Encoding.ASCII.GetBytes 函数包装
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>返回字节数组</returns>
    public static byte[] StringToASCIIBytes(string input)
    {
      return Encoding.ASCII.GetBytes(input);
    }
    /// <summary>
    /// Encoding.UTF8.GetBytes 函数包装
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>返回字节数组</returns>
    public static byte[] StringToUtf8Bytes(string input)
    {
      return Encoding.UTF8.GetBytes(input);
    }
    /// <summary>
    /// Encoding.Unicode.GetBytes 函数包装
    /// </summary>
    /// <param name="input">输入字符串</param>
    /// <returns>返回字节数组</returns>
    public static byte[] StringToUnicodeBytes(string input)
    {
      return Encoding.Unicode.GetBytes(input);
    }
    /// <summary>
    /// 在 input 查找 find ，如果找到，则从input找到find的末尾位置截取input至结尾。 
    /// </summary>
    /// <param name="input">要截取的字符串</param>
    /// <param name="find">要查找的字符串</param>
    /// <returns></returns>
    public static string RemoveStringByStringStart(string input, string find)
    {
      int index = input.IndexOf(find);
      if(index > 0) 
        return input.Substring(index + find.Length);
      return input;
    }

    public const string SpeicalPattern = "[\n`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}]";

    /// <summary>
    /// 检测字符串中是否存在特殊字符
    /// </summary>
    /// <param name="input">要截取的字符串</param>
    /// <returns></returns>
    public static bool ContainsSpeicalChars(string input)
    {
      return Regex.IsMatch(input, SpeicalPattern);
    }
    /// <summary>
    /// 替换字符串中的特殊字符
    /// </summary>
    /// <param name="input">要截取的字符串</param>
    /// <returns></returns>
    public static string RemoveSpeicalChars(string input)
    {
      return Regex.Replace(input, SpeicalPattern, "");
    }

  }
}
