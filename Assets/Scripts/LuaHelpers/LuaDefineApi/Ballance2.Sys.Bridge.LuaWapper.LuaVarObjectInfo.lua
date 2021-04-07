---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaVarObjectInfo
---@field public Type number 值类型
---@field public Name string 值名称
local LuaVarObjectInfo={ }
---
---@public
---@return string 
function LuaVarObjectInfo:ToString() end
---
---@public
---@return Vector2 
function LuaVarObjectInfo:Vector2() end
---
---@public
---@return Vector2Int 
function LuaVarObjectInfo:Vector2Int() end
---
---@public
---@return Vector3 
function LuaVarObjectInfo:Vector3() end
---
---@public
---@return Vector3Int 
function LuaVarObjectInfo:Vector3Int() end
---
---@public
---@return Vector4 
function LuaVarObjectInfo:Vector4() end
---
---@public
---@return Rect 
function LuaVarObjectInfo:Rect() end
---
---@public
---@return RectInt 
function LuaVarObjectInfo:RectInt() end
---
---@public
---@return Gradient 
function LuaVarObjectInfo:Gradient() end
---
---@public
---@return number 
function LuaVarObjectInfo:Layer() end
---
---@public
---@return AnimationCurve 
function LuaVarObjectInfo:Curve() end
---
---@public
---@return Color 
function LuaVarObjectInfo:Color() end
---
---@public
---@return BoundsInt 
function LuaVarObjectInfo:BoundsInt() end
---
---@public
---@return Bounds 
function LuaVarObjectInfo:Bounds() end
---
---@public
---@return Object 
function LuaVarObjectInfo:Object() end
---
---@public
---@return Object 
function LuaVarObjectInfo:GameObject() end
---
---@public
---@return number 
function LuaVarObjectInfo:Long() end
---
---@public
---@return string 
function LuaVarObjectInfo:String() end
---
---@public
---@return number 
function LuaVarObjectInfo:Int() end
---
---@public
---@return number 
function LuaVarObjectInfo:Double() end
---
---@public
---@return boolean 
function LuaVarObjectInfo:Bool() end
---更新数据到 Lua
---@public
---@param LuaSelf LuaTable 
---@return void 
function LuaVarObjectInfo:UpdateToLua(LuaSelf) end
---从 Lua 获取数据
---@public
---@param LuaSelf LuaTable 
---@return void 
function LuaVarObjectInfo:UpdateFromLua(LuaSelf) end
---lua 引入 var 信息
Ballance2.Sys.Bridge.LuaWapper.LuaVarObjectInfo = LuaVarObjectInfo