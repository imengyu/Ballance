/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameEntry.cs
* 
* 用途：
* 整个游戏的发生错误的枚举
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-14 创建
*
*/

namespace Ballance2.System.Debug
{
    [SLua.CustomLuaClass]
    /// <summary>
    /// 游戏发生错误的枚举
    /// </summary>
    public enum GameError
    {
        /// <summary>
        /// 无错误
        /// </summary>
        None,
        AlreadyRegistered,
        NotRegister,
        NotLoad,
        ConfigueNotRight,
        NotImplemented,
        ContextMismatch,
        ParamNotProvide,
        Empty,
        MissingAttribute,
        PackageCanNotRun,
        PackageIncompatible,
        ClassNotFound,
        FileNotFound,
        FunctionNotFound,
        PackageDefNotFound,
        AssetBundleNotFound,
        NotReturn,
        InvalidPackageName,
        RegisterPackageFailed,
        NotSupportFileType,
        NetworkError,
        ExecutionFailed,
    }

    /// <summary>
    /// 错误信息
    /// </summary>
    [SLua.CustomLuaClass]
    public static class GameErrorInfo
    {
        /// <summary>
        /// 获取错误代码的说明信息
        /// </summary>
        /// <param name="err">错误代码</param>
        /// <returns></returns>
        public static string GetErrorMessage(GameError err)
        {
            switch(err)
            {
                default: return "未知错误: " + err.ToString();
                case GameError.None: return "无错误。";
                case GameError.InvalidPackageName: return "无效的包名。";
                case GameError.ExecutionFailed: return "执行 Lua 代码失败。";
                case GameError.NetworkError: return "网络错误。";
                case GameError.NotSupportFileType: return "不支持的文件格式。";
                case GameError.RegisterPackageFailed: return "注册包失败。";
                case GameError.MissingAttribute: return "必须的配置属性丢失。";
                case GameError.PackageCanNotRun: return "包不能运行。";
                case GameError.PackageIncompatible: return "包不兼容。";
                case GameError.ClassNotFound: return "未找到类。";
                case GameError.FileNotFound: return "未找到文件。";
                case GameError.FunctionNotFound: return "未找到函数。";
                case GameError.AssetBundleNotFound: return "未找到 AssetBundle 。";
                case GameError.PackageDefNotFound: return "未找到 PackageDef.xml 。";
                case GameError.NotReturn: return "函数未返回参数。";
                case GameError.Empty: return "操作为空。";
                case GameError.ParamNotProvide: return "参数未提供。";
                case GameError.ContextMismatch: return "上下文无权限。";
                case GameError.NotImplemented: return "功能未实现。";
                case GameError.NotLoad: return "资源未初始化。";
                case GameError.AlreadyRegistered: return "指定资源已注册。";
                case GameError.NotRegister: return "指定资源未注册。";
                case GameError.ConfigueNotRight: return "游戏配置存在错误，这可能是当前版本存在非正式的修改，尝试下载最新版本解决问题。";
                
            }
        }
    }

}
