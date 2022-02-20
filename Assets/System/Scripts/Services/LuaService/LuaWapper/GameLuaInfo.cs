using System;
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
*/

namespace Ballance2.Services.LuaService.LuaWapper
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
    [HideInInspector, SerializeField, DoNotToLua]
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
    public float floatVal;
    [HideInInspector, SerializeField, DoNotToLua]
    public string componentClassVal;

    public Component ComponentClass()
    {
      if(componentClassVal.Contains(':')) {
        Component[] components = null;
        string[] buf = componentClassVal.Split(':');
        string componentClassValSp = buf[0];
        int componentClassValIndex = int.Parse(buf[1]);
        switch (componentClassValSp)
        {
          case "UnityEngine.Transform": components = gameObjectVal.GetComponents<Transform>(); break;
          case "UnityEngine.RectTransform": components = gameObjectVal.GetComponents<RectTransform>(); break;
          case "UnityEngine.Skybox": components = gameObjectVal.GetComponents<UnityEngine.Skybox>(); break;
          case "UnityEngine.Light": components = gameObjectVal.GetComponents<UnityEngine.Light>(); break;
          case "UnityEngine.Animation": components = gameObjectVal.GetComponents<UnityEngine.Animation>(); break;
          case "UnityEngine.Camera": components = gameObjectVal.GetComponents<UnityEngine.Camera>(); break;
          case "UnityEngine.MeshRenderer": components = gameObjectVal.GetComponents<UnityEngine.MeshRenderer>(); break;
          case "UnityEngine.MeshFilter": components = gameObjectVal.GetComponents<UnityEngine.MeshFilter>(); break;
          case "UnityEngine.Animator": components = gameObjectVal.GetComponents<UnityEngine.Animator>(); break;
          case "UnityEngine.AudioSource": components = gameObjectVal.GetComponents<UnityEngine.AudioSource>(); break;
          case "UnityEngine.BillboardRenderer": components = gameObjectVal.GetComponents<UnityEngine.BillboardRenderer>(); break;
          case "UnityEngine.LineRenderer": components = gameObjectVal.GetComponents<UnityEngine.LineRenderer>(); break;
          case "UnityEngine.TrailRenderer": components = gameObjectVal.GetComponents<UnityEngine.TrailRenderer>(); break;
          case "UnityEngine.SkinnedMeshRenderer": components = gameObjectVal.GetComponents<UnityEngine.SkinnedMeshRenderer>(); break;
          case "UnityEngine.SpriteRenderer": components = gameObjectVal.GetComponents<UnityEngine.SpriteRenderer>(); break;
          case "UnityEngine.SpriteMask": components = gameObjectVal.GetComponents<UnityEngine.SpriteMask>(); break;
          case "UnityEngine.OcclusionPortal": components = gameObjectVal.GetComponents<UnityEngine.OcclusionPortal>(); break;
          case "UnityEngine.OcclusionArea": components = gameObjectVal.GetComponents<UnityEngine.OcclusionArea>(); break;
          case "UnityEngine.Rigidbody": components = gameObjectVal.GetComponents<UnityEngine.Rigidbody>(); break;
          case "UnityEngine.Rigidbody2D": components = gameObjectVal.GetComponents<UnityEngine.Rigidbody2D>(); break;
          case "UnityEngine.Tree": components = gameObjectVal.GetComponents<UnityEngine.Tree>(); break;
          case "UnityEngine.TextMesh": components = gameObjectVal.GetComponents<UnityEngine.TextMesh>(); break;
          case "UnityEngine.CanvasRenderer": components = gameObjectVal.GetComponents<UnityEngine.CanvasRenderer>(); break;
          case "UnityEngine.WindZone": components = gameObjectVal.GetComponents<UnityEngine.WindZone>(); break;
          case "UnityEngine.Cloth": components = gameObjectVal.GetComponents<UnityEngine.Cloth>(); break;
          case "UnityEngine.CharacterController": components = gameObjectVal.GetComponents<UnityEngine.CharacterController>(); break;
          case "UnityEngine.MeshCollider": components = gameObjectVal.GetComponents<UnityEngine.MeshCollider>(); break;
          case "UnityEngine.CapsuleCollider": components = gameObjectVal.GetComponents<UnityEngine.CapsuleCollider>(); break;
          case "UnityEngine.BoxCollider": components = gameObjectVal.GetComponents<UnityEngine.BoxCollider>(); break;
          case "UnityEngine.SphereCollider": components = gameObjectVal.GetComponents<UnityEngine.SphereCollider>(); break;
          case "UnityEngine.TerrainCollider": components = gameObjectVal.GetComponents<UnityEngine.TerrainCollider>(); break;
          case "UnityEngine.WheelCollider": components = gameObjectVal.GetComponents<UnityEngine.WheelCollider>(); break;
          case "UnityEngine.HingeJoint": components = gameObjectVal.GetComponents<UnityEngine.HingeJoint>(); break;
          case "UnityEngine.SpringJoint": components = gameObjectVal.GetComponents<UnityEngine.SpringJoint>(); break;
          case "UnityEngine.FixedJoint": components = gameObjectVal.GetComponents<UnityEngine.FixedJoint>(); break;
          case "UnityEngine.CharacterJoint": components = gameObjectVal.GetComponents<UnityEngine.CharacterJoint>(); break;
          case "UnityEngine.ConfigurableJoint": components = gameObjectVal.GetComponents<UnityEngine.ConfigurableJoint>(); break;
          case "UnityEngine.MonoBehaviour": components = gameObjectVal.GetComponents<UnityEngine.MonoBehaviour>(); break;
          case "UnityEngine.FlareLayer": components = gameObjectVal.GetComponents<UnityEngine.FlareLayer>(); break;
          case "UnityEngine.ReflectionProbe": components = gameObjectVal.GetComponents<UnityEngine.ReflectionProbe>(); break;
          case "UnityEngine.Projector": components = gameObjectVal.GetComponents<UnityEngine.Projector>(); break;
          case "UnityEngine.LensFlare": components = gameObjectVal.GetComponents<UnityEngine.LensFlare>(); break;
          case "UnityEngine.AudioBehaviour": components = gameObjectVal.GetComponents<UnityEngine.AudioBehaviour>(); break;
          case "UnityEngine.AudioLowPassFilter": components = gameObjectVal.GetComponents<UnityEngine.AudioLowPassFilter>(); break;
          case "UnityEngine.AudioHighPassFilter": components = gameObjectVal.GetComponents<UnityEngine.AudioHighPassFilter>(); break;
          case "UnityEngine.AudioReverbFilter": components = gameObjectVal.GetComponents<UnityEngine.AudioReverbFilter>(); break;
          case "UnityEngine.AudioListener": components = gameObjectVal.GetComponents<UnityEngine.AudioListener>(); break;
          case "UnityEngine.AudioReverbZone": components = gameObjectVal.GetComponents<UnityEngine.AudioReverbZone>(); break;
          case "UnityEngine.AudioDistortionFilter": components = gameObjectVal.GetComponents<UnityEngine.AudioDistortionFilter>(); break;
          case "UnityEngine.AudioEchoFilter": components = gameObjectVal.GetComponents<UnityEngine.AudioEchoFilter>(); break;
          case "UnityEngine.AudioChorusFilter": components = gameObjectVal.GetComponents<UnityEngine.AudioChorusFilter>(); break;
          case "UnityEngine.ConstantForce": components = gameObjectVal.GetComponents<UnityEngine.ConstantForce>(); break;
          case "UnityEngine.ArticulationBody": components = gameObjectVal.GetComponents<UnityEngine.ArticulationBody>(); break;
          case "UnityEngine.CanvasGroup": components = gameObjectVal.GetComponents<UnityEngine.CanvasGroup>(); break;
          case "UnityEngine.Canvas": components = gameObjectVal.GetComponents<UnityEngine.Canvas>(); break;
          case "UnityEngine.Animations.AimConstraint": components = gameObjectVal.GetComponents<UnityEngine.Animations.AimConstraint>(); break;
          case "UnityEngine.Animations.ParentConstraint": components = gameObjectVal.GetComponents<UnityEngine.Animations.ParentConstraint>(); break;
          case "UnityEngine.Animations.ScaleConstraint": components = gameObjectVal.GetComponents<UnityEngine.Animations.ScaleConstraint>(); break;
          case "UnityEngine.Animations.LookAtConstraint": components = gameObjectVal.GetComponents<UnityEngine.Animations.LookAtConstraint>(); break;
          case "UnityEngine.Animations.PositionConstraint": components = gameObjectVal.GetComponents<UnityEngine.Animations.PositionConstraint>(); break;
          case "UnityEngine.Animations.RotationConstraint": components = gameObjectVal.GetComponents<UnityEngine.Animations.RotationConstraint>(); break;
          default:
            {
              var c = gameObjectVal.GetComponents<Component>();
              if (c != null) {
                foreach(var t in c) {
                  if(t.GetType().FullName == componentClassValSp)
                    return t;
                }
              }
              
              UnityEngine.Debug.LogWarning("Can't get component: " + componentClassVal + " May be should add reference in LuaVarObjectInfo?");
              return null;
            }
        }

        if(components != null)
          return components[componentClassValIndex];
        else {
          UnityEngine.Debug.LogWarning("Can't get component: " + componentClassVal + " May be should add reference in LuaVarObjectInfo?");
          return null;
        }
      } else {
        switch (componentClassVal)
        {
          case "UnityEngine.Transform": return gameObjectVal.GetComponent<Transform>();
          case "UnityEngine.RectTransform": return gameObjectVal.GetComponent<RectTransform>();
          case "UnityEngine.Skybox": return gameObjectVal.GetComponent<UnityEngine.Skybox>();
          case "UnityEngine.Light": return gameObjectVal.GetComponent<UnityEngine.Light>();
          case "UnityEngine.Animation": return gameObjectVal.GetComponent<UnityEngine.Animation>();
          case "UnityEngine.Camera": return gameObjectVal.GetComponent<UnityEngine.Camera>();
          case "UnityEngine.MeshRenderer": return gameObjectVal.GetComponent<UnityEngine.MeshRenderer>();
          case "UnityEngine.MeshFilter": return gameObjectVal.GetComponent<UnityEngine.MeshFilter>();
          case "UnityEngine.Animator": return gameObjectVal.GetComponent<UnityEngine.Animator>();
          case "UnityEngine.AudioSource": return gameObjectVal.GetComponent<UnityEngine.AudioSource>();
          case "UnityEngine.BillboardRenderer": return gameObjectVal.GetComponent<UnityEngine.BillboardRenderer>();
          case "UnityEngine.LineRenderer": return gameObjectVal.GetComponent<UnityEngine.LineRenderer>();
          case "UnityEngine.TrailRenderer": return gameObjectVal.GetComponent<UnityEngine.TrailRenderer>();
          case "UnityEngine.SkinnedMeshRenderer": return gameObjectVal.GetComponent<UnityEngine.SkinnedMeshRenderer>();
          case "UnityEngine.SpriteRenderer": return gameObjectVal.GetComponent<UnityEngine.SpriteRenderer>();
          case "UnityEngine.SpriteMask": return gameObjectVal.GetComponent<UnityEngine.SpriteMask>();
          case "UnityEngine.OcclusionPortal": return gameObjectVal.GetComponent<UnityEngine.OcclusionPortal>();
          case "UnityEngine.OcclusionArea": return gameObjectVal.GetComponent<UnityEngine.OcclusionArea>();
          case "UnityEngine.Rigidbody": return gameObjectVal.GetComponent<UnityEngine.Rigidbody>();
          case "UnityEngine.Rigidbody2D": return gameObjectVal.GetComponent<UnityEngine.Rigidbody2D>();
          case "UnityEngine.Tree": return gameObjectVal.GetComponent<UnityEngine.Tree>();
          case "UnityEngine.TextMesh": return gameObjectVal.GetComponent<UnityEngine.TextMesh>();
          case "UnityEngine.CanvasRenderer": return gameObjectVal.GetComponent<UnityEngine.CanvasRenderer>();
          case "UnityEngine.WindZone": return gameObjectVal.GetComponent<UnityEngine.WindZone>();
          case "UnityEngine.Cloth": return gameObjectVal.GetComponent<UnityEngine.Cloth>();
          case "UnityEngine.CharacterController": return gameObjectVal.GetComponent<UnityEngine.CharacterController>();
          case "UnityEngine.MeshCollider": return gameObjectVal.GetComponent<UnityEngine.MeshCollider>();
          case "UnityEngine.CapsuleCollider": return gameObjectVal.GetComponent<UnityEngine.CapsuleCollider>();
          case "UnityEngine.BoxCollider": return gameObjectVal.GetComponent<UnityEngine.BoxCollider>();
          case "UnityEngine.SphereCollider": return gameObjectVal.GetComponent<UnityEngine.SphereCollider>();
          case "UnityEngine.TerrainCollider": return gameObjectVal.GetComponent<UnityEngine.TerrainCollider>();
          case "UnityEngine.WheelCollider": return gameObjectVal.GetComponent<UnityEngine.WheelCollider>();
          case "UnityEngine.HingeJoint": return gameObjectVal.GetComponent<UnityEngine.HingeJoint>();
          case "UnityEngine.SpringJoint": return gameObjectVal.GetComponent<UnityEngine.SpringJoint>();
          case "UnityEngine.FixedJoint": return gameObjectVal.GetComponent<UnityEngine.FixedJoint>();
          case "UnityEngine.CharacterJoint": return gameObjectVal.GetComponent<UnityEngine.CharacterJoint>();
          case "UnityEngine.ConfigurableJoint": return gameObjectVal.GetComponent<UnityEngine.ConfigurableJoint>();
          case "UnityEngine.MonoBehaviour": return gameObjectVal.GetComponent<UnityEngine.MonoBehaviour>();
          case "UnityEngine.FlareLayer": return gameObjectVal.GetComponent<UnityEngine.FlareLayer>();
          case "UnityEngine.ReflectionProbe": return gameObjectVal.GetComponent<UnityEngine.ReflectionProbe>();
          case "UnityEngine.Projector": return gameObjectVal.GetComponent<UnityEngine.Projector>();
          case "UnityEngine.LensFlare": return gameObjectVal.GetComponent<UnityEngine.LensFlare>();
          case "UnityEngine.AudioBehaviour": return gameObjectVal.GetComponent<UnityEngine.AudioBehaviour>();
          case "UnityEngine.AudioLowPassFilter": return gameObjectVal.GetComponent<UnityEngine.AudioLowPassFilter>();
          case "UnityEngine.AudioHighPassFilter": return gameObjectVal.GetComponent<UnityEngine.AudioHighPassFilter>();
          case "UnityEngine.AudioReverbFilter": return gameObjectVal.GetComponent<UnityEngine.AudioReverbFilter>();
          case "UnityEngine.AudioListener": return gameObjectVal.GetComponent<UnityEngine.AudioListener>();
          case "UnityEngine.AudioReverbZone": return gameObjectVal.GetComponent<UnityEngine.AudioReverbZone>();
          case "UnityEngine.AudioDistortionFilter": return gameObjectVal.GetComponent<UnityEngine.AudioDistortionFilter>();
          case "UnityEngine.AudioEchoFilter": return gameObjectVal.GetComponent<UnityEngine.AudioEchoFilter>();
          case "UnityEngine.AudioChorusFilter": return gameObjectVal.GetComponent<UnityEngine.AudioChorusFilter>();
          case "UnityEngine.ConstantForce": return gameObjectVal.GetComponent<UnityEngine.ConstantForce>();
          case "UnityEngine.ArticulationBody": return gameObjectVal.GetComponent<UnityEngine.ArticulationBody>();
          case "UnityEngine.CanvasGroup": return gameObjectVal.GetComponent<UnityEngine.CanvasGroup>();
          case "UnityEngine.Canvas": return gameObjectVal.GetComponent<UnityEngine.Canvas>();
          case "UnityEngine.Animations.AimConstraint": return gameObjectVal.GetComponent<UnityEngine.Animations.AimConstraint>();
          case "UnityEngine.Animations.ParentConstraint": return gameObjectVal.GetComponent<UnityEngine.Animations.ParentConstraint>();
          case "UnityEngine.Animations.ScaleConstraint": return gameObjectVal.GetComponent<UnityEngine.Animations.ScaleConstraint>();
          case "UnityEngine.Animations.LookAtConstraint": return gameObjectVal.GetComponent<UnityEngine.Animations.LookAtConstraint>();
          case "UnityEngine.Animations.PositionConstraint": return gameObjectVal.GetComponent<UnityEngine.Animations.PositionConstraint>();
          case "UnityEngine.Animations.RotationConstraint": return gameObjectVal.GetComponent<UnityEngine.Animations.RotationConstraint>();
          default:
            {
              var c = gameObjectVal.GetComponent(componentClassVal);
              if (c == null)
                UnityEngine.Debug.LogWarning("Can't get component: " +
                    componentClassVal + " " + (c == null ? "(null)" : (c + "(" + c.GetType().Name + ")"))
                    + " May be should add reference in LuaVarObjectInfo?");
              return c;
            }
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
    public string String() { return stringVal; }
    public int Int() { return intVal; }
    public double Double() { return doubleVal; }
    public bool Bool() { return boolVal; }
    public float Float() { return floatVal; }

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
        case LuaVarObjectType.Float: LuaSelf[Name] = Float(); break;
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
        case LuaVarObjectType.Float: floatVal = (float)LuaSelf[Name]; break;
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
    Float,
  }


}
