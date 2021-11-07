
local Table = require('Table')
local DefaultHightScoreLev01_11Data = {
  { name = "Mr. Default", score = 4000, date = "2004/8/8" },
  { name = "Mr. Default", score = 3600, date = "2004/8/8" },
  { name = "Mr. Default", score = 3200, date = "2004/8/8" },
  { name = "Mr. Default", score = 2800, date = "2004/8/8" },
  { name = "Mr. Default", score = 2400, date = "2004/8/8" },
  { name = "Mr. Default", score = 2000, date = "2004/8/8" },
  { name = "Mr. Default", score = 1600, date = "2004/8/8" },
  { name = "Mr. Default", score = 1200, date = "2004/8/8" },
  { name = "Mr. Default", score = 800, date = "2004/8/8" },
  { name = "Mr. Default", score = 400, date = "2004/8/8" },
}

local DefaultHighscoreLevelNamesData = {
  "Level01",
  "Level02",
  "Level03",
  "Level04",
  "Level05",
  "Level06",
  "Level07",
  "Level08",
  "Level09",
  "Level10",
  "Level11",
  "Level12",
  "Level13"
}
local DefaultHighscoreData = {
  Level01 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level02 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level03 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level04 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level05 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level06 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level07 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level08 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level09 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level10 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level11 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  Level12 = {
    { name = "Mr. Default", score = 7000, date = "2004/8/8" },
    { name = "Mr. Default", score = 6600, date = "2004/8/8" },
    { name = "Mr. Default", score = 6200, date = "2004/8/8" },
    { name = "Mr. Default", score = 5800, date = "2004/8/8" },
    { name = "Mr. Default", score = 5400, date = "2004/8/8" },
    { name = "Mr. Default", score = 5000, date = "2004/8/8" },
    { name = "Mr. Default", score = 4600, date = "2004/8/8" },
    { name = "Mr. Default", score = 4200, date = "2004/8/8" },
    { name = "Mr. Default", score = 3800, date = "2004/8/8" },
    { name = "Mr. Default", score = 3600, date = "2004/8/8" },
  },
  Level13 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
}

return {
  DefaultHightScoreLev01_11Data = DefaultHightScoreLev01_11Data,
  DefaultHighscoreLevelNamesData = DefaultHighscoreLevelNamesData,
  DefaultHighscoreData = DefaultHighscoreData
}