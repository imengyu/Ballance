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

---LuaTimer用于在限定时间周期性的回调lua函数, 强烈建议不要使用系统自带timer, slua timer会在lua虚拟机被关闭后停止timer工作,而一般系统自带timer可能会在lua虚拟机被关闭后任然触发timer,导致调用lua函数失败,从而产生闪退等.
---@class LuaTimer
LuaTimer = {}

---增加一个一次性Timer, timer在delay时间后触发, 单位ms.
---@param delay number
---@param func function
---@return number
---@returns number 返回一个ID，可以使用 LuaTimer.Delete(id) 删除Timer
function LuaTimer.Add(delay,func) end 

---增加一个Timer, delay表示延迟时间,单位ms, cycle表示周期时间,单位ms, func为回调的lua函数, Add函数返回一个timer id,用于Delete函数删除某个已经添加的Timer
---@param delay number
---@param cycle number
---@param func function
---@return number
---@returns number 返回一个ID，可以使用 LuaTimer.Delete(id) 删除Timer
function LuaTimer.Add(delay,cycle,func) end 

---删除指定id的timer.
---@param id number
function LuaTimer.Delete(id) end 

---uCoroutine Yieldk
---@param y any
---@param f function
UnityEngine.Yieldk = function (y, f) end

---@diagnostic disable-next-line: lowercase-global
uCoroutine = {}

---创建协程
---@param x function
uCoroutine.create = function(x) end
---等待协程
---@param x any
uCoroutine.yield = function(x) end

---Slua 全局命名空间
Slua = SluaNamespace