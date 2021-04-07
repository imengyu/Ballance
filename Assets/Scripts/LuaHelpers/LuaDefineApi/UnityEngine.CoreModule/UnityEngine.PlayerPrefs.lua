---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PlayerPrefs
local PlayerPrefs={ }
---
---@public
---@param key string 
---@param value number 
---@return void 
function PlayerPrefs.SetInt(key, value) end
---
---@public
---@param key string 
---@param defaultValue number 
---@return number 
function PlayerPrefs.GetInt(key, defaultValue) end
---
---@public
---@param key string 
---@return number 
function PlayerPrefs.GetInt(key) end
---
---@public
---@param key string 
---@param value number 
---@return void 
function PlayerPrefs.SetFloat(key, value) end
---
---@public
---@param key string 
---@param defaultValue number 
---@return number 
function PlayerPrefs.GetFloat(key, defaultValue) end
---
---@public
---@param key string 
---@return number 
function PlayerPrefs.GetFloat(key) end
---
---@public
---@param key string 
---@param value string 
---@return void 
function PlayerPrefs.SetString(key, value) end
---
---@public
---@param key string 
---@param defaultValue string 
---@return string 
function PlayerPrefs.GetString(key, defaultValue) end
---
---@public
---@param key string 
---@return string 
function PlayerPrefs.GetString(key) end
---
---@public
---@param key string 
---@return boolean 
function PlayerPrefs.HasKey(key) end
---
---@public
---@param key string 
---@return void 
function PlayerPrefs.DeleteKey(key) end
---
---@public
---@return void 
function PlayerPrefs.DeleteAll() end
---
---@public
---@return void 
function PlayerPrefs.Save() end
---
UnityEngine.PlayerPrefs = PlayerPrefs