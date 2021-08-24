using PhysicsRT;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsSpring), true)]
[CanEditMultipleObjects]
class PhysicsSpringEditor : Editor
{
    public PhysicsSpringEditor() {
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit"), base.target);
    }
}