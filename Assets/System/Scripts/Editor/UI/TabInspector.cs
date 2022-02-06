using Ballance2.UI.Core.Controls;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tab), true)]
class TabInspector : Editor
{
  private Tab myScript;
  private SerializedProperty pIsOn;

  private string addTab = "";
  private string addTabTitle = "";

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    myScript = (Tab)target;

    EditorGUI.BeginChangeCheck();

    //Add
    GUILayout.BeginVertical("box");

    GUILayout.BeginHorizontal();
    GUILayout.Label("Name: ");
    addTab = GUILayout.TextField(addTab);
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Label("Title: ");
    addTabTitle = GUILayout.TextField(addTabTitle);
    GUILayout.EndHorizontal();

    if(GUILayout.Button("Add tab")) {
      myScript.AddTab(addTab, addTabTitle, null);
      addTab = "";
      addTabTitle = "";
    }

    GUILayout.EndVertical();


    if (EditorGUI.EndChangeCheck())
    {
      serializedObject.ApplyModifiedProperties();
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

