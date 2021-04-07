---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CrashReport
---@field public time DateTime 
---@field public text string 
---@field public reports CrashReport[] 
---@field public lastReport CrashReport 
local CrashReport={ }
---
---@public
---@return void 
function CrashReport.RemoveAll() end
---
---@public
---@return void 
function CrashReport:Remove() end
---
UnityEngine.CrashReport = CrashReport