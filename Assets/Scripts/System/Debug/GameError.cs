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
        ConfigueNotRight,
        NotImplemented,
        ContextMismatch,
        ParamNotProvide,
        Empty,
        MissingAttribute,
        PackageCanNotRun,
        ClassNotFound,
        NotReturn,
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
                case GameError.None: return "无错误";
                case GameError.MissingAttribute: return "必须的配置属性丢失。";
                case GameError.PackageCanNotRun: return "包不能运行。";
                case GameError.ClassNotFound: return "未找到类。";
                case GameError.NotReturn: return "函数未返回参数。";
                case GameError.Empty: return "操作为空。";
                case GameError.ParamNotProvide: return "参数未提供。";
                case GameError.ContextMismatch: return "上下文无权限。";
                case GameError.NotImplemented: return "功能未实现。";
                case GameError.AlreadyRegistered: return "指定资源已注册。";
                case GameError.NotRegister: return "指定资源未注册。";
                case GameError.ConfigueNotRight: return "游戏配置存在错误，这可能是当前版本存在非正式的修改，尝试下载最新版本解决问题。";
                
            }
        }
    }

}
