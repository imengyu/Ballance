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
      SetFlag(GetFlag() & (GamePackage.FLAG_PACK_NOT_UNLOADABLE | GamePackage.FLAG_PACK_SYSTEM_PACKAGE));
      PackageName = GamePackageManager.SYSTEM_PACKAGE_NAME;
    }

    public override bool CheckCodeAssetExists(string pathorname)
    {
      return Resources.Load<TextAsset>(pathorname) != null;
    }
    public override T GetAsset<T>(string pathorname)
    {
      return Resources.Load<T>(pathorname);
    }
    public override CodeAsset GetCodeAsset(string pathorname)
    {
      bool enableDebugger = Entry.GameEntry.Instance.DebugEnableV8Debugger;
      TextAsset asset = Resources.Load<TextAsset>(pathorname);
      if(asset == null)
        throw new FileNotFoundException("Filed to load code file: \"" + pathorname + "\" !");
      var realtivePath = PathUtils.ReplaceAbsolutePathToRelativePath(pathorname);
      return new CodeAsset(asset.bytes, pathorname, realtivePath,  enableDebugger ? MakeDebugJSPath(realtivePath) : ConstStrings.EDITOR_SYSTEMPACKAGE_LOAD_ENV_SCRIPT_PATH + pathorname);
    }
  }
}
