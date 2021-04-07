---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AudioMixer : Object
---@field public outputAudioMixerGroup AudioMixerGroup 
---@field public updateMode number 
local AudioMixer={ }
---
---@public
---@param name string 
---@return AudioMixerSnapshot 
function AudioMixer:FindSnapshot(name) end
---
---@public
---@param subPath string 
---@return AudioMixerGroup[] 
function AudioMixer:FindMatchingGroups(subPath) end
---
---@public
---@param snapshots AudioMixerSnapshot[] 
---@param weights Single[] 
---@param timeToReach number 
---@return void 
function AudioMixer:TransitionToSnapshots(snapshots, weights, timeToReach) end
---
---@public
---@param name string 
---@param value number 
---@return boolean 
function AudioMixer:SetFloat(name, value) end
---
---@public
---@param name string 
---@return boolean 
function AudioMixer:ClearFloat(name) end
---
---@public
---@param name string 
---@param value Single& 
---@return boolean 
function AudioMixer:GetFloat(name, value) end
---
UnityEngine.Audio.AudioMixer = AudioMixer