using UnityEngine;

/*
 * Copyright (c) 2021  mengyu
 * 
 * 模块名：     
 * TiggerTester.cs
 * 
 * 用途：
 * Tigger检查脚本。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  [SLua.CustomLuaClass]
  [LuaApiDescription("Tigger检查脚本")]
  [LuaApiNotes("此脚本是对 OnTriggerEnter 和 OnTriggerExit 的封装，方便在 Lua 中的触发器检测。")]
  public class TiggerTester : MonoBehaviour
  {   
    [SLua.CustomLuaClass]
    public delegate void OnTiggerTesterEventCallback(GameObject self, GameObject other);

    [LuaApiDescription("当有碰撞体进入当前触发器时，发送此事件（OnTriggerEnter）")]
    public OnTiggerTesterEventCallback onTriggerEnter;
    [LuaApiDescription("当有碰撞体离开当前触发器时，发送此事件（OnTriggerExit）")]
    public OnTiggerTesterEventCallback onTriggerExit;

    private void OnTriggerEnter(Collider other) {
      if(onTriggerEnter != null) 
        onTriggerEnter.Invoke(gameObject, other.gameObject);
    }    
    private void OnTriggerExit(Collider other) {
      if(onTriggerExit != null) 
        onTriggerExit.Invoke(gameObject, other.gameObject);
    }
  }
}