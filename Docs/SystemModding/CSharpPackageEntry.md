## C# 模块入口

- C# 模块是为了满足高级 Mod 需求才支持的，可以使用反射等高自定义需求，但缺点也很明显，无法在 AOT 平台上使用，因此，推荐使用 Lua 模块。

在程序集中添加一个 Package 类，访问性为 public，具体功能如下：

```C#
/// <summary>
/// C#模块的入口
/// </summary>
public class Package
{
    /// <summary>
    /// 模块在被加载时调用
    /// </summary>
    /// <param name="package">当前模块数据实例</param>
    public void PackageEntry(GamePackage package)
    {
        //在这里初始化模块
    }
    /// <summary>
    /// 模块将被卸载时调用
    /// </summary>
    public void PackageBeforeUnLoad()
    {
        //在这里卸载资源
    }
}
```

请将此 dll 命名为 程序名称.dll.bytes, 并在 PackageDef.xml 中定义 EntryCode 为 **“程序名称.dll.bytes”**，请一并打包进入 AssetBundle，包管理器会自动加载你的程序集并运行。
