---@type GameLuaObjectHostClass
---@class CamManager
CamManager = {
  
}

function CreateClass_CamManager()
  function CamManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function CamManager:Start(thisGameObject)

  end
  function CamManager:OnDestroy()

  end

  return CamManager:new(nil)
end