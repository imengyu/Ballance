---机关定义
---@class ModulBase : GameLuaObjectHostClass
ModulBase = ClassicObject:extend()

function ModulBase:new()
end

---机关激活时发出此事件（进入当前小节）
---此事件在Backup事后发出
function ModulBase:Active()
  self.gameObject:SetActive(true)
end
---机关隐藏时发出此事件（当前小节结束）
function ModulBase:Deactive()
  self.gameObject:SetActive(false)
end
---关卡卸载时发出此事件
function ModulBase:UnLoad()
end
---机关重置为初始状态时发出此事件（玩家失败，重新开始一节）。Reset在Deactive之后发出
---@param type "sectorRestart"|"levelRestart" 重置类型 sectorRestart 玩家失败，重新开始一节，levelRestart 关卡重新开始
function ModulBase:Reset(type)
end
---机关就绪，可以保存状态时发出此事件（初次加载完成）
function ModulBase:Backup()
end

function CreateClass_ModulBase()
  return ModulBase()
end