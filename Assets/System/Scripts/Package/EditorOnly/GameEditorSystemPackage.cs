using Ballance2.Services;
using Ballance2.Res;
using Ballance2.Game;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystemPackage.cs
* 
* 用途：
* 游戏主模块特殊结构的声明
*
* 作者：
* mengyu
*/

namespace Ballance2.Package
{
  class GameEditorSystemPackage : GameEditorDebugPackage
  {
    public GameEditorSystemPackage() {
      _Status = GamePackageStatus.LoadSuccess;
      Type = GamePackageType.Module;
      SetFlag(GetFlag() | (GamePackage.FLAG_PACK_NOT_UNLOADABLE | GamePackage.FLAG_PACK_SYSTEM_PACKAGE));
      PackageName = GamePackageManager.SYSTEM_PACKAGE_NAME;
      PackageEntry = GameCorePackageEntry.Main();
      PackageFilePath = GamePathManager.DEBUG_CORE_FOLDER;
    }

    protected override string DebugFolder => GamePathManager.DEBUG_CORE_FOLDER;
  }
}
