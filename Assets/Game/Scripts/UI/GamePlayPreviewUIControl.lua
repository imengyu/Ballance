---主游戏菜单控制器类
---@class GamePlayPreviewUIControl : GameLuaObjectHostClass
---@field _TextLevelInfo Text
---@field _TextCameraSpeed Text
---@field _CheckBoxSkyBox Toggle
---@field _CheckBoxWireFrame Toggle
---@field _CheckBoxAudio Toggle
---@field _CheckBoxFog Toggle
---@field _CheckBoxDethTethCubes Toggle
---@field _CheckBoxSkylayer Toggle
---@field _GizmoRenderer SceneGizmoController
GamePlayPreviewUIControl = ClassicObject:extend()

function GamePlayPreviewUIControl:new() 
end
function GamePlayPreviewUIControl:Start() 

  self._CheckBoxSkyBox.onValueChanged:AddListener(function (check)
    self._FreeCamera.SkyBox = check
  end)
  self._CheckBoxWireFrame.onValueChanged:AddListener(function (check)
    self._FreeCamera.Wireframe = check
  end)
  self._CheckBoxFog.onValueChanged:AddListener(function (check)
    self._FreeCamera.Fog = check
  end)
  self._CheckBoxAudio.onValueChanged:AddListener(function (check)
    self._FreeCamera.Audio = check
  end)
  self._CheckBoxDethTethCubes.onValueChanged:AddListener(function (check)
    GamePlay.GamePreviewManager:ToggleDethTethCubesVisible(check)
  end)
  self._CheckBoxSkylayer.onValueChanged:AddListener(function (check)
    GamePlay.GamePreviewManager:ToggleSkylayerVisible(check)
  end)

  GameUI.GamePreviewUI = self
end

---设置预览关卡信息
---@param name string
---@param author string
---@param version string
function GamePlayPreviewUIControl:SetLevelInfo(name, author, version) 
  local json = Game.LevelBuilder._CurrentLevelJson
  local level = json.level
  self._TextLevelInfo.text = name..'\n'
    ..'\nAuthor:'..author
    ..'\nVersion:'..version..'\n'
    ..'\nFirstBall:'..level.firstBall
    ..'\nLevelScore:'..level.levelScore
    ..'\nStartPoint:'..level.startPoint
    ..'\nStartLife:'..level.startLife
    ..'\nSector count:'..level.sectorCount
    ..'\nLightColor:'..level.lightColor
    ..'\nSkyBox:'..level.skyBox
    ..'\nMusicTheme:'..level.musicTheme
end
---@param freeCam FreeCamera
function GamePlayPreviewUIControl:SetFreeCamera(freeCam) 
  self._FreeCamera = freeCam
  self._GizmoRenderer.ReferenceTransform = freeCam.transform
  freeCam.onCamSpeedChanged = function ()
    self._TextCameraSpeed.text = 'CamSpeed: '..string.format("%.2f", self._FreeCamera.cameraSpeed)
  end
end

function CreateClass:GamePlayPreviewUIControl() 
  return GamePlayPreviewUIControl()
end