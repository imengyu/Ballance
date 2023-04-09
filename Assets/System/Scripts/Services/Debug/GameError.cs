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
  public enum GameError
  {
    /// <summary>
    /// 无错误
    /// </summary>
    None,
    
    UnKnow,
    
    AlreadyRegistered,
    
    NotRegister,
    
    NotLoad,
    
    NotFound,
    
    ConfigueNotRight,
    
    NotImplemented,
    
    ContextMismatch,
    
    ParamNotProvide,
    
    ParamNotFound,
    
    ParamReadOnly,
    
    Empty,
    
    MissingAttribute,
    
    PackageCanNotRun,
    
    PackageIncompatible,
    
    ClassNotFound,
    
    FileNotFound,
    
    FunctionNotFound,
    
    PackageDefNotFound,
    
    AssetBundleNotFound,
    
    AssetNotFound,
    
    FileReadFailed,
    
    NotReturn,
    
    InvalidPackageName,
    
    RegisterPackageFailed,
    
    NotSupportFileType,
    
    NetworkError,
    
    ExecutionFailed,
    
    AccessDenined,
    
    IsLoading,
    
    SystemPackageNotLoad,
    
    SystemPackageLoadFailed,
    
    SystemNotInit,
    
    UnKnowType,
    
    EnvBindCheckFailed,
    
    OnlyCanUseInEditor,
    
    PrefabNotFound,
    
    PackageNotFound,
    
    NotAvailable,
    
    RequestFailed,
  }

  /// <summary>
  /// 错误信息
  /// </summary>
  public static class GameErrorInfo
  {
    /// <summary>
    /// 获取错误代码的说明信息
    /// </summary>
    /// <param name="err">错误代码</param>
    /// <returns>错误代码的说明信息</returns>
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
