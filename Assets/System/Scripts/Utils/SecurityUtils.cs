using System.IO;
using Ballance2.Config.Settings;
using UnityEngine;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* SecurityUtils.cs
* 
* 用途：
* Lua 虚拟机安全工具类。
* 负责修复 Lua 虚拟机不安全的函数与相关require功能。
* 负责检查文件系统的不安全访问并抛出异常。
*
* 作者：
* mengyu
*/

namespace Ballance2.Utils
{
  public static class SecurityUtils
  {
    private static string currentDir = "";
    private static string[] disabledRequire = new string[] {
      "package",
      "io"
    };

    public static bool CheckRequire(string pathOrName)
    {
      foreach (var n in disabledRequire)
        if (pathOrName == n)
          return true;
      return false;
    }
    public static void CheckFileAccess(string path)
    {
      if (currentDir == "")
      {
        currentDir = Directory.GetCurrentDirectory();
        currentDir = currentDir.Replace("\\", "/");
      }
      if (path.StartsWith("http://") || path.StartsWith("https://") ||
          path.StartsWith("ftp://") || path.StartsWith("ftps://"))
        return;
      if (path.StartsWith("file:///"))
        path = path.Substring(8);
      if (!Path.IsPathRooted(path))
        path = currentDir + "/" + path;
      else
        path = Path.GetFullPath(path);
      path = path.Replace("\\", "/");

#if UNITY_EDITOR
      if (!(path.StartsWith(currentDir)
          || path.StartsWith(DebugSettings.Instance.DebugFolder)
          || path.StartsWith(DebugSettings.Instance.OutputFolder)))
        throw new FileAccessException(path);

      if (!(path.StartsWith(currentDir)
          || path.StartsWith(Application.dataPath)
          || path.StartsWith(Application.persistentDataPath)
          || path.StartsWith(Application.temporaryCachePath)
          || path.StartsWith(Application.streamingAssetsPath)))
        throw new FileAccessException(path);
#else
#endif
    }
  }

  /// <summary>
  /// 文件异常访问
  /// </summary>
  public class FileAccessException : System.Exception
  {
    public FileAccessException(string filePath) : base("Can not access file " + filePath)
    {
    }
  }
}