using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Modding.LevelMaker
{
    class LevelMaker
    {
        [@MenuItem("Ballance/关卡制作/创建关卡模板", false, 101)]
        static void MakeLevelFile()
        {
            EditorWindow.GetWindowWithRect(typeof(LevelMakerWindow), new Rect(200, 150, 350, 200));
        }

        [@MenuItem("Ballance/关卡制作/打包关卡", false, 101)]
        static void PackLevelFile()
        {

        }
    }
}
