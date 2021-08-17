local KeyCode = UnityEngine.KeyCode
local Log = Ballance2.Utils.Log
local KeyListener = Ballance2.Sys.Utils.KeyListener
local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils
local Vector3 = UnityEngine.Vector3
local GameSettingsManager = Ballance2.Config.GameSettingsManager
local GameErrorChecker = Ballance2.Sys.Debug.GameErrorChecker
local GameManager = Ballance2.Sys.GameManager
local GameError = Ballance2.Sys.Debug.GameError
local LuaUtils = Ballance2.Utils.LuaUtils
local MotionType = PhysicsRT.MotionType
local GUI = UnityEngine.GUI
local Rect = UnityEngine.Rect

---球推动定义
---@class BallPushType
BallPushType = {
  None = 0,
  Forward = 0x2,-- 前
  Back = 0x4,-- 后
  Left = 0x8,-- 左
  Right = 0x10,-- 右
  Up = 0x20, -- 上
  Down = 0x40, -- 下
  ForwardLeft = 0xA, --Forward | Left,
  ForwardRight = 0x12, --Forward | Right,
  BackLeft = 0xC, --Back | Left,
  BackRight = 0x14,-- Back | Right,
}

---指定球的控制状态
---@class BallControlStatus
BallControlStatus = {
  ---没有控制（无控制、无物理效果，无摄像机跟随）
  NoControl = 0,
  ---正常控制（可控制、物理效果，摄像机跟随）
  Control = 1,
  ---释放模式（例如球坠落，球仍然有物理效果，但无法控制，摄像机不跟随）
  UnleashingMode = 2,
  ---锁定模式（例如变换球时，无物理效果，无法控制，但摄像机跟随）
  LockMode = 3,
}

---@class BallRegStorage
local BallRegStorage = {
  name = '',
  ball = nil, ---@type Ball
  rigidbody = nil, ---@type PhysicsBody
}

---球管理器
---@class BallManager : GameLuaObjectHostClass
---@field _BallLightningSphere GameLuaObjectHost
---@field _BallWood GameObject
---@field _BallStone GameObject
---@field _BallPaper GameObject
---@field _BallSmoke GameObject
---@field CurrentBall Ball 获取当前的球 [R]
---@field PushType number 获取或者设置当前球的推动方向 [RW]
---@field CanControll boolean 获取当前用户是否可以控制球 [R]
---@field CanControllCamera boolean 获取当前用户是否可以控制摄像机 [R]
---@field ShiftPressed boolean 获取当前用户是否按下Shift键 [R]
BallManager = ClassicObject:extend()

local TAG = 'BallManager'

function BallManager:new()
  self.PushType = 0;
  self.CurrentBall = nil;
  self.CanControll = false;
  self.CanControllCamera = false;
  self.ShiftPressed = false;
  self._DebugMode = false
  self._private = {
    BallLightningSphere = nil, ---@type BallLightningSphere
    GameSettings = nil, ---@type GameSettingsActuator

    settingsCallbackId = 0,
    controllingStatus = BallControlStatus.NoControl, ---当前状态
    lastSaveLinearVelocity = nil, ---@type Vector3
    lastSaveAngularVelocity = nil, ---@type Vector3
    reverseControl = false, ---是否反向控制
    ---已注册的球
    registerBalls = {}, ---@type BallRegStorage[] 
    ---当前激活的球
    currentBall = nil, ---@type BallRegStorage 
    currentActiveBall = nil, ---@type BallRegStorage 
    keyListener = nil, ---@type KeyListener
    ---控制按键设置
    keySets = {
      keyFront = KeyCode.UpArrow,
      keyBack = KeyCode.DownArrow,
      keyLeft = KeyCode.LeftArrow,
      keyRight = KeyCode.RightArrow,
      keyUpCamera = KeyCode.Space,
      keyRoateCamera = KeyCode.LeftShift,
      keyFront2 = KeyCode.W,
      keyBack2 = KeyCode.S,
      keyLeft2 = KeyCode.A,
      keyRight2 = KeyCode.D,
      keyRoateCamera2 = KeyCode.RightShift,
      keyUp = KeyCode.Q,
      keyDown = KeyCode.E,
    }, 
    nextRecoverPos = Vector3.zero, ---@type Vector3
    rect = Rect(20,100,200,20),
  }

  GamePlay.BallManager = self
end

function BallManager:Start()
  self._private.BallLightningSphere = self._BallLightningSphere:GetLuaClass()

  self:RegisterBall('BallWood', self._BallWood)
  self:RegisterBall('BallStone', self._BallStone)
  self:RegisterBall('BallPaper', self._BallPaper)

  --初始化控制设置
  local GameSettings = GameSettingsManager.GetSettings("core")
  self._private.settingsCallbackId = GameSettings:RegisterSettingsUpdateCallback("control", function () 
    self:_OnControlSettingsChanged() 
    return false
  end)
  self._private.GameSettings = GameSettings
  self._DebugMode = GameManager.DebugMode

  --初始化键盘侦听
  self._private.keyListener = KeyListener.Get(self.gameObject)
  --请求触发设置更新函数
  GameSettings:RequireSettingsLoad("control")
end
function BallManager:OnDestroy()
  self._private.GameSettings:UnRegisterSettingsUpdateCallback(self._private.settingsCallbackId)
  self._private.keyListener:ClearKeyListen()
end
function BallManager:FixedUpdate()
  if self.CanControll then
    self.CurrentBall:Push(self.PushType)
  end
end
--[[
function BallManager:OnGUI()
  if(self._DebugMode) then
    local rect = self._private.rect
    local ball = self._private.currentBall rect.y = 100
    
    GUI.Label(rect, "ControlState: "..self._private.controllingStatus) rect.y = rect.y + 16
    if(ball ~= nil) then
      GUI.Label(rect, "CurrentBall: "..ball.name) rect.y = rect.y + 16
      GUI.Label(rect, "Pos: "..LuaUtils.Vector3ToString(ball.ball.transform.position)) rect.y = rect.y + 16
      GUI.Label(rect, "Rot: "..LuaUtils.Vector3ToString(ball.ball.transform.eulerAngles)) rect.y = rect.y + 16
      GUI.Label(rect, "LinearVelocity : "..LuaUtils.Vector3ToString(ball.rigidbody.LinearVelocity)) rect.y = rect.y + 16
      GUI.Label(rect, "AngularVelocity : "..LuaUtils.Vector3ToString(ball.rigidbody.AngularVelocity)) rect.y = rect.y + 16
    else
      GUI.Label(rect, "CurrentBall: nil") rect.y = rect.y + 16
    end
    GUI.Label(rect, "PushType: "..self.PushType) rect.y = rect.y + 16

    local cam = GamePlay.CamManager
    if(cam ~= nil) then
      GUI.Label(rect, "CameraDirection: "..cam.CamRotateValue) rect.y = rect.y + 16
      GUI.Label(rect, "CameraFollow: "..LuaUtils.BooleanToString(cam.FollowEnable)) rect.y = rect.y + 16
      GUI.Label(rect, "CameraLook: "..LuaUtils.BooleanToString(cam.LookEnable)) rect.y = rect.y + 16
    end
  end
end
]]--

--#region 球基础控制方法

---注册球 
---@param name string 球名称
---@param gameObject GameObject 球对象
function BallManager:RegisterBall(name, gameObject)

  --检查是否注册
  if(self:GetRegisterBall(name) ~= nil) then
    GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, 'Ball {0} already registered', { name })
    return
  end

  --检查是否添加了刚体组件
  local body = gameObject:GetComponent(PhysicsRT.PhysicsBody)
  if(body == nil) then
    GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotFound, TAG, 'Not fuoud PhysicsBody on Ball {0} , please add it before call RegisterBall', { name })
    return
  end

  gameObject:SetActive(false)

  local ball = GameObjectToLuaClass(gameObject)
  if(ball == nil) then
    GameErrorChecker.SetLastErrorAndLog(GameError.ClassNotFound, TAG, 'Not found Ball class on {0} !', { name })
  end
  local pieces = ball:GetPieces()
  if(pieces ~= nil) then
    ObjectStateBackupUtils.BackUpObjectAndChilds(pieces) --备份碎片的状态
  end

  table.insert(self._private.registerBalls, {
    name = name,
    ball = ball,
    rigidbody = body
  })
end
---取消注册球 
---@param name string 球名称
function BallManager:UnRegisterBall(name)
  local registerBalls = self._private.registerBalls
  for i = 1, #registerBalls do  
    if(registerBalls[i].name == name) then
      table.remove(registerBalls, i)
      return true
    end 
  end 
  GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, 'Ball {0} not register', { name })
  return false
end
---获取注册了的球 
---@param name string 球名称
function BallManager:GetRegisterBall(name)
  local registerBalls = self._private.registerBalls
  for i = 1, #registerBalls do  
    if(registerBalls[i].name == name) then
      return registerBalls[i]
    end 
  end
end
---获取当前的球
function BallManager:GetCurrentBall() return self._private.currentBall end
---设置当前正在控制的球 
---@param name string 球名称，不可为空
---@param status number|nil 同时设置新的状态
function BallManager:SetCurrentBall(name, status)
  local ball = self:GetRegisterBall(name)
  if(ball == nil) then
    GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, 'Ball {0} not register', { name })
  end
  if(self._private.currentBall ~= ball) then
    self:_DeactiveCurrentBall()
    self._private.currentBall = ball
    self.CurrentBall = ball.ball
    self:SetControllingStatus(status)
  end
end
---设置当前球的控制的状态
---@param status number|nil 新的状态值（@see BallControlStatus）,为 nil 时不改变状态只刷新
function BallManager:SetControllingStatus(status)
  if(status ~= nil) then 
    self._private.controllingStatus = status
  end
  self:_FlushCurrentBallAllStatus()
end
---重新恢复 LockMode 之前的速度值
function BallManager:RestoreCurrentBallSpeed()
  self:_RestoreRigidbodySpeed(self._private.currentBall.rigidbody)
end
---设置下一次球出生位置
---@param pos Vector3
function BallManager:SetNextRecoverPos(pos)
  self._private.nextRecoverPos = pos
end

  --#region 球状态工作方法

function BallManager:_FlushCurrentBallAllStatus() 
  local status = self._private.controllingStatus
  local current = self._private.currentBall;
  if status == BallControlStatus.NoControl then
    self.CanControll = false
    self.CanControllCamera = false
    self:_DeactiveCurrentBall()
    GamePlay.CamManager:DisbleAll()
  elseif status == BallControlStatus.Control then
    self.CanControll = true
    self.CanControllCamera = true
    self:_ActiveCurrentBall()
    self:_SetRigidbodyDaymic(current.rigidbody)
    GamePlay.CamManager:SetCamLook(true):SetCamFollow(true)
  elseif status == BallControlStatus.LockMode then
    self.CanControll = false
    self.CanControllCamera = true
    self:_ActiveCurrentBall()
    self:_ZeroSpeedRigidbody(current.rigidbody);
    self:_SetRigidbodyKeyFramed(current.rigidbody)
    GamePlay.CamManager:SetCamLook(true):SetCamFollow(true)
  elseif status == BallControlStatus.UnleashingMode then
    self.CanControll = false
    self.CanControllCamera = true
    self:_ActiveCurrentBall()
    self:_SetRigidbodyDaymic(current.rigidbody)
    GamePlay.CamManager:SetCamLook(true):SetCamFollow(false)
  end
end
---取消激活当前的球
function BallManager:_DeactiveCurrentBall() 
  local current = self._private.currentActiveBall
  if current ~= nil then
    --取消激活
    current.rigidbody:ForceDePhysics()
    current.ball.gameObject:SetActive(false)
    current.ball:Deactive()
    --清空摄像机跟随对象
    GamePlay.CamManager:SetTarget(nil)
    self._private.currentActiveBall = nil
  end
end
function BallManager:_ActiveCurrentBall() 
  local current = self._private.currentActiveBall
  if current == nil then
    current = self._private.currentBall
    self._private.currentActiveBall = self._private.currentBall

    ---设置位置
    current.ball.transform.position = self._private.nextRecoverPos
    --激活
    current.ball.gameObject:SetActive(true)
    current.rigidbody:ForcePhysics()
    current.ball:Active()
    if self._private.controllingStatus ~= BallControlStatus.LockMode then
      current.rigidbody:ForceActive()
    end
    --设置摄像机跟随对象
    GamePlay.CamManager:SetTarget(current.ball.transform)
  end
end
---设置刚体的速度为0
---@param rigidbody PhysicsBody
function BallManager:_ZeroSpeedRigidbody(rigidbody) 
  self._private.lastSaveLinearVelocity = rigidbody.LinearVelocity
  self._private.lastSaveAngularVelocity = rigidbody.AngularVelocity
  rigidbody.LinearVelocity = Vector3.zero
  rigidbody.AngularVelocity = Vector3.zero
end
function BallManager:_RestoreRigidbodySpeed(rigidbody) 
  rigidbody.LinearVelocity = self._private.lastSaveLinearVelocit
  rigidbody.AngularVelocity = self._private.lastSaveAngularVelocity
end
---设置刚体KeyFramed模式
---@param rigidbody PhysicsBody
function BallManager:_SetRigidbodyKeyFramed(rigidbody) 
  rigidbody:ForceDeactive()
  rigidbody.MotionType = MotionType.Keyframed
end
---设置刚体Daymic模式
---@param rigidbody PhysicsBody
function BallManager:_SetRigidbodyDaymic(rigidbody) 
  rigidbody.MotionType = MotionType.Dynamic
  rigidbody:ForceActive()
end

  --#endregion

--#endregion

--#region 球附加工具方法

---播放球出生时的烟雾
---@param pos Vector3 放置位置
function BallManager:PlaySmoke(pos)
  self._BallSmoke.transform.position = pos
  self._BallSmoke:SetActive(true)
end
---播放球出生时的闪电效果
---@param pos Vector3 放置位置
---@param smallToBig boolean 是否由小到大
---@param lightAnim boolean 是否同时播放灯光效果
function BallManager:PlayLighting(pos, smallToBig, lightAnim) 
  self._private.BallLightningSphere:PlayLighting(pos, smallToBig, lightAnim)
end

--#endregion

--#region 键盘设置与初始化

function BallManager:_InitKeyEvents()
  --添加按键事件
  local keyListener = self._private.keyListener
  local keySets = self._private.keySets
  keyListener:ClearKeyListen()
  keyListener:AddKeyListen(keySets.keyFront, keySets.keyFront2, function (key, down) self:_UpArrow_Key(key, down) end)
  keyListener:AddKeyListen(keySets.keyBack, keySets.keyBack2, function (key, down) self:_DownArrow_Key(key, down) end)
  keyListener:AddKeyListen(keySets.keyUp, function (key, down) self:_Up_Key(key, down) end)
  keyListener:AddKeyListen(keySets.keyDown,function (key, down) self:_Down_Key(key, down) end)
  keyListener:AddKeyListen(keySets.keyUpCamera, function (key, down) self:_Space_Key(key, down)  end)
  keyListener:AddKeyListen(keySets.keyRoateCamera, keySets.keyRoateCamera2, function (key, down) self:_Shift_Key(key, down)  end)

  --是否反向控制  
  if(self._private.reverseControl) then
    keyListener:AddKeyListen(keySets.keyLeft, keySets.keyLeft2, function (key, down) self:_RightArrow_Key(key, down) end)
    keyListener:AddKeyListen(keySets.keyRight, keySets.keyRight2, function (key, down) self:_LeftArrow_Key(key, down) end)
  else
    keyListener:AddKeyListen(keySets.keyLeft, keySets.keyLeft2, function (key, down) self:_LeftArrow_Key(key, down) end)
    keyListener:AddKeyListen(keySets.keyRight, keySets.keyRight2, function (key, down) self:_RightArrow_Key(key, down) end)
  end

  --测试按扭
  if self._DebugMode then
    self._private.keyListener:AddKeyListen(KeyCode.Alpha1, function (key, downed)
      if(downed) then
        self:SetCurrentBall('BallWood', BallControlStatus.Control)
      end
    end)
    self._private.keyListener:AddKeyListen(KeyCode.Alpha2, function (key, downed)
      if(downed) then
        self:SetCurrentBall('BallStone', BallControlStatus.Control)
      end
    end)
    self._private.keyListener:AddKeyListen(KeyCode.Alpha3, function (key, downed)
      if(downed) then
        self:SetCurrentBall('BallPaper', BallControlStatus.Control)
      end
    end)
    self._private.keyListener:AddKeyListen(KeyCode.Alpha4, function (key, downed)
      if(downed) then
        self:SetControllingStatus(BallControlStatus.NoControl)
        self:SetNextRecoverPos(Vector3.zero)
      end
    end)     
  end
end
function BallManager:_OnControlSettingsChanged()
  --当设置更改时或加载时，更新设置到当前变量
  local GameSettings = self._private.GameSettings
  local keySets = self._private.keySets
  keySets.keyFront = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.front", "UpArrow"))
  keySets.keyFront2 = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.front2", "W"))
  keySets.keyUp = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.up", "Q"))
  keySets.keyDown = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.down", "E"))
  keySets.keyBack = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.back", "DownArrow"))
  keySets.keyBack2 = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.back2", "S"))
  keySets.keyLeft = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.left", "LeftArrow"))
  keySets.keyLeft2 = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.left2", "A"))
  keySets.keyRight = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.right", "RightArrow"))
  keySets.keyRight2 = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.right2", "D"))
  keySets.keyRoateCamera = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.roate", "LeftShift"))
  keySets.keyRoateCamera2 = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.roate2", "RightShift"))
  keySets.keyUpCamera = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.up_cam", "Space"))
  keySets.keyUpCamera = LuaUtils.StringToKeyCode(GameSettings:GetString("control.key.up_cam", "Space"))

  self._private.reverseControl = GameSettings:GetBool("control.reverse", false)
  self:_InitKeyEvents()
end

--#endregion

--#region 键盘事件处理

function BallManager:_UpArrow_Key(key, down)
  if (down) then
    self:AddBallPush(BallPushType.Forward)
  else
    self:RemoveBallPush(BallPushType.Forward)
  end
end
function BallManager:_DownArrow_Key(key, down)
  if (down) then
    self:AddBallPush(BallPushType.Back)
  else
    self:RemoveBallPush(BallPushType.Back)
  end
end
function BallManager:_RightArrow_Key(key, down)
  self._RightPressed = down;
  if (down) then
      if (self.ShiftPressed) then
        self:RemoveBallPush(BallPushType.Right)
      else
        self:AddBallPush(BallPushType.Right)
      end
  else
    self:RemoveBallPush(BallPushType.Right)
    --旋转摄像机
    if (self.ShiftPressed) then
      GamePlay.CamManager:RotateRight()
    end
  end
end
function BallManager:_LeftArrow_Key(key, down)
  self._LeftPressed = down;
  if (down) then
    if (self.ShiftPressed) then
      self:RemoveBallPush(BallPushType.Left)
    else
      self:AddBallPush(BallPushType.Left)
    end
  else
    self:RemoveBallPush(BallPushType.Left)
    --旋转摄像机
    if (self.ShiftPressed) then
      GamePlay.CamManager:RotateLeft()
    end
  end
end
function BallManager:_Down_Key(key, down)
  if (self._DebugMode and down) then
    self:AddBallPush(BallPushType.Down)
  else
    self:RemoveBallPush(BallPushType.Down)
  end
end
function BallManager:_Up_Key(key, down)
  if (self._DebugMode and down) then
    self:AddBallPush(BallPushType.Up)
  else
    self:RemoveBallPush(BallPushType.Up)
  end
end
function BallManager:_Space_Key(key, down) 
  GamePlay.CamManager:RotateUp(down) 
end
function BallManager:_Shift_Key(key, down) 
  self.ShiftPressed = down 
end

--#endregion

--#region 推动方法

---添加球推动方向
---@param t BallPushType 推动方向
function BallManager:AddBallPush(t)
  self.PushType = LuaUtils.Or(self.PushType, t)
end
---去除球推动方向
---@param t BallPushType 推动方向
function BallManager:RemoveBallPush(t)
  self.PushType = LuaUtils.And(self.PushType, LuaUtils.Not(t))
end

--#endregion

function CreateClass_BallManager()
  return BallManager() 
end