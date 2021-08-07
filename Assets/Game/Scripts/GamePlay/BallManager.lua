---@type GameLuaObjectHostClass
---@class BallManager
BallManager = {
  
}

function CreateClass_BallManager()
  function BallManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function BallManager:Start(thisGameObject)

  end
  function BallManager:OnDestroy()

  end

  return BallManager:new(nil)
end