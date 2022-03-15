local GameSoundType = Ballance2.Services.GameSoundType
local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils
local SmoothFly = Ballance2.Game.Utils.SmoothFly
local SmoothFlyType = Ballance2.Game.Utils.SmoothFlyType
local Physics = UnityEngine.Physics
local Vector3 = UnityEngine.Vector3
local AudioSource = UnityEngine.AudioSource

---@class P_Extra_Point : ModulBase 
---@field P_Extra_Point_Tigger TiggerTester
---@field P_Extra_Point_Floor GameObject
---@field P_Extra_Point_Ball0 GameObject
---@field P_Extra_Point_Ball1 GameObject
---@field P_Extra_Point_Ball2 GameObject
---@field P_Extra_Point_Ball3 GameObject
---@field P_Extra_Point_Ball4 GameObject
---@field P_Extra_Point_Ball5 GameObject
---@field P_Extra_Point_Ball6 GameObject
---@field P_Extra_Point_Ball_Povit1 GameObject
---@field P_Extra_Point_Ball_Povit2 GameObject
---@field P_Extra_Point_Ball_Povit4 GameObject
---@field P_Extra_Point_Ball_Povit5 GameObject
---@field P_Extra_Point_Ball_Povit6 GameObject
---@field P_Extra_Point_Fizz GameObject
---@field P_Extra_Point_Sound AudioSource
P_Extra_Point = ModulBase:extend()

function P_Extra_Point:new()
  P_Extra_Point.super.new(self)
  self._Actived = false
  self._RotDegree = 6
  self._Rotate = false
  self._FlyUpTime = 2.5
  self._FlyFollowTime = 0.3
  self.AutoActiveBaseGameObject = false
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 80
end

function P_Extra_Point:Start()
  ModulBase.Start(self)
  
  if not self.IsPreviewMode then
    --初始化xiao'q
    self._P_Extra_Point_Ball_Fly1 = self.P_Extra_Point_Ball1:GetComponent(SmoothFly) ---@type SmoothFly
    self._P_Extra_Point_Ball_Fly2 = self.P_Extra_Point_Ball2:GetComponent(SmoothFly) ---@type SmoothFly
    self._P_Extra_Point_Ball_Fly3 = self.P_Extra_Point_Ball3:GetComponent(SmoothFly) ---@type SmoothFly
    self._P_Extra_Point_Ball_Fly4 = self.P_Extra_Point_Ball4:GetComponent(SmoothFly) ---@type SmoothFly
    self._P_Extra_Point_Ball_Fly5 = self.P_Extra_Point_Ball5:GetComponent(SmoothFly) ---@type SmoothFly
    self._P_Extra_Point_Ball_Fly6 = self.P_Extra_Point_Ball6:GetComponent(SmoothFly) ---@type SmoothFly

    for i = 1, 6, 1 do
      local fly = self['P_Extra_Point_Ball'..i]:GetComponent(SmoothFly) ---@type SmoothFly
      local hitAudio = self.transform:Find('P_Extra_Point_Ball'..i..'/P_Extra_Point_Hit'):GetComponent(AudioSource) ---@type AudioSource
      local hitFizz = self.transform:Find('P_Extra_Point_Ball'..i..'/P_Extra_Point_Fizz').gameObject
      local ballParticle = self.transform:Find('P_Extra_Point_Ball'..i..'/P_Extra_Point_Ball').gameObject
      local flowParticle = self.transform:Find('P_Extra_Point_Ball'..i..'/P_Extra_Point_Flow').gameObject

      Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, hitAudio)
      self['_P_Extra_Point_Ball_Fly'..i] = fly
      self['_P_Extra_Point_Ball_Rest'..i] = function ()
        ballParticle:SetActive(true)
        flowParticle:SetActive(true)
        hitFizz:SetActive(false)
        hitAudio:Stop()
      end

      fly.StopWhenArrival = true
      fly.ArrivalDiatance = 2
      fly.ArrivalCallback = function ()
        if not self._FlyUp then
          ballParticle:SetActive(false)
          flowParticle:SetActive(false)
          hitFizz:SetActive(true)
          hitAudio:Play()
          GamePlay.GamePlayManager:AddPoint(20) --小球是20分
        end
      end
    end

    ---@param otherBody GameObject
    self.P_Extra_Point_Tigger.onTriggerEnter = function (_, otherBody)
      if not self._Actived and otherBody.tag == 'Ball' then
        self._Actived = true
        self:StartFly()
        GamePlay.GamePlayManager:AddPoint(100) --大球是100分
      end
    end
  end

  self._OnFloor = false
  --触发射线，检查当前下方是不是路面，如果是，则显示 Shadow 
  ---@type boolean
  local ok, 
  ---@type RaycastHit
  hitinfo = Physics.Raycast(self.transform.position, Vector3(0, -1, 0), Slua.out, 5) 
  if ok and hitinfo.collider ~= nil then
    local parentName = hitinfo.collider.gameObject.tag
    if (parentName == 'Phys_Floors' or parentName == 'Phys_FloorWoods') and  parentName ~= 'Phys_FloorRails' then
      self._OnFloor = true
    end
  end

  self._RotCenter = self.transform.position
  self._RotAxis1 = self.P_Extra_Point_Ball_Povit1.transform.up
  self._RotAxis2 = self.P_Extra_Point_Ball_Povit2.transform.up
  self._RotAxis4 = self.P_Extra_Point_Ball_Povit4.transform.up
  self._RotAxis5 = self.P_Extra_Point_Ball_Povit5.transform.up
  self._RotAxis6 = self.P_Extra_Point_Ball_Povit6.transform.up
  self.P_Extra_Point_Ball_Povit1:SetActive(false)
  self.P_Extra_Point_Ball_Povit2:SetActive(false)
  self.P_Extra_Point_Ball_Povit4:SetActive(false)
  self.P_Extra_Point_Ball_Povit5:SetActive(false)
  self.P_Extra_Point_Ball_Povit6:SetActive(false)
end
function P_Extra_Point:StartFly()
  self._Rotate = false
  self._FlyUp = true

  self.P_Extra_Point_Floor:SetActive(false)
  self.P_Extra_Point_Ball0:SetActive(false)
  self.P_Extra_Point_Fizz:SetActive(true)
  self.P_Extra_Point_Sound:Play()

  local upY = 16

  for i = 1, 6, 1 do
    local ball = self['P_Extra_Point_Ball'..i] ---@type GameObject
    local fly = self['_P_Extra_Point_Ball_Fly'..i] ---@type SmoothFly
    local posMult = Vector3(1.2, 1, 1.2)
    local localPos = Vector3.Scale(ball.transform.localPosition, posMult)
  
    fly.Type = SmoothFlyType.Lerp
    fly.TargetTransform = nil
    fly.TargetPos = self.transform:TransformPoint(localPos.x, upY, localPos.z)
    fly.Time = self._FlyUpTime
    fly.Fly = true
    upY = upY + 0.2
  end

  local fTime =  self._FlyFollowTime

  LuaTimer.Add(1250, function ()
    self.P_Extra_Point_Fizz:SetActive(false)
    self._FlyUp = false

    local followTarget = GamePlay.CamManager.Target
    for i = 1, 6, 1 do
      local fly = self['_P_Extra_Point_Ball_Fly'..i] ---@type SmoothFly
      fly.Fly = true
      fly.Type = SmoothFlyType.SmoothDamp
      fly.TargetTransform = followTarget
      fly.Time = fTime
      fTime = fTime - 0.02
    end
  end)
end
function P_Extra_Point:Update()
  --旋转小球
  if self._Rotate then
    self.P_Extra_Point_Ball1.transform:RotateAround(self._RotCenter, self._RotAxis1, self._RotDegree)
    self.P_Extra_Point_Ball2.transform:RotateAround(self._RotCenter, self._RotAxis2, self._RotDegree)
    self.P_Extra_Point_Ball3.transform:RotateAround(self._RotCenter, self._RotAxis1, -self._RotDegree)
    self.P_Extra_Point_Ball4.transform:RotateAround(self._RotCenter, self._RotAxis4, self._RotDegree)
    self.P_Extra_Point_Ball5.transform:RotateAround(self._RotCenter, self._RotAxis5, self._RotDegree)
    self.P_Extra_Point_Ball6.transform:RotateAround(self._RotCenter, self._RotAxis6, self._RotDegree)
  end 
end

function P_Extra_Point:Active()
  ModulBase.Active(self)
  if not self._Actived then
    self.gameObject:SetActive(true)
    self.P_Extra_Point_Floor:SetActive(self._OnFloor)
    self.P_Extra_Point_Ball0:SetActive(true)
    self.P_Extra_Point_Ball1:SetActive(true)
    self.P_Extra_Point_Ball2:SetActive(true)
    self.P_Extra_Point_Ball3:SetActive(true)
    self.P_Extra_Point_Ball4:SetActive(true)
    self.P_Extra_Point_Ball5:SetActive(true)
    self.P_Extra_Point_Ball6:SetActive(true)
    self._FlyModUp = false
    self._FlyModFollow = false
  end
end
function P_Extra_Point:Deactive()
  ModulBase.Deactive(self)
  self._Rotate = false
  self.gameObject:SetActive(false)
end
function P_Extra_Point:Reset(type)
  if(type == 'levelRestart') then
    self._Actived = false
    ObjectStateBackupUtils.RestoreObject(self.P_Extra_Point_Ball1)
    ObjectStateBackupUtils.RestoreObject(self.P_Extra_Point_Ball2)
    ObjectStateBackupUtils.RestoreObject(self.P_Extra_Point_Ball3)
    ObjectStateBackupUtils.RestoreObject(self.P_Extra_Point_Ball4)
    ObjectStateBackupUtils.RestoreObject(self.P_Extra_Point_Ball5)
    ObjectStateBackupUtils.RestoreObject(self.P_Extra_Point_Ball6)
    self._P_Extra_Point_Ball_Fly1.Fly = false
    self._P_Extra_Point_Ball_Fly2.Fly = false
    self._P_Extra_Point_Ball_Fly3.Fly = false
    self._P_Extra_Point_Ball_Fly4.Fly = false
    self._P_Extra_Point_Ball_Fly5.Fly = false
    self._P_Extra_Point_Ball_Fly6.Fly = false
    for i = 1, 6, 1 do
      self['_P_Extra_Point_Ball_Rest'..i]()
    end
  end
end
function P_Extra_Point:Backup()
  ObjectStateBackupUtils.BackUpObject(self.P_Extra_Point_Ball1)
  ObjectStateBackupUtils.BackUpObject(self.P_Extra_Point_Ball2)
  ObjectStateBackupUtils.BackUpObject(self.P_Extra_Point_Ball3)
  ObjectStateBackupUtils.BackUpObject(self.P_Extra_Point_Ball4)
  ObjectStateBackupUtils.BackUpObject(self.P_Extra_Point_Ball5)
  ObjectStateBackupUtils.BackUpObject(self.P_Extra_Point_Ball6)
end

function P_Extra_Point:ActiveForPreview()
  self:Active()
end
function P_Extra_Point:DeactiveForPreview()
  self:Deactive()
end

function P_Extra_Point:BallEnterRange()
  if not self._Actived and not self._Rotate then --进入范围后才旋转
    self._Rotate = true
  end
end
function P_Extra_Point:BallLeaveRange()
  if not self._Actived and self._Rotate then --离开范围后停止旋转
    self._Rotate = false
  end
end

function CreateClass:P_Extra_Point()
  return P_Extra_Point()
end