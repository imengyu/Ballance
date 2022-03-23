# Ballance2.Services.GameSoundManager 
声音管理器

## 注解

声音管理器，用于管理声音部分通用功能，支持快速使用名称播放一个声音。

## 字段

|名称|类型|说明|
|---|---|---|
|TAG|string ||
|GameMainAudioMixer|[AudioMixer](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html) |游戏主AudioMixer|
|GameUIAudioMixer|[AudioMixer](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html) |游戏UI AudioMixer|

## 方法



### LoadAudioResource(assets)

加载模块中的音乐资源


#### 参数


`assets` string <br/>资源路径（模块:音乐路径）



#### 返回值

[AudioClip](https://docs.unity3d.com/ScriptReference/AudioClip.html) <br/>如果加载失败则返回null，否则返回AudioClip实例


### LoadAudioResource(package, assets)

加载模块中的音乐资源


#### 参数


`package` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>所属模块

`assets` string <br/>音乐路径



#### 返回值

[AudioClip](https://docs.unity3d.com/ScriptReference/AudioClip.html) <br/>如果加载失败则返回null，否则返回AudioClip实例


### RegisterSoundPlayer(type, assets, playOnAwake, activeStart, name)

注册一个声音，并设置声音资源文件


#### 参数


`type` number [GameSoundType](./Ballance2.Services.GameSoundType.md)<br/>

`assets` string <br/>音频资源字符串

`playOnAwake` boolean <br/>是否在开始时播放

`activeStart` boolean <br/>播放对象是否开始时激活

`name` string <br/>播放对象的名称



#### 返回值

[AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>返回 AudioSource 实例


### RegisterSoundPlayer(type, audioClip, playOnAwake, activeStart, name)

注册一个声音，并设置声音资源文件


#### 参数


`type` number [GameSoundType](./Ballance2.Services.GameSoundType.md)<br/>

`audioClip` [AudioClip](https://docs.unity3d.com/ScriptReference/AudioClip.html) <br/>音频源文件

`playOnAwake` boolean <br/>是否在开始时播放

`activeStart` boolean <br/>播放对象是否开始时激活

`name` string <br/>播放对象的名称



#### 返回值

[AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>返回 AudioSource 实例


### RegisterSoundPlayer(type, audioSource)

注册已有 AudioSource 至声音管理器


#### 参数


`type` number [GameSoundType](./Ballance2.Services.GameSoundType.md)<br/>声音所属类型

`audioSource` [AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>AudioSource



#### 返回值

[AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>返回原 AudioSource 实例


### IsSoundPlayerRegistered(audioSource)

检查指定 AudioSource 是否已经注册至声音管理器


#### 参数


`audioSource` [AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>AudioSource



#### 返回值

boolean <br/>如果已经注册至声音管理器返回 true，否则返回 false


### DestroySoundPlayer(audioSource)

销毁 AudioSource


#### 参数


`audioSource` [AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>要销毁的 AudioSource 实例



#### 返回值

boolean <br/>返回销毁是否成功


### PlayFastVoice(soundName, type)

快速播放一个短声音


#### 参数


`soundName` string <br/>声音资源字符串

`type` number [GameSoundType](./Ballance2.Services.GameSoundType.md)<br/>声音类型



#### 返回值

boolean <br/>返回播放是否成功


### PlayFastVoice(package, soundName, type)

快速播放一个指定模块包中的短声音资源


#### 参数


`package` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>所属模块

`soundName` string <br/>声音资源路径

`type` number [GameSoundType](./Ballance2.Services.GameSoundType.md)<br/>声音类型



#### 返回值

boolean <br/>返回播放是否成功
