
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

---带格式和参数打印到 `stdout`。
---@param fmt string 格式
function Printf(fmt, ...)
  print(string.format(tostring(fmt), ...))
end

function CheckNumber(value, base)
  return tonumber(value, base) or 0
end

function CheckInt(value)
  return math.tointeger(CheckNumber(value))
end

function CheckBool(value)
  return (value ~= nil and value ~= false)
end

function CheckTable(value)
  if type(value) ~= "table" then value = {} end
  return value
end

function IsSet(hashtable, key)
  local t = type(hashtable)
  return (t == "table" or t == "userdata") and hashtable[key] ~= nil
end

---克隆一个表
---@param object table 原表
---@return table 返回新的表
function Clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local newObject = {}
        lookup_table[object] = newObject
        for key, value in pairs(object) do
            newObject[_copy(key)] = _copy(value)
        end
        return setmetatable(newObject, getmetatable(object))
    end
    return _copy(object)
end
