--- Lua 模组 示例 代码

--这个MOD非常简单，它示例了下面三种MOD模式：
--* 为游戏添加代码性的修改（以计时器为例子，制作一个计算并显示过关时间的MOD）。
--* 为Ballance游戏添加自定义机关（以弹射器为例子，制作一个可把球弹射出去的机关）。
--* 为Ballance游戏添加自定义球（以弹弹球例子，制作一个高弹性的弹球）。
--你可以通过这个简单的MOD来了解如何制作你自己的MOD。
--关于更多帮助，可以参考模组开发文档。

local GameManager = Ballance2.Services.GameManager
local Log = Ballance2.Log

--这里导入了 TimerMain 文件, 这是计时器的代码，然后在下方就可以调用相关函数
require('TimerMain')

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    InitTimerMain(thisGamePackage) ---初始化Timer
    
    return true
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)
    -- 在这里可以做一些资源释放的操作

    return true
  end,
  ---返回版本号
  PackageVersion = 1,
}
