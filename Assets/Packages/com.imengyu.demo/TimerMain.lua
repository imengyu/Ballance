--获取系统服务
local GameManager = Ballance2.Services.GameManager
local GameUIManager = GameManager.GetSystemService('GameUIManager') ---@type GameUIManager
local GameTimeMachine = GameManager.GetSystemService('GameTimeMachine') ---@type GameTimeMachine
--导入一些定义
local Text = UnityEngine.UI.Text
local Time = UnityEngine.Time

local TimerView = nil ---@type RectTransform
local TimerText = nil ---@type Text


---这个MOD非常简单，他的功能是，计算玩家从开始游戏至过关消耗了多少时间。
---@param thisGamePackage GamePackage
function InitTimerMain(thisGamePackage)
  --这里注册了关卡开始加载事件, 在这个时机就可以初始化我们的代码功能了
  Game.Mediator:RegisterEventHandler(thisGamePackage, 'EVENT_LEVEL_BUILDER_START', 'TimerMainHandler', function (evtName, params)
    
    --在初始化的时候，添加我们在编辑器中预制的Prefab，这个就相当于我们的显示视图了，稍后在游戏中需要显示
    TimerView = GameUIManager:InitViewToCanvas(thisGamePackage:GetPrefabAsset('TimerView.prefab'), 'GameTimerView', false)
    --获取View中的Text组件，稍后显示时间要用
    TimerText = TimerView:Find('TimerText'):GetComponent(Text)

    local GamePlayManager = GamePlay.GamePlayManager

    local UpdateTick = nil ---@type GameTimeMachineTimeTicket
    local UpdateTick2 = nil ---@type GameTimeMachineTimeTicket
    local TimeValue = 0 --当前玩家时间, 单位秒
    local CreateTimer = function () --创建定时器
      --防止重复创建
      if UpdateTick == nil then

        --这里我们注册了一个更新函数，目的是：
        --因为我们这个脚本并没有绑定到GameObjectLuaHost，所以不存在Update函数
        --使用GameTimeMachine管理器可以在任意一个地方注册Update函数，并且可以设置
        --Update的更新频率等等，这样，我们就可以定时更新Text上的时间了
        UpdateTick = GameTimeMachine:RegisterUpdate(function ()
          TimeValue = TimeValue + Time.deltaTime --增加时间
        end, 0, 0) 
        --下面的更新函数用于更新Text，因为Text不需要每帧更新，不然消耗较大
        UpdateTick2 = GameTimeMachine:RegisterUpdate(function ()
          --这里还需要把秒转换下，变成时分秒格式
          local _, ms = math.modf(TimeValue)
          local seconds = math.fmod(TimeValue, 60)
          local min = math.floor(TimeValue / 60)
          local hour = math.floor(min/60) 
          --格式化然后设置到Text上
          TimerText.text = string.format('%d:%02d:%02d.%03d', hour, min, seconds, ms * 1000)
        end, 0, 10) --大约10帧更新一次Text
      end
    end
    local StopTimer = function () --停止定时器
      --防止重复释放
      if UpdateTick ~= nil then
        UpdateTick:Unregister()
        UpdateTick = nil
      end
      if UpdateTick2 ~= nil then
        UpdateTick2:Unregister()
        UpdateTick2 = nil
      end
    end

    --监听游戏开始事件
    GamePlayManager.EventStart:On(function ()
      CreateTimer() --创建并开始定时器
    end)

    --监听游戏暂停事件和下落/死亡事件，我们的计时器也需要停止计时
    GamePlayManager.EventPause:On(function ()
      StopTimer() --停止定时器
    end)
    GamePlayManager.EventFall:On(function ()
      StopTimer() --停止定时器
    end)
    GamePlayManager.EventDeath:On(function ()
      StopTimer() --停止定时器
    end)

    --监听游戏继续事件, 继续时，我们的计时器也需要继续计时
    GamePlayManager.EventResume:On(function ()
      CreateTimer() --开始定时器
    end)

    --监听游戏过关事件, 过关时，停止计时器
    GamePlayManager.EventQuit:On(function ()
      StopTimer() --停止定时器
    end)

    --监听游戏退出事件, 退出时，停止计时器
    GamePlayManager.EventQuit:On(function ()
      StopTimer() --停止定时器
    end)

    return false
  end)
  --这里注册了关卡卸载加载事件，在这个时机释放相关资源
  Game.Mediator:RegisterEventHandler(thisGamePackage, 'EVENT_LEVEL_BUILDER_UNLOAD_START', 'TimerMainHandler', function (evtName, params)
    
    --关卡卸载时，需要移除我们添加的UI
    if not Slua.IsNull(TimerView) then
      UnityEngine.Object.Destroy(TimerView.gameObject)
      TimerText = nil
      TimerView = nil
    end

    return false
  end)
end