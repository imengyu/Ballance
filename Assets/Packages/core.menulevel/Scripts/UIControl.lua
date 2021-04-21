GameManager = Ballance2.Sys.GameManager
GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
Log = Ballance2.Utils.Log


---创建主菜单UI
---@param package GamePackage
function CreateMenuLevelUI(package)
  local MessageCenter = GameUIManager:CreateUIMessageCenter('MenuLevelUICM')

  local PageMain = GameUIManager:RegisterPage('PageMain', 'PageCommon')
  local PageAbout = GameUIManager:RegisterPage('PageAbout', 'PageCommon')
  local PageHighscore = GameUIManager:RegisterPage('PageHighscore', 'PageCommon')
  local PageQuit = GameUIManager:RegisterPage('PageQuit', 'PageCommon')
  local PageSettings = GameUIManager:RegisterPage('PageSettings', 'PageCommon')
  local PageSettingsAudio = GameUIManager:RegisterPage('PageSettingsAudio', 'PageCommon')
  local PageSettingsControls = GameUIManager:RegisterPage('PageSettingsControls', 'PageCommon')
  local PageSettingsGraphics = GameUIManager:RegisterPage('PageSettingsGraphics', 'PageCommon')
  local PageStart = GameUIManager:RegisterPage('PageStart', 'PageCommon')
  local PageLightZone = GameUIManager:RegisterPage('PageLightZone', 'PageTransparent')

  PageMain:CreateContent(package)
  PageAbout:CreateContent(package)
  PageHighscore:CreateContent(package)
  PageQuit:CreateContent(package)
  PageSettings:CreateContent(package)
  PageSettingsAudio:CreateContent(package)
  PageSettingsControls:CreateContent(package)
  PageSettingsGraphics:CreateContent(package)
  PageStart:CreateContent(package)
  PageLightZone:CreateContent(package)

  MessageCenter:SubscribeEvent('BtnBackClick', function () GameUIManager:BackPreviusPage() end)
  MessageCenter:SubscribeEvent('BtnStartClick', function () GameUIManager:GoPage('PageStart') end)
  MessageCenter:SubscribeEvent('BtnHighscrollClick', function () GameUIManager:GoPage('PageHighscore') end)
  MessageCenter:SubscribeEvent('BtnAboutClick', function () GameUIManager:GoPage('PageAbout') end)
  MessageCenter:SubscribeEvent('BtnSettingsClick', function () GameUIManager:GoPage('PageSettings') end)
  MessageCenter:SubscribeEvent('BtnQuitClick', function () GameUIManager:GoPage('PageQuit') end)
  MessageCenter:SubscribeEvent('BtnQuitSureClick', function () 
    GameUIManager:CloseAllPage()
    GameManager.Instance:QuitGame() 
  end)

  MessageCenter:SubscribeEvent('BtnLightZoneBackClick', function () 
    MenuLevelControl:SwitchLightZone(false)
    GameUIManager:BackPreviusPage() 
  end)
  MessageCenter:SubscribeEvent('BtnStartLightZoneClick', function () 
    MenuLevelControl:SwitchLightZone(true)
    GameUIManager:GoPage('PageLightZone') 
  end)

  MessageCenter:SubscribeEvent('BtnSettingsGraphicsClick', function () GameUIManager:GoPage('PageSettingsGraphics') end)
  MessageCenter:SubscribeEvent('BtnSettingsControlsClick', function () GameUIManager:GoPage('PageSettingsControls') end)
  MessageCenter:SubscribeEvent('BtnSettingsAudioClick', function () GameUIManager:GoPage('PageSettingsAudio') end)
  MessageCenter:SubscribeEvent('BtnSettingsPackageClick', function () 
    GameManager.Instance.GameStore['DbgShowPackageManageWindow'] = true
  end)





  GameUIManager:GoPage('PageMain')
end