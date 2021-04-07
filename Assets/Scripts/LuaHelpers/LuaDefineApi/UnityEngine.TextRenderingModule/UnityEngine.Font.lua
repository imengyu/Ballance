---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Font : Object
---@field public material Material 
---@field public fontNames String[] 
---@field public dynamic boolean 
---@field public ascent number 
---@field public fontSize number 
---@field public characterInfo CharacterInfo[] 
---@field public lineHeight number 
---@field public textureRebuildCallback FontTextureRebuildCallback 
local Font={ }
---
---@public
---@param value Action`1 
---@return void 
function Font.add_textureRebuilt(value) end
---
---@public
---@param value Action`1 
---@return void 
function Font.remove_textureRebuilt(value) end
---
---@public
---@param fontname string 
---@param size number 
---@return Font 
function Font.CreateDynamicFontFromOSFont(fontname, size) end
---
---@public
---@param fontnames String[] 
---@param size number 
---@return Font 
function Font.CreateDynamicFontFromOSFont(fontnames, size) end
---
---@public
---@param str string 
---@return number 
function Font.GetMaxVertsForString(str) end
---
---@public
---@param c Char 
---@return boolean 
function Font:HasCharacter(c) end
---
---@public
---@return String[] 
function Font.GetOSInstalledFontNames() end
---
---@public
---@return String[] 
function Font.GetPathsToOSFonts() end
---
---@public
---@param ch Char 
---@param info CharacterInfo& 
---@param size number 
---@param style number 
---@return boolean 
function Font:GetCharacterInfo(ch, info, size, style) end
---
---@public
---@param ch Char 
---@param info CharacterInfo& 
---@param size number 
---@return boolean 
function Font:GetCharacterInfo(ch, info, size) end
---
---@public
---@param ch Char 
---@param info CharacterInfo& 
---@return boolean 
function Font:GetCharacterInfo(ch, info) end
---
---@public
---@param characters string 
---@param size number 
---@param style number 
---@return void 
function Font:RequestCharactersInTexture(characters, size, style) end
---
---@public
---@param characters string 
---@param size number 
---@return void 
function Font:RequestCharactersInTexture(characters, size) end
---
---@public
---@param characters string 
---@return void 
function Font:RequestCharactersInTexture(characters) end
---
UnityEngine.Font = Font