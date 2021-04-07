---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RectTransformUtility
local RectTransformUtility={ }
---
---@public
---@param point Vector2 
---@param elementTransform Transform 
---@param canvas Canvas 
---@return Vector2 
function RectTransformUtility.PixelAdjustPoint(point, elementTransform, canvas) end
---
---@public
---@param rectTransform RectTransform 
---@param canvas Canvas 
---@return Rect 
function RectTransformUtility.PixelAdjustRect(rectTransform, canvas) end
---
---@public
---@param rect RectTransform 
---@param screenPoint Vector2 
---@return boolean 
function RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint) end
---
---@public
---@param rect RectTransform 
---@param screenPoint Vector2 
---@param cam Camera 
---@return boolean 
function RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, cam) end
---
---@public
---@param rect RectTransform 
---@param screenPoint Vector2 
---@param cam Camera 
---@param offset Vector4 
---@return boolean 
function RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, cam, offset) end
---
---@public
---@param rect RectTransform 
---@param screenPoint Vector2 
---@param cam Camera 
---@param worldPoint Vector3& 
---@return boolean 
function RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPoint, cam, worldPoint) end
---
---@public
---@param rect RectTransform 
---@param screenPoint Vector2 
---@param cam Camera 
---@param localPoint Vector2& 
---@return boolean 
function RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, localPoint) end
---
---@public
---@param cam Camera 
---@param screenPos Vector2 
---@return Ray 
function RectTransformUtility.ScreenPointToRay(cam, screenPos) end
---
---@public
---@param cam Camera 
---@param worldPoint Vector3 
---@return Vector2 
function RectTransformUtility.WorldToScreenPoint(cam, worldPoint) end
---
---@public
---@param root Transform 
---@param child Transform 
---@return Bounds 
function RectTransformUtility.CalculateRelativeRectTransformBounds(root, child) end
---
---@public
---@param trans Transform 
---@return Bounds 
function RectTransformUtility.CalculateRelativeRectTransformBounds(trans) end
---
---@public
---@param rect RectTransform 
---@param axis number 
---@param keepPositioning boolean 
---@param recursive boolean 
---@return void 
function RectTransformUtility.FlipLayoutOnAxis(rect, axis, keepPositioning, recursive) end
---
---@public
---@param rect RectTransform 
---@param keepPositioning boolean 
---@param recursive boolean 
---@return void 
function RectTransformUtility.FlipLayoutAxes(rect, keepPositioning, recursive) end
---
UnityEngine.RectTransformUtility = RectTransformUtility