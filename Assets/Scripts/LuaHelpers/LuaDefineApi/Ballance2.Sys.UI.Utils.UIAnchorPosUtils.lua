---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UIAnchorPosUtils
local UIAnchorPosUtils={ }
---设置 UI 组件锚点
---@public
---@param rectTransform RectTransform UI 组件
---@param x number X 轴锚点
---@param y number Y 轴锚点
---@return void 
function UIAnchorPosUtils.SetUIAnchor(rectTransform, x, y) end
---获取 UI 组件锚点
---@public
---@param rectTransform RectTransform UI 组件
---@return UIAnchor[] 
function UIAnchorPosUtils.GetUIAnchor(rectTransform) end
---
---@public
---@param rectTransform RectTransform 
---@param axis number 
---@return number 
function UIAnchorPosUtils.GetUIAnchor(rectTransform, axis) end
---设置 UI 组件 上 右 坐标
---@public
---@param rectTransform RectTransform 
---@param right number 
---@param top number 
---@return void 
function UIAnchorPosUtils.SetUIRightTop(rectTransform, right, top) end
---设置 UI 组件 左 下坐标
---@public
---@param rectTransform RectTransform 
---@param left number 
---@param bottom number 
---@return void 
function UIAnchorPosUtils.SetUILeftBottom(rectTransform, left, bottom) end
---设置 UI 组件 左 上 右 下坐标
---@public
---@param rectTransform RectTransform 
---@param left number 
---@param top number 
---@param right number 
---@param bottom number 
---@return void 
function UIAnchorPosUtils.SetUIPos(rectTransform, left, top, right, bottom) end
---
---@public
---@param rectTransform RectTransform 
---@return number 
function UIAnchorPosUtils.GetUIRight(rectTransform) end
---
---@public
---@param rectTransform RectTransform 
---@return number 
function UIAnchorPosUtils.GetUITop(rectTransform) end
---
---@public
---@param rectTransform RectTransform 
---@return number 
function UIAnchorPosUtils.GetUILeft(rectTransform) end
---
---@public
---@param rectTransform RectTransform 
---@return number 
function UIAnchorPosUtils.GetUIBottom(rectTransform) end
---设置 UI 组件枢轴
---@public
---@param rectTransform RectTransform UI 组件
---@param pivot number 
---@return void 
function UIAnchorPosUtils.SetUIPivot(rectTransform, pivot) end
---
---@public
---@param rectTransform RectTransform 
---@param pivot number 
---@param axis number 
---@return void 
function UIAnchorPosUtils.SetUIPivot(rectTransform, pivot, axis) end
---获取 UI 组件枢轴
---@public
---@param rectTransform RectTransform UI 组件
---@return number 
function UIAnchorPosUtils.GetUIPivot(rectTransform) end
---
---@public
---@param rectTransform RectTransform 
---@param value number 
---@param axis number 
---@return number 
function UIAnchorPosUtils.GetUIPivotLocationOffest(rectTransform, value, axis) end
---
Ballance2.Sys.UI.Utils.UIAnchorPosUtils = UIAnchorPosUtils