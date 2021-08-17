local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local GameManager = Ballance2.Sys.GameManager
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local GameSoundManager = GameManager.Instance:GetSystemService('GameSoundManager') ---@type GameSoundManager

---游戏管理器
---@class GamePlayManager : GameLuaObjectHostClass
GamePlayManager = ClassicObject:extend()

function GamePlayManager:new()

  self.GameLightGameObject = nil
  self.GameLightA = nil
  self.GameLightB = nil

  self.CurrentScore = 0 ---当前分数
  self.CurrentLife = 0 ---当前生命数
  self.CurrentSector = 0 ---当前小节

  
  self._IsCountDownScore = false

  GamePlay.GamePlayManager = self
end
function GamePlayManager:Start()
  self._SoundBallFall = GameSoundManager:RegisterSoundPlayer(GameSoundType.Normal, GameSoundManager:LoadAudioResource('core.sounds:Misc_Fall.wav'), false, true, 'Misc_Lightning')
  self._SoundAddLife = GameSoundManager:RegisterSoundPlayer(GameSoundType.Normal, GameSoundManager:LoadAudioResource('core.sounds:Misc_extraball.wav'), false, true, 'Misc_Lightning')
end
function GamePlayManager:OnDestroy()
  if (not Slua.IsNull(self.GameLightGameObject)) then UnityEngine.Object.Destroy(self.GameLightGameObject) end 
  self.GameLightGameObject = nil
end
function GamePlayManager:FixedUpdate()
  --分数每半秒减一
  if self._IsCountDownScore and self.CurrentScore > 0 then
    self.CurrentScore = self.CurrentScore - 1
    GamePlay.GamePlayUI:SetScoreText(self.CurrentScore)
  end
end

---初始化灯光和天空盒
---@param skyBoxPre string A-K 或者空，为空则使用 customSkyMat 材质
---@param customSkyMat Material 自定义天空盒材质
---@param lightColor Color 灯光颜色
function GamePlayManager:CreateSkyAndLight(skyBoxPre, customSkyMat, lightColor)
  Game.GamePlay.CamManager:SetSkyBox(customSkyMat or SkyBoxUtils.MakeSkyBox(skyBoxPre)) --Init sky

  if self.GameLightGameObject == nil then
    self.GameLightGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/GameLight.prefab'), 'GameLight')
    self.GameLightA = self.GameLightGameObject.transform:Find('Light'):GetComponent(UnityEngine.Light) ---@type Light
    self.GameLightB = self.GameLightGameObject.transform:Find('LightSecond'):GetComponent(UnityEngine.Light) ---@type Light
  end

  self.GameLightA.color = lightColor
  self.GameLightB.color = lightColor
end

---重新开始关卡
function GamePlayManager:RestartLevel() 
end
---退出关卡
function GamePlayManager:QuitLevel() 
end
---开始关卡
function GamePlayManager:StartLevel() 

end

---球坠落
function GamePlayManager:Fall() 

  --禁用控制
  GamePlay.BallManager:SetControllingStatus(BallControlStatus.UnleashingMode)
  --下落音乐
  self._SoundBallFall:Play()

  if self.CurrentLife > 0 then
    self.CurrentLife = self.CurrentLife - 1
    GamePlay.GamePlayUI:RemoveLifeBall()
    Game.UIManager:MaskWhiteFadeIn(1)

  else
    
  end
end

---添加生命
function GamePlayManager:AddLife() 
  self._SoundAddLife:Play()
  self.CurrentLife = self.CurrentLife + 1
  GamePlay.GamePlayUI:AddLifeBall()
end
---添加分数
---@param count number
function GamePlayManager:AddScore(count) 
  self.CurrentScore = self.CurrentScore + count
  GamePlay.GamePlayUI:SetScoreText(self.CurrentScore)
  GamePlay.GamePlayUI:TwinkleScore()
end

function CreateClass_GamePlayManager() return GamePlayManager() end