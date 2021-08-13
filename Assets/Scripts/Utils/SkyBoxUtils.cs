using Ballance2.LuaHelpers;
using Ballance2.Sys;
using Ballance2.Sys.Package;
using Ballance2.Sys.Services;
using Ballance2.Utils;
using UnityEngine;

/*

* Copyright(c) 2021  mengyu
*
* 模块名：     
* SkyBoxUtils.cs
* 
* 用途：
* 天空盒生成工具类
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Game.Utils
{
    /// <summary>
    /// 天空盒生成器
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("天空盒生成器")]
    public static class SkyBoxUtils
    {
        private const string TAG = "SkyBoxUtils";

        private static GamePackage skyAssetPack = null;
        private static GamePackageManager GamePackageManager = null;

        /// <summary>
        /// 创建预制的天空盒
        /// </summary>
        /// <param name="s">天空盒名字，（必须是 A~K ，对应原版游戏11个天空）</param>
        /// <returns>返回创建好的天空盒材质</returns>
        [LuaApiDescription("创建预制的天空盒", "返回创建好的天空盒材质")]
        [LuaApiParamDescription("s", "天空盒名字，（必须是 A~K ，对应原版游戏11个天空）")]
        public static Material MakeSkyBox(string s)
        { 
            if (GamePackageManager == null) GamePackageManager = GameManager.Instance.GetSystemService<GamePackageManager>();
            if (skyAssetPack == null) skyAssetPack = GamePackageManager.FindPackage("core.assets.skys");
            if (skyAssetPack == null)
            {
                Log.E(TAG, "MakeSkyBox failed because skybase pack core.assets.skys not load !");
                return null;
            }

            Texture SkyLeft = skyAssetPack.GetAsset<Texture>("Sky_"+s+"_Left.BMP");
            Texture SkyRight = skyAssetPack.GetAsset<Texture>("Sky_" + s + "_Right.BMP");
            Texture SkyFront = skyAssetPack.GetAsset<Texture>("Sky_" + s + "_Front.BMP");
            Texture SkyBack = skyAssetPack.GetAsset<Texture>("Sky_" + s + "_Back.BMP");
            Texture SkyDown = skyAssetPack.GetAsset<Texture>("Sky_" + s + "_Down.BMP");

            return MakeCustomSkyBox(SkyLeft, SkyRight, SkyFront, SkyBack, SkyDown, null);
        }
        /// <summary>
        /// 创建自定义天空盒
        /// </summary>
        /// <param name="SkyLeft">左边的图像</param>
        /// <param name="SkyRight">右边的图像</param>
        /// <param name="SkyFront">前边的图像</param>
        /// <param name="SkyBack">后边的图像</param>
        /// <param name="SkyDown">下边的图像</param>
        /// <param name="SkyTop">上边的图像</param>
        /// <returns>返回创建好的天空盒材质</returns>
        [LuaApiDescription("创建自定义天空盒", "返回创建好的天空盒材质")]
        [LuaApiParamDescription("SkyLeft", "左边的图像")]
        [LuaApiParamDescription("SkyRight", "右边的图像")]
        [LuaApiParamDescription("SkyFront", "前边的图像")]
        [LuaApiParamDescription("SkyBack", "后边的图像")]
        [LuaApiParamDescription("SkyDown", "下边的图像")]
        [LuaApiParamDescription("SkyTop", "上边的图像")]
        public static Material MakeCustomSkyBox(Texture SkyLeft, Texture SkyRight, Texture SkyFront, Texture SkyBack, Texture SkyDown, Texture SkyTop)
        {
            Material m = new Material(Shader.Find("Skybox/6 Sided"));
            m.SetTexture("_FrontTex", SkyFront);
            m.SetTexture("_BackTex", SkyBack);
            m.SetTexture("_LeftTex", SkyRight);
            m.SetTexture("_RightTex", SkyLeft);
            m.SetTexture("_DownTex", SkyDown);
            m.SetTexture("_TopTex", SkyTop);
            return m;
        }
    }
}
