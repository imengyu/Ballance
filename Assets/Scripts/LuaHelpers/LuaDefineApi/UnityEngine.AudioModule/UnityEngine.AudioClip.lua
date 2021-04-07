---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AudioClip : Object
---@field public length number 
---@field public samples number 
---@field public channels number 
---@field public frequency number 
---@field public isReadyToPlay boolean 
---@field public loadType number 
---@field public preloadAudioData boolean 
---@field public ambisonic boolean 
---@field public loadInBackground boolean 
---@field public loadState number 
local AudioClip={ }
---
---@public
---@return boolean 
function AudioClip:LoadAudioData() end
---
---@public
---@return boolean 
function AudioClip:UnloadAudioData() end
---
---@public
---@param data Single[] 
---@param offsetSamples number 
---@return boolean 
function AudioClip:GetData(data, offsetSamples) end
---
---@public
---@param data Single[] 
---@param offsetSamples number 
---@return boolean 
function AudioClip:SetData(data, offsetSamples) end
---
---@public
---@param name string 
---@param lengthSamples number 
---@param channels number 
---@param frequency number 
---@param _3D boolean 
---@param stream boolean 
---@return AudioClip 
function AudioClip.Create(name, lengthSamples, channels, frequency, _3D, stream) end
---
---@public
---@param name string 
---@param lengthSamples number 
---@param channels number 
---@param frequency number 
---@param _3D boolean 
---@param stream boolean 
---@param pcmreadercallback PCMReaderCallback 
---@return AudioClip 
function AudioClip.Create(name, lengthSamples, channels, frequency, _3D, stream, pcmreadercallback) end
---
---@public
---@param name string 
---@param lengthSamples number 
---@param channels number 
---@param frequency number 
---@param _3D boolean 
---@param stream boolean 
---@param pcmreadercallback PCMReaderCallback 
---@param pcmsetpositioncallback PCMSetPositionCallback 
---@return AudioClip 
function AudioClip.Create(name, lengthSamples, channels, frequency, _3D, stream, pcmreadercallback, pcmsetpositioncallback) end
---
---@public
---@param name string 
---@param lengthSamples number 
---@param channels number 
---@param frequency number 
---@param stream boolean 
---@return AudioClip 
function AudioClip.Create(name, lengthSamples, channels, frequency, stream) end
---
---@public
---@param name string 
---@param lengthSamples number 
---@param channels number 
---@param frequency number 
---@param stream boolean 
---@param pcmreadercallback PCMReaderCallback 
---@return AudioClip 
function AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback) end
---
---@public
---@param name string 
---@param lengthSamples number 
---@param channels number 
---@param frequency number 
---@param stream boolean 
---@param pcmreadercallback PCMReaderCallback 
---@param pcmsetpositioncallback PCMSetPositionCallback 
---@return AudioClip 
function AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, pcmsetpositioncallback) end
---
UnityEngine.AudioClip = AudioClip