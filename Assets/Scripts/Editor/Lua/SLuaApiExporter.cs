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
using Ballance2.LuaHelpers;

namespace Slua
{
  public static class SLuaApiExporter
  {
    const string path = "Assets/Scripts/LuaHelpers/LuaDefineApi/";
    const string SubUnityPath = "Unity/";
    const string SubCustomPath = "Ballance/";
    /// <summary>
    /// 设置Unity源DLL的路径。如果这个路径不配置，那么生成的lua定义文件将没有注释。
    /// </summary>
    const string unityDocPath = @"E:\Program Files\Unity\Editor\Data\Managed\UnityEngine\";

    private static bool PreCheck() 
    {
      if (EditorApplication.isCompiling) 
      {
        Debug.LogWarning("编辑器正在编译，请等待编译完成再生成");
        return false;
      }

      if (!Directory.Exists(path)) 
      {
        Directory.CreateDirectory(path);
      }
      return true;
    }

    [MenuItem("Ballance/Lua API 定义文件/生成所有", false, 102)]
    public static void GenAll()
    {      
      GenerateForEngine(() => {
        GenCustom();
      });
    }
    [MenuItem("Ballance/Lua API 定义文件/生成 UnityEngine", false, 102)]
    public static void GenUnityEngine()
    {      
      if(!PreCheck())
        return;

      GenerateForEngine(null);
    }
    [MenuItem("Ballance/Lua API 定义文件/生成游戏内核", false, 102)]
    public static void GenCustom()
    {            
      if(!PreCheck())
        return;
      GenNext();
    }
    [MenuItem("Ballance/Lua API 定义文件/生成单个类", false, 103)]
    public static void GenClass() {
      var pathThis = path + SubCustomPath;
      if (EditorApplication.isCompiling) {
        Debug.LogWarning("编辑器正在编译，请等待编译完成再生成");
        return;
      }
      if (!Directory.Exists(pathThis)) Directory.CreateDirectory(pathThis);

      disableXmlComment = true;
      var w = ChooseExportClass.ShowWindow();
      w.Chooseed = (types) => {
        foreach(var t in types)
          GenType(t, true, pathThis);
        w.Close();
        Debug.Log("生成完成");
      };
    }  
    [MenuItem("Ballance/Lua API 定义文件/清空", false, 104)]
    public static void GenClear()
    {
      if (Directory.Exists(path))
        Directory.Delete(path, true);
      EditorUtility.DisplayDialog("完成", "清空完成", "好");
    }
    [MenuItem("Ballance/Lua API 定义文件/清空 UnityEngine", false, 104)]
    public static void GenClearUnityEngine()
    {
      if (Directory.Exists(path + SubUnityPath))
        Directory.Delete(path + SubUnityPath, true);
      EditorUtility.DisplayDialog("完成", "清空完成", "好");
    }
    [MenuItem("Ballance/Lua API 定义文件/清空游戏内核 ", false, 104)]
    public static void GenClearCustom()
    {
      if (Directory.Exists(path + SubCustomPath))
        Directory.Delete(path + SubCustomPath, true);
      EditorUtility.DisplayDialog("完成", "清空完成", "好");
    }
    
    private static bool disableXmlComment = false;
    private static void GenNext()
    {
      GenCustom(path + SubCustomPath);
      EditorUtility.DisplayDialog("完成", "定义文件已经放在 /Assets/Scripts/LuaHelpers/LuaDefineApi/ 下，推荐使用 VS Code IDE 来编辑你的 Lua 代码，并将定义文件的目录添加到你的编辑器设置中。", "好");
    }
    private static void GenerateForEngine(Action finish) {
      DocsByReflection.SetEntendXmlSearchPath(unityDocPath);
#if UNITY_2017_2_OR_NEWER
      ModuleSelector wnd = EditorWindow.GetWindow<ModuleSelector>("ModuleSelector");
      wnd.onExport = (string[] module) =>
      {
        GenerateFor(module, path + SubUnityPath);
        if(finish != null) finish.Invoke();
      };
#else
      GenerateFor("UnityEngine", path + SubUnityPath);
      if(finish != null)  finish.Invoke();
#endif
    }
    private static void GenerateFor(string[] asemblyNames, string path)
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
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
      
      Debug.Log("Generate assembly " + name);
      try
      {
        DocsByReflection.XMLFromAssembly(assembly);
        disableXmlComment = false;
        Debug.Log("Assembly doc " + name + " loaded");
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
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      var assembly = Assembly.Load("Assembly-CSharp-firstpass");
      var types = assembly.GetTypes();
      disableXmlComment = true;
      foreach (Type t in types)
      {
        if (t.IsDefined(typeof(CustomLuaClassAttribute), false))
        {
          GenType(t, true, path);
        }
      }

      assembly = Assembly.Load("Assembly-CSharp");
      types = Assembly.Load("Assembly-CSharp").GetTypes();
      
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
      var sb = new StringBuilder(
        //"---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field" +
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
      if(string.IsNullOrEmpty(t.Namespace)) sb.AppendFormat("{0} = {1}", t.Name, t.Name);
      else sb.AppendFormat("{0}.{1} = {2}", t.Namespace, t.Name, t.Name);

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
      foreach (var field in fields)
      {
        if (field.IsDefined(typeof(DoNotToLuaAttribute), false))
          continue;
        
        comment = GetSummary(field);
        sb.AppendFormat("---@field public {0} {1} {2}\n", ReplaceLuaKeyWord(field.Name), GetLuaType(field.FieldType, out paramOrgType), AppendParamOrgType(paramOrgType, comment));
      }
      PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
      foreach (var pro in properties)
      {
        if (pro.IsDefined(typeof(DoNotToLuaAttribute), false))
          continue;

        comment = GetSummary(pro);
        sb.AppendFormat("---@field public {0} {1} {2}\n", ReplaceLuaKeyWord(pro.Name), GetLuaType(pro.PropertyType, out paramOrgType), AppendParamOrgType(paramOrgType, comment));
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
              else if(node.Name == "param" && node.Attributes["name"] != null)
                methodVarComments.Add(FixString(node.Attributes["name"].InnerText), FixString(node.InnerText));
              else if(node.Name == "returns")
                methodRetComment = FixString(node.InnerText);
            }
          }
        }

        sb.AppendLine("---"+methodComment);
        sb.AppendLine("---@public");

        var paramOrgType = "";
        foreach (var param in method.GetParameters())
        {
          var paramComment = "";
          methodVarComments.TryGetValue(param.Name, out paramComment);
          sb.AppendFormat("---@param {0} {1} {2}\n", ReplaceLuaKeyWord(param.Name), GetLuaType(param.ParameterType, out paramOrgType), 
            AppendParamOrgType(paramOrgType, paramComment));
          if (paramstr.Length != 0)
            paramstr.Append(", ");
          paramstr.Append(ReplaceLuaKeyWord(param.Name));
        }

        if(method.ReturnType != null && method.ReturnType != typeof(void))
          sb.AppendFormat("---@return {0} {1}\n", GetLuaType(method.ReturnType, out paramOrgType), AppendParamOrgType(paramOrgType, methodRetComment));

        if (method.IsStatic)      
          sb.AppendFormat("function {0}.{1}({2}) end\n", ReplaceLuaKeyWord(t.Name), ReplaceLuaKeyWord(method.Name), paramstr);     
        else      
          sb.AppendFormat("function {0}:{1}({2}) end\n", ReplaceLuaKeyWord(t.Name), ReplaceLuaKeyWord(method.Name), paramstr);
      }
    }
    private static string AppendParamOrgType(string paramOrgType, string comment) {
      return comment + (paramOrgType != "" ? (" 原类型 " + paramOrgType + "") : "") ;
    }
    private static string GetLuaType(Type t, out string paramOrgType)
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
          ) {
        paramOrgType = t.Name;
        return "number";
      }
      paramOrgType = "";
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
      var str = luaKeywords.Contains(word) ? ("_"+word) : word;
      if(str.Contains("'")) str = str.Replace('\'', ' ');
      if(str.Contains("&")) str = str.Replace('&', ' ');
      if(str.Contains("`")) str = str.Substring(0, str.IndexOf('`'));
      return str;
    }
  }
}

