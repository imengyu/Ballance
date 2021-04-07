---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AsyncOperation
---@field public isDone boolean 
---@field public progress number 
---@field public priority number 
---@field public allowSceneActivation boolean 
local AsyncOperation={ }
---
---@public
---@param value Action`1 
---@return void 
function AsyncOperation:add_completed(value) end
---
---@public
---@param value Action`1 
---@return void 
function AsyncOperation:remove_completed(value) end
---
UnityEngine.AsyncOperation = AsyncOperation