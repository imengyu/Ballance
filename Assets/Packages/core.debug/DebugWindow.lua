---@type GameLuaObjectHostClass
DebugWindow = {
  var = nil,
  --自定义属性
}

function class_DebugWindow()
  function DebugWindow:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function DebugWindow:Start(thisGameObject)
    
  end
  function DebugWindow:OnDestroy()
    
  end

  return DebugWindow:new(nil)
end

