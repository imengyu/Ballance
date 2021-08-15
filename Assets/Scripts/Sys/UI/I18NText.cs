using Ballance2.Sys.Language;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ballance2.Sys.UI
{
    [SLua.CustomLuaClass]
    [ExecuteInEditMode]
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