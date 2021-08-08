local KeyCode = UnityEngine.KeyCode
local Log = Ballance2.Utils.Log
local KeyListener = Ballance2.Sys.Utils.KeyListener
local Vector3 = UnityEngine.Vector3
local GameSettingsManager = Ballance2.Config.GameSettingsManager
local GameErrorChecker = Ballance2.Sys.Debug.GameErrorChecker
local GameError = Ballance2.Sys.Debug.GameError
local LuaUtils = Ballance2.Utils.LuaUtils

---指定球的控制状态
BallControlStatus = {
  ---没有控制（无控制、无物理效果，无摄像机跟随）
  NoControl = 0,
  ---正常控制（可控制、物理效果，摄像机跟随）
  Control = 1,
  ---释放模式（例如球坠落，球仍然有物理效果，但无法控制，摄像机不跟随）
  UnleashingMode = 2,
  ---锁定模式（例如变换球时，无物理效果，无法控制，但摄像机跟随）
  LockMode = 3,
}

---@type GameLuaObjectHostClass
---@class BallManager
BallManager = {
  -- editor
  _BallLightningSphere = nil, ---@type GameLuaObjectHost
  _BallWood = nil, ---@type GameObject
  _BallStone = nil, ---@type GameObject
  _BallPaper = nil, ---@type GameObject
  _BallSmoke = nil, ---@type GameObject

  -- public

  --当前球（仅获取，设置请使用SetCurrentBall方法）
  CurrentBall = '',

  -- private
  _private = {
    BallLightningSphere = nil, ---@type BallLightningSphere
    GameSettings = nil, ---@type GameSettingsActuator
    settingsCallbackId = 0,
    controllingStatus = BallControlStatus.NoControl, ---当前状态
    reverseControl = false, ---是否反向控制
    registerBalls = {}, ---已注册的球
    keyListener = nil, ---@type KeyListener
    ---控制按键设置
    keySets = {
      keyFront = KeyCode.UpArrow,
      keyBack = KeyCode.DownArrow,
      keyLeft = KeyCode.LeftArrow,
      keyRight = KeyCode.RightArrow,
      keyUpCamera = KeyCode.Space,
      keyRoateCamera = KeyCode.LeftShift,
      keyFront2 = KeyCode.W,
      keyBack2 = KeyCode.S,
      keyLeft2 = KeyCode.A,
      keyRight2 = KeyCode.D,
      keyRoateCamera2 = KeyCode.RightShift,
      keyUp = KeyCode.Q,
      keyDown = KeyCode.E,
    }, 
  }
}

local TAG = 'BallManager'

function CreateClass_BallManager()
  function BallManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function BallManager:Start(thisGameObject)
    self._private.BallLightningSphere = self._BallLightningSphere:GetLuaClass()

    self:RegisterBall('BallWood', self._BallWood)
    self:RegisterBall('BallStone', self._BallStone)
    self:RegisterBall('BallPaper', self._BallPaper)

    --初始化控制设置
    local GameSettings = GameSettingsManager.GetSettings("core")
    self._private.settingsCallbackId = GameSettings:RegisterSettingsUpdateCallback("control", self.OnControlSettingsChanged)
    GameSettings:RequireSettingsLoad("control")
    self._private.GameSettings = GameSettings

    --初始化键盘侦听
    self._private.keyListener = KeyListener.Get(self.gameObject)
    self._private.keyListener:AddKeyListen(KeyCode.T, function (key, downed)
      if(downed) then
        self._private.BallLightningSphere:PlayLighting(Vector3.zero, true, true)
      end
    end)
    self._private.keyListener:AddKeyListen(KeyCode.R, function (key, downed)
      if(downed) then
        Log.I('test', 'R')
      end
    end)
  end
  function BallManager:OnDestroy()
    self._private.GameSettings:UnRegisterSettingsUpdateCallback(self._private.settingsCallbackId)
    self._private.keyListener:ClearKeyListen()
  end
  function BallManager:Update()

  end

  --#region 球基础控制方法

  ---注册球 
  ---@param name string 球名称
  ---@param gameObject string 球对象
  function BallManager:RegisterBall(name, gameObject)
    if(self:GetRegisterBall(name) ~= nil) then
      GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, 'Ball {0} already registered', { name })
      return
    end
    table.insert(self._private.registerBalls, {
      name = name,
      gameObject = gameObject
    })
  end
  ---取消注册球 
  ---@param name string 球名称
  function BallManager:UnRegisterBall(name)
    local registerBalls = self._private.registerBalls
    for i = 1, #registerBalls do  
      if(registerBalls[i].name == name) then
        table.remove(registerBalls, i)
        break
      end 
    end  
  end
  ---获取注册了的球 
  ---@param name string 球名称
  function BallManager:GetRegisterBall(name)
    local registerBalls = self._private.registerBalls
    for i = 1, #registerBalls do  
      if(registerBalls[i].name == name) then
        return registerBalls[i].gameObject
      end 
    end  
    return nil
  end
  ---设置当前正在控制的球 
  ---@param name string 球名称
  function BallManager:SetCurrentBall(name)

  end
  ---设置当前球的控制的状态
  ---@param status number 新的状态
  function BallManager:SetControllingStatus(status)
    self._private.controllingStatus = status

  end

  --#endregion

  --#region 球附加工具方法

  ---播放球出生时的烟雾
  ---@param pos Vector3 放置位置
  function BallManager:PlaySmoke(pos)
    self._BallSmoke.transform.position = pos
    self._BallSmoke:SetActive(true)
  end
  ---播放球出生时的闪电效果
  ---@param pos Vector3 放置位置
  ---@param smallToBig boolean 是否由小到大
  ---@param lightAnim boolean 是否同时播放灯光效果
  function BallManager:PlayLighting(pos, smallToBig, lightAnim) 
    self._private.BallLightningSphere:PlayLighting(pos, smallToBig, lightAnim)
  end

  --#endregion

  --#region 键盘设置与初始化

  function BallManager:InitKeyEvents()
    --添加按键事件
    local keyListener = self._private.keyListener
    local keySets = self._private.keySets
    keyListener.ClearKeyListen()
    keyListener.AddKeyListen(keySets.keyFront, keySets.keyFront2, self.UpArrow_Key)
    keyListener.AddKeyListen(keySets.keyBack, keySets.keyBack2, self.DownArrow_Key)
    keyListener.AddKeyListen(keySets.keyUp, self.Up_Key)
    keyListener.AddKeyListen(keySets.keyDown,self.Down_Key)
    keyListener.AddKeyListen(keySets.keyUpCamera, self.Space_Key)
    keyListener.AddKeyListen(keySets.keyRoateCamera, keySets.keyRoateCamera2, self.Shift_Key)

    --是否反向控制  
    if(self._private.reverseControl) then
      keyListener.AddKeyListen(keySets.keyLeft, keySets.keyLeft2, self.RightArrow_Key)
      keyListener.AddKeyListen(keySets.keyRight, keySets.keyRight2, self.LeftArrow_Key)
    else
      keyListener.AddKeyListen(keySets.keyLeft, keySets.keyLeft2, self.LeftArrow_Key)
      keyListener.AddKeyListen(keySets.keyRight, keySets.keyRight2, self.RightArrow_Key)
    end

  end
  function BallManager:OnControlSettingsChanged()
    --当设置更改时或加载时，更新设置到当前变量
    local GameSettings = self._private.GameSettings
    local keySets = self._private.keySets
    keySets.keyFront = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.front", "UpArrow"))
    keySets.keyFront2 = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.front2", "W"))
    keySets.keyUp = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.up", "Q"))
    keySets.keyDown = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.down", "E"))
    keySets.keyBack = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.back", "DownArrow"))
    keySets.keyBack2 = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.back2", "S"))
    keySets.keyLeft = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.left", "LeftArrow"))
    keySets.keyLeft2 = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.left2", "A"))
    keySets.keyRight = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.right", "RightArrow"))
    keySets.keyRight2 = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.right2", "D"))
    keySets.keyRoateCamera = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.roate", "LeftShift"))
    keySets.keyRoateCamera2 = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.roate2", "RightShift"))
    keySets.keyUpCamera = LuaUtils.StringToKeyCode(GameSettings.GetString("control.key.up_cam", "Space"))

    self._private.reverseControl = GameSettings.GetBool("control.reverse", false)
    self.InitKeyEvents()
  end

  --#endregion

  --#region 键盘事件

  function BallManager:UpArrow_Key(key, down)
    
  end
  function BallManager:DownArrow_Key(key, down)
    
  end
  function BallManager:RightArrow_Key(key, down)
    
  end
  function BallManager:LeftArrow_Key(key, down)
    
  end
  function BallManager:Down_Key(key, down)
    
  end
  function BallManager:Up_Key(key, down)
    
  end
  function BallManager:Space_Key(key, down)
    
  end
  function BallManager:Shift_Key(key, down)
    
  end

  --#endregion

  return BallManager:new(nil)
end