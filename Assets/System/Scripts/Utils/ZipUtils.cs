using Ballance2.Services.Debug;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using StbImageSharp;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* ZipUtils.cs
* 
* 用途：
* Zip 帮助类。用于读取操作Zip文件
*
* 作者：
* mengyu
*
*/
namespace Ballance2.Utils
{
  /// <summary>
  /// Zip 帮助类。用于读取操作Zip文件
  /// </summary>
  public class ZipUtils
  {
    /// <summary>
    /// 添加文件夹至 Zip 文件中
    /// </summary>
    /// <param name="zipStream">Zip 文件</param>
    /// <param name="dir">要添加的文件夹路径</param>
    /// <param name="patten">子文件搜索筛选，所有文件则可以是 *</param>
    /// <param name="crc">Crc32校验</param>
    public static void AddDirFileToZip(ZipOutputStream zipStream, string baseDir, string dir, string patten, ref Crc32 crc)
    {
      var dirinfo = new DirectoryInfo(dir);
      var dirs = dirinfo.GetFiles(patten, SearchOption.TopDirectoryOnly);
      foreach(var file in dirs)
        AddFileToZip(zipStream, file.FullName, baseDir.Length, ref crc);
    }
    /// <summary>
    /// 添加文件至 Zip 文件中
    /// </summary>
    /// <param name="zipStream">Zip 文件</param>
    /// <param name="file">要添加的文件路径</param>
    /// <param name="subIndex">从路径字符串中指定位置截取成为文件名</param>
    /// <param name="crc">Crc32校验</param>
    public static void AddFileToZip(ZipOutputStream zipStream, string file, int subIndex, ref Crc32 crc)
    {
      string fileName = file.Substring(subIndex);
      AddFileToZip(zipStream, file, fileName, ref crc);
    }
    /// <summary>
    /// 添加文件至 Zip 文件中
    /// </summary>
    /// <param name="zipStream">Zip 文件</param>
    /// <param name="file">要添加的文件路径</param>
    /// <param name="inZipFilePath">指定文件在Zip中的路径</param>
    /// <param name="crc">Crc32校验</param>
    public static void AddFileToZip(ZipOutputStream zipStream, string file, string inZipFilePath, ref Crc32 crc)
    {
      FileStream fileStream = File.OpenRead(file);
      byte[] buffer = new byte[fileStream.Length];
      fileStream.Read(buffer, 0, buffer.Length);

      ZipEntry entry = new ZipEntry(inZipFilePath);
      entry.DateTime = DateTime.Now;
      entry.Size = fileStream.Length;
      fileStream.Close();
      crc.Reset();
      crc.Update(buffer);
      entry.Crc = crc.Value;
      zipStream.PutNextEntry(entry);
      zipStream.Write(buffer, 0, buffer.Length);
    }
    /// <summary>
    /// 创建 Zip 文件
    /// </summary>
    /// <param name="file">Zip 文件路径</param>
    /// <returns>Zip 文件流句柄</returns>
    public static ZipOutputStream CreateZipFile(string file)
    {
      ZipOutputStream zipStream = new ZipOutputStream(File.Create(file));
      zipStream.SetLevel(0);  // 压缩级别 0-9
      return zipStream;
    }
    /// <summary>
    /// 关闭 Zip 文件
    /// </summary>
    /// <param name="file">Zip 文件路径</param>
    /// <returns>Zip 文件流句柄</returns>
    public static void CloseZipFile(ZipOutputStream file)
    {
      file.Close();
      file.Dispose();
    }
    /// <summary>
    /// 打开已存在的 Zip 文件
    /// </summary>
    /// <param name="file">Zip 文件路径</param>
    /// <returns>Zip 文件流句柄</returns>
    public static ZipInputStream OpenZipFile(string file)
    {
      ZipInputStream zipStream = null;
      try
      {
        zipStream = new ZipInputStream(File.OpenRead(file));
      }
      catch (Exception e)
      {
        Log.E("ZipUtils", "Load Zip file {0} failed! {1}", file, e.ToString());
        GameErrorChecker.LastError = GameError.FileReadFailed;
      }
      return zipStream;
    }
    /// <summary>
    /// 打开流为 Zip 文件句柄
    /// </summary>
    /// <param name="stream">文件流</param>
    /// <returns>Zip 文件流句柄</returns>
    public static ZipInputStream OpenZipStream(Stream stream)
    {
      ZipInputStream zipStream = null;
      try
      {
        zipStream = new ZipInputStream(stream);
      }
      catch (Exception e)
      {
        Log.E("ZipUtils", "Load Zip stream failed! {1}", e.ToString());
        GameErrorChecker.LastError = GameError.FileReadFailed;
      }
      return zipStream;
    }
    /// <summary>
    /// 从 Zip 文件读取至内存
    /// </summary>
    /// <param name="zip">Zip 文件</param>
    /// <returns></returns>
    public static MemoryStream ReadZipFileToMemory(ZipInputStream zip)
    {
      int size;
      MemoryStream ms = new MemoryStream();
      byte[] buffer = new byte[4096];
      while (true)
      {
        size = zip.Read(buffer, 0, 4096);
        if (size > 0)
          ms.Write(buffer, 0, size);
        else break;
      }
      return ms;
    }
    /// <summary>
    /// 异步从 Zip 文件读取至内存
    /// </summary>
    /// <param name="zip">Zip 文件</param>
    /// <returns></returns>
    public static async Task<MemoryStream> ReadZipFileToMemoryAsync(ZipInputStream zip)
    {
      MemoryStream ms = new MemoryStream();
      byte[] buffer = new byte[1048576];
      while (true)
      {
        int i = await zip.ReadAsync(buffer, 0, 1048576);
        if (i > 0)
          ms.Write(buffer, 0, i);
        else break;
      }
      return ms;
    }

    public static bool MatchRootName(string name, ZipEntry theEntry)
    {
      return theEntry.Name == name || theEntry.Name == $"/{name}";
    }
    public static bool MatchRootName2(string path, string ext, ZipEntry theEntry)
    {
      return (theEntry.Name.StartsWith(path) || theEntry.Name.StartsWith($"/{path}")) && theEntry.Name.EndsWith(ext);
    }

    /// <summary>
    /// 读取ZIP文件当前文件为字符串返回
    /// </summary>
    /// <param name="zip"></param>
    /// <param name="theEntry"></param>
    /// <returns></returns>
    public static async Task<string> LoadStringInZip(ZipInputStream zip, ZipEntry theEntry)
    {
      MemoryStream ms = await ZipUtils.ReadZipFileToMemoryAsync(zip);
      string content = "";

      try
      {
        content = StringUtils.GetUtf8Bytes(StringUtils.FixUtf8BOM(ms.ToArray()));
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        ms.Close();
        ms.Dispose();
      }

      return content;
    }
    /// <summary>
    /// 直接从ZIP文件中读取Sprite
    /// </summary>
    /// <param name="zip"></param>
    /// <param name="theEntry"></param>
    /// <returns></returns>
    public static async Task<Sprite> LoadSpriteInZip(ZipInputStream zip, ZipEntry theEntry)
    {
      MemoryStream ms = await ReadZipFileToMemoryAsync(zip);
      try
      {
        ImageInfo? info = ImageInfo.FromStream(ms);
        if (info == null)
          throw new Exception("Get ImageInfo failed");
        Texture2D t2d = new Texture2D(info.Value.Width, info.Value.Height);
        t2d.LoadImage(ms.ToArray());
        t2d.Apply();
        return TextureUtils.CreateSpriteFromTexture(t2d);
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        ms.Close();
        ms.Dispose();
      }
    }
    /// <summary>
    /// 直接从ZIP文件中读取Texture2D
    /// </summary>
    /// <param name="zip"></param>
    /// <param name="theEntry"></param>
    /// <returns></returns>
    public static async Task<Texture2D> LoadTextureInZip(ZipInputStream zip, ZipEntry theEntry)
    {
      MemoryStream ms = await ReadZipFileToMemoryAsync(zip);
      try
      {
        ImageInfo? info = ImageInfo.FromStream(ms);
        if (info == null)
          throw new Exception("Get ImageInfo failed");
        Texture2D t2d = new Texture2D(info.Value.Width, info.Value.Height);
        t2d.LoadImage(ms.ToArray());
        t2d.Apply();
        return t2d;
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        ms.Close();
        ms.Dispose();
      }
    }
  }
}
