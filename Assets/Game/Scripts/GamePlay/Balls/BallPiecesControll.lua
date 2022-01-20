local CommonUtils = Ballance2.Utils.CommonUtils
local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils
local FadeManager = Game.UIManager.UIFadeManager
local Vector3 = UnityEngine.Vector3

---球碎片回收器
---@class BallPiecesControll : GameLuaObjectHostClass
BallPiecesControll = {
  _CamMgr = nil, ---@type CamManager
  _Rigidbody = nil, ---@type PhysicsObject,
  _Force = 0,
  _UpForce = 0,
  _DownForce = 0,
  _TimerIds = {}
} 

---球碎片数据
---@class BallPiecesData
---@field bodys PhysicsObject[]
---@field parent GameObject
---@field throwed boolean
BallPiecesData = {}

function CreateClass:BallPiecesControll()
  
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
  ---@param data BallPiecesData
  ---@param pos Vector3
  ---@param minForce number 推动最小力
  ---@param maxForce number 推动最大力
  ---@param timeLive number 碎片存活时间（30个tick为单位）
  function BallPiecesControll:ThrowPieces(data, pos, minForce, maxForce, timeLive)

    local parent = data.parent
    if not parent.activeSelf then
      parent:SetActive(true)

      --还原初始状态
      ObjectStateBackupUtils.RestoreObjectAndChilds(parent)
      --设置位置
      data.parent.transform.position = pos
      data.throwed = true

      for _, body in ipairs(data.bodys) do
        local forceDir = body.transform.localPosition
        body.gameObject:SetActive(true)
        forceDir.y = forceDir.y + 2
        forceDir:Normalize() --力的方向是从原点向碎片位置
        body:Physicalize() --物理
        body:Impluse(forceDir * CommonUtils.RandomFloat(minForce, maxForce)) --施加力
      end

      ---延时消失
      local iid = parent:GetInstanceID()
      self._TimerIds[iid] = LuaTimer.Add((timeLive or 20) * 2000, function ()
        self._TimerIds[iid] = nil
        self:ResetPieces(data)
      end)

    end
  end
  ---回收碎片
  ---@param data BallPiecesData
  function BallPiecesControll:ResetPieces(data)

    local parent = data.parent
    if parent.activeSelf then
      
      local iid = parent:GetInstanceID()
      local id = self._TimerIds[iid]
      if id ~= nil then
        self._TimerIds[iid] = nil
        LuaTimer.Delete(id)
      end

      --渐变淡出隐藏其材质
      for i = 0, parent.transform.childCount - 1 do
        FadeManager:AddFadeOut(parent.transform:GetChild(i).gameObject, 3, true, nil)
      end

      data.throwed = false

      --延时
      LuaTimer.Add(2990, function ()
        
        --去除物理
        for _, body in ipairs(data.bodys) do
          body:UnPhysicalize(true) 
        end

        parent:SetActive(false) --隐藏
      end)
    end
  end

  return BallPiecesControll:new(nil)
end