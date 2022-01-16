using Ballance2.Config.Settings;
using Ballance2.Res;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* PackagePackerWindow.cs
* 
* 用途：
* 打包模块代码
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Editor.Modding
{
  public class PackageMakerAllWindow : EditorWindow
    {
        public PackageMakerAllWindow()
        {
            titleContent = new GUIContent("打包Packages下所有模块包至Debug目录");
        }

        private SerializedObject serializedObject;
        private SerializedProperty pPackTarget = null;

        private bool isError = false;
        private string errStr = "";
        private bool isClosed = false;

        [SerializeField]
        private BuildTarget packTarget = BuildTarget.NoTarget;

        private GUIStyle groupBox = null;

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            pPackTarget = serializedObject.FindProperty("packTarget");

            LoadDefConfig();
            LoadModsPath();
        }
        private void OnDisable()
        {
            EditorUtility.ClearProgressBar();
            SaveDefConfig();
        }
        private void OnGUI()
        {
            serializedObject.Update();

            if (groupBox == null)
                groupBox = GUI.skin.FindStyle("GroupBox");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical(groupBox);
            EditorGUILayout.PropertyField(pPackTarget, new GUIContent("目标平台 "));

            GUILayout.Space(50);

            if (GUILayout.Button("开始打包"))
                DoPack();

            GUILayout.Space(5);
            
            if(isError)
                EditorGUILayout.HelpBox(errStr, MessageType.Error);

            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck() && !isClosed)
                serializedObject.ApplyModifiedProperties();
        }

        private List<string> packsPath = new List<string>();
        private string[] packsPathArr = null;

        private void LoadDefConfig()
        {
            packTarget = (BuildTarget)EditorPrefs.GetInt("ModMakerDefTarget", -2);
        }
        private void LoadModsPath()
        {
            packsPath.Clear();
            packsPath.Add("core");

            DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_PACKAGE_FOLDER);
            DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
                packsPath.Add(dirs[i].Name);

            packsPathArr = packsPath.ToArray();
        }
        private void SaveDefConfig()
        {
            EditorPrefs.SetInt("ModMakerDefTarget", (int)packTarget);
        }
        private void DoPack()
        {
            if (packTarget == BuildTarget.NoTarget)
            {
                isError = true;
                errStr = "请选择目标平台";
                return;
            }
            if(!BuildPipeline.IsBuildTargetSupported(BuildPipeline.GetBuildTargetGroup(packTarget), packTarget))
            {
                isError = true;
                errStr = "你的 Unity 似乎不支持目标平台 "  + packTarget + " 的编译，可能你没有安装对应模块";
                return;
            }

            string dir = "";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            EditorUtility.DisplayProgressBar("正在打包", "正在打包，请稍后...", 0);

            foreach(string path in packsPath) {
                TextAsset packageDef = null;
                if(path == "core") {
                    packageDef = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Game/PackageDef.xml");
                    dir = DebugSettings.Instance.DebugFolder + "/Core/";
                }
                else {
                    packageDef = AssetDatabase.LoadAssetAtPath<TextAsset>(GamePathManager.DEBUG_PACKAGE_FOLDER + "/" + path + "/PackageDef.xml");
                    dir = DebugSettings.Instance.DebugFolder + "/Packages/";
                }
                if(packageDef  == null)
                {
                    isError = true;
                    Debug.LogWarning("没有在 " + path + " 模块下找到 PackageDef.xml，跳过打包此模块");
                    continue;
                } 

                string err = PackagePacker.DoPackPackage(packTarget, packageDef, path, dir);
                if(!string.IsNullOrEmpty(err))
                    Debug.LogError("打包模块 " + path + " 发生错误：" + err);
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提示", "打包成功！", "好的");

            isError = false;
        }
    }
}
