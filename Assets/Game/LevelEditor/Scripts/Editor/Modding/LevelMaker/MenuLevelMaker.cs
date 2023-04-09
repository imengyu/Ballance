using System.IO;
using System.Text.RegularExpressions;
using Ballance2.Config.Settings;
using Ballance2.Res;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Modding.LevelMaker
{
  class MenuLevelMaker
  {
    [@MenuItem("Ballance/关卡制作/创建关卡模板", false, 101)]
    static void MakeLevelFile()
    {
      EditorWindow.GetWindowWithRect(typeof(WindowLevelMaker), new Rect(200, 150, 350, 200));
    }

    [@MenuItem("Ballance/关卡制作/打包关卡", false, 101)]
    static void PackLevelFile()
    {
      EditorWindow.GetWindowWithRect(typeof(WindowLevelPacker), new Rect(200, 150, 350, 400));
    }

    [@MenuItem("Ballance/关卡制作/打包所有内置关卡至输出目录", false, 102)]
    static void PackBuiltInLevelFile()
    {
      WindowChoosePlatform choosePlatformWindow = EditorWindow.GetWindowWithRect<WindowChoosePlatform>(new Rect(200, 150, 450, 250));
      choosePlatformWindow.OnChoose = (target) =>
      {
        if(!BuildPipeline.IsBuildTargetSupported(BuildPipeline.GetBuildTargetGroup(target), target))
        {
          EditorUtility.DisplayDialog("提示", "你的 Unity 似乎不支持目标平台 "  + target + " 的编译，可能你没有安装对应模块", "好的");
          return;
        }

        DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_LEVEL_FOLDER);
        DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < dirs.Length; i++){
          var name = dirs[i].Name;

          EditorUtility.DisplayProgressBar("正在打包", "正在打包 " + name, i / (float)dirs.Length);
          
          if(name != "MakerAssets" && Regex.IsMatch(name.ToLower(), "^level([0]{1}[0-9]{1})|([1]{1}[0-3]{1})$", RegexOptions.IgnoreCase)) {
            string p = GamePathManager.DEBUG_LEVEL_FOLDER + "/" + name + "/Level.json";
            var levelJsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(p);
            if(levelJsonFile != null)
            {
              string err = LevelPacker.DoPackPackage(
                target, 
                levelJsonFile, 
                name, 
                DebugSettings.Instance.DebugFolder + "/" + target + "/Levels/"
              );
              if(!string.IsNullOrEmpty(err))
              {
                Debug.LogError("打包关卡 " + name + " 失败，错误信息：" + err);
              }
            }
          }
        }
        
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("提示", "打包成功！", "好的");
      };
      choosePlatformWindow.Show();
    }
  }
}
