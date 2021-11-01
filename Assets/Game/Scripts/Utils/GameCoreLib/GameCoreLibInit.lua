require('Print.lua')

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

---获取变量是不是 nil 或空字符串
---@param value any
---@return boolean
function IsNilOrEmpty(value)
  return value == nil or value == ''
end

---带格式和参数打印到 `stdout`。
---@param fmt string 格式
function Printf(fmt, ...)
  print(string.format(tostring(fmt), ...))
end

---检查变量是否为数字，否则返回 0
---@param value any 要检查的变量
---@param base any
---@return integer
function CheckNumber(value, base)
  return tonumber(value, base) or 0
end

---检查变量是否为整形，否则返回 0
---@param value any 要检查的变量
---@return integer
function CheckInt(value)
  return math.tointeger(CheckNumber(value))
end

---检查变量是否为布尔值，否则返回 false
---@param value any 要检查的变量
---@return boolean
function CheckBool(value)
  return (value ~= nil and value ~= false)
end

---检查变量是否为 Table，否则返回一个新的 Table
---@param value table 要检查的变量
---@return table
function CheckTable(value)
  if type(value) ~= "table" then value = {} end
  return value
end

---检查Table是否设置了某个键值
---@param hashtable any Table
---@param key string 键值
---@return boolean
function IsSet(hashtable, key)
  local t = type(hashtable)
  return (t == "table" or t == "userdata") and hashtable[key] ~= nil
end

---获取元素在数组中的索引
---@param table table
---@param item any
---@return integer 索引，如果未找到，则返回-1
function IndexOf(table, item)
  for i = 1, #table, 1 do
    if table[i] == item then
      return i
    end
  end
  return -1
end

---获取元素在数组中最后一个的索引
---@param table table
---@param item any
---@return integer 索引，如果未找到，则返回-1
function IndexOf(table, item)
  for i = #table, 1, -1 do
    if table[i] == item then
      return i
    end
  end
  return -1
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