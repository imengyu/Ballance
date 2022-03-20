# Ballance Rebuild 项目教程

这个教程的目的是让您了解如何下载本项目并在Unity中运行起来，以方便您后续开发自定义关卡或者模组。

## 准备

* Unity 2021.2.1 以上版本.
* 编辑器：VScode 或者 Visual Studio
* 克隆本项目 https://github.com/imengyu/Ballance 至您的本地.

## 教程

### 如何下载 Unity ？

Unity 个人版是免费的，你可以在 [Unity官网](https://unity.cn/releases) 上下载（需要先注册个账号）

注意，本项目最低需要 Unity 2021.2.7+ 的版本。

推荐下载 Unity Hub，可在Unity Hub中自动安装 Unity。

### 如何导入本项目至 Unity ？

你可以打开 Unity Hub，选择从本地导入项目，然后选择你下载存放本项目的路径，即可导入。

![image](1.jpg)

导入后即可在列表中找到 Ballance项目，点击打开即可。

### 在编辑器里运行

提示：*(目前暂无Mac/Linux版本的物理引擎文件，请使用Win版本的Unity进行调试)*

1. 使用 Unity 2021.2.7+ 版本打开项目。
2. 第一次运行的时候，你需要点击菜单“SLua”>“All”>“Make” 以生成Lua相关文件，生成之后就不需要再重复点击生成了。
3. 打开 System/Scenes/MainScene.unity 场景。
4. 选择 GameEntry 对象，设置“Debug Type”为“NoDebug”。
5. 点击运行，即可查看效果啦。

### 如何开启发行版的调试模式

调试模式可查看日志，运行测试命令，球可飞行等等。在 UnityEditor

1. 运行游戏
2. 在主菜单 > 关于中，连续点击
3. 重新启动游戏，然后你就进入了调试模式。
