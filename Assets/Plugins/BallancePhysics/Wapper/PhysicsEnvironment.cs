using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallancePhysics.Wapper
{
  public class PhysicsEnvironment : MonoBehaviour
  {
    [Tooltip("世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效，请使用 SetGravity 更改)")]
    private Vector3 Gravity = new Vector3(0, -9.81f, 0);

    /// <summary>
    /// 所有物理场景
    /// </summary>
    /// <typeparam name="int">Unity场景的buildIndex</typeparam>
    /// <typeparam name="PhysicsWorld"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, PhysicsEnvironment> PhysicsWorlds { get; } = new Dictionary<int, PhysicsEnvironment>();
    /// <summary>
    /// 获取当前场景的物理场景
    /// </summary>
    /// <returns></returns>
    public static PhysicsEnvironment GetCurrentScensePhysicsWorld()
    {
      int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
      if (PhysicsWorlds.TryGetValue(currentScenseIndex, out var a))
        return a;
      return null;
    }

    private IntPtr handle = IntPtr.Zero;

    private void Awake()
    {
      int currentScenseIndex = SceneManager.GetActiveScene().buildIndex;
      if (PhysicsWorlds.ContainsKey(currentScenseIndex))
        Debug.LogError("There can only one PhysicsWorld instance in a scense.");
      else
      {
        var layerNames = Resources.Load<PhysicsLayerNames>("PhysicsLayerNames");          
        Debug.Assert(layerNames != null);

        PhysicsWorlds.Add(currentScenseIndex, this);
        handle = PhysicsApi.API.create_environment(Gravity, 66, -2147483647, layerNames.GetGroupFilterMasks());
      }
    }
  }
}