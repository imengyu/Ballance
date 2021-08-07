using Ballance.LuaHelpers;

namespace Ballance2.Sys.Language
{
    [SLua.CustomLuaClass]
    [LuaApiDescription("国际化字符串提供类")]
    public static class I18N
    {       
        [LuaApiDescription("获取国际化字符串", "返回找到的字符串，如果未找到，则返回 [Key xxx not found!]")]
        [LuaApiParamDescription("key", "字符串键")]
        public static string Tr(string key) {
            var str = I18NProvider.GetLanguageString(key);
            if(str != null)
                return str;
            return string.Format("[Key {0} not found!]", key);
        }
        [LuaApiDescription("获取国际化字符串")]
        [LuaApiParamDescription("key", "字符串键")]
        [LuaApiParamDescription("defaultString", "如果没有找到指定的字符串国际化信息，则返回此默认字符串")]
        public static string Tr(string key, string defaultString) {
            var str = I18NProvider.GetLanguageString(key);
            if(str != null)
                return str;
            return default;
        }     
        [LuaApiDescription("获取国际化字符串并自定义格式化参数")]
        [LuaApiParamDescription("key", "字符串键")]
        [LuaApiParamDescription("formatParams", "要自定义格式化的参数")]
        public static string TrF(string key, params object[] formatParams) {
            var str = I18NProvider.GetLanguageString(key);
            if(str != null)
                return string.Format(str, formatParams);
            return string.Format("[Key {0} not found!]", key);
        }
        [LuaApiDescription("获取国际化字符串并自定义格式化参数")]
        [LuaApiParamDescription("key", "字符串键")]
        [LuaApiParamDescription("defaultString", "如果没有找到指定的字符串国际化信息，则使用此此默认字符串")]
        [LuaApiParamDescription("formatParams", "要自定义格式化的参数")]
        public static string TrF(string key, string defaultString, params object[] formatParams) {
            var str = I18NProvider.GetLanguageString(key);
            return string.Format(str != null ? str : defaultString, formatParams);
        }
    }
}