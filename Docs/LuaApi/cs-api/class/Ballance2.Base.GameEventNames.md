# Ballance2.Base.GameEventNames 
游戏内部事件说明

## 字段

|名称|类型|说明|
|---|---|---|
|EVENT_BASE_INIT_FINISHED|string |全局（基础管理器）全部初始化完成时触发该事件|
|EVENT_GAME_MANAGER_INIT_FINISHED|string |GameManager初始化完成时触发该事件，在这个事件后子模块接管控制流程，游戏主逻辑开始运行|
|EVENT_UI_MANAGER_INIT_FINISHED|string |全局（UI管理器）全部初始化完成时触发该事件|
|EVENT_GLOBAL_ALERT_CLOSE|string |全局对话框（Alert，Confirm）关闭时触发该事件|
|EVENT_BEFORE_GAME_QUIT|string |游戏即将退出时触发该事件|
|EVENT_PACKAGE_LOAD_SUCCESS|string |模块加载成功事件|
|EVENT_PACKAGE_LOAD_FAILED|string |模块加载失败事件|
|EVENT_PACKAGE_REGISTERED|string |模块注册事件|
|EVENT_PACKAGE_UNREGISTERED|string |模块已注销事件|
|EVENT_PACKAGE_UNLOAD|string |模块卸载事件|
|EVENT_SCREEN_SIZE_CHANGED|string |屏幕分辨率更改事件|
|EVENT_LOGIC_SECNSE_ENTER|string |进入逻辑场景事件|
|EVENT_LOGIC_SECNSE_QUIT|string |退出逻辑场景事件|
