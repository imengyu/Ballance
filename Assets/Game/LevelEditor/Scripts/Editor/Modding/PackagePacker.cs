using System.IO;
using UnityEditor;
using UnityEngine;
using ICSharpCode.SharpZipLib.Checksum;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;
using System.Collections.Generic;

namespace Ballance2.Editor.Modding
{
  class PackagePacker
  {
    private static string projPath = "";
    private static string projModDefFile = "";
    private static string projModDirPath = "";
    private static string projLogoFile = "";
    private static string packPackageName = "";

    private static List<string> allAssetsPath = new List<string>();

    public static string DoPackPackage(BuildTarget packTarget, TextAsset packDefFile, string sourceName, string targetDir)
    {
      string targetPath = targetDir + "/" + sourceName + ".ballance";
      if (!string.IsNullOrEmpty(targetPath))
      {

        DoSolvePackageDef(packDefFile);

        if (string.IsNullOrEmpty(packPackageName))
          return "PackageDef.xml 必须填写包名 packageName";

        allAssetsPath.Clear();

        bool isCore = packPackageName == "core";
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
              string filesPath = files[i].FullName.Replace("\\", "/");

              if (filesPath.EndsWith(".meta")) continue;
              if (filesPath.EndsWith(".shader")) continue;
              if (filesPath.EndsWith(".shadergraph")) continue;
              if (filesPath.EndsWith(".cginc")) continue;
              if (filesPath.EndsWith(".md")) continue;
              if (filesPath.Contains("NoPackage")) continue;
              if (filesPath.EndsWith(".cs")) continue;
              if (filesPath.EndsWith(".csproj")) continue;
              if (isCore && filesPath.Contains("Levels")) continue;
              if (isCore && filesPath.Contains("Scripts/Native")) continue;

              allAssetsPath.Add(filesPath.Replace(projPath, ""));
            }
          }

          string name = Path.GetFileNameWithoutExtension(targetPath);

          //打包
          AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
          assetBundleBuild.assetBundleName = packPackageName;
          assetBundleBuild.assetBundleVariant = "assetbundle";
          assetBundleBuild.assetNames = allAssetsPath.ToArray();

          //打包
          BuildPipeline.BuildAssetBundles(dirTargetPath, new AssetBundleBuild[]{
                        assetBundleBuild
                    }, BuildAssetBundleOptions.None, packTarget);

          EditorUtility.DisplayProgressBar("正在打包", "正在打包，请稍后...", 0.6f);

          //ballance 包处理
          DoSolveBallancePack(packTarget, dirTargetPath, dirTargetPath + "/" + name, targetPath);

          EditorUtility.ClearProgressBar();
        }
        else return "PackageDef.xml 不在本项目中 ";
      }
      return "";
    }
    
    private static void DoSolvePackageDef(TextAsset packDefFile)
    {
      projPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/";
      projModDefFile = projPath + AssetDatabase.GetAssetPath(packDefFile);
      projModDirPath = projPath + Path.GetDirectoryName(AssetDatabase.GetAssetPath(packDefFile));
      projLogoFile = projModDirPath + Path.DirectorySeparatorChar + "PackageLogo.png";

      //模块信息处理
      XmlDocument packDefXmlDoc = new XmlDocument();
      packDefXmlDoc.LoadXml(packDefFile.text);
      XmlNode nodePackage = packDefXmlDoc.SelectSingleNode("Package");

      if (nodePackage.Attributes["name"] != null)
        packPackageName = nodePackage.Attributes["name"].Value;

      foreach (XmlNode node in nodePackage.ChildNodes)
      {
        if (node.Name == "BaseInfo")
        {
          foreach (XmlAttribute attribute in node.Attributes)
          {
            if (attribute.Name == "packageName")
              packPackageName = attribute.Value;
          }
          break;
        }
      }
    }
    private static void DoSolveBallancePack(BuildTarget target, string dirTargetPath, string bundlePath, string targetPath)
    {
      Crc32 crc = new Crc32();
      ZipOutputStream zipStream = ZipUtils.CreateZipFile(targetPath);
      string basePath = projModDirPath.Replace("\\", "/").Replace(projPath, "");

      //添加到包里
      ZipUtils.AddFileToZip(zipStream, bundlePath + ".assetbundle", "/assets/" + Path.GetFileName(bundlePath) + ".assetbundle", ref crc);
      ZipUtils.AddFileToZip(zipStream, bundlePath + ".assetbundle.manifest", "/assets/" + Path.GetFileName(bundlePath) + ".assetbundle.manifest", ref crc);
      ZipUtils.AddFileToZip(zipStream, projModDefFile, projModDirPath.Length, ref crc);

      //添加I18脚本
      var i18nRes = projModDirPath + Path.DirectorySeparatorChar + "PackageLanguageResPre.xml"; 
      if (File.Exists(i18nRes)) ZipUtils.AddFileToZip(zipStream, i18nRes, projModDirPath.Length, ref crc);

      //添加logo图片
      if (File.Exists(projLogoFile)) ZipUtils.AddFileToZip(zipStream, projLogoFile, projModDirPath.Length, ref crc);

      //添加DLL
      string projDllFile = projModDirPath + Path.DirectorySeparatorChar + ".dll" + Path.DirectorySeparatorChar + packPackageName + ".dll";
      if (File.Exists(projDllFile)) 
        ZipUtils.AddFileToZip(zipStream, projDllFile, "csharp/" + packPackageName + ".dll", ref crc);

      zipStream.Finish();
      zipStream.Close();
    }
  }
}
