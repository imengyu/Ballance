---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ApplicationShimBase
---@field public isEditor boolean 
---@field public platform number 
---@field public isMobilePlatform boolean 
---@field public isConsolePlatform boolean 
---@field public systemLanguage number 
---@field public internetReachability number 
local ApplicationShimBase={ }
---
---@public
---@return void 
function ApplicationShimBase:Dispose() end
---
---@public
---@return boolean 
function ApplicationShimBase:IsActive() end
---
---@public
---@return void 
function ApplicationShimBase:OnLowMemory() end
---
UnityEngine.ApplicationShimBase = ApplicationShimBase