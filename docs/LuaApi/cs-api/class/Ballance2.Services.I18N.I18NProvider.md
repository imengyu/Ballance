# Ballance2.Services.I18N.I18NProvider 
国际化支持类

## 注解

国际化支持类，用于加载并处理国际化字符串资源以及读取本地化字符串。


## 方法



### `静态` ClearAllLanguageResources()

由GameManager调用。不要调用



### `静态` LoadLanguageResources(xmlAssets)

加载语言定义文件


#### 参数


`xmlAssets` string <br/>语言定义XML字符串



#### 返回值

boolean <br/>加载是否成功


### `静态` PreLoadLanguageResources(xmlAssets)

加载语言定义文件


#### 参数


`xmlAssets` string <br/>语言定义XML字符串



#### 返回值

table <br/>加载是否成功


### `静态` LoadLanguageResources(xmlAssets)

加载语言定义文件


#### 参数


`xmlAssets` [TextAsset](https://docs.unity3d.com/ScriptReference/TextAsset.html) <br/>语言定义XML资源文件



#### 返回值

boolean <br/>加载是否成功


### `静态` SetCurrentLanguage(language)

设置当前游戏语言。此函数只能设置语言至设置，无法立即生效，必须重启游戏才能生效。


#### 参数


`language` number [SystemLanguage](https://docs.unity3d.com/ScriptReference/SystemLanguage.html)<br/>语言




### `静态` GetCurrentLanguage()

获取当前游戏语言


#### 返回值

number [SystemLanguage](https://docs.unity3d.com/ScriptReference/SystemLanguage.html)<br/>


### `静态` GetLanguageString(key)

获取语言字符串


#### 参数


`key` string <br/>字符串键值



#### 返回值

string <br/>如果找到对应键值字符串，则返回字符串，否则返回null
