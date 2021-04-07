---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class TransformSceneHandle : ValueType
local TransformSceneHandle={ }
---
---@public
---@param stream AnimationStream 
---@return boolean 
function TransformSceneHandle:IsValid(stream) end
---
---@public
---@param stream AnimationStream 
---@return Vector3 
function TransformSceneHandle:GetPosition(stream) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3 
---@return void 
function TransformSceneHandle:SetPosition(stream, position) end
---
---@public
---@param stream AnimationStream 
---@return Vector3 
function TransformSceneHandle:GetLocalPosition(stream) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3 
---@return void 
function TransformSceneHandle:SetLocalPosition(stream, position) end
---
---@public
---@param stream AnimationStream 
---@return Quaternion 
function TransformSceneHandle:GetRotation(stream) end
---
---@public
---@param stream AnimationStream 
---@param rotation Quaternion 
---@return void 
function TransformSceneHandle:SetRotation(stream, rotation) end
---
---@public
---@param stream AnimationStream 
---@return Quaternion 
function TransformSceneHandle:GetLocalRotation(stream) end
---
---@public
---@param stream AnimationStream 
---@param rotation Quaternion 
---@return void 
function TransformSceneHandle:SetLocalRotation(stream, rotation) end
---
---@public
---@param stream AnimationStream 
---@return Vector3 
function TransformSceneHandle:GetLocalScale(stream) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3& 
---@param rotation Quaternion& 
---@param scale Vector3& 
---@return void 
function TransformSceneHandle:GetLocalTRS(stream, position, rotation, scale) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3& 
---@param rotation Quaternion& 
---@return void 
function TransformSceneHandle:GetGlobalTR(stream, position, rotation) end
---
---@public
---@param stream AnimationStream 
---@param scale Vector3 
---@return void 
function TransformSceneHandle:SetLocalScale(stream, scale) end
---
UnityEngine.Animations.TransformSceneHandle = TransformSceneHandle