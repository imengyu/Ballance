---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AudioMixerPlayable : ValueType
local AudioMixerPlayable={ }
---
---@public
---@param graph PlayableGraph 
---@param inputCount number 
---@param normalizeInputVolumes boolean 
---@return AudioMixerPlayable 
function AudioMixerPlayable.Create(graph, inputCount, normalizeInputVolumes) end
---
---@public
---@return PlayableHandle 
function AudioMixerPlayable:GetHandle() end
---
---@public
---@param playable AudioMixerPlayable 
---@return Playable 
function AudioMixerPlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AudioMixerPlayable 
function AudioMixerPlayable.op_Explicit(playable) end
---
---@public
---@param other AudioMixerPlayable 
---@return boolean 
function AudioMixerPlayable:Equals(other) end
---
UnityEngine.Audio.AudioMixerPlayable = AudioMixerPlayable