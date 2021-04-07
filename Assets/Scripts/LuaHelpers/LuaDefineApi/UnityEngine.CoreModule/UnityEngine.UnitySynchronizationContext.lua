---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UnitySynchronizationContext : SynchronizationContext
local UnitySynchronizationContext={ }
---
---@public
---@param callback SendOrPostCallback 
---@param state Object 
---@return void 
function UnitySynchronizationContext:Send(callback, state) end
---
---@public
---@return void 
function UnitySynchronizationContext:OperationStarted() end
---
---@public
---@return void 
function UnitySynchronizationContext:OperationCompleted() end
---
---@public
---@param callback SendOrPostCallback 
---@param state Object 
---@return void 
function UnitySynchronizationContext:Post(callback, state) end
---
---@public
---@return SynchronizationContext 
function UnitySynchronizationContext:CreateCopy() end
---
UnityEngine.UnitySynchronizationContext = UnitySynchronizationContext