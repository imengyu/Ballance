local json = require("json")
local defaultHighscoreData = require("DefaultHighscoreData")
local HighscoreDataPath = 'HighscoreData.json'

---关卡高分数据
HighscoreData = {
  Data = {},
  LevelNames = {}
}

---高分管理器
---否则管理每个关卡的分数
---@class HighscoreManager
HighscoreManager = {}

---加载。此函数由系统自动调用，勿手动调用
function HighscoreManager.Load()
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
end
---保存。此函数由系统自动调用，勿手动调用
function HighscoreManager.Save()
  Game.Manager:WriteFile(HighscoreDataPath, false, json.encode({
    data = HighscoreData.Data,
    names = HighscoreData.LevelNames
  }))
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
