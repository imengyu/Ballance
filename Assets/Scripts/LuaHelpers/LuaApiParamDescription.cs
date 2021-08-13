
/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * LuaApiDescription.cs
 *
 * 用途：
 * lua api 注释属性
 * 
 * 作者：
 * mengyu
 * 
 *
 */

namespace Ballance2.LuaHelpers
{
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LuaApiParamDescription : System.Attribute
    {
        readonly string descriptionString;
        readonly string paramName;
        
        public LuaApiParamDescription(string paramName, string descriptionString)
        {
            this.descriptionString = descriptionString;
            this.paramName = paramName;
        }      
        public string DescriptionString
        {
            get { return descriptionString; }
        }
        public string ParamName
        {
            get { return paramName; }
        }
    }
}