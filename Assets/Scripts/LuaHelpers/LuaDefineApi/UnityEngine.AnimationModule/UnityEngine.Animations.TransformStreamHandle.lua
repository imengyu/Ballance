---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class TransformStreamHandle : ValueType
local TransformStreamHandle={ }
---
---@public
---@param stream AnimationStream 
---@return boolean 
function TransformStreamHandle:IsValid(stream) end
---
---@public
---@param stream AnimationStream 
---@return void 
function TransformStreamHandle:Resolve(stream) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function TransformStreamHandle:IsResolved(stream) end
---
---@public
---@param stream AnimationStream 
---@return Vector3 
function TransformStreamHandle:GetPosition(stream) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3 
---@return void 
function TransformStreamHandle:SetPosition(stream, position) end
---
---@public
---@param stream AnimationStream 
---@return Quaternion 
function TransformStreamHandle:GetRotation(stream) end
---
---@public
---@param stream AnimationStream 
---@param rotation Quaternion 
---@return void 
function TransformStreamHandle:SetRotation(stream, rotation) end
---
---@public
---@param stream AnimationStream 
---@return Vector3 
function TransformStreamHandle:GetLocalPosition(stream) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3 
---@return void 
function TransformStreamHandle:SetLocalPosition(stream, position) end
---
---@public
---@param stream AnimationStream 
---@return Quaternion 
function TransformStreamHandle:GetLocalRotation(stream) end
---
---@public
---@param stream AnimationStream 
---@param rotation Quaternion 
---@return void 
function TransformStreamHandle:SetLocalRotation(stream, rotation) end
---
---@public
---@param stream AnimationStream 
---@return Vector3 
function TransformStreamHandle:GetLocalScale(stream) end
---
---@public
---@param stream AnimationStream 
---@param scale Vector3 
---@return void 
function TransformStreamHandle:SetLocalScale(stream, scale) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function TransformStreamHandle:GetPositionReadMask(stream) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function TransformStreamHandle:GetRotationReadMask(stream) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function TransformStreamHandle:GetScaleReadMask(stream) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3& 
---@param rotation Quaternion& 
---@param scale Vector3& 
---@return void 
function TransformStreamHandle:GetLocalTRS(stream, position, rotation, scale) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3 
---@param rotation Quaternion 
---@param scale Vector3 
---@param useMask boolean 
---@return void 
function TransformStreamHandle:SetLocalTRS(stream, position, rotation, scale, useMask) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3& 
---@param rotation Quaternion& 
---@return void 
function TransformStreamHandle:GetGlobalTR(stream, position, rotation) end
---
---@public
---@param stream AnimationStream 
---@param position Vector3 
---@param rotation Quaternion 
---@param useMask boolean 
---@return void 
function TransformStreamHandle:SetGlobalTR(stream, position, rotation, useMask) end
---
UnityEngine.Animations.TransformStreamHandle = TransformStreamHandle