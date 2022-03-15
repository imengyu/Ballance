local Vector3 = UnityEngine.Vector3
local Quaternion = UnityEngine.Quaternion
local Time = UnityEngine.Time
local CamFollow = Ballance2.Game.CamFollow

CamRotateType = {
  North = 0,
  East = 1,
  South = 2,
  West = 3,
}

---摄像机管理器
---@class CamManager : GameLuaObjectHostClass
---@field _CameraHost GameObject
---@field _CameraHostTransform Transform
---@field _SkyBox Skybox
---@field _CamRotateSpeedCurve AnimationCurve 
---@field _CamUpSpeedCurve AnimationCurve 
---@field _CameraRotateTime number
---@field _CameraRotateUpTime number
---@field _CameraNormalZ number
---@field _CameraNormalY number
---@field _CameraSpaceY number
---@field _PosFrame Transform 
---@field CamDirectionRef Transform 获取球参照的摄像机旋转方向变换 [R]
---@field CamRightVector Vector3 获取摄像机右侧向量 [R]
---@field CamLeftVector Vector3 获取摄像机左侧向量 [R]
---@field CamForwerdVector Vector3 获取摄像机向前向量 [R]
---@field CamBackVector Vector3 获取摄像机向后向量 [R]
---@field CamFollowSpeed number 摄像机跟随速度 [RW]
---@field CamIsSpaced boolean 获取摄像机是否空格键升高了 [R]
---@field CamRotateValue number 获取当前摄像机方向（0-3, CamRotateType） [R] @see CamRotateType 设置请使用 RotateTo 方法
---@field CamFollow CamFollow 获取摄像机跟随脚本 [R]
CamManager = ClassicObject:extend()

function CamManager:new()
  self._CameraRotateTime = 0.4
  self._CameraRotateUpTime = 0.8
  self._CameraNormalZ = 18
  self._CameraNormalY = 30
  self._CameraSpaceY = 55
  self._CameraSpaceZ = 8
  self.CamRightVector = Vector3.right
  self.CamLeftVector = Vector3.left
  self.CamForwerdVector = Vector3.forward
  self.CamBackVector = Vector3.back
  self.CamDirectionRef = nil
  self.CamFollowSpeed = 0.05
  self.CamIsSpaced = false
  self.Target = nil
  self.CamRotateValue = CamRotateType.North

  self._CamIsRotateing = false
  self._CamRotateTick = 0
  self._CamIsRotateingUp = false
  self._CamRotateUpTick = 0
  self._CamRotateUpStart = { 
    z = 0,
    y = 0
  }
  self._CamRotateUpTarget = { 
    z = 0,
    y = 0
  }
  self._CamOutSpeed = Vector3.zero
  self._CamRotate = 0
  self._CamRotateStartDegree = 0
  self._CamRotateTargetDegree = 0

  GamePlay.CamManager = self
end

function CamManager:Start()
  self.CamFollow = self._CameraHost:GetComponent(CamFollow) ---@type CamFollow
  self.transform.localPosition = Vector3(0, self._CameraNormalY, -self._CameraNormalZ)
  self.transform:LookAt(Vector3.zero)
  self.CamDirectionRef = self._CameraHost.transform

  --注册事件
  local events = Game.Mediator:RegisterEventEmitter('CamManager')
  self.EventRotateUpStateChanged = events:RegisterEvent('RotateUpStateChanged') --空格键升起摄像机状态变化事件
  self.EventRotateDirectionChanged = events:RegisterEvent('RotateDirectionChanged') --摄像机旋转方向变化事件
  self.EventCamFollowChanged = events:RegisterEvent('CamFollowChanged') --摄像机跟踪目标变化事件
  self.EventCamLookChanged = events:RegisterEvent('CamLookChanged') --摄像机对准目标变化事件
  self.EventCamFollowTargetChanged = events:RegisterEvent('CamFollowTargetChanged') --摄像机跟踪目标变化事件

  self._CommandId = Game.Manager.GameDebugCommandServer:RegisterCommand('cam', function (eyword, fullCmd, argsCount, args)
    local type = args[1]
    if type == 'left' then
      self:RotateLeft()
    elseif type == 'right' then
      self:RotateLeft()
    elseif type == 'up' then
      self:RotateUp(true)
    elseif type == 'down' then
      self:RotateUp(false)
    elseif type == 'follow' then
      self:SetCamFollow(true)
    elseif type == 'no-follow' then
      self:SetCamFollow(false)
    elseif type == 'look' then
      self:SetCamLook(true)
    elseif type == 'no-look' then
      self:SetCamLook(false)
    end
    return true
  end, 1, "cam <left/right/up/down/-all> 摄像机管理器命令"..
          "  left      ▶ 向左旋转摄像机"..
          "  right     ▶ 向右旋转摄像机"..
          "  up        ▶ 空格键升起摄像机"..
          "  down      ▶ 空格放开落下摄像机"..
          "  follow    ▶ 开启摄像机跟随"..
          "  no-follow ▶ 关闭摄像机跟随"..
          "  look      ▶ 开启摄像机看球"..
          "  no-look   ▶ 关闭摄像机看球"
  )
end
function CamManager:OnDestroy() 
  Game.Mediator:UnRegisterEventEmitter('CamManager')
  Game.Manager.GameDebugCommandServer:UnRegisterCommand(self._CommandId)
end

function CamManager:FixedUpdate()
  --摄像机水平旋转
  if self._CamIsRotateing then
    self._CamRotateTick = self._CamRotateTick + Time.deltaTime

    local v = self._CamRotateSpeedCurve:Evaluate(self._CamRotateTick / self._CameraRotateTime)
    self._CameraHostTransform.localEulerAngles = Vector3(0, self._CamRotateStartDegree + v * self._CamRotateTargetDegree, 0)
    if v >= 1 then
      self._CamIsRotateing = false
      self:ResetVector()
    end
  end
  --摄像机垂直向上
  if self._CamIsRotateingUp then
    self._CamRotateUpTick = self._CamRotateUpTick + Time.deltaTime
    
    local v = 0
    if self.CamIsSpaced then
      v = self._CamUpSpeedCurve:Evaluate(self._CamRotateUpTick / self._CameraRotateUpTime)
    else
      v = self._CamRotateSpeedCurve:Evaluate(self._CamRotateUpTick / self._CameraRotateUpTime)
    end
    self.transform.localPosition = Vector3(0, self._CamRotateUpStart.y + v * self._CamRotateUpTarget.y, self._CamRotateUpStart.z + v * self._CamRotateUpTarget.z)
    if v >= 1 then
      self._CamIsRotateingUp = false
    end
  end
end

---摄像机面对向量重置
function CamManager:ResetVector()
  --根据摄像机朝向重置几个球推动的方向向量
  local y = -self._CameraHostTransform.localEulerAngles.y - 90
  self.CamRightVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.right
  self.CamLeftVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.left
  self.CamForwerdVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.forward
  self.CamBackVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.back
end
---通过旋转方向获取目标角度
---@param type number CamRotateType
function CamManager:GetRotateDegreeByType(type)
  if type == CamRotateType.North then return 90
  elseif type == CamRotateType.East then return 180
  elseif type == CamRotateType.South then return 270
  elseif type == CamRotateType.West then return 0
  end
  return 0
end
function CamManager:_UpdateStateForDebugStats()
  if BALLANCE_DEBUG then 
    if self.CamRotateValue == CamRotateType.North then 
      GameUI.GamePlayUI._DebugStatValues['CamDirection'].Value = 'North'
    elseif self.CamRotateValue == CamRotateType.East then 
      GameUI.GamePlayUI._DebugStatValues['CamDirection'].Value = 'East'
    elseif self.CamRotateValue == CamRotateType.South then 
      GameUI.GamePlayUI._DebugStatValues['CamDirection'].Value = 'South'
    elseif self.CamRotateValue == CamRotateType.West then 
      GameUI.GamePlayUI._DebugStatValues['CamDirection'].Value = 'West'
    end

    GameUI.GamePlayUI._DebugStatValues['CamState'].Value = 'IsSpaced: '..tostring(self.CamIsSpaced)
      ..' Follow: '..tostring(self.CamFollow.Follow)..' Look: '..tostring(self.CamFollow.Look)
  end
end

--#region 公共方法

---通过RestPoint占位符设置摄像机的方向和位置
---@param go GameObject RestPoint占位符
function CamManager:SetPosAndDirByRestPoint(go) 
  local rot = go.transform.eulerAngles.y
  local type = 0
  rot = rot % 360
  if rot < 0 then rot = rot + 360 
  elseif rot > 315 then rot = rot - 360 
  end

  if rot >= -45 and rot < 45 then type = CamRotateType.South
  elseif rot >= 45 and rot < 135 then type = CamRotateType.West
  elseif rot >= 135 and rot < 225 then type = CamRotateType.North
  elseif rot >= 225 and rot < 315 then type = CamRotateType.East
  end

  self._CameraHostTransform.eulerAngles = Vector3(0, self:GetRotateDegreeByType(type), 0)
  self._CameraHostTransform.position = go.transform.position
  self.CamRotateValue = type
  self:ResetVector()
  self:_UpdateStateForDebugStats()
  return self
end
---空格键向上旋转
---@param enable boolean
function CamManager:RotateUp(enable)
  self.CamIsSpaced = enable
  self._CamRotateUpStart.y = self.transform.localPosition.y
  self._CamRotateUpStart.z = self.transform.localPosition.z
  if enable then
    self._CamRotateUpTarget.y = self._CameraSpaceY - self._CamRotateUpStart.y
    self._CamRotateUpTarget.z = -self._CameraSpaceZ - self._CamRotateUpStart.z
  else
    self._CamRotateUpTarget.y = self._CameraNormalY - self._CamRotateUpStart.y
    self._CamRotateUpTarget.z = -self._CameraNormalZ - self._CamRotateUpStart.z
  end
  self._CamRotateUpTick = 0
  self._CamIsRotateingUp = true
  self.EventRotateUpStateChanged:Emit(enable)
  self:_UpdateStateForDebugStats()
  return self
end
---摄像机旋转指定角度
---@param val number 方向 CamRotateValue
function CamManager:RotateTo(val)
  self.CamRotateValue = val
  local target = self:GetRotateDegreeByType(self.CamRotateValue)

  self._CamRotateStartDegree = self._CameraHostTransform.eulerAngles.y
  if(target > self._CamRotateStartDegree) then
    target = target - 360
  end 

  self._CamRotateTargetDegree = target - self._CamRotateStartDegree
  self._CamRotateTick = 0
  self._CamIsRotateing = true
  self.EventRotateDirectionChanged:Emit(self._CamRotateTargetDegree)
  self:_UpdateStateForDebugStats()
  return self
end
---摄像机向左旋转
function CamManager:RotateRight()
  self.CamRotateValue = self.CamRotateValue - 1
  if(self.CamRotateValue < 0) then self.CamRotateValue = 3 end
  local target = self:GetRotateDegreeByType(self.CamRotateValue)

  self._CamRotateStartDegree = self._CameraHostTransform.eulerAngles.y
  if(target > self._CamRotateStartDegree) then
    target = target - 360
  end 

  self._CamRotateTargetDegree = target - self._CamRotateStartDegree
  self._CamRotateTick = 0
  self._CamIsRotateing = true
  self.EventRotateDirectionChanged:Emit(self._CamRotateTargetDegree)
  self:_UpdateStateForDebugStats()
  return self
end
---摄像机向右旋转
function CamManager:RotateLeft()
  self.CamRotateValue = self.CamRotateValue + 1
  if(self.CamRotateValue > 3) then self.CamRotateValue = 0 end
  local target = self:GetRotateDegreeByType(self.CamRotateValue)

  self._CamRotateStartDegree = self._CameraHostTransform.eulerAngles.y
  if(target < self._CamRotateStartDegree) then
    target = target + 360
  end 

  self._CamRotateTargetDegree = target - self._CamRotateStartDegree
  self._CamRotateTick = 0
  self._CamIsRotateing = true
  self.EventRotateDirectionChanged:Emit(self._CamRotateTargetDegree)
  self:_UpdateStateForDebugStats()
  return self
end
---设置主摄像机天空盒材质
---@param mat Material
function CamManager:SetSkyBox(mat)
  self._SkyBox.material = mat
  return self
end
---指定摄像机跟随球是否开启
---@param enable boolean
function CamManager:SetCamFollow(enable)
  self.CamFollow.Follow = enable
  self.EventCamFollowChanged:Emit(enable)
  self:_UpdateStateForDebugStats()
  return self
end
---指定摄像机看着球是否开启
---@param enable boolean
function CamManager:SetCamLook(enable)
  self.CamFollow.Look = enable
  self.EventCamLookChanged:Emit(enable)
  self:_UpdateStateForDebugStats()
  return self
end
---指定当前跟踪的目标
---@param target Transform
function CamManager:SetTarget(target)
  self.Target = target
  self.CamFollow.Target = target
  self.EventCamFollowTargetChanged:Emit(target)
  return self
end
function CamManager:DisbleAll()
  self.CamFollow.Follow = false
  self.CamFollow.Look = false
  self.CamFollow.Target = nil
  self.EventCamFollowChanged:Emit(false)
  self.EventCamLookChanged:Emit(false)
  self.EventCamFollowTargetChanged:Emit(nil)
  self:_UpdateStateForDebugStats()
  return self
end

--#endregion

function CreateClass:CamManager()
  return CamManager()
end