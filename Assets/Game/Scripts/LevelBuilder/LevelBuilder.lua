---@gendoc

require('AutoGroup')
local json = require('json') ---@type json

local GameErrorChecker = Ballance2.Services.Debug.GameErrorChecker
local GameError = Ballance2.Services.Debug.GameError
local GameSoundType = Ballance2.Services.GameSoundType
local GameLuaObjectHost = Ballance2.Services.LuaService.LuaWapper.GameLuaObjectHost
local GameSettingsManager = Ballance2.Services.GameSettingsManager
local GameEventNames = Ballance2.Base.GameEventNames
local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local DebugUtils = Ballance2.Utils.DebugUtils
local StringUtils = Ballance2.Utils.StringUtils
local Log = Ballance2.Log
local I18N = Ballance2.Services.I18N.I18N

local PhysicsObject = BallancePhysics.Wapper.PhysicsObject
local TiggerTester = Ballance2.Game.TiggerTester

local MeshRenderer = UnityEngine.MeshRenderer
local Application = UnityEngine.Application
local GUIUtility = UnityEngine.GUIUtility
local RenderSettings = UnityEngine.RenderSettings
local GameObject = UnityEngine.GameObject
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local WaitForEndOfFrame = UnityEngine.WaitForEndOfFrame
local MeshFilter = UnityEngine.MeshFilter
local BoxCollider = UnityEngine.BoxCollider
local MeshCollider = UnityEngine.MeshCollider
local Renderer = UnityEngine.Renderer

local TAG = 'LevelBuilder'

---@class LevelBuilderModulStorage
---@field go GameObject
---@field modul ModulBase
local LevelBuilderModulStorage = {}
---@class LevelBuilderModulRegStorage
---@field basePrefab GameObject
---@field name string
local LevelBuilderModulRegStorage = {}

---关卡加载器
---@class LevelBuilder : GameLuaObjectHostClass
---@field IsPreviewMode boolean 是否是预览模式
---@field _HalfTransparentMaterial Material
LevelBuilder = ClassicObject:extend()

function CreateClass:LevelBuilder()
  return LevelBuilder()
end

function LevelBuilder:new()
  self._RegisteredModuls = {} ---@type LevelBuilderModulRegStorage[]
  self._RegisteredLoadSteps = {}
  self._CurrentLevelJson = {}
  self._CurrentLevelModuls = {} ---@type LevelBuilderModulStorage[]
  self._CurrentLevelFloors = {} ---@type GameObject[]
  self._CurrentLevelPrefab = nil
  self._CurrentLevelObject = nil
  self._CurrentLevelAsset = nil ---@type LevelAssets
  self._IsLoading = false
end
function LevelBuilder:Start()
  Game.Mediator:RegisterEventHandler(Game.CorePackage, GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED, "LevelBuilder", function ()
    Game.Mediator:RegisterGlobalEvent('EVENT_LEVEL_BUILDER_JSON_LOADED')
    Game.Mediator:RegisterGlobalEvent('EVENT_LEVEL_BUILDER_MAIN_PREFAB_STANDBY')
    Game.Mediator:RegisterGlobalEvent('EVENT_LEVEL_BUILDER_START')
    Game.Mediator:RegisterGlobalEvent('EVENT_LEVEL_BUILDER_UNLOAD_START')

    self._LevelBuilderUI = Game.UIManager:InitViewToCanvas(Game.CorePackage:GetPrefabAsset('LevelBuilderUI.prefab'), 'GameLevelBuilderUI', true)
    self._LevelBuilderUIProgress = self._LevelBuilderUI:Find('Progress'):GetComponent(Ballance2.UI.Core.Controls.Progress) ---@type Progress
    self._LevelBuilderUITextErrorContent = self._LevelBuilderUI:Find('PanelFailed/ScrollView/Viewport/TextErrorContent'):GetComponent(UnityEngine.UI.Text) ---@type Text
    self._LevelBuilderUIPanelFailed = self._LevelBuilderUI:Find('PanelFailed').gameObject
    self._LevelBuilderUIButtonBack = self._LevelBuilderUI:Find('PanelFailed/ButtonBack'):GetComponent(UnityEngine.UI.Button) ---@type Button
    self._LevelBuilderUIButtonSubmitBug = self._LevelBuilderUI:Find('PanelFailed/ButtonSubmitBug'):GetComponent(UnityEngine.UI.Button) ---@type Button
    self._LevelBuilderUIButtonCopyErrInfo = self._LevelBuilderUI:Find('PanelFailed/ButtonCopyErrInfo'):GetComponent(UnityEngine.UI.Button) ---@type Button
    self._LevelBuilderUIPanelFailed.gameObject:SetActive(false)
    self._LevelBuilderUI.gameObject:SetActive(false)

    Game.CommandServer:RegisterCommand('lb', function (keyword, fullCmd, argsCount, args)
      local k2 = args[1]
      if k2 == 'quit' then
        Log.V(TAG, 'Unload')
        self:UnLoadLevel()
      elseif k2 == 'load' then
        if argsCount < 2 then
          Log.E(TAG, 'Missing arg 2')
        else
          Log.V(TAG, 'Load')
          Game.Mediator:NotifySingleEvent('CoreStartLoadLevel', { args[2] })
        end
      else
        Log.W(TAG, 'Unknow option '..type)
        return false
      end
      return true
    end, 0, 'lb <quit/load>\n'..
      '  <quit>                    ▶ 终止当前的关卡加载\n'..
      '  <load> <levelname:string> ▶ 加载指定关卡\n')

    self._LevelBuilderUIButtonBack.onClick:AddListener(function () 
      Game.UIManager:MaskBlackSet(true)
      Game.Manager:SetGameBaseCameraVisible(true)
      self._LevelBuilderUI.gameObject:SetActive(false)
      self:UnLoadLevel()  
    end)
    self._LevelBuilderUIButtonSubmitBug.onClick:AddListener(function () Application.OpenURL(ConstLinks.BugReportURL) end)  
    self._LevelBuilderUIButtonCopyErrInfo.onClick:AddListener(function () 
      GUIUtility.systemCopyBuffer = self._LevelBuilderCurrentError
      Game.UIManager:GlobalToast(I18N.Tr('core.ui.ErrorUIErrInfoCopied'))
    end)

    self._LevelLoaderNative = self.gameObject:GetComponent(Ballance2.Game.GameLevelLoaderNative) ---@type GameLevelLoaderNative
    self:UpdateErrStatus(false, nil)
    return false
  end)
end
function LevelBuilder:OnDestroy()
  if self.LevelBuilderUI ~= nil then
    UnityEngine.Object.Destroy(self.LevelBuilderUI.gameObject)
    self.LevelBuilderUI = nil
  end
end

---设置进度条百分比
---@param precent number
function LevelBuilder:UpdateLoadProgress(precent)
  self._LevelBuilderUIProgress.value = precent
end

---设置加载失败状态
---@param err boolean 是否失败
---@param statuaCode string 状态码
---@param errMessage string 错误信息
function LevelBuilder:UpdateErrStatus(err, statuaCode, errMessage)  
  
  if err then
    self._IsLoading = false

    Log.D(TAG, 'Load level error {0} err:  {1}', { statuaCode, errMessage })

    LuaTimer.Add(1000, function ()
      self._LevelBuilderCurrentError = 'Code: '..statuaCode..'\n'..errMessage
      self._LevelBuilderUITextErrorContent.text = self._LevelBuilderCurrentError
      self._LevelBuilderUIPanelFailed.gameObject:SetActive(true)
      Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal)
    end)
    
  else
    self._LevelBuilderUIPanelFailed.gameObject:SetActive(false)
  end
end

---开始加载关卡序列
---@param name string 关卡文件名
---@param preview boolean 是否是预览模式
function LevelBuilder:LoadLevel(name, preview)

  if self._IsLoading then
    Log.E(TAG, 'Level is loading! ')
    return
  end

  self._IsLoading = true
  self.IsPreviewMode = preview

  Log.D(TAG, 'Load level start ')
  if preview then
    Log.I(TAG, 'Load level in preview mode')
  end

  ---设置UI为初始状态
  self._LevelBuilderUI.gameObject:SetActive(true)
  self:UpdateLoadProgress(0)
  self:UpdateErrStatus(false, nil)
  Game.UIManager:MaskBlackSet(true)

  --加载内置模块
  InitBulitInModuls()

  if not preview then
    --加载物理环境
    GamePlay.GamePlayManager.GamePhysicsWorld:Create()
  end

  --发送开始事件
  Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_BEFORE_START', nil)

  --加载内置声音
  InitBulitInModulCustomSounds()

  --由C#代码加载文件
  self._LevelLoaderNative:LoadLevel(name, 
    ---加载文件就绪回调
    ---@param prefab GameObject
    ---@param jsonString string
    ---@param level LevelAssets
    function (prefab, jsonString, level)
      self._CurrentLevelAsset = level

      --加载基础数据
      local status, res = pcall(json.decode, jsonString)
      if not status then
        self:UpdateErrStatus(true, 'BAD_LEVEL_JSON', 'Failed to decode json, error: '..res)
        return
      end
      
      Log.D(TAG, 'Level asset loaded')

      self._CurrentLevelJson = res
      self._CurrentLevelPrefab = prefab
      Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_JSON_LOADED', { self._CurrentLevelJson })

      --检查基础适配
      local missedPackages = ''
      local requiredPackages = self._CurrentLevelJson.requiredPackages
      if type(requiredPackages) == "table" and #requiredPackages > 0 then
        for k, v in pairs(requiredPackages) do
          if v.name ~= nil and not Game.PackageManager.CheckRequiredPackage(v.name. v.minVersion or 0) then 
            missedPackages = missedPackages + '\nName： '..v.name..' Version：'..v.minVersion 
          end
        end
      end
      --模组适配
      if missedPackages ~= '' then
        self:UpdateErrStatus(true, 'DEPENDS_CHECK_FAILED', 'The level depends on the following module. The module may not be enabled or failed to load: '..missedPackages)
      end

      Log.D(TAG, 'Load level prefab')

      --载入Prefab
      self._CurrentLevelObject = Game.Manager:InstancePrefab(self._CurrentLevelPrefab, self.gameObject.transform, 'GameLevelMain')
      Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_MAIN_PREFAB_STANDBY', { self._CurrentLevelObject })

      --加载
      local errStack = ''
      local err = ''
      local status = xpcall(function () 
        self:_LoadLevelInternal() 
      end, function (errM)
        err = errM
        errStack = debug.traceback()
      end)
      if not status then
        self:UpdateErrStatus(true, 'LOADER_EXPECTION', 'Loading failed, exception information: \n'..err..'\n'..errStack)
      end
    end, 
    function (code, err)
      self:UpdateErrStatus(true, code, err)
    end)
end

---加载序列
function LevelBuilder:_LoadLevelInternal()

  --发送开始事件
  Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_START', nil)
  --加载基础设置
  local level = self._CurrentLevelJson.level
  local levelName = self._CurrentLevelJson.name

  local SectorManager = GamePlay.SectorManager
  local GamePlayManager = GamePlay.GamePlayManager

  self._CurrentLevelModuls = {}
  self._CurrentLevelSkyLayer = nil
  self._CurrentLevelFloors = {}
  
  Log.D(TAG, 'Pre load')
  --调用自定义加载步骤 pre
  self:_CallLoadStep("pre")
  self:CallLevelCustomModEvent('beforeLoad')
  if level.customModEventName and level.customModEventName ~= '' then
    Game.Mediator:RegisterGlobalEvent(level.customModEventName)
    Game.Mediator:DispatchGlobalEvent(level.customModEventName, { 'pre' })
  end

  Log.D(TAG, 'Check config')

  if self.IsPreviewMode and not self._CurrentLevelJson.allowPreview then
    self:UpdateErrStatus(true, 'BAD_CONFIG', 'Not support preview')
    return
  end
  --首先检查配置是否正确
  if level.sectorCount < 1 then
    self:UpdateErrStatus(true, 'BAD_CONFIG', 'There must be at least 1 sector in this level')
    return
  end
  if level.sectorCount > 32 then
    self:UpdateErrStatus(true, 'BAD_CONFIG', 'There are too many sectors (more than 32)')
    return
  end  
  if level.autoGroup == true then
    Log.D(TAG, 'Generate auto group')
    DoLevelAutoGroup(level, self._CurrentLevelObject.transform) --配置了 autoGroup 自动归组，则自动生成归组信息
  else
    if type(level.internalObjects) ~= "table" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects\' is invalid')
      return
    end
    if type(level.internalObjects.PS_LevelStart) ~= "string" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PS_LevelStart\' is invalid')
      return
    end
    if type(level.internalObjects.PE_LevelEnd) ~= "string" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PE_LevelEnd\' is invalid')
      return
    end  
    if type(level.internalObjects.PC_CheckPoints) ~= "table" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PC_CheckPoints\' is invalid')
      return
    end  
    if type(level.internalObjects.PR_ResetPoints) ~= "table" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PR_ResetPoints\' is invalid')
      return
    end
    if type(level.sectors) ~= "table" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'sectors\' is invalid')
      return
    end
    if type(level.floors) ~= "table" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'floors\' is invalid')
      return
    end  
    if type(level.groups) ~= "table" then
      self:UpdateErrStatus(true, 'BAD_CONFIG', '\'groups\' is invalid')
      return
    end
  end

  Log.D(TAG, 'Load level data')

  --首先填充基础信息（出生点、节、结尾）
  SectorManager.CurrentLevelSectorCount = level.sectorCount
  SectorManager.CurrentLevelSectors = {}
  SectorManager.CurrentLevelRestPoints = {}
  if not self.IsPreviewMode then
    GamePlayManager.NextLevelName = tostring(level.nextLevel) or ''
    GamePlayManager.StartBall = level.firstBall or 'BallWood'
    GamePlayManager.CurrentLevelName = self._CurrentLevelJson.name
    GamePlayManager.CurrentEndWithUFO = level.endWithUFO or false
  
    Log.D(TAG, 'Level Name: '..GamePlayManager.CurrentLevelName..'\nSectors: '..level.sectorCount)

    if type(level.defaultHighscoreData) == 'table' then
      Game.HighScoreManager.TryAddDefaultLevelHighScore(levelName, level.defaultHighscoreData)
    else
      Game.HighScoreManager.TryAddDefaultLevelHighScore(levelName, nil)
      Log.D(TAG, 'Not found user config defaultHighscoreData for this level, using system defaultHighscoreData')
    end

    if level.startLife and (level.startLife > 0 or level.startLife == -1) then
      GamePlayManager.StartLife = level.startLife
    end
    if level.startPoint and level.startPoint > 0 then
      GamePlayManager.StartPoint = level.startPoint
    end
    if level.levelScore and level.levelScore > 0 then
      GamePlayManager.LevelScore = level.levelScore
    end
    
  end

  self:UpdateLoadProgress(0.1)
  
  --加载
  local loadCo = coroutine.create(function()
    --加载内部对象
    -----------------------------

    Log.D(TAG, 'Load level internal objects')

    --加载出生点和火焰
    -----------------------------
    for i = 1, level.sectorCount, 1 do

      local flame = nil

      if i == 1 then
        flame = self:ReplacePrefab(level.internalObjects.PS_LevelStart, self:FindRegisterModul('PS_LevelStart'))
        if flame == nil then
          self:UpdateErrStatus(true, 'OBJECT_MISSING', 'Object \'PS_LevelStart\' is missing')
          return
        end
      else
        flame = self:ReplacePrefab(level.internalObjects.PC_CheckPoints[tostring(i)], self:FindRegisterModul('PC_CheckPoints'))
        if flame == nil then
          self:UpdateErrStatus(true, 'OBJECT_MISSING', 'Object \'PC_CheckPoints.'..i..'\' is missing')
          return
        end
      end

      local objName = level.internalObjects.PR_ResetPoints[tostring(i)]
      if objName == nil then
        self:UpdateErrStatus(true, 'FIELD_MISSING', 'Field \'level.internalObjects.PR_ResetPoints['..i..']\' is nil')
        return
      end

      local r = GameObject.Find(objName)
      if r == nil then
        self:UpdateErrStatus(true, 'OBJECT_MISSING', '\'level.internalObjects.PR_ResetPoints['..i..']\' => \''..objName..'\' not found')
        return
      end

      r:SetActive(false)

      SectorManager.CurrentLevelRestPoints[i] = {
        point = r,
        flame = flame
      }
      
    end
    --加载结尾
    -----------------------------
    SectorManager.CurrentLevelEndBalloon = self:ReplacePrefab(level.internalObjects.PE_LevelEnd, self:FindRegisterModul('PE_LevelEnd'))
    if SectorManager.CurrentLevelEndBalloon == nil then
      self:UpdateErrStatus(true, 'OBJECT_MISSING', '\'level.internalObjects.PE_LevelEnd\' => \''..level.internalObjects.PE_LevelEnd..'\' not found')
      return
    end

    Yield(WaitForSeconds(0.02))
    self:UpdateLoadProgress(0.2)

    --预览模式无须物理化这些
    if not self.IsPreviewMode then

      Log.D(TAG, 'Load level floors')

      ---加载 物理路面
      -----------------------------
      for _, floor in ipairs(level.floors) do
        
        local floorCount = 0
        local sleepCount = 0
        local physicsData = GamePhysFloor[floor.name] 
        if physicsData ~= nil then

          --StaticCompound
          local floorStatic = Game.Manager:InstanceNewGameObject(self.gameObject.transform, floor.name)
          table.insert(self._CurrentLevelFloors, floorStatic)
          
          --Floor childs
          for _, name in ipairs(floor.objects) do

            if sleepCount > 128 then
              Yield(WaitForSeconds(0.01))
              sleepCount = 0
            end

            local go = GameObject.Find(name)
            if go ~= nil then
              --Mesh
              local meshFilter = go:GetComponent(MeshFilter) ---@type MeshFilter
              if meshFilter ~= nil and meshFilter.mesh  ~= nil then
                go.transform:SetParent(floorStatic.transform)
                go.tag = floor.name
                go:AddComponent(MeshCollider)
                local body = go:AddComponent(PhysicsObject) ---@type PhysicsObject
                body.DoNotAutoCreateAtAwake = true
                body.Fixed = true
                body.Concave:Add(meshFilter.mesh)
                body.Friction = physicsData.Friction
                body.Elasticity = physicsData.Elasticity
                body.BuildRootConvexHull = false
                body.Layer = physicsData.Layer
                body.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName(physicsData.CollisionLayerName)
                body:Physicalize()
                if go:GetComponent(MeshCollider) == nil then
                  go:AddComponent(MeshCollider)
                end

                if physicsData.HitSound then
                  local hitSound = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, physicsData.HitSound, false, true, 'Floor_'..name..'_HitSound')
                  
                  body:EnableContractEventCallback()
                  body.EnableCollisionEvent = true
                  body:AddCollDetection(GamePlay.BallSoundManager:GetSoundCollIDByName('WoodenFlap'), 0.2, 10, 0.5, 0.1)
                  body:AddCollDetection(GamePlay.BallSoundManager:GetSoundCollIDByName('Wood'), 0.2, 10, 0.5, 0.1)
                  body:AddCollDetection(GamePlay.BallSoundManager:GetSoundCollIDByName('WoodOnlyHit'), 0.2, 10, 0.5, 0.1)
                  --撞击处理回调
                  ---@param col_id number
                  ---@param speed_precent number
                  body.OnPhysicsCollDetection = function (_, col_id, speed_precent)
                    hitSound.volume = speed_precent
                    hitSound:Play()
                  end
                end
              else
                Log.W(TAG, 'Not found MeshFilter or mesh in floor  \''..name..'\'')
              end
              floorCount = floorCount + 1
              sleepCount = sleepCount + 1
            else
              Log.W(TAG, 'Not found floor  \''..name..'\' in type \''..floor.name..'\'')
            end
          end

          if floorCount == 0 then
            ---没有路面，则隐藏当前静态父级
            floorStatic:SetActive(false)
          else
            Log.D(TAG, 'Loaded floor '..floor.name..' count: '..floorCount)
          end
          
        else
          Log.E(TAG, 'Unknow floor type \''..floor.name..'\'')
        end
      end

      self:UpdateLoadProgress(0.3)

      --加载坠落检测区
      -----------------------------
      for _, name in ipairs(level.depthTestCubes) do
        local go = GameObject.Find(name)
        if go ~= nil then

          --禁用Renderer使物体隐藏
          local renderer = go:GetComponent(Renderer) ---@type Renderer
          if renderer ~= nil then renderer.enabled = false end
          
          --添加坠落检测区Mesh
          local collider = go:AddComponent(BoxCollider) ---@type BoxCollider
          local tigger = go:AddComponent(TiggerTester) ---@type TiggerTester
          
          collider.isTrigger = true
          ---@param other GameObject
          tigger.onTriggerEnter = function (_, other)
            --触发球坠落
            if other.tag == 'Ball' then
              GamePlayManager:Fall()
            end
          end
        else
          Log.W(TAG, 'Not found object \''..name..'\' in depthTestCubes')
        end
      end
        
    else
      --加载坠落检测区信息
      -----------------------------

      local depthTestCubes = {}
      for _, name in ipairs(level.depthTestCubes) do
        local go = GameObject.Find(name)
        if go ~= nil then

          --添加边框材质
          local renderer = go:GetComponent(MeshRenderer) ---@type MeshRenderer
          if renderer == nil then 
            renderer = go:AddComponent(MeshRenderer) ---@type MeshRenderer
          end
          
          renderer.material = self._HalfTransparentMaterial

          table.insert(depthTestCubes, go)
        else
          Log.W(TAG, 'Not found object \''..name..'\' in depthTestCubes')
        end
      end

      GamePlay.GamePreviewManager.GameDepthTestCubes = depthTestCubes
    end

    self:UpdateLoadProgress(0.4)

    Log.D(TAG, 'Load level moduls')

    --调用自定义加载步骤 modul
    -----------------------------
    self:_CallLoadStep("modul")
    self:UpdateLoadProgress(0.5)

    --加载 modul
    -----------------------------
    local tickCount = 0
    for _, group in ipairs(level.groups) do
      local modul = self:FindRegisterModul(group.name)
      if modul ~= nil then
        
        local modulCount = 0
        local modulFailedCount = 0

        Log.D(TAG, 'Load modul '..group.name)

        for _, name in ipairs(group.objects) do

          if tickCount > 16 then
            Yield(WaitForSeconds(0.02))
            tickCount = 0
          end

          local m = self:ReplacePrefab(name, modul)
          if m ~= nil then
            tickCount = tickCount + 1
            modulCount = modulCount + 1
          else
            modulFailedCount = modulFailedCount + 1
          end 
        end

        Log.D(TAG, 'Loaded modul '..group.name..' count : '..modulCount..(modulFailedCount > 0 and (' failed count: '..modulFailedCount) or ''))
        if modulFailedCount > 0 then
          Log.W(TAG, 'Load failed modul '..group.name..' count : '..modulFailedCount)
        end
      else
        Log.W(TAG, 'Modul \''..group.name..'\' not register')
      end

    end

    self:UpdateLoadProgress(0.6)

    Log.D(TAG, 'Load init moduls')

    --首次加载 modul
    -----------------------------
    Yield(WaitForSeconds(0.02))

    --预览模式无须备份机关信息
    if not self.IsPreviewMode then
      SectorManager:DoInitAllModuls()
    end

    self:UpdateLoadProgress(0.7)

    Log.D(TAG, 'Load level sector data')

    --填充节数据
    -----------------------------  
    for i = 1, level.sectorCount, 1 do
      local sector = level.sectors[tostring(i)]
      if sector == nil then
        Log.W(TAG, 'Sector key \''..i..'\' not found')
      else
        local moduls = {}
        for _, name in ipairs(sector) do
          local modul = self._CurrentLevelModuls[name]
          if modul ~= nil then
            table.insert(moduls, modul.modul)
          else
            Log.W(TAG, 'Not found modul \''..name..'\' in sectors.'..i)
          end
        end
        SectorManager.CurrentLevelSectors[i] = {
          moduls = moduls
        }
      end
    end

    self:UpdateLoadProgress(0.8)

    Yield(WaitForSeconds(0.02))
    Log.D(TAG, 'Load sky and light')

    --加载天空盒和灯光
    -----------------------------
    RenderSettings.fog = false
    if level.skyBox == 'custom' then
      --加载自定义天空盒
      if type(level.customSkyBox) ~= 'table' then
        Log.E(TAG, 'The skyBox is set to \'custom\', but \'customSkyBox\' field is invalid')
      else
        Log.D(TAG, 'Load custom SkyBox')

        local B = self._CurrentLevelAsset:GetTextureAsset(level.customSkyBox.B)
        local F = self._CurrentLevelAsset:GetTextureAsset(level.customSkyBox.F)
        local L = self._CurrentLevelAsset:GetTextureAsset(level.customSkyBox.L)
        local R = self._CurrentLevelAsset:GetTextureAsset(level.customSkyBox.R)
        local T = self._CurrentLevelAsset:GetTextureAsset(level.customSkyBox.T)
        local D = self._CurrentLevelAsset:GetTextureAsset(level.customSkyBox.D)
        if B == nil then Log.W(TAG, 'Failed to load customSkyBox.B texture') end
        if F == nil then Log.W(TAG, 'Failed to load customSkyBox.F texture') end
        if L == nil then Log.W(TAG, 'Failed to load customSkyBox.L texture') end
        if R == nil then Log.W(TAG, 'Failed to load customSkyBox.R texture') end
        if D == nil then Log.W(TAG, 'Failed to load customSkyBox.D texture') end

        local skyMat = SkyBoxUtils.MakeCustomSkyBox(L, R, F, B, D, T)
        if self.IsPreviewMode then
          GamePlay.GamePreviewManager:CreateSkyAndLight('', skyMat, level.lightColor)
        else
          GamePlayManager:CreateSkyAndLight('', skyMat, level.lightColor)
        end
      end
    elseif type(level.skyBox) == "string" then
      --使用自带天空盒 
      if self.IsPreviewMode then
        GamePlay.GamePreviewManager:CreateSkyAndLight(level.skyBox, nil, StringUtils.StringToColor(level.lightColor))
      else
        GamePlayManager:CreateSkyAndLight(level.skyBox, nil, StringUtils.StringToColor(level.lightColor))
      end
    else
      Log.E(TAG, 'Invalid field \'level.skyBox\': '..DebugUtils.PrintLuaVarAuto(level.skyBox, 1))
    end

    --加载SkyLayer
    -----------------------------
    if level.skyLayer == 'SkyLayer' then
      local oldSkyLayer = GameObject.Find('SkyLayer')
      if oldSkyLayer ~= nil then
        self._CurrentLevelSkyLayer = Game.Manager:InstancePrefab(Game.CorePackage:GetPrefabAsset('FinalSkyLayer.prefab'), 'SkyLayer')
        self._CurrentLevelSkyLayer.transform.position = oldSkyLayer.transform.position
        self._CurrentLevelSkyLayer.transform.rotation = oldSkyLayer.transform.rotation
        self._CurrentLevelSkyLayer.transform.localScale = oldSkyLayer.transform.localScale
        oldSkyLayer:SetActive(false)
        Log.D(TAG, 'Load SkyLayer object')
      else
        Log.W(TAG, 'Not found \'level.skyLayer\': SkyLayer object.')
      end
    elseif level.skyLayer == 'SkyVoterx' then
      local oldSkyLayer = GameObject.Find('SkyVoterx')
      if oldSkyLayer ~= nil then
        self._CurrentLevelSkyLayer = Game.Manager:InstancePrefab(Game.CorePackage:GetPrefabAsset('FinalSkyVoterx.prefab'), 'SkyVoterx')
        self._CurrentLevelSkyLayer.transform.position = oldSkyLayer.transform.position
        self._CurrentLevelSkyLayer.transform.rotation = oldSkyLayer.transform.rotation
        self._CurrentLevelSkyLayer.transform.localScale = oldSkyLayer.transform.localScale
        oldSkyLayer:SetActive(false)
        Log.D(TAG, 'Load SkyVoterx object')
      else
        Log.W(TAG, 'Not found \'level.skyLayer\': SkyVoterx object.')
      end
    else
      Log.D(TAG, 'No SkyLayer')
    end
    local GameSettings = GameSettingsManager.GetSettings("core")
    --如果设置禁用了云层，则隐藏
    if not GameSettings:GetBool('video.cloud', true) then
      self._CurrentLevelSkyLayer:SetActive(false)
    end

    Log.D(TAG, 'Load music')

    --加载自定义音乐
    -----------------------------
    local customMusicTheme = level.customMusicTheme
    if type(customMusicTheme) == 'table' then
      if type(customMusicTheme.id) == 'nil' then
        Log.E(TAG, 'The \'customMusicTheme.id\' field is not set')
      else

        local loadCustomAudio = function (name, orgArr)
          local arr = {}
          if type(orgArr) == 'table' then
            for index, value in ipairs(orgArr) do
              local audio = self._CurrentLevelAsset:GetAudioClipAsset(value)
              if audio ~= nil then
                table.insert(arr, audio)
              else
                audio = Game.PackageManager:GetAudioClipAsset(value)
                if audio ~= nil then
                  table.insert(arr, audio)
                else
                  Log.W(TAG, 'Not found custom audio resource in customMusicTheme.'..name..'.'..index..' , name : '..value..' , ignore this sound')
                end
              end
            end
          end
          return arr
        end
        local id = tonumber(customMusicTheme.id)

        Log.D(TAG, 'Load customMusicTheme '..id)

        GamePlay.MusicManager.Musics[id] = {
          atmos = loadCustomAudio(customMusicTheme.atmos),
          musics = loadCustomAudio(customMusicTheme.musics),
          baseInterval = customMusicTheme.baseInterval or 5,
          maxInterval = customMusicTheme.baseInterval or 30,
          atmoInterval = customMusicTheme.baseInterval or 6,
          atmoMaxInterval = customMusicTheme.baseInterval or 15,
        }
      end
    end
    --设置音乐
    if type(level.musicTheme) == "number" then
      Log.D(TAG, 'Set MusicTheme '..level.musicTheme)
      GamePlay.MusicManager:SetCurrentTheme(level.musicTheme)
    else
      Log.D(TAG, 'No MusicTheme')
      GamePlay.MusicManager:SetCurrentTheme(0)
    end

    self:UpdateLoadProgress(0.9)
    Yield(WaitForEndOfFrame())

    Log.D(TAG, 'Load others')

    --加载第一关教程
    -----------------------------
    if levelName == 'level01' then
      Game.Manager:InstancePrefab(Game.CorePackage:GetPrefabAsset('TutorialCore.prefab'), 'GameTutorialCore')
    end

    --调用自定义加载步骤 last
    -----------------------------
    self:_CallLoadStep("last")
    self:CallLevelCustomModEvent('finishLoad')

    Log.D(TAG, 'Load finish')

    self:UpdateLoadProgress(1)
    Yield(WaitForSeconds(0.5))

    --最后加载步骤
    -----------------------------
    if self.IsPreviewMode then
      Game.Mediator:DelayedNotifySingleEvent('CoreGamePreviewManagerInitAndStart', 0.1, {
        self._CurrentLevelJson.name,
        self._CurrentLevelJson.author,
        self._CurrentLevelJson.version
      })
    else
      Game.Mediator:DelayedNotifySingleEvent('CoreGamePlayManagerInitAndStart', 0.1, {})
    end

    Yield(WaitForSeconds(0.2))

    --隐藏加载UI
    Game.UIManager:MaskBlackSet(true)
    self._LevelBuilderUI.gameObject:SetActive(false)
    self._IsLoading = false
  end)
  local status = coroutine.resume(loadCo)

  if not status then
    Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal)
  else
    ---检查 coroutine 是否已经停止但是状态还是未设置
    local checkTime = 0
    self.checkTimeId = LuaTimer.Add(1000, 1000, function ()
      self.checkTime = checkTime + 1
      if checkTime > 10 then 
        LuaTimer.Delete(self.checkTimeId ) 
        self.checkTimeId = nil
      end
      if self._IsLoading and coroutine.status(loadCo) == "dead" then
        self:UpdateErrStatus(true, 'LOADER_STOPPED', debug.traceback(loadCo))
      end
    end)
  end
end

---替换占位符并生成机关
---@param objName string 占位符的名称
---@param modulPrefab LevelBuilderModulRegStorage 机关预制体
---@return ModulBase|nil 返回机关类，如果出现错误则返回nil
function LevelBuilder:ReplacePrefab(objName, modulPrefab)
  --检查同名机关
  if self._CurrentLevelModuls[objName] ~= nil then
    Log.E(TAG, 'Find modul with the same name \''..objName..'\'')
    return nil
  end
  local obj = GameObject.Find(objName)
  if obj == nil then 
    Log.E(TAG, 'Not find modul placeholder \''..objName..'\'')
    return nil 
  end

  --隐藏占位符
  obj:SetActive(false) 
  --克隆机关
  local modul = Game.Manager:InstancePrefab(modulPrefab.basePrefab, self.gameObject.transform, "ModulInstance_"..objName)
  --同步机关位置
  modul.transform.position = obj.transform.position
  modul.transform.rotation = obj.transform.rotation
  --获取类
  local modulClass = GameLuaObjectHost.GetLuaClassFromGameObject(modul) ---@type ModulBase 
  if not modulClass then
    Log.E(TAG, 'Not find ModulBase class on modul \''..objName..'\'')
    return nil
  end

  modulClass.IsPreviewMode = self.IsPreviewMode
  self._CurrentLevelModuls[objName] = {
    go = modul,
    modul = modulClass
  }
  --获取类
  return modulClass
end

---卸载当前加载的关卡
---@param endCallback function|nil 完成回调
function LevelBuilder:UnLoadLevel(endCallback)
  
  if self._IsLoading then
    Log.E(TAG, 'Level is loading! ')
    return
  end

  if self.checkTimeId then 
    LuaTimer.Delete(self.checkTimeId) 
    self.checkTimeId = nil
  end

  self._IsLoading = true
  --
  coroutine.resume(coroutine.create(function()

    --发送开始事件
    Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_UNLOAD_START', nil)

    Log.D(TAG, 'UnLoad moduls')

    --通知所有modul卸载
    GamePlay.SectorManager:DoUnInitAllModuls()
    
    --通知关卡自定义回调卸载
    if self._CurrentLevelJson and self._CurrentLevelJson.level then
      local level = self._CurrentLevelJson.level
      self:_CallLoadStep('unload')
      if level.customModEventName and level.customModEventName ~= '' then
        Game.Mediator:DispatchGlobalEvent(level.customModEventName, { 'unload' })
        Game.Mediator:UnRegisterGlobalEvent(level.customModEventName)
      end
    end

    Yield(WaitForEndOfFrame())

    Log.D(TAG, 'Clear all')

    --清空数据
    GamePlay.SectorManager:ClearAll() 

    --删除音乐数据
    if self._CurrentLevelJson then
      local customMusicTheme = self._CurrentLevelJson.customMusicTheme
      if type(customMusicTheme) == 'table' and type(customMusicTheme.id) == 'number' then
        GamePlay.MusicManager.Musics[customMusicTheme.id] = nil
      end
    end
    GamePlay.MusicManager:SetCurrentTheme(1)
    --停止所有背景声音
    GamePlay.BallSoundManager:StopAllSound()

    --删除所有modul
    local tickCount = 0
    for _, value in pairs(self._CurrentLevelModuls) do
      if value ~= nil then 
        if tickCount > 16 then
          Yield(WaitForEndOfFrame())
          tickCount = 0
        end
        UnityEngine.Object.Destroy(value.go)
        tickCount = tickCount + 1
      end
    end

    --删除路面
    for _, value in pairs(self._CurrentLevelFloors) do
      if value ~= nil then 
        UnityEngine.Object.Destroy(value)
      end
    end

    --清空天空和云层
    if self.IsPreviewMode then
      GamePlay.GamePreviewManager:HideSkyAndLight()
    else
      GamePlay.GamePlayManager:HideSkyAndLight()
    end
    if self._CurrentLevelSkyLayer ~= nil then
      UnityEngine.Object.Destroy(self._CurrentLevelSkyLayer)
      self._CurrentLevelSkyLayer = nil
    end

    --删除关卡元件
    UnityEngine.Object.Destroy(self._CurrentLevelObject)

    --清空变量
    self._CurrentLevelObject = nil
    self._CurrentLevelJson = nil
    self._CurrentLevelPrefab = nil
    self._CurrentLevelModuls = {}
    self._CurrentLevelFloors = {}

    Log.D(TAG, 'Unload level asset')

    --卸载AssetBundle
    self._LevelLoaderNative:UnLoadLevel(self._CurrentLevelAsset)
    self._CurrentLevelAsset = nil

    --删除关卡中所有的物理碰撞信息
    if not self.IsPreviewMode and GamePlay.GamePlayManager and GamePlay.GamePlayManager.GamePhysicsWorld then
      GamePlay.GamePlayManager.GamePhysicsWorld:Destroy()
    end

    Log.D(TAG, 'Unload level finish')

    self._IsLoading = false

    if type(endCallback) == 'function' then
      endCallback()
    else
      --通知回到menulevel
      Game.Manager:RequestEnterLogicScense('MenuLevel')
    end
  end))
end

---注册机关
---@param name string 机关名称
---@param basePrefab GameObject 机关的基础Prefab
function LevelBuilder:RegisterModul(name, basePrefab)
  if self._RegisteredModuls[name] ~= nil then
    GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, 'Modul {0} already registered! ', { name })
    return
  end
  if basePrefab == nil then
    GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG, 'Failed to rgister modul {0}, basePrefab is null', { name })
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
function LevelBuilder:FindRegisterModul(name) 
  return self._RegisteredModuls[name] 
end

---注册自定义加载步骤
---@param name string 名称
---@param callback function(levelBuilder, type) 回调， 第一个参数为 levelBuilder 实例；第二个参数是当前步骤的类型。
---@param type "pre"|"modul"|"last"|"unload" 回调类型
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

function LevelBuilder:CallLevelCustomModEvent(type) 
  local level = self._CurrentLevelJson.level
  if level and level.customModEventName and level.customModEventName ~= '' then
    Game.Mediator:DispatchGlobalEvent(level.customModEventName, { type })
  end
end
function LevelBuilder:_CallLoadStep(type)
  for _, value in pairs(self._RegisteredLoadSteps) do
    if value ~= nil and value.type == type then
      value.callback(self, type)
    end
  end
end
