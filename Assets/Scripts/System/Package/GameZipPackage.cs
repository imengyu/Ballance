using Ballance2.System.Debug;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Text;
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
* 更改历史：
* 2021-1-14 创建
*
*/

namespace Ballance2.System.Package
{
    public class GameZipPackage: GamePackage
    {
        private readonly string TAG = "GameZipPackage";


        public override void Destroy()
        {
            base.Destroy();
        }
        public override async Task<bool> LoadInfo(string filePath)
        {
            PackageFilePath = filePath;

            bool defFileLoadSuccess = false;
            bool defFileFounded = false;

            //在zip中加载ModDef
            ZipInputStream zip = ZipUtils.OpenZipFile(PackageFilePath);
            ZipEntry theEntry;
            while ((theEntry = zip.GetNextEntry()) != null)
            {
                if (theEntry.Name == "/PackageDef.xml" || theEntry.Name == "PackageDef.xml")
                    defFileLoadSuccess = await LoadModDefInZip(zip, theEntry);
                else if (theEntry.Name == "/" + BaseInfo.Logo || theEntry.Name == BaseInfo.Logo)
                    LoadLogoInZip(zip, theEntry);
            }
            zip.Close();
            zip.Dispose();

            if(!defFileFounded)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.PackageDefNotFound, TAG, "PackageDef.xml not found");
                LoadError = "模块并不包含 PackageDef.xml";
                return false;
            }

            return defFileLoadSuccess;
        }
        public override async Task<bool> LoadPackage()
        {
            //从zip读取AssetBundle
            MemoryStream ms = await LoadAssetBundleToMemoryInZipAsync();

            if (ms == null)
            {
                LoadError = "错误的包，并不包含 AssetBundle";
                GameErrorChecker.SetLastErrorAndLog(Debug.GameError.AssetBundleNotFound, TAG, "Not found AssetBundle in Package");
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
                GameErrorChecker.SetLastErrorAndLog(Debug.GameError.AssetBundleNotFound, TAG, "AssetBundle load failed in Package");
                return false;
            }

            return await base.LoadPackage();
        }

        private async Task<bool> LoadModDefInZip(ZipInputStream zip, ZipEntry theEntry)
        {
            MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);

            PackageDef = new XmlDocument();
            PackageDef.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));

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

        //在zip中加载AssetBundle
        private async Task<MemoryStream> LoadAssetBundleToMemoryInZipAsync()
        {
            ZipInputStream zip = ZipUtils.OpenZipFile(PackageFilePath);
            ZipEntry theEntry;
            while ((theEntry = zip.GetNextEntry()) != null)
            {
                if (theEntry.Name == PackageName + ".assetbundle"
                    || theEntry.Name == "/" + PackageName + ".assetbundle")
                {
                    MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);
                    zip.Close();
                    zip.Dispose();
                    return ms;
                }
            }

            zip.Close();
            zip.Dispose();
            return null;
        }
    }
}
