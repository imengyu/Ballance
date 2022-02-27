
local Table = require('Table') ---@type Table
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
  "level01",
  "level02",
  "level03",
  "level04",
  "level05",
  "level06",
  "level07",
  "level08",
  "level09",
  "level10",
  "level11",
  "level12",
  "level13"
}
local DefaultHighscoreData = {
  level01 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level02 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level03 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level04 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level05 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level06 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level07 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level08 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level09 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level10 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level11 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
  level12 = {
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
  level13 = Table.DeepCopy(DefaultHightScoreLev01_11Data),
}

return {
  DefaultHightScoreLev01_11Data = DefaultHightScoreLev01_11Data,
  DefaultHighscoreLevelNamesData = DefaultHighscoreLevelNamesData,
  DefaultHighscoreData = DefaultHighscoreData
}