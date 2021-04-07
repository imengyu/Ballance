---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationSceneHandleUtility
local AnimationSceneHandleUtility={ }
---
---@public
---@param stream AnimationStream 
---@param handles NativeArray`1 
---@param buffer NativeArray`1 
---@return void 
function AnimationSceneHandleUtility.ReadInts(stream, handles, buffer) end
---
---@public
---@param stream AnimationStream 
---@param handles NativeArray`1 
---@param buffer NativeArray`1 
---@return void 
function AnimationSceneHandleUtility.ReadFloats(stream, handles, buffer) end
---
UnityEngine.Animations.AnimationSceneHandleUtility = AnimationSceneHandleUtility