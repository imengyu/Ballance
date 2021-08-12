local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3

---球碎片回收器
---@class BallPaperPiecesWind : GameLuaObjectHostClass
BallPaperPiecesWind = {
  _TickTime = 0,
  _PushForce = Vector3(-0.03, 0, 0.03),
} 

function CreateClass_BallPaperPiecesWind()
  
  function BallPaperPiecesWind:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function BallPaperPiecesWind:FixUpdate()
    if self._TickTime < 65536 then self._TickTime = self._TickTime + 1 else self._TickTime = 0 end
    if self._TickTime % 30 == 0 then
      --每半秒
      for i = 0, self.transform:GetChildCount() do
        local body = self.transform:GetChild(i).gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
        body:ApplyForce(self._PushForce) --施加力
      end
    end
  end

  return BallPaperPiecesWind:new(nil)
end
