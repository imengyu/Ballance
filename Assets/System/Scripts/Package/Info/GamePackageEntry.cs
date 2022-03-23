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
  /// 模块代码入口反馈体。
  /// 所有模块代码层的处理事件、回调都将在这里触发。
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiNoDoc]
  public class GamePackageEntry {

    //事件回调

    /// <summary>
    /// 包加载回调
    /// </summary>
    public GamePackageEntryDelogate OnLoad;
    /// <summary>
    /// 包卸载回调
    /// </summary>
    public GamePackageEntryDelogate OnBeforeUnLoad;

    //返回C#端的参数

    /// <summary>
    /// 模块版本参数
    /// </summary>
    public int Version = 0;

    //Internal
    

  }
  
  /// <summary>
  /// 模块入口回调
  /// </summary>
  /// <returns></returns>
  [SLua.CustomLuaClass]
  public delegate bool GamePackageEntryDelogate(GamePackage package);
}
