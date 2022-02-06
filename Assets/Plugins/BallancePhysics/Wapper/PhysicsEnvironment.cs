using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsEnvironment")]
  [DefaultExecutionOrder(10)]
  [DisallowMultipleComponent]
  /// <summary>
  /// 物理环境
  /// </summary>
  public class PhysicsEnvironment : MonoBehaviour
  {
    [Tooltip("世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效)")]
    /// <summary>
    /// 世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效)
    /// </summary>
    /// <returns></returns>
    public Vector3 Gravity = new Vector3(0, -9.81f, 0);
    [Tooltip("模拟速率（10-100，一秒钟进行物理模拟的速率）(模拟开始后更改此值无效)")]
    /// <summary>
    /// 模拟速率（10-100，一秒钟进行物理模拟的速率）(模拟开始后更改此值无效)
    /// </summary>
    public int SimulationRate = 66;
    [Tooltip("用于物理对象模拟的时间乘以的因子。因此，如果“物理时间因子”为2.0，而不是1.0（正常速度），则物理对象下落的速度将加倍。")]
    /// <summary>
    /// 用于物理对象模拟的时间乘以的因子。因此，如果“物理时间因子”为2.0，而不是1.0（正常速度），则物理对象下落的速度将加倍。
    /// </summary>
    public int TimeFactor = 1;
    [Tooltip("是否在销毁环境时自动删除所有碰撞层")]
    /// <summary>
    /// 是否在销毁环境时自动删除所有碰撞层
    /// </summary>
    public bool DeleteAllSurfacesWhenDestroy = true;
    [Tooltip("是否启用模拟")]
    /// <summary>
    /// 是否启用模拟
    /// </summary>
    public bool Simulate = true;
    [Tooltip("是否自动创建")]
    /// <summary>
    /// 是否自动创建
    /// </summary>
    public bool AutoCreate = true;

    /// <summary>
    /// 所有物理环境
    /// </summary>
    /// <typeparam name="int">Unity场景的buildIndex</typeparam>
    /// <typeparam name="PhysicsWorld"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, PhysicsEnvironment> PhysicsWorlds { get; } = new Dictionary<int, PhysicsEnvironment>();
    /// <summary>
    /// 获取当前场景的物理环境
    /// </summary>
    /// <returns></returns>
    public static PhysicsEnvironment GetCurrentScensePhysicsWorld()
    {
      int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
      if (PhysicsWorlds.TryGetValue(currentScenseIndex, out var a))
        return a;
      return null;
    }

    public IntPtr Handle { get; private set; } = IntPtr.Zero;

    private int createTick = 0;

    /// <summary>
    /// 创建物理环境
    /// </summary>
    public void Create()
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
    /// <summary>
    /// 销毁物理环境
    /// </summary>
    public void Destroy()
    {
      if (Handle != IntPtr.Zero)
      {
        List<PhysicsObject> list = new List<PhysicsObject>(objectsDict.Values);
        foreach(var o in list)
          o.UnPhysicalize(true);

        if(DeleteAllSurfacesWhenDestroy)
          PhysicsApi.API.delete_all_surfaces(Handle);
        PhysicsApi.API.destroy_environment(Handle);
        Handle = IntPtr.Zero;

        int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
        PhysicsWorlds.Remove(currentScenseIndex);
      }
    }

    private void Awake()
    {
      createTick = 2;
    }
    private void OnDestroy()
    {
      if (Handle != IntPtr.Zero)
        Destroy();
    }
    private void FixedUpdate() {
      if(createTick > 0) {
        createTick--;
        if(createTick == 0 && AutoCreate) Create();
      }
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
    public void DeleteAllSurfaces() { PhysicsApi.API.delete_all_surfaces(Handle); }
    /// <summary>
    /// 通过ID查找世界中的物理物体
    /// </summary>
    /// <param name="bodyId">ID</param>
    /// <returns>如果未找到则返回null</returns>
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

    /// <summary>
    /// 射线处理标志
    /// </summary>
    public enum RaySolverFlag
    {
      /// <summary>
      /// 射线将会碰撞全部物体
      /// </summary>
      All = 0,
      /// <summary>
      /// 射线将会碰撞幻影
      /// </summary>
      Phantoms = 1,
      /// <summary>
      /// 射线将会碰撞可移动的物体
      /// </summary>
      Movings = 2,
      /// <summary>
      /// 射线将会碰撞不可移动的物体
      /// </summary>
      Statics = 4
    }
    /// <summary>
    /// 射线碰撞结果
    /// </summary>
    public struct RayCastResult
    {
      /// <summary>
      /// 碰撞的物体
      /// </summary>
      public List<PhysicsObject> HitObjects;
      /// <summary>
      /// 碰撞的物体距离射线发射原点的位置
      /// </summary>
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