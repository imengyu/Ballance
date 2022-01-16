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
  [SLua.CustomLuaClass]
  public static class I18NProvider
  {
    private const string TAG = "I18NProvider";
    private class I18NLanguagePack
    {
      public SystemLanguage Language;
      public Dictionary<string, string> LanguageValues = new Dictionary<string, string>();
    }

    private static SystemLanguage currentLanguage = SystemLanguage.ChineseSimplified;
    private static I18NLanguagePack currentLanguagePack = null;

    private static Dictionary<SystemLanguage, I18NLanguagePack> languagePacks = new Dictionary<SystemLanguage, I18NLanguagePack>();

    private static I18NLanguagePack GetOrAddLanguagePack(SystemLanguage language)
    {
      if (languagePacks.TryGetValue(language, out var v)) return v;
      else
      {
        v = new I18NLanguagePack();
        v.Language = language;
        languagePacks[language] = v;
        return v;
      }
    }

    //由GameManager调用
    public static void ClearAllLanguageResources()
    {
      currentLanguage = SystemLanguage.ChineseSimplified;
      currentLanguagePack = null;
      foreach (var v in languagePacks)
        v.Value.LanguageValues.Clear();
      languagePacks.Clear();
    }

    /// <summary>
    /// 加载语言定义文件
    /// </summary>
    /// <param name="xmlAssets">语言定义XML字符串</param>
    /// <returns>加载是否成功</returns>
    public static bool LoadLanguageResources(string xmlAssets)
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
            if (System.Enum.TryParse<SystemLanguage>(nodeLanguageName.Value, true, out var languageName))
            {
              var languagePack = GetOrAddLanguagePack(languageName);
              //Search Text node
              foreach (XmlElement nodeText in nodeLanguage.ChildNodes)
              {
                var nodeTextName = nodeText.Attributes["name"];
                if (nodeText.Name == "Text" && nodeTextName != null
                    && !string.IsNullOrEmpty(nodeTextName.Value) && !string.IsNullOrEmpty(nodeText.InnerXml))
                {
                  languagePack.LanguageValues[nodeTextName.Value] = nodeText.InnerXml;
                }
              }
            }
            else
              Log.E(TAG, "Language name value \"" + nodeLanguageName.Value + "\" is not a valid language type name.");
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
    /// <param name="xmlAssets">语言定义XML资源文件</param>
    /// <returns>加载是否成功</returns>
    public static bool LoadLanguageResources(TextAsset xmlAssets)
    {
      return LoadLanguageResources(xmlAssets.text);
    }

    /// <summary>
    /// 设置当前游戏语言
    /// </summary>
    /// <param name="language">语言</param>
    public static void SetCurrentLanguage(SystemLanguage language)
    {
      if (currentLanguage != language)
      {
        currentLanguage = language;
      }
      currentLanguagePack = GetOrAddLanguagePack(language);
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
    /// 使用当前系统语言获取语言字符串
    /// </summary>
    /// <param name="key">字符串键值</param>
    /// <returns>如果找到对应键值字符串，则返回字符串，否则返回null</returns>
    public static string GetLanguageString(string key)
    {
      return GetLanguageString(key, currentLanguage);
    }
    /// <summary>
    /// 使用指定语言获取语言字符串
    /// </summary>
    /// <param name="key">字符串键值</param>
    /// <param name="lang">指定语言</param>
    /// <returns>如果找到对应键值字符串，则返回字符串，否则返回null</returns>
    public static string GetLanguageString(string key, SystemLanguage lang)
    {
      if (lang == currentLanguage)
        if (currentLanguagePack != null && currentLanguagePack.LanguageValues.TryGetValue(key, out var s)) return s;
        else
        {
          var pack = GetOrAddLanguagePack(lang);
          if (pack.LanguageValues.TryGetValue(key, out var s1)) return s1;
        }
      return null;
    }
  }
}