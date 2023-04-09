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
* 作者：
* mengyu
*
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 文件工具类
  /// </summary>
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
      byte[] temp = new byte[head.Length];
      FileStream fs = new FileStream(PathUtils.FixFilePathScheme(file), FileMode.Open);
      fs.Read(temp, 0, head.Length);
      fs.Close();
      return StringUtils.TestBytesMatch(temp, head);
    }

    /// <summary>
    /// 写入字符串至文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="append">是否是追加写入</param>
    /// <param name="data">写入内容</param>
    public static void WriteFile(string path, bool append, string data)
    {
      var sw = new StreamWriter(path, append);
      sw.Write(data);
      sw.Close();
      sw.Dispose();
    }
  
    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public static bool FileExists(string path) { return File.Exists(path); }
    
    /// <summary>
    /// 检查目录是否存在
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public static bool DirectoryExists(string path) { return Directory.Exists(path); }
    
    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="path">路径</param>
    public static void CreateDirectory(string path)
    {
      Directory.CreateDirectory(path);
    }
    
    /// <summary>
    /// 读取文件为字符串
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public static string ReadFile(string path)
    {
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
    /// <returns>返回字节数组</returns>
    public static byte[] ReadAllToBytes(string file)
    {
      FileStream fs = new FileStream(PathUtils.FixFilePathScheme(file), FileMode.Open);
      byte[] temp = new byte[fs.Length];
      fs.Read(temp, 0, temp.Length);
      fs.Close();
      return temp;
    }
    
    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path">路径</param>
    public static void RemoveFile(string path)
    {
      if (Directory.Exists(path))
        Directory.Delete(path, true);
      else if (File.Exists(path))
        File.Delete(path);
    }
    
    /// <summary>
    /// 删除目录
    /// </summary>
    /// <param name="path">路径</param>
    public static void RemoveDirectory(string path)
    {
      if (Directory.Exists(path))
        Directory.Delete(path, true);
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
