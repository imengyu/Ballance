using UnityEditor;
using UnityEngine;

class StaticEditors : ScriptableObject
{

    [MenuItem("PhysicsRT/Edit PhysicsBody tag names")]
    public static void EditCustomPhysicsBodyTagNames()
    {
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<CustomPhysicsBodyTagNames>("Assets/Resources/CustomPhysicsBodyTagNames.asset");
    }
    [MenuItem("PhysicsRT/Edit Physics material tag names")]
    public static void EditCustomMaterialTagNames()
    {
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<CustomPhysicsMaterialTagNames>("Assets/Resources/CustomMaterialTagNames.asset");
    }
    [MenuItem("PhysicsRT/Edit Physics layer names")]
    public static void EditPhysicsLayerNames()
    {
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<PhysicsLayerNames>("Assets/Resources/PhysicsLayerNames.asset");
    }
    
}
