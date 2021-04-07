---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AudioSource : AudioBehaviour
---@field public panLevel number 
---@field public pan number 
---@field public volume number 
---@field public pitch number 
---@field public time number 
---@field public timeSamples number 
---@field public clip AudioClip 
---@field public outputAudioMixerGroup AudioMixerGroup 
---@field public isPlaying boolean 
---@field public isVirtual boolean 
---@field public loop boolean 
---@field public ignoreListenerVolume boolean 
---@field public playOnAwake boolean 
---@field public ignoreListenerPause boolean 
---@field public velocityUpdateMode number 
---@field public panStereo number 
---@field public spatialBlend number 
---@field public spatialize boolean 
---@field public spatializePostEffects boolean 
---@field public reverbZoneMix number 
---@field public bypassEffects boolean 
---@field public bypassListenerEffects boolean 
---@field public bypassReverbZones boolean 
---@field public dopplerLevel number 
---@field public spread number 
---@field public priority number 
---@field public mute boolean 
---@field public minDistance number 
---@field public maxDistance number 
---@field public rolloffMode number 
---@field public minVolume number 
---@field public maxVolume number 
---@field public rolloffFactor number 
local AudioSource={ }
---
---@public
---@return void 
function AudioSource:Play() end
---
---@public
---@param delay number 
---@return void 
function AudioSource:Play(delay) end
---
---@public
---@param delay number 
---@return void 
function AudioSource:PlayDelayed(delay) end
---
---@public
---@param time number 
---@return void 
function AudioSource:PlayScheduled(time) end
---
---@public
---@param clip AudioClip 
---@return void 
function AudioSource:PlayOneShot(clip) end
---
---@public
---@param clip AudioClip 
---@param volumeScale number 
---@return void 
function AudioSource:PlayOneShot(clip, volumeScale) end
---
---@public
---@param time number 
---@return void 
function AudioSource:SetScheduledStartTime(time) end
---
---@public
---@param time number 
---@return void 
function AudioSource:SetScheduledEndTime(time) end
---
---@public
---@return void 
function AudioSource:Stop() end
---
---@public
---@return void 
function AudioSource:Pause() end
---
---@public
---@return void 
function AudioSource:UnPause() end
---
---@public
---@param clip AudioClip 
---@param position Vector3 
---@return void 
function AudioSource.PlayClipAtPoint(clip, position) end
---
---@public
---@param clip AudioClip 
---@param position Vector3 
---@param volume number 
---@return void 
function AudioSource.PlayClipAtPoint(clip, position, volume) end
---
---@public
---@param type number 
---@param curve AnimationCurve 
---@return void 
function AudioSource:SetCustomCurve(type, curve) end
---
---@public
---@param type number 
---@return AnimationCurve 
function AudioSource:GetCustomCurve(type) end
---
---@public
---@param numSamples number 
---@param channel number 
---@return Single[] 
function AudioSource:GetOutputData(numSamples, channel) end
---
---@public
---@param samples Single[] 
---@param channel number 
---@return void 
function AudioSource:GetOutputData(samples, channel) end
---
---@public
---@param numSamples number 
---@param channel number 
---@param window number 
---@return Single[] 
function AudioSource:GetSpectrumData(numSamples, channel, window) end
---
---@public
---@param samples Single[] 
---@param channel number 
---@param window number 
---@return void 
function AudioSource:GetSpectrumData(samples, channel, window) end
---
---@public
---@param index number 
---@param value number 
---@return boolean 
function AudioSource:SetSpatializerFloat(index, value) end
---
---@public
---@param index number 
---@param value Single& 
---@return boolean 
function AudioSource:GetSpatializerFloat(index, value) end
---
---@public
---@param index number 
---@param value Single& 
---@return boolean 
function AudioSource:GetAmbisonicDecoderFloat(index, value) end
---
---@public
---@param index number 
---@param value number 
---@return boolean 
function AudioSource:SetAmbisonicDecoderFloat(index, value) end
---
UnityEngine.AudioSource = AudioSource