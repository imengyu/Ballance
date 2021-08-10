
---返回绑定在 GameObject 上的第一个 Lua 类实例
---@param go GameObject 要获取的实体
---@return table 类实例，如果为空则返回nil
function GameObjectToLuaClass(go)
  if (not Slua.IsNull(go)) then 
    local host = go:GetComponent(Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost) ---@type GameLuaObjectHost
    if(host ~= nil) then
			return host:GetLuaClass()
		end 
  end
	return nil
end
---返回绑定在 GameObject 上的第一个 GameLuaObjectHost 组件
---@param go GameObject 要获取的实体
---@return GameLuaObjectHost 类实例，如果为空则返回nil
function GameObjectToGameLuaObjectHost(go)
  if (not Slua.IsNull(go)) then 
    return go:GetComponent(Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost)
  else
    return nil
  end
end