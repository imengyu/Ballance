---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ErrorState : ValueType
---@field public ErrorCode number 
---@field public NativeErrorStatePtr Baselib_ErrorState* 
local ErrorState={ }
---
---@public
---@return void 
function ErrorState:ThrowIfFailed() end
---
---@public
---@param verbosity number 
---@return string 
function ErrorState:Explain(verbosity) end
---
Unity.Baselib.ErrorState = ErrorState