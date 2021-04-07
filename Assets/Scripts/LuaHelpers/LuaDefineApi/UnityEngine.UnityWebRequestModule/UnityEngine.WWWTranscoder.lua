---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class WWWTranscoder
local WWWTranscoder={ }
---
---@public
---@param toEncode string 
---@return string 
function WWWTranscoder.URLEncode(toEncode) end
---
---@public
---@param toEncode string 
---@param e Encoding 
---@return string 
function WWWTranscoder.URLEncode(toEncode, e) end
---
---@public
---@param toEncode Byte[] 
---@return Byte[] 
function WWWTranscoder.URLEncode(toEncode) end
---
---@public
---@param toEncode string 
---@return string 
function WWWTranscoder.DataEncode(toEncode) end
---
---@public
---@param toEncode string 
---@param e Encoding 
---@return string 
function WWWTranscoder.DataEncode(toEncode, e) end
---
---@public
---@param toEncode Byte[] 
---@return Byte[] 
function WWWTranscoder.DataEncode(toEncode) end
---
---@public
---@param toEncode string 
---@return string 
function WWWTranscoder.QPEncode(toEncode) end
---
---@public
---@param toEncode string 
---@param e Encoding 
---@return string 
function WWWTranscoder.QPEncode(toEncode, e) end
---
---@public
---@param toEncode Byte[] 
---@return Byte[] 
function WWWTranscoder.QPEncode(toEncode) end
---
---@public
---@param input Byte[] 
---@param escapeChar number 
---@param space Byte[] 
---@param forbidden Byte[] 
---@param uppercase boolean 
---@return Byte[] 
function WWWTranscoder.Encode(input, escapeChar, space, forbidden, uppercase) end
---
---@public
---@param toEncode string 
---@return string 
function WWWTranscoder.URLDecode(toEncode) end
---
---@public
---@param toEncode string 
---@param e Encoding 
---@return string 
function WWWTranscoder.URLDecode(toEncode, e) end
---
---@public
---@param toEncode Byte[] 
---@return Byte[] 
function WWWTranscoder.URLDecode(toEncode) end
---
---@public
---@param toDecode string 
---@return string 
function WWWTranscoder.DataDecode(toDecode) end
---
---@public
---@param toDecode string 
---@param e Encoding 
---@return string 
function WWWTranscoder.DataDecode(toDecode, e) end
---
---@public
---@param toDecode Byte[] 
---@return Byte[] 
function WWWTranscoder.DataDecode(toDecode) end
---
---@public
---@param toEncode string 
---@return string 
function WWWTranscoder.QPDecode(toEncode) end
---
---@public
---@param toEncode string 
---@param e Encoding 
---@return string 
function WWWTranscoder.QPDecode(toEncode, e) end
---
---@public
---@param toEncode Byte[] 
---@return Byte[] 
function WWWTranscoder.QPDecode(toEncode) end
---
---@public
---@param input Byte[] 
---@param escapeChar number 
---@param space Byte[] 
---@return Byte[] 
function WWWTranscoder.Decode(input, escapeChar, space) end
---
---@public
---@param s string 
---@return boolean 
function WWWTranscoder.SevenBitClean(s) end
---
---@public
---@param s string 
---@param e Encoding 
---@return boolean 
function WWWTranscoder.SevenBitClean(s, e) end
---
---@public
---@param input Byte[] 
---@return boolean 
function WWWTranscoder.SevenBitClean(input) end
---
UnityEngine.WWWTranscoder = WWWTranscoder