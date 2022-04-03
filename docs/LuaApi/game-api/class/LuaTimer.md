# LuaTimer

LuaTimer 是 Slua框架提供的一个定时器组件。

LuaTimer用于在限定时间周期性的回调lua函数, 强烈建议不要使用系统自带timer, slua timer会在lua虚拟机被关闭后停止timer工作,而一般系统自带timer可能会在lua虚拟机被关闭后任然触发timer,导致调用lua函数失败,从而产生闪退等.

要了解详细信息，可以访问 [Slua官方文档](https://github.com/pangweiwei/slua/blob/master/help.md) 。

## 方法

### LuaTimer.Add(delay,func)

增加一个一次性Timer, timer在delay时间后触发, 单位ms.

#### 参数

`delay` number <br>delay时间后触发, 单位ms

`func` function <br>回调函数

#### 返回

number

### LuaTimer.Add(delay,cycle,func)

增加一个Timer, delay表示延迟时间,单位ms, cycle表示周期时间,单位ms, func为回调的lua函数, Add函数返回一个timer id,用于Delete函数删除某个已经添加的Timer

#### 参数

`delay` number <br>延迟时间,单位ms

`cycle` number <br>周期时间,单位ms

`func` function <br>回调函数

#### 返回

number<br>返回一个ID，可以使用 LuaTimer.Delete(id) 删除Timer

### LuaTimer.Delete(id)

删除指定id的timer

#### 参数

`id` number <br>LuaTimer.Add返回的ID