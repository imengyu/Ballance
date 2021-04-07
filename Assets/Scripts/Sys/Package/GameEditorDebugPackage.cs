﻿using Ballance2.Sys.Debug;
using Ballance2.Sys.Res;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
*
* 更改历史：
* 2021-1-21 创建
*
*/

namespace Ballance2.Sys.Package
{
    public class GameEditorDebugPackage : GamePackage
    {
        private readonly string TAG = "GameEditorDebugPackage";
        
        public override void Destroy()
        {
            
        }
        public override async Task<bool> LoadInfo(string filePath)
        {
            PackageFilePath = filePath;

#if !UNITY_EDITOR
            GameErrorChecker.SetLastErrorAndLog(GameError.OnlyCanUseInEditor, TAG, "Package can only use in editor.");
            return false;
#endif

            string defPath = filePath + "/PackageDef.xml";
            if (!File.Exists(defPath))
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.PackageDefNotFound, TAG, "PackageDef.xml not found");
                LoadError = "模块并不包含 PackageDef.xml";
                return false;
            } 
            else
            {
                PackageDef = new XmlDocument();
                PackageDef.Load(defPath);

                return ReadInfo(PackageDef);
            }
        }
        public override async Task<bool> LoadPackage()
        {
            if(!string.IsNullOrEmpty(BaseInfo.Logo))
                LoadLogo(PackageFilePath + "/" + BaseInfo.Logo);
            return await base.LoadPackage();
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

        public override string GetCodeLuaAsset(string pathorname)
        {
            if (GamePathManager.IsAbsolutePath(pathorname) || pathorname.StartsWith("Assets/"))
            {
                if (File.Exists(pathorname))
                    return File.ReadAllText(pathorname);
            }
            else
            {
                string path = PackageFilePath + "/" + pathorname;
                if (File.Exists(path))
                    return File.ReadAllText(path);
            }

            GameErrorChecker.LastError = GameError.FileNotFound;
            return null;
        }
        public override Assembly LoadCodeCSharp(string pathorname)
        {
            if (GamePathManager.IsAbsolutePath(pathorname) || pathorname.StartsWith("Assets/"))
            {
                if (File.Exists(pathorname)) 
                    return Assembly.LoadFile(pathorname);
            }
            else
            {
                string path = PackageFilePath + "/" + pathorname;
                if (File.Exists(path))
                    return Assembly.LoadFile(path);
            }
            return base.LoadCodeCSharp(pathorname);
        }

        public override T GetAsset<T>(string pathorname)
        {
#if UNITY_EDITOR
            if(pathorname.StartsWith("Assets/"))
                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(pathorname);
            else return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(
                GamePathManager.DEBUG_PACKAGE_FOLDER + "/" + PackageName + "/" + pathorname);
#else
            GameErrorChecker.SetLastErrorAndLog(GameError.OnlyCanUseInEditor, TAG, "Package can only use in editor.");
            return false;
#endif
        }


    }
}
