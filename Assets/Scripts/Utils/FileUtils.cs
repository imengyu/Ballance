
using Ballance2.System.Res;
using System.IO;

namespace Ballance2.Utils
{
    /// <summary>
    /// 文件工具
    /// </summary>
    [SLua.CustomLuaClass]
    public class FileUtils
    {
        private static byte[] zipHead = new byte[4] { 0x50, 0x4B, 0x03, 0x04 };
        private static byte[] untyFsHead = new byte[7] { 0x55, 0x6e, 0x69, 0x74, 0x79, 0x46, 0x53 };

        public static bool TestFileIsZip(string file)
        {
            return TestFileHead(file, zipHead);
        }
        public static bool TestFileIsAssetBundle(string file)
        {
            return TestFileHead(file, untyFsHead);
        }

        public static bool TestFileHead(string file, byte[] head)
        {
            byte[] temp = new byte[head.Length];
            FileStream fs = new FileStream(GamePathManager.FixFilePathScheme(file), FileMode.Open);
            fs.Read(temp, 0, head.Length);
            fs.Close();
            return StringUtils.TestBytesMatch(temp, head);
        }
    }


}
