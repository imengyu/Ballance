/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystemPackage.cs
* 
* 用途：
* 游戏核心代码模块特殊结构的声明
*
* 作者：
* mengyu
*/

using System.IO;
using Ballance2.Config;
using Ballance2.Services;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.Package
{
  class GameSystemPackage : GamePackage
  {
    public GameSystemPackage() {
      CodeType = GamePackageCodeType.Lua;
      PackageName = GamePackageManager.SYSTEM_PACKAGE_NAME;
      _Status = GamePackageStatus.LoadSuccess;
      SetFlag(GetFlag() & (GamePackage.FLAG_PACK_NOT_UNLOADABLE | GamePackage.FLAG_PACK_SYSTEM_PACKAGE));
    }

    public override T GetAsset<T>(string pathorname)
    {
      return Resources.Load<T>(pathorname);
    }
    public override CodeAsset GetCodeAsset(string pathorname)
    {
#if UNITY_EDITOR
      if(PathUtils.IsAbsolutePath(pathorname) && File.Exists(pathorname)) {
        var bytes = FileUtils.ReadAllToBytes(pathorname);
        var realtivePath = PathUtils.ReplaceAbsolutePathToRelativePath(pathorname);
        return new CodeAsset(bytes, pathorname, realtivePath, ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ENV_SCRIPT_PATH + pathorname);
      } else {
        var path = ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ENV_SCRIPT_PATH + pathorname;
        if(File.Exists(path)) {
          var bytes = FileUtils.ReadAllToBytes(path);
          var realtivePath = PathUtils.ReplaceAbsolutePathToRelativePath(path);
          return new CodeAsset(bytes, path, realtivePath, path);
        }
        return null;
      }
#else
      TextAsset asset = Resources.Load<TextAsset>(pathorname);
      if(asset == null)
        throw new FileNotFoundException("Filed to load code file: \"" + pathorname + "\" !");
      var realtivePath = PathUtils.ReplaceAbsolutePathToRelativePath(pathorname);
      return new CodeAsset(asset.bytes, pathorname, realtivePath, ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ENV_SCRIPT_PATH + pathorname);
#endif
    }
  }
}
