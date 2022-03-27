# ModulBase

机关定义

你可以继承此类来实现自己的机关：例如:
```lua
---@class P_Modul_Test : ModulBase
P_Modul_Test = ModulBase:extend()

function P_Modul_Test:new()
  P_Modul_Test.super.new(self)
  --new 一般是初始化某些变量
end
function P_Modul_Test:Start()
  --初始化时可以执行某些操作
end
function P_Modul_Test:Active()
  self.gameObject:SetActive(true)
  --这是机关激活时执行的操作，例如关激活时一般会物理化组件
  self.P_Modul_Test_Pusher:Physicalize()
end
function P_Modul_Test:Deactive()
  --这是机关失活时执行的操作，例如一般会取消物理化组件
  self.P_Modul_Test_Pusher:UnPhysicalize(true)
  self.gameObject:SetActive(false)
end
function P_Modul_Test:Reset()
  --这是机关重置时执行的操作，一般会恢复当前物体至初始位置（Backup -> Reset）
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
end
function P_Modul_Test:Backup()
  --这是机关初始化时，需要备份当前物体的初始位置
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end

function CreateClass:P_Modul_Test()
  --需要公开此类给 Lua 承载组件
  return P_Modul_Test()
end
```

## 配置

这些配置都是可以更改的，需要在 new 函数中修改，可以使用 `self.*` 来访问。

也可以在编辑器中手动添加变量来设置。

|名称|类型|说明|
|---|---|---|
|EnableBallRangeChecker|boolean|是否开启球区域检测，开启后会定时检测，如果球进入指定范围，则发出 BallEnterRange 事件|
|BallCheckeRange|number|球区域检测范围。创建之后也可以手动设置 modul.BallRangeChecker.Diatance 属性来设置|
|IsActive|boolean|获取当前机关是否激活|
|BallInRange|boolean|获取玩家球是否在当前机关球区域检测范围内|
|IsPreviewMode|boolean|获取当前机关是否在预览模式中加载|
|AutoActiveBaseGameObject|boolean|获取或者设置当前机关基类是否自动控制当前机关的激活与失活|

## 可继承方法

### Active()

机关激活时发出此事件（进入当前小节）

### Deactive()

机关隐藏时发出此事件（当前小节结束）

### UnLoad()

关卡卸载时发出此事件

### Reset(type)

机关重置为初始状态时发出此事件（玩家失败，重新开始一节）。Reset在Deactive之后发出

#### 参数

`type` "sectorRestart"|"levelRestart" <br/>重置类型 sectorRestart 玩家失败，重新开始一节，levelRestart 关卡重新开始

### Backup()

机关就绪，可以保存状态时发出此事件（初次加载完成）

### BallEnterRange()

球进入当前机关指定范围时发出此事件

### BallLeaveRange()

球离开当前机关指定范围时发出此事件

### ActiveForPreview()

在预览模式中激活时发出此事件

### DeactiveForPreview()

在预览模式中隐藏时发出此事件

### Custom(index)

调试环境的自定义调试操作回调

#### 参数

`index` number <br/>按扭参数