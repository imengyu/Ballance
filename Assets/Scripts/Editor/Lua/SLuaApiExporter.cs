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
  public static class SLuaApiExporter
  {
    const string path = "Assets/Scripts/LuaHelpers/LuaDefineApi/";

    [MenuItem("SLua/Lua API 定义文件/生成", false)]
    static void Gen()
    {      
      if (EditorApplication.isCompiling) {
        Debug.LogWarning("编辑器正在编译，请等待编译完成再生成");
        return;
      }

      if (Directory.Exists(path))
        Directory.Delete(path, true);

      Directory.CreateDirectory(path);

#if UNITY_2017_2_OR_NEWER
      ModuleSelector wnd = EditorWindow.GetWindow<ModuleSelector>("ModuleSelector");
      wnd.onExport = (string[] module) =>
      {
        GenerateFor(module, path);
        GenNext(path);
      };
#else
      GenerateFor("UnityEngine", path);
      GenNext(path);
#endif
    }
    [MenuItem("SLua/Lua API 定义文件/清空 ", false)]
    private static void GenClear()
    {
      if (Directory.Exists(path))
        Directory.Delete(path, true);
      EditorUtility.DisplayDialog("完成", "清空完成", "好");
    }
    
    private static bool disableXmlComment = false;
    private static void GenNext(string path)
    {
      GenCustom(path);
      EditorUtility.DisplayDialog("完成", "定义文件已经放在 /Assets/Scripts/LuaHelpers/LuaDefineApi/ 下，推荐使用 VS Code IDE 来编辑你的 Lua 代码，并将定义文件的目录添加到你的编辑器设置中。", "好");
    }
    private static void GenerateFor(string[] asemblyNames, string path)
    {

      foreach (string name in asemblyNames) {
        string dir = path + name + "/";
        if(!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        GenAssembly(name, dir);
      }
    }
    
    private static void GenAssembly(string name, string path)
    {
      List<string> excludeList;
      List<string> includeList;
      CustomExport.OnGetNoUseList(out excludeList);
      CustomExport.OnGetUseList(out includeList);
      var assembly = Assembly.Load(name);
      var types = assembly.GetTypes();
      
      try
      {
        DocsByReflection.XMLFromAssembly(assembly);
        disableXmlComment = false;
      } 
      catch(Exception e)
      {
        disableXmlComment = true;
        Debug.LogWarning("无法获取 " + name + " 的XML注释文档，生成的Lua定义文件将不包含注释" + e.ToString());
      }

      foreach (Type t in types)
      {
        if (LuaCodeGen.filterType(t, excludeList, includeList))
        {
          GenType(t, false, path);
        }
      }
    }
    private static void GenCustom(string path)
    {
      var assembly = Assembly.Load("Assembly-CSharp-firstpass");
      var types = assembly.GetTypes();
      try
      {
        DocsByReflection.XMLFromAssembly(assembly);
        disableXmlComment = false;
      } 
      catch(Exception e)
      {
        disableXmlComment = true;
        Debug.LogWarning("无法获取 Assembly-CSharp-firstpass 的XML注释文档，生成的Lua定义文件将不包含注释" + e.ToString());
      }
      foreach (Type t in types)
      {
        if (t.IsDefined(typeof(CustomLuaClassAttribute), false))
        {
          GenType(t, true, path);
        }
      }

      assembly = Assembly.Load("Assembly-CSharp");
      types = Assembly.Load("Assembly-CSharp").GetTypes();
      try
      {
        DocsByReflection.XMLFromAssembly(assembly);
        disableXmlComment = false;
      } 
      catch(Exception e)
      {
        disableXmlComment = true;
        Debug.LogWarning("无法获取 Assembly-CSharp 的XML注释文档，生成的Lua定义文件将不包含注释" + e.ToString());
      }
      foreach (Type t in types)
      {
        if (t.IsDefined(typeof(CustomLuaClassAttribute), false))
        {
          GenType(t, true, path);
        }
      }
    }
    
    private static void GenType(Type t, bool custom, string path)
    {
      if (!CheckType(t, custom))
        return;
      //TODO System.MulticastDelegate
      var sb = new StringBuilder(
        "---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field" +
        "\n"
      );
      if (!CheckType(t.BaseType, custom))
        sb.AppendFormat("---@class {0}\n", t.Name);
      else
        sb.AppendFormat("---@class {0} : {1}\n", t.Name, t.BaseType.Name);

      GenTypeField(t, sb);
      sb.AppendFormat("local {0}={{ }}\n", t.Name);
      GenTypeMethod(t, sb);
      sb.Append("---" + GetSummaryByType(t) + "\n");
      sb.AppendFormat("{0}.{1} = {2}", t.Namespace, t.Name, t.Name);

      File.WriteAllText(path + t.FullName + ".lua", sb.ToString(), Encoding.UTF8);
    }
    private static bool CheckType(Type t, bool custom)
    {
      if (t == null)
        return false;
      if (t == typeof(System.Object))
        return false;
      if (t.IsGenericTypeDefinition)
        return false;
      if (t.IsDefined(typeof(ObsoleteAttribute), false))
        return false;
      if (t == typeof(YieldInstruction))
        return false;
      if (t == typeof(Coroutine))
        return false;
      if (t.IsNested)
        return false;
      if (custom && !t.IsDefined(typeof(CustomLuaClassAttribute), false))
        return false;
      return true;
    }
    private static string GetSummary(MemberInfo info) {
      try {
        if(!disableXmlComment) {
          XmlElement documentation = DocsByReflection.XMLFromMember(info);
          if(documentation != null && documentation["summary"] != null)
            return FixString(documentation["summary"].InnerText);
        }
      } catch {
      }
      return "";
    }
    private static string GetSummaryByType(Type t) {
      try {
        if(!disableXmlComment) {
          XmlElement documentation = DocsByReflection.XMLFromType(t);
          if(documentation != null && documentation["summary"] != null)
            return FixString(documentation["summary"].InnerText);
        }
      } catch {
      }
      return "";
    }
    private static string FixString(string s) {
      return s.Trim().Replace("\n", "").Replace("\r", "");
    }

    private static void GenTypeField(Type t, StringBuilder sb)
    {
      FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      string comment = "";
      foreach (var field in fields)
      {
        if (field.IsDefined(typeof(DoNotToLuaAttribute), false))
          continue;
        
        comment = GetSummary(field);
        sb.AppendFormat("---@field public {0} {1} {2}\n", ReplaceLuaKeyWord(field.Name), GetLuaType(field.FieldType), comment);
      }
      PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      foreach (var pro in properties)
      {
        if (pro.IsDefined(typeof(DoNotToLuaAttribute), false))
          continue;

        comment = GetSummary(pro);
        sb.AppendFormat("---@field public {0} {1} {2}\n", ReplaceLuaKeyWord(pro.Name), GetLuaType(pro.PropertyType), comment);
      }
    }
    private static void GenTypeMethod(Type t, StringBuilder sb)
    {
      var methodComment = "";
      var methodVarComments = new Dictionary<string, string>();
      var methodRetComment = "";

      MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      foreach (var method in methods)
      {
        if (method.IsGenericMethod)
          continue;
        if (method.IsDefined(typeof(DoNotToLuaAttribute), false))
          continue;
        if (method.Name.StartsWith("get_") || method.Name.StartsWith("set_"))
          continue;

        var paramstr = new StringBuilder();

        methodVarComments.Clear();
        methodComment = "";
        methodRetComment = "";

        if(!disableXmlComment) { 
          try{
            XmlElement documentation = DocsByReflection.XMLFromMember(method);
            if(documentation != null) {
              for(int i = 0; i < documentation.ChildNodes.Count; i++) {
                XmlNode node = documentation.ChildNodes[i];
                if(node.Name == "summary")
                  methodComment = FixString(node.InnerText.Trim());
                else if(node.Name == "param" && node.Attributes["name"] != null)
                  methodVarComments.Add(FixString(node.Attributes["name"].InnerText), FixString(node.InnerText));
                else if(node.Name == "returns")
                  methodRetComment = FixString(node.InnerText);
              }
            }
          } catch{}
        }

        sb.AppendLine("---"+methodComment);
        sb.AppendLine("---@public");

        foreach (var param in method.GetParameters())
        {
          var paramComment = "";
          methodVarComments.TryGetValue(param.Name, out paramComment);
          sb.AppendFormat("---@param {0} {1} {2}\n", ReplaceLuaKeyWord(param.Name), GetLuaType(param.ParameterType), paramComment);
          if (paramstr.Length != 0)
            paramstr.Append(", ");
          paramstr.Append(ReplaceLuaKeyWord(param.Name));
        }

        if(method.ReturnType != null)
          sb.AppendFormat("---@return {0} {1}\n", GetLuaType(method.ReturnType), methodRetComment);

        if (method.IsStatic)      
          sb.AppendFormat("function {0}.{1}({2}) end\n", ReplaceLuaKeyWord(t.Name), ReplaceLuaKeyWord(method.Name), paramstr);     
        else      
          sb.AppendFormat("function {0}:{1}({2}) end\n", ReplaceLuaKeyWord(t.Name), ReplaceLuaKeyWord(method.Name), paramstr);
      }
    }

    private static string GetLuaType(Type t)
    {
      if (t.IsEnum
          || t == typeof(ulong)
          || t == typeof(long)
          || t == typeof(int)
          || t == typeof(uint)
          || t == typeof(float)
          || t == typeof(double)
          || t == typeof(byte)
          || t == typeof(ushort)
          || t == typeof(short)
          )
        return "number";
      if (t == typeof(bool))
        return "boolean";
      if (t == typeof(string))
        return "string";
      if (t == typeof(void))
        return "void";

      return t.Name;
    }
  
    private static List<string> luaKeywords = new List<string>() {
        "and","break","do","else","elseif","end",
        "false","for","function","goto","if","in",
        "local","nil","not","or","repeat","return",
        "then","true","until","while"
      };
    private static string ReplaceLuaKeyWord(string word) {
      return luaKeywords.Contains(word) ? ("_"+word) : word;
    }
  }
}

