local Text = UnityEngine.UI.Text
local I18N = Ballance2.Sys.Language.I18N

---分数统计界面控制
---@class HighscoreUIControl : GameLuaObjectHostClass
---@field TextLevelName Text
HighscoreUIControl = ClassicObject:extend()

function HighscoreUIControl:new() 
end
function HighscoreUIControl:Start() 

  self._CurrentIndex = 1
  self._TextItem = {}
  for i = 1, 10, 1 do
    self._TextItem[i] = {
      textName = self.transform:Find('ItemHighscore'..i..'/TextValue'):GetComponent(Text), ---@type Text
      testScore = self.transform:Find('ItemHighscore'..i..'/TextScoreValue'):GetComponent(Text) ---@type Text
    }
  end

  GameUI.HighscoreUIControl = self
end

---加载关卡分数数据
---@param name string|number
function HighscoreUIControl:LoadLevelData(name) 

  self.LevelNames = Game.HighScoreManager.GetLevelNames()

  local setIndex = true
  if type(name) == 'number' then
    name = self.LevelNames[name]
    setIndex = false
  end

  local data = Game.HighScoreManager.GetData(name)
  if data ~= nil then
    if setIndex then
      self._CurrentIndex = IndexOf(self.LevelNames, name)
    end

    for i = 1, 10, 1 do
      local item = data[i]
      if item ~= nil then
        self._TextItem[i].textName.text = item.name
        self._TextItem[i].testScore.text = tostring(item.score)
      else
        self._TextItem[i].textName.text = ''
        self._TextItem[i].testScore.text = ''
      end
    end    

    self.TextLevelName.text = name
  else
    self._CurrentIndex = 1
    for i = 1, 10, 1 do
      self._TextItem[i].textName.text = ''
      self._TextItem[i].testScore.text = ''
    end  
    self.TextLevelName.text = I18N.Tr('ui.noLevelData')
  end
end
function HighscoreUIControl:Next() 
  if self._CurrentIndex < #self.LevelNames then
    self._CurrentIndex = self._CurrentIndex + 1
  else
    self._CurrentIndex = 1
  end
  self:LoadLevelData(self._CurrentIndex)
end
function HighscoreUIControl:Prev() 
  if self._CurrentIndex > 1 then
    self._CurrentIndex = self._CurrentIndex - 1
  else
    self._CurrentIndex = #self.LevelNames
  end
  self:LoadLevelData(self._CurrentIndex)
end


function CreateClass_HighscoreUIControl() 
  return HighscoreUIControl
end