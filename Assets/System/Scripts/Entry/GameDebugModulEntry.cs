using Ballance2.Config;
using Ballance2.Services.Debug;
using Ballance2.Services.Init;
using Ballance2.Services.InputManager;
using Ballance2.Tests;
using Ballance2.UI.CoreUI;
using SubjectNerd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* GameDebugModulEntry.cs
* 
* 用途：
* 机关调试场景的配置。
*
* 作者：
* mengyu
*/

namespace Ballance2.Entry
{
  [SLua.CustomLuaClass]
  public class GameDebugModulEntry : MonoBehaviour
  {
    public static GameDebugModulEntry Instance { get; private set; }

    [SLua.DoNotToLua]
    public GameDebugModulEntry() {
      Instance = this;
    }
    
    [Tooltip("机关名称, 将查找此名称的占位物体并把机关替换上去")]
    public string ModulName = "";
    [Tooltip("机关实例")]
    public GameObject ModulInstance = null;
    [Tooltip("机关测试路面的实例")]
    public GameObject ModulTestFloor = null; 
    [Tooltip("机关测试UI的实例")]
    public GameObject ModulTestUI = null; 
  }
}
