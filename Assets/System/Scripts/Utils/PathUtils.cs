using System.Collections.Generic;
using System.IO;
using System.Text;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* PathUtils.cs
* 
* 用途：
* 路径字符串工具类
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 路径字符串工具类, 提供了一些路径的处理工具方法。
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("路径字符串工具类, 提供了一些路径的处理工具方法。")]
  public static class PathUtils
  {
    /// <summary>
    /// 检测一个路径是否是绝对路径
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>是否是绝对路径</returns>
    [LuaApiDescription("检测一个路径是否是绝对路径", "是否是绝对路径")]
    [LuaApiParamDescription("path", "路径")]
    public static bool IsAbsolutePath(string path)
    {
#if (UNITY_EDITOR && UNITY_EDITOR_WIN) || UNITY_STANDALONE_WIN || UNITY_WSA
      if (path.Length > 2)
        return path[1] == ':' && (path[2] == '\\' || path[2] == '/');
      return false;
#elif (UNITY_EDITOR && UNITY_EDITOR_OSX) || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
      return path.StartsWith("/");
#elif UNITY_IOS
      return path.StartsWith("/");
#endif
    }
    /// <summary>
    /// 替换绝对路径至当前项目相对路径
    /// </summary>
    /// <param name="path">绝对路径</param>
    /// <returns></returns>
    [LuaApiDescription("替换绝对路径至当前项目相对路径")]
    [LuaApiParamDescription("path", "绝对路径")]
    public static string ReplaceAbsolutePathToRelativePath(string path)
    {
      path = path.Replace("\\", "/");
      if(IsAbsolutePath(path)) {
        var dir = Directory.GetCurrentDirectory().Replace("\\", "/");
        var index = path.IndexOf(dir);
        return path.Substring(index + dir.Length);
      } else if(path.StartsWith("Assets/"))
        return path.Substring(7);
      else if(path.StartsWith("/Assets/")) 
        return path.Substring(8);

      return path;
    }
#if UNITY_EDITOR
    /**
      * 物理路径转为项目路径
      */
    [SLua.DoNotToLua]
    public static string AbsPathToRelPath(string path) {
      path = path.Replace('\\', '/');
      if(PathUtils.IsAbsolutePath(path)) {
        var current = System.IO.Directory.GetCurrentDirectory().Replace('\\', '/');
        var startIndex = path.IndexOf(current);
        if(startIndex == 0)
          path = path.Substring(current.Length);
        if(path.EndsWith("/"))
          path = path.Remove(path.Length - 1);
        if(path.StartsWith("/"))
          path = path.Remove(0, 1);
      }
      return path;
    }
#endif
    /// <summary>
    /// 合并两个相对路径为完整路径，它会处理当前目录（./）和上级目录（../）
    /// </summary>
    /// <param name="path1">主路径</param>
    /// <param name="path2">副路径</param>
    /// <returns>处理完成的完整路径</returns>
    [LuaApiDescription("合并两个相对路径为完整路径，它会处理当前目录（./）和上级目录（../）", "处理完成的完整路径")]
    [LuaApiParamDescription("path1", "主路径")]
    [LuaApiParamDescription("path2", "副路径")]
    public static string JoinTwoPath(string path1, string path2)
    {
      StringBuilder sb = new StringBuilder();
      List<string> dirs1 = new List<string>(path1.Split('/'));
      List<string> dirs2 = new List<string>(path2.Split('/'));

      for (int i = 0; i < dirs2.Count; i++)
      {
        string n = dirs2[i];
        if (n == ".")
          continue;
        else if (n == "..")
          dirs1.RemoveAt(dirs1.Count - 1); //移除一个，目录
        else
          dirs1.Add(n); //添加
      }
      for (int i = 0; i < dirs1.Count; i++)
      {
        sb.Append(dirs1[i]);
        if (i != dirs1.Count - 1 || path2.EndsWith("/"))
          sb.Append('/');
      }
      return sb.ToString();
    }
    /// <summary>
    /// 获取路径中的文件名（不包括后缀）
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    [LuaApiDescription("获取路径中的文件名（不包括后缀）", "")]
    [LuaApiParamDescription("path", "路径")]
    public static string GetFileNameWithoutExt(string path)
    {
      bool chooseRight = path.Contains("/") && !path.Contains("\\");
      int lastPos = path.LastIndexOf(chooseRight ? '/' : '\\');
      if (lastPos > 0)
      {
        if (lastPos == path.Length - 1)
        {
          path = path.Remove(path.Length - 1);
          lastPos = path.LastIndexOf(chooseRight ? '/' : '\\');
        }
        path = path.Substring(lastPos + 1);
        if (path.Contains("."))
        {
          lastPos = path.LastIndexOf('.');
          path = path.Substring(0, lastPos);
        }
      }
      return path;
    }
    /// <summary>
    /// 获取路径中的文件名（包括后缀）
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    [LuaApiDescription("获取路径中的文件名（包括后缀）", "")]
    [LuaApiParamDescription("path", "路径")]
    public static string GetFileName(string path)
    {
      bool chooseRight = path.Contains("/") && !path.Contains("\\");
      int lastPos = path.LastIndexOf(chooseRight ? '/' : '\\');
      if (lastPos > 0)
      {
        if (lastPos == path.Length - 1)
        {
          path = path.Remove(path.Length - 1);
          lastPos = path.LastIndexOf(chooseRight ? '/' : '\\');
        }
        path = path.Substring(lastPos + 1);
      }
      return path;
    }
    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    [LuaApiDescription("检查文件是否存在", "")]
    [LuaApiParamDescription("path", "文件路径")]
    public static bool Exists(string path)
    {
      if (path.StartsWith("file:///"))
        return File.Exists(path.Substring(8));
      return File.Exists(path);
    }
    /// <summary>
    /// 修复文件路径的 file:/// 前缀
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>返回不带 file:/// 前缀的路径</returns>
    [LuaApiDescription("修复文件路径的 file:/// 前缀", "返回不带 file:/// 前缀的路径")]
    [LuaApiParamDescription("path", "路径")]
    public static string FixFilePathScheme(string path)
    {
      if (path.StartsWith("file:///"))
        return path.Substring(8);
      return path;
    }

  }
}
