
using UnityEditor;
using UnityEngine;

namespace BallancePhysics
{
  /// <summary>
  /// 物理引擎配置
  /// </summary>
  public class PhysicsOptions : ScriptableObject
  {
      [Tooltip("是否显示控制台窗口（仅Win版本有效）")]
      /// <summary>
      /// 是否显示控制台窗口（仅Win版本有效）
      /// </summary>
      public bool ShowConsole = false;
      [Tooltip("")]
      [Range(8, 64)]
      /// <summary>
      /// 小内存池的大小
      /// </summary>
      public int SmallPoolSize = 16;

      #region Base

      private static PhysicsOptions _instance = null;
      
      public static PhysicsOptions Instance{
          get {
              if(_instance == null) {
                  _instance = Resources.Load<PhysicsOptions>("PhysicsOptions");

              #if UNITY_EDITOR
                  if(_instance == null){
                      _instance =  PhysicsOptions.CreateInstance<PhysicsOptions>();
                      try
                      {
                          AssetDatabase.CreateAsset(_instance, "Assets/Resources/BallancePhysicsOptions.asset");
                      }
                      catch {
                          
                      }
                  }
              #endif

              }
              return _instance;
          }
      }
      
      #if UNITY_EDITOR
      [MenuItem("Ballance/Physics/Settings")]
      public static void Open() {
        Selection.activeObject = Instance;
      }
      #endif

      #endregion
  }
}
