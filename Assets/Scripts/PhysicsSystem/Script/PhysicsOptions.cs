
using UnityEditor;
using UnityEngine;

[SLua.CustomLuaClass]
public class PhysicsOptions : ScriptableObject
{
    [Tooltip("是否启用多线程模拟")]
    public bool EnableMultithreaded = false;
    [Tooltip("小内存池的大小")]
    [Range(8, 2048)]
    public int SmallPoolSize = 32;
    [Tooltip("更新内存缓冲区的大小")]
    [Range(16, 2048)]
    public int UpdateBufferSize = 128;

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
                        AssetDatabase.CreateAsset(_instance, "Assets/Resources/PhysicsOptions.asset");
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
    [MenuItem("PhysicsRT/Settings")]
    public static void Open() {
        Selection.activeObject = Instance;
    }
    #endif

    #endregion
}
