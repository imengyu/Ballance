using Ballance2.Config;
using UnityEngine;
using System.IO;
using Ballance2.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Ballance2.Services.Debug;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystemPackage.cs
* 
* 用途：
* 游戏主模块特殊结构的声明
*
* 作者：
* mengyu
*/

namespace Ballance2.Package
{  
  class GameSystemPackage : GameZipPackage
  {

    public override async Task<bool> LoadInfo(string filePath)
    {
      PackageFilePath = filePath;
      if (disableLoadFileInUnity)
      {
        return await base.LoadInfo(filePath);
      }
      else
      {
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
          }
          catch (System.Exception e)
          {
            GameErrorChecker.SetLastErrorAndLog(GameError.PackageIncompatible, TAG, "Format error in PackageDef.xml : " + e);
            return false;
          }
          UpdateTime = File.GetLastWriteTime(defPath);
          return ReadInfo(PackageDef);
        }
      }
    }
    public override async Task<bool> LoadPackage()
    {
      if (disableLoadFileInUnity)
      {
        return await base.LoadPackage();
      }
      else
      {
        if (!string.IsNullOrEmpty(BaseInfo.Logo))
          LoadLogo(PackageFilePath + "/" + BaseInfo.Logo);

        DoSearchScriptNames();
        LoadAllFileNames();

        disableZipLoad = true;
        return await base.LoadPackage();
      }
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
      catch (System.Exception e)
      {
        BaseInfo.LogoTexture = null;
        Log.E(TAG, "在加载模块的 Logo {0} 失败\n错误信息：{1}", path, e.ToString());
      }
    }

    private List<CodeAsset> packageCodeAsset = new List<CodeAsset>();
    private Dictionary<string, string> fileList = new Dictionary<string, string>();

    private void LoadAllFileNames()
    {
      DirectoryInfo theFolder = new DirectoryInfo(PackageFilePath);
      FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.AllDirectories);
      foreach (FileInfo NextFile in thefileInfo)
      { //遍历文件
        string path = NextFile.FullName.Replace("\\", "/");
        if (path.EndsWith(".meta")) continue;
        if (path.EndsWith(".js")) continue;
        int index = path.IndexOf("Assets/");
        if (index > 0)
          path = path.Substring(index);

        fileList.Add(NextFile.Name, path);
      }
    }
    private string GetFullPathByName(string name)
    {
      if (fileList.TryGetValue(name, out string fullpath))
        return fullpath;
      return null;
    }
    public override void Destroy()
    {
      packageCodeAsset.Clear();
      base.Destroy();
    }

    private bool disableLoadFileInUnity = false;

    public void SetDisableLoadFileInUnity()
    {
      disableLoadFileInUnity = true;
    }
    public void ManualLoadPackageCodeBase() {
      LoadPackageCodeBase();
    }
    
    private CodeAsset FindScriptName(string pathOrName) {
      foreach (var item in packageCodeAsset)
      {
        if(item.Match(pathOrName))
          return item;
      }
      return null;
    }
    private void DoSearchScriptNames()
    {
#if UNITY_EDITOR
      //构建一下所有脚本名称和路径的列表
      DirectoryInfo dir = new DirectoryInfo(ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_SCRIPT_PATH);//扫描game模块下的代码
      FileInfo[] fi = dir.GetFiles("*.js", SearchOption.AllDirectories);
      string pathFinder = "Assets/Game/Scripts/";
      foreach (var f in fi)
      {
        string path = f.FullName.Replace("\\", "/"), realtivePath = path;
        int index = path.IndexOf(pathFinder);
        if (index > 0)
          realtivePath = path.Substring(index + pathFinder.Length);
        packageCodeAsset.Add(new CodeAsset(f.Name, realtivePath, path));
      }
      dir = new DirectoryInfo(ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ENV_SCRIPT_PATH);//扫描system模块下的代码
      fi = dir.GetFiles("*.js", SearchOption.AllDirectories);
      pathFinder = "Assets/System/Scripts/";
      foreach (var f in fi)
      {
        string path = f.FullName.Replace("\\", "/"), realtivePath = path;
        int index = path.IndexOf(pathFinder);
        if (index > 0)
          realtivePath = path.Substring(index + pathFinder.Length);
        packageCodeAsset.Add(new CodeAsset(f.Name, realtivePath, path));
      }
      //添加PackageEntry.js
      string entryPath = ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ASSET_PATH + "PackageEntry.js";
      packageCodeAsset.Add(new CodeAsset("PackageEntry.js", entryPath, Directory.GetCurrentDirectory() + "/" + entryPath));
#endif
    }

    public override T GetAsset<T>(string pathorname)
    {
#if UNITY_EDITOR
      if (disableLoadFileInUnity)
      {
        return base.GetAsset<T>(pathorname);
      }
      else
      {
        if (pathorname.StartsWith("Assets"))
          return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(pathorname);
        var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ASSET_PATH + pathorname);
        if (asset == null && !pathorname.Contains("/") && !pathorname.Contains("\\"))
        {
          string fullPath = GetFullPathByName(pathorname);
          if (fullPath != null)
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        return asset;
      }
#else
      return base.GetAsset<T>(pathorname);
#endif
    }
    public override bool CheckCodeAssetExists(string pathorname)
    {
#if UNITY_EDITOR
      if (disableLoadFileInUnity)
      {
        return base.CheckCodeAssetExists(pathorname);
      }
      else
      {
        //绝对路径
        if (PathUtils.IsAbsolutePath(pathorname) || pathorname.StartsWith("Assets"))
        {
          return File.Exists(pathorname) || File.Exists(pathorname + ".js") ;
        }
        //直接拼接路径
        var scriptPath = ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_SCRIPT_PATH + pathorname;
        if (File.Exists(scriptPath) || File.Exists(scriptPath + ".js"))
        {
          return true;
        }
        //尝试使用路径列表里的路径
        var code = FindScriptName(pathorname);
        if (code != null)
        {
          return true;
        }
        code = FindScriptName(pathorname + ".js");
        if (code != null)
        {
          return true;
        }
        return false;
      }
#else
      return base.CheckCodeAssetExists(pathorname);
#endif  
    }
    public override byte[] GetCodeAsset(string pathorname, out string realPath)
    {
#if UNITY_EDITOR
      if (disableLoadFileInUnity)
      {
        return base.GetCodeAsset(pathorname, out realPath);
      }
      else
      {
        //绝对路径
        if (PathUtils.IsAbsolutePath(pathorname) || pathorname.StartsWith("Assets"))
        {
          if(File.Exists(pathorname)) {
            realPath = pathorname;
            return FileUtils.ReadAllToBytes(pathorname);
          } else if(File.Exists(pathorname + ".js")) {
            realPath = pathorname + ".js";
            return FileUtils.ReadAllToBytes(pathorname + ".js");
          } 
        }
        //直接拼接路径
        var scriptPath = ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_SCRIPT_PATH + pathorname;
        if (File.Exists(scriptPath))
        {
          realPath = scriptPath;
          return FileUtils.ReadAllToBytes(scriptPath);
        } 
        else if (File.Exists(scriptPath + ".js"))
        {
          realPath = scriptPath + ".js";
          return FileUtils.ReadAllToBytes(scriptPath + ".js");
        } 
        //尝试使用路径列表里的路径
        var code = FindScriptName(pathorname);
        if (code != null)
        {
          realPath = code.fullPath;
          return FileUtils.ReadAllToBytes(code.fullPath);
        }
        code = FindScriptName(pathorname + ".js");
        if (code != null)
        {
          realPath = code.fullPath;
          return FileUtils.ReadAllToBytes(code.fullPath);
        }
        realPath = "";
        return null;
      }
#else
      return base.GetCodeLuaAsset(pathorname, out realPath);
#endif
    }
  }
}
