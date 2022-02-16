using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Ballance2.Editor.Modding.LevelMaker
{
  class LevelPacker
  {
    private static string projLevelDirPath = "";

    public static string DoPackPackage(BuildTarget packTarget, TextAsset packDefFile, string sourceName, string targetDir)
    {
      string targetPath = targetDir + "/" + sourceName + ".ballance";
      if (!string.IsNullOrEmpty(targetPath))
      {
        projLevelDirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(packDefFile)).Replace("\\", "/");

        string dirTargetPath = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(projLevelDirPath))
        {
          List<string> allAssetsPath = new List<string>();
          allAssetsPath.Add(AssetDatabase.GetAssetPath(packDefFile));
          allAssetsPath.Add(projLevelDirPath + "/LevelLogo.png");
          allAssetsPath.Add(projLevelDirPath + "/Level.prefab");

          //打包
          AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
          assetBundleBuild.assetBundleName = sourceName;
          assetBundleBuild.assetBundleVariant = "ballance";
          assetBundleBuild.assetNames = allAssetsPath.ToArray();

          if(!Directory.Exists(dirTargetPath))
            Directory.CreateDirectory(dirTargetPath);

          //打包
          var ass = BuildPipeline.BuildAssetBundles(dirTargetPath, new AssetBundleBuild[]{
            assetBundleBuild
          }, BuildAssetBundleOptions.None, packTarget);

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
