生成 LUA 定义代码
---

可以生成C#端的定义代码至lua，这样Lua编辑器就有代码提示了（推荐使用Visual studio code编辑Lua代码）。

1. 先在 Assembly-CSharp.csproj 添加：
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <DocumentationFile>Library\ScriptAssemblies\Assembly-CSharp.xml</DocumentationFile>
</PropertyGroup>
```
2. 在 Assembly-CSharp-firstpass.csproj 添加：
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <DocumentationFile>Library\ScriptAssemblies\Assembly-CSharp-firstpass.xml</DocumentationFile>
</PropertyGroup>
```
3. 在VS 命令行工具中运行以下代码，生成xml注释文档
```shell
devenv "E:\Programming\GameProjects\Ballance\Assembly-CSharp.csproj" /build
```

4. 在 Unity 编辑器点击 SLua/Lua API 定义文件/生成，生成后Lua定义文件就放在
Assets/Scripts/LuaHelpers/LuaDefineApi/下，将它导入你的IDE中，lua就有代码提示啦！
