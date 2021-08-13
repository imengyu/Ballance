
using Ballance2.LuaHelpers;
using Ballance2.Sys.Res;
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
* 文件工具类
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Utils
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("文件工具类")]
    public class FileUtils
    {
        private static byte[] zipHead = new byte[4] { 0x50, 0x4B, 0x03, 0x04 };
        private static byte[] untyFsHead = new byte[7] { 0x55, 0x6e, 0x69, 0x74, 0x79, 0x46, 0x53 };

        /// <summary>
        /// 检测文件头是不是zip
        /// </summary>
        /// <param name="file">要检测的文件路径</param>
        /// <returns>如果文件头匹配则返回true，否则返回false</returns>
        [LuaApiDescription("检测文件头是不是zip", "如果文件头匹配则返回true，否则返回false")]
        [LuaApiParamDescription("file", "要检测的文件路径")]
        public static bool TestFileIsZip(string file)
        {
            return TestFileHead(file, zipHead);
        }
        /// <summary>
        /// 检测文件头是不是unityFs
        /// </summary>
        /// <param name="file">要检测的文件路径</param>
        /// <returns>如果文件头匹配则返回true，否则返回false</returns>
        [LuaApiDescription("检测文件头是不是unityFs", "如果文件头匹配则返回true，否则返回false")]
        [LuaApiParamDescription("file", "要检测的文件路径")]
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
        [LuaApiDescription("检测自定义文件头", "如果文件头匹配则返回true，否则返回false")]
        [LuaApiParamDescription("file", "要检测的文件路径")]
        [LuaApiParamDescription("head", "自定义文件头")]
        public static bool TestFileHead(string file, byte[] head)
        {
            byte[] temp = new byte[head.Length];
            FileStream fs = new FileStream(GamePathManager.FixFilePathScheme(file), FileMode.Open);
            fs.Read(temp, 0, head.Length);
            fs.Close();
            return StringUtils.TestBytesMatch(temp, head);
        }

        /// <summary>
        /// 把文件大小（字节）按单位转换为可读的字符串
        /// </summary>
        /// <param name="longFileSize">文件大小（字节）</param>
        /// <returns>可读的字符串，例如2.5M</returns>
        [LuaApiDescription("把文件大小（字节）按单位转换为可读的字符串", "可读的字符串，例如2.5M")]
        [LuaApiParamDescription("longFileSize", "文件大小（字节）")]
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
