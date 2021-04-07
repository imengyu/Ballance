---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class DrawHandler : MulticastDelegate
local DrawHandler={ }
---
---@public
---@param style GUIStyle 
---@param rect Rect 
---@param content GUIContent 
---@param states DrawStates 
---@return boolean 
function DrawHandler:Invoke(style, rect, content, states) end
---
---@public
---@param style GUIStyle 
---@param rect Rect 
---@param content GUIContent 
---@param states DrawStates 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function DrawHandler:BeginInvoke(style, rect, content, states, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return boolean 
function DrawHandler:EndInvoke(result) end
---
UnityEngine.DrawHandler = DrawHandler