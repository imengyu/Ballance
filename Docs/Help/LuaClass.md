# LUA 类的帮助

```lua
-- 这个是lua类的定义

类名 = {

  变量0 = nil,
  变量1 = nil,
  -- 更多变量...
}

function CreateClass_类名()

  function 类名:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---成员函数
  function 类名:MyFunction(arg)
  end
  function 类名:MyFunction2()
  end

  --[[
    如果该类被 GameLuaObjectHost 使用或在 GamePackage 中直接调用 RegisterLuaObject 注册了一个 Lua 对象时，
    此类可以写MonoBehaviour中的Start、Awake、Update、OnGUI等等和On*函数，具体请参考，GameLuaObjectHost的说明。

    start函数
    function 类名:Start(thisGameObject)
    end
    --]]

  return 类名:new(nil)
end
```
