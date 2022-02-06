using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ballance2.LuaHelpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallancePhysics.Wapper
{
  [AddComponentMenu("BallancePhysics/PhysicsEnvironment")]
  [DefaultExecutionOrder(10)]
  [DisallowMultipleComponent]
  [SLua.CustomLuaClass]
  [LuaApiDescription("物理环境")]
  public class PhysicsEnvironment : MonoBehaviour
  {
    [Tooltip("世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效)")]
    public Vector3 Gravity = new Vector3(0, -9.81f, 0);
    [Tooltip("模拟速率（10-100，一秒钟进行物理模拟的速率）(模拟开始后更改此值无效)")]
    public int SimulationRate = 66;
    [Tooltip("用于物理对象模拟的时间乘以的因子。因此，如果“物理时间因子”为2.0，而不是1.0（正常速度），则物理对象下落的速度将加倍。")]
    public int TimeFactor = 1;
    [Tooltip("是否在销毁环境时自动删除所有碰撞层")]
    public bool DeleteAllSurfacesWhenDestroy = true;
    [Tooltip("是否启用模拟")]
    public bool Simulate = true;

    /// <summary>
    /// 所有物理环境
    /// </summary>
    /// <typeparam name="int">Unity场景的buildIndex</typeparam>
    /// <typeparam name="PhysicsWorld"></typeparam>
    /// <returns></returns>
    [LuaApiDescription("所有物理环境")]
    public static Dictionary<int, PhysicsEnvironment> PhysicsWorlds { get; } = new Dictionary<int, PhysicsEnvironment>();
    /// <summary>
    /// 获取当前场景的物理环境
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取当前场景的物理环境")]
    public static PhysicsEnvironment GetCurrentScensePhysicsWorld()
    {
      int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
      if (PhysicsWorlds.TryGetValue(currentScenseIndex, out var a))
        return a;
      return null;
    }

    public IntPtr Handle { get; private set; } = IntPtr.Zero;

    private void Awake()
    {
      int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
      if (PhysicsWorlds.ContainsKey(currentScenseIndex))
        Debug.LogError("There can only one PhysicsWorld instance in a scense.");
      else
      {
        var layerNames = Resources.Load<PhysicsLayerNames>("BallancePhysicsLayerNames");
        if(layerNames == null)
          throw new Exception("BallancePhysicsLayerNames not found. Click menu in Assets to create it.");

        PhysicsWorlds.Add(currentScenseIndex, this);
        Handle = PhysicsApi.API.create_environment(Gravity, 1.0f / SimulationRate, -2147483647, layerNames.GetGroupFilterMasks());
      }
    }
    private void OnDestroy()
    {
      if (Handle != IntPtr.Zero)
      {
        List<PhysicsObject> list = new List<PhysicsObject>(objectsDict.Values);
        foreach(var o in list)
          o.UnPhysicalize(true);

        PhysicsApi.API.delete_all_surfaces(Handle);
        PhysicsApi.API.destroy_environment(Handle);
        Handle = IntPtr.Zero;

        int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
        PhysicsWorlds.Remove(currentScenseIndex);
      }
    }
    private void FixedUpdate() {
      if(Simulate && Handle != IntPtr.Zero) {
        PhysicsApi.API.environment_simulate_dtime(Handle, Time.fixedDeltaTime * TimeFactor);
        PhysicsApi.API.do_update_all(Handle);

        float[] dat = new float[4];
        foreach(var bodyCurrent in objectsDict.Values) 
        {
          if(bodyCurrent.Fixed)
            continue;

          var t = bodyCurrent.gameObject.transform;
          IntPtr ptr = bodyCurrent.Handle; //pos 0
          if(ptr == IntPtr.Zero)
            continue;

          ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf<int>()); //pos 1
          Marshal.Copy(ptr, dat, 0, 3);      //float[3]

          var p = new Vector3(dat[0], dat[1], dat[2]);
          t.position = p;

          ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf<float>() * 3); //pos 4
          Marshal.Copy(ptr, dat, 0, 4);      //float[4]

          t.rotation = new Quaternion(dat[0], dat[1], dat[2], dat[3]);
        }
      }
    }

    private Dictionary<string, int> dictSystemGroup = new Dictionary<string, int>();
    private Dictionary<int, PhysicsObject> objectsDict = new Dictionary<int, PhysicsObject>();

    /// <summary>
    /// [由PhysicsObject自动调用，请勿手动调用]
    /// </summary>
    /// <param name="id"></param>
    /// <param name="body"></param>
    internal void AddPhysicsObject(int id, PhysicsObject body)
    {
      objectsDict.Add(id, body);
    }
    /// <summary>
    /// [由PhysicsObject自动调用，请勿手动调用]
    /// </summary>
    /// <param name="body"></param>
    internal void RemovePhysicsObject(PhysicsObject body)
    {
      objectsDict.Remove(body.Id);
    }

    /// <summary>
    /// 通过名称获取碰撞子组信息。如果没有指定名称的子组，则自动生成
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [LuaApiDescription("通过名称获取碰撞子组信息。如果没有指定名称的子组，则自动生成")]
    public int GetSystemGroup(string name)
    {
      if (Handle == IntPtr.Zero || string.IsNullOrEmpty(name))
        return 0;
      if (dictSystemGroup.TryGetValue(name, out var i))
        return i;
      i = PhysicsApi.API.environment_new_system_group(Handle);
      dictSystemGroup[name] = i;
      return i;
    }
    /// <summary>
    /// 删除物理系统中的所有碰撞层
    /// </summary>
    [LuaApiDescription("删除物理系统中的所有碰撞层")]
    public void DeleteAllSurfaces() { PhysicsApi.API.delete_all_surfaces(Handle); }
    /// <summary>
    /// 通过ID查找世界中的物理物体
    /// </summary>
    /// <param name="bodyId">ID</param>
    /// <returns>如果未找到则返回null</returns>
    [LuaApiDescription("删除物理系统中的所有碰撞层", "如果未找到则返回null")]
    [LuaApiParamDescription("bodyId", "ID")]
    public PhysicsObject GetObjectById(int bodyId)
    {
      if (objectsDict.TryGetValue(bodyId, out var r))
        return r;
      return null;
    }

    /// <summary>
    /// 从指定位置发射射线，返回第一个碰撞物体。
    /// </summary>
    /// <param name="startPoint">射线发射位置</param>
    /// <param name="dirction">射线方向向量</param>
    /// <param name="rayLength">射线长度</param>
    /// <param name="distance">第一个碰撞物体的距离</param>
    /// <returns>如果有物体碰撞，则返回第一个物体，否则返回null</returns>
    [LuaApiDescription("从指定位置发射射线，返回第一个碰撞物体。", "如果有物体碰撞，则返回第一个物体，否则返回null")]
    [LuaApiParamDescription("startPoint", "射线发射位置")]
    [LuaApiParamDescription("dirction", "射线方向向量")]
    [LuaApiParamDescription("rayLength", "射线长度")]
    [LuaApiParamDescription("distance", "第一个碰撞物体的距离")]
    public PhysicsObject RaycastingOne(Vector3 startPoint, Vector3 dirction, float rayLength, out float distance)
    {
      if (Handle == IntPtr.Zero)
        throw new Exception("World not create or destroyed");

      var _distance = 0.0f;
      var id = PhysicsApi.API.raycasting_one(Handle, startPoint, dirction, rayLength, ref _distance);
      distance = _distance;
      return GetObjectById(id);
    }
    /// <summary>
    /// 从指定位置发射射线，射线是否与指定物体碰撞。
    /// </summary>
    /// <param name="obj">指定物体</param>
    /// <param name="startPoint">射线发射位置</param>
    /// <param name="dirction">射线方向向量</param>
    /// <param name="rayLength">射线长度</param>
    /// <param name="distance">第一个碰撞物体的距离</param>
    /// <returns>如果射线有和物体碰撞，则返回true，否则返回false</returns>
    [LuaApiDescription("从指定位置发射射线，射线是否与指定物体碰撞。", "如果射线有和物体碰撞，则返回true，否则返回false")]
    [LuaApiParamDescription("obj", "指定物体")]
    [LuaApiParamDescription("startPoint", "射线发射位置")]
    [LuaApiParamDescription("dirction", "射线方向向量")]
    [LuaApiParamDescription("rayLength", "射线长度")]
    [LuaApiParamDescription("distance", "第一个碰撞物体的距离")]
    public bool RaycastingObject(PhysicsObject obj, Vector3 startPoint, Vector3 dirction, float rayLength, out float distance)
    {
      if (Handle == IntPtr.Zero)
        throw new Exception("World not create or destroyed");
      if(obj.Handle == IntPtr.Zero)
        throw new Exception("Object " + obj.name + " is not physicalized");

      var _distance = 0.0f;
      var b = PhysicsApi.API.raycasting_object(obj.Handle, startPoint, dirction, rayLength, ref _distance);
      distance = _distance;
      return b;
    }

    [LuaApiDescription("射线处理标志")]
    [SLua.CustomLuaClass]
    public enum RaySolverFlag
    {
      [LuaApiDescription("射线将会碰撞全部物体")]
      All = 0,
      [LuaApiDescription("射线将会碰撞幻影")]
      Phantoms = 1,
      [LuaApiDescription("射线将会碰撞可移动的物体")]
      Movings = 2,
      [LuaApiDescription("射线将会碰撞不可移动的物体")]
      Statics = 4
    };
    [LuaApiDescription("射线碰撞结果")]
    [SLua.CustomLuaClass]
    public struct RayCastResult
    {
      [LuaApiDescription("碰撞的物体")]
      public List<PhysicsObject> HitObjects;
      [LuaApiDescription("碰撞的物体距离射线发射原点的位置")]
      public List<float> HitDistances;
    }

    /// <summary>
    /// 从指定位置发射射线，获取射线碰撞的全部物体
    /// </summary>
    /// <param name="flags">射线处理标志</param>
    /// <param name="startPoint">射线发射位置</param>
    /// <param name="dirction">射线方向向量</param>
    /// <param name="rayLength">射线长度</param>
    /// <returns></returns>
    [LuaApiDescription("从指定位置发射射线，获取射线碰撞的全部物体")]
    [LuaApiParamDescription("flags", "射线处理标志")]
    [LuaApiParamDescription("startPoint", "射线发射位置")]
    [LuaApiParamDescription("dirction", "射线方向向量")]
    [LuaApiParamDescription("rayLength", "射线长度")]
    public RayCastResult Raycasting(RaySolverFlag flags, Vector3 startPoint, Vector3 dirction, float rayLength)
    {
      if (Handle == IntPtr.Zero)
        throw new Exception("World not create or destroyed");

      var result = PhysicsApi.API.raycasting(Handle, (int)flags, startPoint, dirction, rayLength);
      var rs = new RayCastResult();

      foreach(var o in result.hit_objects)
        rs.HitObjects.Add(GetObjectById(PhysicsApi.API.physics_get_id(o)));
      foreach(var o in result.hit_distances)
        rs.HitDistances.Add(o);

      return rs;
    }
  }
}