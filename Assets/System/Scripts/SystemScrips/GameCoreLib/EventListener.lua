local classic = require('classic')

---事件回调存储类
---@class EventListener
EventListener = classic:extend()

function EventListener:new()
  self.Listeners = {} ---@type function[]
end
---添加回调
---@param fun function
function EventListener:AddListener(fun)
  table.insert(self.Listeners, fun)
end
---移除所有回调
function EventListener:RemoveAllListeners()
  self.Listeners = {}
end
---移除指定回调
---@param fun function
function EventListener:DeleteListener(fun)
  for index, value in ipairs(self.Listeners) do
    if value == fun then
      table.remove(self.Listeners, index)
      return
    end
  end
end
---调用所有回调
---@param ... any 附加参数
function EventListener:CallListeners(...)
  for _, value in pairs(self.Listeners) do
    if value then
      pcall(value, ...)
      return
    end
  end
end

return EventListener