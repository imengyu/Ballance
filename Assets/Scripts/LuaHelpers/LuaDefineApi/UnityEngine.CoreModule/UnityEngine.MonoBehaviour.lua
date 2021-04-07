---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class MonoBehaviour : Behaviour
---@field public useGUILayout boolean 
---@field public runInEditMode boolean 
local MonoBehaviour={ }
---
---@public
---@return boolean 
function MonoBehaviour:IsInvoking() end
---
---@public
---@return void 
function MonoBehaviour:CancelInvoke() end
---
---@public
---@param methodName string 
---@param time number 
---@return void 
function MonoBehaviour:Invoke(methodName, time) end
---
---@public
---@param methodName string 
---@param time number 
---@param repeatRate number 
---@return void 
function MonoBehaviour:InvokeRepeating(methodName, time, repeatRate) end
---
---@public
---@param methodName string 
---@return void 
function MonoBehaviour:CancelInvoke(methodName) end
---
---@public
---@param methodName string 
---@return boolean 
function MonoBehaviour:IsInvoking(methodName) end
---
---@public
---@param methodName string 
---@return Coroutine 
function MonoBehaviour:StartCoroutine(methodName) end
---
---@public
---@param methodName string 
---@param value Object 
---@return Coroutine 
function MonoBehaviour:StartCoroutine(methodName, value) end
---
---@public
---@param routine IEnumerator 
---@return Coroutine 
function MonoBehaviour:StartCoroutine(routine) end
---
---@public
---@param routine IEnumerator 
---@return Coroutine 
function MonoBehaviour:StartCoroutine_Auto(routine) end
---
---@public
---@param routine IEnumerator 
---@return void 
function MonoBehaviour:StopCoroutine(routine) end
---
---@public
---@param routine Coroutine 
---@return void 
function MonoBehaviour:StopCoroutine(routine) end
---
---@public
---@param methodName string 
---@return void 
function MonoBehaviour:StopCoroutine(methodName) end
---
---@public
---@return void 
function MonoBehaviour:StopAllCoroutines() end
---
---@public
---@param message Object 
---@return void 
function MonoBehaviour.print(message) end
---
UnityEngine.MonoBehaviour = MonoBehaviour