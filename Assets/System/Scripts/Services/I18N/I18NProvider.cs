using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* I18NProvider.cs
* 
* 用途：
* 国际化支持类，用于加载并处理国际化字符串资源以及读取本地化字符串。
*
* 作者：
* mengyu
*/

namespace Ballance2.Services.I18N
{
  /// <summary>
  /// 国际化字符串提供类
  /// </summary>
  public static class I18NProvider
  {
    private const string TAG = "I18NProvider";

    private static SystemLanguage currentLanguage = SystemLanguage.ChineseSimplified;
    public static Dictionary<string, string> FallbackLanguageValues { get; } = new Dictionary<string, string>();
    public static Dictionary<string, string> LanguageValues { get; } = new Dictionary<string, string>();
    public static Dictionary<SystemLanguage, string> AdditionalLanguageFiles { get; } = new Dictionary<SystemLanguage, string>();
    public static Dictionary<string, Func<string, string>> SettingStringReplacer { get; } = new Dictionary<string, Func<string, string>>();

    //由GameManager调用

    internal static void ClearAllLanguageResources()
    {
      currentLanguage = SystemLanguage.ChineseSimplified;
      LanguageValues.Clear();
    }
    internal static void LoadLanguageFile()
    {
      var files = new DirectoryInfo(Application.streamingAssetsPath + "/Languages").GetFiles("*.xml", SearchOption.TopDirectoryOnly);
      foreach (var file in files)
      {
        var xml = new XmlDocument();
        var fileContent = File.ReadAllText(file.FullName);
        xml.LoadXml(fileContent);
        if (xml.DocumentElement != null && xml.DocumentElement.Name == "I18n" && xml.DocumentElement.ChildNodes.Count > 0)
        {
          var child = xml.DocumentElement.ChildNodes[0];
          if (child.Name == "Language" && child.ChildNodes.Count > 0 && child.Attributes["name"] != null)
          {
            if (Enum.TryParse<SystemLanguage>(child.Attributes["name"].Value, out var result))
            {
              if (result == currentLanguage)
                LoadLanguageResources(fileContent);
              if (!AdditionalLanguageFiles.ContainsKey(result))
                AdditionalLanguageFiles.Add(result, child.ChildNodes[0].InnerText);
            }
          }
        }
      }
    }

    private static void LoadLanguageNodeChild(Dictionary<string, string> dict, XmlNode nodeLanguage, string prefix) 
    {
      //Search Text node
      foreach (XmlElement nodeText in nodeLanguage.ChildNodes)
      {
        var nodeTextName = nodeText.Attributes["name"];
        if (nodeTextName != null && !string.IsNullOrEmpty(nodeTextName.Value) && !string.IsNullOrEmpty(nodeText.InnerXml))
        {
          var path = prefix + (string.IsNullOrEmpty(prefix) ? "" : ".") + nodeTextName.Value;
          if (nodeText.Name == "Text")
            dict[path.ToUpper()] = nodeText.InnerText;
          else if (nodeText.Name == "Group")
            LoadLanguageNodeChild(dict, nodeText, path);
        }
      }
    }

    private static int StringBuilderIndexOf(StringBuilder sb, string value, int startIndex = 0, bool ignoreCase = false)
    {
      int len = value.Length;
      int max = (sb.Length - len) + 1;
      var v1 = (ignoreCase)
          ? value.ToLower() : value;
      var func1 = (ignoreCase)
          ? new Func<char, char, bool>((x, y) => char.ToLower(x) == y)
          : new Func<char, char, bool>((x, y) => x == y);
      for (int i1 = startIndex; i1 < max; ++i1)
        if (func1(sb[i1], v1[0]))
        {
          int i2 = 1;
          while ((i2 < len) && func1(sb[i1 + i2], v1[i2]))
            ++i2;
          if (i2 == len)
            return i1;
        }
      return -1;
    }
    private static string StringBuilderSubString(StringBuilder sb, int start, int len)
    {
      StringBuilder str = new StringBuilder(len);
      for (int i = start, j = 0; j < len && i < sb.Length; i++, j++)
        str.Append(sb[i]);
      return str.ToString();
    }
    private static string PreMatchSettingStringReplacer(string value)
    {
      const string key = "<settings-replacer=";
      const string keyend = "</settings-replacer>";
      const int maxCount = 32;

      if (!value.Contains(key))
        return value;
      var sb = new StringBuilder(value);
      var count = 0;

      while(count < maxCount)
      {
        var startIndex = StringBuilderIndexOf(sb, key);
        if (startIndex == -1)
          break;
        var tagEndIndex = StringBuilderIndexOf(sb, keyend, startIndex);
        if (tagEndIndex == -1) 
          break;
        var startEndIndex = -1;
        var endIndex = tagEndIndex + 19;

        for (int i = startIndex; i < sb.Length; i++)
          if (sb[i] == '>')
          {
            startEndIndex = i;
            break;
          }
        if (startEndIndex < startIndex)
          break;
        var valueStartIndex = startIndex + 19;
        var tag = StringBuilderSubString(sb, valueStartIndex, startEndIndex - valueStartIndex);
        var inner = StringBuilderSubString(sb, startEndIndex + 1, tagEndIndex - startEndIndex - 1);

        var result = "";
        if (SettingStringReplacer.TryGetValue(tag, out var replacer))
          result = replacer(inner);

        sb.Remove(startIndex, endIndex - startIndex + 1);
        sb.Insert(startIndex, result);

        count++;
      }

      return sb.ToString();
    }

    /// <summary>
    /// 注册字符串动态设置文字修改器
    /// </summary>
    /// <param name="key">键值，用于取消注册</param>
    /// <param name="cb">回调，用于替换字符串，参数是字符串资源中的原字符串</param>
    public static void RegisterSettingStringReplacer(string key, Func<string, string> cb)
    {
      if (SettingStringReplacer.ContainsKey(key))
        SettingStringReplacer[key] = cb;
      else
        SettingStringReplacer.Add(key, cb);
    }
    /// <summary>
    /// 取消注册字符串动态设置文字修改器
    /// </summary>
    /// <param name="key"></param>
    public static void UnRegisterSettingStringReplacer(string key)
    {
      if (SettingStringReplacer.ContainsKey(key))
        SettingStringReplacer.Remove(key);
    }

    /// <summary>
    /// 直接加载语言文件到字典中
    /// </summary>
    /// <param name="dict">目标字典</param>
    /// <param name="xmlAssets">语言文件XML</param>
    /// <returns></returns>
    public static bool DirectLoadLanguageResources(Dictionary<string, string> dict, string xmlAssets)
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlAssets);
        XmlElement root = doc.DocumentElement;
        //Search Language node
        foreach (XmlElement nodeLanguage in root.ChildNodes)
        {
          var nodeLanguageName = nodeLanguage.Attributes["name"];
          if (nodeLanguage.Name == "Language" && nodeLanguageName != null)
          {
            if (Enum.TryParse<SystemLanguage>(nodeLanguageName.Value, true, out var languageName))
            {
              if (languageName == currentLanguage)
              {
                LoadLanguageNodeChild(dict, nodeLanguage, "");
                break;
              }
              if (languageName == SystemLanguage.ChineseSimplified)
              {
                //中文作为兜底语言资源
                LoadLanguageNodeChild(FallbackLanguageValues, nodeLanguage, "");
                break;
              }
            }
          }
        }
        return true;
      }
      catch (XmlException e)
      {
        Log.E(TAG, "LoadLanguageResources failed: " + e.ToString());
      }
      return false;
    }

    /// <summary>
    /// 预加载语言定义文件
    /// </summary>
    /// <param name="xmlAssets">语言定义XML字符串</param>
    /// <returns>加载是否成功</returns>
    public static Dictionary<string, string> PreLoadLanguageResources(string xmlAssets)
    {
      Dictionary<string, string> LanguageValues = new Dictionary<string, string>();
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlAssets);
        XmlElement root = doc.DocumentElement;
        //Search Language node
        foreach (XmlElement nodeLanguage in root.ChildNodes)
        {
          var nodeLanguageName = nodeLanguage.Attributes["name"];
          if (nodeLanguage.Name == "Language" && nodeLanguageName != null)
          {
            if (nodeLanguageName.Value == currentLanguage.ToString())
            {
              LoadLanguageNodeChild(LanguageValues, nodeLanguage, "");
              break;
            }
          }
        }
        return LanguageValues;
      }
      catch (XmlException e)
      {
        Log.E(TAG, "LoadLanguageResources failed: " + e.ToString());
      }
      return null;
    }

    /// <summary>
    /// 加载语言定义文件
    /// </summary>
    /// <param name="xmlAssets">语言定义XML字符串</param>
    /// <returns>加载是否成功</returns>
    public static bool LoadLanguageResources(string xmlAssets)
    {
      return DirectLoadLanguageResources(LanguageValues, xmlAssets);
    }
    /// <summary>
    /// 加载语言定义文件
    /// </summary>
    /// <param name="xmlAssets">语言定义XML资源文件</param>
    /// <returns>加载是否成功</returns>
    public static bool LoadLanguageResources(TextAsset xmlAssets)
    {
      return LoadLanguageResources(xmlAssets.text);
    }

    /// <summary>
    /// 设置当前游戏语言。此函数只能设置语言至设置，无法立即生效，必须重启游戏才能生效。
    /// </summary>
    /// <param name="language">语言</param>
    public static void SetCurrentLanguage(SystemLanguage language)
    {
      if (currentLanguage != language)
        currentLanguage = language;
    }
    /// <summary>
    /// 获取当前游戏语言
    /// </summary>
    /// <returns></returns>  
    public static SystemLanguage GetCurrentLanguage()
    {
      return currentLanguage;
    }

    /// <summary>
    /// 获取语言字符串
    /// </summary>
    /// <param name="key">字符串键值</param>
    /// <returns>如果找到对应键值字符串，则返回字符串，否则返回null</returns>
    public static string GetLanguageString(string key, bool withFallback)
    {
      if (LanguageValues.TryGetValue(key.ToUpper(), out var s1))
        return PreMatchSettingStringReplacer(s1);
      if (withFallback && FallbackLanguageValues.TryGetValue(key.ToUpper(), out s1))
        return PreMatchSettingStringReplacer(s1);
      return null;
    }
  }
}