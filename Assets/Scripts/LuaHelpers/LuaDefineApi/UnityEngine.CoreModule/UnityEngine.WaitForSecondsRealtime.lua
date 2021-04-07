---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class WaitForSecondsRealtime : CustomYieldInstruction
---@field public waitTime number 
---@field public keepWaiting boolean 
local WaitForSecondsRealtime={ }
---
---@public
---@return void 
function WaitForSecondsRealtime:Reset() end
---
UnityEngine.WaitForSecondsRealtime = WaitForSecondsRealtime