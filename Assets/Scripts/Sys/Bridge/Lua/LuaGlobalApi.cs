using Ballance2.Sys.Package;
using Ballance2.Sys.Services;
using Ballance2.Sys.Utils;
using SLua;
using UnityEngine;

namespace Ballance2.Sys.Bridge.Lua
{
    [CustomLuaClass]
    public class LuaGlobalApi
    {
        [CustomLuaClass]
        public delegate object RequireDelegate(string n);
        private static RequireDelegate originalRequire = null;
        private static GamePackageManager pm = null;

        internal static void SetRequire(LuaFunction fun)
        {
            originalRequire = fun.cast<RequireDelegate>();
        }

        /// <summary>
        /// 自动Require
        /// </summary>
        /// <param name="pathOrName"></param>
        /// <returns></returns>
        public static object require(string pathOrName)
        {
            if(SecurityUtils.CheckRequire(pathOrName))
                throw new RequireProhibitAccessException(pathOrName);

            if(pm == null) 
                pm = GameManager.Instance.GetSystemService<GamePackageManager>();

            //处理以包名 __xxx__/ 为开头的字符串
            var lastIdx = pathOrName.IndexOf("__/");
            if(pathOrName.StartsWith("__") && lastIdx >= 0) {
                var packname = pathOrName.Substring(2, lastIdx - 2);
                var pack = pm.FindPackage(packname);
                if(pack == null)
                    throw new RequireFailedException("Package " + packname + " not found");
                return pack.RequireLuaFile(pathOrName.Substring(lastIdx + 3));
            } 
            //有斜杠
            if(pathOrName.Contains("/")) 
                return GamePackage.GetSystemPackage().RequireLuaFile(pathOrName.Substring(lastIdx + 3));
            
            //普通require
            return originalRequire(pathOrName);
        }
    }
    
    public class RequireProhibitAccessException : System.Exception
    {
        public RequireProhibitAccessException(string pathOrName) : base("Require " + pathOrName + " are not allowed") {}
    } 
    public class RequireFailedException : System.Exception
    {
        public RequireFailedException(string msg) : base(msg) {}
    } 
    public class EmptyFileException : System.Exception
    {
        public EmptyFileException(string msg) : base(msg) {}
    } 
    
}