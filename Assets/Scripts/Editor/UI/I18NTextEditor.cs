using UnityEditor;

[CustomEditor(typeof(Ballance2.Sys.UI.I18NText), true)]
class I18NTextEditor : Editor
{
    private Ballance2.Sys.UI.I18NText myScript;

    private SerializedProperty m_I18N;
    private SerializedProperty m_ResourceKey;
    private SerializedProperty m_DefaultString;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        myScript = (Ballance2.Sys.UI.I18NText)target;

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

