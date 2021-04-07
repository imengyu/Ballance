---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CustomYieldInstruction
---@field public keepWaiting boolean 
---@field public Current Object 
local CustomYieldInstruction={ }
---
---@public
---@return boolean 
function CustomYieldInstruction:MoveNext() end
---
---@public
---@return void 
function CustomYieldInstruction:Reset() end
---
UnityEngine.CustomYieldInstruction = CustomYieldInstruction