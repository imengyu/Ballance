using Ballance.LuaHelpers;
using Ballance2.Config;
using Ballance2.Config.Settings;
using Ballance2.Sys.Debug;
using System;
using System.IO;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameInit.cs
* 
* 用途：
* 管理外部资源文件的路径
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Sys.Res
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
        /// 调试路径（输出目录）<c>（您在调试时请点击菜单 "Ballance">"开发设置">"Debug Settings" 将其更改为自己调试输出存放目录）</c>
        /// </summary>
        [LuaApiDescription("调试模组包存放路径")]
        public static string DEBUG_PATH 
        {
            get
            {
                DebugSettings debugSettings = DebugSettings.Instance;
                if (debugSettings != null)
                    return debugSettings.DebugFolder.Replace("\\", "/");
                return "";
            }
        }
        /// <summary>
        /// 调试路径（模组目录）
        /// </summary>
        [LuaApiDescription("调试模组包存放路径")]
        public static string DEBUG_PACKAGES_PATH { get { return DEBUG_PATH + "/packages/"; } }
        /// <summary>
        /// 调试路径（关卡目录）
        /// </summary>
        [LuaApiDescription("调试模组包存放路径")]
        public static string DEBUG_LEVELS_PATH { get { return DEBUG_PATH + "/levels/"; } }

        /// <summary>
        /// 安卓系统数据目录
        /// </summary>
        [LuaApiDescription("调试模组包存放路径")]
        public const string ANDROID_FOLDER_PATH = "/sdcard/games/com.imengyu.ballance2/";
        /// <summary>
        /// 安卓系统模组目录
        /// </summary>
        [LuaApiDescription("调试模组包存放路径")]
        public const string ANDROID_PACKAGES_PATH = ANDROID_FOLDER_PATH + "packages/";
        /// <summary>
        /// 安卓系统关卡目录
        /// </summary>
        [LuaApiDescription("调试模组包存放路径")]
        public const string ANDROID_LEVELS_PATH = ANDROID_FOLDER_PATH + "levels/";



        /// <summary>
        /// 检测一个路径是否是绝对路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否是绝对路径</returns>
        [LuaApiDescription("检测一个路径是否是绝对路径", "是否是绝对路径")]
        [LuaApiParamDescription("path", "路径")]
        public static bool IsAbsolutePath(string path)
        {
#if (UNITY_EDITOR && UNITY_EDITOR_WIN) || UNITY_STANDALONE_WIN
            if (path.Length > 2)
                return path[1] == ':' && ((path.Length > 3 && path[2] == '\\' && path[3] == '\\') || path[2] == '/');
#elif (UNITY_EDITOR && UNITY_EDITOR_OSX) || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            return path.StartsWith("/");
#elif UNITY_IOS
            return path.StartsWith("/");
#endif
            return false;
        }
        /// <summary>
        /// 将资源的相对路径转为资源真实路径
        /// </summary>
        /// <param name="type">资源种类（gameinit、core: 核心文件、level：关卡、package：模块）</param>
        /// <param name="pathorname">相对路径或名称</param>
        /// <param name="replacePlatform">是否替换文件路径中的[Platform]</param>
        /// <returns></returns>
        [LuaApiDescription("将资源的相对路径转为资源真实路径")]
        [LuaApiParamDescription("type", "资源种类（gameinit、core: 核心文件、level：关卡、package：模块）")]
        [LuaApiParamDescription("pathorname", "相对路径或名称")]
        [LuaApiParamDescription("replacePlatform", "是否替换文件路径中的[Platform]")]
        public static string GetResRealPath(string type, string pathorname, bool replacePlatform = true)
        {
            string result = null;
            string pathbuf = "";
            string[] spbuf = null;

            if (replacePlatform && pathorname.Contains("[Platform]"))
                pathorname = pathorname.Replace("[Platform]", GameConst.GamePlatformIdentifier);

            spbuf = SplitResourceIdentifier(pathorname, out pathbuf);

            if (type == "" && pathorname.Contains(":"))
                type = spbuf[0].ToLower();

            if (type == "gameinit")
            {
#if UNITY_EDITOR
                result = DEBUG_PATH + "/core/game.init.xml";
#elif UNITY_STANDALONE || UNITY_ANDROID
                result = Application.dataPath + "/core/game.init.xml";
#elif UNITY_IOS
                result = Application.streamingAssetsPath + "/core/game.init.xml";
#endif
            }
            else if (type == "systeminit")
            {
#if UNITY_EDITOR
                result = DEBUG_PATH + "/core/system.init.xml";
#elif UNITY_STANDALONE || UNITY_ANDROID
                result = Application.dataPath + "/core/system.init.xml";
#elif UNITY_IOS
                result = Application.streamingAssetsPath + "/core/system.init.xml";
#endif
            }
            else if (type == "logfile")
            {
#if UNITY_EDITOR
                result = DEBUG_PATH + "/output.log";
#elif UNITY_STANDALONE || UNITY_ANDROID
                result = Application.dataPath + "/output.log";
#elif UNITY_IOS
                result = Application.persistentDataPath + "/output.log";
#endif
            }
            else if (type == "level") return GetLevelRealPath(pathbuf);
            else if (type == "package")
            {
                if (pathbuf.Contains(":"))
                {
                    if (IsAbsolutePath(pathbuf)) return pathbuf;
#if UNITY_EDITOR
                    pathbuf = DEBUG_PACKAGES_PATH + pathbuf;
#elif UNITY_STANDALONE
                    pathbuf= Application.dataPath + "/packages/" + pathbuf;
#elif UNITY_ANDROID
                    pathbuf = ANDROID_PACKAGES_PATH + pathbuf;
#elif UNITY_IOS
                    pathbuf = pathbuf;
#endif
                    result = ReplacePathInResourceIdentifier(pathbuf, ref spbuf);
                }
                else
                {
#if UNITY_EDITOR
                    result = DEBUG_PACKAGES_PATH + pathbuf;
#elif UNITY_STANDALONE
                    result = Application.dataPath + "/packages/" + pathbuf;
#elif UNITY_ANDROID
                    result = ANDROID_PACKAGES_PATH + pathbuf;
#elif UNITY_IOS
                    result = pathorname;
#endif
                }
            }
            else if (type == "core")
            {
                if (pathbuf.Contains(":"))
                {
                    if (IsAbsolutePath(pathbuf)) return pathbuf;
#if UNITY_EDITOR
                    pathbuf = DEBUG_PATH + "/core/" + pathbuf;
#elif UNITY_STANDALONE || UNITY_ANDROID
                    pathbuf = Application.dataPath + "/core/" + pathbuf;
#elif UNITY_IOS
                    pathbuf = Application.streamingAssetsPath + "/core/" + pathbuf;
#endif
                    result = ReplacePathInResourceIdentifier(pathbuf, ref spbuf);
                }
                else
                {
#if UNITY_EDITOR
                    result = DEBUG_PATH + "/core/" + pathbuf;
#elif UNITY_STANDALONE || UNITY_ANDROID
                    result = Application.dataPath + "/core/" + pathbuf;
#elif UNITY_IOS
                    result = Application.streamingAssetsPath + "/core/" + pathbuf;
#endif
                }
            }
            else
            {
                GameErrorChecker.LastError = GameError.UnKnowType;
                return pathorname;
            }

            return "file:///" + result;
        }
        /// <summary>
        /// 将关卡资源的相对路径转为关卡资源真实路径
        /// </summary>
        /// <param name="pathorname">关卡的相对路径或名称</param>
        /// <returns></returns>
        [LuaApiDescription("将关卡资源的相对路径转为关卡资源真实路径", "")]
        [LuaApiParamDescription("pathorname", "关卡的相对路径或名称")]
        public static string GetLevelRealPath(string pathorname)
        {
            string result = "";
            string pathbuf = "";
            string[] spbuf = null;

            if (pathorname.Contains(":"))
            {
                spbuf = SplitResourceIdentifier(pathorname, out pathbuf);

                if (IsAbsolutePath(pathbuf)) return pathbuf;
#if UNITY_EDITOR
                pathbuf = DEBUG_LEVELS_PATH + pathbuf;
#elif UNITY_STANDALONE
                pathbuf= Application.dataPath + "/levels/" + pathbuf;
#elif UNITY_ANDROID
                pathbuf= ANDROID_LEVELS_PATH + pathbuf;
#elif UNITY_IOS
                pathbuf = pathbuf;
#endif
                result = ReplacePathInResourceIdentifier(pathbuf, ref spbuf);
            }
            else
            {
#if UNITY_EDITOR
                result = DEBUG_LEVELS_PATH + pathorname;
#elif UNITY_STANDALONE
                result = Application.dataPath + "/levels/" + pathorname;
#elif UNITY_ANDROID
                result = ANDROID_LEVELS_PATH + pathorname;
#elif UNITY_IOS
                result = pathorname;
#endif
            }

            return "file:///" + result;
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
        /// <returns></returns>
        [LuaApiDescription("修复文件路径的 file:/// 前缀", "")]
        [LuaApiParamDescription("path", "路径")]
        public static string FixFilePathScheme(string path)
        {
            if (path.StartsWith("file:///"))
                return path.Substring(8);
            return path;
        }
    }
}
