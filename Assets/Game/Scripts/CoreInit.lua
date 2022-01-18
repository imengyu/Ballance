local Intro = require('IntroInit')
local MenuLevel = require('MenuLevelInit')

function CoreInit()
  Intro.Init()
  MenuLevel.Init()
end
function CoreUnload()
  Intro.Unload()
  MenuLevel.Unload()
end