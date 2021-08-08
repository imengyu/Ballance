--- Lua 模组示例入口代码

-- 这里来导入C#类定义（一个模块定义一次即可）
local GameManager = Ballance2.Sys.GameManager
local Log = Ballance2.Utils.Log

--[[
  使用slua的 
  import Ballance2
  import UnityEngine
  可以快速全部导入C#定义，不过推荐要用到哪些类就声明哪些类。
  要知道类的命名空间，你可以在Vs中打开“视图”>“类视图”看完整的命名空间。
  提示：只有标记了 [CustomLuaClass] C#的类才可以在Lua访问，未标记导出的不能强行访问，会报错。
  另外，如果你的模组要在低版本游戏上运行，你也必须知道那些版本有没有对应函数导出，要做兼容。。
]]

MyPackage = nil -- 这是你的模组实例，可以通过它来操作你的模组
local TAG = "MyMod:Main" -- 

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    MyPackage = thisGamePackage
    Log.D(TAG, "这是模块入口 !")

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
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)
    -- 在这里可以做一些资源释放的操作

    Log.D(TAG, "模块将被卸载 !")
    return true
  end
}
