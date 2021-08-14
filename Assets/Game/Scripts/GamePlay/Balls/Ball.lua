local LuaUtils = Ballance2.Utils.LuaUtils
local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3

---纸球定义
---@class Ball : GameLuaObjectHostClass
---@field _CamMgr CamManager
---@field _Rigidbody PhysicsBody
---@field _Pieces GameObject
---@field _PaperPiecesSound AudioSource
---@field _PiecesMinForce number
---@field _PiecesMaxForce number
---@field _Force number
---@field _UpForce number
---@field _DownForce number
Ball = ClassicObject:extend()

function Ball:new()
  self._CamMgr = nil;
  self._Rigidbody = nil;
  self._Pieces = nil;
  self._PiecesMinForce = 0;
  self._PiecesMaxForce = 5;
  self._Force = 0;
  self._UpForce = 0;
  self._DownForce = 0;
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
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamLeftVector * self._Force)
  elseif LuaUtils.And(pushType, BallPushType.Right) == BallPushType.Right then
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamRightVector * self._Force)
  end
  if LuaUtils.And(pushType, BallPushType.Forward) == BallPushType.Forward then
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamForwerdVector * self._Force)
  elseif LuaUtils.And(pushType, BallPushType.Back) == BallPushType.Back then
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamBackVector * self._Force)
  end
  if LuaUtils.And(pushType, BallPushType.Up) == BallPushType.Up then
    self._Rigidbody:ApplyLinearImpulse(Vector3.up * self._UpForce)
  elseif LuaUtils.And(pushType, BallPushType.Down) == BallPushType.Down then
    self._Rigidbody:ApplyLinearImpulse(Vector3.down * self._DownForce)
  end
end

---激活时
function Ball:Active()
  
end
---取消激活时
function Ball:Deactive()
  
end
---获取碎片
function Ball:GetPieces()
  return self._Pieces
end
---丢出此作类的碎片时
function Ball:ThrowPieces()
  GamePlay.BallPiecesControll:ThrowPieces(self._Pieces, self.transform.position, self._PiecesMinForce, self._PiecesMaxForce)
end
---回收此作类的碎片时
function Ball:ResetPieces()
  GamePlay.BallPiecesControll:ResetPieces(self._Pieces)
end

function CreateClass_Ball()
  return Ball()
end