
---返回绑定在 GameObject 上的第一个 Lua 类实例
---@param go GameObject 要获取的实体
---@return table 类实例，如果为空则返回nil
function GameObjectToLuaClass(go)
  if (not Slua.IsNull(GameBallsManagerGameObject)) then 
    local host = go:GetComponent(Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost) ---@type GameLuaObjectHost
    return host == nil and nil or host.LuaSelf
  else
    return nil
  end
end
---返回绑定在 GameObject 上的第一个 GameLuaObjectHost 组件
---@param go GameObject 要获取的实体
---@return GameLuaObjectHost 类实例，如果为空则返回nil
function GameObjectToGameLuaObjectHost(go)
  if (not Slua.IsNull(GameBallsManagerGameObject)) then 
    return go:GetComponent(Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost)
  else
    return nil
  end
end