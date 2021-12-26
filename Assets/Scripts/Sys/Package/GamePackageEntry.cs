using Ballance2.Base;
using Ballance2.Base.Handler;
using Ballance2.Config;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GamePackage.cs
* 
* 用途：
* 游戏模块的入口声明。
*
* 作者：
* mengyu
*/

namespace Ballance2.Package
{
  /// <summary>
  /// 模块入口结构变量
  /// </summary>
  public class GamePackageEntry {
    public GamePackageEntryDelogate OnLoad;
    public GamePackageEntryDelogate OnBeforeUnLoad;
  }
  
  /// <summary>
  /// 模块入口回调
  /// </summary>
  /// <returns></returns>
  public delegate bool GamePackageEntryDelogate(GamePackage package);
}
