using System.IO;
using UnityEditor;
using UnityEngine;
using ICSharpCode.SharpZipLib.Checksum;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;
using System.Collections.Generic;

namespace Ballance2.Editor.LevelMaker
{
    class LevelPacker
    {            
        private static List<string> allAssetsPath = new List<string>();

        private static string projModDirPath = "";
        private static string projModDefFile = "";
        private static string projPath = "";

        public static string DoPackPackage(BuildTarget packTarget, TextAsset packDefFile, string sourceName, string targetDir) {
            string targetPath = targetDir + "/" + sourceName + ".ballance";
            if (!string.IsNullOrEmpty(targetPath))
            {
                allAssetsPath.Clear();

                projPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/";
                projModDefFile = projPath + AssetDatabase.GetAssetPath(packDefFile);
                projModDirPath = projPath + Path.GetDirectoryName(AssetDatabase.GetAssetPath(packDefFile));

                string dirTargetPath = Path.GetDirectoryName(targetPath);
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
                    }

                    allAssetsPath.Add(projModDirPath + "/LevelLogo.png");

                    string name = Path.GetFileNameWithoutExtension(targetPath);

                    //打包
                    AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
                    assetBundleBuild.assetBundleName = sourceName;
                    assetBundleBuild.assetBundleVariant = "ballance";
                    assetBundleBuild.assetNames = allAssetsPath.ToArray();

                    //打包
                    BuildPipeline.BuildAssetBundles(dirTargetPath, new AssetBundleBuild[]{
                        assetBundleBuild
                    }, BuildAssetBundleOptions.None, packTarget);

                    EditorUtility.ClearProgressBar();
                }
            }
            return "";
        }
    }
}
