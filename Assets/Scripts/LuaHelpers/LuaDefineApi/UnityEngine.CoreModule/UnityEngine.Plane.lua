---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Plane : ValueType
---@field public normal Vector3 
---@field public distance number 
---@field public flipped Plane 
local Plane={ }
---
---@public
---@param inNormal Vector3 
---@param inPoint Vector3 
---@return void 
function Plane:SetNormalAndPosition(inNormal, inPoint) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@param c Vector3 
---@return void 
function Plane:Set3Points(a, b, c) end
---
---@public
---@return void 
function Plane:Flip() end
---
---@public
---@param translation Vector3 
---@return void 
function Plane:Translate(translation) end
---
---@public
---@param plane Plane 
---@param translation Vector3 
---@return Plane 
function Plane.Translate(plane, translation) end
---
---@public
---@param point Vector3 
---@return Vector3 
function Plane:ClosestPointOnPlane(point) end
---
---@public
---@param point Vector3 
---@return number 
function Plane:GetDistanceToPoint(point) end
---
---@public
---@param point Vector3 
---@return boolean 
function Plane:GetSide(point) end
---
---@public
---@param inPt0 Vector3 
---@param inPt1 Vector3 
---@return boolean 
function Plane:SameSide(inPt0, inPt1) end
---
---@public
---@param ray Ray 
---@param enter Single& 
---@return boolean 
function Plane:Raycast(ray, enter) end
---
---@public
---@return string 
function Plane:ToString() end
---
---@public
---@param format string 
---@return string 
function Plane:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Plane:ToString(format, formatProvider) end
---
UnityEngine.Plane = Plane