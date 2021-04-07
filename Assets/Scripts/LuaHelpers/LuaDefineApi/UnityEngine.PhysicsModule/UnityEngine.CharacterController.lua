---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CharacterController : Collider
---@field public velocity Vector3 
---@field public isGrounded boolean 
---@field public collisionFlags number 
---@field public radius number 
---@field public height number 
---@field public center Vector3 
---@field public slopeLimit number 
---@field public stepOffset number 
---@field public skinWidth number 
---@field public minMoveDistance number 
---@field public detectCollisions boolean 
---@field public enableOverlapRecovery boolean 
local CharacterController={ }
---
---@public
---@param speed Vector3 
---@return boolean 
function CharacterController:SimpleMove(speed) end
---
---@public
---@param motion Vector3 
---@return number 
function CharacterController:Move(motion) end
---
UnityEngine.CharacterController = CharacterController