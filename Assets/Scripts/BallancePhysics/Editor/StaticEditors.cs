using UnityEditor;
using UnityEngine;

namespace BallancePhysics.Editor
{
  class StaticEditors : ScriptableObject
  {
    [MenuItem("Ballance/Physics/Edit Physics layer names")]
    public static void EditPhysicsLayerNames()
    {
      Selection.activeObject = AssetDatabase.LoadAssetAtPath<PhysicsLayerNames>("Assets/Resources/BallancePhysicsLayerNames.asset");
    }
  }
}
