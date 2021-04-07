---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SystemClock
---@field public now DateTime 
local SystemClock={ }
---
---@public
---@param date DateTime 
---@return number 
function SystemClock.ToUnixTimeMilliseconds(date) end
---
---@public
---@param date DateTime 
---@return number 
function SystemClock.ToUnixTimeSeconds(date) end
---
UnityEngine.SystemClock = SystemClock