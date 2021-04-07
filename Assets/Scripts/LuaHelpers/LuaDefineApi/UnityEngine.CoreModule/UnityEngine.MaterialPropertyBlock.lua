---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class MaterialPropertyBlock
---@field public isEmpty boolean 
local MaterialPropertyBlock={ }
---
---@public
---@param name string 
---@param value number 
---@return void 
function MaterialPropertyBlock:AddFloat(name, value) end
---
---@public
---@param nameID number 
---@param value number 
---@return void 
function MaterialPropertyBlock:AddFloat(nameID, value) end
---
---@public
---@param name string 
---@param value Vector4 
---@return void 
function MaterialPropertyBlock:AddVector(name, value) end
---
---@public
---@param nameID number 
---@param value Vector4 
---@return void 
function MaterialPropertyBlock:AddVector(nameID, value) end
---
---@public
---@param name string 
---@param value Color 
---@return void 
function MaterialPropertyBlock:AddColor(name, value) end
---
---@public
---@param nameID number 
---@param value Color 
---@return void 
function MaterialPropertyBlock:AddColor(nameID, value) end
---
---@public
---@param name string 
---@param value Matrix4x4 
---@return void 
function MaterialPropertyBlock:AddMatrix(name, value) end
---
---@public
---@param nameID number 
---@param value Matrix4x4 
---@return void 
function MaterialPropertyBlock:AddMatrix(nameID, value) end
---
---@public
---@param name string 
---@param value Texture 
---@return void 
function MaterialPropertyBlock:AddTexture(name, value) end
---
---@public
---@param nameID number 
---@param value Texture 
---@return void 
function MaterialPropertyBlock:AddTexture(nameID, value) end
---
---@public
---@return void 
function MaterialPropertyBlock:Clear() end
---
---@public
---@param name string 
---@param value number 
---@return void 
function MaterialPropertyBlock:SetFloat(name, value) end
---
---@public
---@param nameID number 
---@param value number 
---@return void 
function MaterialPropertyBlock:SetFloat(nameID, value) end
---
---@public
---@param name string 
---@param value number 
---@return void 
function MaterialPropertyBlock:SetInt(name, value) end
---
---@public
---@param nameID number 
---@param value number 
---@return void 
function MaterialPropertyBlock:SetInt(nameID, value) end
---
---@public
---@param name string 
---@param value Vector4 
---@return void 
function MaterialPropertyBlock:SetVector(name, value) end
---
---@public
---@param nameID number 
---@param value Vector4 
---@return void 
function MaterialPropertyBlock:SetVector(nameID, value) end
---
---@public
---@param name string 
---@param value Color 
---@return void 
function MaterialPropertyBlock:SetColor(name, value) end
---
---@public
---@param nameID number 
---@param value Color 
---@return void 
function MaterialPropertyBlock:SetColor(nameID, value) end
---
---@public
---@param name string 
---@param value Matrix4x4 
---@return void 
function MaterialPropertyBlock:SetMatrix(name, value) end
---
---@public
---@param nameID number 
---@param value Matrix4x4 
---@return void 
function MaterialPropertyBlock:SetMatrix(nameID, value) end
---
---@public
---@param name string 
---@param value ComputeBuffer 
---@return void 
function MaterialPropertyBlock:SetBuffer(name, value) end
---
---@public
---@param nameID number 
---@param value ComputeBuffer 
---@return void 
function MaterialPropertyBlock:SetBuffer(nameID, value) end
---
---@public
---@param name string 
---@param value GraphicsBuffer 
---@return void 
function MaterialPropertyBlock:SetBuffer(name, value) end
---
---@public
---@param nameID number 
---@param value GraphicsBuffer 
---@return void 
function MaterialPropertyBlock:SetBuffer(nameID, value) end
---
---@public
---@param name string 
---@param value Texture 
---@return void 
function MaterialPropertyBlock:SetTexture(name, value) end
---
---@public
---@param nameID number 
---@param value Texture 
---@return void 
function MaterialPropertyBlock:SetTexture(nameID, value) end
---
---@public
---@param name string 
---@param value RenderTexture 
---@param element number 
---@return void 
function MaterialPropertyBlock:SetTexture(name, value, element) end
---
---@public
---@param nameID number 
---@param value RenderTexture 
---@param element number 
---@return void 
function MaterialPropertyBlock:SetTexture(nameID, value, element) end
---
---@public
---@param name string 
---@param value ComputeBuffer 
---@param offset number 
---@param size number 
---@return void 
function MaterialPropertyBlock:SetConstantBuffer(name, value, offset, size) end
---
---@public
---@param nameID number 
---@param value ComputeBuffer 
---@param offset number 
---@param size number 
---@return void 
function MaterialPropertyBlock:SetConstantBuffer(nameID, value, offset, size) end
---
---@public
---@param name string 
---@param value GraphicsBuffer 
---@param offset number 
---@param size number 
---@return void 
function MaterialPropertyBlock:SetConstantBuffer(name, value, offset, size) end
---
---@public
---@param nameID number 
---@param value GraphicsBuffer 
---@param offset number 
---@param size number 
---@return void 
function MaterialPropertyBlock:SetConstantBuffer(nameID, value, offset, size) end
---
---@public
---@param name string 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:SetFloatArray(name, values) end
---
---@public
---@param nameID number 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:SetFloatArray(nameID, values) end
---
---@public
---@param name string 
---@param values Single[] 
---@return void 
function MaterialPropertyBlock:SetFloatArray(name, values) end
---
---@public
---@param nameID number 
---@param values Single[] 
---@return void 
function MaterialPropertyBlock:SetFloatArray(nameID, values) end
---
---@public
---@param name string 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:SetVectorArray(name, values) end
---
---@public
---@param nameID number 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:SetVectorArray(nameID, values) end
---
---@public
---@param name string 
---@param values Vector4[] 
---@return void 
function MaterialPropertyBlock:SetVectorArray(name, values) end
---
---@public
---@param nameID number 
---@param values Vector4[] 
---@return void 
function MaterialPropertyBlock:SetVectorArray(nameID, values) end
---
---@public
---@param name string 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:SetMatrixArray(name, values) end
---
---@public
---@param nameID number 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:SetMatrixArray(nameID, values) end
---
---@public
---@param name string 
---@param values Matrix4x4[] 
---@return void 
function MaterialPropertyBlock:SetMatrixArray(name, values) end
---
---@public
---@param nameID number 
---@param values Matrix4x4[] 
---@return void 
function MaterialPropertyBlock:SetMatrixArray(nameID, values) end
---
---@public
---@param name string 
---@return number 
function MaterialPropertyBlock:GetFloat(name) end
---
---@public
---@param nameID number 
---@return number 
function MaterialPropertyBlock:GetFloat(nameID) end
---
---@public
---@param name string 
---@return number 
function MaterialPropertyBlock:GetInt(name) end
---
---@public
---@param nameID number 
---@return number 
function MaterialPropertyBlock:GetInt(nameID) end
---
---@public
---@param name string 
---@return Vector4 
function MaterialPropertyBlock:GetVector(name) end
---
---@public
---@param nameID number 
---@return Vector4 
function MaterialPropertyBlock:GetVector(nameID) end
---
---@public
---@param name string 
---@return Color 
function MaterialPropertyBlock:GetColor(name) end
---
---@public
---@param nameID number 
---@return Color 
function MaterialPropertyBlock:GetColor(nameID) end
---
---@public
---@param name string 
---@return Matrix4x4 
function MaterialPropertyBlock:GetMatrix(name) end
---
---@public
---@param nameID number 
---@return Matrix4x4 
function MaterialPropertyBlock:GetMatrix(nameID) end
---
---@public
---@param name string 
---@return Texture 
function MaterialPropertyBlock:GetTexture(name) end
---
---@public
---@param nameID number 
---@return Texture 
function MaterialPropertyBlock:GetTexture(nameID) end
---
---@public
---@param name string 
---@return Single[] 
function MaterialPropertyBlock:GetFloatArray(name) end
---
---@public
---@param nameID number 
---@return Single[] 
function MaterialPropertyBlock:GetFloatArray(nameID) end
---
---@public
---@param name string 
---@return Vector4[] 
function MaterialPropertyBlock:GetVectorArray(name) end
---
---@public
---@param nameID number 
---@return Vector4[] 
function MaterialPropertyBlock:GetVectorArray(nameID) end
---
---@public
---@param name string 
---@return Matrix4x4[] 
function MaterialPropertyBlock:GetMatrixArray(name) end
---
---@public
---@param nameID number 
---@return Matrix4x4[] 
function MaterialPropertyBlock:GetMatrixArray(nameID) end
---
---@public
---@param name string 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:GetFloatArray(name, values) end
---
---@public
---@param nameID number 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:GetFloatArray(nameID, values) end
---
---@public
---@param name string 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:GetVectorArray(name, values) end
---
---@public
---@param nameID number 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:GetVectorArray(nameID, values) end
---
---@public
---@param name string 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:GetMatrixArray(name, values) end
---
---@public
---@param nameID number 
---@param values List`1 
---@return void 
function MaterialPropertyBlock:GetMatrixArray(nameID, values) end
---
---@public
---@param lightProbes List`1 
---@return void 
function MaterialPropertyBlock:CopySHCoefficientArraysFrom(lightProbes) end
---
---@public
---@param lightProbes SphericalHarmonicsL2[] 
---@return void 
function MaterialPropertyBlock:CopySHCoefficientArraysFrom(lightProbes) end
---
---@public
---@param lightProbes List`1 
---@param sourceStart number 
---@param destStart number 
---@param count number 
---@return void 
function MaterialPropertyBlock:CopySHCoefficientArraysFrom(lightProbes, sourceStart, destStart, count) end
---
---@public
---@param lightProbes SphericalHarmonicsL2[] 
---@param sourceStart number 
---@param destStart number 
---@param count number 
---@return void 
function MaterialPropertyBlock:CopySHCoefficientArraysFrom(lightProbes, sourceStart, destStart, count) end
---
---@public
---@param occlusionProbes List`1 
---@return void 
function MaterialPropertyBlock:CopyProbeOcclusionArrayFrom(occlusionProbes) end
---
---@public
---@param occlusionProbes Vector4[] 
---@return void 
function MaterialPropertyBlock:CopyProbeOcclusionArrayFrom(occlusionProbes) end
---
---@public
---@param occlusionProbes List`1 
---@param sourceStart number 
---@param destStart number 
---@param count number 
---@return void 
function MaterialPropertyBlock:CopyProbeOcclusionArrayFrom(occlusionProbes, sourceStart, destStart, count) end
---
---@public
---@param occlusionProbes Vector4[] 
---@param sourceStart number 
---@param destStart number 
---@param count number 
---@return void 
function MaterialPropertyBlock:CopyProbeOcclusionArrayFrom(occlusionProbes, sourceStart, destStart, count) end
---
UnityEngine.MaterialPropertyBlock = MaterialPropertyBlock