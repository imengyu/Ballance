local json = require("json")
local defaultHighscoreData = require("DefaultHighscoreData")
local Application = UnityEngine.Application
local HighscoreDataPath = Application.persistentDataPath..'/HighscoreData.json'
local LevelPassStateDataPath = Application.persistentDataPath..'/LevelPassStateData.json'
local GameManager = Ballance2.Services.GameManager
local Log = Ballance2.Log

---关卡高分数据
HighscoreData = {
  Data = {},
  LevelPassStateData = {},
  LevelNames = {}
}

---高分管理器
---负则管理每个关卡的分数
---@class HighscoreManager
HighscoreManager = {}

---加载。此函数由系统自动调用，勿手动调用
function HighscoreManager.Load()
  HighscoreManager.InitCommand()
  --加载关卡高分数据
  if Game.Manager:FileExists(HighscoreDataPath) then 
    local str = Game.Manager:ReadFile(HighscoreDataPath)
    local data = json.decode(str)
    HighscoreData.Data = data.data
    HighscoreData.LevelNames = data.names

    if data.data == nil then
      HighscoreData.Data = defaultHighscoreData.DefaultHighscoreData
      HighscoreData.LevelNames = defaultHighscoreData.DefaultHighscoreLevelNamesData
    end
  else
    HighscoreData.Data = defaultHighscoreData.DefaultHighscoreData
    HighscoreData.LevelNames = defaultHighscoreData.DefaultHighscoreLevelNamesData
  end
  --加载关卡过关状态数据
  if Game.Manager:FileExists(LevelPassStateDataPath) then 
    local str = Game.Manager:ReadFile(LevelPassStateDataPath)
    local data = json.decode(str)
    if data.data ~= nil then
      HighscoreData.LevelPassStateData = data.data
    end
  end
end
---保存。此函数由系统自动调用，勿手动调用
function HighscoreManager.Save()

  --保存关卡高分数据
  Game.Manager:WriteFile(HighscoreDataPath, false, json.encode({
    data = HighscoreData.Data,
    names = HighscoreData.LevelNames
  }))

  --保存关卡过关状态数据
  Game.Manager:WriteFile(LevelPassStateDataPath, false, json.encode({
    data = HighscoreData.LevelPassStateData
  }))

end

local HighscoreManagerCommand = nil

function HighscoreManager.InitCommand()
  if HighscoreManagerCommand == nil then
    GameManager.Instance.GameDebugCommandServer:RegisterCommand('highscore', function (keyword, fullCmd, argsCount, args)
      local type = args[0]
      if type == 'clear' then
        HighscoreData.Data = defaultHighscoreData.DefaultHighscoreData
        HighscoreData.LevelNames = defaultHighscoreData.DefaultHighscoreLevelNamesData
        Log.D('highscore', 'Reset to default')
        return true
      elseif type == 'open-all' then
        HighscoreData.LevelPassStateData['level01'] = true
        HighscoreData.LevelPassStateData['level02'] = true
        HighscoreData.LevelPassStateData['level03'] = true
        HighscoreData.LevelPassStateData['level04'] = true
        HighscoreData.LevelPassStateData['level05'] = true
        HighscoreData.LevelPassStateData['level06'] = true
        HighscoreData.LevelPassStateData['level07'] = true
        HighscoreData.LevelPassStateData['level08'] = true
        HighscoreData.LevelPassStateData['level09'] = true
        HighscoreData.LevelPassStateData['level10'] = true
        HighscoreData.LevelPassStateData['level11'] = true
        HighscoreData.LevelPassStateData['level12'] = true
        Log.D('highscore', 'Open level01-level12')
        return true
      elseif type == 'load' then 
        HighscoreManager.Load()
        Log.D('highscore', ' Load manually')
        return true
      elseif type == 'save' then 
        HighscoreManager.Save()
        Log.D('highscore', 'Save manually')
        return true
      else
        Log.E('highscore', 'Unknow type '..type)
        return false
      end
    end, 1, '')
  end
end

---获取指定关卡的分数列表
---@param levelName string
function HighscoreManager.GetData(levelName)
  return HighscoreData.Data[levelName]
end
---获取分数管理器中有存储数据的所有关卡名称
function HighscoreManager.GetLevelNames()
  return HighscoreData.LevelNames
end

---在指定关卡添加用户的分数数据
---@param levelName string 关卡名称
---@param userName string 名字
---@param score number 分数
function HighscoreManager.AddItem(levelName, userName, score)
  if HighscoreData.Data[levelName] == nil then
    HighscoreData.Data[levelName] = {}
    table.insert(HighscoreData.LevelNames, levelName)
  end
  --设置已经过关
  HighscoreData.LevelPassStateData[levelName] = true

  local levelData = HighscoreData.Data[levelName]
  for index, value in ipairs(levelData) do
    if score > value.score then
      --插入到指定位置
      table.insert(levelData, index, {
        name = userName, score = score, date = os.date("%Y/%m/%d")
      })
      return
    end
  end
  --插入到最后
  table.insert(levelData, {
    name = userName, score = score, date = os.date("%Y/%m/%d")
  })
end

---检查指定分数是否在关卡有新的高分
---@param levelName string 关卡名称
---@param score number 分数
---@return boolean 是否有新的高分
function HighscoreManager.CheckLevelHighScore(levelName, score)
  local levelData = HighscoreData.Data[levelName]
  if levelData ~= nil then
    for _, value in ipairs(levelData) do
      if value.score > score then
        return false
      end
    end
  end
  return true
end

---检查指定关卡是否有过关记录
---@param levelName string 关卡名称
function HighscoreManager.CheckLevelPassState(levelName)
  local levelData = HighscoreData.LevelPassStateData[levelName]
  return levelData == true
end

---添加默认分数至指定关卡中
---@param levelName string 关卡名称
---@param data table 默认数据，可为 nil，为 nil 时根据在 DefaultHighscoreData 中定义的 defaultHighscoreData.DefaultHightScoreLev01_11Data 加载数据
function HighscoreManager.TryAddDefaultLevelHighScore(levelName, data)
  if HighscoreData.Data[levelName] == nil then
    HighscoreData.Data[levelName] = data or defaultHighscoreData.DefaultHightScoreLev01_11Data
    table.insert(HighscoreData.LevelNames, levelName)
  end
end

return HighscoreManager
