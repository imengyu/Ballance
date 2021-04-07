---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CloneUtils
local CloneUtils={ }
---
---@public
---@param prefab GameObject 
---@param name string 
---@return GameObject 
function CloneUtils.CloneNewObject(prefab, name) end
---
---@public
---@param prefab GameObject 
---@param parent Transform 
---@param name string 
---@return GameObject 
function CloneUtils.CloneNewObjectWithParent(prefab, parent, name) end
---
---@public
---@param prefab GameObject 
---@param parent Transform 
---@return GameObject 
function CloneUtils.CloneNewObjectWithParent(prefab, parent) end
---
---@public
---@param prefab GameObject 
---@param parent Transform 
---@param name string 
---@param active boolean 
---@return GameObject 
function CloneUtils.CloneNewObjectWithParent(prefab, parent, name, active) end
---
---@public
---@param name string 
---@return GameObject 
function CloneUtils.CreateEmptyObject(name) end
---
---@public
---@param parent Transform 
---@param name string 
---@return GameObject 
function CloneUtils.CreateEmptyObjectWithParent(parent, name) end
---
---@public
---@param parent Transform 
---@return GameObject 
function CloneUtils.CreateEmptyObjectWithParent(parent) end
---
---@public
---@param name string 
---@return GameObject 
function CloneUtils.CreateEmptyUIObject(name) end
---
---@public
---@param parent Transform 
---@param name string 
---@return GameObject 
function CloneUtils.CreateEmptyUIObjectWithParent(parent, name) end
---
---@public
---@param parent Transform 
---@return GameObject 
function CloneUtils.CreateEmptyUIObjectWithParent(parent) end
---克隆工具类
Ballance2.Sys.Utils.CloneUtils = CloneUtils