local GameManager = Ballance2.Services.GameManager
local GameUIManager = GameManager.GetSystemService('GameUIManager') ---@type GameUIManager
local GameSettingsManager = Ballance2.Services.GameSettingsManager
local Image = UnityEngine.UI.Image
local EventTriggerListener = Ballance2.Services.InputManager.EventTriggerListener

---手机菜单控制菜单控制类
---@class MobileControlSettingsUIControl : GameLuaObjectHostClass
---@field ScrollView ScrollView
MobileControlSettingsUIControl = ClassicObject:extend()

function MobileControlSettingsUIControl:new() 
  self.items = {}
end
function MobileControlSettingsUIControl:Start() 
  local settings = GameSettingsManager.GetSettings('core')
  local page = GameUIManager:FindPage('PageSettingsControlsMobile')
  local controlKeypadSettting = ''

  ---@param index number
  ---@param item RectTransform
  self.ScrollView.updateFunc = function (index, item)
    local data = self.items[index + 1]
    if data then
      local EventTriggerListener = item:GetComponent(EventTriggerListener) ---@type EventTriggerListener
      local ImageBg = item:Find('ImageBg') 
      local ImageBgActive = item:Find('ImageBgActive') 
      local image = item:Find('Image'):GetComponent(Image) ---@type Image

      if controlKeypadSettting == data.name then
        ImageBg.gameObject:SetActive(false)
        ImageBgActive.gameObject:SetActive(false)
      else
        ImageBg.gameObject:SetActive(false)
        ImageBgActive.gameObject:SetActive(false)
      end

      image.sprite = data.image
      EventTriggerListener.onClick = function ()
        controlKeypadSettting = data.name
        settings:SetString('control.keypad', data.name)

        --更新列表
        self.ScrollView:UpdateData(false)

        --如果游戏正在运行中，则动态更换键盘
        if GamePlay.GamePlayManager then
          GameUI.GamePlayUI:ReBuildMobileKeyPad()
        end
      end
    end
  end
  self.ScrollView.itemCountFunc = function ()
    return #self.items
  end

  page.OnShow = function ()
    controlKeypadSettting = settings:GetString('control.keypad', 'BaseLeft')
    self:LoadList()
    self.ScrollView:UpdateData(false)
  end
end
function MobileControlSettingsUIControl:LoadList() 
  self.items = {}
  for key, value in pairs(KeypadUIManager.GetAllKeypad()) do
    if value then
      table.insert(self.items, {
        name = key,
        image = value.image,
      })
    end
  end
end

function CreateClass:MobileControlSettingsUIControl() 
  return MobileControlSettingsUIControl()
end