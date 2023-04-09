using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameEditorDebugPackage.cs
* 
* 用途：
* 提供游戏模块在Unity编辑器中直接加载的模式
*
* 作者：
* mengyu
*/

#pragma warning disable 1998

namespace Ballance2.Package
{
  public class GameEditorDebugPackage : GamePackage
  {
    public override async Task<bool> LoadInfo(string filePath)
    {
      PackageFilePath = filePath;
#if !UNITY_EDITOR
      GameErrorChecker.SetLastErrorAndLog(GameError.OnlyCanUseInEditor, TAG, "This package can only use in editor.");
      await base.LoadInfo(filePath);
      return false;
#else

      string defPath = filePath + "/PackageDef.xml";
      if (!File.Exists(defPath))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.PackageDefNotFound, TAG, "PackageDef.xml not found");
        LoadError = "模块并不包含 PackageDef.xml";
        return false;
      }
      else
      {
        try
        {
          PackageDef = new XmlDocument();
          PackageDef.Load(defPath);
          XmlNode nodePackage = PackageDef.SelectSingleNode("Package");
          XmlAttribute attributeName = nodePackage.Attributes["name"];
          PackageName = attributeName.Value;
        }
        catch (Exception e)
        {
          GameErrorChecker.SetLastErrorAndLog(GameError.PackageIncompatible, TAG, "Format error in PackageDef.xml : " + e);
          return false;
        }
        try
        {
          PreLoadI18NResource(null);
        }
        catch (Exception e)
        {
          Log.W(TAG, "Pre load language failed : " + e);
        }
        UpdateTime = File.GetLastWriteTime(defPath);
        if(ReadInfo(PackageDef)) 
        {
          if (!string.IsNullOrEmpty(BaseInfo.Logo))
            LoadLogo(PackageFilePath + "/" + BaseInfo.Logo);
          return true;
        }
        return false;
      }
#endif
    }
    public override async Task<bool> LoadPackage()
    {
      LoadAllFileNames();
      return await base.LoadPackage();
    }

    private Dictionary<string, string> fileList = new Dictionary<string, string>();
    private void LoadAllFileNames()
    {
      DirectoryInfo theFolder = new DirectoryInfo(PackageFilePath);
      FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.AllDirectories);
      foreach (FileInfo NextFile in thefileInfo)
      { //遍历文件
        string path = NextFile.FullName.Replace("\\", "/");

        if (path.EndsWith(".meta")) continue;
        if (path.EndsWith(".map")) continue;

        int index = path.IndexOf("Assets/");
        if (index > 0)
          path = path.Substring(index);

        if(!fileList.ContainsKey(NextFile.Name))
          fileList.Add(NextFile.Name, path);
      }
    }
    private string GetFullPathByName(string name)
    {
      if (fileList.TryGetValue(name, out string fullpath))
        return fullpath;
      return null;
    }
    private void LoadLogo(string path)
    {
      try
      {
        Texture2D texture2D = new Texture2D(128, 128);
        texture2D.LoadImage(File.ReadAllBytes(path));

        BaseInfo.LogoTexture = Sprite.Create(texture2D,
            new Rect(Vector2.zero, new Vector2(texture2D.width, texture2D.height)),
            new Vector2(0.5f, 0.5f));
      }
      catch (Exception e)
      {
        BaseInfo.LogoTexture = null;
        Log.E(TAG, "在加载模块的 Logo {0} 失败\n错误信息：{1}", path, e.ToString());
      }
    }

    protected virtual string DebugFolder => GamePathManager.DEBUG_PACKAGE_FOLDER;

    public override T GetAsset<T>(string pathorname)
    {
#if UNITY_EDITOR
      if (pathorname.StartsWith("Assets/"))
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(pathorname);
      else
      {


        var path = (PackageName == GamePackageManager.SYSTEM_PACKAGE_NAME ? DebugFolder : (DebugFolder + "/" + PackageName)) + "/" + pathorname;
        var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        if (asset == null && pathorname.Contains("."))
          asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(Path.GetFileNameWithoutExtension(path));
        if (asset == null && !pathorname.Contains("/") && !pathorname.Contains("\\"))
        {
          string fullPath = GetFullPathByName(pathorname);
          if (fullPath != null)
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        return asset;
      }
#else
      GameErrorChecker.SetLastErrorAndLog(GameError.OnlyCanUseInEditor, TAG, "Package can only use in editor.");
      return null;
#endif
    }
  }
}
