---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ProfilerCategory : ValueType
---@field public Name string 
---@field public Color Color32 
---@field public Render ProfilerCategory 
---@field public Scripts ProfilerCategory 
---@field public Gui ProfilerCategory 
---@field public Physics ProfilerCategory 
---@field public Animation ProfilerCategory 
---@field public Ai ProfilerCategory 
---@field public Audio ProfilerCategory 
---@field public Video ProfilerCategory 
---@field public Particles ProfilerCategory 
---@field public Lighting ProfilerCategory 
---@field public Network ProfilerCategory 
---@field public Loading ProfilerCategory 
---@field public Vr ProfilerCategory 
---@field public Input ProfilerCategory 
---@field public Memory ProfilerCategory 
---@field public VirtualTexturing ProfilerCategory 
---@field public Internal ProfilerCategory 
local ProfilerCategory={ }
---
---@public
---@return string 
function ProfilerCategory:ToString() end
---
---@public
---@param category ProfilerCategory 
---@return number 
function ProfilerCategory.op_Implicit(category) end
---
Unity.Profiling.ProfilerCategory = ProfilerCategory