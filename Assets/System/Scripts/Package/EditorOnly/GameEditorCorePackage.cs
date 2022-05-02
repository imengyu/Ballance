using Ballance2.Config;
using UnityEngine;
using System.IO;
using Ballance2.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Ballance2.Services.Debug;
using Ballance2.Services;
using Ballance2.Res;

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
  class GameEditorCorePackage : GameEditorDebugPackage
  {
    public GameEditorCorePackage(string packageRealPath) {
      SetFlag(GetFlag() | (GamePackage.FLAG_PACK_NOT_UNLOADABLE | GamePackage.FLAG_PACK_SYSTEM_PACKAGE));
      PackageName = GamePackageManager.CORE_PACKAGE_NAME;
      PackageFilePath = packageRealPath;
    }

    protected override string DebugFolder => GamePathManager.DEBUG_CORE_FOLDER;

  }
}
