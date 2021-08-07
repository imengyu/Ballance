local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local GameSettingsManager = Ballance2.Config.GameSettingsManager
local Screen = UnityEngine.Screen
local QualitySettings = UnityEngine.QualitySettings
local KeyCode = UnityEngine.KeyCode
local WaitForSeconds = UnityEngine.WaitForSeconds
local Yield = UnityEngine.Yield

---创建主设置菜单UI
---@param package GamePackage
function CreateSettingsUI(package)
  local PageSettings = GameUIManager:RegisterPage('PageSettings', 'PageCommon')
  local PageSettingsAudio = GameUIManager:RegisterPage('PageSettingsAudio', 'PageCommon')
  local PageSettingsControls = GameUIManager:RegisterPage('PageSettingsControls', 'PageCommon')
  local PageSettingsGraphics = GameUIManager:RegisterPage('PageSettingsGraphics', 'PageCommon')

  PageSettings:CreateContent(package)
  PageSettingsAudio:CreateContent(package)
  PageSettingsControls:CreateContent(package)
  PageSettingsGraphics:CreateContent(package)

  coroutine.resume(coroutine.create(function ()
    Yield(WaitForSeconds(0.5))
    BindSettingsUI(MessageCenter)
  end))
end

---绑定设置UI
---@param MessageCenter GameUIMessageCenter
function BindSettingsUI(MessageCenter)
    
  local GameSettings = GameSettingsManager.GetSettings("core")

  --设置数据绑定
  --声音
  local updateVoiceMain = MessageCenter:SubscribeValueBinder('VoiceMain', function(val)
    GameSettings:SetFloat("voice.main", val)
    GameSettings:NotifySettingsUpdate("voice")
  end)
  local updateVoiceUI = MessageCenter:SubscribeValueBinder('VoiceUI', function(val)
    GameSettings:SetFloat("voice.ui", val)
    GameSettings:NotifySettingsUpdate("voice")
  end)
  local updateVioceBackground = MessageCenter:SubscribeValueBinder('VioceBackground', function(val)
    GameSettings:SetFloat("voice.background", val)
    GameSettings:NotifySettingsUpdate("voice")
  end)
  --视频
  local updateGrResolution = MessageCenter:SubscribeValueBinder('GrResolution', function(val)
    GameSettings:SetInt("video.resolution", val)
  end)
  local updateGrFullScreen = MessageCenter:SubscribeValueBinder('GrFullScreen', function(val)
    GameSettings:SetBool("video.fullScreen", val)
  end)
  local updateGrCloud = MessageCenter:SubscribeValueBinder('GrCloud', function(val)
    GameSettings:SetBool("video.cloud", val)
  end)
  local updateGrVSync = MessageCenter:SubscribeValueBinder('GrVSync', function(val)
    GameSettings:SetInt("video.vsync", val and 1 or 0 )
  end)
  --控制
  local updateControlKeyUp = MessageCenter:SubscribeValueBinder('ControlKeyUp', function(val)
    GameSettings:SetInt("control.keyup", val)
  end)
  local updateControlKeyDown = MessageCenter:SubscribeValueBinder('ControlKeyDown', function(val)
    GameSettings:SetInt("control.keydown", val)
  end)
  local updateControlKeyLeft = MessageCenter:SubscribeValueBinder('ControlKeyLeft', function(val)
    GameSettings:SetInt("control.keyleft", val)
  end)
  local updateControlKeyRight = MessageCenter:SubscribeValueBinder('ControlKeyRight', function(val)
    GameSettings:SetInt("control.keyright", val)
  end)
  local updateControlKeyOverlookCam = MessageCenter:SubscribeValueBinder('ControlKeyOverlookCam', function(val)
    GameSettings:SetInt("control.overlookcam", val)
  end)
  local updateControlKeyRoateCam = MessageCenter:SubscribeValueBinder('ControlKeyRoateCam', function(val)
    GameSettings:SetInt("control.roatecam", val)
  end)
  local updateControlReverse = MessageCenter:SubscribeValueBinder('ControlReverse', function(val)
    GameSettings:SetBool("control.reverse", val)
  end)

  local resolutions = nil

  --设置数据加载
  MessageCenter:SubscribeEvent('BtnSettingsGraphicsClick', function () 
    updateGrFullScreen:Invoke(GameSettings:GetBool("video.fullScreen", Screen.fullScreen))
    updateGrCloud:Invoke(GameSettings:GetBool("video.cloud", true))
    updateGrVSync:Invoke(GameSettings:GetInt("video.vsync", QualitySettings.vSyncCount) >= 1 and true or false)
    GameUIManager:GoPage('PageSettingsGraphics') 

    if(resolutions == nil) then
      coroutine.resume(coroutine.create( function ()
        Yield(WaitForSeconds(0.5))
        --加载分辨率设置
        resolutions = Screen.resolutions
        local GrResolution = MessageCenter:GetComponentInstance('GrResolution'):GetComponent(Ballance2.Sys.UI.Updown) ---@type Updown
        local currentResolution = Screen.currentResolution
        for i = 1, #resolutions, 1 do
          local resolution = resolutions[i] ---@type Resolution
          GrResolution:AddOption(resolution.width .. 'x' .. resolution.height .. '@' .. resolution.refreshRate)
          if(
            currentResolution.width == resolution.width and currentResolution.height == resolution.height
            and currentResolution.refreshRate == resolution.refreshRate
          ) then updateGrResolution:Invoke(i - 1) end
        end
      end )) 
    end
  end)
  MessageCenter:SubscribeEvent('BtnSettingsControlsClick', function () 
    updateControlKeyUp:Invoke(GameSettings:GetInt("control.keyup", KeyCode.UpArrow))
    updateControlKeyDown:Invoke(GameSettings:GetInt("control.keydown", KeyCode.DownArrow))
    updateControlKeyLeft:Invoke(GameSettings:GetInt("control.keyleft", KeyCode.LeftArrow))
    updateControlKeyRight:Invoke(GameSettings:GetInt("control.keyright", KeyCode.RightArrow))
    updateControlKeyOverlookCam:Invoke(GameSettings:GetInt("control.overlookcam", KeyCode.Space))
    updateControlKeyRoateCam:Invoke(GameSettings:GetInt("control.roatecam", KeyCode.LeftShift))
    updateControlReverse:Invoke(GameSettings:GetBool("control.reverse", false))
    GameUIManager:GoPage('PageSettingsControls') 
  end)
  MessageCenter:SubscribeEvent('BtnSettingsAudioClick', function () 
    updateVioceBackground:Invoke(GameSettings:GetFloat("voice.background", 100))
    updateVoiceMain:Invoke(GameSettings:GetFloat("voice.main", 100))
    updateVoiceUI:Invoke(GameSettings:GetFloat("voice.ui", 100))
    GameUIManager:GoPage('PageSettingsAudio') 
  end)
  MessageCenter:SubscribeEvent('BtnSettingsPackageClick', function () 
    GameManager.Instance.GameStore['DbgShowPackageManageWindow'] = true
  end)

  --设置保存
  MessageCenter:SubscribeEvent('BtnSettingsGraphicsBackClick', function () 
    GameSettings:NotifySettingsUpdate("video")
    GameUIManager:BackPreviusPage()
  end)
  MessageCenter:SubscribeEvent('BtnSettingsControlsBackClick', function () 
    GameSettings:NotifySettingsUpdate("control")
    GameUIManager:BackPreviusPage()
  end)
  MessageCenter:SubscribeEvent('BtnSettingsAudioBackClick', function () 
    GameSettings:NotifySettingsUpdate("voice")
    GameUIManager:BackPreviusPage()
  end)

end