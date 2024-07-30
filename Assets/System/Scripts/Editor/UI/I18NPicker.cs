using System;
using System.Collections.Generic;
using Ballance2.Services.I18N;
using UnityEditor;
using UnityEngine;

public class I18NPicker : EditorWindow
{
  public I18NPicker()
  {
    titleContent = new GUIContent("选择国际化字符串");
  }

  private Vector2 scrollPosition = Vector2.zero;
  private Dictionary<string, string> i18NDict = new Dictionary<string, string>();
  private string filter = "";

  public Action<string, string> onSelect;

  private void OnEnable()
  {
    I18NProvider.DirectLoadLanguageResources(i18NDict, AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Game/PackageLanguageRes.xml").text);
  }
  private void OnDisable()
  {
  }

  private void OnGUI()
  {    
    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

    filter = EditorGUILayout.TextField(filter);

    EditorGUILayout.BeginVertical();
    EditorGUILayout.Space(50);

    foreach (var item in i18NDict)
    {
      if (filter != "" && !item.Key.Contains(filter, StringComparison.OrdinalIgnoreCase) && !item.Value.Contains(filter, StringComparison.OrdinalIgnoreCase))
        continue;

      EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button(item.Key, GUILayout.Width(210)))
      {
        onSelect.Invoke(item.Key, item.Value);
        Close();
      }
      GUILayout.Label(item.Value);
      EditorGUILayout.EndHorizontal();
    }

    EditorGUILayout.EndVertical();
    EditorGUILayout.EndScrollView();
  }
}
