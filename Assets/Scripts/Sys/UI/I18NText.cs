using Ballance2.Sys.Language;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Progress.cs
* 
* 用途：
* 一个自动加载I18N字符串的文字组件。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// 一个自动加载I18N字符串的文字组件。
    /// </summary>
    [SLua.CustomLuaClass]
    [ExecuteInEditMode]
    [Ballance2.LuaHelpers.LuaApiDescription("一个自动加载I18N字符串的文字组件")]
    [AddComponentMenu("Ballance/UI/Controls/I18NText")]
    public class I18NText : Text
    {
        [SerializeField]
        private string m_ResourceKey = "";
        [SerializeField]
        private string m_DefaultString = "";
        [SerializeField]
        private bool m_I18N = true;

        public string resourceKey {
            get {
                return m_ResourceKey;
            }
            set {
                m_ResourceKey = value;
                UpdateText();
            }
        }
        public string defaultString {
            get {
                return m_DefaultString;
            }
            set {
                m_DefaultString = value;
                UpdateText();
            }
        }
        public bool i18N {
            get {
                return m_I18N;
            }
            set {
                m_I18N = value;
                UpdateText();
            }
        }

        private new void Start() {
            base.Start();
            UpdateText();
        }
        private string GetDefaultString() {
            return string.IsNullOrEmpty(defaultString) ? resourceKey : defaultString;
        }
        public void UpdateText() {
            #if UNITY_EDITOR
            if(EditorApplication.isPlaying)
                text = (i18N && m_ResourceKey != "") ? 
                    I18N.Tr(resourceKey, defaultString) : GetDefaultString();
            else
                text = defaultString;
            #else
            text = (i18N && m_ResourceKey != "") ? 
                I18N.Tr(resourceKey, defaultString) : GetDefaultString();
            #endif
        }
    }
}