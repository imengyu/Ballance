using System.IO;
using Ballance2.Config;
using SLua;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * LuaDebugMini.cs
 * 
 * 用途：
 * Lua 迷你调试环境。
 * 
 * 作者：
 * mengyu
 */

[CustomLuaClass]
    [LuaApiNoDoc]
public class LuaDebugMini : MonoBehaviour {

  [DoNotToLua]
  public static LuaDebugMini Instance;
  [DoNotToLua]
  public static LuaState LuaState;
  [DoNotToLua]
  public static LuaSvr LuaSvr;

  private void Awake() {
#if UNITY_EDITOR 
    Instance = this;
    LuaSvr = new LuaSvr();
    LuaSvr.init(null, () =>
    {
        LuaState = LuaSvr.mainState;
        //检测lua绑定状态
        object o = LuaState.doString(@"require = function(name) return LuaDebugMini.LuaDebugMiniRequire(name) end
        return Ballance2.Services.GameManager.LuaBindingCallback()", "LuaDebugMiniInit");
        if (o != null &&  (
                (o.GetType() == typeof(int) && (int)o == GameConst.GameBulidVersion)
                || (o.GetType() == typeof(double) && (double)o == GameConst.GameBulidVersion)
            ))
            Debug.Log("Game Lua bind check ok.");
        else
            Debug.LogError("Game Lua bind check failed");

    });
#else
    Debug.LogError("LuaDebugMini can only use in editor");
#endif
  }

  public static object LuaDebugMiniRequire(string name) {
#if UNITY_EDITOR 
    return LuaState.doString(File.ReadAllText(name));
#else
    Debug.LogError("LuaDebugMini can only use in editor");
    return null;
#endif
    
  }
}