--- Lua 模组示例入口代码

local GameManager = Ballance2.Services.GameManager
local Log = Ballance2.Log

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    
    
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
