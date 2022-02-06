using Ballance2.Config;
using Ballance2.Config.Settings;
using System;
using Ballance2.Utils;
using Ballance2.Services.Debug;
using UnityEngine;
using System.IO;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameInit.cs
* 
* 用途：
* 游戏外部资源文件的路径配置与路径转换工具类。
*
* 作者：
* mengyu
*/

namespace Ballance2.Res
{
  /// <summary>
  /// 路径管理器
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("路径管理器")]
  public static class GamePathManager
  {
    /// <summary>
    /// 调试模组包存放路径
    /// </summary>
    [LuaApiDescription("调试模组包存放路径")]
    public const string DEBUG_PACKAGE_FOLDER = "Assets/Packages";
    /// <summary>
    /// 调试关卡存放路径
    /// </summary>
    [LuaApiDescription("调试关卡存放路径")]
    public const string DEBUG_LEVEL_FOLDER = "Assets/Levels";

    private static string _DEBUG_PATH = "";
    private static string _DEBUG_OUTPUT_PATH = "";
 
    private static string FixMaker(string src) {
#if UNITY_EDITOR_WIN
      if(Directory.Exists(src + "/StandaloneWindows64/"))
        src += "/StandaloneWindows64/";
      else
        src += "/StandaloneWindows/";
#elif UNITY_EDITOR_LINUX
      if(Directory.Exists(src + "/StandaloneLinux64/"))
        src += "/StandaloneLinux64/";
      else
        src += "/StandaloneLinux/";
#elif UNITY_EDITOR_OSX
      src += "/StandaloneOSX/";
#endif
      return src;
    }

    /// <summary>
    /// 调试路径（输出目录）<c>（您在调试时请点击菜单 "Ballance">"开发设置">"Debug Settings" 将其更改为自己调试输出存放目录）</c>
    /// </summary>
    [LuaApiDescription("调试路径（输出目录）")]
    public static string DEBUG_PATH
    {
      get
      {
        if(_DEBUG_PATH == "") {
          DebugSettings debugSettings = DebugSettings.Instance;
          if (debugSettings != null) {
            var realtivePath = debugSettings.DebugFolder.Replace("\\", "/");

            if(PathUtils.IsAbsolutePath(realtivePath))
              _DEBUG_PATH = realtivePath;
            else
              _DEBUG_PATH = PathUtils.JoinTwoPath(Directory.GetCurrentDirectory(), realtivePath);

            _DEBUG_PATH = FixMaker(_DEBUG_PATH);
          }
        }
        return _DEBUG_PATH;
      }
    }
    /// <summary>
    /// 输出目录<c>（您在调试时请点击菜单 "Ballance">"开发设置">"Debug Settings" 将其更改为自己调试输出存放目录）</c>
    /// </summary>
    [LuaApiDescription("输出目录（您在调试时请点击菜单 \"Ballance\">\"开发设置\">\"Debug Settings\" 将其更改为自己调试输出存放目录）")]
    public static string DEBUG_OUTPUT_PATH
    {
      get
      {
        if(_DEBUG_OUTPUT_PATH == "") {
          DebugSettings debugSettings = DebugSettings.Instance;
          if (debugSettings != null) {
            var realtivePath = debugSettings.OutputFolder.Replace("\\", "/");
            if(PathUtils.IsAbsolutePath(realtivePath))
              _DEBUG_OUTPUT_PATH = realtivePath;
            else
              _DEBUG_OUTPUT_PATH = PathUtils.JoinTwoPath(Directory.GetCurrentDirectory(), realtivePath);

            _DEBUG_OUTPUT_PATH = FixMaker(_DEBUG_OUTPUT_PATH);
          }
        }
        return _DEBUG_OUTPUT_PATH;
      }
    }
    /// <summary>
    /// 调试路径（模组目录）
    /// </summary>
    [LuaApiDescription("调试路径（模组目录）")]
    public static string DEBUG_PACKAGES_PATH { get { return DEBUG_PATH + "/Packages/"; } }
    /// <summary>
    /// 调试路径（关卡目录）
    /// </summary>
    [LuaApiDescription("调试路径（关卡目录）")]
    public static string DEBUG_LEVELS_PATH { get { return DEBUG_PATH + "/Levels/"; } }

    /// <summary>
    /// 内置模块的文件名
    /// </summary>
    private static string[] builtInPackagesFiles = new string[] {
      "core.assets.skys.ballance",
      "core.sounds.ballance",
      "core.sounds.music.ballance",
    };
    /// <summary>
    /// 内置关卡的文件名
    /// </summary>
    private static string[] builtInLevelsFiles = new string[] {
      "level01.ballance",
      "level02.ballance",
      "level03.ballance",
      "level04.ballance",
      "level05.ballance",
      "level06.ballance",
      "level07.ballance",
      "level08.ballance",
      "level09.ballance",
      "level10.ballance",
      "level11.ballance",
      "level12.ballance",
      "level13.ballance",
    };

    internal static bool CheckPackageInBuiltInPackage(string packageName) {
      for (int i = 0; i < builtInPackagesFiles.Length; i++)
      { 
        if(builtInPackagesFiles[i] == packageName)
          return true;
      }
      return false;
    }
    internal static bool CheckPackageInBuiltInLevel(string packageName) {
      for (int i = 0; i < builtInLevelsFiles.Length; i++)
      { 
        if(builtInLevelsFiles[i] == packageName)
          return true;
      }
      return false;
    }

    /// <summary>
    /// 将资源的相对路径转为资源真实路径
    /// </summary>
    /// <param name="type">资源种类（core: 核心文件、level：关卡、package：模块）</param>
    /// <param name="pathorname">相对路径或名称</param>
    /// <param name="replacePlatform">是否替换文件路径中的[Platform]</param>
    /// <returns></returns>
    [LuaApiDescription("将资源的相对路径转为资源真实路径")]
    [LuaApiParamDescription("type", "资源种类（gameinit、core: 核心文件、level：关卡、package：模块）")]
    [LuaApiParamDescription("pathorname", "相对路径或名称")]
    [LuaApiParamDescription("replacePlatform", "是否替换文件路径中的[Platform]")]
    public static string GetResRealPath(string type, string pathorname, bool replacePlatform = true, bool withFileSheme = true)
    {
      string result = null;
      string pathbuf = "";
      string[] spbuf = null;

      if (replacePlatform && pathorname.Contains("[Platform]"))
        pathorname = pathorname.Replace("[Platform]", GameConst.GamePlatformIdentifier);

      spbuf = SplitResourceIdentifier(pathorname, out pathbuf);

      if (type == "" && pathorname.Contains(":"))
        type = spbuf[0].ToLower();

      if (type == "logfile")
      {
#if UNITY_EDITOR
        result = DEBUG_PATH + "/output.log";
#else
        result = Application.temporaryCachePath + "/output.log";
#endif
      }
      else if (type == "level") return GetLevelRealPath(pathbuf, withFileSheme);
      else if (type == "package")
      {
        if (pathbuf.Contains(":"))
        {
          if (PathUtils.IsAbsolutePath(pathbuf)) 
            return pathbuf;
#if UNITY_EDITOR
          pathbuf = DEBUG_PACKAGES_PATH + pathbuf;
#elif UNITY_STANDALONE
          pathbuf= Application.dataPath + "/Packages/" + pathbuf;
#elif UNITY_ANDROID || UNITY_IOS
          if(CheckPackageInBuiltInPackage(pathbuf)
            pathbuf = Application.streamingAssetsPath + "/BuiltInPackages/Packages/" + pathbuf;
          else 
            pathbuf = Application.persistentDataPath + "/Packages/" + pathbuf;
#else
          pathbuf = pathbuf;
#endif
          result = ReplacePathInResourceIdentifier(pathbuf, ref spbuf);
        }
        else
        {
#if UNITY_EDITOR
          result = DEBUG_PACKAGES_PATH + pathbuf;
#elif UNITY_STANDALONE
          result = Application.dataPath + "/Packages/" + pathbuf;
#elif UNITY_ANDROID || UNITY_IOS
          if(CheckPackageInBuiltInPackage(pathbuf))
            result = Application.streamingAssetsPath + "/BuiltInPackages/Packages/" + pathbuf;
          else 
            result = Application.persistentDataPath + "/Packages/" + pathbuf;
#else
          result = pathorname;
#endif
        }
      }
      else if (type == "core")
      {
        if (pathbuf.Contains(":"))
        {
          if (PathUtils.IsAbsolutePath(pathbuf)) 
            return pathbuf;
#if UNITY_EDITOR
          pathbuf = DEBUG_PATH + "/Core/" + pathbuf;
#elif UNITY_STANDALONE
          pathbuf = Application.dataPath + "/Core/" + pathbuf;
#elif UNITY_ANDROID || UNITY_IOS
          pathbuf = Application.streamingAssetsPath + "/BuiltInPackages/Core/" + pathbuf;
#else
          GameErrorChecker.LastError = GameError.NotImplemented;
          return pathorname;
#endif
          result = ReplacePathInResourceIdentifier(pathbuf, ref spbuf);
        }
        else
        {
#if UNITY_EDITOR
          result = DEBUG_PATH + "/Core/" + pathbuf;
#elif UNITY_STANDALONE
          result = Application.dataPath + "/Core/" + pathbuf;
#elif UNITY_ANDROID || UNITY_IOS
          result = Application.streamingAssetsPath + "/BuiltInPackages/Core/" + pathbuf;
#else
          GameErrorChecker.LastError = GameError.NotImplemented;
          return pathorname;
#endif
        }
      }
      else
      {
        GameErrorChecker.LastError = GameError.UnKnowType;
        return pathorname;
      }

      return (withFileSheme && result.StartsWith("jar:") ? "file:///" : "") + result;
    }
    /// <summary>
    /// 将关卡资源的相对路径转为关卡资源真实路径
    /// </summary>
    /// <param name="pathorname">关卡的相对路径或名称</param>
    /// <returns></returns>
    [LuaApiDescription("将关卡资源的相对路径转为关卡资源真实路径", "")]
    [LuaApiParamDescription("pathorname", "关卡的相对路径或名称")]
    public static string GetLevelRealPath(string pathorname, bool withFileSheme = true)
    {
      string result = "";
      string pathbuf = "";
      string[] spbuf = null;

      if (pathorname.Contains(":"))
      {
        spbuf = SplitResourceIdentifier(pathorname, out pathbuf);

        if (PathUtils.IsAbsolutePath(pathbuf)) 
          return pathbuf;
#if UNITY_EDITOR
        pathbuf = DEBUG_LEVELS_PATH + pathbuf;
#elif UNITY_STANDALONE
        pathbuf = Application.dataPath + "/Levels/" + pathbuf;
#elif UNITY_ANDROID || UNITY_IOS
        if(CheckPackageInBuiltInLevel(pathbuf))
          pathbuf = Application.streamingAssetsPath + "/BuiltInPackages/Levels/" + pathbuf;
        else
          pathbuf = Application.persistentDataPath + "/Levels/" + pathbuf;
#else
        pathbuf = pathbuf;
#endif
        result = ReplacePathInResourceIdentifier(pathbuf, ref spbuf);
      }
      else
      {
#if UNITY_EDITOR
        result = DEBUG_LEVELS_PATH + pathorname;
#elif UNITY_STANDALONE
        result = Application.dataPath + "/Levels/" + pathorname;
#elif UNITY_ANDROID || UNITY_IOS
        if(CheckPackageInBuiltInLevel(pathorname))
          result = Application.streamingAssetsPath + "/BuiltInPackages/Levels/" + pathorname;
        else
          result = Application.persistentDataPath + "/Levels/" + pathorname;
#else
        result = pathorname;
#endif
      }
      
      if(!result.EndsWith(".ballance"))
        result += ".ballance";

      return (withFileSheme ? "file:///" : "") + result;
    }
    /// <summary>
    /// Replace Path In Resource Identifier (Identifier:Path:Arg0:Arg1)
    /// </summary>
    /// <param name="oldIdentifier">Identifier Want br replace</param>
    /// <param name="newPath"></param>
    /// <param name="buf"></param>
    /// <returns></returns>
    public static string ReplacePathInResourceIdentifier(string newPath, ref string[] buf)
    {
      if (buf.Length > 1)
      {
        buf[1] = newPath;
        string s = "";
        foreach (string si in buf)
          s += ":" + si;
        return s.Remove(0, 1);
      }
      return newPath;
    }
    /// <summary>
    /// 分割资源标识符
    /// </summary>
    /// <param name="oldIdentifier">资源标识符</param>
    /// <param name="outPath">输出资源路径</param>
    /// <returns></returns>
    public static string[] SplitResourceIdentifier(string oldIdentifier, out string outPath)
    {
      string[] buf = oldIdentifier.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

      if (buf.Length > 2)
      {
        if (buf[1].Length == 1 && (buf[2].StartsWith("/") || buf[2].StartsWith("\\")))
        {
          string[] newbuf = new string[buf.Length - 1];
          newbuf[0] = buf[0];
          newbuf[1] = buf[1] + buf[2];
          for (int i = 2; i < newbuf.Length; i++)
            newbuf[i] = buf[i + 1];
          buf = newbuf;
        }
      }
      if (buf.Length > 1) outPath = buf[1];
      else outPath = oldIdentifier;
      return buf;
    }
  }
}
