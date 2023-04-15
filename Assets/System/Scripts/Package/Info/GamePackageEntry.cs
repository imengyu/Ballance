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
    /// <summary>
    /// UI加载回调。此回调在UI控制器初始化完成后回调，你可以在这里注册自定义UI界面
    /// </summary>
    public GamePackageEntryDelogate OnLoadUI;
    /// <summary>
    /// 模块版本参数
    /// </summary>
    public int Version = 0;
  }
  
  /// <summary>
  /// 模块入口回调
  /// </summary>
  /// <returns></returns>
  
  public delegate bool GamePackageEntryDelogate(GamePackage package);
}
