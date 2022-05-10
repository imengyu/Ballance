# Ballance

[English readme](./README.en.md)

## 简介

这是 Ballance 游戏的开源 Unity 重制版.

![image](/Assets/System/Textures/splash_app.bmp)

---

## 特性

* 原版游戏内容和玩法
* 1-13 关游戏内容
* 自制地图（以魔脓空间站为例）
* 关卡预览器
* 模组管理器
* **使用Lua开发自定义模组或者机关**

## 系统需求

支持系统

* Windows 7 或更高
* MacOS High Sierra 10.13+ (Intel) 或更高
* Android 6.0 或更高

||最低配置|推荐配置|
|---|---|---|
|处理器|Quad core 3Ghz+|Dual core 3Ghz+|
|内存|1 GB RAM (512MB或许也可以运行，但是有可能会OOM) |2 GB RAM|
|显卡|DirectX 10.1 capable GPU with 512 MB VRAM - GeForce GTX 260, Radeon HD 4850 or Intel HD Graphics 5500|DirectX 11 capable GPU with 2 GB VRAM - GeForce GTX 750 Ti, Radeon R7 360|
|DirectX 版本|11|11|
|存储空间|60 MB 可用空间|100 MB 可用空间|

## 安装

1. 前往 [Releases](https://github.com/imengyu/Ballance/releases) 找到最新版本。
2. 下载对应的 zip 安装包。
3. 解压后，运行其中的 `Ballance.exe` 即可开始游戏。

## 开启调试模式

在 UnityEditor 中运行时，永远是调试模式。

### 如果你需要开启独立版的调试模式，可以：

1. 在关于页面，连续点击版本号8次，弹出调试模式提示，
2. 然后重启游戏，就进入了调试模式。
3. 按F12可以开启控制台。

在调试模式中，可以按Q键上升球，E键下降球。

在控制台输入 `quit-dev` 指令可以关闭调试模式。

### 开启所有原版关卡

进入调试模式后在控制台输入 highscore open-all 指令就可以开启全部关卡。

### 项目源码的运行

需要：

* Unity 2021.2.3 以上版本.
* 编辑器：VScode 或者 Visual Studio
* 克隆或者下载本项目 `https://github.com/imengyu/Ballance` 至您的本地.

步骤：

1. 使用 Unity 打开项目。
2. 第一次运行的时候，你需要点击菜单“SLua”>“All”>“Make” 以生成Lua相关文件，生成之后就不需要再重复点击生成了。
3. 打开 `Scenes/MainScene.unity` 场景。
4. 选择 GameEntry 对象，设置“Debug Type”为“NoDebug”。
5. 点击运行，即可查看效果。

## 文档

[完整文档可以参考这里](https://imengyu.github.io/Ballance/#/readme)

[API文档参考这里](https://imengyu.github.io/Ballance/#/LuaApi/readme)

## 物理引擎

物理引擎的C++源代码可以到[这里](https://github.com/nillerusr/source-physics) 查看 (这个不是作者本人的仓库), 如果需要拓展引擎，或者想在你的其他项目中使用这个物理引擎，你需要自己编译源代码。

## 联系我

wechart: brave_imengyu

## 游戏相册

原版关卡

![Demo](docs/DemoImages/11.jpg)
![Demo](docs/DemoImages/12.jpg)
![Demo](docs/DemoImages/13.jpg)
![Demo](docs/DemoImages/14.jpg)
![Demo](docs/DemoImages/18.jpg)
![Demo](docs/DemoImages/9.jpg)
![Demo](docs/DemoImages/6.jpg)
![Demo](docs/DemoImages/7.jpg)
![Demo](docs/DemoImages/15.jpg)
![Demo](docs/DemoImages/16.jpg)
![Demo](docs/DemoImages/17.jpg)

13关的大螺旋

![Demo](docs/DemoImages/9.gif)
![Demo](docs/DemoImages/10.png)

（转译版）自制地图（魔脓空间站）

![Demo](docs/DemoImages/3.jpg)
![Demo](docs/DemoImages/4.jpg)
![Demo](docs/DemoImages/5.jpg)

关卡预览器查看13关

![Demo](docs/DemoImages/8.jpg)

用关卡预览器查看自制地图

![Demo](docs/DemoImages/1.jpg)
![Demo](docs/DemoImages/2.jpg)
