---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Collision2D
---@field public collider Collider2D 
---@field public otherCollider Collider2D 
---@field public rigidbody Rigidbody2D 
---@field public otherRigidbody Rigidbody2D 
---@field public transform Transform 
---@field public gameObject GameObject 
---@field public relativeVelocity Vector2 
---@field public enabled boolean 
---@field public contacts ContactPoint2D[] 
---@field public contactCount number 
local Collision2D={ }
---
---@public
---@param index number 
---@return ContactPoint2D 
function Collision2D:GetContact(index) end
---
---@public
---@param contacts ContactPoint2D[] 
---@return number 
function Collision2D:GetContacts(contacts) end
---
---@public
---@param contacts List`1 
---@return number 
function Collision2D:GetContacts(contacts) end
---
UnityEngine.Collision2D = Collision2D