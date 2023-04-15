using UnityEditor;
using UnityEngine;
using Ballance2.Entry;
using System.Collections.Generic;
using System.IO;
using Ballance2.Res;

[CustomEditor(typeof(GameDebugEntry), true)]
public class GameDebugEntryEditor : Editor
{
  private new GameDebugEntry target = null;

  private SerializedProperty ModulName;
  private SerializedProperty ModulInstance;
  private SerializedProperty ModulTestFloor;
  private SerializedProperty ModulTestUI;
  private SerializedProperty LevelName;
  private List<string> levelNames = new List<string>();

  private void OnEnable()
  {
    ModulName = serializedObject.FindProperty("ModulName");
    ModulInstance = serializedObject.FindProperty("ModulInstance");
    ModulTestFloor = serializedObject.FindProperty("ModulTestFloor");
    ModulTestUI = serializedObject.FindProperty("ModulTestUI");
    LevelName = serializedObject.FindProperty("LevelName");

    levelNames.Clear();
    DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_LEVEL_FOLDER);
    DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
    for (int i = 0; i < dirs.Length; i++)
      levelNames.Add(dirs[i].Name);
  }
  private void OnDisable()
  {
  }

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    target = (GameDebugEntry)base.target;

    EditorGUI.BeginChangeCheck();

    EditorGUILayout.PropertyField(ModulName);
    EditorGUILayout.PropertyField(ModulInstance);
    EditorGUILayout.PropertyField(ModulTestFloor);
    EditorGUILayout.PropertyField(ModulTestUI);
    
    var newIndex = EditorGUILayout.Popup(LevelName.displayName, levelNames.IndexOf(LevelName.stringValue), levelNames.ToArray());
    if (newIndex >= 0 && newIndex < levelNames.Count)
      LevelName.stringValue = levelNames[newIndex];

    if (EditorGUI.EndChangeCheck())
    {
      serializedObject.ApplyModifiedProperties();
    }
  }
}