## Lua 模块

Lua 模块提供完整的 Unity 导出、游戏 API 导出，无需动态 JIT。框架对其提供了高度的支持，包括“插槽式可插拔设计”、“动态热更新”等，推荐使用 Lua 模块作为模组开发（事实上 Ballance2 游戏内核大部分都是基于 Lua 的）。

### 模组模块中使用 Lua

- 只需要在模块中新建代码文件 xxx.lua.txt 并一并打包入 AssetBundle 即可在模块中导入 Lua.
- Lua 入口代码声明：在 PackageDef.xml 中定义 EntryCode 为 lua 入口代码文件的路径（相对路径，例如：xxx.lua.txt 或 dir/xxx.lua.txt），lua 入口格式如下所示，您可以在入口回调函数中导入其他 lua 文件，执行您的自定义操作.

### Lua 模组示例入口代码

```lua
-- 这里来导入C#类定义（定义一次即可）
GameManager = Ballance2.System.GameManager
Log = Ballance2.Utils.Log

--[[
  使用slua的
  import Ballance2
  import UnityEngine
  可以快速全部导入C#定义，但效率不高，推荐要用到哪些类就声明哪些类。
  要知道类的命名空间，你可以在Vs中打开“视图”>“类视图”看完整的命名空间。
  提示：只有标记了 [CustomLuaClass] C#的类才可以在Lua访问，未标记导出的不能强行访问，会报错。
  另外，如果你的模组要在低版本游戏上运行，你也必须知道那些版本有没有对应函数导出，要做兼容。。
]]

---这是你的模组实例，可以通过它来操作你的模组
---@type GamePackage
GamePackage = nil 
local TAG = "MyMod:Main" --

---模块入口函数
---@param thisGamePackage GamePackage 当前模块包实例
---@return boolean 返回初始化是否成功
function PackageEntry(thisGamePackage)
  GamePackage = thisGamePackage

  Log:D(TAG, "这是模块入口 !")

  --[[
    ...在这里添加你的模块初始化代码...

    提示：你可以查看开发帮助 “模组开发” 这个文档合集 来了解如何写你的模组。

    提示：使用
    GamePackage:RequireLuaFile("xxx.lua.txt")
    可以引入其他Lua文件，然后就可以调用其他lua文件的函数。

    提示：使用
    local classCreate = GamePackage:RequireLuaClass("className")
    local myClassInstance = classCreate()
    可以快速引入一个Lua类，详情请参考开发帮助 “模组开发”>“Lua”>“类” 来了解更多。
  ]]

  return true
end

---模块卸载前函数
---@param thisGamePackage GamePackage 当前模块包实例
---@return boolean 返回卸载是否成功
function PackageBeforeUnLoad(thisGamePackage)
  GamePackage = thisGamePackage

  -- 在这里可以做一些资源释放的操作

  Log:D(TAG, "模块将被卸载 !")
  return true
end
```
