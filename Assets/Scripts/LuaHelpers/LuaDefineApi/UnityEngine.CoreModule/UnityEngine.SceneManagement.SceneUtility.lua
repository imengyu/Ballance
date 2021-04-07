---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SceneUtility
local SceneUtility={ }
---
---@public
---@param buildIndex number 
---@return string 
function SceneUtility.GetScenePathByBuildIndex(buildIndex) end
---
---@public
---@param scenePath string 
---@return number 
function SceneUtility.GetBuildIndexByScenePath(scenePath) end
---
UnityEngine.SceneManagement.SceneUtility = SceneUtility