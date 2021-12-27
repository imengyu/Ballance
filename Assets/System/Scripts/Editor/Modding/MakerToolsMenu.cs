using System.IO;
using UnityEditor;
using Ballance2.Config.Settings;
using Ballance2.Res;

namespace Ballance2.Editor.Modding
{
    class MakerTools
    {
        [@MenuItem("Ballance/工具/复制系统初始化文件到Debug目录", false, 102)]
        static void CopySystemInitFileToDebugFolder()
        {
            string debugFolder = DebugSettings.Instance.DebugFolder;
            if (string.IsNullOrEmpty(debugFolder))
            {
                EditorUtility.DisplayDialog("提示", "请先设置 DebugFolder ", "确定");
                return;
            }

            if (Directory.Exists(debugFolder))
            {
                if (!Directory.Exists(debugFolder + "/core"))
                    Directory.CreateDirectory(debugFolder + "/core");

                File.Copy("Assets/Packages/SystemInit.xml", debugFolder + "/Core/system.init.xml", true);
                File.Copy("Assets/Packages/GameInit.xml", debugFolder + "/Core/game.init.xml", true);

                EditorUtility.DisplayDialog("提示", "复制成功", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "DebugFolder 不存在", "确定");
            }
        }
        [@MenuItem("Ballance/工具/复制Debug文件夹到Output目录", false, 103)]
        static void CopyDebugFolderToOutput()
        {
            string debugFolder = DebugSettings.Instance.DebugFolder;
            string folder = GamePathManager.DEBUG_OUTPUT_PATH;
            if (Directory.Exists(folder) && Directory.Exists(folder))
            {
                if (folder != GamePathManager.DEBUG_PATH)
                    EditorPrefs.SetString("CopyDebugFolderDefSaveDir", GamePathManager.DEBUG_PATH);

                CopyDebugFolder("Core", debugFolder, folder);
                CopyDebugFolder("Packages", debugFolder, folder);
                CopyDebugFolder("Levels", debugFolder, folder);

                EditorUtility.DisplayDialog("提示", "复制成功", "确定");
            }
            else EditorUtility.DisplayDialog("错误", "Output文件夹不存在：\n" + debugFolder + "\n▼\n" + folder, "确定"); 
        }
        [@MenuItem("Ballance/工具/复制Debug文件夹到自定义目录", false, 103)]
        static void CopyDebugFolder()
        {
            string debugFolder = DebugSettings.Instance.DebugFolder;
            string folder = EditorUtility.OpenFolderPanel("选择输出目录", EditorPrefs.GetString("CopyDebugFolderDefSaveDir", GamePathManager.DEBUG_PATH), "");
            if (string.IsNullOrEmpty(folder))
                return;
            if (Directory.Exists(folder) && Directory.Exists(folder))
            {
                if (folder != GamePathManager.DEBUG_PATH)
                    EditorPrefs.SetString("CopyDebugFolderDefSaveDir", GamePathManager.DEBUG_PATH);

                CopyDebugFolder("Core", debugFolder, folder);
                CopyDebugFolder("Packages", debugFolder, folder);
                CopyDebugFolder("Levels", debugFolder, folder);

                EditorUtility.DisplayDialog("提示", "复制成功", "确定");
            }
            else EditorUtility.DisplayDialog("错误", "文件夹不存在：\n" + debugFolder + "\n▼\n" + folder, "确定"); 
        }

        private static void CopyDebugFolder(string name, string debugFolder, string folder)
        {
            string folderCoreSrc = debugFolder + "/" + name;
            string folderCoreTarget = folder + "/" + name;
            if (Directory.Exists(folderCoreSrc))
            {
                if (!Directory.Exists(folderCoreTarget))
                    Directory.CreateDirectory(folderCoreTarget);

                DirectoryInfo direction = new DirectoryInfo(folderCoreSrc);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".ballance") 
                        || files[i].Name.EndsWith(".txt")
                        || files[i].Name.EndsWith(".xml"))
                    {
                        File.Copy(folderCoreSrc + "/" + files[i].Name, folderCoreTarget + "/" + files[i].Name, true);
                    }
                }
            }
        }
    }
}
