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

    [@MenuItem("Ballance/关卡制作/打包Levels下所有关卡至输出目录", false, 102)]
    static void PackBuiltInLevelFile()
    {
      WindowChoosePlatform choosePlatformWindow = EditorWindow.GetWindowWithRect<WindowChoosePlatform>(new Rect(200, 150, 450, 250));
      choosePlatformWindow.OnChoose = (target) =>
      {
        
      };
      choosePlatformWindow.Show();
    }
  }
}
