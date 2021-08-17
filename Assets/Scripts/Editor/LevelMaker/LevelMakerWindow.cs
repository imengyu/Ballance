using Ballance2.Sys.Res;
using Ballance2.Utils;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.LevelMaker
{
    public class LevelMakerWindow : EditorWindow
    {
        public LevelMakerWindow()
        {
            titleContent = new GUIContent("创建 Ballance 关卡模板");
        }

        private string levelName = "";

        private SerializedObject serializedObject;

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
        }

        private void OnDisable() {
            serializedObject = null;
        }

        private void OnGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(50);

            levelName = EditorGUILayout.TextField("文件夹名称", levelName);

            var empty = StringUtils.isNullOrEmpty(levelName);
               

            EditorGUI.BeginDisabledGroup(empty);

            if(GUILayout.Button("生成"))
                Make();

            EditorGUI.EndDisabledGroup();

            if(empty)
                EditorGUILayout.HelpBox("文件夹名称为空", MessageType.Error);

            EditorGUILayout.Space(50);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck() && serializedObject != null)
                serializedObject.ApplyModifiedProperties();
        }

        private void Make()
        {
            string folderPath = GamePathManager.DEBUG_LEVEL_FOLDER + "/" + levelName;
            if (Directory.Exists(folderPath))
            {
                if (!EditorUtility.DisplayDialog("提示", "指定关卡文件夹 " + levelName + " 已经在： \n" +
                    folderPath + "\n存在了，是否要替换？", "替换", "取消"))
                    return;
            }

            DirectoryInfo directoryInfo = Directory.CreateDirectory(folderPath);
            if(directoryInfo == null)
            {
                EditorUtility.DisplayDialog("错误", "创建文件夹失败：\n" + folderPath, "确定");
                return;
            }

            File.Copy(GamePathManager.DEBUG_LEVEL_FOLDER + "/template_Level.json", folderPath + "/Level.json");
            File.Copy(GamePathManager.DEBUG_LEVEL_FOLDER + "/template_LevelLogo.png", folderPath + "/LevelLogo.png");

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "生成模板成功！", "好的");

            Close();
        }
    }
}
