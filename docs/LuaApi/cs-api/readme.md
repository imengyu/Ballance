# Ballance 项目 C# 框架 公开的 API

框架向Lua提供了很多的 API，你可以按需使用。

框架的类基本存放在 Ballance.* 命名空间下，你可以导入后使用

```lua
local GameManager = Ballance2.Services.GameManager

GameManager.Instance:RequestEnterLogicScense('Level')
```

框架大致分为两个模块：

* 主框架，命名空间以 `Ballance2` 开头。
* 物理引擎框架，命名空间以 `BallancePhysics` 开头。

具体模块说明请参考每个模块的详细文档。
