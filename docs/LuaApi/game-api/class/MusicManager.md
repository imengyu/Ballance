# BallManager

背景音乐管理器，控制游戏中的背景音乐。

## 事件

* EventMusicThemeChanged 音乐主题变化事件
  * 参数 number 表示当前的音乐主题
* EventMusicDisable 音乐禁用事件
* EventMusicEnable 音乐启用事件

## 方法

### MusicManager:SetCurrentTheme(theme)

设置当前背景音乐预设

#### 参数

`theme` number <br/>预设，原版（1-5），自定义预设参见自定义模组中的自定义音乐预设

### MusicManager:EnableBackgroundMusic()

开启音乐

### MusicManager:DisableBackgroundMusic()

暂停音乐

### MusicManager:DisableBackgroundMusicWithoutAtmo()

暂停音乐（Atmo除外）

### MusicManager:DisableInSec(sec)

从当前时间开始暂停音乐指定秒

#### 参数

`sec` number <br/>
