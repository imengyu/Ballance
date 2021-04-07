---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Joint2D : Behaviour
---@field public attachedRigidbody Rigidbody2D 
---@field public connectedBody Rigidbody2D 
---@field public enableCollision boolean 
---@field public breakForce number 
---@field public breakTorque number 
---@field public reactionForce Vector2 
---@field public reactionTorque number 
---@field public collideConnected boolean 
local Joint2D={ }
---
---@public
---@param timeStep number 
---@return Vector2 
function Joint2D:GetReactionForce(timeStep) end
---
---@public
---@param timeStep number 
---@return number 
function Joint2D:GetReactionTorque(timeStep) end
---
UnityEngine.Joint2D = Joint2D