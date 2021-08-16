local CloneUtils = Ballance2.Sys.Utils.CloneUtils

---关卡建造器
---@class LevelBuilder : GameLuaObjectHostClass
LevelBuilder = ClassicObject:extend()

function CreateClass_LevelBuilder()
  return LevelBuilder()
end

function LevelBuilder:new()
  
end
function LevelBuilder:Start()
  
end

---加载关卡
---@param prefab GameObject 关卡的基础Prefab
function LevelBuilder:LoadLevel(prefab)
  
end
---卸载当前加载的关卡
function LevelBuilder:UnLoadLevel()
  
end

---注册机关
---@param name string 机关名称
---@param basePrefab GameObject 机关的基础Prefab
function LevelBuilder:RegisterModul(name, basePrefab)
  
end
---取消注册机关
---@param name string 机关名称
function LevelBuilder:UnRegisterModul(name)
  
end

