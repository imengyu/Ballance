local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3
local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local FadeManager = Game.UIManager.UIFadeManager

---球碎片回收器
---@class BallPiecesControll : GameLuaObjectHostClass
BallPiecesControll = {
  _CamMgr = nil, ---@type CamManager
  _Rigidbody = nil, ---@type PhysicsBody,
  _Force = 0,
  _UpForce = 0,
  _DownForce = 0,
  _TimerIds = {}
} 

function CreateClass_BallPiecesControll()
  
  function BallPiecesControll:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function BallPiecesControll:Start()
    GamePlay.BallPiecesControll = self
  end
    
  ---抛出碎片
  ---@param parent GameObject 父级
  ---@param pos Vector3
  ---@param minForce number 推动最小力
  ---@param maxForce number 推动最大力
  ---@param timeLive number 碎片存活时间（30个tick为单位）
  function BallPiecesControll:ThrowPieces(parent, pos, minForce, maxForce, timeLive)
    if not parent.activeSelf then
      parent:SetActive(true)

      --还原初始状态
      ObjectStateBackupUtils.RestoreObjectAndChilds(parent)
      --设置位置
      parent.transform.position = pos

      for i = 0, parent.transform.childCount - 1 do
        local child = parent.transform:GetChild(i)
        local body = child.gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
        local forceDir = child.localPosition
        child.gameObject:SetActive(true)
        forceDir:Normalize() --力的方向是从原点向碎片位置
        body:ForcePhysics() --物理
        body:ApplyPointImpulse(forceDir * math.random(minForce, maxForce), Vector3.up) --施加力
      end

      ---延时消失
      local iid = parent:GetInstanceID()
      self._TimerIds[iid] = LuaTimer.Add((timeLive or 20) * 2000, function ()
        self._TimerIds[iid] = nil
        self:ResetPieces(parent)
      end)

    end
  end
  ---回收碎片
  ---@param parent GameObject 父级
  function BallPiecesControll:ResetPieces(parent)
    if parent.activeSelf then
      
      local iid = parent:GetInstanceID()
      local id = self._TimerIds[iid]
      if id ~= nil then
        self._TimerIds[iid] = nil
        LuaTimer.Delete(id)
      end

      --去除物理
      for i = 0, parent.transform.childCount - 1 do
        local child = parent.transform:GetChild(i)
        local body = child.gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
        body:ForceDePhysics() 
      end

      --渐变淡出隐藏其材质
      for i = 0, parent.transform.childCount - 1 do
        FadeManager:AddFadeOut(parent.transform:GetChild(i).gameObject, 3, true, nil)
      end

      --延时
      LuaTimer.Add(3000, function ()
        parent:SetActive(false) --隐藏
      end)
    end
  end

  return BallPiecesControll:new(nil)
end
