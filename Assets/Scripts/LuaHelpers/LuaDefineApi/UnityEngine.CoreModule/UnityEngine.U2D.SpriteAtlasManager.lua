---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SpriteAtlasManager
local SpriteAtlasManager={ }
---
---@public
---@param value Action`2 
---@return void 
function SpriteAtlasManager.add_atlasRequested(value) end
---
---@public
---@param value Action`2 
---@return void 
function SpriteAtlasManager.remove_atlasRequested(value) end
---
---@public
---@param value Action`1 
---@return void 
function SpriteAtlasManager.add_atlasRegistered(value) end
---
---@public
---@param value Action`1 
---@return void 
function SpriteAtlasManager.remove_atlasRegistered(value) end
---
UnityEngine.U2D.SpriteAtlasManager = SpriteAtlasManager