---获取变量是不是 nil 或空字符串
---@param value any
---@return boolean
function IsNilOrEmpty(value)
  return value == nil or value == ''
end

---检查Table是否设置了某个键值
---@param hashtable any Table
---@param key string 键值
---@return boolean
function IsSet(hashtable, key)
  local t = type(hashtable)
  return (t == "table" or t == "userdata") and hashtable[key] ~= nil
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