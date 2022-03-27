using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using SLua;
using System.IO;
using UnityEngine;
using JimBlackler.DocsByReflection;
using System.Xml;

namespace Slua
{
  public static class GameDocApiExporter
  {
    const string BackPath = "/LuaApi/cs-api/readme.md";
    const string HomePath = "/LuaApi/readme.md";
    const string OutPath = "docs/LuaApi/cs-api/class";
    const string SidePath = "docs/LuaApi/cs-api/_sidebar.md";

    private static bool PreCheck() 
    {
      if (EditorApplication.isCompiling) 
      {
        Debug.LogWarning("编辑器正在编译，请等待编译完成再生成");
        return false;
      }
      if (!Directory.Exists(OutPath)) 
        Directory.CreateDirectory(OutPath);
      return true;
    }

    [MenuItem("Ballance/Lua API 文档/生成框架", false, 102)]
    public static void GenAll()
    {      
      if(!PreCheck())
        return;
      GenNext();
    }
    [MenuItem("Ballance/Lua API 文档/清空框架", false, 104)]
    public static void GenClear()
    {
      if (Directory.Exists(OutPath))
        Directory.Delete(OutPath, true);
      EditorUtility.DisplayDialog("完成", "清空完成", "好");
    }
    
    private static bool disableXmlComment = false;
    private static void GenNext()
    {
      GenCustom(OutPath);
      EditorUtility.DisplayDialog("完成", "生成完成", "好");
    }  
    private static void GenCustom(string path)
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      var sbSide = new StringBuilder("* [返回主页]" + HomePath + ")\n* [简介](" + BackPath + ")\n");

      var assembly = Assembly.Load("Assembly-CSharp-firstpass");
      var types = assembly.GetTypes();
      var sidePath = "";
      disableXmlComment = true;
      foreach (Type t in types)
      {
        if (t.IsDefined(typeof(CustomLuaClassAttribute), false))
        {
          GenType(t, true, path, out sidePath);
          if(sidePath != "")
            sbSide.AppendFormat("* [{0}](/LuaApi/cs-api/class/{1})\n", t.FullName, sidePath);
        }
      }

      assembly = Assembly.Load("Assembly-CSharp");
      types = Assembly.Load("Assembly-CSharp").GetTypes();
      
      foreach (Type t in types)
      {
        if (t.IsDefined(typeof(CustomLuaClassAttribute), false))
        {
          GenType(t, true, path, out sidePath);
          if(sidePath != "")
            sbSide.AppendFormat("* [{0}](/LuaApi/cs-api/class/{1})\n", t.FullName, sidePath);
        }
      }


      try {
        File.WriteAllText(SidePath, sbSide.ToString(), Encoding.UTF8);
      } catch(System.Exception e) {
        Debug.LogError("Failed to save sidebar : " + e.ToString());
      }
    }
    
    private static void GenType(Type t, bool custom, string path, out string sidePath)
    {
      if (!CheckType(t, custom)) {
        sidePath = "";
        return;
      }
      var sb = new StringBuilder();
      sb.AppendFormat("# {0} {1}\n", t.FullName, t.IsEnum ? "`枚举`" : "");
      sb.AppendLine(GetSummaryByType(t));
      sb.AppendLine("");

      if(t.IsDefined(typeof(LuaApiNotes))) {
        var attr = t.GetCustomAttribute<LuaApiNotes>();
        if(!string.IsNullOrEmpty(attr.AddAtString)) {
          sb.AppendLine("");
          sb.AppendLine("> " + attr.AddAtString);
        }
      }

      /* sb.AppendLine("## 定义");
      sb.AppendLine("");
      sb.AppendLine("```csharp");
      if (!CheckType(t.BaseType, custom))
        sb.AppendFormat("public class {0}\n", t.Name);
      else
        sb.AppendFormat("public class {0} : {1}\n", t.Name, t.BaseType.Name);
      sb.AppendLine("```");
      sb.AppendLine(""); */

      if(t.IsDefined(typeof(LuaApiNotes))) {
        var attr = t.GetCustomAttribute<LuaApiNotes>();
        if(!string.IsNullOrEmpty(attr.NoteString)) {
          sb.AppendLine("## 注解");
          sb.AppendLine("");
          sb.AppendLine(attr.NoteString);
          sb.AppendLine("");
        }
        if(!string.IsNullOrEmpty(attr.DemoString)) {
          sb.AppendLine("## 示例");
          sb.AppendLine("");
          sb.AppendLine(attr.DemoString);
          sb.AppendLine("");
        }
      }

      GenTypeField(t, sb);
      GenTypeMethod(t, sb);

      try {
        File.WriteAllText(path + "/" + t.FullName + ".md", sb.ToString(), Encoding.UTF8);
        sidePath = t.FullName + ".md";
      } catch(System.Exception e) {
        sidePath = "";
        Debug.LogError("Failed to save \"" + t.FullName + "\" : " + e.ToString());
      }
    }
    private static bool CheckType(Type t, bool custom)
    {
      if (t == null)
        return false;
      if (!t.FullName.StartsWith("Ballance"))
        return false;
      if (t == typeof(System.Object))
        return false;
      if (t.IsSubclassOf(typeof(Delegate)))
        return false;
      if (t.IsGenericTypeDefinition)
        return false; 
      if (t.IsDefined(typeof(ObsoleteAttribute), false))
        return false;
      if (t == typeof(YieldInstruction))
        return false;
      if (t == typeof(Coroutine))
        return false;
      if (t.IsDefined(typeof(LuaApiNoDoc), false))
        return false;
      if (t.IsNested) {
        if(!t.IsPublic && !t.IsDefined(typeof(CustomLuaClassAttribute), false))
          return false;
      }
      return true;
    }
    private static string GetSummary(MemberInfo info) {
      if(info.IsDefined(typeof(LuaApiDescription))) {
        var attr = info.GetCustomAttribute<LuaApiDescription>();
        return attr.DescriptionString;
      } else if(!disableXmlComment) {
        XmlElement documentation = DocsByReflection.XMLFromMember(info);
        if(documentation != null && documentation["summary"] != null)
          return FixString(documentation["summary"].InnerText);
      }
      return "";
    }
    private static string GetSummaryByType(Type t) {
      if(t.IsDefined(typeof(LuaApiDescription))) {
        var attr = t.GetCustomAttribute<LuaApiDescription>();
        return attr.DescriptionString;
      } else if(!disableXmlComment) {
        XmlElement documentation = DocsByReflection.XMLFromType(t);
        if(documentation != null && documentation["summary"] != null)
          return FixString(documentation["summary"].InnerText);
      }
      return "";
    }
    private static string FixString(string s) {
      return s.Trim().Replace("\n", "").Replace("\r", "");
    }

    private static void GenTypeField(Type t, StringBuilder sb)
    {
      var paramOrgType = "";
      FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      string comment = "";

      if(fields.Length> 0) {
        if(t.IsEnum) {
          sb.AppendLine();
          foreach (var field in fields)
          {
            if (field.IsDefined(typeof(DoNotToLuaAttribute), false) || field.IsDefined(typeof(LuaApiNoDoc), false) || field.Name == "value__")
              continue;
            
            comment = GetSummary(field);
            sb.AppendFormat("* **{0}** {1}\n", ReplaceLuaKeyWord(field.Name), comment);
          }
        } else {
          sb.AppendLine("## 字段");
          sb.AppendLine();
          sb.AppendLine("|名称|类型|说明|");
          sb.AppendLine("|---|---|---|");
          foreach (var field in fields)
          {
            if (field.IsDefined(typeof(DoNotToLuaAttribute), false) || field.IsDefined(typeof(LuaApiNoDoc), false) || field.Name == "value__")
              continue;
            
            comment = GetSummary(field);

            sb.AppendFormat("|{0}|{1} {2}|{3}|\n", ReplaceLuaKeyWord(field.Name), GetLuaType(field.FieldType, out paramOrgType), paramOrgType, comment);
          }
        }
      }
      
      PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      if(properties.Length> 0) {
        sb.AppendLine("## 属性");
        sb.AppendLine();
        sb.AppendLine("|名称|类型|说明|");
        sb.AppendLine("|---|---|---|");
        foreach (var pro in properties)
        {
          if (pro.IsDefined(typeof(DoNotToLuaAttribute), false))
            continue;
          if (pro.IsDefined(typeof(LuaApiNoDoc), false))
            continue;

          comment = GetSummary(pro);
          sb.AppendFormat("|{0}|{1} {2}|{3}|\n", ReplaceLuaKeyWord(pro.Name), GetLuaType(pro.PropertyType, out paramOrgType), paramOrgType, comment);
        }
      }
    }
    private static void GenTypeMethod(Type t, StringBuilder sb)
    {
      var methodComment = "";
      var methodVarComments = new Dictionary<string, string>();
      var methodRetComment = "";

      MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

      if(methods.Length > 0)
      {
        sb.AppendLine("");
        sb.AppendLine("## 方法");
        sb.AppendLine("");
      }

      foreach (var method in methods)
      {
        if (method.IsGenericMethod)
          continue;
        if (method.IsDefined(typeof(DoNotToLuaAttribute), false))
          continue;
        if (method.IsDefined(typeof(LuaApiNoDoc), false))
          continue;
        if (method.Name.StartsWith("get_") || method.Name.StartsWith("set_"))
          continue;

        methodVarComments.Clear();
        methodComment = "";
        methodRetComment = "";

        if(method.IsDefined(typeof(LuaApiDescription))) {
          var attr = method.GetCustomAttribute<LuaApiDescription>();
          var paramDesps = method.GetCustomAttributes<LuaApiParamDescription>();
          methodComment = attr.DescriptionString;
          methodRetComment = attr.ReturnString;
          foreach(var dep in paramDesps)
            if(!methodVarComments.ContainsKey(dep.ParamName))
              methodVarComments.Add(dep.ParamName, dep.DescriptionString);
        } else if(!disableXmlComment) { 
          XmlElement documentation = DocsByReflection.XMLFromMember(method);
          if(documentation != null) {
            for(int i = 0; i < documentation.ChildNodes.Count; i++) {
              XmlNode node = documentation.ChildNodes[i];
              if(node.Name == "summary")
                methodComment = FixString(node.InnerText.Trim());
              else if(node.Name == "param" && node.Attributes["name"] != null) {
                string key = FixString(node.Attributes["name"].InnerText);
                if(methodVarComments.ContainsKey(key)) methodVarComments[key] = FixString(node.InnerText);
                else methodVarComments.Add(key, FixString(node.InnerText));
              } else if(node.Name == "returns")
                methodRetComment = FixString(node.InnerText);
            }
          }
        }

        sb.AppendLine();
        sb.AppendLine();
        sb.Append("### ");
        if(method.IsStatic)
          sb.Append("`静态` ");
        if(method.IsDefined(typeof(ObsoleteAttribute)))
          sb.Append("`已弃用` ");
        sb.Append(method.Name);

        var paramstr = new StringBuilder();
        var paramsTable = new StringBuilder("\n");
        var paramOrgType = "";
        var parameters = method.GetParameters();
        foreach (var param in parameters)
        {
          var paramComment = "";

          methodVarComments.TryGetValue(param.Name, out paramComment);
          paramsTable.AppendFormat("`{0}` {1} {2}<br/>{3}\n\n", 
            ReplaceLuaKeyWord(param.Name), 
            GetLuaType(param.ParameterType, out paramOrgType),
            paramOrgType,
            paramComment);
          
          if (paramstr.Length != 0)
            paramstr.Append(", ");
          paramstr.Append(ReplaceLuaKeyWord(param.Name));
        }

        sb.Append("(");
        sb.Append(paramstr.ToString());
        sb.Append(")");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(methodComment);
        sb.AppendLine();


        if(method.IsDefined(typeof(ObsoleteAttribute))) {
          var attrs = method.GetCustomAttribute<ObsoleteAttribute>();
          sb.AppendLine();
          sb.AppendLine("#### 已弃用");
          sb.AppendLine(attrs.Message);
        }

        if(parameters.Length > 0) {
          sb.AppendLine();
          sb.AppendLine("#### 参数");
          sb.AppendLine();
          sb.AppendLine(paramsTable.ToString());
        }
        if(method.ReturnType != typeof(void)) {
          sb.AppendLine();
          sb.AppendLine("#### 返回值");
          sb.AppendLine();
          sb.AppendFormat("{0} {1}<br/>{2}\n", GetLuaType(method.ReturnType, out paramOrgType), paramOrgType, methodRetComment);
        }
        if(method.IsDefined(typeof(LuaApiException))) {
          var attrs = method.GetCustomAttributes<LuaApiException>();
          foreach(var attr in attrs) {
            sb.AppendLine();
            sb.AppendLine(attr.NameString);
            sb.AppendLine();
            sb.AppendLine(attr.ExplanString);
          }
        }
        if(method.IsDefined(typeof(LuaApiNotes))) {
          var attr = method.GetCustomAttribute<LuaApiNotes>();
          if(!string.IsNullOrEmpty(attr.NoteString)) {
            sb.AppendLine();
            sb.AppendLine("#### 注解");
            sb.AppendLine();
            sb.AppendLine(attr.NoteString);
          }
          if(!string.IsNullOrEmpty(attr.DemoString)) {
            sb.AppendLine();
            sb.AppendLine("#### 示例");
            sb.AppendLine();
            sb.AppendLine(attr.DemoString);
          }
        }
      }
    }
    private static string GetCsharpTypeLink(Type t, string forceName = "") {
      if(forceName == "")
        forceName = t.Name;
      if(t.IsSubclassOf(typeof(Delegate))) {
        MethodInfo method = t.GetMethod("Invoke");
        var paramstr = new StringBuilder("`回调` ");
        paramstr.Append(ReplaceLuaKeyWord(t.Name));
        paramstr.Append("(");
        var count = 0;
        var parameters = method.GetParameters();
        foreach (var param in parameters)
        {
          if (count != 0)
            paramstr.Append(", ");
          paramstr.Append(ReplaceLuaKeyWord(param.Name));
          paramstr.Append(": ");
          paramstr.Append(GetCsharpTypeLink(param.ParameterType));
          count++;
        }
        paramstr.Append(")");
        if(method.ReturnType != typeof(void)) {
          paramstr.Append(" -> ");
          paramstr.Append(GetCsharpTypeLink(method.ReturnType));
        }
        return paramstr.ToString();
      }
      else if(t.FullName.StartsWith("System.")) 
        return "["+forceName+"](https://docs.microsoft.com/zh-cn/dotnet/api/" + t.FullName + ")";
      else if(t.FullName.StartsWith("UnityEngine")) 
        return "["+forceName+"](https://docs.unity3d.com/ScriptReference/" + t.FullName.Replace("UnityEngine.", "") + ".html)";
      else if(t.FullName.StartsWith("Ballance")) 
        return "["+forceName+"](./" + t.FullName + ".md)";
      else
        return "`" + t.FullName + "`";
    }
    private static string GetLuaType(Type t, out string paramOrgType)
    {
      if(t.IsGenericType) {
        paramOrgType = "";
        return "table";
      }
      if (t.IsEnum) {
        paramOrgType = GetCsharpTypeLink(t);
        return "number";
      }
      paramOrgType = "";
      if (t == typeof(ulong)) {
        paramOrgType = "[ulong](../types.md)";
        return "number";
      }
      if (t == typeof(long)) {
        paramOrgType = "[long](../types.md)";
        return "number";
      }
      if (t == typeof(int)) {
        paramOrgType = "[int](../types.md)";
        return "number";
      }
      if (t == typeof(uint)) {
        paramOrgType = "[uint](../types.md)";
        return "number";
      }
      if (t == typeof(float)) {
        paramOrgType = "[float](../types.md)";
        return "number";
      }
      if (t == typeof(double)) {
        paramOrgType = "[double](../types.md)";
        return "number";
      }
      if (t == typeof(byte)) {
        paramOrgType = "[byte](../types.md)";
        return "number";
      }
      if (t == typeof(ushort)) {
        paramOrgType = "[ushort](../types.md)";
        return "number";
      }
      if (t == typeof(short)) {
        paramOrgType = "[short](../types.md)";
        return "number";
      }
      if (t == typeof(bool))
        return "boolean";
      if (t == typeof(string))
        return "string";
      if (t == typeof(void))
        return "void";

      return GetCsharpTypeLink(t);
    }
  
    private static List<string> luaKeywords = new List<string>() {
        "and","break","do","else","elseif","end",
        "false","for","function","goto","if","in",
        "local","nil","not","or","repeat","return",
        "then","true","until","while"
      };
    private static string ReplaceLuaKeyWord(string word) {
      var str = luaKeywords.Contains(word) ? ("_"+word) : word;
      if(str.Contains("'")) str = str.Replace('\'', ' ');
      if(str.Contains("&")) str = str.Replace('&', ' ');
      if(str.Contains("`")) str = str.Substring(0, str.IndexOf('`'));
      return str;
    }
  }
}

