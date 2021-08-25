local GameErrorChecker = Ballance2.Sys.Debug.GameErrorChecker
local GameError = Ballance2.Sys.Debug.GameError
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local GameLuaObjectHost = Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost
local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local Log = Ballance2.Utils.Log
local json = Game.SystemPackage:RequireLuaFile('json') ---@type json

local PhysicsBody = PhysicsRT.PhysicsBody
local PhysicsShape = PhysicsRT.PhysicsShape
local PhysicsPhantom = PhysicsRT.PhysicsPhantom
local ShapeType = PhysicsRT.ShapeType
local MotionType = PhysicsRT.MotionType

local GameObject = UnityEngine.GameObject
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local MeshFilter = UnityEngine.MeshFilter
local Renderer = UnityEngine.Renderer

local TAG = 'LevelBuilder'

---@class LevelBuilderModulStorage
---@field go GameObject
---@field modul ModulBase
local LevelBuilderModulStorage = {}

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
  self._CurrentLevelModuls = {} ---@type LevelBuilderModulStorage[]
  self._CurrentLevelFloors = {} ---@type GameObject[]
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
function LevelBuilder:UpdateLoadProgress(precent)
  self._LevelBuilderUIProgress.value = precent
end
---设置加载失败状态
---@param err boolean
---@param statuaCode string
---@param errMessage string
function LevelBuilder:UpdateErrStatus(err, statuaCode, errMessage)  
  self._LevelBuilderUIPanelFailed.gameObject:SetActive(err)
  if err then
    self._LevelBuilderUITextErrorContent.text = 'Code: '..statuaCode..'\n'..errMessage
    Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal)
  end
end

---加载关卡
---@param name string 关卡文件名
function LevelBuilder:LoadLevel(name)
  --播放加载声音
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_load.wav', GameSoundType.Normal)

  ---设置UI为初始状态
  self._LevelBuilderUI.gameObject:SetActive(true)
  self:UpdateLoadProgress(0)
  self:UpdateErrStatus(false, nil)
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
        self:UpdateErrStatus(true, 'BAD_LEVEL_JSON', 'Failed to decode json, error: '..res)
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
        self:UpdateErrStatus(true, 'DEPENDS_CHECK_FAILED', '关卡依赖以下此模组，可能是模组未启用或加载失败: '..missedPackages)
      end

      --载入Prefab
      self._CurrentLevelObject = Game.Manager:InstancePrefab(self._CurrentLevelPrefab, self.gameObject.transform, 'GameLevelMain')
      Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_MAIN_PREFAB_STANDBY', '*', { self._CurrentLevelObject })

      --加载
      local status, rs = pcall(function () self:_LoadLevelInternal() end)
      if not status then
        self:UpdateErrStatus(true, 'LOADER_EXPECTION', '加载失败，异常信息: '..rs)
      end
    end, 
    function (code, err)
      self:UpdateErrStatus(true, code, err)
    end)
end
function LevelBuilder:_LoadLevelInternal()

  --发送开始事件
  Game.Mediator:DispatchGlobalEvent('EVENT_LEVEL_BUILDER_START', '*')
  --加载基础设置
  local level = self._CurrentLevelJson.level

  self._CurrentLevelModuls = {}
  self._CurrentLevelSkyLayer = nil
  self._CurrentLevelFloors = {}
  
  --调用自定义加载步骤 pre
  self:_CallLoadStep("pre")

  --首先填充基础信息（出生点、节、结尾）
  if level.sectorCount ~= #level.sectors then
    self:UpdateErrStatus(true, 'BAD_CONFIG', 'level.sectorCount ~= #level.sectors')
    return
  end
  if level.sectorCount < 1 then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '该关卡至少要存在1个节')
    return
  end
  if level.sectorCount > 16 then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '该关卡节太多（超过16）')
    return
  end

  GamePlay.SectorManager.CurrentLevelSectorCount = level.sectorCount
  GamePlay.SectorManager.CurrentLevelSectors = {}
  GamePlay.SectorManager.CurrentLevelRestPoints = {}
  
  --首先检查配置是否正确
  if type(level.internalObjects) ~= "table" then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects\' 字段错误')
    return
  end
  if type(level.internalObjects.PS_LevelStart) ~= "string" then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PS_LevelStart\' 字段错误')
    return
  end
  if type(level.internalObjects.PE_LevelEnd) ~= "string" then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PE_LevelEnd\' 字段错误')
    return
  end  
  if type(level.internalObjects.PC_CheckPoints) ~= "table" then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PC_CheckPoints\' 字段错误')
    return
  end  
  if type(level.internalObjects.PR_ResetPoints) ~= "table" then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PR_ResetPoints\' 字段错误')
    return
  end
  if #level.internalObjects.PR_ResetPoints ~= level.sectorCount then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PR_ResetPoints\' 字段数量与 sectorCount 节数不一致')
    return
  end
  if #level.internalObjects.PC_CheckPoints < level.sectorCount - 1 then
    self:UpdateErrStatus(true, 'BAD_CONFIG', '\'internalObjects.PC_CheckPoints\' 字段数量少于 sectorCount 节数所需的数量')
    return
  end

  self:UpdateLoadProgress(0.1)

  --加载
  coroutine.resume(coroutine.create(function()

    local SectorManager = GamePlay.SectorManager
    local GamePlayManager = GamePlay.GamePlayManager

    --加载内部对象
    -----------------------------

    --加载出生点和火焰
    -----------------------------
    for i = 1, level.sectorCount, 1 do
      if i == 1 then
        local start = self:ReplacePrefab(level.internalObjects.PS_LevelStart, self:FindRegisterModul('PS_LevelStart'))
        local rp01 = GameObject.Find(level.internalObjects.PR_ResetPoints[tostring(i)])
        
        if start == nil then
          self:UpdateErrStatus(true, 'OBJECT_MISSING', '\'PS_LevelStart\' 丢失')
          return
        end
        if rp01 == nil then
          self:UpdateErrStatus(true, 'OBJECT_MISSING', '\'PR_ResetPoints.1\' 丢失')
          return
        end

        table.insert(SectorManager.CurrentLevelRestPoints, {
          point = rp01,
          modul = start
        })
      else
        local flame = self:ReplacePrefab(level.internalObjects.PC_CheckPoints[tostring(i)], self:FindRegisterModul('PC_CheckPoints'))
        local r = GameObject.Find(level.internalObjects.PR_ResetPoints[tostring(i)])
        
        if flame == nil then
          self:UpdateErrStatus(true, 'OBJECT_MISSING', '\'PC_CheckPoints.'..i..'\' 丢失')
          return
        end
        if r == nil then
          self:UpdateErrStatus(true, 'OBJECT_MISSING', '\'PR_ResetPoints.'..i..'\' 丢失')
          return
        end

        table.insert(SectorManager.CurrentLevelRestPoints, {
          point = r,
          modul = flame
        })
      end
      
    end
    --加载结尾
    -----------------------------
    SectorManager.CurrentLevelEndBalloon = self:ReplacePrefab(level.internalObjects.PE_LevelEnd, self:FindRegisterModul('PS_LevelStart'))
    if SectorManager.CurrentLevelEndBalloon == nil then
      self:UpdateErrStatus(true, 'OBJECT_MISSING', '\'PE_LevelEnd\' 丢失')
      return
    end
    
    Yield(WaitForSeconds(0.1))
    self:UpdateLoadProgress(0.2)

    --加载 物理路面
    -----------------------------
    for _, floor in ipairs(level.floors) do

      local physicsData = GamePhysFloor[floor.name] 
      if physicsData ~= nil then

        --StaticCompound
        local floorStatic = Game.Manager:InstancePrefab(self.gameObject.transform, 'FloorStaticCompound_'..floor.name)
        local shape = floorStatic:AddComponent(PhysicsShape) ---@type PhysicsShape
        shape.ShapeType = ShapeType.StaticCompound
        local body = floorStatic:AddComponent(PhysicsBody) ---@type PhysicsBody
        body.MotionType = MotionType.Fixed
        body.Friction = physicsData.Friction
        body.Restitution = physicsData.Restitution
        body.Layer = physicsData.Layer
        table.insert(self._CurrentLevelFloors, floorStatic)
        
        --Floor childs
        for _, name in ipairs(floor.objects) do     
          local go = GameObject.Find(name)
          if go ~= nil then
            --Mesh
            local meshFilter = go:GetComponent(MeshFilter) ---@type MeshFilter
            if meshFilter ~= nil and meshFilter.mesh  ~= nil then
              local shape = go:AddComponent(PhysicsShape) ---@type PhysicsShape
              shape.ShapeType = ShapeType.BvCompressedMesh
              shape.ShapeConvexRadius = 0.1
              shape.ShapeMesh = meshFilter.mesh
            else
              Log.W(TAG, 'Not found MeshFilter or mesh in floor  \''..name..'\'')
            end
            
          else
            Log.W(TAG, 'Not found floor  \''..name..'\' in type \''..floor.name..'\'')
          end
        end

      else
        Log.E(TAG, 'Unknow floor type \''..floor.name..'\'')
      end
    end

    self:UpdateLoadProgress(0.3)

    --调用自定义加载步骤 modul
    -----------------------------
    self:_CallLoadStep("modul")
    self:UpdateLoadProgress(0.4)

    --加载坠落检测区
    -----------------------------
    for _, name in ipairs(level.depthTestCubes) do
      local go = GameObject.Find(name)
      if go ~= nil then

        --禁用Renderer使物体隐藏
        local renderer = go:AddComponent(Renderer) ---@type Renderer
        if renderer ~ nil then renderer.enabled = false end
        
        --添加幻影
        local phantom = go:AddComponent(PhysicsPhantom) ---@type PhysicsPhantom
        phantom.SetAabbBySelf()
        ---@param self PhysicsPhantom
        ---@param other PhysicsBody
        phantom.onOverlappingCollidableAdd = function (self, other)
          --触发球坠落
          if other.gameObject.tag == 'Ball' then
            GamePlayManager:Fall()
          end
        end

      else
        Log.W(TAG, 'Not found object \''..name..'\' in depthTestCubes')
      end
    end

    self:UpdateLoadProgress(0.5)

    --加载 modul
    -----------------------------
    local tickCount = 0
    for _, group in ipairs(level.groups) do
      local modul = self:FindRegisterModul(group.name)
      if modul ~= nil then
        for _, name in ipairs(group.objects) do

          if tickCount > 16 then
            Yield(WaitForSeconds(0.08))
            tickCount = 0
          end

          local go = GameObject.Find(name)
          if go ~= nil then
            self:ReplacePrefab(go, modul)
            tickCount = tickCount + 1
          else
            Log.W(TAG, 'Not found object \''..name..'\' in group \''..group.name..'\'')
          end 
        end

      else
        Log.W(TAG, 'Modul \''..group.name..'\' is not registered')
      end

    end

    self:UpdateLoadProgress(0.6)

    --首次加载 modul
    -----------------------------
    Yield(WaitForSeconds(0.5))
    SectorManager:DoInitAllModuls();
    
    self:UpdateLoadProgress(0.7)

    --填充节数据
    -----------------------------
    for id, sector in ipairs(level.sectors) do
      if SectorManager.CurrentLevelSectors[id] ~= nil then
        Log.W(TAG, 'Duplicate key \''..id..'\' in sectors')
      else
        local moduls = {}
        for _, name in ipairs(sector) do
          local modul = self._CurrentLevelModuls[name]
          if modul ~= nil then
            table.insert(moduls, modul)
          else
            Log.W(TAG, 'Not found modul \''..name..'\' in sectors.'..id)
          end
        end
        SectorManager.CurrentLevelSectors[id] = {
          moduls = moduls
        }
      end
    end

    self:UpdateLoadProgress(0.8)

    --加载天空盒和灯光
    -----------------------------
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
      GamePlayManager:CreateSkyAndLight('', skyMat, level.lightColor)
    else
      --使用自带天空盒 
      GamePlayManager:CreateSkyAndLight(level.skyBox, nil, level.lightColor)
    end

    --加载SkyLayer
    -----------------------------
    if level.skyLayer == 'SkyLayer' then
      local oldSkyLayer = GameObject.Find('SkyLayer')
      self._CurrentLevelSkyLayer = Game.Manager:InstancePrefab(Game.SystemPackage:GetPrefabAsset('SkyLayer.prefab'), 'SkyLayer')
      self._CurrentLevelSkyLayer.transform.position = oldSkyLayer.transform.position
      self._CurrentLevelSkyLayer.transform.rotation = oldSkyLayer.transform.rotation
      oldSkyLayer:SetActive(false)
    elseif level.skyLayer == 'SkyVoterx' then
      local oldSkyLayer = GameObject.Find('SkyVoterx')
      self._CurrentLevelSkyLayer = Game.Manager:InstancePrefab(Game.SystemPackage:GetPrefabAsset('SkyVoterx.prefab'), 'SkyVoterx')
      self._CurrentLevelSkyLayer.transform.position = oldSkyLayer.transform.position
      self._CurrentLevelSkyLayer.transform.rotation = oldSkyLayer.transform.rotation
      oldSkyLayer:SetActive(false)
    end

    self:UpdateLoadProgress(0.9)

    --调用自定义加载步骤 last
    -----------------------------
    self:_CallLoadStep("last")

    self:UpdateLoadProgress(1)

    --最后加载步骤
    -----------------------------
    GamePlayManager:Init()
  end))
end

---替换占位符并生成机关
---@param objName string 占位符的名称
---@param modulPrefab GameObject 机关预制体
---@return ModulBase|nil 返回机关类，如果出现错误则返回nil
function LevelBuilder:ReplacePrefab(objName, modulPrefab)
  --检查同名机关
  if self._CurrentLevelModuls[objName] ~= nil then
    Log.E(TAG, 'Find modul with the same name \''..objName..'\'')
    return nil
  end
  local obj = GameObject.Find(objName)
  if obj == nil then
    Log.E(TAG, 'Not found placeholder \''..objName..'\'')
    return nil
  end
  --隐藏占位符
  obj:SetActive(false) 
  --克隆机关
  local modul = Game.Manager:InstancePrefab(modulPrefab, self.gameObject.transform, "ModulInstance_"..objName)
  --同步机关位置
  modul.transform.position = obj.transform.position
  modul.transform.rotation = obj.transform.rotation
  --获取类
  local modulClass = GameLuaObjectHost.GetLuaClassFromGameObject(modul)
  self._CurrentLevelModuls[objName] = {
    go = modul,
    modul = modulClass
  }
  --获取类
  return modulClass
end

---卸载当前加载的关卡
function LevelBuilder:UnLoadLevel()
  --播放加载声音
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_load.wav', GameSoundType.Normal)
  --
  coroutine.resume(coroutine.create(function()

    --通知所有modul卸载
    GamePlay.SectorManager:DoUnInitAllModuls()

    Yield(WaitForSeconds(0.5))

    --清空数据
    GamePlay.SectorManager:ClearAll() 

    --删除所有modul
    local tickCount = 0
    for _, value in ipairs(self._CurrentLevelModuls) do
      if tickCount > 16 then
        Yield(WaitForSeconds(0.08))
        tickCount = 0
      end
      UnityEngine.Object.Destroy(value.go)
      tickCount = tickCount + 1
    end

    --删除路面
    for _, value in ipairs(self._CurrentLevelFloors) do
      UnityEngine.Object.Destroy(value)
    end

    Yield(WaitForSeconds(0.1))

    --清空天空和云层
    GamePlay.GamePlayManager:HideSkyAndLight()
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

    Yield(WaitForSeconds(0.1))

    --卸载AssetBundle
    self._LevelLoaderNative:UnLoadLevel(self._CurrentLevelAssetBundle)
    self._CurrentLevelAssetBundle = nil

    Yield(WaitForSeconds(0.1))

    --通知回到menulevel
    Game.Manager:RequestEnterLogicScense('MenuLevel')

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
