local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3

---球碎片回收器
---@class BallPaperPiecesWind : GameLuaObjectHostClass
BallPaperPiecesWind = {
  _PushForce = Vector3(-0.09, 0, 0.09),
} 

function CreateClass_BallPaperPiecesWind()
  
  function BallPaperPiecesWind:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function BallPaperPiecesWind:FixedUpdate()
    for i = 0, self.transform.childCount - 1 do
      local body = self.transform:GetChild(i).gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
      body:ApplyLinearImpulse(self._PushForce) --施加力
    end
  end

  return BallPaperPiecesWind:new(nil)
end
