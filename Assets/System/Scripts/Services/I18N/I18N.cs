
/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* I18N.cs
* 
* 用途：
* I18N 工具类，可快速获取当前游戏语言的对应本地化字符串。
*
* 作者：
* mengyu
*/

namespace Ballance2.Services.I18N
{
  /// <summary>
  /// 国际化字符串提供类，可快速获取当前游戏语言的对应本地化字符串。
  /// </summary>
  [JSExport]
  public static class I18N
  {
    /// <summary>
    /// 获取国际化字符串
    /// </summary>
    /// <param name="key">字符串键</param>
    /// <returns>返回找到的字符串，如果未找到，则返回 [Key xxx not found!]</returns>
    public static string Tr(string key)
    {
      var str = I18NProvider.GetLanguageString(key);
      if (str != null)
        return str;
      return string.Format("[Key {0} not found!]", key);
    }
    /// <summary>
    /// 获取国际化字符串
    /// </summary>
    /// <param name="key">字符串键</param>
    /// <param name="defaultString">如果没有找到指定的字符串国际化信息，则返回此默认字符串</param>
    /// <returns></returns>
    public static string Tr(string key, string defaultString)
    {
      var str = I18NProvider.GetLanguageString(key);
      if (str != null)
        return str;
      return default;
    }
    /// <summary>
    /// 获取国际化字符串并自定义格式化参数
    /// </summary>
    /// <param name="key">字符串键</param>
    /// <param name="formatParams">要自定义格式化的参数</param>
    /// <returns></returns>
    public static string TrF(string key, params object[] formatParams)
    {
      var str = I18NProvider.GetLanguageString(key);
      if (str != null)
        return string.Format(str, formatParams);
      return string.Format("[Key {0} not found!]", key);
    }
    /// <summary>
    /// 获取国际化字符串并自定义格式化参数
    /// </summary>
    /// <param name="key">字符串键</param>
    /// <param name="defaultString">如果没有找到指定的字符串国际化信息，则使用此此默认字符串代替</param>
    /// <param name="formatParams">要自定义格式化的参数</param>
    /// <returns></returns>
    public static string TrF(string key, string defaultString, params object[] formatParams)
    {
      var str = I18NProvider.GetLanguageString(key);
      return string.Format(str != null ? str : defaultString, formatParams);
    }
  }
}