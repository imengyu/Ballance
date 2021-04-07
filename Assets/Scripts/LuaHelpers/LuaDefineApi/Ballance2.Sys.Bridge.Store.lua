---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Store
---@field public PoolName string 池的名称
---@field public PoolDatas Dictionary`2 池中的数据
local Store={ }
---
---@public
---@return void 
function Store:Destroy() end
---添加数据
---@public
---@param name string 数据名称
---@param access number 
---@param storeDataType number 
---@return StoreData 添加成功，则返回数据，如果数据已经存在，则返回存在的实例
function Store:AddParameter(name, access, storeDataType) end
---移除数据
---@public
---@param name string 数据名称
---@return boolean 如果移除成功，返回true，如果数据不存在，返回false
function Store:RemoveAddParameter(name) end
---获取池中的数据
---@public
---@param name string 数据名称
---@return StoreData 返回数据实例
function Store:GetParameter(name) end
---全局数据共享存储池类
Ballance2.Sys.Bridge.Store = Store