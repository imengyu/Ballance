using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ballance2;
using Ballance2.Services;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using static BallancePhysics.PhysicsApi;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsEnvironment")]
  [LuaApiDescription("物理世界承载组件")]
  [DefaultExecutionOrder(10)]
  [DisallowMultipleComponent]
  /// <summary>
  /// 物理世界承载组件
  /// </summary>
  public class PhysicsEnvironment : MonoBehaviour
  {
    [Tooltip("世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效)")]
    [LuaApiDescription("世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效)")]
    /// <summary>
    /// 世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效)
    /// </summary>
    /// <returns></returns>
    public Vector3 Gravity = new Vector3(0, -9.81f, 0);
    [Tooltip("指定y坐标低于这个值的时候自动失活物体. 大于 0 的时候不启用")]
    [LuaApiDescription("指定y坐标低于这个值的时候自动失活物体. 大于 0 的时候不启用")]
    /// <summary>
    /// 指定y坐标低于这个值的时候自动失活物体. 大于 0 的时候不启用
    /// </summary>
    /// <returns></returns>
    public float DePhysicsFall = -5000;
    [Tooltip("模拟速率（10-100，一秒钟进行物理模拟的速率）(模拟开始后更改此值无效)")]
    [LuaApiDescription("模拟速率（10-100，一秒钟进行物理模拟的速率）(模拟开始后更改此值无效)")]
    /// <summary>
    /// 模拟速率（10-100，一秒钟进行物理模拟的速率）(模拟开始后更改此值无效)
    /// </summary>
    public int SimulationRate = 66;
    [LuaApiDescription("用于物理对象模拟的时间乘以的因子。因此，如果“物理时间因子”为2.0，而不是1.0（正常速度），则物理对象下落的速度将加倍。")]
    [Tooltip("用于物理对象模拟的时间乘以的因子。因此，如果“物理时间因子”为2.0，而不是1.0（正常速度），则物理对象下落的速度将加倍。")]
    /// <summary>
    /// 用于物理对象模拟的时间乘以的因子。因此，如果“物理时间因子”为2.0，而不是1.0（正常速度），则物理对象下落的速度将加倍。
    /// </summary>
    public int TimeFactor = 1;
    [LuaApiDescription("是否在销毁环境时自动删除所有碰撞层")]
    [Tooltip("是否在销毁环境时自动删除所有碰撞层")]
    /// <summary>
    /// 是否在销毁环境时自动删除所有碰撞层
    /// </summary>
    public bool DeleteAllSurfacesWhenDestroy = true;
    [LuaApiDescription("是否启用模拟")]
    [Tooltip("是否启用模拟")]
    /// <summary>
    /// 是否启用模拟
    /// </summary>
    public bool Simulate = true;
    [LuaApiDescription("是否自动创建")]
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
    [LuaApiDescription("所有物理环境索引")]
    public static Dictionary<int, PhysicsEnvironment> PhysicsWorlds { get; } = new Dictionary<int, PhysicsEnvironment>();
    /// <summary>
    /// 获取当前场景的物理环境
    /// </summary>
    /// <returns>如果当前场景没有创建物理环境，则返回null</returns>
    [LuaApiDescription("获取当前场景的物理环境", "如果当前场景没有创建物理环境，则返回null")]
    public static PhysicsEnvironment GetCurrentScensePhysicsWorld()
    {
      int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
      if (PhysicsWorlds.TryGetValue(currentScenseIndex, out var a))
        return a;
      return null;
    }

    /// <summary>
    /// 获取当前物理环境的底层指针
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前物理环境的底层指针")]
    public IntPtr Handle { get; private set; } = IntPtr.Zero;

    private ErrorReportCallback callback = (int level, int len, IntPtr _msg) =>
    {
      string msg = Marshal.PtrToStringAnsi(_msg, len);
      if (level == sInfo)
        Debug.Log(msg);
      else if (level == sWarning)
        Debug.LogWarning(msg);
      else if (level == sError)
        Debug.LogError(msg);
      else 
        Debug.Log(msg);
      return 1;
    };

    private GameTimeMachine.GameTimeMachineTimeTicket fixedUpdateTicket = null;

    /// <summary>
    /// 手动创建物理环境
    /// </summary>
    [LuaApiDescription("手动创建物理环境")]
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
        Handle = PhysicsApi.API.create_environment(Gravity, 1.0f / SimulationRate, -2147483647, layerNames.GetGroupFilterMasks(), Marshal.GetFunctionPointerForDelegate(callback));
      
        fixedUpdateTicket = GameManager.GameTimeMachine.RegisterFixedUpdate(_FixedUpdate, 15);
      }
    }

    private bool destroyLock = false;

    /// <summary>
    /// 手动销毁物理环境
    /// </summary>
    [LuaApiDescription("手动销毁物理环境")]
    public void Destroy()
    {
      if (Handle != IntPtr.Zero)
      {
        if(fixedUpdateTicket != null)
        {
          fixedUpdateTicket.Unregister();
          fixedUpdateTicket = null;
        }

        Simulate = false;
        destroyLock = true;

        LinkedListNode<PhysicsObject> obj = objects.First;
        while (obj != null) {
          var bodyCurrent = obj.Value;
          if(bodyCurrent.IsPhysicalized) {
            obj.Value.UnPhysicalize(true);
            obj = obj.Next;
            continue;
          }
        }
        Debug.Log("Destroy: " + objects.Count + " PhysicsObject destroyed. ");
        objects.Clear();

        if(DeleteAllSurfacesWhenDestroy)
          PhysicsApi.API.delete_all_surfaces(Handle);
        PhysicsApi.API.destroy_environment(Handle);
        Handle = IntPtr.Zero;

        int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
        PhysicsWorlds.Remove(currentScenseIndex);

        destroyLock = false;
      }
    }

    private void Awake() { 
      if(AutoCreate)
        StartCoroutine(LateCreate()); 
    }
    private void OnDestroy()
    {
      if (Handle != IntPtr.Zero)
        Destroy();
    }
    private IEnumerator LateCreate() {
      yield return new WaitForSeconds(0.02f);
      Create();
    }

    /// <summary>
    /// 获取上一帧的物理执行时间 (秒)
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取上一帧的物理执行时间 (秒)")]
    public float PhysicsTime { get; private set; }
    /// <summary>
    /// 获取当前激活的物理对象个数
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前激活的物理对象个数")]
    public int PhysicsActiveBodies { get { return _PhysicsActiveBodies; } }
    /// <summary>
    /// 获取当前所有的物理对象个数
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前所有的物理对象个数")]
    public int PhysicsBodies { get; private set; }
    /// <summary>
    /// 获取当前模拟的物理时间
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前模拟的物理时间")]
    public double PhysicsSimuateTime { get { return _PhysicsSimuateTime; } }
    /// <summary>
    /// 获取当前正在恒力推动的物理对象个数
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前正在恒力推动的物理对象个数")]
    public int PhysicsConstantPushBodies { get { return _PhysicsConstantPushBodies; } }
    /// <summary>
    /// 获取当前坠落回收物理对象个数
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前坠落回收物理对象个数")]
    public int PhysicsFallCollectBodies { get { return _PhysicsFallCollectBodies; } }
    /// <summary>
    /// 获取当前固定物理对象个数
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前固定物理对象个数")]
    public int PhysicsFixedBodies { get { return _PhysicsFixedBodies; } }
    /// <summary>
    /// 获取当前更新物理对象个数
    /// </summary>
    /// <value></value>
    [LuaApiDescription("获取当前更新物理对象个数")]
    public int PhysicsUpdateBodies { get { return _PhysicsUpdateBodies; } }
    /// <summary>
    /// 获取物理力大小系数。
    /// </summary>
    [LuaApiDescription("获取物理力大小系数。")]
    public float PhysicsFactorFinalValue = 1;

    private int _PhysicsActiveBodies = 0;
    private int _PhysicsConstantPushBodies = 0;
    private int _PhysicsFallCollectBodies = 0;
    private int _PhysicsFixedBodies = 0;
    private int _PhysicsUpdateBodies = 0;
    private double _PhysicsSimuateTime = 0;
    private bool lastPauseIsSimuate = false;
    private List<PhysicsObject> nextNeedUnFallCollectObjects = new List<PhysicsObject>();

    private void _FixedUpdate() {
      if(Simulate && Handle != IntPtr.Zero) {

        if(destroyLock) {
          Debug.Log("Bad state, no Simulate at destroy!");
          return;
        }

        Profiler.BeginSample("PhysicsEnvironmentUpdate");

        PhysicsFactorFinalValue = 1;//TimeFactor;//(Time.fixedDeltaTime / (1.0f / SimulationRate));
        
        //设置计数

	      float startTime = Time.realtimeSinceStartup;
        PhysicsBodies = objects.Count;
        _PhysicsUpdateBodies = 0;
        _PhysicsConstantPushBodies = 0;
        _PhysicsFallCollectBodies = 0;
        _PhysicsFixedBodies = 0;

        //先更新推力
        float[] dat = new float[4];
        LinkedListNode<PhysicsObject> obj = objects.First;
  
        //模拟
        Profiler.BeginSample("PhysicsEnvironmentSimulate");
        //var max = 3;
        //for (var i = 0; i < max; i++)
          PhysicsApi.API.environment_simulate_dtime(Handle, /* Time.fixedDeltaTime */(1.0f / SimulationRate));
        Profiler.EndSample();

        PhysicsApi.API.do_update_all(Handle);
        //获取一些参数
        PhysicsApi.API.get_stats(Handle, ref _PhysicsActiveBodies, ref _PhysicsSimuateTime);

        //更新位置到C#
        obj = objects.First;
        while (obj != null) {
          var bodyCurrent = obj.Value;
          if(bodyCurrent.Fixed) {
            obj = obj.Next;
            _PhysicsFixedBodies++;
            continue;
          }

          var t = bodyCurrent.gameObject.transform;
          IntPtr ptr = bodyCurrent.Handle; //pos 0
          ptr = IntPtr.Add(ptr, Marshal.SizeOf<int>()); //pos 1
          Marshal.Copy(ptr, dat, 0, 3);      //float[3]

          var p = new Vector3(dat[0], dat[1], dat[2]);
          t.position = p;

          ptr = IntPtr.Add(ptr, Marshal.SizeOf<float>() * 3); //pos 4
          Marshal.Copy(ptr, dat, 0, 4);      //float[4]

          t.rotation = new Quaternion(dat[0], dat[1], dat[2], dat[3]);
          _PhysicsUpdateBodies++;

          //坠落回收
          if(DePhysicsFall < 0 && p.y < DePhysicsFall) {
            //DePhysics and DeActive
            nextNeedUnFallCollectObjects.Add(bodyCurrent);
            _PhysicsFallCollectBodies++;
          }
          else if(bodyCurrent.EnableConstantForce) {
            bodyCurrent.DoApplyConstantForce();
            _PhysicsConstantPushBodies++;
          }

          obj = obj.Next;
        }

        Profiler.EndSample();

        //更新碰撞处理器
        if(GameTimeMachine.FixedUpdateTick % 48 == 0) {
          Profiler.BeginSample("PhysicsEnvironmentContactEvent");
          PhysicsApi.API.do_update_all_physics_contact_detection(Handle);
          Profiler.EndSample();
        }
        //需要反物理化坠落的物体
        if(nextNeedUnFallCollectObjects.Count > 0) {
          for (var i = nextNeedUnFallCollectObjects.Count - 1; i >= 0; i--)
          {
            var bodyCurrent = nextNeedUnFallCollectObjects[i];
            bodyCurrent.UnPhysicalize(true);
            bodyCurrent.gameObject.SetActive(false);
          }
          nextNeedUnFallCollectObjects.Clear();
        }

        //结束时间
        PhysicsTime = Time.realtimeSinceStartup - startTime;
      }
    }

    [SLua.DoNotToLua]
    public static void HandleEditorPause() {
      foreach(var world in PhysicsWorlds.Values) {
        world.lastPauseIsSimuate = world.Simulate;
        world.Simulate = false;
      }
    }
    [SLua.DoNotToLua]
    public static void HandleEditorPlay() {
      foreach(var world in PhysicsWorlds.Values) {
        if(world.lastPauseIsSimuate) {
          world.Simulate = true;
          world.lastPauseIsSimuate = false;
        }
      }
    }

    private Dictionary<string, int> dictSystemGroup = new Dictionary<string, int>();
    private LinkedList<PhysicsObject> objects = new LinkedList<PhysicsObject>();

    /// <summary>
    /// [由PhysicsObject自动调用，请勿手动调用]
    /// </summary>
    /// <param name="id"></param>
    /// <param name="body"></param>
    internal void AddPhysicsObject(int id, PhysicsObject body)
    {
      if(destroyLock) {
        Debug.Log("Bad state, not allow create PhysicsObject at destroy!");
        return;
      }

      objects.AddLast(body);
    }
    /// <summary>
    /// [由PhysicsObject自动调用，请勿手动调用]
    /// </summary>
    /// <param name="body"></param>
    internal void RemovePhysicsObject(PhysicsObject body)
    {
      if(!destroyLock) 
        objects.Remove(body);
    }

    /// <summary>
    /// 通过名称获取碰撞子组信息。如果没有指定名称的子组，则自动生成
    /// </summary>
    /// <param name="name">子组名称</param>
    /// <returns>返回碰撞组子ID</returns>
    [LuaApiDescription("获取上一帧的物理执行时间 (秒)", "返回碰撞组子ID")]
    [LuaApiParamDescription("name", "子组名称")]
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
    /// <param name="id">ID</param>
    /// <returns>如果未找到则返回null</returns>
    [LuaApiDescription("通过ID查找世界中的物理物体", "如果未找到则返回null")]
    [LuaApiParamDescription("id", "ID")]
    public PhysicsObject GetObjectById(int id)
    {
      LinkedListNode<PhysicsObject> obj = objects.First;
      while (obj != null) {
        if(obj.Value.Id == id)
          return obj.Value;
        obj = obj.Next;
      }
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
    /// 从指定位置发射射线，获取射线是否与指定物体碰撞。
    /// </summary>
    /// <param name="obj">指定物体</param>
    /// <param name="startPoint">射线发射位置</param>
    /// <param name="dirction">射线方向向量</param>
    /// <param name="rayLength">射线长度</param>
    /// <param name="distance">第一个碰撞物体的距离</param>
    /// <returns>如果射线有和物体碰撞，则返回true，否则返回false</returns>
    [LuaApiDescription("从指定位置发射射线，获取射线是否与指定物体碰撞。", "如果射线有和物体碰撞，则返回true，否则返回false")]
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

    /// <summary>
    /// 射线处理标志
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("射线处理标志")]
    public enum RaySolverFlag
    {
      /// <summary>
      /// 射线将会碰撞全部物体
      /// </summary>
      [LuaApiDescription("射线将会碰撞全部物体")]
      All = 0,
      /// <summary>
      /// 射线将会碰撞幻影
      /// </summary>
      [LuaApiDescription("射线将会碰撞幻影")]
      Phantoms = 1,
      /// <summary>
      /// 射线将会碰撞可移动的物体
      /// </summary>
      Movings = 2,
      /// <summary>
      /// 射线将会碰撞不可移动的物体
      /// </summary>
      [LuaApiDescription("射线将会碰撞不可移动的物体")]
      Statics = 4
    }
    /// <summary>
    /// 射线碰撞结果
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("射线碰撞结果")]
    public struct RayCastResult
    {
      /// <summary>
      /// 碰撞的物体
      /// </summary>
      [LuaApiDescription("碰撞的物体")]
      public List<PhysicsObject> HitObjects;

      /// <summary>
      /// 获取碰撞的物体数量
      /// </summary>
      /// <returns></returns>
      [LuaApiDescription("获取碰撞的物体数量")]
      public int GetHitObjectsCount() { return HitObjects.Count; }
      /// <summary>
      /// 获取第几个碰撞的物体
      /// </summary>
      /// <param name="index">索引</param>
      /// <returns></returns>
      [LuaApiDescription("获取第几个碰撞的物体")]
      public PhysicsObject GetHitObjectsAt(int index) { return HitObjects[index]; }

      /// <summary>
      /// 碰撞的物体距离射线发射原点的位置
      /// </summary>
      [LuaApiDescription("碰撞的物体距离射线发射原点的位置")]
      public List<float> HitDistances;

      
      /// <summary>
      /// 获取碰撞的距离射线发射原点的位置信息数量
      /// </summary>
      /// <returns></returns>
      [LuaApiDescription("获取碰撞的距离射线发射原点的位置信息数量")]
      public int GetHitDistancesCount() { return HitDistances.Count; }
      /// <summary>
      /// 获取指定第几个碰撞的距离射线发射原点的位置信息
      /// </summary>
      /// <param name="index">索引</param>
      /// <returns></returns>
      [LuaApiDescription("获取指定第几个碰撞的距离射线发射原点的位置信息")]
      public float GetHitDistancesAt(int index) { return HitDistances[index]; }
    }

    /// <summary>
    /// 从指定位置发射射线，获取射线碰撞的全部物体
    /// </summary>
    /// <param name="flags">射线处理标志</param>
    /// <param name="startPoint">射线发射位置</param>
    /// <param name="dirction">射线方向向量</param>
    /// <param name="rayLength">射线长度</param>
    /// <returns>返回碰撞信息</returns>
    [LuaApiDescription("从指定位置发射射线，获取射线碰撞的全部物体", "返回碰撞信息")]
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