/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* ConverUtils.cs
* 
* 用途：
* 字符串转换类
*
* 作者：
* mengyu
*
*/

using System;

namespace Ballance2.Utils
{
  /// <summary>
  /// 字符串转换类
  /// </summary>
  [JSExport]
  public static class ConverUtils
  {
    private static readonly string TAG = "ConverUtils";

    private static void LogErr(string paramName, string stringValue, string valType, string defaultValue)
    {
      Log.W(TAG, "Failed to parse param {0} because \"{1}\" is not a {2} value, use default value {3} insted.",
              paramName, stringValue, valType, defaultValue);
    }

    /// <summary>
    /// 字符串转为枚举
    /// </summary>
    /// <param name="stringValue">要转换的字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="paramName">参数名称（用于错误日志）</param>
    /// <returns>转换成功的枚举，如果输入字符串无法转为枚举，则返回默认值</returns>
    public static T StringToEnum<T>(string stringValue, T defaultValue, string paramName = "") where T : struct
    {
      if (string.IsNullOrEmpty(stringValue))
        return defaultValue;
      T outValue;
      if (!Enum.TryParse<T>(stringValue, out outValue))
      {
        if (!string.IsNullOrEmpty(paramName))
          LogErr(paramName, stringValue, typeof(T).Name, defaultValue.ToString());
        outValue = defaultValue;
      }
      return outValue;
    }
    /// <summary>
    /// 字符串转为数字
    /// </summary>
    /// <param name="stringValue">要转换的字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="paramName">参数名称（用于错误日志）</param>
    /// <returns>转换成功的数字，如果输入字符串无法转为数字，则返回默认值</returns>
    public static int StringToInt(string stringValue, int defaultValue, string paramName = "")
    {
      if (string.IsNullOrEmpty(stringValue))
        return defaultValue;
      int outValue;
      if (!int.TryParse(stringValue, out outValue))
      {
        if (!string.IsNullOrEmpty(paramName))
          LogErr(paramName, stringValue, "int", defaultValue.ToString());
        outValue = defaultValue;
      }
      return outValue;
    }
    /// <summary>
    /// 字符串转为长整型数字
    /// </summary>
    /// <param name="stringValue">要转换的字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="paramName">参数名称（用于错误日志）</param>
    /// <returns>转换成功的长整型数字，如果输入字符串无法转为长整型数字，则返回默认值</returns>
    public static long StringToLong(string stringValue, long defaultValue, string paramName = "")
    {
      if (string.IsNullOrEmpty(stringValue))
        return defaultValue;
      long outValue;
      if (!long.TryParse(stringValue, out outValue))
      {
        if (!string.IsNullOrEmpty(paramName))
          LogErr(paramName, stringValue, "long", defaultValue.ToString());
        outValue = defaultValue;
      }
      return outValue;
    }
    /// <summary>
    /// 字符串转为布尔类型
    /// </summary>
    /// <param name="stringValue">要转换的字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="paramName">参数名称（用于错误日志）</param>
    /// <returns>转换成功的布尔类型，如果输入字符串无法转为布尔类型，则返回默认值</returns>
    public static bool StringToBoolean(string stringValue, bool defaultValue, string paramName = "")
    {
      if (string.IsNullOrEmpty(stringValue))
        return defaultValue;
      bool outValue;
      if (stringValue.ToLower() == "true" || stringValue == "1")
        return true;
      if (!bool.TryParse(stringValue, out outValue))
      {
        if (!string.IsNullOrEmpty(paramName))
          LogErr(paramName, stringValue, "bool", defaultValue.ToString());
        outValue = defaultValue;
      }
      return outValue;
    }
    /// <summary>
    /// 字符串转为浮点数
    /// </summary>
    /// <param name="stringValue">要转换的字符串</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="paramName">参数名称（用于错误日志）</param>
    /// <returns>转换成功的浮点数，如果输入字符串无法转为浮点数，则返回默认值</returns>
    public static float StringToFloat(string stringValue, float defaultValue, string paramName = "")
    {
      if (string.IsNullOrEmpty(stringValue))
        return defaultValue;
      float outValue;
      if (!float.TryParse(stringValue, out outValue))
      {
        if (!string.IsNullOrEmpty(paramName))
          LogErr(paramName, stringValue, "float", defaultValue.ToString());
        outValue = defaultValue;
      }
      return outValue;
    }
  }
}
