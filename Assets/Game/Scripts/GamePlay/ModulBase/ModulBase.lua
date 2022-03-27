---@gendoc

local TiggerTester = Ballance2.Game.TiggerTester
local SphereCollider = UnityEngine.SphereCollider

---机关定义
---您可以通过继承此类来定义您的机关。
---@class ModulBase : GameLuaObjectHostClass
---@field EnableBallRangeChecker boolean 是否开启球区域检测，开启后会定时检测，如果球进入指定范围，则发出 BallEnterRange 事件
---@field IsActive boolean 获取当前机关是否激活
---@field BallCheckeRange number 球区域检测范围。创建之后也可以手动设置 modul.BallRangeChecker.Diatance 属性来设置
ModulBase = ClassicObject:extend()

function ModulBase:new()
  ModulBase.super.new(self)
  self.BallRangeChecker = nil
  self.BallRangeCollider = nil
  self.BallInRange = false
  self.IsPreviewMode = false --指定当前机关是否在预览模式中加载
  self.AutoActiveBaseGameObject = true
end

---初始化
function ModulBase:Start()
  --机关内置的球区域检测功能初始化
  if self.EnableBallRangeChecker then
    self.BallRangeCollider = self.gameObject:AddComponent(SphereCollider) ---@type SphereCollider
    self.BallRangeCollider.radius = self.BallCheckeRange or 100
    self.BallRangeCollider.isTrigger = true
    self.BallRangeChecker = self.gameObject:AddComponent(TiggerTester) ---@type TiggerTester
    ---@param obj GameObject
    ---@param other GameObject
    self.BallRangeChecker.onTriggerEnter = function (obj, other)
      if not self.BallInRange and other.tag == 'Ball' then
        self.BallInRange = true
        self:BallEnterRange()
      end
    end
    ---@param obj GameObject
    ---@param other GameObject
    self.BallRangeChecker.onTriggerExit = function (obj, other)
      if self.BallInRange and other.tag == 'Ball' then
        self.BallInRange = false
        self:BallLeaveRange()
      end
    end
  end
end
---机关激活时发出此事件（进入当前小节）
---此事件在Backup事后发出
function ModulBase:Active()
  if self.AutoActiveBaseGameObject then
    self.gameObject:SetActive(true)
  end
  self.IsActive = true
end
---机关隐藏时发出此事件（当前小节结束）
function ModulBase:Deactive()
  self.IsActive = false
  if self.AutoActiveBaseGameObject then
    self.gameObject:SetActive(false)
  end
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

---球进入当前机关指定范围时发出此事件
function ModulBase:BallEnterRange()
end
---球离开当前机关指定范围时发出此事件
function ModulBase:BallLeaveRange()
end


---在预览模式中激活时发出此事件
function ModulBase:ActiveForPreview()
end
---在预览模式中隐藏时发出此事件
function ModulBase:DeactiveForPreview()
end

---调试环境的自定义调试操作回调
---@param index number 参数
function ModulBase:Custom(index)
end

function CreateClass:ModulBase()
  return ModulBase()
end