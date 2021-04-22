using Ballance2.Sys.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Updown), true)]
class UpdownInspector : Editor
{
    private Updown myScript;
    private SerializedProperty pValue;
    private SerializedProperty pMinValue;
    private SerializedProperty pMaxValue;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        myScript = (Updown)target;

        EditorGUI.BeginChangeCheck();

        float newValue = EditorGUILayout.Slider(new GUIContent("Value", pValue.tooltip), pValue.floatValue, pMinValue.floatValue, pMaxValue.floatValue);
        if (newValue != pValue.floatValue)
        {
            GUI.changed = true;
            pValue.floatValue = newValue;
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            myScript.UpdateValue();
        }

        base.OnInspectorGUI();
    }
    private void OnEnable()
    {
        pValue = serializedObject.FindProperty("_value");
        pMinValue = serializedObject.FindProperty("MinValue");
        pMaxValue = serializedObject.FindProperty("MaxValue");
    }
    private void OnDisable()
    {
    }

}

