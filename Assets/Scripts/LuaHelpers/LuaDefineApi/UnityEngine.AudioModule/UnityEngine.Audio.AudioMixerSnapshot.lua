---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AudioMixerSnapshot : Object
---@field public audioMixer AudioMixer 
local AudioMixerSnapshot={ }
---
---@public
---@param timeToReach number 
---@return void 
function AudioMixerSnapshot:TransitionTo(timeToReach) end
---
UnityEngine.Audio.AudioMixerSnapshot = AudioMixerSnapshot