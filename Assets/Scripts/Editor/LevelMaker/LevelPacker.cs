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
        private static string projLogoFile = "";

        public static string DoPackPackage(BuildTarget packTarget, TextAsset packDefFile, string sourceName, string targetDir) {
            string targetPath = targetDir + "/" + sourceName + ".ballance";
            if (!string.IsNullOrEmpty(targetPath))
            {
                allAssetsPath.Clear();

                projPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/";
                projModDefFile = projPath + AssetDatabase.GetAssetPath(packDefFile);
                projModDirPath = projPath + Path.GetDirectoryName(AssetDatabase.GetAssetPath(packDefFile));
                projLogoFile = projModDirPath + "/LevelLogo.png";

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

                    string name = Path.GetFileNameWithoutExtension(targetPath);

                    //打包
                    AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
                    assetBundleBuild.assetBundleName = sourceName;
                    assetBundleBuild.assetBundleVariant = "assetbundle";
                    assetBundleBuild.assetNames = allAssetsPath.ToArray();

                    //打包
                    BuildPipeline.BuildAssetBundles(dirTargetPath, new AssetBundleBuild[]{
                        assetBundleBuild
                    }, BuildAssetBundleOptions.None, packTarget);

                    EditorUtility.DisplayProgressBar("正在打包", "正在打包，请稍后...", 0.6f);

                    //ballance 包处理
                    DoSolveBallancePack(dirTargetPath, dirTargetPath + "/" + name, targetPath);

                    EditorUtility.ClearProgressBar();
                }
            }
            return "";
        }
        private static void DoSolveBallancePack(string dirTargetPath, string bundlePath, string targetPath)
        {
            Crc32 crc = new Crc32();
            ZipOutputStream zipStream = ZipUtils.CreateZipFile(targetPath);
            string basePath = projModDirPath.Replace(projPath, "").Replace("\\", "/");

            //添加到包里
            ZipUtils.AddFileToZip(zipStream, bundlePath + ".assetbundle", "/assets/" + Path.GetFileName(bundlePath) + ".assetbundle", ref crc);
            ZipUtils.AddFileToZip(zipStream, bundlePath + ".assetbundle.manifest", "/assets/" + Path.GetFileName(bundlePath) + ".assetbundle.manifest", ref crc);
            ZipUtils.AddFileToZip(zipStream, projModDefFile, projModDirPath.Length, ref crc);

            //添加logo图片
            if (File.Exists(projLogoFile)) ZipUtils.AddFileToZip(zipStream, projLogoFile, projModDirPath.Length, ref crc);

            zipStream.Finish();
            zipStream.Close();
        }
    }
}
