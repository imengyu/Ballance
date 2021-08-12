local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3
local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

---球碎片回收器
---@class BallPiecesControll : GameLuaObjectHostClass
BallPiecesControll = {
  _CamMgr = nil, ---@type CamManager
  _Rigidbody = nil, ---@type PhysicsBody,
  _Force = 0,
  _UpForce = 0,
  _DownForce = 0,
  
  _TickTime = 0,
  _PieceThrown = {}, ---@type BallPiecesTimeStorage[]
} 

---@class BallPiecesTimeStorage
BallPiecesTimeStorage = {
  GameObject = nil, ---@type GameObject
  TimeLive = 0,
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
  function BallPiecesControll:Update()
    if self._TickTime < 65536 then self._TickTime = self._TickTime + 1 else self._TickTime = 0 end
    if self._TickTime % 30 == 0 then
      --超时恢复碎片
      for k, v in ipairs(self._PieceThrown) do
        v.TimeLive = v.TimeLive - 1
        if v.TimeLive <= 0 then
          self:ResetPieces(v.GameObject)
        end
      end
    end
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

      --添加数据，让碎片自动消失
      table.insert(self._PieceThrown, {
        GameObject = parent,
        TimeLive = timeLive,
      })

      for i = 0, parent.transform:GetChildCount() do
        local child = parent.transform:GetChild(i)
        local body = child.gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
        body:ForcePhysics() --物理
        body:ApplyPointImpulse(Vector3.up * math.random(minForce, maxForce), Vector3.up) --施加力
      end

    end
  end
  ---回收碎片
  ---@param parent GameObject 父级
  function BallPiecesControll:ResetPieces(parent)
    if parent.activeSelf then
      
      --移除数据
      for i = #self._PieceThrown, 0, -1 do
        if(self._PieceThrown[i].GameObject == parent) then
          table.remove(self._PieceThrown, i)
          break
        end
      end

      --隐藏其材质
      for i = 0, parent.transform:GetChildCount() do
        GameUIManager.UIFadeManager:AddFadeOut2(parent.transform:GetChild(i), 2, false, nil)
      end

      --延时
      coroutine.resume(coroutine.create(function()
        Yield(WaitForSeconds(2))

        --去除物理
        for i = 0, parent.transform:GetChildCount() do
          local child = parent.transform:GetChild(i)
          local body = child.gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
          body:ForceDeactive() 
        end

        parent:SetActive(false)
      end))

    end
  end

  return BallPiecesControll:new(nil)
end
