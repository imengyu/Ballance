# Ballance Rebuild 发行

本文档将带你了解下如何生成Ballance重制版的游戏文件。

1. 使用 Unity 打开项目
2. 打开 Scenses/MainScense
  1. 第一次运行的时候，你需要点击菜单“SLua”>“All”>“Make” 以生成Lua相关文件，生成之后就不需要再重复点击生成了。
  2. 打开 Scenes/MainScene.unity 场景。
  3. 选择 GameEntry 对象，设置“Debug Type”为“NoDebug”。
4. 生成相关文件：
  1. 单击菜单 “Ballance”>“模块开发”>“打包Packages下所有模块包至Debug目录”，选择目标平台，等待生成完毕。
  2. 单击菜单 “Ballance”>“关卡制作”>“打包所有内置关卡至输出目录”，选择目标平台，等待生成完毕。
  3. 单击菜单 “Ballance”>“工具”>“复制Debug文件夹到预设输出目录”，选择目标平台。
5. 单击菜单 “File”>“Build Settings”>“Build”，生成游戏程序，选择目标平台，选择输出目录为 `项目根目录/Output` (因为上一步相关文件会复制到这里)。
6. 生成完毕，打开游戏运行。
