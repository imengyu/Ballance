using Ballance2.Services.Debug;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

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
    public override async Task<bool> LoadInfo(string filePath)
    {
      PackageFilePath = PathUtils.FixFilePathScheme(filePath);

      ZipInputStream zip = null;
      MemoryStream ms = null;

      try {
        //如果路径以jar:开头，则使用www方式读取
        if(PackageFilePath.Contains("jar:file://")) {

          UnityWebRequest request = UnityWebRequest.Get(PackageFilePath);
          await request.SendWebRequest();

          if (request.result == UnityWebRequest.Result.Success)
          {
            ms = new MemoryStream(request.downloadHandler.data);
            zip = ZipUtils.OpenZipStream(ms);
          }
          else {
            GameErrorChecker.SetLastErrorAndLog(GameError.RequestFailed, TAG, "Request " + PackageFilePath + " failed. Error: " + request.error);
            return false;
          }
        }
        else {
          //在zip中加载Def
          zip = ZipUtils.OpenZipFile(PackageFilePath);
        }
        if (zip == null)
          return false;

        UpdateTime = File.GetLastWriteTime(PackageFilePath);

        XmlDocument packageDef = null;
        Sprite logo = null;

        //搜索zip内容
        ZipEntry theEntry;
        while ((theEntry = zip.GetNextEntry()) != null)
        {
          if (ZipUtils.MatchRootName("PackageDef.xml", theEntry))
            packageDef = await LoadPackageDefInZip(zip, theEntry); //加载定义文件
          else if (ZipUtils.MatchRootName("PackageLanguageResPre.xml", theEntry))
            await LoadPackageLanguageResPreInZip(zip, theEntry); //加载语言文件
          else if (ZipUtils.MatchRootName("PackageLogo.png", theEntry))
            logo = await LoadLogoInZip(zip, theEntry); //加载图标
        }

        zip.Close();
        zip.Dispose();

        if (packageDef == null)
        {
          GameErrorChecker.SetLastErrorAndLog(GameError.PackageDefNotFound, TAG, "PackageDef.xml not found");
          LoadError = "模块并不包含 PackageDef.xml";
          return false;
        } 
        else 
        {
          bool res = ReadInfo(packageDef);
          BaseInfo.LogoTexture = logo;
          return res;
        }
      }
      catch(Exception e) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "加载异常 " + e.ToString());
      }
      finally { 
        if(ms != null)
          ms.Dispose();
        if(zip != null)
          zip.Dispose();
      }
      return false;
    }
    public override async Task<bool> LoadPackage()
    {
      ZipInputStream zip = null;
      MemoryStream ms = null;

      try{
        //如果路径以jar:开头，则使用www方式读取
        if(PackageFilePath.StartsWith("jar:")) {

          UnityWebRequest request = UnityWebRequest.Get(PackageFilePath);
          await request.SendWebRequest();

          if (request.result == UnityWebRequest.Result.Success)
          {
            ms = new MemoryStream(request.downloadHandler.data);
            zip = ZipUtils.OpenZipStream(ms);
          }
          else {
            GameErrorChecker.SetLastErrorAndLog(GameError.RequestFailed, TAG, "Request " + PackageFilePath + " failed. Error: " + request.error);
            return false;
          }
        }
        else {
          //在zip中加载Def
          zip = ZipUtils.OpenZipFile(PathUtils.FixFilePathScheme(PackageFilePath));
        }

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
        }

        if(assetbundleFound) 
          result = await base.LoadPackage();
        else
          GameErrorChecker.SetLastErrorAndLog(GameError.FileNotFound, TAG, "未找到 " + PackageName + ".assetbundle");
        return result;
      }
      catch(Exception e) {
        GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, TAG, "加载异常 " + e.ToString());
      }
      finally { 
        if(ms != null)
          ms.Dispose();
        if(zip != null)
          zip.Dispose();
      }
      return false;
    }

    private async Task<XmlDocument> LoadPackageDefInZip(ZipInputStream zip, ZipEntry theEntry)
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
        return null;
      }
      finally
      {
        ms.Close();
        ms.Dispose();
      }

      return PackageDef;
    }
    private async Task<bool> LoadPackageLanguageResPreInZip(ZipInputStream zip, ZipEntry theEntry)
    {
      MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);
      try
      {
        PreLoadI18NResource(StringUtils.GetUtf8Bytes(StringUtils.FixUtf8BOM(ms.ToArray())));
      }
      catch
      {
        return false;
      }
      finally
      {
        ms.Close();
        ms.Dispose();
      }
      return true;
    }
    private async Task<Sprite> LoadLogoInZip(ZipInputStream zip, ZipEntry theEntry)
    {
      try
      {
        Texture2D texture2D = new Texture2D(128, 128);
        MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);
        texture2D.LoadImage(ms.ToArray());
        ms.Close();
        ms.Dispose();

        return Sprite.Create(texture2D,
            new Rect(Vector2.zero, new Vector2(texture2D.width, texture2D.height)),
            new Vector2(0.5f, 0.5f));
      }
      catch (Exception e)
      {
        Log.E(TAG, "在加载模块的 Logo {0} 失败\n错误信息：{1}", BaseInfo.Logo, e.ToString());
        return null;
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
    protected override Assembly LoadCodeCSharp(string pathorname)
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
