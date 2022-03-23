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
*/

namespace Ballance2.Services.Debug
{
  /// <summary>
  /// 游戏发生错误的枚举
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("游戏发生错误的枚举")]
  public enum GameError
  {
    /// <summary>
    /// 无错误
    /// </summary>
    [LuaApiDescription("无错误")]
    None,
    [LuaApiDescription("未知错误")]
    UnKnow,
    [LuaApiDescription("事件、资源等受控对象重复注册了")]
    AlreadyRegistered,
    [LuaApiDescription("事件、资源等受控对象没有注册")]
    NotRegister,
    [LuaApiDescription("模块，或者代码资源没有加载")]
    NotLoad,
    [LuaApiDescription("未找到指定资源")]
    NotFound,
    [LuaApiDescription("游戏的配置不正确")]
    ConfigueNotRight,
    [LuaApiDescription("这个功能未实现")]
    NotImplemented,
    [LuaApiDescription("上下文无权限")]
    ContextMismatch,
    [LuaApiDescription("参数未提供")]
    ParamNotProvide,
    [LuaApiDescription("未找到参数")]
    ParamNotFound,
    [LuaApiDescription("参数只读")]
    ParamReadOnly,
    [LuaApiDescription("表示没有错误")]
    Empty,
    [LuaApiDescription("找不到属性")]
    MissingAttribute,
    [LuaApiDescription("指定的模块包不能运行")]
    PackageCanNotRun,
    [LuaApiDescription("指定的模块包与当前版本游戏不兼容")]
    PackageIncompatible,
    [LuaApiDescription("指定的类未找到")]
    ClassNotFound,
    [LuaApiDescription("指定的文件不存在")]
    FileNotFound,
    [LuaApiDescription("指定的函数不存在")]
    FunctionNotFound,
    [LuaApiDescription("指定的模块包未加载或者不存在")]
    PackageDefNotFound,
    [LuaApiDescription("指定的 AssetBundle 未加载或者不存在")]
    AssetBundleNotFound,
    [LuaApiDescription("指定的 AssetBundle 未加载或者不存在")]
    AssetNotFound,
    [LuaApiDescription("指定的 AssetBundle 未加载或者不存在")]
    FileReadFailed,
    [LuaApiDescription("函数未返回参数")]
    NotReturn,
    [LuaApiDescription("无效的包名")]
    InvalidPackageName,
    [LuaApiDescription("注册包失败。")]
    RegisterPackageFailed,
    [LuaApiDescription("不支持的文件格式。")]
    NotSupportFileType,
    [LuaApiDescription("网络错误。")]
    NetworkError,
    [LuaApiDescription("执行代码失败。")]
    ExecutionFailed,
    [LuaApiDescription("无法执行此操作。")]
    AccessDenined,
    [LuaApiDescription("正在加载，请稍后。")]
    IsLoading,
    [LuaApiDescription("系统包未加载。")]
    SystemPackageNotLoad,
    [LuaApiDescription("系统包加载失败，这可能是当前版本存在非正式的修改，尝试下载最新版本解决问题。")]
    SystemPackageLoadFailed,
    [LuaApiDescription("系统未初始化。")]
    SystemNotInit,
    [LuaApiDescription("未知类型")]
    UnKnowType,
    [LuaApiDescription("Lua环境绑定检查失败，通常是配置不正确造成的")]
    EnvBindCheckFailed,
    [LuaApiDescription("表示功能只能在编辑器中使用")]
    OnlyCanUseInEditor,
    [LuaApiDescription("未找到预制体")]
    PrefabNotFound,
    [LuaApiDescription("未找到模块")]
    PackageNotFound,
    [LuaApiDescription("不可用")]
    NotAvailable,
    [LuaApiDescription("请求失败")]
    RequestFailed,
  }

  /// <summary>
  /// 错误信息
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("错误信息")]
  public static class GameErrorInfo
  {
    /// <summary>
    /// 获取错误代码的说明信息
    /// </summary>
    /// <param name="err">错误代码</param>
    /// <returns>错误代码的说明信息</returns>
    [LuaApiDescription("获取错误代码的说明信息", "错误代码的说明信息")]
    [LuaApiParamDescription("err", "错误代码")]
    public static string GetErrorMessage(GameError err)
    {
      switch (err)
      {
        case GameError.None: return "无错误。";
        case GameError.InvalidPackageName: return "无效的包名。";
        case GameError.EnvBindCheckFailed: return "Lua绑定检测失败。";
        case GameError.OnlyCanUseInEditor: return "这个功能只能在Unity编辑器中使用。";
        case GameError.PrefabNotFound: return "未找到预制体。";
        case GameError.FileReadFailed: return "读取文件失败。";
        case GameError.AssetNotFound: return "未找到指定的资源。";
        case GameError.IsLoading: return "正在加载，请稍后。";
        case GameError.AccessDenined: return "不能执行这个操作。";
        case GameError.ExecutionFailed: return "执行代码失败。";
        case GameError.NetworkError: return "网络错误。";
        case GameError.RequestFailed: return "请求失败。";
        case GameError.ParamNotFound: return "未找到参数。";
        case GameError.ParamReadOnly: return "参数只读。";
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
        case GameError.SystemPackageNotLoad: return "系统包未加载。";
        case GameError.SystemNotInit: return "系统未初始化。";
        case GameError.SystemPackageLoadFailed: return "系统包加载失败，这可能是当前版本存在非正式的修改，尝试下载最新版本解决问题。";
        case GameError.PackageNotFound: return "未找到模块包。";
        case GameError.NotFound: return "未找到对象。";
        case GameError.NotAvailable: return "不可用";
        default: return "未知错误: " + err.ToString();
      }
    }
  }

}
