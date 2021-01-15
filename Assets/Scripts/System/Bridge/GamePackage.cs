using Ballance2.Config;
using Ballance2.System.Debug;
using Ballance2.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePackage.cs
* 
* 用途：
* 游戏模块声明
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-14 创建
*
*/

namespace Ballance2.System.Bridge
{
    /// <summary>
    /// 模块包实例
    /// </summary>
    [SLua.CustomLuaClass]
    public class GamePackage
    {
        private readonly string TAG = "GamePackage";

        [SLua.DoNotToLua]
        public bool ReadInfo(XmlDocument xml)
        {
            XmlNode nodePackage = xml.SelectSingleNode("Package");
            XmlAttribute attributeName = nodePackage.Attributes["name"];
            XmlAttribute attributeVersion = nodePackage.Attributes["version"];
            XmlNode nodeBaseInfo = nodePackage.SelectSingleNode("BaseInfo");

            if (attributeName == null)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.MissingAttribute, TAG, "Package attribute name is null");
                return false;
            }
            if (attributeVersion == null)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.MissingAttribute, TAG, "Package attribute version is null");
                return false;
            }
            if (nodeBaseInfo == null)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.MissingAttribute, TAG, "Package node BaseInfo is null");
                return false;
            }

            //Version and PackageName
            PackageName = attributeName.Value;
            PackageVersion = ConverUtils.StringToInt(attributeVersion.Value, 0, "Package/version");

            //BaseInfo
            BaseInfo = new GamePackageBaseInfo(nodeBaseInfo);

            //Compatibility
            XmlNode nodeCompatibility = nodePackage.SelectSingleNode("Compatibility");
            if (nodeCompatibility != null)
                for (int i = 0; i < nodeCompatibility.Attributes.Count; i++)
                {
                    switch (nodeCompatibility.ChildNodes[i].Name)
                    {
                        case "TargetVersion":
                            TargetVersion = ConverUtils.StringToInt(nodeCompatibility.ChildNodes[i].InnerText,
                                GameConst.GameBulidVersion, "Compatibility/TargetVersion");
                            break;
                        case "MinVersion":
                            MinVersion = ConverUtils.StringToInt(nodeCompatibility.ChildNodes[i].InnerText,
                                GameConst.GameBulidVersion, "Compatibility/MinVersion");
                            break;
                    }
                }

            return true;
        }

        /// <summary>
        /// 模块包名
        /// </summary>
        public string PackageName { get; private set; }
        /// <summary>
        /// 模块版本号
        /// </summary>
        public int PackageVersion { get; private set; }
        /// <summary>
        /// 基础信息
        /// </summary>
        public GamePackageBaseInfo BaseInfo { get; private set; }

        /// <summary>
        /// 表示模块目标游戏内核版本
        /// </summary>
        public int TargetVersion { get; private set; } = GameConst.GameBulidVersion;
        /// <summary>
        /// 表示模块可以正常使用的最低游戏内核版本
        /// </summary>
        public int MinVersion { get; private set; } = GameConst.GameBulidVersion;

        /// <summary>
        /// 入口代码
        /// </summary>
        public string EntryCode { get; private set; }
        /// <summary>
        /// 模块类型
        /// </summary>
        public GamePackageType Type { get; private set; }
        /// <summary>
        /// 代码类型
        /// </summary>
        public GamePackageCodeType CodeType { get; private set; }
        /// <summary>
        /// 共享Lua虚拟机
        /// </summary>
        public string ShareLuaState { get; private set; }
    }
    /// <summary>
    /// 模块代码类型
    /// </summary>
    [SLua.CustomLuaClass]
    public enum GamePackageCodeType
    {
        /// <summary>
        /// Lua
        /// </summary>
        Lua,
        /// <summary>
        /// C# DLL
        /// </summary>
        CSharp
    }
    /// <summary>
    /// 模块类型
    /// </summary>
    [SLua.CustomLuaClass]
    public enum GamePackageType
    {
        /// <summary>
        /// 代码包
        /// </summary>
        Module,
        /// <summary>
        /// 资源包
        /// </summary>
        Asset
    }
    /// <summary>
    /// 模块基础信息
    /// </summary>
    [SLua.CustomLuaClass]
    public class GamePackageBaseInfo
    {
        [SLua.DoNotToLua]
        public GamePackageBaseInfo(XmlNode xmlNodeBaseInfo)
        {
            for (int i = 0; i < xmlNodeBaseInfo.Attributes.Count; i++)
            {
                switch (xmlNodeBaseInfo.ChildNodes[i].Name)
                {
                    case "Name": Name = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "Author": Author = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "Introduction": Introduction = xmlNodeBaseInfo.ChildNodes[i].InnerXml; break;
                    case "Logo": Logo = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "Link": Link = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "VersionName": VersionName = xmlNodeBaseInfo.ChildNodes[i].InnerText; break;
                    case "Dependencies":
                        for (int j = 0, jc = xmlNodeBaseInfo.ChildNodes[i].ChildNodes.Count; j < jc; j++)
                        {
                            Dependencies.Add(new GamePackageDependencies(
                                xmlNodeBaseInfo.ChildNodes[i].ChildNodes[j]));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 模块作者
        /// </summary>
        public string Author { get; private set; }
        /// <summary>
        /// 模块介绍
        /// </summary>
        public string Introduction { get; private set; }
        /// <summary>
        /// 模块Logo
        /// </summary>
        public string Logo { get; private set; }
        /// <summary>
        /// 模块链接
        /// </summary>
        public string Link { get; private set; }
        /// <summary>
        /// 显示给用户看的版本
        /// </summary>
        public string VersionName { get; private set; }
        /// <summary>
        /// 模块依赖
        /// </summary>
        public List<GamePackageDependencies> Dependencies { get; } = new List<GamePackageDependencies>();
    }
    /// <summary>
    /// 模块依赖信息
    /// </summary>
    [SLua.CustomLuaClass]
    public class GamePackageDependencies
    {
        [SLua.DoNotToLua]
        public GamePackageDependencies(XmlNode xmlNode)
        {
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

        public string Name { get; private set; }
        public int MinVersion { get; private set; }
        public bool MustLoad { get; private set; }

    }
}
