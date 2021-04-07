---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameObject : Object
---@field public transform Transform 
---@field public layer number 
---@field public active boolean 
---@field public activeSelf boolean 
---@field public activeInHierarchy boolean 
---@field public isStatic boolean 
---@field public tag string 
---@field public scene Scene 
---@field public sceneCullingMask number 
---@field public gameObject GameObject 
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
local GameObject={ }
---
---@public
---@param type number 
---@return GameObject 
function GameObject.CreatePrimitive(type) end
---
---@public
---@param type Type 
---@return Component 
function GameObject:GetComponent(type) end
---
---@public
---@param type string 
---@return Component 
function GameObject:GetComponent(type) end
---
---@public
---@param type Type 
---@param includeInactive boolean 
---@return Component 
function GameObject:GetComponentInChildren(type, includeInactive) end
---
---@public
---@param type Type 
---@return Component 
function GameObject:GetComponentInChildren(type) end
---
---@public
---@param type Type 
---@param includeInactive boolean 
---@return Component 
function GameObject:GetComponentInParent(type, includeInactive) end
---
---@public
---@param type Type 
---@return Component 
function GameObject:GetComponentInParent(type) end
---
---@public
---@param type Type 
---@return Component[] 
function GameObject:GetComponents(type) end
---
---@public
---@param type Type 
---@param results List`1 
---@return void 
function GameObject:GetComponents(type, results) end
---
---@public
---@param type Type 
---@return Component[] 
function GameObject:GetComponentsInChildren(type) end
---
---@public
---@param type Type 
---@param includeInactive boolean 
---@return Component[] 
function GameObject:GetComponentsInChildren(type, includeInactive) end
---
---@public
---@param type Type 
---@return Component[] 
function GameObject:GetComponentsInParent(type) end
---
---@public
---@param type Type 
---@param includeInactive boolean 
---@return Component[] 
function GameObject:GetComponentsInParent(type, includeInactive) end
---
---@public
---@param type Type 
---@param component Component& 
---@return boolean 
function GameObject:TryGetComponent(type, component) end
---
---@public
---@param tag string 
---@return GameObject 
function GameObject.FindWithTag(tag) end
---
---@public
---@param methodName string 
---@param options number 
---@return void 
function GameObject:SendMessageUpwards(methodName, options) end
---
---@public
---@param methodName string 
---@param options number 
---@return void 
function GameObject:SendMessage(methodName, options) end
---
---@public
---@param methodName string 
---@param options number 
---@return void 
function GameObject:BroadcastMessage(methodName, options) end
---
---@public
---@param componentType Type 
---@return Component 
function GameObject:AddComponent(componentType) end
---
---@public
---@param value boolean 
---@return void 
function GameObject:SetActive(value) end
---
---@public
---@param state boolean 
---@return void 
function GameObject:SetActiveRecursively(state) end
---
---@public
---@param tag string 
---@return boolean 
function GameObject:CompareTag(tag) end
---
---@public
---@param tag string 
---@return GameObject 
function GameObject.FindGameObjectWithTag(tag) end
---
---@public
---@param tag string 
---@return GameObject[] 
function GameObject.FindGameObjectsWithTag(tag) end
---
---@public
---@param methodName string 
---@param value Object 
---@param options number 
---@return void 
function GameObject:SendMessageUpwards(methodName, value, options) end
---
---@public
---@param methodName string 
---@param value Object 
---@return void 
function GameObject:SendMessageUpwards(methodName, value) end
---
---@public
---@param methodName string 
---@return void 
function GameObject:SendMessageUpwards(methodName) end
---
---@public
---@param methodName string 
---@param value Object 
---@param options number 
---@return void 
function GameObject:SendMessage(methodName, value, options) end
---
---@public
---@param methodName string 
---@param value Object 
---@return void 
function GameObject:SendMessage(methodName, value) end
---
---@public
---@param methodName string 
---@return void 
function GameObject:SendMessage(methodName) end
---
---@public
---@param methodName string 
---@param parameter Object 
---@param options number 
---@return void 
function GameObject:BroadcastMessage(methodName, parameter, options) end
---
---@public
---@param methodName string 
---@param parameter Object 
---@return void 
function GameObject:BroadcastMessage(methodName, parameter) end
---
---@public
---@param methodName string 
---@return void 
function GameObject:BroadcastMessage(methodName) end
---
---@public
---@param name string 
---@return GameObject 
function GameObject.Find(name) end
---
---@public
---@param clip Object 
---@param time number 
---@return void 
function GameObject:SampleAnimation(clip, time) end
---
---@public
---@param className string 
---@return Component 
function GameObject:AddComponent(className) end
---
---@public
---@param animation Object 
---@return void 
function GameObject:PlayAnimation(animation) end
---
---@public
---@return void 
function GameObject:StopAnimation() end
---
UnityEngine.GameObject = GameObject