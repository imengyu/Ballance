local Time = UnityEngine.Time
local Color = UnityEngine.Color

local GamePackage = Ballance2.Package.GamePackage

---第一关教程管理器
---@class Tutorial : GameLuaObjectHostClass
Tutorial = ClassicObject:extend()

function Tutorial:new()
  self._Tutorial = false
end
function Tutorial:Start()
  Game.Mediator:RegisterEventHandler(GamePackage.GetCorePackage(), 'CoreTutorialLevelEventHandler', 'TutorialHandler', function (evtName, params)
    if params[1] == 'beforeStart' then
      Game.Mediator:RegisterEventHandler(GamePackage.GetCorePackage(), 'GAME_START', 'TutorialHandler', function ()
        GamePlay.GamePlayManager._ShouldStartByCustom = true
        

        self._Tutorial = true
        return false
      end)
      Game.Mediator:RegisterEventHandler(GamePackage.GetCorePackage(), 'GAME_QUIT', 'TutorialHandler', function ()
        self._Tutorial = false
        return false
      end)
    end
    return false
  end)
end

function CreateClass:Tutorial()
  return Tutorial()
end