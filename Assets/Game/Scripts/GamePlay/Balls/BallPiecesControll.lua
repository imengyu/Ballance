local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3
local GameManager = Ballance2.Sys.GameManager
local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils
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
  function BallPiecesControll:FixedUpdate()
    --超时恢复碎片
    for _, v in ipairs(self._PieceThrown) do
      v.TimeLive = v.TimeLive - 1
      if v.TimeLive <= 0 then
        self:ResetPieces(v.GameObject)
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

      --还原初始状态
      ObjectStateBackupUtils.RestoreObjectAndChilds(parent)
      --设置位置
      parent.transform.position = pos

      --添加数据，让碎片自动消失
      table.insert(self._PieceThrown, {
        GameObject = parent,
        TimeLive = timeLive or 20,
      })

      for i = 0, parent.transform.childCount - 1 do
        local child = parent.transform:GetChild(i)
        local body = child.gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
        local forceDir = child.position
        child.gameObject:SetActive(true)
        forceDir:Normalize() --力的方向是从原点向碎片位置
        body:ForcePhysics() --物理
        body:ApplyPointImpulse(forceDir * math.random(minForce, maxForce), Vector3.up) --施加力
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

      --去除物理
      for i = 0, parent.transform.childCount - 1 do
        local child = parent.transform:GetChild(i)
        local body = child.gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
        body:ForceDePhysics() 
      end

      --渐变淡出隐藏其材质
      for i = 0, parent.transform.childCount - 1 do
        GameUIManager.UIFadeManager:AddFadeOut(parent.transform:GetChild(i).gameObject, 2, true, nil)
      end

      --延时
      coroutine.resume(coroutine.create(function()
        Yield(WaitForSeconds(2))
        parent:SetActive(false) --隐藏
      end))

    end
  end

  return BallPiecesControll:new(nil)
end
