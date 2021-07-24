using Ballance.LuaHelpers;
using Ballance2.Config;
using Ballance2.Sys.Res;
using Ballance2.Utils;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePackageInfo.cs
* 
* 用途：
* 游戏模块所用信息结构体和枚举声明
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-16 创建
*
*/

namespace Ballance2.Sys.Package
{
    /// <summary>
    /// 模块代码类型
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("模块代码类型")]
    public enum GamePackageCodeType
    {
        /// <summary>
        /// 无代码
        /// </summary>
        [LuaApiDescription("无代码")]
        None,
        /// <summary>
        /// 代码类型是 Lua
        /// </summary>
        [LuaApiDescription("代码类型是 Lua")]
        Lua,
        /// <summary>
        /// 代码类型是 C# DLL
        /// </summary>
        [LuaApiDescription("代码类型是 C# DLL")]
        CSharp
    }
    /// <summary>
    /// 模块类型
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("模块类型")]
    public enum GamePackageType
    {
        /// <summary>
        /// 代码包
        /// </summary>
        [LuaApiDescription("代码包")]
        Module,
        /// <summary>
        /// 资源包
        /// </summary>
        [LuaApiDescription("资源包")]
        Asset
    }
    /// <summary>
    /// 模块加载状态
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("模块加载状态")]
    public enum GamePackageStatus
    {
        /// <summary>
        /// 未加载
        /// </summary>
        [LuaApiDescription("未加载")]
        NotLoad = 0,
        /// <summary>
        /// 正在注册
        /// </summary>
        [LuaApiDescription("正在注册")]
        Registing = 1,
        /// <summary>
        /// 正在加载
        /// </summary>
        [LuaApiDescription("正在加载")]
        Loading = 2,
        /// <summary>
        /// 加载成功
        /// </summary>
        [LuaApiDescription("加载成功")]
        LoadSuccess = 3,
        /// <summary>
        /// 加载失败
        /// </summary>
        [LuaApiDescription("加载失败")]
        LoadFailed = 4,
        /// <summary>
        /// 正在等待卸载
        /// </summary>
        [LuaApiDescription("正在等待卸载")]
        UnloadWaiting = 5,
        /// <summary>
        /// 已经注册但未加载
        /// </summary>
        [LuaApiDescription("已经注册但未加载")]
        Registered = 6,
    }

    /// <summary>
    /// 模块基础信息
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("模块基础信息")]
    public class GamePackageBaseInfo
    {
        [SLua.DoNotToLua]
        public GamePackageBaseInfo(XmlNode xmlNodeBaseInfo)
        {
            if(xmlNodeBaseInfo != null)
            for (int i = 0; i < xmlNodeBaseInfo.ChildNodes.Count; i++)
            {
                switch (xmlNodeBaseInfo.ChildNodes[i].Name)
                {
                    case "Name": Name = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "Author": Author = FixCdData(xmlNodeBaseInfo.ChildNodes[i].InnerText); break;
                    case "Introduction": Introduction = FixCdData(xmlNodeBaseInfo.ChildNodes[i].InnerXml); break;
                    case "Logo": Logo = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "Link": Link = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "DocLink": DocLink = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "AuthorLink": AuthorLink = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "Description": Description = FixCdData(xmlNodeBaseInfo.ChildNodes[i].InnerText); break;
                    case "VersionName": VersionName = FixCdData(xmlNodeBaseInfo.ChildNodes[i].InnerText); break;
                    case "Dependencies":
                        for (int j = 0, jc = xmlNodeBaseInfo.ChildNodes[i].ChildNodes.Count; j < jc; j++)
                        {
                            var node = xmlNodeBaseInfo.ChildNodes[i].ChildNodes[j];
                            if(node.Attributes != null)
                                Dependencies.Add(new GamePackageDependencies(node));
                        }
                        break;
                }
            }
        }
        [SLua.DoNotToLua]
        public GamePackageBaseInfo(bool isSystemPackage) {
            if(isSystemPackage) {
                Name = "Ballance";
                Author = "";
                Introduction = "基础游戏系统";
                Description = "游戏的基础系统，承载游戏的基础代码";
                Link = GameConst.BallanceHomePage;
                DocLink = GameConst.BallanceDocPage;
                AuthorLink = GameConst.MengyuHomePage;
                LogoTexture = GameStaticResourcesPool.FindStaticAssets<Sprite>("GameLogo");
            } else {
                Name = "InternalPackage";
                LogoTexture = GameStaticResourcesPool.FindStaticAssets<Sprite>("PackageDefaultLogo");
            }
        }

        private string FixCdData(string x) {
            if(x.Length > 12 && x.StartsWith("<![CDATA[") && x.EndsWith("]]>"))
                return x.Substring(9, x.Length - 12);
            return x;
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        [LuaApiDescription("模块名称")]
        public string Name { get; private set; }
        /// <summary>
        /// 模块作者
        /// </summary>
        [LuaApiDescription("模块作者")]
        public string Author { get; private set; }
        /// <summary>
        /// 模块介绍
        /// </summary>
        [LuaApiDescription("模块介绍")]
        public string Introduction { get; private set; }
        /// <summary>
        /// 说明文字
        /// </summary>
        [LuaApiDescription("说明文字")]
        public string Description { get; private set; }
        /// <summary>
        /// 模块Logo
        /// </summary>
        [LuaApiDescription("模块Logo")]
        public string Logo { get; private set; }
        /// <summary>
        /// 模块Logo Sprite
        /// </summary>
        [LuaApiDescription("模块Logo Sprite")]
        public Sprite LogoTexture { get; set; }
        /// <summary>
        /// 模块链接
        /// </summary>
        [LuaApiDescription("模块链接")]
        public string Link { get; private set; }
        /// <summary>
        /// 模块作者主页URL
        /// </summary>
        [LuaApiDescription("模块作者主页URL")]
        public string AuthorLink { get; private set; }
        /// <summary>
        /// 模块文档URL
        /// </summary>
        [LuaApiDescription("模块文档URL")]
        public string DocLink { get; private set; }
        /// <summary>
        /// 显示给用户看的版本
        /// </summary>
        [LuaApiDescription("显示给用户看的版本")]
        public string VersionName { get; private set; }
        /// <summary>
        /// 模块依赖
        /// </summary>
        [LuaApiDescription("模块依赖")]
        public List<GamePackageDependencies> Dependencies { get; } = new List<GamePackageDependencies>();
    }
    /// <summary>
    /// 模块依赖信息
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("模块依赖信息")]
    public class GamePackageDependencies
    {
        [SLua.DoNotToLua]
        public GamePackageDependencies(XmlNode xmlNode)
        {
            if(xmlNode != null) {
                for (int i = 0; i < xmlNode.Attributes.Count; i++)
                {
                    switch(xmlNode.Attributes[i].Name)
                    {
                        case "name":
                            Name = xmlNode.Attributes[i].Value;
                            break;
                        case "minVer":
                            MinVersion = ConverUtils.StringToInt(xmlNode.Attributes[i].Value,
                                0, "Dependencies/minVer");
                            break;
                        case "mustLoad":
                            MustLoad = ConverUtils.StringToBoolean(xmlNode.Attributes[i].Value, 
                                false, "Dependencies/mustLoad");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 依赖模块包名
        /// </summary>
        [LuaApiDescription("依赖模块包名")]
        public string Name { get; private set; }
        /// <summary>
        /// 依赖模块最低版本
        /// </summary>
        [LuaApiDescription("依赖模块最低版本")]
        public int MinVersion { get; private set; }
        /// <summary>
        /// 依赖模块是否必须加载
        /// </summary>
        [LuaApiDescription("依赖模块是否必须加载")]
        public bool MustLoad { get; private set; }

    }
}
