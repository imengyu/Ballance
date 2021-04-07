---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BillboardAsset : Object
---@field public width number 
---@field public height number 
---@field public bottom number 
---@field public imageCount number 
---@field public vertexCount number 
---@field public indexCount number 
---@field public material Material 
local BillboardAsset={ }
---
---@public
---@param imageTexCoords List`1 
---@return void 
function BillboardAsset:GetImageTexCoords(imageTexCoords) end
---
---@public
---@return Vector4[] 
function BillboardAsset:GetImageTexCoords() end
---
---@public
---@param imageTexCoords List`1 
---@return void 
function BillboardAsset:SetImageTexCoords(imageTexCoords) end
---
---@public
---@param imageTexCoords Vector4[] 
---@return void 
function BillboardAsset:SetImageTexCoords(imageTexCoords) end
---
---@public
---@param vertices List`1 
---@return void 
function BillboardAsset:GetVertices(vertices) end
---
---@public
---@return Vector2[] 
function BillboardAsset:GetVertices() end
---
---@public
---@param vertices List`1 
---@return void 
function BillboardAsset:SetVertices(vertices) end
---
---@public
---@param vertices Vector2[] 
---@return void 
function BillboardAsset:SetVertices(vertices) end
---
---@public
---@param indices List`1 
---@return void 
function BillboardAsset:GetIndices(indices) end
---
---@public
---@return UInt16[] 
function BillboardAsset:GetIndices() end
---
---@public
---@param indices List`1 
---@return void 
function BillboardAsset:SetIndices(indices) end
---
---@public
---@param indices UInt16[] 
---@return void 
function BillboardAsset:SetIndices(indices) end
---
UnityEngine.BillboardAsset = BillboardAsset