using System;
using Ballance.LuaHelpers;
using SLua;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameLuaInfo.cs
* 
* 用途：
* Lua定义结构体。
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-21 创建
*
*/

namespace Ballance2.Sys.Bridge.LuaWapper
{
    /// <summary>
    /// lua 引入 var 信息
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("lua 引入 var 信息")]
    [Serializable]
    public class LuaVarObjectInfo
    {
        /// <summary>
        /// 值类型
        /// </summary>
        [Tooltip("值类型")]
        [SerializeField]
        [LuaApiDescription("值类型")]
        public LuaVarObjectType Type;
        /// <summary>
        /// 值名称
        /// </summary>
        [Tooltip("值名称")]
        [SerializeField]
        [LuaApiDescription("值名称")]
        public string Name;

        public LuaVarObjectInfo()
        {
            Type = LuaVarObjectType.None;
            Name = "";
        }

        public override string ToString() { return Name; }

        [HideInInspector, SerializeField, DoNotToLua]
        public Vector2 vector2;
        [HideInInspector, SerializeField, DoNotToLua]
        public Vector2Int vector2Int;
        [HideInInspector, SerializeField, DoNotToLua]
        public Vector3 vector3;
        [HideInInspector, SerializeField, DoNotToLua]
        public Vector3Int vector3Int;
        [HideInInspector,SerializeField,DoNotToLua]
        public Vector4 vector4;
        [HideInInspector, SerializeField, DoNotToLua]
        public Rect rect;
        [HideInInspector, SerializeField, DoNotToLua]
        public RectInt rectInt;
        [HideInInspector, SerializeField, DoNotToLua]
        public Gradient gradient;
        [HideInInspector, SerializeField, DoNotToLua]
        public int layer;
        [HideInInspector, SerializeField, DoNotToLua]
        public AnimationCurve curve;
        [HideInInspector, SerializeField, DoNotToLua]
        public Color color;
        [HideInInspector, SerializeField, DoNotToLua]
        public BoundsInt boundsInt;
        [HideInInspector, SerializeField, DoNotToLua]
        public Bounds bounds;
        [HideInInspector, SerializeField, DoNotToLua]
        public UnityEngine.Object objectVal;
        [HideInInspector, SerializeField, DoNotToLua]
        public GameObject gameObjectVal;
        [HideInInspector, SerializeField, DoNotToLua]
        public long longVal;
        [HideInInspector, SerializeField, DoNotToLua]
        public string stringVal;
        [HideInInspector, SerializeField, DoNotToLua]
        public int intVal;
        [HideInInspector, SerializeField, DoNotToLua]
        public double doubleVal;
        [HideInInspector, SerializeField, DoNotToLua]
        public bool boolVal;
        [HideInInspector, SerializeField, DoNotToLua]
        public string componentClassVal;

        public Component ComponentClass() {
            switch (componentClassVal)
            { 
                case "UnityEngine.Transform": return gameObjectVal.GetComponent<Transform>();
                case "UnityEngine.RectTransform": return gameObjectVal.GetComponent<RectTransform>();
                case "UnityEngine.Skybox": return gameObjectVal.GetComponent<UnityEngine.Skybox>();
                default: { 
                    var c = gameObjectVal.GetComponent(componentClassVal);
                    if(c == null)
                        UnityEngine.Debug.Log("Can't get component: " + 
                            componentClassVal + " " + (c == null ? "(null)" : (c + "(" + c.GetType().Name + ")"))
                            + "\nMay be should add reference in LuaVarObjectInfo");
                    return c;
                }
            }
        }
        public Vector2 Vector2() { return vector2; }
        public Vector2Int Vector2Int() { return vector2Int; }
        public Vector3 Vector3() { return vector3; }
        public Vector3Int Vector3Int() { return vector3Int; }
        public Vector4 Vector4() { return vector4; }
        public Rect Rect() { return rect; }
        public RectInt RectInt() { return rectInt; }
        public Gradient Gradient() { return gradient; }
        public int Layer() { return layer; }
        public AnimationCurve Curve() { return curve; }
        public Color Color() { return color; }
        public BoundsInt BoundsInt() { return boundsInt; }
        public Bounds Bounds() { return bounds; }
        public UnityEngine.Object Object() { return objectVal; }
        public UnityEngine.Object GameObject() { return gameObjectVal; }
        public long Long() { return longVal; }
        public string String() { return stringVal ; }
        public int Int() { return intVal; }
        public double Double() { return doubleVal; }
        public bool Bool() { return boolVal; }

        /// <summary>
        /// 更新数据到 Lua
        /// </summary>
        /// <param name="LuaSelf">self</param>
        [LuaApiDescription("更新数据到 Lua")]
        [LuaApiParamDescription("LuaSelf", "self")]
        public void UpdateToLua(LuaTable LuaSelf)
        {
            switch (Type)
            {
                case LuaVarObjectType.None: LuaSelf[Name] = null; break;
                case LuaVarObjectType.Vector2: LuaSelf[Name] = Vector2(); break;
                case LuaVarObjectType.Vector2Int: LuaSelf[Name] = Vector2Int(); break;
                case LuaVarObjectType.Vector3: LuaSelf[Name] = Vector3(); break;
                case LuaVarObjectType.Vector3Int: LuaSelf[Name] = Vector3Int(); break;
                case LuaVarObjectType.Vector4: LuaSelf[Name] = Vector4(); break;
                case LuaVarObjectType.Rect: LuaSelf[Name] = Rect(); break;
                case LuaVarObjectType.RectInt: LuaSelf[Name] = RectInt(); break;
                case LuaVarObjectType.Gradient: LuaSelf[Name] = Gradient(); break;
                case LuaVarObjectType.Layer: LuaSelf[Name] = Layer(); break;
                case LuaVarObjectType.Curve: LuaSelf[Name] = Curve(); break;
                case LuaVarObjectType.Color: LuaSelf[Name] = Color(); break;
                case LuaVarObjectType.BoundsInt: LuaSelf[Name] = BoundsInt(); break;
                case LuaVarObjectType.Bounds: LuaSelf[Name] = Bounds(); break;
                case LuaVarObjectType.Object: LuaSelf[Name] = Object(); break;
                case LuaVarObjectType.GameObject: LuaSelf[Name] = GameObject(); break;
                case LuaVarObjectType.Long: LuaSelf[Name] = Long(); break;
                case LuaVarObjectType.Int: LuaSelf[Name] = Int(); break;
                case LuaVarObjectType.String: LuaSelf[Name] = String(); break;
                case LuaVarObjectType.Double: LuaSelf[Name] = Double(); break;
                case LuaVarObjectType.Bool: LuaSelf[Name] = Bool(); break;
                case LuaVarObjectType.ComponentClass: LuaSelf[Name] = ComponentClass(); break;
            }
        }
        /// <summary>
        /// 从 Lua 获取数据
        /// </summary>
        /// <param name="LuaSelf">self</param>
        [LuaApiDescription("从 Lua 获取数据")]
        [LuaApiParamDescription("LuaSelf", "self")]
        public void UpdateFromLua(LuaTable LuaSelf)
        {
            switch (Type)
            {
                case LuaVarObjectType.Vector2: vector2 = (Vector2)LuaSelf[Name]; break;
                case LuaVarObjectType.Vector2Int: vector2Int = (Vector2Int)LuaSelf[Name]; break;
                case LuaVarObjectType.Vector3: vector3 = (Vector3)LuaSelf[Name]; break;
                case LuaVarObjectType.Vector3Int: vector3Int = (Vector3Int)LuaSelf[Name]; break;
                case LuaVarObjectType.Vector4: vector4 = (Vector4)LuaSelf[Name]; break;
                case LuaVarObjectType.Rect: rect = (Rect)LuaSelf[Name]; break;
                case LuaVarObjectType.RectInt: rectInt = (RectInt)LuaSelf[Name]; ; break;
                case LuaVarObjectType.Gradient: gradient = (Gradient)LuaSelf[Name]; break;
                case LuaVarObjectType.Layer: intVal = (int)LuaSelf[Name]; break;
                case LuaVarObjectType.Curve: curve = (AnimationCurve)LuaSelf[Name]; break;
                case LuaVarObjectType.Color: color = (Color)LuaSelf[Name]; break;
                case LuaVarObjectType.BoundsInt: boundsInt = (BoundsInt)LuaSelf[Name]; break;
                case LuaVarObjectType.Bounds: bounds = (Bounds)LuaSelf[Name]; break;
                case LuaVarObjectType.Object: objectVal = (UnityEngine.Object)LuaSelf[Name]; break;
                case LuaVarObjectType.GameObject: gameObjectVal = (GameObject)LuaSelf[Name]; break;
                case LuaVarObjectType.Long: longVal = (long)LuaSelf[Name]; break;
                case LuaVarObjectType.Int: intVal = (int)LuaSelf[Name]; break;
                case LuaVarObjectType.String: stringVal = (string)LuaSelf[Name]; break;
                case LuaVarObjectType.Double: doubleVal = (double)LuaSelf[Name]; break;
                case LuaVarObjectType.Bool: boolVal = (bool)LuaSelf[Name]; break;
            }
        }
    }

    /// <summary>
    /// 指定引入数据的类型
    /// </summary>
    [Serializable]
    [CustomLuaClass]
    [LuaApiDescription("指定引入数据的类型")]
    public enum LuaVarObjectType
    {
        None = 0,
        Vector2,
        Vector2Int,
        Vector3,
        Vector3Int,
        Vector4,
        Rect,
        RectInt,
        Gradient,
        Layer,
        Curve,
        Color,
        BoundsInt,
        Bounds,
        Object,
        GameObject,
        ComponentClass,

        Long,
        Int,
        String,
        Double,
        Bool,
    }


}
