---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Security
local Security={ }
---
---@public
---@param assemblyData Byte[] 
---@param authorizationKey string 
---@return Assembly 
function Security.LoadAndVerifyAssembly(assemblyData, authorizationKey) end
---
---@public
---@param assemblyData Byte[] 
---@return Assembly 
function Security.LoadAndVerifyAssembly(assemblyData) end
---
---@public
---@param ip string 
---@param atPort number 
---@return boolean 
function Security.PrefetchSocketPolicy(ip, atPort) end
---
---@public
---@param ip string 
---@param atPort number 
---@param timeout number 
---@return boolean 
function Security.PrefetchSocketPolicy(ip, atPort, timeout) end
---
UnityEngine.Security = Security