---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimatorJobExtensions
local AnimatorJobExtensions={ }
---
---@public
---@param animator Animator 
---@param jobHandle JobHandle 
---@return void 
function AnimatorJobExtensions.AddJobDependency(animator, jobHandle) end
---
---@public
---@param animator Animator 
---@param transform Transform 
---@return TransformStreamHandle 
function AnimatorJobExtensions.BindStreamTransform(animator, transform) end
---
---@public
---@param animator Animator 
---@param transform Transform 
---@param type Type 
---@param property string 
---@return PropertyStreamHandle 
function AnimatorJobExtensions.BindStreamProperty(animator, transform, type, property) end
---
---@public
---@param animator Animator 
---@param property string 
---@param type number 
---@return PropertyStreamHandle 
function AnimatorJobExtensions.BindCustomStreamProperty(animator, property, type) end
---
---@public
---@param animator Animator 
---@param transform Transform 
---@param type Type 
---@param property string 
---@param isObjectReference boolean 
---@return PropertyStreamHandle 
function AnimatorJobExtensions.BindStreamProperty(animator, transform, type, property, isObjectReference) end
---
---@public
---@param animator Animator 
---@param transform Transform 
---@return TransformSceneHandle 
function AnimatorJobExtensions.BindSceneTransform(animator, transform) end
---
---@public
---@param animator Animator 
---@param transform Transform 
---@param type Type 
---@param property string 
---@return PropertySceneHandle 
function AnimatorJobExtensions.BindSceneProperty(animator, transform, type, property) end
---
---@public
---@param animator Animator 
---@param transform Transform 
---@param type Type 
---@param property string 
---@param isObjectReference boolean 
---@return PropertySceneHandle 
function AnimatorJobExtensions.BindSceneProperty(animator, transform, type, property, isObjectReference) end
---
---@public
---@param animator Animator 
---@param stream AnimationStream& 
---@return boolean 
function AnimatorJobExtensions.OpenAnimationStream(animator, stream) end
---
---@public
---@param animator Animator 
---@param stream AnimationStream& 
---@return void 
function AnimatorJobExtensions.CloseAnimationStream(animator, stream) end
---
---@public
---@param animator Animator 
---@return void 
function AnimatorJobExtensions.ResolveAllStreamHandles(animator) end
---
---@public
---@param animator Animator 
---@return void 
function AnimatorJobExtensions.ResolveAllSceneHandles(animator) end
---
UnityEngine.Animations.AnimatorJobExtensions = AnimatorJobExtensions