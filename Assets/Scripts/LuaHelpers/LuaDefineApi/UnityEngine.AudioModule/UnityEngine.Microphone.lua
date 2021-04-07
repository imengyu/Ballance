---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Microphone
---@field public devices String[] 
local Microphone={ }
---
---@public
---@param deviceName string 
---@param loop boolean 
---@param lengthSec number 
---@param frequency number 
---@return AudioClip 
function Microphone.Start(deviceName, loop, lengthSec, frequency) end
---
---@public
---@param deviceName string 
---@return void 
function Microphone.End(deviceName) end
---
---@public
---@param deviceName string 
---@return boolean 
function Microphone.IsRecording(deviceName) end
---
---@public
---@param deviceName string 
---@return number 
function Microphone.GetPosition(deviceName) end
---
---@public
---@param deviceName string 
---@param minFreq Int32& 
---@param maxFreq Int32& 
---@return void 
function Microphone.GetDeviceCaps(deviceName, minFreq, maxFreq) end
---
UnityEngine.Microphone = Microphone