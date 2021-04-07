---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BurstCompilerService
---@field public IsInitialized boolean 
local BurstCompilerService={ }
---
---@public
---@param m MethodInfo 
---@param compilerOptions string 
---@return string 
function BurstCompilerService.GetDisassembly(m, compilerOptions) end
---
---@public
---@param delegateMethod Object 
---@param compilerOptions string 
---@return number 
function BurstCompilerService.CompileAsyncDelegateMethod(delegateMethod, compilerOptions) end
---
---@public
---@param userID number 
---@return Void* 
function BurstCompilerService.GetAsyncCompiledAsyncDelegateMethod(userID) end
---
---@public
---@param key Hash128& 
---@param size_of number 
---@param alignment number 
---@return Void* 
function BurstCompilerService.GetOrCreateSharedMemory(key, size_of, alignment) end
---
---@public
---@param method MethodInfo 
---@return string 
function BurstCompilerService.GetMethodSignature(method) end
---
---@public
---@param environment number 
---@return void 
function BurstCompilerService.SetCurrentExecutionMode(environment) end
---
---@public
---@return number 
function BurstCompilerService.GetCurrentExecutionMode() end
---
---@public
---@param userData Void* 
---@param logType number 
---@param message Byte* 
---@param filename Byte* 
---@param lineNumber number 
---@return void 
function BurstCompilerService.Log(userData, logType, message, filename, lineNumber) end
---
---@public
---@param fullPathToLibBurstGenerated string 
---@return boolean 
function BurstCompilerService.LoadBurstLibrary(fullPathToLibBurstGenerated) end
---
---@public
---@param folderRuntime string 
---@param extractCompilerFlags ExtractCompilerFlags 
---@return void 
function BurstCompilerService.Initialize(folderRuntime, extractCompilerFlags) end
---
Unity.Burst.LowLevel.BurstCompilerService = BurstCompilerService