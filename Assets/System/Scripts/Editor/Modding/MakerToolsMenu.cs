using System.IO;
using UnityEditor;
using Ballance2.Config.Settings;
using Ballance2.Res;
using Ballance2.Utils;
using UnityEngine;
using Ballance2.Editor.Lua;

namespace Ballance2.Editor.Modding
{
    class MakerTools
    {
        [@MenuItem("Ballance/工具/复制Debug文件夹到Output目录", false, 103)]
        static void CopyDebugFolderToOutput()
        {
            string debugFolder = DebugSettings.Instance.DebugFolder;
            string folder = DebugSettings.Instance.OutputFolder;
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
        [@MenuItem("Ballance/工具/编译系统脚本到Reources目录", false, 103)]
        public static void CopyScriptToReourcesFolder()
        {
            const string folderSrc = "Assets/System/Scripts/SystemScrips";
            const string folderTarget = "Assets/System/Resources/SystemScrips";
            int count = 0;

            DirectoryInfo direction = new DirectoryInfo(folderSrc);
            FileInfo[] files = direction.GetFiles("*.lua", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var rel = StringUtils.RemoveStringByStringStart(files[i].FullName.Replace('\\', '/'), folderSrc);
                var src = folderSrc + "/" + rel;
                var dest = folderTarget + "/" + rel + ".bytes";
                var dir = Path.GetDirectoryName(dest);
                if(!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var outPath = "";
                if(LuaCompiler.CompileLuaFile(src, false, out outPath)) {
                    File.Copy(outPath, dest, true);
                    File.Delete(outPath);
                } else {
                    Debug.LogError("编译 " + src + " 失败, 将lua文件原样打包。");
                    File.Copy(src, dest, true);
                }
                count++;
            }

            EditorUtility.DisplayDialog("提示", "成功。编译 " + count + " 个文件", "确定");
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
                        File.Copy(
                            folderCoreSrc + "/" + files[i].Name, 
                            folderCoreTarget + "/" + files[i].Name,
                            true);
                    }
                }
            }
        }
    }
}
