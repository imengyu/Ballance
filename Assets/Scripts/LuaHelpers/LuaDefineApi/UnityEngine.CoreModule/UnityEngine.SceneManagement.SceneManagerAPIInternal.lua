---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SceneManagerAPIInternal
local SceneManagerAPIInternal={ }
---
---@public
---@return number 
function SceneManagerAPIInternal.GetNumScenesInBuildSettings() end
---
---@public
---@param buildIndex number 
---@return Scene 
function SceneManagerAPIInternal.GetSceneByBuildIndex(buildIndex) end
---
---@public
---@param sceneName string 
---@param sceneBuildIndex number 
---@param parameters LoadSceneParameters 
---@param mustCompleteNextFrame boolean 
---@return AsyncOperation 
function SceneManagerAPIInternal.LoadSceneAsyncNameIndexInternal(sceneName, sceneBuildIndex, parameters, mustCompleteNextFrame) end
---
---@public
---@param sceneName string 
---@param sceneBuildIndex number 
---@param immediately boolean 
---@param options number 
---@param outSuccess Boolean& 
---@return AsyncOperation 
function SceneManagerAPIInternal.UnloadSceneNameIndexInternal(sceneName, sceneBuildIndex, immediately, options, outSuccess) end
---
UnityEngine.SceneManagement.SceneManagerAPIInternal = SceneManagerAPIInternal