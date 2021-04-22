using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Modding
{
    class ModMaker
    {
        [@MenuItem("Ballance/模块开发/生成模块包模板", false, 100)]
        static void MakeModFile()
        {
            EditorWindow.GetWindowWithRect(typeof(PackageMakerWindow), new Rect(200, 150, 450, 400));
        }
        [@MenuItem("Ballance/模块开发/打包模块包", false, 100)]
        static void PackModFile()
        {
            EditorWindow.GetWindowWithRect(typeof(PackagePackerWindow), new Rect(200, 150, 450, 550));
        }
        [@MenuItem("Ballance/模块开发/打包Packages下所有模块包至Debug目录", false, 100)]
        static void PackModFileAll()
        {
            EditorWindow.GetWindowWithRect(typeof(PackageMakerAllWindow), new Rect(200, 150, 350, 150));
        }
    

    }
}
