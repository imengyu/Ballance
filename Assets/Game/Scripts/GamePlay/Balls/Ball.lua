---球定义
---@class Ball : GameLuaObjectHostClass
Ball = {

} 

function CreateClass_Ball()
  
  function Ball:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---推动
  ---@param pushType number
  function Ball:Push(pushType)
    
  end

  ---激活时
  function Ball:Active()
    
  end
  ---取消激活时
  function Ball:Deactive()
    
  end

  return Ball:new(nil)
end