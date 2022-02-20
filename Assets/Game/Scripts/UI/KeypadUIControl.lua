local EventTriggerListener = Ballance2.Services.InputManager.EventTriggerListener
local GameManager = Ballance2.Services.GameManager

---手机上的游戏控制键盘控制类
---@class KeypadUIControl : GameLuaObjectHostClass
KeypadUIControl = ClassicObject:extend()

function KeypadUIControl:new() 
  self.shiftPressCount = 0
  self.shiftPressOne = false
  self.lastCameraSpaceByShift = false
end
function KeypadUIControl:Start() 
  --自动扫描开头为Button的对象作为按钮，最多扫描2级
  for i = 0, self.transform.childCount - 1, 1 do
    local child = self.transform:GetChild(i);
    if(string.startWith(child.gameObject.name, "Button")) then
      self:AddButton(child.gameObject);
    else 
      for j = 0, child.childCount - 1, 1 do
        local child2 = child:GetChild(j).gameObject;
        if(string.startWith(child2.name, "Button")) then
          self:AddButton(child2);
        end
      end
    end
  end
end
---添加按钮
---@param go GameObject
function KeypadUIControl:AddButton(go) 
  local name = go.name;
  local listener = EventTriggerListener.Get(go);
  local CamManager = Game.GamePlay.CamManager
  local BallManager = Game.GamePlay.BallManager

  if name == "ButtonLeft" then

    --左键
    listener.onDown = function () 
      if self.shiftPressCount == 1 or self.shiftPressOne then
        --按下shift时为旋转摄像机
        CamManager:RotateLeft()
      else
        BallManager:AddBallPush(BallPushType.Left)
      end
    end
    listener.onUp = function () 
      BallManager:RemoveBallPush(BallPushType.Right)
    end

  elseif name == "ButtonRight" then

    --右键
    listener.onDown = function () 
      if self.shiftPressCount == 1 or self.shiftPressOne then
        --按下shift时为旋转摄像机
        CamManager:RotateRight()
      else
        BallManager:AddBallPush(BallPushType.Right)
      end
    end
    listener.onUp = function () 
      BallManager:RemoveBallPush(BallPushType.Right)
    end

  elseif name == "ButtonForward" then

    --前进键
    listener.onDown = function () BallManager:AddBallPush(BallPushType.Forward) end
    listener.onUp = function () BallManager:RemoveBallPush(BallPushType.Forward) end

  elseif name == "ButtonBack" then

    --后退键
    listener.onDown = function () BallManager:AddBallPush(BallPushType.Back) end
    listener.onUp = function () BallManager:RemoveBallPush(BallPushType.Back) end

  elseif name == "ButtonUp" then

    --只有调试模式才显示
    if not GameManager.DebugMode then
      go:SetActive(false)
    else
      --上升键
      listener.onDown = function () BallManager:AddBallPush(BallPushType.Up) end
      listener.onUp = function () BallManager:RemoveBallPush(BallPushType.Up) end
    end


  elseif name == "ButtonDown" then

    --只有调试模式才显示
    if not GameManager.DebugMode then
      go:SetActive(false)
    else
      --下降键
      listener.onDown = function () BallManager:AddBallPush(BallPushType.Down) end
      listener.onUp = function () BallManager:RemoveBallPush(BallPushType.Down) end
    end

  elseif name == "ButtonSpaceShift" then

    --shift和space二合一键
    listener.onDown = function () 
      self.shiftPressCount = self.shiftPressCount + 1
      if self.shiftPressCount >= 2 then
        self.lastCameraSpaceByShift = true
        CamManager:RotateUp(true)
      end
    end
    listener.onUp = function () 
      self.shiftPressCount = self.shiftPressCount - 1
      if self.lastCameraSpaceByShift then
        self.lastCameraSpaceByShift = false
        CamManager:RotateUp(false)
      end
    end
  elseif name == "ButtonSpace" then

    --空格键
    listener.onDown = function () CamManager:RotateUp(true) end
    listener.onUp = function () CamManager:RotateUp(false) end

  elseif name == "ButtonShift" then

    --shift键
    listener.onDown = function () self.shiftPressOne = true end
    listener.onUp = function () self.shiftPressOne = false end

  elseif name == "ButtonCameraLeft" then

    --摄像机左转键
    listener.onClick = function () CamManager:RotateLeft() end

  elseif name == "ButtonCameraRight" then

    --摄像机右转键
    listener.onClick = function () CamManager:RotateRight() end

  end
end

function CreateClass:KeypadUIControl() 
  return KeypadUIControl()
end