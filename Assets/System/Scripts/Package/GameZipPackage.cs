using Ballance2.Services.Debug;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Zip;
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
* GameZipPackage.cs
* 
* 用途：
* 游戏模块(Zip)声明
*
* 作者：
* mengyu
*/

namespace Ballance2.Package
{
  public class GameZipPackage : GamePackage
  {
    public override void Destroy()
    {
      packageCodeAsset.Clear();
      base.Destroy();
    }
    public override async Task<bool> LoadInfo(string filePath)
    {
      PackageFilePath = filePath;

      bool defFileLoadSuccess = false;
      bool defFileFounded = false;

      //在zip中加载Def
      ZipInputStream zip = ZipUtils.OpenZipFile(PathUtils.FixFilePathScheme(PackageFilePath));
      if (zip == null)
        return false;

      UpdateTime = File.GetLastWriteTime(PathUtils.FixFilePathScheme(PackageFilePath));

      ZipEntry theEntry;
      while ((theEntry = zip.GetNextEntry()) != null)
      {
        if (theEntry.Name == "/PackageDef.xml" || theEntry.Name == "PackageDef.xml")
        {
          defFileFounded = true;
          defFileLoadSuccess = await LoadPackageDefInZip(zip, theEntry);
        }
        else if (BaseInfo != null &&
            (theEntry.Name == "/" + BaseInfo.Logo || theEntry.Name == BaseInfo.Logo))
          LoadLogoInZip(zip, theEntry);
      }

      zip.Close();
      zip.Dispose();

      if (!defFileFounded)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.PackageDefNotFound, TAG, "PackageDef.xml not found");
        LoadError = "模块并不包含 PackageDef.xml";
        return false;
      }

      return defFileLoadSuccess;
    }

    private struct CodeAssetStorage
    {
      public byte[] asset;
      public string innerPath;
      public string fullPath;
      public CodeAssetStorage(byte[] asset, string innerPath, string fullPath)
      {
        this.asset = asset;
        this.innerPath = innerPath;
        this.fullPath = fullPath;
      }
    }
    private Dictionary<string, CodeAssetStorage> packageCodeAsset = new Dictionary<string, CodeAssetStorage>();

    public override async Task<bool> LoadPackage()
    {
      //从zip读取AssetBundle
      ZipInputStream zip = ZipUtils.OpenZipFile(PathUtils.FixFilePathScheme(PackageFilePath));
      if (zip == null)
        return false;

      bool result = false;
      bool assetbundleFound = false;

      ZipEntry theEntry;
      while ((theEntry = zip.GetNextEntry()) != null)
      {
        if (theEntry.Name == "assets/" + PackageName + ".assetbundle")
        {
          if (await LoadAssetBundleToMemoryInZipAsync(zip, theEntry))
            assetbundleFound = true;
        }
        else if (theEntry.Name.StartsWith("class/"))
        {
          await LoadCodeAsset(zip, theEntry);
        }
      }

      if(assetbundleFound) 
        result = await base.LoadPackage();
      else
        GameErrorChecker.SetLastErrorAndLog(GameError.FileNotFound, TAG, "未找到 " + PackageName + ".assetbundle");

      return result;
    }

    private async Task<bool> LoadPackageDefInZip(ZipInputStream zip, ZipEntry theEntry)
    {
      MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);

      PackageDef = new XmlDocument();

      string content = "";

      try
      {
        content = StringUtils.GetUtf8Bytes(StringUtils.FixUtf8BOM(ms.ToArray()));
        PackageDef.LoadXml(content);
      }
      catch (System.Exception e)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.PackageIncompatible, TAG, "Format error in PackageDef.xml : " + e + "\nCheck content: " + content);
        return false;
      }
      finally
      {
        ms.Close();
        ms.Dispose();
      }

      return ReadInfo(PackageDef);
    }
    private void LoadLogoInZip(ZipInputStream zip, ZipEntry theEntry)
    {
      try
      {
        Texture2D texture2D = new Texture2D(128, 128);
        MemoryStream ms = ZipUtils.ReadZipFileToMemory(zip);
        texture2D.LoadImage(ms.ToArray());
        ms.Close();
        ms.Dispose();

        BaseInfo.LogoTexture = Sprite.Create(texture2D,
            new Rect(Vector2.zero, new Vector2(texture2D.width, texture2D.height)),
            new Vector2(0.5f, 0.5f));
      }
      catch (Exception e)
      {
        BaseInfo.LogoTexture = null;
        Log.E(TAG, "在加载模块的 Logo {0} 失败\n错误信息：{1}", BaseInfo.Logo, e.ToString());
      }
    }

    private async Task<bool> LoadAssetBundleToMemoryInZipAsync(ZipInputStream zip, ZipEntry theEntry)
    {
      MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);

      if (ms == null)
      {
        LoadError = "错误的包，并不包含 AssetBundle";
        GameErrorChecker.SetLastErrorAndLog(GameError.AssetBundleNotFound, TAG, "Not found AssetBundle in Package");
        return false;
      }

      //加载 AssetBundle
      AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(ms.ToArray());
      await createRequest;

      AssetBundle = createRequest.assetBundle;

      ms.Close();
      ms.Dispose();

      if (AssetBundle == null)
      {
        LoadError = "错误的包，加载 AssetBundle 失败";
        GameErrorChecker.SetLastErrorAndLog(GameError.AssetBundleNotFound, TAG, "AssetBundle load failed in Package");
        return false;
      }

      Log.D(TAG, "AssetBundle {0} loaded", theEntry.Name);
      return true;
    }
    private async Task LoadCodeAsset(ZipInputStream zip, ZipEntry theEntry)
    {
      MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);

      var codeAssetStorage = new CodeAssetStorage(StringUtils.FixUtf8BOM(ms.ToArray()), theEntry.Name, PackageFilePath + "/" + theEntry.Name);
      var key = theEntry.Name;
      packageCodeAsset.Add(key, codeAssetStorage);
      key = Path.GetFileName(theEntry.Name);
      if(!packageCodeAsset.ContainsKey(key))
        packageCodeAsset.Add(key, codeAssetStorage);
      key = Path.GetFileNameWithoutExtension(theEntry.Name);
      if(!packageCodeAsset.ContainsKey(key))
        packageCodeAsset.Add(key, codeAssetStorage);

      ms.Close();
      ms.Dispose();
    }

    public override CodeAsset GetCodeAsset(string pathorname)
    {
      if (packageCodeAsset.TryGetValue(pathorname, out var k))
        return new CodeAsset(k.asset, k.fullPath, k.innerPath, k.fullPath);
      if (packageCodeAsset.TryGetValue(Path.GetFileName(pathorname), out k))
        return new CodeAsset(k.asset, k.fullPath, k.innerPath, k.fullPath);
      if (packageCodeAsset.TryGetValue(Path.GetFileNameWithoutExtension(pathorname), out k))
        return new CodeAsset(k.asset, k.fullPath, k.innerPath, k.fullPath);

      GameErrorChecker.LastError = GameError.FileNotFound;
      return null;
    }
    public override Assembly LoadCodeCSharp(string pathorname)
    {
#if ENABLE_MONO || ENABLE_DOTNET

      ZipInputStream zip = ZipUtils.OpenZipFile(PathUtils.FixFilePathScheme(PackageFilePath));
      if (zip == null)
        return null;

      ZipEntry theEntry;
      while ((theEntry = zip.GetNextEntry()) != null)
      {
        if (theEntry.Name == "csharp/" + pathorname)
        {
          MemoryStream ms = ZipUtils.ReadZipFileToMemory(zip);
          try
          {
            return Assembly.Load(ms.ToArray());
          }
          catch (Exception e)
          {
            Log.E(TAG, "模组包 {0} 加载代码 {1} 错误, 错误：\n{2}", PackageName, pathorname, e.ToString());
            return null;
          } 
          finally 
          {
            if(ms != null)
              ms.Dispose();
          }
        }
      }

      Log.E(TAG, "模组包 {0} 加载代码错误, 代码 {1} 未找到", PackageName, pathorname);
#else
      Log.E(TAG, "模组包 {0} 加载代码错误：当前使用 IL2CPP 模式，因此 C# DLL 不能加载", PackageName);
#endif
      return null;
    }
  }
}
