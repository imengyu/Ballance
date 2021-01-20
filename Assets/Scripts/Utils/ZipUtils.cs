using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Ballance2.Utils
{
    /// <summary>
    /// zip 帮助类
    /// </summary>
    public class ZipUtils
    {
        public static void AddFileToZip(ZipOutputStream zipStream, string file, int subIndex, Crc32 crc)
        {
            string fileName = file.Substring(subIndex);
            AddFileToZip(zipStream, file, fileName, crc);
        }
        public static void AddFileToZip(ZipOutputStream zipStream, string file, string inZipFilePath, Crc32 crc)
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
        public static ZipOutputStream CreateZipFile(string file)
        {
            ZipOutputStream zipStream = new ZipOutputStream(File.Create(file));
            zipStream.SetLevel(0);  // 压缩级别 0-9
            return zipStream;
        }
        public static ZipInputStream OpenZipFile(string file)
        {
            ZipInputStream zipStream = new ZipInputStream(File.OpenRead(file));
            return zipStream;
        }
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
        public static async Task<MemoryStream> ReadZipFileToMemoryAsync(ZipInputStream zip)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1048576];
            while (true)
            {
                int i = await zip.ReadAsync(buffer, 0, 1048576);
                if (i > 0)
                    ms.Write(buffer, 0, 1048576);
                else break;
            }
            return ms;
        }
    }
}
