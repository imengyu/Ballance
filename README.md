# Ballance

![image](https://github.com/imengyu/Ballance/blob/master/Assets/System/Textures/splash_app.bmp)

这是2004年Arail 发布的Ballance游戏的开源重制版（制作中）。

## 先睹为快

[先看看演示视频](https://www.bilibili.com/video/BV1Dg411P7xp/)

## 简介

这是Ballance游戏的开源重制版，在制作中，还未完成，但已初具雏形。

游戏使用Unity开发，主要流程大都使用Lua作为语言（为了兼顾MOD与热更），游戏框架使用C#写。

这个项目目前由作者工作之余一个人做，大约5天更新一次。

**如果您想与我一起开发，欢迎 Fork 项目并提交您的修改，我会对您的 PR 非常重视。**

---

![Demo13](https://raw.githubusercontent.com/imengyu/Ballance/main/demo1.png)

![Demo13](https://raw.githubusercontent.com/imengyu/Ballance/main/demo1.gif)

## 目标

* 与原版物理差不多。（2022/01 已完成）
* **ivp物理引擎**。（2022/01 已完成）
* 支持MOD和自定义关卡加载。（已完成）
* 支持用Lua来开发MOD。（已完成）
* 完善游戏主体。

## 欢迎体验

目前仅有Level1体验版，仅有一个关卡，你可以先尝尝鲜。。

体验版文件放在项目根目录 `体验版-Windows-Ballance-Beta.zip` ， 或者点击[这里](https://github.com/imengyu/Ballance/raw/main/%E4%BD%93%E9%AA%8C%E7%89%88-Windows-Ballance-Beta.zip)下载, 解压后运行 `Ballance.exe` 就可以看到效果啦。

Android 体验版放在项目根目录 `体验版-Android-Ballance-Beta.apk` ，想尝尝鲜的话可以安装这个哦（手机版更新不是特别及时，先体验 Windows 版吧）。

目前可能BUG比较多，会有很多问题，不要抱太大的期待哦。。等我慢慢修复了。

## 开发状态

目前游戏主体架构已经开发的差不多了。整体流程已经可以运行了。
可以加载关卡，加载机关，游戏UI，游戏流程基本完成。
目前仅剩细节优化。

<details>
<summary>关于作者</summary>

作者是一个Ballance忠实粉丝，从最初为原版Ballance作图，到后面开发相关的小工具，最后又想让这个老游戏重新焕发生机。
这个项目从2018年就开始了，当时还在B吧里发布过一个测试版本，可惜太烂。中间又高考，停了好长时间，一直到大学快毕业才又想起来，一直想把它做好，可是因为天生拖延症，一拖再拖。到现在工作了，才终于有动力做。

</details>

<details>
<summary>关于物理引擎</summary>

物理引擎使用ivp的源代码，发现这个这个物理引擎真的很意外。通过反编译virtools的physics.dll，然后不断搜索，通过比对，发现，曾经Valve的某个知名游戏发生
源代码泄露事件（hl2）中的物理引擎源代码，与virtools物理引擎里面的字符串居然一模一样，可以说virtools物理引擎就是这个源代码编译出来的。

[物理引擎的C++源代码可以到这里查看](https://github.com/nillerusr/source-physics) (这个不是作者本人的仓库，我在这里复制了一份用来编译).

后来仔细了解了下，才知道这个物理引擎ivp全名是Ipion Virtual Physics，也是很早有名的引擎了（年龄比我还大，我是00后，这个物理引擎是98年的），后来被havok收购，相关的信息应该都被封杀了，互联网上现在已经找不到了。

Virtools诞生比较早，应该也是购买了这个引擎。很幸运，找到了这个引擎，可以让重制版游戏与原版物理效果几乎一模一样的。
</details>

---

## 自定义关卡或者模组

### [制作自定义关卡](./Docs/LevelMaking/readme.md)

**重制版不能直接加载 Virtools 的 nmo 文件！** Virtools 的 nmo/cmo 文件是闭源的，谁也不知道怎么加载它，所以本重制版也不打算支持直接加载 nmo。

* 你需要手工重新制作自定义关卡，制作一个新的关卡包，然后才能加载进入游戏。步骤请参考[制作自定义关卡文档](./Docs/LevelMaking/readme.md)。
* 将制作好的关卡放在 `游戏目录\Ballance_Data\Levels` 文件夹下，打开游戏，在开始中选择 “自定义关卡” 菜单，即可加载自定义关卡。

### [自定义模组](./Docs/LevelMaking/readme.md)

文档TODO。

## 项目编辑器内运行步骤

提示：*(目前暂无Mac/Linux版本的物理引擎文件，请使用Win版本的Unity进行调试)*

1. 请下载 Unity 2021.2.7+ 版本打开项目。
2. 点击菜单“SLua”>“All”>“Make”以生成Lua相关文件。
3. 打开 System/Scenes/MainScene.unity 场景。
4. 选择 GameEntry 对象，设置“Debug Type”为“NoDebug”。
5. 点击运行，即可查看效果啦

## TODO: 项目待完成内容

* ✅ 已完成
* ❎ 完成能用但存在问题
* 🅿 功能有计划但目前暂停开发

---

* ✅ 基础系统
* ✅ 事件系统
* ✅ 操作与数据系统
* ✅ 基础系统
* ✅ 模组加载卸载
* ✅ 模组管理器
* ✅ Lua代码动态载入
* ✅ 模组包功能逻辑
* ✅ 调试命令管理器
* ✅ Lua调试功能
* ✅ 模组包打包功能
* ✅ 关卡包打包功能
* ✅ 逻辑场景
* ✅ Intro进入动画
* ✅ MenuLevel场景
* ✅ MenuLevel的那个滚球动画
* ✅ 主菜单与设置菜单
* ✅ 关于菜单
* ✅ I18N
* ✅ 调试日志输出到unity
* ✅ core主模块独立打包装载
* ✅ BallLightningSphere球闪电动画
* ✅ BallManager球管理器主逻辑
* ✅ TranfoAminControl变球器动画逻辑
* ✅ 球碎片管理器主逻辑
* ✅ CamManager摄像机管理器主逻辑
* ✅ 关于菜单
* ✅ luac代码编译功能
* ✅ LUA 安全性
* ✅ LUA 按包鉴别
* ✅ 模块包安全系研究
* ✅ 【弃用】修复物理坐标问题
* ✅ 【弃用】修复物理约束碰撞问题
* ✅ 【弃用】物理弹簧
* ✅ 【弃用】物理滑动约束
* ✅ LevelEnd
* ✅ LevelBuilder
* ✅ 机关逻辑
* ✅ 简单机关
* ✅ 生命球和分数球机关
* ✅ SectorManager节逻辑
* ✅ GameManager相关逻辑
* ✅ 背景音乐相关逻辑
* ✅ 分数相关逻辑
* ✅ 自动归组
* ✅ ivp 物理引擎的C#包装与编译
* ✅ ivp 物理引擎初步调试成功
* ✅ 将基础球的物理从hovok迁移至ivp物理引擎
* ✅ 将机关的物理从hovok迁移至ivp物理引擎
* ✅ 重写球声音管理器
* ✅ 复杂机关 01
* ✅ 复杂机关 03
* ✅ 复杂机关 08
* ✅ 复杂机关 17
* ✅ 复杂机关 18
* ✅ 复杂机关 19
* ✅ 复杂机关 25
* ✅ 复杂机关 26
* ✅ 复杂机关 29
* ✅ 复杂机关 30
* ✅ 复杂机关 37
* ✅ 复杂机关 41
* ✅ 第1关
* ✅ 第2关
* ✅ 第3关
* ✅ 第4关
* ✅ 第5关
* ✅ 第6关
* ✅ 第7关
* ✅ 第8关
* ✅ 第9关
* ✅ 第10关
* ✅ 第11关
* ✅ 第12关
* ✅ 第13关
* ✅ 迷你机关调试环境
* ✅ 模组管理菜单
* ✅ 关卡管理菜单
* ✅ 关于菜单
* ✅ 手机端适配
* ✅ 过关后才能进入下一关
* ✅ 第一关的教程
* ✅ 菜单的键盘逻辑
* ✅ 手机方向键盘
* ✅ 更换Shader并尽量接近原版材质效果
* ✅ 自定义关卡制作教程文档
* ✅ 添加球的阴影
* ✅ 关卡预览器
* 🅾 最终整体调试
* 🅿 Android and ios 物理模块调试
* 🅿 制作魔脓空间站的转译版本地图并测试整体系统功能
* 🅿 自定义模组开发教程文档

---

* 下面的内容不一定会完成，看大家喜不喜欢，如果大家还喜欢这个游戏，我就继续完善下去
* 🅿 steam接入
* 🅿 发布steam
* 🅿 发布其他平台
* 🅿 更新服务器与联网更新功能
* 🅿 联机玩（关卡，模组，分数共享平台，多人游戏）

## 联系我

wechart: brave_imengyu
