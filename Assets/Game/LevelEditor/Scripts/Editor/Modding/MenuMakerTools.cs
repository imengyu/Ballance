using System.IO;
using UnityEditor;
using Ballance2.Config.Settings;
using Ballance2.Res;
using UnityEngine;

namespace Ballance2.Editor.Modding
{
  class MenuMakerTools
  {
    [@MenuItem("Ballance/工具/复制Debug文件夹到预设输出目录", false, 103)]
    static void CopyDebugFolderToOutput()
    {
      WindowChoosePlatform choosePlatformWindow = EditorWindow.GetWindowWithRect<WindowChoosePlatform>(new Rect(200, 150, 450, 250));
      choosePlatformWindow.OnChoose = (target) =>
      {
        Debug.Log("OnChoose: " + target);

        string debugFolder = DebugSettings.Instance.DebugFolder + "/" + target;

        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        {
          //Windows 直接复制到输出目录
          string folder = DebugSettings.Instance.OutputFolder;
          if (Directory.Exists(debugFolder) && Directory.Exists(folder))
          {
            CopyDebugFolder("Core", debugFolder, folder);
            CopyDebugFolder("Packages", debugFolder, folder);
            CopyDebugFolder("Levels", debugFolder, folder);

            EditorUtility.DisplayDialog("提示", "复制成功", "确定");
          }
          else EditorUtility.DisplayDialog("错误", "Output文件夹不存在：\n" + debugFolder + "\n▼\n" + folder, "确定");
        }
        else if (target == BuildTarget.StandaloneOSX)
        {
          //Mac 直接复制到输出目录
          string folder = DebugSettings.Instance.OutputAppMac + "/Contents/";
          if (Directory.Exists(debugFolder) && Directory.Exists(folder))
          {
            CopyDebugFolder("Core", debugFolder, folder);
            CopyDebugFolder("Packages", debugFolder, folder);
            CopyDebugFolder("Levels", debugFolder, folder);

            EditorUtility.DisplayDialog("提示", "复制成功", "确定");
          }
          else EditorUtility.DisplayDialog("错误", "Output文件夹不存在：\n" + debugFolder + "\n▼\n" + folder, "确定");
        }
        else if (target == BuildTarget.Android || target == BuildTarget.iOS || target == BuildTarget.WSAPlayer || target == BuildTarget.Switch)
        {
          //IOS/Android 则复制到 StreamAssets
          string folder = "Assets/StreamingAssets/BuiltInPackages";
          if (Directory.Exists(debugFolder))
          {
            if(!Directory.Exists(folder))
              Directory.CreateDirectory(folder);

            CopyDebugFolder("Core", debugFolder, folder);
            CopyDebugFolder("Packages", debugFolder, folder);
            CopyDebugFolder("Levels", debugFolder, folder);

            EditorUtility.DisplayDialog("提示", "复制成功", "确定");
          }
          else EditorUtility.DisplayDialog("错误", "debugFolder 文件夹不存在：\n" + debugFolder + "\n▼\n" + folder, "确定");
        }
        else
        {
          EditorUtility.DisplayDialog("错误", "暂不支持此平台", "确定");
        }
      };
      choosePlatformWindow.Show();
    }
    [@MenuItem("Ballance/工具/复制Debug文件夹到自定义目录", false, 103)]
    static void CopyDebugFolder()
    {
      WindowChoosePlatform choosePlatformWindow = EditorWindow.GetWindowWithRect<WindowChoosePlatform>(new Rect(200, 150, 450, 250));
      choosePlatformWindow.OnChoose = (target) =>
      {
        string debugFolder = DebugSettings.Instance.DebugFolder + "/";
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
      };
      choosePlatformWindow.Show();
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
