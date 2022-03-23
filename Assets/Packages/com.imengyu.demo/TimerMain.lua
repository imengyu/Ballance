---这个MOD非常简单，他的功能是，计算玩家从开始游戏至过关消耗了多少时间。
---@param thisGamePackage GamePackage
function InitTimerMain(thisGamePackage)
  --这里注册了关卡开始加载事件, 在这里就可以初始化我们的代码模组了
  Game.Mediator:RegisterEventHandler(thisGamePackage, 'EVENT_LEVEL_BUILDER_START', 'TimerMainHandler', function (evtName, params)
    


    return false
  end)
  --这里注册了关卡卸载加载事件，释放相关资源
  Game.Mediator:RegisterEventHandler(thisGamePackage, 'EVENT_LEVEL_BUILDER_UNLOAD_START', 'TimerMainHandler', function (evtName, params)
    
    return false
  end)
end