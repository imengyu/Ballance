# Slua

Slua 是 Slua框架提供的一套API。

要了解详细信息，可以访问 [Slua官方文档](https://github.com/pangweiwei/slua/blob/master/help.md) 。

## 方法

### Slua.CreateAction(func)

创建自定义Action

#### 参数

`func` function <br>回调函数

#### 返回

any

### Slua.CreateClass(cls, ...)

使用自定义参数创建C#端的Class

#### 参数

`cls` string <br>类的命名空间

#### 返回

any

### Slua.GetClass(cls)

获取C#端的Class

#### 参数

`cls` string <br>类的命名空间

#### 返回

any

### Slua.iter(o)

迭代对象

#### 参数

`o` any <br>要迭代的对象

#### 返回

table

#### 注解

c#中使用foreach语句遍历IEnumertable,例如List,Array等, 在slua中,可以使用Slua.iter作为迭代函数遍历这些对象, 例如:

```lua
for t in Slua.iter(Canvas.transform) do
  print("foreach transorm",t)
end
```

### Slua.ToString(var)

转为字符串

#### 参数

`var` any <br>

#### 返回

string

### Slua.As(var,type)

强制转换对象为某个类型

#### 参数

`var` any <br>对象

`type` string <br>类的命名空间

#### 返回

any

#### 注解

正常使用情况，一般不会在lua层面做类型转换，因为所有的对象到了lua里都是userdata，在c#层面维护了一张表保存每个userdata的类型，在少数情况下需要downcast为子类的时候，需要在lua层面转换c#的数据类型，可以使用As方法，例如：

```lua
local v = CreateBase() -- 返回BaseObject
local x = Slua.As(v,Child) -- Child继承自Base
```

### Slua.IsNull(var)

判断一个对象是否是null（可以判断Unity对象例如GameObject）

#### 参数

`var` any <br>要判断的参数

#### 返回

boolean

#### 注解

因为Unity GameObject被destroy后，并不是真正的null，而是一个被标记了为destroyed的GameObject，而GameObject重载了==操作符，在c#中可以==判断是否为null（虽然它不是null），而这个gameobject被push到lua后，并不能判断==nil，所以slua提供IsNull函数，用于判断是否GameObject被Destory，或者GetComponent的返回值其实不存在，也可以通过IsNull判断，例如：

```lua
local go = GameObject()
local comp=go.GetComponent(SomeNotExistsComponent)
Slua.IsNull(comp) --true
GameObject.Destroy(go)
Slua.IsNull(go) -- true
```