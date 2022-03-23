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


[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public sealed class LuaApiNoDoc : Attribute
{
  public LuaApiNoDoc()
  {
  }
}
