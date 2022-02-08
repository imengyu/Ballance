using System.Runtime.InteropServices;
using SLua;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* LuaUtils.cs
* 
* 用途：
* Lua 工具类
*
* 作者：
* mengyu
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// Lua 工具类
  /// </summary>
  [CustomLuaClass]
  [LuaApiDescription("Lua 工具类. 部分函数为 C# 设计，在Lua端调用可能会有性能问题。")]
  public class LuaUtils
  {
    /// <summary>
    /// 将参数LuaTable自动转为c#类型
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static object[] LuaTableArrayToObjectArray(object[] param)
    {
      if (param == null)
        return null;
      if (param.Length == 1 && param[0] is LuaTable)
      {
        LuaTable arr = param[0] as LuaTable;
        object[] arrFixed = new object[arr.length()];
        for (int i = 0, c = arrFixed.Length; i < c; i++)
          arrFixed[i] = arr[i + 1];
        return arrFixed;
      }
      else
        return param;
    }
    /// <summary>
    /// 检查参数是不是LuaTable
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static bool CheckParamIsLuaTable(object[] param)
    {
      return param != null && param.Length == 1 && param[0] != null && param[0].GetType() == typeof(LuaTable);
    }
    /// <summary>
    /// 自动检查参数是不是LuaTable并自动转为c#类型
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static object[] AutoCheckParamIsLuaTableAndConver(object[] param)
    {
      if (CheckParamIsLuaTable(param))
        return LuaTableArrayToObjectArray(param);
      return param;
    }

    [DoNotToLua]
    public static LuaTable ToLuaTableForCS(object param)
    {
      return param as LuaTable;
    }
    [DoNotToLua]
    public static LuaFunction ToLuaFunctionForCS(object param)
    {
      return param as LuaFunction;
    }

    /// <summary>
    /// 布尔值转为字符串
    /// </summary>
    /// <param name="param">布尔值</param>
    /// <returns>字符串 "true" 或者 "false"</returns>
    [LuaApiDescription("布尔值转为字符串", "字符串 \"true\" 或者 \"false\"")]
    [LuaApiParamDescription("param", "布尔值")]
    public static string BooleanToString(bool param)
    {
      return param ? "true" : "false";
    }
    [LuaApiDescription("Vector3转为字符串表示")]
    public static string Vector3ToString(Vector3 param) { return param.ToString(); }
    [LuaApiDescription("Vector4转为字符串表示")]
    public static string Vector4ToString(Vector4 param) { return param.ToString(); }
    [LuaApiDescription("Vector2转为字符串表示")]
    public static string Vector2ToString(Vector2 param) { return param.ToString(); }
    [LuaApiDescription("字符串转为布尔值")]
    [LuaApiParamDescription("param", "字符串 \"true\" 或者 \"false\"")]
    public static bool StringToBool(string param)
    {
      return param == "true";
    }
    [LuaApiDescription("字符串转为KeyCode")]
    [LuaApiParamDescription("param", "字符串")]
    public static KeyCode StringToKeyCode(string param)
    {
      return (KeyCode)System.Enum.Parse(typeof(KeyCode), param);
    }

    [LuaApiDescription("Lua按位与函数")]
    public static Color HTMLStringToColor(string htmlColor)
    {
      if (!ColorUtility.TryParseHtmlString(htmlColor, out var color))
        color = Color.black;
      return color;
    }

    [LuaApiDescription("Lua按位与函数")]
    public static int And(int a, int b)
    {
      return a & b;
    }
    [LuaApiDescription("Lua按位或函数")]
    public static int Or(int a, int b)
    {
      return a | b;
    }
    [LuaApiDescription("Lua按位异或函数")]
    public static int Xor(int a, int b)
    {
      return a ^ b;
    }
    [LuaApiDescription("Lua按位非函数")]
    public static int Not(int a)
    {
      return ~a;
    }
    [LuaApiDescription("Lua按左移函数")]
    public static int LeftMove(int a, int b)
    {
      return a << b;
    }
    [LuaApiDescription("Lua按右移函数")]
    public static int RightMove(int a, int b)
    {
      return a >> b;
    }

    /// <summary>
    /// 将 UNITY 相关宏定义导入至lua虚拟机，方便lua端使用
    /// </summary>
    /// <param name="state">要导入的lua虚拟机</param>
    internal static void InitMacros(LuaState state)
    {
#if UNITY_EDITOR
      state["UNITY_EDITOR"] = true;
#endif
#if UNITY_EDITOR_64
      state["UNITY_EDITOR_64"] = true;
#endif
#if UNITY_EDITOR_WIN
      state["UNITY_EDITOR_WIN"] = true;
#endif
#if UNITY_EDITOR_OSX
    state["UNITY_EDITOR_OSX"] = true;
#endif
#if UNITY_STANDALONE_OSX
    state["UNITY_STANDALONE_OSX"] = true;
#endif
#if UNITY_STANDALONE_WIN
      state["UNITY_STANDALONE_WIN"] = true;
#endif
#if UNITY_STANDALONE_LINUX
    state["UNITY_STANDALONE_LINUX"] = true;
#endif
#if UNITY_EDITOR_OSX2
    state["UNITY_EDITOR_OSX"] = true;
#endif
#if UNITY_ANDROID
    state["UNITY_ANDROID"] = true;
#endif
#if UNITY_IOS
    state["UNITY_IOS"] = true;
#endif
#if UNITY_PS4
    state["UNITY_PS4"] = true;
#endif
#if UNITY_XBOXONE
    state["UNITY_XBOXONE"] = true;
#endif
#if UNITY_WSA
    state["UNITY_WSA"] = true;
#endif
#if UNITY_WEBGL
    state["UNITY_WEBGL"] = true;
#endif
    }

    /// <summary>
    /// 获取Lua调用文件名，此函数必须在 [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))] 回调中使用。
    /// </summary>
    /// <param name="l">Lua虚拟机</param>
    internal static string GetLuaCallerFileName(System.IntPtr l)
    {
      var fileName = "";
      var ar = Marshal.AllocHGlobal(Marshal.SizeOf<lua_Debug>());
      if (LuaDLL.lua_getstack(l, 2, ar) == 1)
      { //位置2
        LuaDLL.lua_getinfo(l, "S", ar);
        var luaDebug = (lua_Debug)Marshal.PtrToStructure(ar, typeof(lua_Debug));
        fileName = Marshal.PtrToStringAnsi(luaDebug.source);
      }
      Marshal.FreeHGlobal(ar);
      if(fileName.StartsWith("@"))
        fileName = fileName.Substring(1);
      return fileName.Replace("//", "/");
    }

    [SLua.CustomLuaClass]
    public delegate bool BoolDelegate();

    /// <summary>
    /// 创建 WaitUntil 
    /// </summary>
    /// <param name="f">创建 WaitUntil </param>
    /// <returns></returns>
    [LuaApiDescription("创建 WaitUntil ")]
    [LuaApiParamDescription("f", "回调")]
    public WaitUntil CreateWaitUntil(BoolDelegate f) {
      return new WaitUntil(() => f());
    }
    
    /// <summary>
    /// 创建 WaitWhile 
    /// </summary>
    /// <param name="f">创建 WaitWhile </param>
    /// <returns></returns>
    [LuaApiDescription("创建 WaitWhile ")]
    [LuaApiParamDescription("f", "回调")]
    public WaitWhile CreateWaitWhile(BoolDelegate f) {
      return new WaitWhile(() => f());
    }
  }

}
