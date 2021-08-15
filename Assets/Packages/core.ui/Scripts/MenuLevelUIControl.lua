local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local WaitForSeconds = UnityEngine.WaitForSeconds
local Yield = UnityEngine.Yield
local Application = UnityEngine.Application

---创建主菜单UI
---@param package GamePackage
function CreateMenuLevelUI(package)

  local PageMain = GameUIManager:RegisterPage('PageMain', 'PageCommon')
  local PageAbout = GameUIManager:RegisterPage('PageAbout', 'PageCommon')
  local PageHighscore = GameUIManager:RegisterPage('PageHighscore', 'PageCommon')
  local PageQuit = GameUIManager:RegisterPage('PageQuit', 'PageCommon')
  local PageStart = GameUIManager:RegisterPage('PageStart', 'PageCommon')
  local PageLightZone = GameUIManager:RegisterPage('PageLightZone', 'PageTransparent')

  PageMain:CreateContent(package)
  PageAbout:CreateContent(package)
  PageHighscore:CreateContent(package)
  PageQuit:CreateContent(package)
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
    GameManager.GameMediator:NotifySingleEvent(EVENT_SWITCH_LIGHTZONE, { false })
    GameUIManager:BackPreviusPage() 
  end)
  MessageCenter:SubscribeEvent('BtnStartLightZoneClick', function () 
    GameManager.GameMediator:NotifySingleEvent(EVENT_SWITCH_LIGHTZONE, { true })
    GameUIManager:GoPage('PageLightZone') 
  end)

  local PanelScrollCaption = PageAbout.Content.transform:Find("PanelScrollCaption").gameObject
  local PanelMyAbout = PageAbout.Content.transform:Find("PanelMyAbout").gameObject
 
  MessageCenter:SubscribeEvent('ButtonOpenSourceLicenseClick', function () 

  end)
  MessageCenter:SubscribeEvent('ButtonProjectClick', function () 
    PanelScrollCaption:SetActive(false)
    PanelMyAbout:SetActive(true)
  end)
  MessageCenter:SubscribeEvent('BtnAboutBackClick', function () 
    if PanelMyAbout.activeSelf then
      PanelScrollCaption:SetActive(true)
      PanelMyAbout:SetActive(false)
    else
      GameUIManager:BackPreviusPage()
    end
  end)
  MessageCenter:SubscribeEvent('BtnGoBallanceBaClick', function () 
    Application.OpenURL('https://tieba.baidu.com/f?kw=%E5%B9%B3%E8%A1%A1%E7%90%83')
  end)
  MessageCenter:SubscribeEvent('BtnGoGithubClick', function () 
    Application.OpenURL('https://github.com/imengyu/Ballance')
  end)

  coroutine.resume(coroutine.create(function ()
    Yield(WaitForSeconds(0.5))
    GameUIManager:GoPage('PageMain')
    Yield(WaitForSeconds(1))
    GameUIManager:MaskBlackFadeOut(1)
  end))
end