---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Camera : Behaviour
---@field public onPreCull CameraCallback 
---@field public onPreRender CameraCallback 
---@field public onPostRender CameraCallback 
---@field public nearClipPlane number 
---@field public farClipPlane number 
---@field public fieldOfView number 
---@field public renderingPath number 
---@field public actualRenderingPath number 
---@field public allowHDR boolean 
---@field public allowMSAA boolean 
---@field public allowDynamicResolution boolean 
---@field public forceIntoRenderTexture boolean 
---@field public orthographicSize number 
---@field public orthographic boolean 
---@field public opaqueSortMode number 
---@field public transparencySortMode number 
---@field public transparencySortAxis Vector3 
---@field public depth number 
---@field public aspect number 
---@field public velocity Vector3 
---@field public cullingMask number 
---@field public eventMask number 
---@field public layerCullSpherical boolean 
---@field public cameraType number 
---@field public overrideSceneCullingMask number 
---@field public layerCullDistances Single[] 
---@field public useOcclusionCulling boolean 
---@field public cullingMatrix Matrix4x4 
---@field public backgroundColor Color 
---@field public clearFlags number 
---@field public depthTextureMode number 
---@field public clearStencilAfterLightingPass boolean 
---@field public usePhysicalProperties boolean 
---@field public sensorSize Vector2 
---@field public lensShift Vector2 
---@field public focalLength number 
---@field public gateFit number 
---@field public rect Rect 
---@field public pixelRect Rect 
---@field public pixelWidth number 
---@field public pixelHeight number 
---@field public scaledPixelWidth number 
---@field public scaledPixelHeight number 
---@field public targetTexture RenderTexture 
---@field public activeTexture RenderTexture 
---@field public targetDisplay number 
---@field public cameraToWorldMatrix Matrix4x4 
---@field public worldToCameraMatrix Matrix4x4 
---@field public projectionMatrix Matrix4x4 
---@field public nonJitteredProjectionMatrix Matrix4x4 
---@field public useJitteredProjectionMatrixForTransparentRendering boolean 
---@field public previousViewProjectionMatrix Matrix4x4 
---@field public main Camera 
---@field public current Camera 
---@field public scene Scene 
---@field public stereoEnabled boolean 
---@field public stereoSeparation number 
---@field public stereoConvergence number 
---@field public areVRStereoViewMatricesWithinSingleCullTolerance boolean 
---@field public stereoTargetEye number 
---@field public stereoActiveEye number 
---@field public allCamerasCount number 
---@field public allCameras Camera[] 
---@field public commandBufferCount number 
---@field public isOrthoGraphic boolean 
---@field public mainCamera Camera 
---@field public near number 
---@field public far number 
---@field public fov number 
---@field public hdr boolean 
---@field public stereoMirrorMode boolean 
local Camera={ }
---
---@public
---@return void 
function Camera:Reset() end
---
---@public
---@return void 
function Camera:ResetTransparencySortSettings() end
---
---@public
---@return void 
function Camera:ResetAspect() end
---
---@public
---@return void 
function Camera:ResetCullingMatrix() end
---
---@public
---@param shader Shader 
---@param replacementTag string 
---@return void 
function Camera:SetReplacementShader(shader, replacementTag) end
---
---@public
---@return void 
function Camera:ResetReplacementShader() end
---
---@public
---@return number 
function Camera:GetGateFittedFieldOfView() end
---
---@public
---@return Vector2 
function Camera:GetGateFittedLensShift() end
---
---@public
---@param colorBuffer RenderBuffer 
---@param depthBuffer RenderBuffer 
---@return void 
function Camera:SetTargetBuffers(colorBuffer, depthBuffer) end
---
---@public
---@param colorBuffer RenderBuffer[] 
---@param depthBuffer RenderBuffer 
---@return void 
function Camera:SetTargetBuffers(colorBuffer, depthBuffer) end
---
---@public
---@return void 
function Camera:ResetWorldToCameraMatrix() end
---
---@public
---@return void 
function Camera:ResetProjectionMatrix() end
---
---@public
---@param clipPlane Vector4 
---@return Matrix4x4 
function Camera:CalculateObliqueMatrix(clipPlane) end
---
---@public
---@param position Vector3 
---@param eye number 
---@return Vector3 
function Camera:WorldToScreenPoint(position, eye) end
---
---@public
---@param position Vector3 
---@param eye number 
---@return Vector3 
function Camera:WorldToViewportPoint(position, eye) end
---
---@public
---@param position Vector3 
---@param eye number 
---@return Vector3 
function Camera:ViewportToWorldPoint(position, eye) end
---
---@public
---@param position Vector3 
---@param eye number 
---@return Vector3 
function Camera:ScreenToWorldPoint(position, eye) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Camera:WorldToScreenPoint(position) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Camera:WorldToViewportPoint(position) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Camera:ViewportToWorldPoint(position) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Camera:ScreenToWorldPoint(position) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Camera:ScreenToViewportPoint(position) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Camera:ViewportToScreenPoint(position) end
---
---@public
---@param pos Vector3 
---@param eye number 
---@return Ray 
function Camera:ViewportPointToRay(pos, eye) end
---
---@public
---@param pos Vector3 
---@return Ray 
function Camera:ViewportPointToRay(pos) end
---
---@public
---@param pos Vector3 
---@param eye number 
---@return Ray 
function Camera:ScreenPointToRay(pos, eye) end
---
---@public
---@param pos Vector3 
---@return Ray 
function Camera:ScreenPointToRay(pos) end
---
---@public
---@param viewport Rect 
---@param z number 
---@param eye number 
---@param outCorners Vector3[] 
---@return void 
function Camera:CalculateFrustumCorners(viewport, z, eye, outCorners) end
---
---@public
---@param output Matrix4x4& 
---@param focalLength number 
---@param sensorSize Vector2 
---@param lensShift Vector2 
---@param nearClip number 
---@param farClip number 
---@param gateFitParameters GateFitParameters 
---@return void 
function Camera.CalculateProjectionMatrixFromPhysicalProperties(output, focalLength, sensorSize, lensShift, nearClip, farClip, gateFitParameters) end
---
---@public
---@param focalLength number 
---@param sensorSize number 
---@return number 
function Camera.FocalLengthToFieldOfView(focalLength, sensorSize) end
---
---@public
---@param fieldOfView number 
---@param sensorSize number 
---@return number 
function Camera.FieldOfViewToFocalLength(fieldOfView, sensorSize) end
---
---@public
---@param horizontalFieldOfView number 
---@param aspectRatio number 
---@return number 
function Camera.HorizontalToVerticalFieldOfView(horizontalFieldOfView, aspectRatio) end
---
---@public
---@param verticalFieldOfView number 
---@param aspectRatio number 
---@return number 
function Camera.VerticalToHorizontalFieldOfView(verticalFieldOfView, aspectRatio) end
---
---@public
---@param eye number 
---@return Matrix4x4 
function Camera:GetStereoNonJitteredProjectionMatrix(eye) end
---
---@public
---@param eye number 
---@return Matrix4x4 
function Camera:GetStereoViewMatrix(eye) end
---
---@public
---@param eye number 
---@return void 
function Camera:CopyStereoDeviceProjectionMatrixToNonJittered(eye) end
---
---@public
---@param eye number 
---@return Matrix4x4 
function Camera:GetStereoProjectionMatrix(eye) end
---
---@public
---@param eye number 
---@param matrix Matrix4x4 
---@return void 
function Camera:SetStereoProjectionMatrix(eye, matrix) end
---
---@public
---@return void 
function Camera:ResetStereoProjectionMatrices() end
---
---@public
---@param eye number 
---@param matrix Matrix4x4 
---@return void 
function Camera:SetStereoViewMatrix(eye, matrix) end
---
---@public
---@return void 
function Camera:ResetStereoViewMatrices() end
---
---@public
---@param cameras Camera[] 
---@return number 
function Camera.GetAllCameras(cameras) end
---
---@public
---@param cubemap Cubemap 
---@param faceMask number 
---@return boolean 
function Camera:RenderToCubemap(cubemap, faceMask) end
---
---@public
---@param cubemap Cubemap 
---@return boolean 
function Camera:RenderToCubemap(cubemap) end
---
---@public
---@param cubemap RenderTexture 
---@param faceMask number 
---@return boolean 
function Camera:RenderToCubemap(cubemap, faceMask) end
---
---@public
---@param cubemap RenderTexture 
---@return boolean 
function Camera:RenderToCubemap(cubemap) end
---
---@public
---@param cubemap RenderTexture 
---@param faceMask number 
---@param stereoEye number 
---@return boolean 
function Camera:RenderToCubemap(cubemap, faceMask, stereoEye) end
---
---@public
---@return void 
function Camera:Render() end
---
---@public
---@param shader Shader 
---@param replacementTag string 
---@return void 
function Camera:RenderWithShader(shader, replacementTag) end
---
---@public
---@return void 
function Camera:RenderDontRestore() end
---
---@public
---@param renderRequests List`1 
---@return void 
function Camera:SubmitRenderRequests(renderRequests) end
---
---@public
---@param cur Camera 
---@return void 
function Camera.SetupCurrent(cur) end
---
---@public
---@param other Camera 
---@return void 
function Camera:CopyFrom(other) end
---
---@public
---@param evt number 
---@return void 
function Camera:RemoveCommandBuffers(evt) end
---
---@public
---@return void 
function Camera:RemoveAllCommandBuffers() end
---
---@public
---@param evt number 
---@param buffer CommandBuffer 
---@return void 
function Camera:AddCommandBuffer(evt, buffer) end
---
---@public
---@param evt number 
---@param buffer CommandBuffer 
---@param queueType number 
---@return void 
function Camera:AddCommandBufferAsync(evt, buffer, queueType) end
---
---@public
---@param evt number 
---@param buffer CommandBuffer 
---@return void 
function Camera:RemoveCommandBuffer(evt, buffer) end
---
---@public
---@param evt number 
---@return CommandBuffer[] 
function Camera:GetCommandBuffers(evt) end
---
---@public
---@param cullingParameters ScriptableCullingParameters& 
---@return boolean 
function Camera:TryGetCullingParameters(cullingParameters) end
---
---@public
---@param stereoAware boolean 
---@param cullingParameters ScriptableCullingParameters& 
---@return boolean 
function Camera:TryGetCullingParameters(stereoAware, cullingParameters) end
---
---@public
---@return number 
function Camera:GetScreenWidth() end
---
---@public
---@return number 
function Camera:GetScreenHeight() end
---
---@public
---@return void 
function Camera:DoClear() end
---
---@public
---@return void 
function Camera:ResetFieldOfView() end
---
---@public
---@param leftMatrix Matrix4x4 
---@param rightMatrix Matrix4x4 
---@return void 
function Camera:SetStereoViewMatrices(leftMatrix, rightMatrix) end
---
---@public
---@param leftMatrix Matrix4x4 
---@param rightMatrix Matrix4x4 
---@return void 
function Camera:SetStereoProjectionMatrices(leftMatrix, rightMatrix) end
---
---@public
---@return Matrix4x4[] 
function Camera:GetStereoViewMatrices() end
---
---@public
---@return Matrix4x4[] 
function Camera:GetStereoProjectionMatrices() end
---
UnityEngine.Camera = Camera