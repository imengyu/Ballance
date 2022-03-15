# LUA 类的帮助

游戏框架提供了Lua面向对象编程的方法，可以与Unity MonoBehavior 无缝衔接，这个是lua类的定义。

```lua
-- 这个是lua类的定义

---自定义Lua类，通常配合 GameLuaObjectHostClass 使用。
---@class MyClass : GameLuaObjectHostClass --如果该类被 GameLuaObjectHost 使用, 则它会继承 GameLuaObjectHostClass 类，你可以直接调用相关方法
MyClass = ClassicObject:extend()

--暴露方法给 GameLuaObjectHost
function CreateClass:MyClass()
  return MyClass()
end

--New 函数
function MyClass:new()
  --Do someting
end

---成员函数
function 类名:MyFunction(arg)
end
function 类名:MyFunction2()
end

--[[
如果该类被 GameLuaObjectHost 使用或在 GamePackage 中直接调用 RegisterLuaObject 注册了一个 Lua 对象时，此类可以写MonoBehaviour中的Start、Awake、Update、OnGUI等等和On*函数，具体请参考 GameLuaObjectHost 的说明。
--]]
function MyClass:Start()
  --Do someting
end
```
