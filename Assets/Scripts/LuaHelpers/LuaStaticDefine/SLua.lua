---@class SluaNamespace
---@field public out any Slua.out 参见文档使用方法
---@field public version string Slua 版本
local SluaNamespace={ }

---创建自定义Action
---@param func function 回调函数
---@return any
function Slua.CreateAction(func) end

---使用自定义参数创建C#端的Class
---@param cls string 类的命名空间 
function Slua.CreateClass(cls, ...) end

---获取C#端的Class
---@param cls string 类的命名空间
---@return any
function Slua.GetClass(cls) end

---迭代对象
---@param o any
---@return table
function Slua.iter(o) end

---转为字符串
---@param var any
---@return string
function Slua.ToString(var) end
---强制转换对象为某个类型
---@param var any
---@param type string 类的命名空间
---@return any
function Slua.As(var,type) end
---@public
---判断一个对象是否是null（可以判断Unity对象例如GameObject）
---@param var any 要判断的参数
---@return boolean
function Slua.IsNull(var) end
---创建数组
---@param type type|string
---@param arr table
---@return table
function Slua.MakeArray(type, arr) end
---转为Bytes数组
---@param arr any
---@return table
function Slua.ToBytes(arr) end

---Slua 全局命名空间
Slua = SluaNamespace