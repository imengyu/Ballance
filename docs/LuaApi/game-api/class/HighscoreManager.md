# HighscoreManager

高分管理器，负责管理每个关卡的分数

## 定义

### HighscoreData 高分数据

高分数据是一个数组，数组每一条是一条记录，记录的定义如下

|名称|类型|说明|
|---|---|---|
|name|string|高分记录的用户名|
|score|number|高分分数|
|date|string|创建时间例如2004/8/8|

## 方法

### HighscoreManager.GetData(levelName)

获取指定关卡的分数列表

#### 参数

`levelName` string <br/>关卡名称。可以使用 GetLevelNames 获取关卡名称列表。

#### 返回

HighscoreData <br> 分数数据

### HighscoreManager.GetLevelNames()

获取分数管理器中有存储数据的所有关卡名称

#### 返回

string[] <br> 所有关卡名称数组

### HighscoreManager.AddItem(levelName, userName, score)

在指定关卡添加用户的分数数据

#### 参数

`levelName` string <br/>关卡名称

`userName` string <br/>名字

`score` number <br/>分数

### HighscoreManager.CheckLevelHighScore(levelName, score)

检查指定分数是否在关卡有新的高分

#### 参数

`levelName` string <br/>关卡名称

`score` number <br/>分数

#### 返回

boolean <br> 是否在关卡有新的高分

### HighscoreManager.CheckLevelPassState(levelName)

检查指定关卡是否有过关记录

#### 参数

`levelName` string <br/>关卡名称

#### 返回

boolean <br> 是否有过关记录

### HighscoreManager.TryAddDefaultLevelHighScore(levelName, data)

添加默认分数至指定关卡的分数数据中

#### 参数

`levelName` string <br/>关卡名称

`data` HighscoreData <br/>默认数据，可为 nil，为 nil 时根据在 DefaultHighscoreData 中定义的 `defaultHighscoreData.DefaultHightScoreLev01_11Data` 加载数据
