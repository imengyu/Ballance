using Ballance2.System.Res;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Modding
{
    public class PackagePackerWindow : EditorWindow
    {
        public PackagePackerWindow()
        {
            titleContent = new GUIContent("打包 Ballance 模组包");
        }

        private SerializedObject serializedObject;
        private SerializedProperty pmodDefFile = null;
        private SerializedProperty pmodTarget = null;

        private bool isError = false;
        private string errStr = "";
        private bool isResult = false;
        private bool isClosed = false;

        [SerializeField]
        private TextAsset modDefFile = null;
        [SerializeField]
        private BuildTarget modTarget = BuildTarget.NoTarget;

        private GUIStyle groupBox = null;
        private int tab = 0;
        private string[] tabText = new string[] {  "选择 Packages 下的模组", "选择 PackageDef.xml" };
        private int selectedMod = 0;
        private Vector2 scrollRect = new Vector2();

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            pmodDefFile = serializedObject.FindProperty("modDefFile");
            pmodTarget = serializedObject.FindProperty("modTarget");

            LoadDefConfig();
            LoadModsPath();
        }
        private void OnDisable()
        {
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
            EditorGUILayout.HelpBox(new GUIContent(@"使用这个工具来打包你的模组包，
你需要先使用“生成”工具来在 Assets/Packages 下生成你的模组包。"), true);
            EditorGUILayout.Space(15);

            tab = GUILayout.Toolbar(tab, tabText);
            EditorGUILayout.Space(10);

            if (tab == 1)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(pmodDefFile, new GUIContent("请选择 PackageDef.xml "));
                if (GUILayout.Button("编辑器中选中的条目", GUILayout.Width(130)))
                    SelectInEditor();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                selectedMod = EditorGUILayout.Popup(new GUIContent("选择一个模组文件夹 "), selectedMod, modsPathArr);
                if (GUILayout.Button("刷新", GUILayout.Width(80)))
                    LoadModsPath();
                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.PropertyField(pmodTarget, new GUIContent("目标平台 "));

            GUILayout.Space(50);

            if (GUILayout.Button("打包"))
                DoPack();

            GUILayout.Space(5);

            if(isError)
                EditorGUILayout.HelpBox(errStr, MessageType.Error);
            if(isResult)
            {
                EditorGUILayout.BeginScrollView(scrollRect, GUI.skin.GetStyle("window"));
                GUILayout.Space(3);
                GUILayout.Label("打包完成！");
                GUILayout.Label(modPackageName + " 包内所有资源:");
                foreach(string s in allAssetsPath)
                    GUILayout.Label(s);
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

        private List<string> allAssetsPath = new List<string>();
        private List<string> modsPath = new List<string>();
        private string[] modsPathArr = null;
        private string modPackageName = "";
        private string modLogoName = "";

        private void LoadDefConfig()
        {
            tab = EditorPrefs.GetInt("ModMakerDefTab", 0);
            selectedMod = EditorPrefs.GetInt("ModMakerDefMod", 0);
            modTarget = (BuildTarget)EditorPrefs.GetInt("ModMakerDefTarget", -2);
        }
        private void LoadModsPath()
        {
            modsPath.Clear();
            modsPath.Add("请选择");

            DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_PACKAGE_FOLDER);
            DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
                modsPath.Add(dirs[i].Name);

            modsPathArr = modsPath.ToArray();
        }
        private void SaveDefConfig()
        {
            EditorPrefs.SetInt("ModMakerDefTab", tab);
            EditorPrefs.SetInt("ModMakerDefMod", selectedMod);
            EditorPrefs.SetInt("ModMakerDefTarget", (int)modTarget);
        }
        private void SelectInEditor()
        {
            if(Selection.activeObject == null)
            {
                EditorUtility.DisplayDialog("提示", "请先在编辑器中选择你的 PackageDef.xml 哦", "好的");
                return;
            }
            if(Selection.activeObject.GetType() != typeof(TextAsset))
            {
                EditorUtility.DisplayDialog("提示", "你选择的 PackageDef.xml 哦文件格式不对，必须是 TextAsset ", "好的");
                return;
            }
            modDefFile = Selection.activeObject as TextAsset;
        }

        private string projPath = "";
        private string projModDefFile = "";
        private string projModDirPath = "";
        private string projLogoFile = "";

        private void DoPack()
        {
            isError = false;

            //selectedMod to modDefFile
            if (tab == 0)
            {
                if (selectedMod == 0)
                {
                    isError = true;
                    errStr = "请选择你的模组";
                    return;
                }
                if (selectedMod > 0 && selectedMod < modsPathArr.Length)
                {
                    string p = GamePathManager.DEBUG_PACKAGE_FOLDER + "/" + 
                        modsPathArr[selectedMod] + "/PackageDef.xml";
                    modDefFile = AssetDatabase.LoadAssetAtPath<TextAsset>(p);
                    if(modDefFile  == null)
                    {
                        isError = true;
                        errStr = "没有在此目录下找到 PackageDef.xml ，请先使用生成工具生成";
                        return;
                    }
                }
            }
            //check
            if (modDefFile == null)
            {
                isError = true;
                errStr = "请选择你的 PackageDef.xml ";
                return;
            }
            if (modTarget == BuildTarget.NoTarget)
            {
                isError = true;
                errStr = "请选择目标平台";
                return;
            }
            if(!BuildPipeline.IsBuildTargetSupported(BuildPipeline.GetBuildTargetGroup(modTarget), modTarget))
            {
                isError = true;
                errStr = "你的 Unity 似乎不支持目标平台 "  + modTarget + " 的编译，可能你没有安装对应模块";
                return;
            }

            string path = EditorUtility.SaveFilePanel("保存模组包",
                   EditorPrefs.GetString("ModMakerDefSaveDir", GamePathManager.DEBUG_PATH),
                   EditorPrefs.GetString("ModMakerDefFileName", "New Mod"), "ballance");

            if (!string.IsNullOrEmpty(path))
            {
                if (path != GamePathManager.DEBUG_PATH)
                    EditorPrefs.SetString("ModMakerDefSaveDir", GamePathManager.DEBUG_PATH);
                EditorPrefs.SetString("ModMakerDefFileName", Path.GetFileNameWithoutExtension(path));

                DoSolveModDef();

                if(string.IsNullOrEmpty(modPackageName))
                {
                    isError = true;
                    errStr = "PackageDef.xml 必须填写包名 packageName";
                    return;
                }

                allAssetsPath.Clear();
                string dirTargetPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(projModDirPath))
                {
                    //遍历文件夹的内容
                    if (Directory.Exists(projModDirPath))
                    {
                        DirectoryInfo direction = new DirectoryInfo(projModDirPath);
                        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                        for (int i = 0; i < files.Length; i++)
                        {
                            if (files[i].Name.EndsWith(".meta")) continue;
                            allAssetsPath.Add(files[i].FullName.Replace("\\", "/").Replace(projPath, ""));
                        }
                        isResult = true;
                    }

                    string name = Path.GetFileNameWithoutExtension(path);

                    EditorUtility.DisplayProgressBar("正在打包", "正在打包，请稍后...", 0);

                    //打包
                    AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
                    assetBundleBuild.assetBundleName = modPackageName;
                    assetBundleBuild.assetBundleVariant = "assetbundle";
                    assetBundleBuild.assetNames = allAssetsPath.ToArray();

                    //打包
                    BuildPipeline.BuildAssetBundles(dirTargetPath, new AssetBundleBuild[]{
                        assetBundleBuild
                    }, BuildAssetBundleOptions.None, modTarget);

                    EditorUtility.DisplayProgressBar("正在打包", "正在打包，请稍后...", 0.6f);

                    //ballance 包处理
                    DoSolveBallancePack(dirTargetPath, dirTargetPath + "/" + name, path);

                    EditorUtility.ClearProgressBar();
                    EditorUtility.DisplayDialog("提示", "打包成功！\n" + path, "好的");
                }
                else
                {
                    isError = true;
                    errStr = "选择的 PackageDef.xml 不在本项目中 ";
                }
            }
            else
            {
                isError = true;
                errStr = "您取消了保存 ";
            }
        }
        private void DoSolveModDef()
        {
            projPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/";
            projModDefFile = projPath + AssetDatabase.GetAssetPath(modDefFile);
            projModDirPath = projPath + Path.GetDirectoryName(AssetDatabase.GetAssetPath(modDefFile));
            modPackageName = "";

            //模组信息处理
            XmlDocument modDefXmlDoc = new XmlDocument();
            modDefXmlDoc.LoadXml(modDefFile.text);
            XmlNode nodeMod = modDefXmlDoc.SelectSingleNode("Mod");
            
            foreach (XmlNode node in nodeMod.ChildNodes)
            {
                if (node.Name == "BaseInfo")
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        if (attribute.Name == "packageName")
                            modPackageName = attribute.Value;
                    }
                    foreach (XmlNode nodec in node.ChildNodes)
                    {
                        if (nodec.Name == "Logo")
                        {
                            modLogoName = nodec.InnerText;
                            projLogoFile = projModDirPath + Path.DirectorySeparatorChar + nodec.InnerText;
                            break;
                        }
                    }
                    break;
                }
            }
        }
        private void DoSolveBallancePack(string dirTargetPath, string bundlePath, string targetPath)
        {
            Crc32 crc = new Crc32();
            ZipOutputStream zipStream = ZipUtils.CreateZipFile(targetPath);

            //添加到包里
            ZipUtils.AddFileToZip(zipStream, bundlePath + ".assetbundle", dirTargetPath.Length, crc);
            ZipUtils.AddFileToZip(zipStream, bundlePath + ".assetbundle.manifest", dirTargetPath.Length, crc);
            ZipUtils.AddFileToZip(zipStream, projModDefFile, projModDirPath.Length, crc);

            if (File.Exists(projLogoFile)) ZipUtils.AddFileToZip(zipStream, projLogoFile, projModDirPath.Length, crc);
            else Debug.LogWarning("模组的 Logo 没有找到：" + projLogoFile);

            zipStream.Finish();
            zipStream.Close();
        }
    }
}
