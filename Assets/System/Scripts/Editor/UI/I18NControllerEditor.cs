using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(I18NController), true)]
class I18NTextEditor : Editor
{
  private I18NController myScript;

  private SerializedProperty m_ResourceKey;
  private SerializedProperty m_DefaultString;
  private SerializedProperty m_ControlText;
  private bool firstSet = true;

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    myScript = (I18NController)target;

    EditorGUI.BeginChangeCheck();

    EditorGUILayout.BeginHorizontal();

    EditorGUILayout.PropertyField(m_ResourceKey);
    if (GUILayout.Button("Pick"))
      ShowPickerWindow();

    EditorGUILayout.EndHorizontal();

    EditorGUILayout.PropertyField(m_DefaultString);
    EditorGUILayout.PropertyField(m_ControlText);

    if (firstSet && m_ControlText != null && m_ControlText.objectReferenceValue == null && myScript != null) {
      m_ControlText.objectReferenceValue = myScript.gameObject.GetComponent<TMP_Text>();
      firstSet = false;
    }

    if (EditorGUI.EndChangeCheck())
    {
      UpdateText();
      serializedObject.ApplyModifiedProperties();
    }
  }

  private I18NPicker pickerWindow = null;

  private void UpdateText()
  {
    if (EditorApplication.isPlaying)
      myScript.UpdateLocalization();
    else if (m_ControlText.objectReferenceValue)
      (m_ControlText.objectReferenceValue as TMP_Text).text = m_DefaultString.stringValue;
  }

  private void ShowPickerWindow()
  {
    if (pickerWindow == null)
    {
      pickerWindow = EditorWindow.GetWindow<I18NPicker>(true);
      pickerWindow.onSelect = (v, a) => {
        m_ResourceKey.stringValue = v;
        m_DefaultString.stringValue = a;
        UpdateText();
        serializedObject.ApplyModifiedProperties();
      };
    }
    pickerWindow.Show();
  }

  private void OnEnable()
  {
    m_ControlText = serializedObject.FindProperty("ControlText");
    m_ResourceKey = serializedObject.FindProperty("ResourceKey");
    m_DefaultString = serializedObject.FindProperty("DefaultString");
  }
  private void OnDisable()
  {
    if (pickerWindow != null)
    {
      pickerWindow.Close();
      pickerWindow = null;
    }
  }
}

