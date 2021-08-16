---机关定义
---@class ModulBase : GameLuaObjectHostClass
ModulBase = ClassicObject:extend()

function ModulBase:new()
end

---机关激活时发出此事件
---此事件在Backup事后发出
function ModulBase:Active()
  self.gameObject:SetActive(true)
end
---机关隐藏时发出此事件
function ModulBase:Deactive()
  self.gameObject:SetActive(false)
end
---机关重置为初始状态时发出此事件。Reset在Deactive之后发出
function ModulBase:Reset()
end
---机关就绪，可以保存状态时发出此事件
function ModulBase:Backup()
end

function CreateClass_ModulBase()
  return ModulBase()
end