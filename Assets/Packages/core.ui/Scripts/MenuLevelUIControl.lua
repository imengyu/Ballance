local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local GamePackage = Ballance2.Sys.Package.GamePackage
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local SystemPackage = GamePackage.GetSystemPackage()
local WaitForSeconds = UnityEngine.WaitForSeconds
local Yield = UnityEngine.Yield
local Application = UnityEngine.Application
local Time = UnityEngine.Time
local Text = UnityEngine.UI.Text
local I18N = Ballance2.Sys.Language.I18N

---创建主菜单UI
---@param package GamePackage
function CreateMenuLevelUI(package)

  local PageMain = GameUIManager:RegisterPage('PageMain', 'PageCommon')
  local PageAbout = GameUIManager:RegisterPage('PageAbout', 'PageCommon')
  local PageHighscore = GameUIManager:RegisterPage('PageHighscore', 'PageCommon')
  local PageQuit = GameUIManager:RegisterPage('PageQuit', 'PageCommon')  
  local PageStart = GameUIManager:RegisterPage('PageStart', 'PageCommon')
  local PageLightZone = GameUIManager:RegisterPage('PageLightZone', 'PageTransparent')
  local PageAboutCreators = GameUIManager:RegisterPage('PageAboutCreators', 'PageCommon')
  local PageAboutProject = GameUIManager:RegisterPage('PageAboutProject', 'PageCommon')

  PageMain:CreateContent(package)
  PageAbout:CreateContent(package)
  PageHighscore:CreateContent(package)
  PageQuit:CreateContent(package)
  PageStart:CreateContent(package)
  PageLightZone:CreateContent(package)
  PageAboutCreators:CreateContent(package)
  PageAboutProject:CreateContent(package)

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
  MessageCenter:SubscribeEvent('ButtonProjectClick', function () GameUIManager:GoPage('PageAboutProject') end)
  MessageCenter:SubscribeEvent('BtnCreatorsClick', function () GameUIManager:GoPage('PageAboutCreators') end)
  
  MessageCenter:SubscribeEvent('BtnLightZoneBackClick', function () 
    GameManager.GameMediator:NotifySingleEvent(EVENT_SWITCH_LIGHTZONE, { false })
    GameUIManager:BackPreviusPage() 
  end)
  MessageCenter:SubscribeEvent('BtnStartLightZoneClick', function () 
    GameManager.GameMediator:NotifySingleEvent(EVENT_SWITCH_LIGHTZONE, { true })
    GameUIManager:GoPage('PageLightZone') 
  end)
  MessageCenter:SubscribeEvent('ButtonOpenSourceLicenseClick', function () 
    local OpenSourceLicenseWindow = GameUIManager:CreateWindow('OpenSource licenses', 
      CloneUtils.CloneNewObjectWithParent(GameUIPackage:GetPrefabAsset('LicensesView.prefab'), GameManager.Instance.GameCanvas).transform, 
      true, 20, -205, 400, 500)
    OpenSourceLicenseWindow:MoveToCenter()
  end)
  MessageCenter:SubscribeEvent('BtnGoBallanceBaClick', function () 
    Application.OpenURL('https://tieba.baidu.com/f?kw=%E5%B9%B3%E8%A1%A1%E7%90%83')
  end)
  MessageCenter:SubscribeEvent('BtnGoGithubClick', function () 
    Application.OpenURL('https://github.com/imengyu/Ballance')
  end)

  --点击版本按扭8次开启开发者模式
  local lastClickTime = 0
  local lastClickCount = 0
  MessageCenter:SubscribeEvent('BtnVersionClick', function () 

    if lastClickTime - Time.time > 5 then lastClickCount = 0 end
    lastClickTime = Time.time

    --增加
    lastClickCount = lastClickCount + 1
    if(lastClickCount >= 4) then
      if GameManager.DebugMode then
        GameUIManager:GlobalToast(I18N.Tr('str.tip.in.debug'))
        return 
      else
        GameUIManager:GlobalToast(I18N.TrF('str.tip.click.debug', { (8-lastClickCount) }))
      end
      if(lastClickCount >= 8) then
        GameManager.GameMediator:CallAction(SystemPackage, 'System', 'EnableDebugMode')
      end
    end
  end)

  --加载版本完整信息
  local ButtonVersionText = PageAbout.Content.transform:Find('ButtonVersion/Text'):GetComponent(Text) ---@type Text
  ButtonVersionText.text = I18N.TrF('ui.about.about.version', { Ballance2.Config.GameConst.GameVersion })

  coroutine.resume(coroutine.create(function ()
    Yield(WaitForSeconds(0.5))
    GameUIManager:GoPage('PageMain')
    Yield(WaitForSeconds(1))
    GameUIManager:MaskBlackFadeOut(1)
  end))
end