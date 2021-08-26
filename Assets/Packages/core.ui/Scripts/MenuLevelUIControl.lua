local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local GamePackage = Ballance2.Sys.Package.GamePackage
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local I18N = Ballance2.Sys.Language.I18N
local SystemPackage = GamePackage.GetSystemPackage()
local Application = UnityEngine.Application
local Time = UnityEngine.Time
local Text = UnityEngine.UI.Text

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
  local PageCustomLevel = GameUIManager:RegisterPage('PageCustomLevel', 'PageWide')

  PageMain:CreateContent(package)
  PageMain.CanEscBack = false
  PageAbout:CreateContent(package)
  PageHighscore:CreateContent(package)
  PageQuit:CreateContent(package)
  PageStart:CreateContent(package)
  PageLightZone:CreateContent(package)
  PageAboutCreators:CreateContent(package)
  PageAboutProject:CreateContent(package)
  PageCustomLevel:CreateContent(package)

  MessageCenter:SubscribeEvent('BtnStartClick', function () GameUIManager:GoPage('PageStart') end)
  MessageCenter:SubscribeEvent('BtnCustomLevelClick', function () GameUIManager:GoPage('PageCustomLevel') end )
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


  MessageCenter:SubscribeEvent('BtnHighscrollClick', function () 
    GameUIManager:GoPage('PageHighscore') 
    GameUI.HighscoreUIControl:LoadLevelData(1)
  end)
  MessageCenter:SubscribeEvent('BtnHighscorePageLeftClick', function () 
    GameUI.HighscoreUIControl:Prev()
  end) 
  MessageCenter:SubscribeEvent('BtnHighscorePageRightClick', function () 
    GameUI.HighscoreUIControl:Next()
  end)

  local loadInternalLevel = function (id)
    GameManager.GameMediator:NotifySingleEvent('CoreStartLoadLevel', { 'Level'..id })
  end

  MessageCenter:SubscribeEvent('BtnStartLev01Click', function () loadInternalLevel('01') end )
  MessageCenter:SubscribeEvent('BtnStartLev02Click', function () loadInternalLevel('02') end )
  MessageCenter:SubscribeEvent('BtnStartLev03Click', function () loadInternalLevel('03') end )
  MessageCenter:SubscribeEvent('BtnStartLev04Click', function () loadInternalLevel('04') end )
  MessageCenter:SubscribeEvent('BtnStartLev05Click', function () loadInternalLevel('05') end )
  MessageCenter:SubscribeEvent('BtnStartLev06Click', function () loadInternalLevel('06') end )
  MessageCenter:SubscribeEvent('BtnStartLev07Click', function () loadInternalLevel('07') end )
  MessageCenter:SubscribeEvent('BtnStartLev08Click', function () loadInternalLevel('08') end )
  MessageCenter:SubscribeEvent('BtnStartLev09Click', function () loadInternalLevel('09') end )
  MessageCenter:SubscribeEvent('BtnStartLev10Click', function () loadInternalLevel('10') end )
  MessageCenter:SubscribeEvent('BtnStartLev11Click', function () loadInternalLevel('11') end )
  MessageCenter:SubscribeEvent('BtnStartLev12Click', function () loadInternalLevel('12') end )
  MessageCenter:SubscribeEvent('ZoneSuDuClick', function () loadInternalLevel('13') end )
  MessageCenter:SubscribeEvent('ZoneLiLiangClick', function () loadInternalLevel('14') end )
  MessageCenter:SubscribeEvent('ZoneNenLiClick', function () loadInternalLevel('15') end )

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
end