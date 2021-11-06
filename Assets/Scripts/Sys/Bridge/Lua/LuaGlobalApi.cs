using System;
using System.IO;
using Ballance2.Sys.Package;
using Ballance2.Sys.Res;
using Ballance2.Sys.Services;
using Ballance2.Sys.Utils;
using Ballance2.Utils;
using SLua;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * LuaGlobalApi.cs
 * 
 * 用途：
 * Lua 全局 API
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Sys.Bridge.Lua
{
  [CustomLuaClass]
  public static class LuaGlobalApi
  {
    [CustomLuaClass]
    public delegate object RequireDelegate(string n);
    private static RequireDelegate originalRequire = null;
    private static GamePackageManager pm = null;
    private static string CurrentPath = "";

    internal static void SetRequire(LuaFunction fun)
    {
      originalRequire = fun.cast<RequireDelegate>();
    }

    private static string[] internalLuaLib = { "string","utf8","table","math","os","debug" };
    private static string[] internalLuaFile = { "json","classic","debugger","vscode-debuggee","mobdebug","dkjson" };

    /// <summary>
    /// 从Lua文件路径获取它属于那个模块包
    /// </summary>
    /// <param name="path">Lua文件路径</param>
    /// <returns></returns>
    public static string PackageNameByLuaFilePath(string path) {
      if(CurrentPath == "")
        CurrentPath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/";
      if(PathUtils.IsAbsolutePath(path))
        path = path.Replace(CurrentPath, "");
      if(path.StartsWith("Assets/Game"))
        return GamePackageManager.SYSTEM_PACKAGE_NAME;
      else if(path.StartsWith("Assets/Packages")) {
        var st = path.Substring(16);
        return st.Substring(0, st.IndexOf('/'));
      } else 
        return "";
    }

    /// <summary>
    /// 全局Require的重写。以适应了在模块包中自动Require，或者是Require当前文件目录下相对的文件
    /// </summary>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [StaticExport]
    public static int require(IntPtr l) 
    {
      try {
        string pathOrName;
        LuaObject.checkType(l, 1, out pathOrName);

        if (pm == null)
          pm = GameManager.Instance.GetSystemService<GamePackageManager>();
        if (SecurityUtils.CheckRequire(pathOrName))
          throw new RequireProhibitAccessException(pathOrName);

        object ret = null;

        //处理Lua内置模块
        foreach(var s in internalLuaLib)
          if(s == pathOrName) {
            //普通require
            ret = originalRequire(pathOrName);
            //去除OS模块中的危险函数
            if(pathOrName == "os") 
              SecurityUtils.FixModuleOs((ret as LuaTable));
            break;
          }
        //处理一些游戏内置模块
        foreach(var s in internalLuaFile)
          if(s == pathOrName) {
            ret = GameSystemPackage.GetSystemPackage().RequireLuaFile(pathOrName + ".lua");
            break;
          }
        
        if(ret == null) {
          //处理以包名 __xxx__/ 为开头的字符串
          var lastIdx = pathOrName.IndexOf("__/");
          if (pathOrName.StartsWith("__") && lastIdx >= 0)
          {
            var packname = pathOrName.Substring(2, lastIdx - 2);
            var pack = pm.FindPackage(packname);
            if (pack == null)
              throw new RequireFailedException("Package " + packname + " not found");
            ret = pack.RequireLuaFile(pathOrName.Substring(lastIdx + 3));
          } else {
            //尝试获取当前lua栈帧所在文件
            var fileName = LuaUtils.GetLuaCallerFileName(l);
            var packName = PackageNameByLuaFilePath(fileName);

            var pack = pm.FindPackage(packName);
            if (pack != null) {
              //有斜杠
              if (pathOrName.Contains("/")) {
                if(pathOrName.StartsWith("/"))
                  ret = pack.RequireLuaFile(pathOrName);//绝对路径加载
                else
                  ret = pack.RequireLuaFile(PathUtils.JoinTwoPath( Path.GetDirectoryName(fileName).Replace("\\","/"), pathOrName));//相对路径加载
              } else
                //无斜杠，从当前目录加载文件
                ret = pack.RequireLuaFile(Path.GetDirectoryName(fileName).Replace("\\","/") + "/" + pathOrName);
            }
          }
        }

        LuaObject.pushValue(l,true);
        LuaObject.pushValue(l,ret);
        return 2;
      }
      catch(Exception e) {
        return LuaObject.error(l,e);
      }
    }

    /// <summary>
    /// 全局加载资源重写
    /// </summary>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [StaticExport]
    public static int loadAsset(IntPtr l) {
      try {
        string pathOrName;
        LuaObject.checkType(l, 1, out pathOrName);

        if (pm == null)
          pm = GameManager.Instance.GetSystemService<GamePackageManager>();
        
        object ret = null;
        //处理以包名 __xxx__/ 为开头的字符串
        var lastIdx = pathOrName.IndexOf("__/");
        if (pathOrName.StartsWith("__") && lastIdx >= 0)
        {
          var packname = pathOrName.Substring(2, lastIdx - 2);
          if(packname == "internal")
            ret = Resources.Load(pathOrName);
          else if(packname == "static.asset")
            ret = GameStaticResourcesPool.FindStaticPrefabs(pathOrName);
          else if(packname == "static.prefab")
            ret = GameStaticResourcesPool.FindStaticAssets<UnityEngine.Object>(pathOrName);
          else {
            var pack = pm.FindPackage(packname);
            if (pack == null)
              throw new RequireFailedException("Package " + packname + " not found");
            ret = pack.GetAsset<UnityEngine.Object>(pathOrName.Substring(lastIdx + 3));
          }
        } else {
          //尝试获取当前lua栈帧所在文件
          var fileName = LuaUtils.GetLuaCallerFileName(l);
          var packName = PackageNameByLuaFilePath(fileName);

          var pack = pm.FindPackage(packName);
          if (pack != null) {
            //有斜杠
            if (pathOrName.Contains("/")) {
              if(pathOrName.StartsWith("/"))
                ret = pack.GetAsset<UnityEngine.Object>(pathOrName);//绝对路径加载
              else
                ret = pack.GetAsset<UnityEngine.Object>(PathUtils.JoinTwoPath( Path.GetDirectoryName(fileName).Replace("\\","/"), pathOrName));//相对路径加载
            } else
              //无斜杠，从当前目录加载资源
              ret = pack.GetAsset<UnityEngine.Object>(Path.GetDirectoryName(fileName).Replace("\\","/") + "/" + pathOrName);
          }
        }
        
        
        LuaObject.pushValue(l,true);
        LuaObject.pushValue(l,ret);
        return 2;
      }
      catch(Exception e) {
        return LuaObject.error(l,e);
      }
    }
  
  }

  /// <summary>
  /// 不允许 Require 异常
  /// </summary>
  public class RequireProhibitAccessException : System.Exception
  {
    public RequireProhibitAccessException(string pathOrName) : base("Require " + pathOrName + " are not allowed") { }
  }
  /// <summary>
  /// Require 失败异常
  /// </summary>
  public class RequireFailedException : System.Exception
  {
    public RequireFailedException(string msg) : base(msg) { }
  }
  /// <summary>
  /// 尝试 Require 文件为空异常
  /// </summary>
  public class EmptyFileException : System.Exception
  {
    public EmptyFileException(string msg) : base(msg) { }
  }

}