using UnityEditor;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* PackagePackerWindow.cs
* 
* 用途：
* 选择平台窗口
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Editor.Modding
{
  public class WindowChoosePlatform : EditorWindow
  {
    public WindowChoosePlatform()
    {
      titleContent = new GUIContent("选择平台");
    }

    private SerializedObject serializedObject;
    private SerializedProperty pPackTarget = null;

    public delegate void OnChooseEvent(BuildTarget target);

    public OnChooseEvent OnChoose;

    [SerializeField]
    private BuildTarget packTarget = BuildTarget.NoTarget;

    private GUIStyle groupBox = null;

    private void OnEnable()
    {
      serializedObject = new SerializedObject(this);
      packTarget = (BuildTarget)EditorPrefs.GetInt("WindowChoosePlatformDialogPackTarget", (int)BuildTarget.NoTarget);
      pPackTarget = serializedObject.FindProperty("packTarget");
    }
    private void OnDisable()
    {
      EditorPrefs.SetInt("WindowChoosePlatformDialogPackTarget", (int)packTarget);
      EditorUtility.ClearProgressBar();
    }
    private void OnGUI()
    {
      bool close = false;
      serializedObject.Update();

      if (groupBox == null)
        groupBox = GUI.skin.FindStyle("GroupBox");

      EditorGUI.BeginChangeCheck();

      EditorGUILayout.BeginVertical(groupBox);
      EditorGUILayout.PropertyField(pPackTarget, new GUIContent("目标平台 "));

      GUILayout.Space(50);

      if (GUILayout.Button("确定")) {
        close = true;
      }

      EditorGUILayout.EndVertical();

      if (EditorGUI.EndChangeCheck())
        serializedObject.ApplyModifiedProperties();

      if(close) {
        OnChoose?.Invoke(packTarget);
        Close();
      }
    }
  }
}
