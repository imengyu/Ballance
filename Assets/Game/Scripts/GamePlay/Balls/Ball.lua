local LuaUtils = Ballance2.Utils.LuaUtils
local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3

---球定义
---@class Ball : GameLuaObjectHostClass
Ball = {
  _CamMgr = nil, ---@type CamManager
  _Rigidbody = nil, ---@type PhysicsBody,
  _Pieces = nil, ---@type GameObject,
  _PiecesMinForce = 0,
  _PiecesMaxForce = 5,
  _Force = 0,
  _UpForce = 0,
  _DownForce = 0,
} 

function CreateClass_Ball()
  
  function Ball:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function Ball:Start()
    self._CamMgr = GamePlay.CamManager
    self._Rigidbody = self.gameObject:GetComponent(PhysicsBody)
  end

  ---推动
  ---@param pushType number
  function Ball:Push(pushType)
    if self._CamMgr == nil then
      self._CamMgr = GamePlay.CamManager
    end
    if LuaUtils.And(pushType, BallPushType.Left) == BallPushType.Left then
      self._Rigidbody:ApplyForce(self._CamMgr.CamLeftVector * self._Force)
    elseif LuaUtils.And(pushType, BallPushType.Right) == BallPushType.Right then
      self._Rigidbody:ApplyForce(self._CamMgr.CamRightVector * self._Force)
    end
    if LuaUtils.And(pushType, BallPushType.Forward) == BallPushType.Forward then
      self._Rigidbody:ApplyForce(self._CamMgr.CamForwerdVector * self._Force)
    elseif LuaUtils.And(pushType, BallPushType.Back) == BallPushType.Back then
      self._Rigidbody:ApplyForce(self._CamMgr.CamBackVector * self._Force)
    end
    if LuaUtils.And(pushType, BallPushType.Up) == BallPushType.Up then
      self._Rigidbody:ApplyForce(Vector3.up * self._UpForce)
    elseif LuaUtils.And(pushType, BallPushType.Down) == BallPushType.Down then
      self._Rigidbody:ApplyForce(Vector3.down * self._DownForce)
    end
  end

  ---激活时
  function Ball:Active()
    
  end
  ---取消激活时
  function Ball:Deactive()
    
  end
  ---丢出此作类的碎片时
  function Ball:ThrowPieces()
    GamePlay.BallPiecesControll:ThrowPieces(self._Pieces, self.transform.position, self._PiecesMinForce, self._PiecesMaxForce)
  end
  ---回收此作类的碎片时
  function Ball:ResetPieces()
    GamePlay.BallPiecesControll:ResetPieces(self._Pieces)
  end

  return Ball:new(nil)
end