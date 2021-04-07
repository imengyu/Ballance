---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Component : Object
---@field public transform Transform 
---@field public gameObject GameObject 
---@field public tag string 
---@field public rigidbody Component 
---@field public rigidbody2D Component 
---@field public camera Component 
---@field public light Component 
---@field public animation Component 
---@field public constantForce Component 
---@field public renderer Component 
---@field public audio Component 
---@field public networkView Component 
---@field public collider Component 
---@field public collider2D Component 
---@field public hingeJoint Component 
---@field public particleSystem Component 
local Component={ }
---
---@public
---@param type Type 
---@return Component 
function Component:GetComponent(type) end
---
---@public
---@param type Type 
---@param component Component& 
---@return boolean 
function Component:TryGetComponent(type, component) end
---
---@public
---@param type string 
---@return Component 
function Component:GetComponent(type) end
---
---@public
---@param t Type 
---@param includeInactive boolean 
---@return Component 
function Component:GetComponentInChildren(t, includeInactive) end
---
---@public
---@param t Type 
---@return Component 
function Component:GetComponentInChildren(t) end
---
---@public
---@param t Type 
---@param includeInactive boolean 
---@return Component[] 
function Component:GetComponentsInChildren(t, includeInactive) end
---
---@public
---@param t Type 
---@return Component[] 
function Component:GetComponentsInChildren(t) end
---
---@public
---@param t Type 
---@return Component 
function Component:GetComponentInParent(t) end
---
---@public
---@param t Type 
---@param includeInactive boolean 
---@return Component[] 
function Component:GetComponentsInParent(t, includeInactive) end
---
---@public
---@param t Type 
---@return Component[] 
function Component:GetComponentsInParent(t) end
---
---@public
---@param type Type 
---@return Component[] 
function Component:GetComponents(type) end
---
---@public
---@param type Type 
---@param results List`1 
---@return void 
function Component:GetComponents(type, results) end
---
---@public
---@param tag string 
---@return boolean 
function Component:CompareTag(tag) end
---
---@public
---@param methodName string 
---@param value Object 
---@param options number 
---@return void 
function Component:SendMessageUpwards(methodName, value, options) end
---
---@public
---@param methodName string 
---@param value Object 
---@return void 
function Component:SendMessageUpwards(methodName, value) end
---
---@public
---@param methodName string 
---@return void 
function Component:SendMessageUpwards(methodName) end
---
---@public
---@param methodName string 
---@param options number 
---@return void 
function Component:SendMessageUpwards(methodName, options) end
---
---@public
---@param methodName string 
---@param value Object 
---@return void 
function Component:SendMessage(methodName, value) end
---
---@public
---@param methodName string 
---@return void 
function Component:SendMessage(methodName) end
---
---@public
---@param methodName string 
---@param value Object 
---@param options number 
---@return void 
function Component:SendMessage(methodName, value, options) end
---
---@public
---@param methodName string 
---@param options number 
---@return void 
function Component:SendMessage(methodName, options) end
---
---@public
---@param methodName string 
---@param parameter Object 
---@param options number 
---@return void 
function Component:BroadcastMessage(methodName, parameter, options) end
---
---@public
---@param methodName string 
---@param parameter Object 
---@return void 
function Component:BroadcastMessage(methodName, parameter) end
---
---@public
---@param methodName string 
---@return void 
function Component:BroadcastMessage(methodName) end
---
---@public
---@param methodName string 
---@param options number 
---@return void 
function Component:BroadcastMessage(methodName, options) end
---
UnityEngine.Component = Component