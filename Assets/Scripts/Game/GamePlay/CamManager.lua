local Quaternion = UnityEngine.Quaternion
local Vector3 = UnityEngine.Vector3
local Mathf = UnityEngine.Mathf

---@type GameLuaObjectHostClass
---@class CamManager
CamManager = {
  TAG = 'CamManager',

  --一些摄像机的旋转动画曲线
  animationCurveCamera = nil, ---@type AnimationCurve ---@public
  animationCurveCameraZ = nil, ---@type AnimationCurve ---@public
  animationCurveCameraMoveY = nil, ---@type AnimationCurve ---@public
  animationCurveCameraMoveYDown = nil, ---@type AnimationCurve ---@public

  isFollowCam = false,
  isLookingBall = false,
  isCameraSpaced = false,
  isCameraRoteingX = false,
  isCameraMovingY = false,
  isCameraMovingYDown = false,
  isCameraMovingZ = false,
  isCameraMovingZFar= false,

  cameraRoteValue = DirectionType.Forward,
  cameraHeight = 15,
  cameraLeaveFarMaxOffest = 15,
  cameraLeaveNearMaxOffestZ = -5,
  cameraLeaveFarMaxOffestZ = -15,
  cameraMaxRoteingOffest = 5,
  cameraSpaceMaxOffest = 80,
  cameraSpaceOffest = 10,

  --摄像机方向控制

  camFollowSpeed = 0.1,
  camFollowSpeed2 = 0.05,
  camMoveSpeedZ = 1,

  ballCamMoveHost = nil, ---@type GameObject
  ballCamera = nil, ---@type Camera
  ballCamFollowHost = nil, ---@type GameObject
  ballCamFollowTarget = nil, ---@type GameObject

  camVelocityTarget2 = Vector3(), ---@type Vector3
  camFollowTarget = nil, ---@type Transform
  camVelocityTarget = Vector3(), ---@type Vector3

  cameraRoteXVal = 0,
  cameraRoteXTarget = 0,
  cameraRoteXAll = 0,
}

---摄像机方向
DirectionType = {
  Forward = 0,
  Left = 1,
  Back = 2,
  Right = 3,
}

function CreateClass_CamManager()

  function CamManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function CamManager:Start(thisGameObject)

  end
  function CamManager:OnDestroy()

  end
  function CamManager:FixedUpdate()
    if (!IsControlling.IsNull() and IsControlling.BoolData()) then
      self:CamUpdate()
    end
    if (self.isFollowCam) then
      self:CamFollow()
    end  
  end

  function CamManager:OnGUI()

  end

  ---将摄像机开启
  function CamManager:CamStart()
    self.ballCamera.transform.position = Vector3(0, self.cameraHeight, -self.cameraLeaveFarMaxOffest)
    self.ballCamera.transform.localEulerAngles = Vector3(45, 0, 0)
    self.ballCamera.gameObject:SetActive(true)
    self.cameraRoteXVal = 0
  end
  ---将摄像机关闭
  function CamManager:CamClose()
    self.ballCamera.gameObject:SetActive(false)
  end
  ---让摄像机不看着球
  function CamManager:CamSetNoLookAtBall()
    self.isLookingBall = false
  end
  ---让摄像机看着球
  function CamManager:CamSetLookAtBall()
    if (!CurrentBall.IsNull()) then
      self.isLookingBall = true
    end
  end
  ---让摄像机只看着球（不跟随）
  function CamManager:CamSetJustLookAtBall()
    if (!CurrentBall.IsNull()) then
      self.isFollowCam = false
      self.isLookingBall = true
    end
  end
  ---摄像机向左旋转
  function CamManager:CamRoteLeft()
    if (self.cameraRoteValue < DirectionType.Right) then
      self.cameraRoteValue = self.cameraRoteValue + 1
    else 
      self.cameraRoteValue = DirectionType.Forward
    end
    self:CamRoteResetTarget(true)
    self:CamRoteResetVector()
    self.isCameraRoteingX = true
  end
  ---摄像机向右旋转
  function CamManager:CamRoteRight()
    if (self.cameraRoteValue > DirectionType.Forward) then 
      self.cameraRoteValue = self.cameraRoteValue - 1
    else 
      self.cameraRoteValue = DirectionType.Right
    end

    self:CamRoteResetTarget(false)
    self:CamRoteResetVector()
    self.isCameraRoteingX = true
  end
  ---摄像机面对向量重置
  function CamManager:CamRoteResetVector()
    --根据摄像机朝向重置几个球推动的方向向量
    --这4个方向向量用于球
    local y = -self.ballCamMoveHost.transform.localEulerAngles.y
    self._thisVector3Right = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.right
    self._thisVector3Left = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.left
    self._thisVector3Forward = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.forward
    self._thisVector3Back = Quaternion.AngleAxis(-y, Vector3.up) * Vector3.back
  end
  ---摄像机旋转目标
  ---@param left boolean 是否是向左旋转
  function CamManager:CamRoteResetTarget(left)
    self.cameraRoteXTarget = self.cameraRoteXTarget + (left and 90 or -90)
    self.cameraRoteXAll = Mathf.Abs(self.cameraRoteXTarget - self.cameraRoteXVal)
  end
  ---摄像机旋转偏移重置
  function CamManager:CamRoteResetTargetOffest()
    if (self.ballCamMoveHost.transform.localEulerAngles.y ~= self.cameraRoteXTarget) then
      self.ballCamMoveHost.transform.localEulerAngles = Vector3(0, self.cameraRoteXTarget, 0)
    end
  end
  ---摄像机 按住 空格键 上升
  function CamManager:CamRoteSpace()
    if (not self.isCameraSpaced) then
      self.isCameraSpaced = true
      self.isCameraMovingY = true
      self.isCameraMovingYDown = false
      self.isCameraMovingZ = true
      self.isCameraMovingZFar = false
    end
  end
  ---摄像机 放开 空格键 下降
  function CamManager:CamRoteSpaceBack()
    if (self.isCameraSpaced) then
      self.isCameraSpaced = false
      self.isCameraMovingY = true
      self.isCameraMovingYDown = true
      self.isCameraMovingZ = true
      self.isCameraMovingZFar = true
    end
  end


  ---@param v number
  function CamManager:CamRoteSpeedFun(v)
    return self.animationCurveCamera.Evaluate((
      Mathf.Abs(self.cameraRoteXTarget -  v) / self.cameraRoteXAll)
      ) * self.cameraMaxRoteingOffest
  end
  ---@param v number
  function CamManager:CamMoveSpeedFunZ(v)
    return self.animationCurveCameraZ.Evaluate(Mathf.Abs(v / self.cameraLeaveFarMaxOffest)) * self.camMoveSpeedZ
  end  
  ---@param v number
  function CamManager:CamMoveSpeedFunY(v)
    return self.animationCurveCameraMoveY.Evaluate(Mathf.Abs(v / self.cameraSpaceMaxOffest)) * self.cameraSpaceOffest
  end
  ---@param v number
  function CamManager:CamMoveSpeedFunYDown(v)
    return self.animationCurveCameraMoveYDown.Evaluate(Mathf.Abs((v) / self.cameraSpaceMaxOffest)) * self.cameraSpaceOffest
  end

  ---摄像机跟随 每帧
  function CamManager:CamFollow()
    if (self.camFollowTarget == nil) then
      self.isFollowCam = false
      return
    end
    if (self.camFollowTarget == nil) then
      if (not CurrentBall.IsNull()) then
        local ball = ((GameBall)CurrentBall.Data())
        self.ballCamFollowTarget.transform.position = Vector3.SmoothDamp( self.ballCamFollowTarget.transform.position, ball.transform.position,  self.camVelocityTarget2, self.camFollowSpeed2)
        self.ballCamFollowTarget.transform.position = Vector3(self.ballCamFollowTarget.transform.position.x, ball.transform.position.y, self.ballCamFollowTarget.transform.position.z)
        self.ballCamFollowHost.transform.position = Vector3.SmoothDamp( self.ballCamFollowHost.transform.position, ball.transform.position,  self.camVelocityTarget, self.camFollowSpeed)
      end
    end
  end  
  ---摄像机更新
  function CamManager:CamUpdate()
    --水平旋转
    if (self.isCameraRoteingX) then
      local abs = Mathf.Abs(self.cameraRoteXVal - self.cameraRoteXTarget)
      local off = self:CamRoteSpeedFun(self.cameraRoteXVal)
      if (abs > 0.8) then
        if (off > abs) then off = abs - 0.1 end
        if (self.cameraRoteXVal < self.cameraRoteXTarget) then
          self.cameraRoteXVal = self.cameraRoteXVal + off
        elseif (self.cameraRoteXVal > self.cameraRoteXTarget) then
          self.cameraRoteXVal = self.cameraRoteXVal - off
        end
        self.ballCamMoveHost.transform.localEulerAngles = Vector3(0, self.cameraRoteXVal, 0)
      else
        self.isCameraRoteingX = false
        self:CamRoteResetTargetOffest()
      end

      self:CamRoteResetVector()
    end

    --空格键 垂直上升
    if (self.isCameraMovingY) then
      if (self.isCameraMovingYDown) then
        if (self.ballCamMoveHost.transform.localPosition.y > 0) then
          self.ballCamMoveHost.transform.localPosition = Vector3(0, (self.ballCamMoveHost.transform.localPosition.y - self:CamMoveSpeedFunYDown(self.ballCamMoveHost.transform.localPosition.y)), 0)
        else
          self.ballCamMoveHost.transform.localPosition = Vector3(0, 0, 0)
          self.isCameraMovingY = false
        end
      else
        if (self.ballCamMoveHost.transform.localPosition.y < self.cameraSpaceMaxOffest) then
          self.ballCamMoveHost.transform.localPosition = Vector3(0, self.ballCamMoveHost.transform.localPosition.y + self:CamMoveSpeedFunY(self.ballCamMoveHost.transform.localPosition.y), 0)
        else
          self.ballCamMoveHost.transform.localPosition = Vector3(0, self.cameraSpaceMaxOffest, 0)
          self.isCameraMovingY = false
        end
      end
    end
    --空格键 靠近球
    if (self.isCameraMovingZ) then
        if (self.isCameraMovingZFar) then
          local abs = Mathf.Abs(self.ballCamera.transform.localPosition.z - self.cameraLeaveFarMaxOffestZ)
          local off = self:CamMoveSpeedFunZ(self.ballCamera.transform.localPosition.z)
          if (abs > 1) then
            if (off > abs) then off = abs - 0.1 end
            if (self.ballCamera.transform.localPosition.z < self.cameraLeaveFarMaxOffestZ) then
              self.ballCamera.transform.localPosition = Vector3(self.ballCamera.transform.localPosition.x, self.ballCamera.transform.localPosition.y, (self.ballCamera.transform.localPosition.z + off))
            elseif (self.ballCamera.transform.localPosition.z > self.cameraLeaveFarMaxOffestZ) then
              self.ballCamera.transform.localPosition = Vector3(self.ballCamera.transform.localPosition.x, self.ballCamera.transform.localPosition.y, (self.ballCamera.transform.localPosition.z - off))
            end
          else
            self.ballCamera.transform.localPosition = Vector3(self.ballCamera.transform.localPosition.x, self.ballCamera.transform.localPosition.y, self.cameraLeaveFarMaxOffestZ)
            self.isCameraMovingZ = false
          end
        else
          local abs = Mathf.Abs(self.ballCamera.transform.localPosition.z - self.cameraLeaveNearMaxOffestZ)
          local off = self:CamMoveSpeedFunZ(self.ballCamera.transform.localPosition.z)
          if (abs > 0.2) then
              if (off > abs) then off = abs - 0.1 end
              if (self.ballCamera.transform.localPosition.z < self.cameraLeaveNearMaxOffestZ) then
                self.ballCamera.transform.localPosition = Vector3(self.ballCamera.transform.localPosition.x, self.ballCamera.transform.localPosition.y, (self.ballCamera.transform.localPosition.z + off))
              elseif (self.ballCamera.transform.localPosition.z > self.cameraLeaveNearMaxOffestZ) then
                self.ballCamera.transform.localPosition = Vector3(self.ballCamera.transform.localPosition.x, self.ballCamera.transform.localPosition.y, (self.ballCamera.transform.localPosition.z - off))
              end
          else
            self.ballCamera.transform.localPosition = Vector3(self.ballCamera.transform.localPosition.x, self.ballCamera.transform.localPosition.y, self.cameraLeaveNearMaxOffestZ)
            self.isCameraMovingZ = false
          end
        end
      end

    --看着球
    if (self.isLookingBall and self.isFollowCam) then
      self.ballCamera.transform.LookAt(self.ballCamFollowTarget.transform)
    elseif (self.isLookingBall and not self.isFollowCam) then
      if (!CurrentBall.IsNull()) then
        self.ballCamera.transform.LookAt(((GameBall)CurrentBall.Data()).transform)
      end    
    end
  end

  function CamManager:RegisterStore()
    self.actionStore.RegisterActions(
      {
        "CamStart",
        "CamClose",
        "CamSetNoLookAtBall",
        "CamSetLookAtBall",
        "CamSetJustLookAtBall",
        "CamRoteLeft",
        "CamRoteRight",
        "CamRoteSpace",
        "CamRoteSpaceBack",
      },
      self.TAG,
      {
          function (param)
            CamStart()
            return GameActionCallResult.SuccessResult
          end,
          function (param)
            CamClose()
            return GameActionCallResult.SuccessResult
          end,
          function (param) 
            CamSetNoLookAtBall()
            return GameActionCallResult.SuccessResult
          end,
          function (param) 
            CamSetLookAtBall()
            return GameActionCallResult.SuccessResult
          end,
          function (param) 
            CamSetJustLookAtBall()
            return GameActionCallResult.SuccessResult
          end,
          function (param) 
            CamRoteLeft()
            return GameActionCallResult.SuccessResult
          end,
          function (param) 
            CamRoteRight()
            return GameActionCallResult.SuccessResult
          end,
          function (param) 
            CamRoteSpace()
            return GameActionCallResult.SuccessResult
          end,
          function(param)
            CamRoteSpaceBack()
            return GameActionCallResult.SuccessResult
          end,
      },
      nil
    )
  end

  return CamManager:new(nil)
end