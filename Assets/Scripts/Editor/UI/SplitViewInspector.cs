using Ballance2.Sys.UI;
using UnityEditor;

[CustomEditor(typeof(SplitView), true)]
class SplitViewInspector : Editor
{
    private SplitView myScript;

    private SerializedProperty pValue;
    private SerializedProperty pMin;
    private SerializedProperty pMax;
    private SerializedProperty pDirection;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        myScript = (SplitView)target;

        EditorGUI.BeginChangeCheck();

        float val = EditorGUILayout.Slider("Value", pValue.floatValue, myScript.min, myScript.max);
        if (val != pValue.floatValue)
            pValue.floatValue = val;

        val = EditorGUILayout.Slider("Min", pMin.floatValue, 0, myScript.max);
        if (val != pMin.floatValue)
            pMin.floatValue = val;
        val = EditorGUILayout.Slider("Max", pMax.floatValue, myScript.min, 1);
        if (val != pMax.floatValue)
            pMax.floatValue = val;

        SplitViewDirection direction = (SplitViewDirection)EditorGUILayout.EnumPopup("Direction",
            (SplitViewDirection)pDirection.enumValueIndex);
        if (direction != (SplitViewDirection)pDirection.enumValueIndex)
            pDirection.enumValueIndex = (int)direction;

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            myScript.UpdateVal();
        }

        base.OnInspectorGUI();
    }
    private void OnEnable()
    {
        pValue = serializedObject.FindProperty("_value");
        pMin = serializedObject.FindProperty("_min");
        pMax = serializedObject.FindProperty("_max");
        pDirection = serializedObject.FindProperty("_direction");
    }
    private void OnDisable()
    {
    }
}

