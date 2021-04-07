---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class HDROutputSettings
---@field public displays HDROutputSettings[] 
---@field public main HDROutputSettings 
---@field public active boolean 
---@field public available boolean 
---@field public automaticHDRTonemapping boolean 
---@field public displayColorGamut number 
---@field public format number 
---@field public graphicsFormat number 
---@field public paperWhiteNits number 
---@field public maxFullFrameToneMapLuminance number 
---@field public maxToneMapLuminance number 
---@field public minToneMapLuminance number 
---@field public HDRModeChangeRequested boolean 
local HDROutputSettings={ }
---
---@public
---@param enabled boolean 
---@return void 
function HDROutputSettings:RequestHDRModeChange(enabled) end
---
---@public
---@param paperWhite number 
---@return void 
function HDROutputSettings.SetPaperWhiteInNits(paperWhite) end
---
UnityEngine.HDROutputSettings = HDROutputSettings