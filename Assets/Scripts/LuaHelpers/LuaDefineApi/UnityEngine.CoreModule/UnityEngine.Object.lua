---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Object
---@field public name string 
---@field public hideFlags number 
local Object={ }
---
---@public
---@return number 
function Object:GetInstanceID() end
---
---@public
---@return number 
function Object:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Object:Equals(other) end
---
---@public
---@param exists Object 
---@return boolean 
function Object.op_Implicit(exists) end
---
---@public
---@param original Object 
---@param position Vector3 
---@param rotation Quaternion 
---@return Object 
function Object.Instantiate(original, position, rotation) end
---
---@public
---@param original Object 
---@param position Vector3 
---@param rotation Quaternion 
---@param parent Transform 
---@return Object 
function Object.Instantiate(original, position, rotation, parent) end
---
---@public
---@param original Object 
---@return Object 
function Object.Instantiate(original) end
---
---@public
---@param original Object 
---@param parent Transform 
---@return Object 
function Object.Instantiate(original, parent) end
---
---@public
---@param original Object 
---@param parent Transform 
---@param instantiateInWorldSpace boolean 
---@return Object 
function Object.Instantiate(original, parent, instantiateInWorldSpace) end
---
---@public
---@param obj Object 
---@param t number 
---@return void 
function Object.Destroy(obj, t) end
---
---@public
---@param obj Object 
---@return void 
function Object.Destroy(obj) end
---
---@public
---@param obj Object 
---@param allowDestroyingAssets boolean 
---@return void 
function Object.DestroyImmediate(obj, allowDestroyingAssets) end
---
---@public
---@param obj Object 
---@return void 
function Object.DestroyImmediate(obj) end
---
---@public
---@param type Type 
---@return Object[] 
function Object.FindObjectsOfType(type) end
---
---@public
---@param type Type 
---@param includeInactive boolean 
---@return Object[] 
function Object.FindObjectsOfType(type, includeInactive) end
---
---@public
---@param target Object 
---@return void 
function Object.DontDestroyOnLoad(target) end
---
---@public
---@param obj Object 
---@param t number 
---@return void 
function Object.DestroyObject(obj, t) end
---
---@public
---@param obj Object 
---@return void 
function Object.DestroyObject(obj) end
---
---@public
---@param type Type 
---@return Object[] 
function Object.FindSceneObjectsOfType(type) end
---
---@public
---@param type Type 
---@return Object[] 
function Object.FindObjectsOfTypeIncludingAssets(type) end
---
---@public
---@param type Type 
---@return Object[] 
function Object.FindObjectsOfTypeAll(type) end
---
---@public
---@param type Type 
---@return Object 
function Object.FindObjectOfType(type) end
---
---@public
---@param type Type 
---@param includeInactive boolean 
---@return Object 
function Object.FindObjectOfType(type, includeInactive) end
---
---@public
---@return string 
function Object:ToString() end
---
---@public
---@param x Object 
---@param y Object 
---@return boolean 
function Object.op_Equality(x, y) end
---
---@public
---@param x Object 
---@param y Object 
---@return boolean 
function Object.op_Inequality(x, y) end
---
UnityEngine.Object = Object