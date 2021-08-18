using Ballance2.Config.Settings;
using Ballance2.Sys.Res;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.LevelMaker
{
    public class LevelPackerWindow : EditorWindow
    {
        public LevelPackerWindow()
        {
            titleContent = new GUIContent("打包 Ballance 关卡");
        }

        private SerializedObject serializedObject;
        private SerializedProperty pPackDefFile = null;
        private SerializedProperty pPackTarget = null;

        private bool isError = false;
        private string errStr = "";
        private bool isResult = false;
        private bool isClosed = false;

        [SerializeField]
        private TextAsset levelJsonFile = null;
        [SerializeField]
        private BuildTarget packTarget = BuildTarget.NoTarget;

        private GUIStyle groupBox = null;
        private int selectedLevel = 0;
        private Vector2 scrollRect = new Vector2();

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            pPackDefFile = serializedObject.FindProperty("levelJsonFile");
            pPackTarget = serializedObject.FindProperty("packTarget");

            LoadDefConfig();
            LoadLevelsPath();
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

            EditorGUILayout.Space(20);
            EditorGUILayout.HelpBox(new GUIContent(@"使用这个工具来打包你的关卡。"), true);
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            selectedLevel = EditorGUILayout.Popup(new GUIContent("选择一个模块文件夹 "), selectedLevel, packsPathArr);
            if (GUILayout.Button("刷新", GUILayout.Width(80)))
                LoadLevelsPath();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(pPackTarget, new GUIContent("目标平台 "));

            GUILayout.Space(50);

            if (GUILayout.Button("打包至DebugFolder"))
                DoPack(false);
            if (GUILayout.Button("打包至自定义目录"))
                DoPack(true);

            GUILayout.Space(5);

            if(isError)
                EditorGUILayout.HelpBox(errStr, MessageType.Error);
            if(isResult)
            {
                EditorGUILayout.BeginScrollView(scrollRect, GUI.skin.GetStyle("window"));
                GUILayout.Space(3);
                GUILayout.Label("打包完成！");
                EditorGUILayout.EndScrollView();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("清空结果"))  isResult = false;
                if (GUILayout.Button("关闭窗口")) { isClosed = true; Close(); }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck() && !isClosed)
                serializedObject.ApplyModifiedProperties();
        }

        private List<string> packsPath = new List<string>();
        private string[] packsPathArr = null;

        private void LoadDefConfig()
        {
            selectedLevel = EditorPrefs.GetInt("LevelMakerDefLevel", 0);
            packTarget = (BuildTarget)EditorPrefs.GetInt("LevelMakerDefTarget", -2);
        }
        private void LoadLevelsPath()
        {
            packsPath.Clear();
            packsPath.Add("请选择");

            DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_LEVEL_FOLDER);
            DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
                packsPath.Add(dirs[i].Name);

            packsPathArr = packsPath.ToArray();
        }
        private void SaveDefConfig()
        {
            EditorPrefs.SetInt("LevelMakerDefLevel", selectedLevel);
            EditorPrefs.SetInt("LevelMakerDefTarget", (int)packTarget);
        }

        private void DoPack(bool chooseFolder)
        {
            isError = false;

            if (selectedLevel == 0)
            {
                isError = true;
                errStr = "请选择你的模块";
                return;
            } 
            else if (selectedLevel < packsPathArr.Length)
            {
                string p = GamePathManager.DEBUG_LEVEL_FOLDER + "/" + packsPathArr[selectedLevel] + "/Level.json";
                levelJsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(p);
                if(levelJsonFile  == null)
                {
                    isError = true;
                    errStr = "没有在此目录下找到 Level.json ，请先使用生成工具生成";
                    return;
                }
            }
            
            //check
            if (levelJsonFile == null)
            {
                isError = true;
                errStr = "请选择你的 Level.json ";
                return;
            }
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
            if (selectedLevel != 1 && chooseFolder)
                dir = EditorUtility.OpenFolderPanel("保存关卡", EditorPrefs.GetString("LevelMakerDefSaveDir", GamePathManager.DEBUG_PATH), "");
            else
            {
                dir = DebugSettings.Instance.DebugFolder + "/Levels/";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            if (!string.IsNullOrEmpty(dir))
            {
                if (dir != GamePathManager.DEBUG_PATH)
                    EditorPrefs.SetString("LevelMakerDefSaveDir", GamePathManager.DEBUG_PATH);

                string err = LevelPacker.DoPackPackage(packTarget, levelJsonFile, packsPathArr[selectedLevel], dir);
                if(!string.IsNullOrEmpty(err))
                {
                    isError = true;
                    errStr = err;
                    return;
                } else {
                    isError = false;
                    EditorUtility.DisplayDialog("提示", "打包成功！", "好的");
                }
            }
            else
            {
                isError = true;
                errStr = "您取消了保存 ";
            }
        }
    }
}
