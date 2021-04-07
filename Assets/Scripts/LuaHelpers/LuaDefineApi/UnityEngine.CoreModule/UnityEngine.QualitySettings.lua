---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class QualitySettings : Object
---@field public currentLevel number 
---@field public pixelLightCount number 
---@field public shadows number 
---@field public shadowProjection number 
---@field public shadowCascades number 
---@field public shadowDistance number 
---@field public shadowResolution number 
---@field public shadowmaskMode number 
---@field public shadowNearPlaneOffset number 
---@field public shadowCascade2Split number 
---@field public shadowCascade4Split Vector3 
---@field public lodBias number 
---@field public anisotropicFiltering number 
---@field public masterTextureLimit number 
---@field public maximumLODLevel number 
---@field public particleRaycastBudget number 
---@field public softParticles boolean 
---@field public softVegetation boolean 
---@field public vSyncCount number 
---@field public antiAliasing number 
---@field public asyncUploadTimeSlice number 
---@field public asyncUploadBufferSize number 
---@field public asyncUploadPersistentBuffer boolean 
---@field public realtimeReflectionProbes boolean 
---@field public billboardsFaceCameraPosition boolean 
---@field public resolutionScalingFixedDPIFactor number 
---@field public renderPipeline RenderPipelineAsset 
---@field public blendWeights number 
---@field public skinWeights number 
---@field public streamingMipmapsActive boolean 
---@field public streamingMipmapsMemoryBudget number 
---@field public streamingMipmapsRenderersPerFrame number 
---@field public streamingMipmapsMaxLevelReduction number 
---@field public streamingMipmapsAddAllCameras boolean 
---@field public streamingMipmapsMaxFileIORequests number 
---@field public maxQueuedFrames number 
---@field public names String[] 
---@field public desiredColorSpace number 
---@field public activeColorSpace number 
local QualitySettings={ }
---
---@public
---@param applyExpensiveChanges boolean 
---@return void 
function QualitySettings.IncreaseLevel(applyExpensiveChanges) end
---
---@public
---@param applyExpensiveChanges boolean 
---@return void 
function QualitySettings.DecreaseLevel(applyExpensiveChanges) end
---
---@public
---@param index number 
---@return void 
function QualitySettings.SetQualityLevel(index) end
---
---@public
---@return void 
function QualitySettings.IncreaseLevel() end
---
---@public
---@return void 
function QualitySettings.DecreaseLevel() end
---
---@public
---@param index number 
---@return RenderPipelineAsset 
function QualitySettings.GetRenderPipelineAssetAt(index) end
---
---@public
---@return number 
function QualitySettings.GetQualityLevel() end
---
---@public
---@param index number 
---@param applyExpensiveChanges boolean 
---@return void 
function QualitySettings.SetQualityLevel(index, applyExpensiveChanges) end
---
UnityEngine.QualitySettings = QualitySettings