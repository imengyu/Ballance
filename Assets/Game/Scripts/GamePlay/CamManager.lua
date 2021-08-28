local Vector3 = UnityEngine.Vector3
local Quaternion = UnityEngine.Quaternion
local Time = UnityEngine.Time
local Log = Ballance2.Utils.Log
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
---@field _CameraNormalX number
---@field _CameraNormalY number
---@field _CameraSpaceY number
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
  self._CameraRotateTime = 0.5
  self._CameraRotateUpTime = 0.8
  self._CameraNormalX = -16
  self._CameraNormalY = 30
  self._CameraSpaceY = 60
  self._CameraSpaceX = -10
  self.CamRightVector = Vector3.right
  self.CamLeftVector = Vector3.left
  self.CamForwerdVector = Vector3.forward
  self.CamBackVector = Vector3.back
  self.CamFollowSpeed = 0.05
  self.CamIsSpaced = false
  self.Target = nil
  self.CamRotateValue = CamRotateType.North

  self._CamIsRotateing = false
  self._CamRotateTick = 0
  self._CamIsRotateingUp = false
  self._CamRotateUpTick = 0
  self._CamRotateUpStart = { 
    y = 0,
    x = 0
  }
  self._CamRotateUpTarget = { 
    y = 0,
    x = 0
  }
  self._CamOutSpeed = Vector3.zero
  self._CamRotate = 0
  self._CamRotateStartDegree = 0
  self._CamRotateTargetDegree = 0

  GamePlay.CamManager = self
end

function CamManager:Start()
  self.CamFollow = self._CameraHost:GetComponent(CamFollow) ---@type CamFollow
  self.transform.localPosition = Vector3(self._CameraNormalX, self._CameraNormalY, 0)
  self.transform:LookAt(Vector3.zero)
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
    self.transform.localPosition = Vector3(self._CamRotateUpStart.x + v * self._CamRotateUpTarget.x, self._CamRotateUpStart.y + v * self._CamRotateUpTarget.y, 0)
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
  if type == CamRotateType.North then return 0
  elseif type == CamRotateType.East then return 90
  elseif type == CamRotateType.South then return 180
  elseif type == CamRotateType.West then return 270
  end
  return 0
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
  return self
end
---空格键向上旋转
---@param enable boolean
function CamManager:RotateUp(enable)
  self.CamIsSpaced = enable
  self._CamRotateUpStart.y = self.transform.localPosition.y
  self._CamRotateUpStart.x = self.transform.localPosition.x
  if enable then
    self._CamRotateUpTarget.y = self._CameraSpaceY - self._CamRotateUpStart.y
    self._CamRotateUpTarget.X = self._CameraSpaceX - self._CamRotateUpStart.x
  else
    self._CamRotateUpTarget.y = self._CameraNormalY - self._CamRotateUpStart.y
    self._CamRotateUpTarget.x = self._CameraNormalX - self._CamRotateUpStart.x
  end
  self._CamRotateUpTick = 0
  self._CamIsRotateingUp = true
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
  return self
end
---指定摄像机看着球是否开启
---@param enable boolean
function CamManager:SetCamLook(enable)
  self.CamFollow.Look = enable
  return self
end
---指定当前跟踪的目标
---@param target Transform
function CamManager:SetTarget(target)
  self.Target = target
  self.CamFollow.Target = target
  return self
end
function CamManager:DisbleAll()
  self.CamFollow.Follow = false
  self.CamFollow.Look = false
  self.CamFollow.Target = nil
  return self
end

--#endregion

function CreateClass_CamManager()
  return CamManager()
end