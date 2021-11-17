
using UnityEditor;
using UnityEngine;

namespace BallancePhysics
{
  [SLua.CustomLuaClass]
  public class PhysicsOptions : ScriptableObject
  {
      [Tooltip("是否显示控制台窗口（仅Win版本有效）")]
      public bool ShowConsole = false;
      [Tooltip("小内存池的大小")]
      [Range(8, 64)]
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
      [SLua.DoNotToLua]
      [MenuItem("Ballance/Physics/Settings")]
      public static void Open() {
          Selection.activeObject = Instance;
      }
      #endif

      #endregion
  }
}
