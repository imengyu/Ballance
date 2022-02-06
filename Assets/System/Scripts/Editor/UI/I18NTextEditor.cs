using Ballance2.UI.Core.Controls;
using UnityEditor;

[CustomEditor(typeof(I18NText), true)]
class I18NTextEditor : Editor
{
    private I18NText myScript;

    private SerializedProperty m_I18N;
    private SerializedProperty m_ResourceKey;
    private SerializedProperty m_DefaultString;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        myScript = (I18NText)target;

        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            myScript.UpdateText();
        }

    }
    private void OnEnable()
    {
        m_I18N = serializedObject.FindProperty("m_I18N");
        m_ResourceKey = serializedObject.FindProperty("m_ResourceKey");
        m_DefaultString = serializedObject.FindProperty("m_DefaultString");
    }
    private void OnDisable()
    {
    }
}

