local Vector3 = UnityEngine.Vector3
local Quaternion = UnityEngine.Quaternion
local Time = UnityEngine.Time
local Log = Ballance2.Utils.Log

CamRotateType = {
  North = 0,
  East = 1,
  South = 2,
  West = 3,
}

---摄像机管理器
---@class CamManager : GameLuaObjectHostClass
CamManager = {
  _CameraHost = nil, ---@type GameObject
  _CameraHostTransform = nil, ---@type Transform
  _SkyBox = nil, ---@type Skybox
  _CamRotateSpeedCurve = nil, ---@type AnimationCurve
  _CameraRotateTime = 1.1,
  _CameraRotateUpTime = 0.9,

  _CameraNormalZ = -22,
  _CameraNormalY = 35,
  _CameraSpaceY = 50,

  ---获取摄像机右侧向量 [R]
  CamRightVector = Vector3.right,
  ---获取摄像机左侧向量 [R]
  CamLeftVector = Vector3.left,
  ---获取摄像机向前向量 [R]
  CamForwerdVector = Vector3.forward,
  ---获取摄像机向后向量 [R]
  CamBackVector = Vector3.back,
  ---摄像机跟随速度 [RW]
  CamFollowSpeed = 0.05,
  ---指定摄像机跟随球是否开启 [RW]
  FollowEnable = false,
  ---指定摄像机看着球是否开启 [RW]
  LookEnable = false,
  ---指定当前跟踪的目标 [RW]
  Target = nil, ---@type Transform 
  ---获取当前摄像机方向（0-3, CamRotateType） [R]
  ---@see CamRotateType 
  CamRotateValue = CamRotateType.North,

  _CamIsRotateing = false,
  _CamRotateTick = 0,
  _CamIsRotateingUp = false,
  _CamRotateUpTick = 0,
  _CamRotateUpStart = { 
    y = 0,
    z = 0
  },
  _CamRotateUpTarget = { 
    y = 0,
    z = 0
  },
  _CamOutSpeed = Vector3.zero,

  _CamRotate = 0,
  _CamRotateStartDegree = 0,
  _CamRotateTargetDegree = 0,
}

function CreateClass_CamManager()
  function CamManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function CamManager:Start()
  end
  function CamManager:OnDestroy()
    self.Target = nil
  end
  function CamManager:Update()
    if self.Target ~= nil then
      --摄像机跟随
      if self.FollowEnable then
        self._CameraHostTransform.position = self.Target.position
        --self._CameraHostTransform.position, self._CamOutSpeed = Vector3.SmoothDamp(self._CameraHostTransform.position, self.Target.position, self._CamOutSpeed, self.CamFollowSpeed);
      end
      --摄像机看着
      if self.LookEnable then
        self.transform:LookAt(self.Target);
      end
    end
    --摄像机水平旋转
    if self._CamIsRotateing then
      self._CamRotateTick = self._CamRotateTick + Time.deltaTime

      local v = self._CamRotateSpeedCurve:Evaluate(self._CamRotateTick / self._CameraRotateTime)
      self._CameraHostTransform.eulerAngles = Vector3(0, self._CamRotateStartDegree + v * self._CamRotateTargetDegree, 0)
      if v >= 1 then
        self._CamIsRotateing = false
      end
    end
    --摄像机垂直向上
    if self._CamIsRotateingUp then
      self._CamRotateUpTick = self._CamRotateUpTick + Time.deltaTime
      
      local v = self._CamRotateSpeedCurve:Evaluate(self._CamRotateUpTick / self._CameraRotateUpTime)
      self.transform.position = Vector3(0, self._CamRotateUpStart.y + v * self._CamRotateUpTarget.y, self._CamRotateUpStart.z + v * self._CamRotateUpTarget.z)
      if v >= 1 then
        self._CamIsRotateingUp = false
      end
    end
  end

  ---摄像机面对向量重置
  function CamManager:ResetVector()
    --根据摄像机朝向重置几个球推动的方向向量
    local y = -self._CameraHostTransform.localEulerAngles.y;
    self.CamRightVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.right;
    self.CamLeftVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.left;
    self.CamForwerdVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.forward;
    self.CamBackVector = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.back;
  end
  ---通过旋转方向获取目标角度
  ---@param type number CamRotateType
  function CamManager:GetRotateDegree(type)
    if type == CamRotateType.North then return 0
    elseif type == CamRotateType.East then return 90
    elseif type == CamRotateType.South then return 180
    elseif type == CamRotateType.West then return 270
    end
    return 0
  end

  --#region 公共方法

  ---空格键向上旋转
  ---@param enable boolean
  function CamManager:RotateUp(enable)
    self._CamRotateUpStart.y = self.transform.position.y
    self._CamRotateUpStart.z = self.transform.position.z
    if enable then
      self._CamRotateUpTarget.y = self._CameraSpaceY - self._CamRotateUpStart.y
      self._CamRotateUpTarget.z = 0 - self._CamRotateUpStart.z
    else
      self._CamRotateUpTarget.y = self._CameraNormalY - self._CamRotateUpStart.y
      self._CamRotateUpTarget.z = self._CameraNormalZ - self._CamRotateUpStart.z
    end
    self._CamRotateUpTick = 0
    self._CamIsRotateingUp = true
    return self
  end
  ---摄像机向左旋转
  function CamManager:RotateLeft()
    self.CamRotateValue = self.CamRotateValue - 1
    if(self.CamRotateValue < 0) then self.CamRotateValue = 3 end
    local target = self:GetRotateDegree(self.CamRotateValue)

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
  function CamManager:RotateRight()
    self.CamRotateValue = self.CamRotateValue + 1
    if(self.CamRotateValue > 3) then self.CamRotateValue = 0 end
    local target = self:GetRotateDegree(self.CamRotateValue)

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
    self.FollowEnable = enable
    return self
  end
  ---指定摄像机看着球是否开启
  ---@param enable boolean
  function CamManager:SetCamLook(enable)
    self.LookEnable = enable
    return self
  end
  ---指定当前跟踪的目标
  ---@param target Transform
  function CamManager:SetTarget(target)
    self.Target = target
    return self
  end
  function CamManager:DisbleAll()
    self.FollowEnable = false
    self.LookEnable = false
    self.Target = nil
    return self
  end

  --#endregion

  return CamManager:new(nil)
end