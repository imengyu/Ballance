using System.Collections.Generic;
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
    private static Dictionary<string, string> LanguageValues = new Dictionary<string, string>();

    //由GameManager调用
    
    public static void ClearAllLanguageResources()
    {
      currentLanguage = SystemLanguage.ChineseSimplified;
      LanguageValues.Clear();
    }

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
            if (System.Enum.TryParse<SystemLanguage>(nodeLanguageName.Value, true, out var languageName) && languageName == currentLanguage)
            {
              //Search Text node
              foreach (XmlElement nodeText in nodeLanguage.ChildNodes)
              {
                var nodeTextName = nodeText.Attributes["name"];
                if (nodeText.Name == "Text" && nodeTextName != null
                    && !string.IsNullOrEmpty(nodeTextName.Value) && !string.IsNullOrEmpty(nodeText.InnerXml))
                {
                  dict[nodeTextName.Value] = nodeText.InnerXml;
                }
              }
              break;
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
    /// 加载语言定义文件
    /// </summary>
    /// <param name="xmlAssets">语言定义XML字符串</param>
    /// <returns>加载是否成功</returns>
    public static bool LoadLanguageResources(string xmlAssets)
    {
      return DirectLoadLanguageResources(LanguageValues, xmlAssets);
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
              //Search Text node
              foreach (XmlElement nodeText in nodeLanguage.ChildNodes)
              {
                var nodeTextName = nodeText.Attributes["name"];
                if (nodeText.Name == "Text" && nodeTextName != null
                    && !string.IsNullOrEmpty(nodeTextName.Value) && !string.IsNullOrEmpty(nodeText.InnerXml))
                {
                  LanguageValues[nodeTextName.Value] = nodeText.InnerXml;
                }
              }
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
    public static string GetLanguageString(string key)
    {
      if(LanguageValues.TryGetValue(key, out var s1))
        return s1;
      return null;
    }
  }
}