Vector3 = UnityEngine.Vector3
Time = UnityEngine.Time
RenderSettings = UnityEngine.RenderSettings
Color = UnityEngine.Color
GameUIManager = GameManager.Instance:GetSystemService("GameUIManager") ---@type GameUIManager
GameSoundManager = GameManager.Instance:GetSystemService("GameSoundManager") ---@type GameSoundManager
GameSoundType = Ballance2.Sys.Services.GameSoundType
SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils


function CreateClass_CameraComtrol()

  ---@type GameLuaObjectHostClass
  local CameraComtrol = {
    I_Zone = nil, ---@type GameObject
    I_Zone_SuDu = nil, ---@type GameObject
    I_Zone_NenLi = nil, ---@type GameObject
    I_Zone_LiLiang = nil, ---@type GameObject
    I_Dome = nil, ---@type GameObject
    I_Light_Night = nil, ---@type GameObject
    I_Light_Day = nil, ---@type GameObject
    skyBox = nil, ---@type Skybox
    skyBoxNight = nil, ---@type Material
    skyBoxDay = nil, ---@type Material
    menuSound = nil,
    speed = -6,
    state = {
      isInLightZone = false,
      isRoatateCam = true,
    },
    domePosition = nil,
    transformI_Zone_SuDu = nil,
    transformI_Zone_NenLi = nil,
    transformI_Zone_LiLiang = nil,
  }

  function CameraComtrol:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function CameraComtrol:Start()
    self.domePosition = self.I_Dome.transform
    self.transformI_Zone_SuDu = self.I_Zone_SuDu.transform
    self.transformI_Zone_NenLi = self.I_Zone_NenLi.transform
    self.transformI_Zone_LiLiang = self.I_Zone_LiLiang.transform
    self.skyBoxNight = SkyBoxUtils.MakeSkyBox('D')
    self.skyBoxDay = SkyBoxUtils.MakeSkyBox('C')
    self.menuSound = GameSoundManager:RegisterSoundPlayer(GameSoundType.Background, GameSoundManager:LoadAudioResource('core.sounds.music:Menu_atmo.wav'), false, false, 'MenuSound')
    self.menuSound.loop = true
    self.menuSound:Play()
    self:SwitchLightZone(false)
  end
  function CameraComtrol:Update()
    if(self.state.isRoatateCam) then
			self.transform:RotateAround(self.domePosition, Vector3.up, Time.deltaTime * self.speed)
			self.transform:LookAt(self.domePosition)
    end
    if(self.state.isInLightZone) then 
      self.transformI_Zone_SuDu:LookAt(self.transform.position, Vector3.up)
      self.transformI_Zone_NenLi:LookAt(self.transform.position, Vector3.up)
      self.transformI_Zone_LiLiang:LookAt(self.transform.position, Vector3.up)
      self.transformI_Zone_SuDu.eulerAngles = Vector3(0, self.transformI_Zone_SuDu.eulerAngles.y, 0)
      self.transformI_Zone_NenLi.eulerAngles = Vector3(0, self.transformI_Zone_NenLi.eulerAngles.y, 0)
      self.transformI_Zone_LiLiang.eulerAngles = Vector3(0, self.transformI_Zone_LiLiang.eulerAngles.y, 0)
    end
  end
  function CameraComtrol:OnDisable()
    self.menuSound:Stop()
  end
  function CameraComtrol:OnEnable()
    self.menuSound:Play()
  end

  function CameraComtrol:SetFog(isLz) 
    RenderSettings.fog = true
    RenderSettings.fogDensity = 0.005
    if(isLz) then
      RenderSettings.fogColor = Color(0.180, 0.254, 0.301)
    else
      RenderSettings.fogColor = Color(0.745, 0.623, 0.384)
    end
  end
  function CameraComtrol:SwitchLightZone(on) 
    if(on) 
    then
      GameUIManager:MaskBlackSet(true)
      GameUIManager:MaskBlackFadeOut(1)
      self.I_Light_Night:SetActive(true)
      self.I_Light_Day:SetActive(false)
      self.I_Zone:SetActive(true)
      self.skyBox.material = self.skyBoxNight
      self.state.isInLightZone = true
      self:SetFog(true)
    else
      GameUIManager:MaskBlackSet(true)
      GameUIManager:MaskBlackFadeOut(1)
      self.I_Light_Day:SetActive(true)
      self.I_Light_Night:SetActive(false)
      self.I_Zone:SetActive(false)
      self.skyBox.material = self.skyBoxDay
      self.state.isInLightZone = false
      self:SetFog(false)
    end
  end

  return CameraComtrol:new(nil)
end