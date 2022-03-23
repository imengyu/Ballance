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
public sealed class LuaApiNotes : Attribute
{
  readonly string noteString;
  readonly string demoString;
  readonly string addAtString;

  public LuaApiNotes(string noteString, string demoString = "", string addAtString = "")
  {
    this.noteString = noteString;
    this.demoString = demoString;
    this.addAtString = addAtString;
  }

  public string DemoString
  {
    get { return demoString; }
  }
  public string NoteString
  {
    get { return noteString; }
  }  
  public string AddAtString
  {
    get { return addAtString; }
  }
}
