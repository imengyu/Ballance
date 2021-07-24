using Ballance2.Config;
using Ballance2.Sys.Res;
using Ballance2.Sys.Services;
using SLua;
using UnityEngine;
using System.IO;
using Ballance2.Utils;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystemPackage.cs
* 
* 用途：
* 游戏主模块结构的声明
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-17 创建
* 2021-4-17 Add BaseInfo for GameSystemPackage
*
*/

namespace Ballance2.Sys.Package
{
    class GameSystemPackage : GamePackage
    {
        public GameSystemPackage()
        {
            PackageName = GamePackageManager.SYSTEM_PACKAGE_NAME;
            PackageFilePath = Application.dataPath;
            UpdateTime = System.DateTime.Now;
            IsCompatible = true;
            Type = GamePackageType.Module;
            BaseInfo = new GamePackageBaseInfo(true);
            baseInited = true;
        }

        public override LuaState PackageLuaState
        {
            get
            {
                if (GameManager.Instance != null)
                    return GameManager.Instance.GameMainLuaState;
                return base.PackageLuaState;
            }
        }

        public override T GetAsset<T>(string pathorname)
        {
#if UNITY_EDITOR
            if(pathorname.StartsWith("Assets"))
                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(pathorname);
            
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(ConstStrings.EDITOR_LOAD_PREFAB_PATH + pathorname);
#else
            if(AssetBundle) 
                return AssetBundle.LoadAsset<T>(pathorname.StartsWith("Assets") ? pathorname : (ConstStrings.EDITOR_LOAD_PREFAB_PATH + pathorname));
            else
                return Resources.Load<T>(pathorname);
#endif
        }
        public override string GetCodeLuaAsset(string pathorname)
        {
#if UNITY_EDITOR
            if(GamePathManager.IsAbsolutePath(pathorname) || pathorname.StartsWith("Assets")) 
                return new StreamReader(pathorname, System.Text.Encoding.UTF8).ReadToEnd();
            var scriptPath = ConstStrings.EDITOR_LOAD_PREFAB_PATH + pathorname;
            if(File.Exists(scriptPath)) 
                return new StreamReader(scriptPath, System.Text.Encoding.UTF8).ReadToEnd();
            
            Log.E(TAG, "Not found script {0}", pathorname);
            return null;
#else
            return base.GetCodeLuaAsset(pathorname);
#endif
        }

        public void SetCoreAssetbundle(AssetBundle asset) {
            AssetBundle = asset;
        }
    }
}
