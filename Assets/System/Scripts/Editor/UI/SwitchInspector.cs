using Ballance2.UI.Core.Controls;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToggleEx), true)]
class SwitchInspector : Editor
{
    private ToggleEx myScript;
    private SerializedProperty pIsOn;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        myScript = (ToggleEx)target;

        EditorGUI.BeginChangeCheck();

        bool newState = EditorGUILayout.Toggle(new GUIContent("IsOn", pIsOn.tooltip), 
            pIsOn.boolValue);
        if (newState != pIsOn.boolValue)
        {
            GUI.changed = true;
            pIsOn.boolValue = newState;
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            myScript.UpdateOn();
        }

        base.OnInspectorGUI();
    }
    private void OnEnable()
    {
        pIsOn = serializedObject.FindProperty("on");
    }
    private void OnDisable()
    {
    }

}

