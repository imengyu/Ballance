using PhysicsRT;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsConstraint), true)]
[CanEditMultipleObjects]
class PhysicsConstraintEditor : Editor
{
    private PhysicsConstraint instance;
    

    public PhysicsConstraintEditor() {
    }

    private void OnEnable() {
        pBreakable = serializedObject.FindProperty("m_Breakable");
        pThreshold = serializedObject.FindProperty("m_Threshold");
        pMaximumAngularImpulse = serializedObject.FindProperty("m_MaximumAngularImpulse");
        pMaximumLinearImpulse = serializedObject.FindProperty("m_MaximumLinearImpulse");
    }
    private void OnDisable() {

    }

    private SerializedProperty pBreakable;
    private SerializedProperty pThreshold;
    private SerializedProperty pMaximumAngularImpulse;
    private SerializedProperty pMaximumLinearImpulse;

    private bool bBreakableFoldout = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        instance = (PhysicsConstraint)target;

        EditorGUI.BeginChangeCheck();

        if(EditorApplication.isPlaying)
            EditorGUILayout.HelpBox("Constraint values can't change at runtime.", MessageType.Warning);

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
        DrawDefaultInspector();
        EditorGUI.EndDisabledGroup();        
        
        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit"), base.target);

        bBreakableFoldout = EditorGUILayout.Foldout(bBreakableFoldout, "Breakable");
        if(bBreakableFoldout) {        

            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

            EditorGUILayout.PropertyField(pBreakable);
            EditorGUILayout.PropertyField(pThreshold);
            EditorGUILayout.PropertyField(pMaximumAngularImpulse);
            EditorGUILayout.PropertyField(pMaximumLinearImpulse);
        
            EditorGUI.EndDisabledGroup();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}