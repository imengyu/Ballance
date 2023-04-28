# <center>Ballance 平衡球 C#/Unity 实现</center>

<p align="center">
  简体中文 | <a href="README.md">English</a>
</p>
<p align="center">
  <a style="text-decoration:none">
    <img src="https://img.shields.io/badge/unity-2021.3.X-blue?style=flat-square" alt="Unity Version" />
  </a>
  <a style="text-decoration:none" href="https://github.com/imengyu/ballance/releases">
    <img src="https://img.shields.io/github/v/release/imengyu/ballance.svg?label=alpha&style=flat-square&color=yellow" alt="Releases" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/badge/platform-Win%20%7C%20Mac%20%7C%20iOS%20%7C%20Android-orange?style=flat-square" alt="Platform" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/badge/license-GPL--3.0-green?style=flat-square" alt="License" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/repo-size/imengyu/ballance?style=flat-square" alt="Size" />
  </a>
</p>

---

![image](/Assets/Game/Common/Textures/UI/LogoPicture.jpg)

* [Gitee 国内镜像](https://gitee.com/imengyu/Ballance)
* [Github](https://github.com/imengyu/Ballance)

## 简介

这是 Ballance 游戏的开源 Unity 重制版.

这是一个使用C#/Unity重新实现的 Ballance 平衡球游戏 的开源项目（注意：不是移植，不是套壳打包，是重写）。实现原理是根据[反编译的游戏脚本](https://github.com/BearKidsTeam/BallanceModding)，在Unity引擎中依照相似原理重新实现。具体实现方法请阅读源代码。

**注意：Ballance属于Cyparade作品，版权属于原作开发商Cyparade所有，本项目开发仅作为学习用途，不可用于任何商业用途。本项目遵循GPL-3.0协议，但仅限于此项目的代码，任何与Ballance有关的3D模型数据、物理引擎、图片，音视频，游戏数据均不在此范围，任何未经版权方许可的情况下使用这些游戏数据进行商业行为都是违法的。**

本项目完成了原版的特性：

* 原版游戏内容和玩法
* 1-13 关游戏内容
* 物理效果相似度 85%

本项目相对于原版增加了以下一些特性：

* **直接加载 NMO 文件**（仅Windows版本）
* Android 版本、Mac版本
* 调整窗口化、全屏、分辨率、帧率、物理速率、球速
* 自制地图接口
* 模组、机关接口（使用C#开发自定义模组或者机关)
* 关卡预览器
* 模组管理器

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

* Windows:

1. 前往 Releases 找到最新版本。
2. 下载对应的 zip 安装包。
3. 解压后，运行其中的 `Ballance.exe` 即可开始游戏。

* MacOS：

  待完成

* Android：

  待完成

* iOS:

  待完成

## 按键以及操作

* PC版操作与原版游戏一直，并无改动。你可以在 设置>控制 菜单中修改按键。
* 手机版增加了触摸键盘，你可以在 设置>控制 菜单中修改键盘样式。

## 开启调试模式

调试模式下球可以飞行，你可以用它来作弊或者测试关卡。

### 开启

1. 在关于页面，连续点击版本号8次，弹出调试模式提示，
2. 然后重启游戏，就进入了调试模式。
3. 按F12可以开启控制台。

在调试模式中，可以按Q键上升球，E键下降球。

在控制台输入 `quit-dev` 指令可以关闭调试模式。

### 开启所有原版关卡

进入调试模式后在控制台输入 highscore open-all 指令就可以开启全部关卡。

## 直接加载 NMO 文件 【NEW】

Ballance Unity Rebuild 0.9.8 版本支持了加载 Ballance 原版关卡文件的功能。

你可以加载通过点击 “开始” > “NMO 关卡” 来加载一个标准的原版关卡。

核心使用 Virtools SDK 5.0 来处理 NMO 文件，因此只支持 Windows 32位 版本。

大部分关卡可以加载成功并且游玩，但目前有少数限制：

* 不能加载带有 Virtools 脚本的关卡。
* 不支持 Virtools 的点、线网格。
* 材质不支持 Virtools 的特殊效果，将使用默认材质代替。
* 不支持设置关卡天空盒、关卡分数，没有背景音乐。

### 项目源码的运行

需要：

* Unity 2021.3.2 以上版本.
* 编辑器：VScode 或者 Visual Studio
* 克隆或者下载本项目 `https://github.com/imengyu/Ballance` 至您的本地.

步骤：

1. 使用 Unity 打开项目。
2. 打开 `Scenes/MainScene.unity` 场景。
3. 选择 GameEntry 对象，设置“Debug Type”为“NoDebug”。
4. 点击运行，即可查看效果。

## 从项目源码生成游戏程序

请参考 [文档](wiki/production(zh).md)。

## 物理引擎

物理引擎的C++源代码可以到[这里](https://github.com/nillerusr/source-physics) 查看 (这个不是作者本人的仓库)。

如果需要拓展引擎，或者想在你的其他项目中使用这个物理引擎，你需要自己编译源代码。

物理引擎的包装DLL代码在项目下方 BallancePhysics 目录下，你需要使用 Visual Studio 2919 以上版本编译。

## 项目进度以及路线图

原版玩法以及关卡复刻已经全部完成，你可以完整的从头玩到尾体验一遍游戏，也可参照开发接口制作自定义关卡。下一步作者会根据大家的反馈继续开发下去，增加更多功能，例如好玩的机关、关卡编辑器等等。

## 如何贡献？

如果您有好的修改，或问题修复者欢迎PR。如果您有好的想法意见, 或者发现了Bug请欢迎提交issue或者加我的微信我讨论。

## 联系我

wechart: brave_imengyu （请备注讨论Ballance）

## 为什么要做这个？

Ballance Unity 是作者的一个小梦想，希望让 Ballance 可以运行在手机上，希望让 Ballance 可以方便的拓展功能开发关卡、模组（后者已经被 [BallanceModLoader](https://github.com/Gamepiaynmo/BallanceModLoader) 实现了）。同时，Ballance Unity 也是我学习 Unity 游戏开发的第一个作品，它对我的技术提升给了非常大的帮助。

## 游戏相册

原版关卡

![Demo](./DemoImages/11.jpg)
![Demo](./DemoImages/12.jpg)
![Demo](./DemoImages/13.jpg)
![Demo](./DemoImages/14.jpg)
![Demo](./DemoImages/18.jpg)
![Demo](./DemoImages/9.jpg)
![Demo](./DemoImages/6.jpg)
![Demo](./DemoImages/7.jpg)
![Demo](./DemoImages/15.jpg)
![Demo](./DemoImages/16.jpg)
![Demo](./DemoImages/17.jpg)

13关的大螺旋

![Demo](./DemoImages/9.gif)
![Demo](./DemoImages/10.png)

（转译版）自制地图（魔脓空间站）

![Demo](./DemoImages/3.jpg)
![Demo](./DemoImages/4.jpg)
![Demo](./DemoImages/5.jpg)

关卡预览器

![Demo](./DemoImages/1.jpg)
![Demo](./DemoImages/2.jpg)

## 项目所使用开源项目

* https://github.com/chrisnolet/QuickOutline
* https://github.com/seedov/AsyncAwaitUtil/
* https://github.com/Tayx94/graphy
* https://github.com/yasirkula/UnityRuntimeSceneGizmo
* https://github.com/yasirkula/UnityRuntimeInspector
* https://github.com/yasirkula/UnitySimpleFileBrowser
