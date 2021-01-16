using Ballance2.Config;
using Ballance2.System.Debug;
using Ballance2.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameAssetBundlePackage.cs
* 
* 用途：
* 游戏模块（AssetBundle）声明
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
    /// <summary>
    /// 模块包 AssetBundle
    /// </summary>
    [SLua.CustomLuaClass]
    public class GameAssetBundlePackage : GamePackage
    {
        private readonly string TAG = "GameAssetBundlePackage";

        public override void Destroy()
        {
            base.Destroy();
        }

        public override async Task<bool> LoadInfo(string filePath)
        {
            await base.LoadInfo(filePath);

            UnityWebRequest request = UnityWebRequest.Get(filePath);
            await request.SendWebRequest();

            if (!request.isNetworkError && string.IsNullOrEmpty(request.error))
            {
                AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(request.downloadHandler.data);
                await assetBundleCreateRequest;
                AssetBundle = assetBundleCreateRequest.assetBundle;

                if (AssetBundle == null)
                {
                    LoadError = "错误的包，加载 AssetBundle 失败";
                    GameErrorChecker.SetLastErrorAndLog(GameError.AssetBundleNotFound, TAG, "Not found AssetBundle in Package");
                    return false;
                }
                else
                {
                    TextAsset modDefTextAsset = AssetBundle.LoadAsset<TextAsset>("PackageDef.xml");
                    if (modDefTextAsset == null || string.IsNullOrEmpty(modDefTextAsset.text))
                    {
                        GameErrorChecker.SetLastErrorAndLog(GameError.PackageDefNotFound, TAG, "PackageDef.xml not found");
                        LoadError = "模块并不包含 PackageDef.xml";
                        return false;
                    } 
                    else
                    {
                        PackageDef = new XmlDocument();
                        PackageDef.LoadXml(modDefTextAsset.text);
                        ReadInfo(PackageDef);

                        return true;
                    }
                }
            }
            else
            {
                if (request.responseCode == 404)
                    LoadError = "未找到资源包";
                else if (request.responseCode == 403)
                    LoadError = "无权限读取资源包";
                else
                    LoadError = "HTTP 请求错误 " + request.responseCode;

                GameErrorChecker.SetLastErrorAndLog(GameError.NetworkError, TAG, "Load AssetBundle failed : " + LoadError + "(" + request.responseCode + ")");
                return false;
            }

        }
    }
}
