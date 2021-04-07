﻿---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RenderSettings : Object
---@field public ambientSkyboxAmount number 
---@field public fog boolean 
---@field public fogStartDistance number 
---@field public fogEndDistance number 
---@field public fogMode number 
---@field public fogColor Color 
---@field public fogDensity number 
---@field public ambientMode number 
---@field public ambientSkyColor Color 
---@field public ambientEquatorColor Color 
---@field public ambientGroundColor Color 
---@field public ambientIntensity number 
---@field public ambientLight Color 
---@field public subtractiveShadowColor Color 
---@field public skybox Material 
---@field public sun Light 
---@field public ambientProbe SphericalHarmonicsL2 
---@field public customReflection Cubemap 
---@field public reflectionIntensity number 
---@field public reflectionBounces number 
---@field public defaultReflectionMode number 
---@field public defaultReflectionResolution number 
---@field public haloStrength number 
---@field public flareStrength number 
---@field public flareFadeSpeed number 
local RenderSettings={ }
---
UnityEngine.RenderSettings = RenderSettings