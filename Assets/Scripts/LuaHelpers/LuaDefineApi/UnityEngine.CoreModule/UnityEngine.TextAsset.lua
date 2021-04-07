---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class TextAsset : Object
---@field public bytes Byte[] 
---@field public text string 
local TextAsset={ }
---
---@public
---@return string 
function TextAsset:ToString() end
---
UnityEngine.TextAsset = TextAsset