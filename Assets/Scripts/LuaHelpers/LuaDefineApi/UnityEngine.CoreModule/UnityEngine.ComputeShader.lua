---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ComputeShader : Object
---@field public shaderKeywords String[] 
local ComputeShader={ }
---
---@public
---@param name string 
---@return number 
function ComputeShader:FindKernel(name) end
---
---@public
---@param name string 
---@return boolean 
function ComputeShader:HasKernel(name) end
---
---@public
---@param nameID number 
---@param val number 
---@return void 
function ComputeShader:SetFloat(nameID, val) end
---
---@public
---@param nameID number 
---@param val number 
---@return void 
function ComputeShader:SetInt(nameID, val) end
---
---@public
---@param nameID number 
---@param val Vector4 
---@return void 
function ComputeShader:SetVector(nameID, val) end
---
---@public
---@param nameID number 
---@param val Matrix4x4 
---@return void 
function ComputeShader:SetMatrix(nameID, val) end
---
---@public
---@param nameID number 
---@param values Vector4[] 
---@return void 
function ComputeShader:SetVectorArray(nameID, values) end
---
---@public
---@param nameID number 
---@param values Matrix4x4[] 
---@return void 
function ComputeShader:SetMatrixArray(nameID, values) end
---
---@public
---@param kernelIndex number 
---@param nameID number 
---@param texture Texture 
---@param mipLevel number 
---@return void 
function ComputeShader:SetTexture(kernelIndex, nameID, texture, mipLevel) end
---
---@public
---@param kernelIndex number 
---@param nameID number 
---@param globalTextureNameID number 
---@return void 
function ComputeShader:SetTextureFromGlobal(kernelIndex, nameID, globalTextureNameID) end
---
---@public
---@param kernelIndex number 
---@param nameID number 
---@param buffer ComputeBuffer 
---@return void 
function ComputeShader:SetBuffer(kernelIndex, nameID, buffer) end
---
---@public
---@param kernelIndex number 
---@param nameID number 
---@param buffer GraphicsBuffer 
---@return void 
function ComputeShader:SetBuffer(kernelIndex, nameID, buffer) end
---
---@public
---@param kernelIndex number 
---@param x UInt32& 
---@param y UInt32& 
---@param z UInt32& 
---@return void 
function ComputeShader:GetKernelThreadGroupSizes(kernelIndex, x, y, z) end
---
---@public
---@param kernelIndex number 
---@param threadGroupsX number 
---@param threadGroupsY number 
---@param threadGroupsZ number 
---@return void 
function ComputeShader:Dispatch(kernelIndex, threadGroupsX, threadGroupsY, threadGroupsZ) end
---
---@public
---@param keyword string 
---@return void 
function ComputeShader:EnableKeyword(keyword) end
---
---@public
---@param keyword string 
---@return void 
function ComputeShader:DisableKeyword(keyword) end
---
---@public
---@param keyword string 
---@return boolean 
function ComputeShader:IsKeywordEnabled(keyword) end
---
---@public
---@param name string 
---@param val number 
---@return void 
function ComputeShader:SetFloat(name, val) end
---
---@public
---@param name string 
---@param val number 
---@return void 
function ComputeShader:SetInt(name, val) end
---
---@public
---@param name string 
---@param val Vector4 
---@return void 
function ComputeShader:SetVector(name, val) end
---
---@public
---@param name string 
---@param val Matrix4x4 
---@return void 
function ComputeShader:SetMatrix(name, val) end
---
---@public
---@param name string 
---@param values Vector4[] 
---@return void 
function ComputeShader:SetVectorArray(name, values) end
---
---@public
---@param name string 
---@param values Matrix4x4[] 
---@return void 
function ComputeShader:SetMatrixArray(name, values) end
---
---@public
---@param name string 
---@param values Single[] 
---@return void 
function ComputeShader:SetFloats(name, values) end
---
---@public
---@param nameID number 
---@param values Single[] 
---@return void 
function ComputeShader:SetFloats(nameID, values) end
---
---@public
---@param name string 
---@param values Int32[] 
---@return void 
function ComputeShader:SetInts(name, values) end
---
---@public
---@param nameID number 
---@param values Int32[] 
---@return void 
function ComputeShader:SetInts(nameID, values) end
---
---@public
---@param name string 
---@param val boolean 
---@return void 
function ComputeShader:SetBool(name, val) end
---
---@public
---@param nameID number 
---@param val boolean 
---@return void 
function ComputeShader:SetBool(nameID, val) end
---
---@public
---@param kernelIndex number 
---@param nameID number 
---@param texture Texture 
---@return void 
function ComputeShader:SetTexture(kernelIndex, nameID, texture) end
---
---@public
---@param kernelIndex number 
---@param name string 
---@param texture Texture 
---@return void 
function ComputeShader:SetTexture(kernelIndex, name, texture) end
---
---@public
---@param kernelIndex number 
---@param name string 
---@param texture Texture 
---@param mipLevel number 
---@return void 
function ComputeShader:SetTexture(kernelIndex, name, texture, mipLevel) end
---
---@public
---@param kernelIndex number 
---@param nameID number 
---@param texture RenderTexture 
---@param mipLevel number 
---@param element number 
---@return void 
function ComputeShader:SetTexture(kernelIndex, nameID, texture, mipLevel, element) end
---
---@public
---@param kernelIndex number 
---@param name string 
---@param texture RenderTexture 
---@param mipLevel number 
---@param element number 
---@return void 
function ComputeShader:SetTexture(kernelIndex, name, texture, mipLevel, element) end
---
---@public
---@param kernelIndex number 
---@param name string 
---@param globalTextureName string 
---@return void 
function ComputeShader:SetTextureFromGlobal(kernelIndex, name, globalTextureName) end
---
---@public
---@param kernelIndex number 
---@param name string 
---@param buffer ComputeBuffer 
---@return void 
function ComputeShader:SetBuffer(kernelIndex, name, buffer) end
---
---@public
---@param kernelIndex number 
---@param name string 
---@param buffer GraphicsBuffer 
---@return void 
function ComputeShader:SetBuffer(kernelIndex, name, buffer) end
---
---@public
---@param nameID number 
---@param buffer ComputeBuffer 
---@param offset number 
---@param size number 
---@return void 
function ComputeShader:SetConstantBuffer(nameID, buffer, offset, size) end
---
---@public
---@param name string 
---@param buffer ComputeBuffer 
---@param offset number 
---@param size number 
---@return void 
function ComputeShader:SetConstantBuffer(name, buffer, offset, size) end
---
---@public
---@param nameID number 
---@param buffer GraphicsBuffer 
---@param offset number 
---@param size number 
---@return void 
function ComputeShader:SetConstantBuffer(nameID, buffer, offset, size) end
---
---@public
---@param name string 
---@param buffer GraphicsBuffer 
---@param offset number 
---@param size number 
---@return void 
function ComputeShader:SetConstantBuffer(name, buffer, offset, size) end
---
---@public
---@param kernelIndex number 
---@param argsBuffer ComputeBuffer 
---@param argsOffset number 
---@return void 
function ComputeShader:DispatchIndirect(kernelIndex, argsBuffer, argsOffset) end
---
---@public
---@param kernelIndex number 
---@param argsBuffer ComputeBuffer 
---@return void 
function ComputeShader:DispatchIndirect(kernelIndex, argsBuffer) end
---
---@public
---@param kernelIndex number 
---@param argsBuffer GraphicsBuffer 
---@param argsOffset number 
---@return void 
function ComputeShader:DispatchIndirect(kernelIndex, argsBuffer, argsOffset) end
---
---@public
---@param kernelIndex number 
---@param argsBuffer GraphicsBuffer 
---@return void 
function ComputeShader:DispatchIndirect(kernelIndex, argsBuffer) end
---
UnityEngine.ComputeShader = ComputeShader