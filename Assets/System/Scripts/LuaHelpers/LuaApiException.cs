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
 */


[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class LuaApiException : Attribute
{
  readonly string nameString;
  readonly string explanString;

  public LuaApiException(string nameString, string explanString = "")
  {
    this.nameString = nameString;
    this.explanString = explanString;
  }

  public string NameString
  {
    get { return nameString; }
  }
  public string ExplanString
  {
    get { return explanString; }
  }  
}
