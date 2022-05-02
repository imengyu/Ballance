# Ballance2.Services.GameTimeMachine 
更新状态管理器

## 注解

TimeMachine提供了另一种方式，实现类似MonoBehaviour中的Update之类的方法的功能。以便于我们在某些场合更方便的编写时间驱动的代码。

## 字段

|名称|类型|说明|
|---|---|---|
|FixedUpdateTick|number [int](../types.md)||

## 方法



### RegisterUpdate(updateAction, order, interval)

注册 Update 更新函数


#### 参数


`updateAction` `回调` Action() <br/>更新函数。

`order` number [int](../types.md)<br/>函数的更新顺序。顺序越小，越先被调用。

`interval` number [int](../types.md)<br/>更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。



#### 返回值

[GameTimeMachineTimeTicket](./Ballance2.Services.GameTimeMachine+GameTimeMachineTimeTicket.md) <br/>返回一个更新实例，使用此实例可以取消注册更新函数。


### UnRegisterUpdate(updateAction)

取消注册 Update 更新函数


#### 参数


`updateAction` `回调` Action() <br/>更新函数。




### RegisterLateUpdate(updateAction, order, interval)

注册 LateUpdate 更新函数


#### 参数


`updateAction` `回调` Action() <br/>更新函数。

`order` number [int](../types.md)<br/>函数的更新顺序。顺序越小，越先被调用。

`interval` number [int](../types.md)<br/>更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。



#### 返回值

[GameTimeMachineTimeTicket](./Ballance2.Services.GameTimeMachine+GameTimeMachineTimeTicket.md) <br/>返回一个更新实例，使用此实例可以取消注册更新函数。


### UnRegisterLateUpdate(updateAction)

取消注册 LateUpdate 更新函数


#### 参数


`updateAction` `回调` Action() <br/>更新函数。




### RegisterFixedUpdate(updateAction, order, interval)

注册 FixedUpdate 更新函数


#### 参数


`updateAction` `回调` Action() <br/>更新函数。

`order` number [int](../types.md)<br/>函数的更新顺序。顺序越小，越先被调用。

`interval` number [int](../types.md)<br/>更新函数的更新帧数。默认为1，表示1帧更新一次。设置为2就是2帧更新一次，以此类推。可以动态改变此值。



#### 返回值

[GameTimeMachineTimeTicket](./Ballance2.Services.GameTimeMachine+GameTimeMachineTimeTicket.md) <br/>返回一个更新实例，使用此实例可以取消注册更新函数。


### UnRegisterFixedUpdate(updateAction)

取消注册 FixedUpdate 更新函数


#### 参数


`updateAction` `回调` Action() <br/>更新函数。


