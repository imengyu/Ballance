using Ballance2.Base;
using SLua;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameLuaWapper.cs
 * 
 * 用途：
 * Lua包装所需的委托。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Services.LuaService.LuaWapper
{
    [CustomLuaClass]
    public delegate void LuaStartDelegate(LuaTable self, GameObject gameObject);
    [CustomLuaClass]
    public delegate void LuaCollisionDelegate(LuaTable self, Collision collision);
    [CustomLuaClass]
    public delegate void LuaColliderDelegate(LuaTable self, Collider collider);

    [CustomLuaClass]
    public delegate void LuaCollision2DDelegate(LuaTable self, Collision2D collision);
    [CustomLuaClass]
    public delegate void LuaCollider2DDelegate(LuaTable self, Collider2D collider);

    [CustomLuaClass]
    public delegate void LuaVector3Delegate(LuaTable self, Vector3 vector3);
    [CustomLuaClass]
    public delegate void LuaBoolDelegate(LuaTable self, bool b);
    [CustomLuaClass]
    public delegate void LuaIntDelegate(LuaTable self, int v);
    [CustomLuaClass]
    public delegate void LuaGameObjectDelegate(LuaTable self, GameObject gameObject);
    [CustomLuaClass]
    public delegate void LuaPointerEventDataDelegate(LuaTable self, PointerEventData pointerEventData);
    [CustomLuaClass]
    public delegate void LuaBaseEventDataDelegate(LuaTable self, BaseEventData baseEvent);
    [CustomLuaClass]
    public delegate void LuaAxisEventDataDelegate(LuaTable self, AxisEventData baseEvent);
    
    [CustomLuaClass]
    public delegate void LuaVoidDelegate(LuaTable self);

    [CustomLuaClass]
    public delegate bool LuaReturnBoolDelegate(LuaTable self);
    [CustomLuaClass]
    public delegate bool LuaActionStoreReturnBoolDelegate(LuaTable self, GameActionStore store);
}
