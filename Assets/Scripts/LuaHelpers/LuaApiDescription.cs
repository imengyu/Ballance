using System;

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
 * 更改历史：
 * 2021-4-19 创建
 *
 */

namespace Ballance.LuaHelpers
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class LuaApiDescription : Attribute
    {
        readonly string descriptionString;
        readonly string returnString;
        
        public LuaApiDescription(string descriptionString, string returnString = "")
        {
            this.descriptionString = descriptionString;
            this.returnString = returnString;
        }
        
        public string DescriptionString
        {
            get { return descriptionString; }
        }
        public string ReturnString
        {
            get { return returnString; }
        }
    }
}