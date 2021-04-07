---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ISerializationCallbackReceiver
local ISerializationCallbackReceiver={ }
---
---@public
---@return void 
function ISerializationCallbackReceiver:OnBeforeSerialize() end
---
---@public
---@return void 
function ISerializationCallbackReceiver:OnAfterDeserialize() end
---
UnityEngine.ISerializationCallbackReceiver = ISerializationCallbackReceiver