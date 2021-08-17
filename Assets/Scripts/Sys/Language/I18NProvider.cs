using System.Collections.Generic;
using System.Xml;
using Ballance2.LuaHelpers;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.Sys.Language
{
    [SLua.CustomLuaClass]
    [LuaApiDescription("国际化支持类")]
    public static class I18NProvider
    {
        private const string TAG = "I18NProvider";
        private class I18NLanguagePack {
            public SystemLanguage Language;
            public Dictionary<string, string> LanguageValues = new Dictionary<string, string>();
        }
        
        private static SystemLanguage currentLanguage = SystemLanguage.ChineseSimplified;
        private static I18NLanguagePack currentLanguagePack = null;

        private static Dictionary<SystemLanguage, I18NLanguagePack> languagePacks = new Dictionary<SystemLanguage, I18NLanguagePack>();

        private static I18NLanguagePack GetOrAddLanguagePack(SystemLanguage language) {
            if(languagePacks.TryGetValue(language, out var v)) return v;
            else {
                v = new I18NLanguagePack();
                v.Language = language;
                languagePacks[language] = v;
                return v;
            }
        }

        public static void ClearAllLanguageResources() {
            currentLanguage = SystemLanguage.ChineseSimplified;
            currentLanguagePack = null;
            foreach(var v in languagePacks)
                v.Value.LanguageValues.Clear();
            languagePacks.Clear();
        }

        [LuaApiDescription("加载语言定义文件")]
        [LuaApiParamDescription("xmlAssets", "语言定义XML文件")]
        public static bool LoadLanguageResources(string xmlAssets) {
            try {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlAssets);
                XmlElement root = doc.DocumentElement;
                //Search Language node
                foreach(XmlElement nodeLanguage in root.ChildNodes) {
                    var nodeLanguageName = nodeLanguage.Attributes["name"];
                    if(nodeLanguage.Name == "Language" && nodeLanguageName != null) {
                        if(System.Enum.TryParse<SystemLanguage>(nodeLanguageName.Value, true, out var languageName)) {
                            var languagePack = GetOrAddLanguagePack(languageName);
                            //Search Text node
                            foreach(XmlElement nodeText in nodeLanguage.ChildNodes) {
                                var nodeTextName = nodeText.Attributes["name"];
                                if(nodeText.Name == "Text" && nodeTextName != null 
                                    && !string.IsNullOrEmpty(nodeTextName.Value) && !string.IsNullOrEmpty(nodeText.InnerXml)) {
                                    languagePack.LanguageValues[nodeTextName.Value] = nodeText.InnerXml;
                                }
                            }
                        } else
                            Log.E(TAG, "Language name value \"" + nodeLanguageName.Value + "\" is not a valid language type name.");
                    }
                }
                return true;
            } catch(XmlException e) {
                Log.E(TAG, "LoadLanguageResources failed: " + e.ToString());
            }
            return false;
        }
        [LuaApiDescription("加载语言定义文件")]
        [LuaApiParamDescription("xmlAssets", "语言定义XML资源文件")]
        public static bool LoadLanguageResources(TextAsset xmlAssets) {
            return LoadLanguageResources(xmlAssets.text);
        }

        public static void SetCurrentLanguage(SystemLanguage language) {
            if(currentLanguage != language) {
                currentLanguage = language;
            }
            currentLanguagePack = GetOrAddLanguagePack(language);
        }
        public static SystemLanguage GetCurrentLanguage() {
            return currentLanguage;
        }      

        [LuaApiDescription("获取语言字符串，使用当前系统语言")]
        [LuaApiParamDescription("key", "字符串键值")]
        public static string GetLanguageString(string key) {
            return GetLanguageString(key, currentLanguage);
        }
        [LuaApiDescription("加载语言定义文件")]
        [LuaApiParamDescription("xmlAssets", "语言定义XML文件")]
        public static string GetLanguageString(string key, SystemLanguage lang) {
            if(lang == currentLanguage)
                if(currentLanguagePack != null && currentLanguagePack.LanguageValues.TryGetValue(key, out var s)) return s;
            else {
                var pack = GetOrAddLanguagePack(lang);
                if(pack.LanguageValues.TryGetValue(key, out var s1)) return s1;
            }
            return null;
        }
    }
}