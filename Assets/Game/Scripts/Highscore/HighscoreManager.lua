local json = Game.SystemPackage:RequireLuaFile("json") ---@type json
local HighscoreDataPath = 'HighscoreData.json'

---关卡高分数据
HighscoreData = {
  Data = {},
  LevelNames = {}
}

function HighscoreManagerLoad()
  if Game.Manager:FileExists(HighscoreDataPath) then 
    local str = Game.Manager:ReadFile(HighscoreDataPath)
    local data = json.decode(str)
    HighscoreData.Data = data.data
    HighscoreData.LevelNames = data.names

    if data.data == nil then
      HighscoreData.Data = DefaultHighscoreData
      HighscoreData.LevelNames = DefaultHighscoreLevelNamesData
    end
  else
    HighscoreData.Data = DefaultHighscoreData
    HighscoreData.LevelNames = DefaultHighscoreLevelNamesData
  end
end
function HighscoreManagerSave()
  Game.Manager:WriteFile(HighscoreDataPath, false, json.encode({
    data = HighscoreData.Data,
    names = HighscoreData.LevelNames
  }))
end
function HighscoreManagerAddItem(levelName, userName, score)
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
function HighscoreManagerCheckLevelHighScore(levelName, score)
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
function HighscoreManagerTryAddDefaultLevelHighScore(levelName, data)
  if HighscoreData.Data[levelName] == nil then
    HighscoreData.Data[levelName] = data
    table.insert(HighscoreData.LevelNames, levelName)
  end
end
