---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Sampler
---@field public isValid boolean 
---@field public name string 
local Sampler={ }
---
---@public
---@return Recorder 
function Sampler:GetRecorder() end
---
---@public
---@param name string 
---@return Sampler 
function Sampler.Get(name) end
---
---@public
---@param names List`1 
---@return number 
function Sampler.GetNames(names) end
---
UnityEngine.Profiling.Sampler = Sampler