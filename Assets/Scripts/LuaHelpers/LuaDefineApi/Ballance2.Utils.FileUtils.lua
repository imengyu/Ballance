---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class FileUtils
local FileUtils={ }
---检测文件头是不是zip
---@public
---@param file string 要检测的文件路径
---@return boolean 
function FileUtils.TestFileIsZip(file) end
---检测文件头是不是unityFs
---@public
---@param file string 要检测的文件路径
---@return boolean 
function FileUtils.TestFileIsAssetBundle(file) end
---检测文件头
---@public
---@param file string 要检测的文件路径
---@param head Byte[] 自定义文件头
---@return boolean 
function FileUtils.TestFileHead(file, head) end
---把文件大小（字节）按单位转换为可读的字符串
---@public
---@param longFileSize number 
---@return string 
function FileUtils.GetBetterFileSize(longFileSize) end
---文件工具
Ballance2.Utils.FileUtils = FileUtils