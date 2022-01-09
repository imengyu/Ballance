/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameCorePackage.cs
* 
* 用途：
* 游戏主模块特殊结构的声明
*
* 作者：
* mengyu
*/

using Ballance2.Services;

namespace Ballance2.Package
{
  class GameCorePackage : GameZipPackage
  {    
    public GameCorePackage() {
      SetFlag(GetFlag() & (GamePackage.FLAG_PACK_NOT_UNLOADABLE | GamePackage.FLAG_PACK_SYSTEM_PACKAGE));
      PackageName = GamePackageManager.CORE_PACKAGE_NAME;
    }
  }
}
