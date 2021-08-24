local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local GameErrorChecker = Ballance2.Sys.Debug.GameErrorChecker
local GameError = Ballance2.Sys.Debug.GameError
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local Log = Ballance2.Utils.Log
local json = Game.SystemPackage:RequireLuaFile('json') ---@type json

local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

local TAG = 'LevelBuilder'

---关卡建造器
---@class LevelBuilder : GameLuaObjectHostClass
LevelBuilder = ClassicObject:extend()

function CreateClass_LevelBuilder()
  return LevelBuilder()
end

function LevelBuilder:new()
  self._RegisteredModuls = {}
  self._RegisteredLoadSteps = {}
  self._CurrentLevelJson = {}
  self._CurrentLevelPrefab = nil
  self._CurrentLevelObject = nil
  self._CurrentLevelAssetBundle = nil
end
function LevelBuilder:Start()
  self._LevelBuilderUI = Game.UIManager:InitViewToCanvas(Game.PackageManager:FindPackage('core.ui'):GetPrefabAsset('LevelBuilderUI.prefab'), 'GameLevelBuilderUI', false)
  self._LevelBuilderUIProgress = self._LevelBuilderUI:FindChild('Progress'):GetComponent(Ballance2.Sys.UI.Progress) ---@type Progress
  self._LevelBuilderUITextErrorContent = self._LevelBuilderUI:FindChild('PanelFailed/ScrollView/Viewport/TextErrorContent'):GetComponent(UnityEngine.UI.Text) ---@type Text
  self._LevelBuilderUIPanelFailed = self._LevelBuilderUI:FindChild('PanelFailed').gameObject
  self._LevelBuilderUIButtonBack = self._LevelBuilderUI:FindChild('PanelFailed/ButtonBack'):GetComponent(UnityEngine.UI.Button) ---@type Button
  self._LevelBuilderUIPanelFailed.gameObject:SetActive(false)
  self._LevelBuilderUI.gameObject:SetActive(false)
  self._LevelBuilderUIButtonBack.onClick:AddListener(function () self:UnLoadLevel()  end)
  self._LevelLoaderNative = Game.Manager:InstancePrefab(Game.SystemPackage:GetPrefabAsset('LevelBuilderUI.prefab'), 'GameLevelLoaderNative'):GetComponent(Ballance2.Game.GameLevelLoaderNative) ---@type GameLevelLoaderNative
end
function LevelBuilder:OnDestroy()
  if self.LevelBuilderUI ~= nil then
    UnityEngine.Object.Destroy(self.LevelBuilderUI.gameObject)
    self.LevelBuilderUI = nil
  end
end

---设置进度条百分比
---@param precent number
function LevelBuilder:_UpdateLoadProgress(precent)
  self._LevelBuilderUIProgress.value = precent
end
---设置加载失败状态
---@param err boolean
---@param statuaCode string
---@param errMessage string
function LevelBuilder:_UpdateErrStatus(err, statuaCode, errMessage)  
  self._LevelBuilderUIPanelFailed.gameObject:SetActive(err)
  if err then
    self._LevelBuilderUITextErrorContent.text = 'Code: '..statuaCode..'\n'..errMessage
    Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal)
  end
end

---加载关卡
---@param name string 关卡文件名
function LevelBuilder:LoadLevel(name)

  ---设置UI为初始状态
  self._LevelBuilderUI.gameObject:SetActive(true)
  self:_UpdateLoadProgress(0)
  self:_UpdateErrStatus(false, nil)
  Game.UIManager:MaskBlackSet(false)

  --由C#代码加载文件
  self._LevelLoaderNative:LoadLevel(name, 
    ---加载文件就绪回调
    ---@param prefab GameObject
    ---@param jsonString string
    ---@param assetBundle any
    function (prefab, jsonString, assetBundle)
      self._CurrentLevelAssetBundle = assetBundle

      --加载基础数据
      local status, res = pcall(json.decode, jsonString)
      if not status then
        self:_UpdateErrStatus(true, 'BAD_LEVEL_JSON', 'Failed to decode json, error: '..res)
        return
      end

      self._CurrentLevelJson = res
      self._CurrentLevelPrefab = prefab
      Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_JSON_LOADED', '*', { self._CurrentLevelJson })

      --检查基础适配
      local missedPackages = ''
      local requiredPackages = self._CurrentLevelJson.requiredPackages
      if type(requiredPackages) == "table" and #requiredPackages > 0 then
        for k, v in pairs(requiredPackages) do
          if v.name ~= nil and not Game.PackageManager.CheckRequiredPackage(v.name. v.minVersion or 0) then 
            missedPackages = missedPackages + '\n包名： '..v.name..' 版本：'..v.minVersion 
          end
        end
      end
      --模组适配
      if missedPackages ~= '' then
        self:_UpdateErrStatus(true, 'DEPENDS_CHECK_FAILED', '关卡依赖以下此模组，可能是模组未启用或加载失败: '..missedPackages)
      end

      --载入Prefab
      self._CurrentLevelObject = Game.Manager:InstancePrefab(self._CurrentLevelPrefab, self.gameObject.transform, 'GameLevelMain')
      Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_MAIN_PREFAB_STANDBY', '*', { self._CurrentLevelObject })

      --加载
      local status, rs = pcall(function () self:_LoadLevelInternal() end)
      if not status then
        self:_UpdateErrStatus(true, 'LOADER_EXPECTION', '加载失败，异常信息: '..rs)
      end
    end, 
    function (code, err)
      self:_UpdateErrStatus(true, code, err)
    end)
end
function LevelBuilder:_LoadLevelInternal()

  --发送开始事件
  Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_START', '*')
  --加载基础设置
  local level = self._CurrentLevelJson.level
  
  --调用自定义加载步骤 pre
  self:_CallLoadStep("pre")

  --首先填充基础信息（出生点、节、结尾）
  if level.sectorCount ~= #level.sectors then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', 'level.sectorCount ~= #level.sectors')
    return
  end
  if level.sectorCount < 1 then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '该关卡至少要存在1个节')
    return
  end
  if level.sectorCount > 256 then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '该关卡节太多（超过256）')
    return
  end

  GamePlay.SectorManager.CurrentLevelSectorCount = level.sectorCount
  GamePlay.SectorManager.CurrentLevelSectors = {}
  GamePlay.SectorManager.CurrentLevelRestPoints = {}
  
  --首先检查配置是否正确
  if type(level.internalObjects) ~= "table" then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects\' 字段错误')
    return
  end
  if type(level.internalObjects.PS_LevelStart) ~= "string" then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PS_LevelStart\' 字段错误')
    return
  end
  if type(level.internalObjects.PE_LevelEnd) ~= "string" then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PE_LevelEnd\' 字段错误')
    return
  end  
  if type(level.internalObjects.PC_CheckPoints) ~= "table" then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PC_CheckPoints\' 字段错误')
    return
  end  
  if type(level.internalObjects.PR_ResetPoints) ~= "table" then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PR_ResetPoints\' 字段错误')
    return
  end
  if #level.internalObjects.PR_ResetPoints ~= level.sectorCount then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PR_ResetPoints\' 字段数量与 sectorCount 节数不一致')
    return
  end
  if #level.internalObjects.PC_CheckPoints < level.sectorCount - 1 then
    self:_UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PC_CheckPoints\' 字段数量少于 sectorCount 节数所需的数量')
    return
  end
  --加载内部对象
  



  --填充节数据
  for _, value in ipairs(level.sectors) do
    table.insert(GamePlay.SectorManager.CurrentLevelSectors, {
      moduls = {}
    })
  end


  --调用自定义加载步骤 modul
  self:_CallLoadStep("modul")

  --加载天空盒和灯光
  if level.skyBox == 'custom' then
    --加载自定义天空盒
    if type(level.customSkyBox) ~= 'table' then
      Log.E(TAG, 'The skyBox is set to \'custom\', but \'customSkyBox\' field is not set')
      return
    end

    local B = self._LevelLoaderNative:GetTextureAsset(self._CurrentLevelAssetBundle, level.customSkyBox.B)
    local F = self._LevelLoaderNative:GetTextureAsset(self._CurrentLevelAssetBundle, level.customSkyBox.F)
    local L = self._LevelLoaderNative:GetTextureAsset(self._CurrentLevelAssetBundle, level.customSkyBox.L)
    local R = self._LevelLoaderNative:GetTextureAsset(self._CurrentLevelAssetBundle, level.customSkyBox.R)
    local T = self._LevelLoaderNative:GetTextureAsset(self._CurrentLevelAssetBundle, level.customSkyBox.T)
    local D = self._LevelLoaderNative:GetTextureAsset(self._CurrentLevelAssetBundle, level.customSkyBox.D)
    if B == nil then Log.W(TAG, 'Failed to load customSkyBox.B texture') end
    if F == nil then Log.W(TAG, 'Failed to load customSkyBox.F texture') end
    if L == nil then Log.W(TAG, 'Failed to load customSkyBox.L texture') end
    if R == nil then Log.W(TAG, 'Failed to load customSkyBox.R texture') end
    if D == nil then Log.W(TAG, 'Failed to load customSkyBox.D texture') end

    local skyMat = SkyBoxUtils.MakeCustomSkyBox(L, R, F, B, D, T)
    GamePlay.GamePlayManager:CreateSkyAndLight('', skyMat, level.lightColor)
  else
    --使用自带天空盒 
    GamePlay.GamePlayManager:CreateSkyAndLight(level.skyBox, nil, level.lightColor)
  end

  --调用自定义加载步骤 last
  self:_CallLoadStep("last")

  coroutine.resume(coroutine.create(function()
    Yield(WaitForSeconds(5))
  end))
end

---卸载当前加载的关卡
function LevelBuilder:UnLoadLevel()
  --TODO:卸载当前加载的关卡
end

---注册机关
---@param name string 机关名称
---@param basePrefab GameObject 机关的基础Prefab
function LevelBuilder:RegisterModul(name, basePrefab)
  if self._RegisteredModuls[name] ~= nil then
    GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, 'Modul {0} already registered! ', { name })
    return
  end

  self._RegisteredModuls[name] = {
    name = name,
    basePrefab = basePrefab,
  }
end
---取消注册机关
---@param name string 机关名称
function LevelBuilder:UnRegisterModul(name)
  self._RegisteredModuls[name] = nil
end
---获取注册的机关，如果没有注册，则返回nil
---@param name string 机关名称
function LevelBuilder:FindRegisterModul(name) return self._RegisteredModuls[name] end
---注册自定义加载步骤
---@param name string 名称
---@param callback function 回调
---@param type "pre"|"modul"|"last" 回调
function LevelBuilder:RegisterLoadStep(name, callback, type)
  if self._RegisteredLoadSteps[name] ~= nil then
    GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, 'LoadStep {0} already registered! ', { name })
    return
  end

  self._RegisteredLoadSteps[name] = {
    type = type,
    callback = callback
  }
end
---取消注册自定义加载步骤
---@param name string 名称
function LevelBuilder:UnRegisterLoadStep(name)
  self._RegisteredLoadSteps[name] = nil
end
function LevelBuilder:_CallLoadStep(type)
  for _, value in pairs(self._RegisteredLoadSteps) do
    if value ~= nil and value.type == type then
      value.callback()
    end
  end
end
