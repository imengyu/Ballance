---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Random
---@field public state State 
---@field public value number 
---@field public insideUnitSphere Vector3 
---@field public insideUnitCircle Vector2 
---@field public onUnitSphere Vector3 
---@field public rotation Quaternion 
---@field public rotationUniform Quaternion 
---@field public seed number 
local Random={ }
---
---@public
---@param seed number 
---@return void 
function Random.InitState(seed) end
---
---@public
---@param minInclusive number 
---@param maxInclusive number 
---@return number 
function Random.Range(minInclusive, maxInclusive) end
---
---@public
---@param minInclusive number 
---@param maxExclusive number 
---@return number 
function Random.Range(minInclusive, maxExclusive) end
---
---@public
---@param min number 
---@param max number 
---@return number 
function Random.RandomRange(min, max) end
---
---@public
---@param min number 
---@param max number 
---@return number 
function Random.RandomRange(min, max) end
---
---@public
---@return Color 
function Random.ColorHSV() end
---
---@public
---@param hueMin number 
---@param hueMax number 
---@return Color 
function Random.ColorHSV(hueMin, hueMax) end
---
---@public
---@param hueMin number 
---@param hueMax number 
---@param saturationMin number 
---@param saturationMax number 
---@return Color 
function Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax) end
---
---@public
---@param hueMin number 
---@param hueMax number 
---@param saturationMin number 
---@param saturationMax number 
---@param valueMin number 
---@param valueMax number 
---@return Color 
function Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax) end
---
---@public
---@param hueMin number 
---@param hueMax number 
---@param saturationMin number 
---@param saturationMax number 
---@param valueMin number 
---@param valueMax number 
---@param alphaMin number 
---@param alphaMax number 
---@return Color 
function Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax) end
---
UnityEngine.Random = Random