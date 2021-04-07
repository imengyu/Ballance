---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationStreamHandleUtility
local AnimationStreamHandleUtility={ }
---
---@public
---@param stream AnimationStream 
---@param handles NativeArray`1 
---@param buffer NativeArray`1 
---@param useMask boolean 
---@return void 
function AnimationStreamHandleUtility.WriteInts(stream, handles, buffer, useMask) end
---
---@public
---@param stream AnimationStream 
---@param handles NativeArray`1 
---@param buffer NativeArray`1 
---@param useMask boolean 
---@return void 
function AnimationStreamHandleUtility.WriteFloats(stream, handles, buffer, useMask) end
---
---@public
---@param stream AnimationStream 
---@param handles NativeArray`1 
---@param buffer NativeArray`1 
---@return void 
function AnimationStreamHandleUtility.ReadInts(stream, handles, buffer) end
---
---@public
---@param stream AnimationStream 
---@param handles NativeArray`1 
---@param buffer NativeArray`1 
---@return void 
function AnimationStreamHandleUtility.ReadFloats(stream, handles, buffer) end
---
UnityEngine.Animations.AnimationStreamHandleUtility = AnimationStreamHandleUtility