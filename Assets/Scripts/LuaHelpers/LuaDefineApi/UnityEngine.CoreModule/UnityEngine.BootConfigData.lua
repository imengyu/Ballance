---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BootConfigData
local BootConfigData={ }
---
---@public
---@param key string 
---@return void 
function BootConfigData:AddKey(key) end
---
---@public
---@param key string 
---@return string 
function BootConfigData:Get(key) end
---
---@public
---@param key string 
---@param index number 
---@return string 
function BootConfigData:Get(key, index) end
---
---@public
---@param key string 
---@param value string 
---@return void 
function BootConfigData:Append(key, value) end
---
---@public
---@param key string 
---@param value string 
---@return void 
function BootConfigData:Set(key, value) end
---
UnityEngine.BootConfigData = BootConfigData