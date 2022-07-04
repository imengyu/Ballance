local GameManager = Ballance2.Services.GameManager.Instance
local GameDebugEntry = Ballance2.Entry.GameDebugEntry.Instance
local GameErrorChecker = Ballance2.Services.Debug.GameErrorChecker
local GameError = Ballance2.Services.Debug.GameError
local PhysicsObject = BallancePhysics.Wapper.PhysicsObject
local MeshFilter = UnityEngine.MeshFilter
local GameLuaObjectHost = Ballance2.Services.LuaService.LuaWapper.GameLuaObjectHost
local StringUtils = Ballance2.Utils.StringUtils

---迷你机关调试环境
function ModulCustomDebug()
  --检查参数
  if Slua.IsNull(GameDebugEntry.ModulTestFloor) then
    GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulTestFloor 未设置！")
    return
  end
  if Slua.IsNull(GameDebugEntry.ModulTestUI) then
    GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulTestUI 未设置！")
    return
  end
  if Slua.IsNull(GameDebugEntry.ModulInstance) then
    GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulInstance 未设置！")
    return
  end
  if GameDebugEntry.ModulName == '' then
    GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulName 未设置！")
    return
  end

  --设置宏定义
  BALLANCE_MODUL_DEBUG = true

  GamePlayInit(function ()
    local PR_Resetpoint = nil---@type GameObject
    local Modul_Placeholder = nil---@type GameObject
    
    --加载物理环境
    GamePlay.GamePlayManager.GamePhysicsWorld:Create()

    --克隆路面
    local physicsData = GamePhysFloor['Phys_Floors'] 
    local physicsDataStopper = GamePhysFloor['Phys_FloorStopper'] 
    local group = GameManager:InstancePrefab(GameDebugEntry.ModulTestFloor, "ModulTestFloor")
    local childCount = group.transform.childCount
    for i = 0, childCount - 1, 1 do
      local go = group.transform:GetChild(i).gameObject
      if go.activeSelf then
        if go.name == 'Floor' then
          local meshFilter = go:GetComponent(MeshFilter) ---@type MeshFilter
          if meshFilter ~= nil and meshFilter.mesh  ~= nil then
            local body = go:AddComponent(PhysicsObject) ---@type PhysicsObject
            body.DoNotAutoCreateAtAwake = true
            body.Fixed = true
            body.BuildRootConvexHull = false
            body.Concave:Add(meshFilter.mesh)
            body.Friction = physicsData.Friction
            body.Elasticity = physicsData.Elasticity
            body.Layer = physicsData.Layer
            body.UseExistsSurface = true
            body.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName(physicsData.CollisionLayerName)
            body:Physicalize()
          end
        elseif go.name == 'FloorStopper' then
          local meshFilter = go:GetComponent(MeshFilter) ---@type MeshFilter
          if meshFilter ~= nil and meshFilter.mesh  ~= nil then
            local body = go:AddComponent(PhysicsObject) ---@type PhysicsObject
            body.DoNotAutoCreateAtAwake = true
            body.Fixed = true
            body.BuildRootConvexHull = false
            body.Concave:Add(meshFilter.mesh)
            body.Friction = physicsDataStopper.Friction
            body.Elasticity = physicsDataStopper.Elasticity
            body.Layer = physicsDataStopper.Layer
            body.UseExistsSurface = true
            body.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName(physicsDataStopper.CollisionLayerName)
            body:Physicalize()
          end
        elseif go.name == 'PR_Resetpoint' then
          PR_Resetpoint = go
        elseif go.name == GameDebugEntry.ModulName then
          Modul_Placeholder = go
        end
      end
    end

    if not PR_Resetpoint then
      GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "没有找到PR_Resetpoint, 请在测试路面预制体中添加一个")
      return
    end
    if not Modul_Placeholder then
      GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "没有找到"..GameDebugEntry.ModulName..", 请在测试路面预制体中添加一个")
      return
    end

    --克隆机关
    local Modul_Instace = GameManager:InstancePrefab(GameDebugEntry.ModulInstance, "ModulInstance")
    Modul_Instace.transform.position = Modul_Placeholder.transform.position
    Modul_Instace.transform.rotation = Modul_Placeholder.transform.rotation
    Modul_Placeholder:SetActive(false)

    local Modul_Class = GameLuaObjectHost.GetLuaClassFromGameObject(Modul_Instace) ---@type ModulBase
    if not GameLuaObjectHost then
      GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "没有在"..Modul_Instace.name.."上找到 LuaClass ！请确定已绑定脚本")
      return
    end
    


    --克隆UI
    local ui = GameManager:InstancePrefab(GameDebugEntry.ModulTestUI, GameManager.GameCanvas, "ModulTestUI")
    local stateText = ui.transform:Find('Text'):GetComponent(UnityEngine.UI.Text) ---@type Text
    local button = ui.transform:Find('Text'):GetComponent(UnityEngine.UI.Button) ---@type Button
    
    function UpdateText(state)
      stateText.text = 'Modul state tools:\n'..Modul_Instace.name..' state: '..state
    end

    button = ui.transform:Find('ButtonActive'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      Modul_Class:Active()
      UpdateText('Active')
    end);
    button = ui.transform:Find('ButtonDeactive'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      Modul_Class:Deactive()
      UpdateText('Deactive')
    end);
    button = ui.transform:Find('ButtonResetLevel'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      Modul_Class:Reset('levelRestart')
      UpdateText('Reset levelRestart')
    end);    
    button = ui.transform:Find('ButtonResetSector'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      Modul_Class:Reset('sectorRestart')
      UpdateText('Reset sectorRestart')
    end);
    button = ui.transform:Find('ButtonQuit'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      ui.gameObject:SetActive(false)
      GameManager:QuitGame()
    end);
    button = ui.transform:Find('ButtonBackup'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      Modul_Class:Backup()
      Game.UIManager:GlobalToast('Modul_Class:Backup() !')
    end);
    button = ui.transform:Find('ButtonCustom1'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      Modul_Class:Custom(1)
      Game.UIManager:GlobalToast('Modul_Class:Custom1() !')
    end);
    button = ui.transform:Find('ButtonCustom2'):GetComponent(UnityEngine.UI.Button) ---@type Button
    button.onClick:AddListener(function ()
      Modul_Class:Custom(2)
      Game.UIManager:GlobalToast('Modul_Class:Custom2() !')
    end);

    PR_Resetpoint:SetActive(false)

    Modul_Class.gameObject:SetActive(true);
    LuaTimer.Add(300, function ()
      --初始备份数据
      Modul_Class:Backup()
      Modul_Class:Deactive()

      --构建数据
      GamePlay.SectorManager.CurrentLevelSectorCount = 1
      GamePlay.SectorManager.CurrentLevelSectors = {
        { moduls = { Modul_Class } }
      }
      GamePlay.SectorManager.CurrentLevelRestPoints = {
        {
          point = PR_Resetpoint,
          flame = nil
        }
      }
      GamePlay.GamePlayManager:CreateSkyAndLight('L', nil, StringUtils.StringToColor('#B09D89'))

      UpdateText('Active')
      --开始关卡
      Game.Mediator:NotifySingleEvent("CoreGamePlayManagerInitAndStart", nil)
    end)
  end)
end