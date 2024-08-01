using Ballance2.UI;
using UnityEditor;

[CustomEditor(typeof(UIKeyButton), true)]
class UIKeyButtonEditor : Editor
{
  private UIKeyButton myScript;

  private bool groupOpenBase = false;
  private bool groupOpenSet = true;

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    myScript = (UIKeyButton)target;

    EditorGUI.BeginChangeCheck();

    groupOpenBase = EditorGUILayout.Foldout(groupOpenBase, "Button");
    if (groupOpenBase)
      base.OnInspectorGUI();

    groupOpenSet = EditorGUILayout.Foldout(groupOpenSet, "Extend");
    if (groupOpenSet)
      OnInspectorGUIExtend();

    if (EditorGUI.EndChangeCheck())
    {
      serializedObject.ApplyModifiedProperties();
    }
  }

  
  private SerializedProperty keyboardText;
  private SerializedProperty actionText;
  private SerializedProperty keyboardBox;
  private SerializedProperty controllerIcon;
  private SerializedProperty actionName;
  private SerializedProperty controlName;

  private void OnInspectorGUIExtend()
  {
    EditorGUILayout.PropertyField(keyboardText);
    EditorGUILayout.PropertyField(actionText);
    EditorGUILayout.PropertyField(keyboardBox);
    EditorGUILayout.PropertyField(controllerIcon);
    EditorGUILayout.PropertyField(actionName);
    EditorGUILayout.PropertyField(controlName);
  }

  private void OnEnable()
  { 
    keyboardText = serializedObject.FindProperty("keyboardText");
    actionText = serializedObject.FindProperty("actionText");
    keyboardBox = serializedObject.FindProperty("keyboardBox");
    controllerIcon = serializedObject.FindProperty("controllerIcon");
    actionName = serializedObject.FindProperty("actionName");
    controlName = serializedObject.FindProperty("controlName");
  }
  private void OnDisable()
  {

  }
}

