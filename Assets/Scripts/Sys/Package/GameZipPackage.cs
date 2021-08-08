using Ballance2.Sys.Debug;
using Ballance2.Sys.Res;
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
*
* 
* 
*
*/

namespace Ballance2.Sys.Package
{
    public class GameZipPackage: GamePackage
    {
        private struct CodeAsset {
            public string asset;
            public string fullPath;
            public CodeAsset(string asset, string fullPath) {
                this.asset = asset;
                this.fullPath = fullPath;
            }
        }
        private Dictionary<string, CodeAsset> packageCodeAsset = new Dictionary<string, CodeAsset>();

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
            ZipInputStream zip = ZipUtils.OpenZipFile(GamePathManager.FixFilePathScheme(PackageFilePath));
            if(zip == null)
                return false;

            UpdateTime = File.GetLastWriteTime(GamePathManager.FixFilePathScheme(PackageFilePath));

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

        protected bool disableZipLoad = false;

        public override async Task<bool> LoadPackage()
        {
            if(disableZipLoad)
                return await base.LoadPackage();
            //从zip读取AssetBundle
            ZipInputStream zip = ZipUtils.OpenZipFile(GamePathManager.FixFilePathScheme(PackageFilePath));
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
                    {
                        assetbundleFound = true;
                        result = await base.LoadPackage();
                    }
                }
                else if(theEntry.Name.StartsWith("class/"))
                {
                    await LoadCodeAsset(zip, theEntry);
                }
            }

            if(!assetbundleFound)
                GameErrorChecker.SetLastErrorAndLog(GameError.FileNotFound,
                    TAG, "未找到 " + PackageName + ".assetbundle");

            return result;
        }

        private async Task<bool> LoadPackageDefInZip(ZipInputStream zip, ZipEntry theEntry)
        {
            MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);

            PackageDef = new XmlDocument();
            try {
                PackageDef.LoadXml(StringUtils.FixUtf8BOM(ms.ToArray()));
            }  catch(System.Exception e) {
                GameErrorChecker.SetLastErrorAndLog(GameError.PackageIncompatible, TAG, "Format error in PackageDef.xml : " + e);
                return false;
            }

            ms.Close();
            ms.Dispose();

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

            packageCodeAsset.Add(theEntry.Name, new CodeAsset(StringUtils.FixUtf8BOM(ms.ToArray()), theEntry.Name));

            //Log.D(TAG, "LoadCodeAsset: {0} -> \n{1}", theEntry.Name, LuaUtils.PrintBytes(ms.ToArray()));

            ms.Close();
            ms.Dispose();
        }

        public override string GetCodeLuaAsset(string pathorname, out string realPath)
        {
            foreach (string key in packageCodeAsset.Keys)
            {
                if (key == pathorname
                        || key == "class" + pathorname
                        || key == "class/" + pathorname
                        || Path.GetFileName(key) == pathorname) {
                    var k = packageCodeAsset[key];
                    realPath = k.fullPath;
                    return k.asset;
                }
            }

            GameErrorChecker.LastError = GameError.FileNotFound;
            realPath = "";
            return null;
        }
        public override Assembly LoadCodeCSharp(string pathorname)
        {
#if ENABLE_MONO || ENABLE_DOTNET

            ZipInputStream zip = ZipUtils.OpenZipFile(GamePathManager.FixFilePathScheme(PackageFilePath));
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
