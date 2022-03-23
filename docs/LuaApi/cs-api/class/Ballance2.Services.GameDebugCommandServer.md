# Ballance2.Services.GameDebugCommandServer 
调试命令服务，使用此服务添加自定义调试命令

## 注解

在游戏中按 <kbd>F12</kbd> 呼出调试控制台，在调试控制台中可以输入调试命令执行。

## 示例


你可以使用 RegisterCommand 注册自定义调试命令，例如：
```lua
GameDebugCommandServer:RegisterCommand('mycommand', function (keyword, fullCmd, argsCount, args) 
  ---这里使用了 DebugUtils 中的调试工具函数获取参数
  local ox, nx = DebugUtils.CheckIntDebugParam(0, args, Slua.out, true, 0)
  if not ox then return false end

  --获取参数成功后就可以执行自定义方法了
  return true
end, 1, 'mycommand <count:number> > 测试调试命令')
```



## 方法



### ExecuteCommand(cmd)

解析并运行指定命令字符串


#### 参数


`cmd` string <br/>命令字符串



#### 返回值

boolean <br/>返回是否成功


### RegisterCommand(keyword, callback, limitArgCount, helpText)

注册调试命令


#### 参数


`keyword` string <br/>命令单词，不能有空格

`callback` `回调` CommandDelegate(keyword: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), fullCmd: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), argsCount: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32), args: [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[])) -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) <br/>命令回调

`limitArgCount` number [int](../types.md)<br/>命令最低参数，默认 0 表示无参数或不限制

`helpText` string <br/>命令帮助文字



#### 返回值

number [int](../types.md)<br/>成功返回命令ID，不成功返回-1


### UnRegisterCommand(cmdId)

取消注册命令


#### 参数


`cmdId` number [int](../types.md)<br/>命令ID




### IsCommandRegistered(keyword)

获取命令是否注册


#### 参数


`keyword` string <br/>命令单词



#### 返回值

boolean <br/>返回命令是否注册
