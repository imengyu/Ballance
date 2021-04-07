---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ArgumentCache
---@field public unityObjectArgument Object 
---@field public unityObjectArgumentAssemblyTypeName string 
---@field public intArgument number 
---@field public floatArgument number 
---@field public stringArgument string 
---@field public boolArgument boolean 
local ArgumentCache={ }
---
---@public
---@return void 
function ArgumentCache:OnBeforeSerialize() end
---
---@public
---@return void 
function ArgumentCache:OnAfterDeserialize() end
---
UnityEngine.Events.ArgumentCache = ArgumentCache