---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AudioRenderer
local AudioRenderer={ }
---
---@public
---@return boolean 
function AudioRenderer.Start() end
---
---@public
---@return boolean 
function AudioRenderer.Stop() end
---
---@public
---@return number 
function AudioRenderer.GetSampleCountForCaptureFrame() end
---
---@public
---@param buffer NativeArray`1 
---@return boolean 
function AudioRenderer.Render(buffer) end
---
UnityEngine.AudioRenderer = AudioRenderer