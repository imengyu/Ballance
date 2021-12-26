using System.IO;
using System.Text;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* FileUtils.cs
* 
* 用途：
* 文件工具类。提供了文件操作相关工具方法。
* 
* Lua 中不允许直接访问文件系统，因此此处提供了一些方法来允许Lua读写本地配置文件,操作或删除本地目录等。
* 但注意，这些API不允许访问用户文件，只允许访问以下目录：
* 游戏主目录（exe同级与子目录）
* Application.dataPath
* Application.persistentDataPath
* Application.temporaryCachePath
* Application.streamingAssetsPath
* 尝试访问不可访问的目录将会抛出异常。
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 文件工具类
  /// </summary>
  [JSExport]
  public class FileUtils
  {
    private static byte[] zipHead = new byte[4] { 0x50, 0x4B, 0x03, 0x04 };
    private static byte[] untyFsHead = new byte[7] { 0x55, 0x6e, 0x69, 0x74, 0x79, 0x46, 0x53 };

    /// <summary>
    /// 检测文件头是不是zip
    /// </summary>
    /// <param name="file">要检测的文件路径</param>
    /// <returns>如果文件头匹配则返回true，否则返回false</returns>
    public static bool TestFileIsZip(string file)
    {
      return TestFileHead(file, zipHead);
    }
    /// <summary>
    /// 检测文件头是不是unityFs
    /// </summary>
    /// <param name="file">要检测的文件路径</param>
    /// <returns>如果文件头匹配则返回true，否则返回false</returns>
    public static bool TestFileIsAssetBundle(string file)
    {
      return TestFileHead(file, untyFsHead);
    }
    /// <summary>
    /// 检测自定义文件头
    /// </summary>
    /// <param name="file">要检测的文件路径</param>
    /// <param name="head">自定义文件头</param>
    /// <returns>如果文件头匹配则返回true，否则返回false</returns>
    public static bool TestFileHead(string file, byte[] head)
    {
      SecurityUtils.CheckFileAccess(file);
      byte[] temp = new byte[head.Length];
      FileStream fs = new FileStream(PathUtils.FixFilePathScheme(file), FileMode.Open);
      fs.Read(temp, 0, head.Length);
      fs.Close();
      return StringUtils.TestBytesMatch(temp, head);
    }
    public static void WriteFile(string path, bool append, string data)
    {
      SecurityUtils.CheckFileAccess(path);
      var sw = new StreamWriter(path, append);
      sw.Write(data);
      sw.Close();
      sw.Dispose();
    }

    public static bool FileExists(string path) { return File.Exists(path); }
    public static bool DirectoryExists(string path) { return Directory.Exists(path); }
    public static void CreateDirectory(string path)
    {
      SecurityUtils.CheckFileAccess(path);
      Directory.CreateDirectory(path);
    }
    public static string ReadFile(string path)
    {
      SecurityUtils.CheckFileAccess(path);

      if (!File.Exists(path))
        throw new FileNotFoundException("Cant read non-exists file", path);

      var sr = new StreamReader(path);
      var rs = sr.ReadToEnd();
      sr.Close();
      sr.Dispose();
      return rs;
    }
    /// <summary>
    /// 读取文件所有内容为字节数组
    /// </summary>
    /// <param name="file">文件路径</param>
    /// <remarks>注意：此 API 不能读取用户个人的本地文件。</remarks>
    /// <returns>返回字节数组</returns>
    public static byte[] ReadAllToBytes(string file)
    {
      SecurityUtils.CheckFileAccess(file);
      FileStream fs = new FileStream(PathUtils.FixFilePathScheme(file), FileMode.Open);
      byte[] temp = new byte[fs.Length];
      fs.Read(temp, 0, temp.Length);
      fs.Close();
      return temp;
    }
    public static void RemoveFile(string path)
    {
      SecurityUtils.CheckFileAccess(path);

      if (Directory.Exists(path))
        Directory.Delete(path, true);
      else if (File.Exists(path))
        File.Delete(path);
    }

    /// <summary>
    /// 把文件大小（字节）按单位转换为可读的字符串
    /// </summary>
    /// <param name="longFileSize">文件大小（字节）</param>
    /// <returns>可读的字符串，例如2.5M</returns>
    public static string GetBetterFileSize(long longFileSize)
    {
      StringBuilder sizeStr = new StringBuilder();
      float fileSize;
      if (longFileSize >= 1073741824)
      {
        fileSize = Mathf.Round(longFileSize / 1073741824 * 100) / 100;
        sizeStr.Append(fileSize);
        sizeStr.Append("G");
      }
      else if (longFileSize >= 1048576)
      {
        fileSize = Mathf.Round(longFileSize / 1048576 * 100) / 100;
        sizeStr.Append(fileSize);
        sizeStr.Append("M");
      }
      else
      {
        fileSize = Mathf.Round(longFileSize / 1024 * 100) / 100;
        sizeStr.Append(fileSize);
        sizeStr.Append("K");
      }
      return sizeStr.ToString();
    }
  }


}
