---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AssertionMessageUtil
local AssertionMessageUtil={ }
---
---@public
---@param failureMessage string 
---@return string 
function AssertionMessageUtil.GetMessage(failureMessage) end
---
---@public
---@param failureMessage string 
---@param expected string 
---@return string 
function AssertionMessageUtil.GetMessage(failureMessage, expected) end
---
---@public
---@param actual Object 
---@param expected Object 
---@param expectEqual boolean 
---@return string 
function AssertionMessageUtil.GetEqualityMessage(actual, expected, expectEqual) end
---
---@public
---@param value Object 
---@param expectNull boolean 
---@return string 
function AssertionMessageUtil.NullFailureMessage(value, expectNull) end
---
---@public
---@param expected boolean 
---@return string 
function AssertionMessageUtil.BooleanFailureMessage(expected) end
---
UnityEngine.Assertions.AssertionMessageUtil = AssertionMessageUtil