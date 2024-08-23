using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

namespace Ballance2.Editor.Modding.LevelMaker
{
  class LevelPacker
  {
    private static string projLevelDirPath = "";

    public static string DoPackPackage(BuildTarget packTarget, TextAsset packDefFile, string sourceName, string targetDir)
    {
      string targetTempPath = targetDir + "/" + sourceName + ".assetbundle";
      string targetPath = targetDir + "/" + sourceName + ".blevel";
      if (!string.IsNullOrEmpty(targetDir))
      {
        projLevelDirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(packDefFile)).Replace("\\", "/");

        string dirTargetPath = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(projLevelDirPath))
        {
          List<string> allAssetsPath = new List<string>() { projLevelDirPath + "/Level.prefab" };

          //打包
          AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
          assetBundleBuild.assetBundleName = sourceName;
          assetBundleBuild.assetBundleVariant = "assetbundle";
          assetBundleBuild.assetNames = allAssetsPath.ToArray();

          if(!Directory.Exists(dirTargetPath))
            Directory.CreateDirectory(dirTargetPath);

          //打包
          var ass = BuildPipeline.BuildAssetBundles(dirTargetPath, new AssetBundleBuild[]{
            assetBundleBuild
          }, BuildAssetBundleOptions.None, packTarget);

          Crc32 crc = new Crc32();
          ZipOutputStream zipStream = ZipUtils.CreateZipFile(targetPath);

          //添加到包里
          ZipUtils.AddFileToZip(zipStream, targetTempPath, "/assets/packed.assetbundle", ref crc);
          ZipUtils.AddFileToZip(zipStream, AssetDatabase.GetAssetPath(packDefFile), "/Level.json", ref crc);
          ZipUtils.AddFileToZip(zipStream, projLevelDirPath + "/LevelLogo.png", "/LevelLogo.png", ref crc);
          ZipUtils.AddFileToZip(zipStream, projLevelDirPath + "/LevelPreview.png", "/LevelPreview.png", ref crc);

          zipStream.Finish();
          zipStream.Close();

          if (File.Exists(targetTempPath))
            File.Delete(targetTempPath);
          if (File.Exists(targetTempPath + ".manifest"))
            File.Delete(targetTempPath + ".manifest");

          EditorUtility.ClearProgressBar();
          
          if(ass == null)
            return " BuildAssetBundles failed";
          return "";
        }
        return "projLevelDirPath not set";
      }
      return "targetPath not set";
    }
  }
}
