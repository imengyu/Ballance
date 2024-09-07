using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MyFindMissingWindow : EditorWindow
{
  [MenuItem("Tools/������ʧ���õ���Դ")]
  static void ShowWindow()
  {
    GetWindow<MyFindMissingWindow>("����Missing��Դ").Show();
    Find();
  }

  static Dictionary<Object, List<Object>> prefabs = new Dictionary<Object, List<Object>>();
  static Dictionary<Object, List<string>> refPaths = new Dictionary<Object, List<string>>();

  //Ѱ��missing����Դ
  static void Find()
  {
    string[] paths = AssetDatabase.GetAllAssetPaths();
    //��������prefab��Դ
    var gos = paths.Where(path => path.EndsWith("prefab")).Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path));

    foreach (var item in gos)
    {
      GameObject go = item as GameObject;
      if (go == null)
      {
        continue;
      }
      Component[] cps = go.GetComponentsInChildren<Component>(true);
      foreach (var cp in cps)
      {
        if (cp == null)  //���Ϊ��
        {
          if (!prefabs.ContainsKey(go)) prefabs.Add(go, new List<Object>());
          prefabs[go].Add(cp);
        }
        else   //�����Ϊ��
        {
          SerializedObject so = new SerializedObject(cp);
          SerializedProperty iterator = so.GetIterator();
          //��ȡ��������
          while (iterator.NextVisible(true))
          {
            if (iterator.propertyType == SerializedPropertyType.ObjectReference)
            {
              //���ö�����null ���� ����ID����0 ˵����ʧ������
              if (iterator.objectReferenceValue == null && iterator.objectReferenceInstanceIDValue != 0)
              {
                if (!refPaths.ContainsKey(cp)) refPaths.Add(cp, new List<string>());
                refPaths[cp].Add(iterator.propertyPath);

                if (!prefabs.ContainsKey(go)) prefabs.Add(go, new List<Object>());
                prefabs[go].Add(cp);
              }

            }
          }
        }
      }
    }
  }

  //����ֻ�ǽ����ҽ����ʾ
  private Vector3 scroll = Vector3.zero;
  private void OnGUI()
  {
    scroll = EditorGUILayout.BeginScrollView(scroll);
    EditorGUILayout.BeginVertical();
    foreach (var item in prefabs)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.ObjectField(item.Key, typeof(GameObject), true, GUILayout.Width(200));
      EditorGUILayout.BeginVertical();
      foreach (var cp in item.Value)
      {
        EditorGUILayout.BeginHorizontal();
        if (cp)
        {
          EditorGUILayout.ObjectField(cp, cp.GetType(), true, GUILayout.Width(200));
          if (refPaths.ContainsKey(cp))
          {
            string missingPath = null;
            foreach (var path in refPaths[cp])
            {
              missingPath += path + "|";
            }
            if (missingPath != null)
              missingPath = missingPath.Substring(0, missingPath.Length - 1);
            GUILayout.Label(missingPath);
          }
        }
        else
        {
          GUILayout.Label("��ʧ�ű���");
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUILayout.EndHorizontal();
    }
    EditorGUILayout.EndVertical();
    EditorGUILayout.EndScrollView();
  }
}