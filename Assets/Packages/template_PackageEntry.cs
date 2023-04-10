using Ballance2.Package;
using Ballance2;

namespace ModNamespace {

  /// <summary>
  /// 模组示例入口代码
  /// </summary>
  public class PackageEntry {
    const string TAG = "MyMod";

    /// <summary>
    /// 这是模组的主入口，你必须在这里返回模组的基础信息
    /// </summary>
    /// <returns></returns>
    public static GamePackageEntry Main() {
      GamePackageEntry entry = new GamePackageEntry();
      entry.OnLoad = (package) => {
        Log.D(TAG, "这是模块入口 !");

        //在这里添加你的模块初始化代码...
        //
        //提示：你可以查看WIKI中的开发帮助 “模组开发” 这个文档合集 来了解如何写你的模组。
        //
        //返回true表示模组初始化成功，否则失败
        return true;
      };
      entry.OnBeforeUnLoad = (package) => {
        //在这里可以做一些资源释放的操作
        Log.D(TAG, "模块卸载前调用此函数 !");
        return true;
      };
      entry.Version = 1; //返回版本号
      return entry;
    }
  }
}