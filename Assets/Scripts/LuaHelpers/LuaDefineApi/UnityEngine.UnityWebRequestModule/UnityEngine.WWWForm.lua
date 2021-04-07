---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class WWWForm
---@field public headers Dictionary`2 
---@field public data Byte[] 
local WWWForm={ }
---
---@public
---@param fieldName string 
---@param value string 
---@return void 
function WWWForm:AddField(fieldName, value) end
---
---@public
---@param fieldName string 
---@param value string 
---@param e Encoding 
---@return void 
function WWWForm:AddField(fieldName, value, e) end
---
---@public
---@param fieldName string 
---@param i number 
---@return void 
function WWWForm:AddField(fieldName, i) end
---
---@public
---@param fieldName string 
---@param contents Byte[] 
---@return void 
function WWWForm:AddBinaryData(fieldName, contents) end
---
---@public
---@param fieldName string 
---@param contents Byte[] 
---@param fileName string 
---@return void 
function WWWForm:AddBinaryData(fieldName, contents, fileName) end
---
---@public
---@param fieldName string 
---@param contents Byte[] 
---@param fileName string 
---@param mimeType string 
---@return void 
function WWWForm:AddBinaryData(fieldName, contents, fileName, mimeType) end
---
UnityEngine.WWWForm = WWWForm