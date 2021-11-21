# Lua 类组件

Lua 类提供了一种类似于MonoBehaviour的lua组件。

## 定义 Lua 类组件

通过下面的代码你可以定义一个 Lua 组件类，它可以被 GameLuaObjectHost 加载并执行Lua代码。

```lua
---@class MyClass : GameLuaObjectHostClass
MyClass = {
  var = nil,
  --自定义属性
}

function CreateClass:MyClass()
  function MyClass:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function MyClass:Start(thisGameObject)
    -- MonoBehaviour Start函数
  end
  function MyClass:OnDestroy()
    -- MonoBehaviour OnDestroy函数
  end

  return MyClass:new(nil)
end
```

## 使用 Lua 类组件

* 可使用代码手动导入 Lua 类组件

```lua
thisGamePackage:RegisterLuaObject('ObjectName', gameObject, 'MyClass')
```

* 也可在编辑器中自动导入 Lua 类组件

在编辑器中，选择目标对象，选择 Component>Ballance>Lua>GameLuaObjectHost，添加一个GameLuaObjectHost组件。

在组件上输入名称、类名、包名，以及其他参数，在其被加载至场景中后会自动加载Lua代码并执行。

## Lua 类事件接收器

默认为了兼顾性能，MonoBehaviour 里面的 On* 这类函数，GameLuaObjectHost 默认不会接收，如果你的脚本需要某些函数，
可以添加对应的事件接收器，需要什么就加什么，lua类中就可以使用这些函数了。

有两种方法可以添加事件接收器：

* 在GameLuaObjectHost组件编辑器中，打开“Lua 类On*事件接收器”，点击按钮添加对应的接收器。
* 使用代码手动添加接收器，例如：

```lua
MyObject:AddComponent('Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectPhysicsEventCaller')
```
