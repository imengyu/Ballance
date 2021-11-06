---分割字符串
---@param input string 要分割的原串
---@param delimiter string 按此字符分割
---@return table
function string.split(input, delimiter)
  input = tostring(input)
  delimiter = tostring(delimiter)
  if (delimiter=='') then return false end
  local pos,arr = 0, {}
  for st,sp in function() return string.find(input, delimiter, pos, true) end do
      table.insert(arr, string.sub(input, pos, st - 1))
      pos = sp + 1
  end
  table.insert(arr, string.sub(input, pos))
  return arr
end

---检测字符串是否包含其他字符串
---@param input string 要分割的原串
---@param needle string 
---@return boolean
function string.contains(input, needle)
  return string.find(input, needle) ~= nil
end

---检测字符串是否以某个字符串开头
---@param input string 要检测的原串
---@param needle string 
---@return boolean
function string.startWith(input, needle)
  local s, _ = string.find(input, needle)
  return s == 1
end

---检测字符串是否以某个字符串结尾
---@param input string 要检测的原串
---@param needle string 
---@return boolean
function string.endWith(input, needle)
  local _, e = string.find(input, needle)
  return e ~= nil and e == 1
end