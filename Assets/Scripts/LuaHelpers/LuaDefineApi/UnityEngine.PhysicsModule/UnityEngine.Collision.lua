---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Collision
---@field public relativeVelocity Vector3 
---@field public rigidbody Rigidbody 
---@field public collider Collider 
---@field public transform Transform 
---@field public gameObject GameObject 
---@field public contactCount number 
---@field public contacts ContactPoint[] 
---@field public impulse Vector3 
---@field public impactForceSum Vector3 
---@field public frictionForceSum Vector3 
---@field public other Component 
local Collision={ }
---
---@public
---@param index number 
---@return ContactPoint 
function Collision:GetContact(index) end
---
---@public
---@param contacts ContactPoint[] 
---@return number 
function Collision:GetContacts(contacts) end
---
---@public
---@param contacts List`1 
---@return number 
function Collision:GetContacts(contacts) end
---
---@public
---@return IEnumerator 
function Collision:GetEnumerator() end
---
UnityEngine.Collision = Collision