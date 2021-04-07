---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GraphicsBuffer
---@field public count number 
---@field public stride number 
local GraphicsBuffer={ }
---
---@public
---@return void 
function GraphicsBuffer:Dispose() end
---
---@public
---@return void 
function GraphicsBuffer:Release() end
---
---@public
---@return boolean 
function GraphicsBuffer:IsValid() end
---
---@public
---@param data Array 
---@return void 
function GraphicsBuffer:SetData(data) end
---
---@public
---@param data Array 
---@param managedBufferStartIndex number 
---@param graphicsBufferStartIndex number 
---@param count number 
---@return void 
function GraphicsBuffer:SetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count) end
---
---@public
---@param data Array 
---@return void 
function GraphicsBuffer:GetData(data) end
---
---@public
---@param data Array 
---@param managedBufferStartIndex number 
---@param computeBufferStartIndex number 
---@param count number 
---@return void 
function GraphicsBuffer:GetData(data, managedBufferStartIndex, computeBufferStartIndex, count) end
---
---@public
---@return IntPtr 
function GraphicsBuffer:GetNativeBufferPtr() end
---
---@public
---@param counterValue number 
---@return void 
function GraphicsBuffer:SetCounterValue(counterValue) end
---
---@public
---@param src ComputeBuffer 
---@param dst ComputeBuffer 
---@param dstOffsetBytes number 
---@return void 
function GraphicsBuffer.CopyCount(src, dst, dstOffsetBytes) end
---
---@public
---@param src GraphicsBuffer 
---@param dst ComputeBuffer 
---@param dstOffsetBytes number 
---@return void 
function GraphicsBuffer.CopyCount(src, dst, dstOffsetBytes) end
---
---@public
---@param src ComputeBuffer 
---@param dst GraphicsBuffer 
---@param dstOffsetBytes number 
---@return void 
function GraphicsBuffer.CopyCount(src, dst, dstOffsetBytes) end
---
---@public
---@param src GraphicsBuffer 
---@param dst GraphicsBuffer 
---@param dstOffsetBytes number 
---@return void 
function GraphicsBuffer.CopyCount(src, dst, dstOffsetBytes) end
---
UnityEngine.GraphicsBuffer = GraphicsBuffer