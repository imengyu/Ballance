﻿/*
* Copyright (c) 2020  mengyu
* 
* 模块名：     
* GameConst.cs
* 
* 用途：
* 配置游戏的静态常量
* 
* 作者：
* mengyu
*/

namespace Ballance2.Config
{
  /// <summary>
  /// 游戏的一些静态常量
  /// </summary>
  public static class GameConst
  {
    /// <summary>
    /// 游戏版本
    /// </summary>
    public const string GameVersion = "2.0.0";
    /// <summary>
    /// 游戏编译版本
    /// </summary>
    public const int GameBulidVersion = 200;
    /// <summary>
    /// 游戏编译版本
    /// </summary>
    public const string GameBulidDate = "2024.08.25";

    #region Unity 环境静态常量

    /// <summary>
    /// 游戏编译平台
    /// </summary>
#if UNITY_EDITOR && UNITY_EDITOR_WIN
    public const string GamePlatform = "Windows Editor Mode";
#elif UNITY_EDITOR && UNITY_EDITOR_OSX
    public const string GamePlatform = "OS X Editor Mode";
#elif UNITY_EDITOR
    public const string GamePlatform = "Editor Mode";
#elif UNITY_IOS
    public const string GamePlatform = "IOS";
#elif UNITY_STANDALONE_OSX
    public const string GamePlatform = "Mac OS X";
#elif UNITY_STANDALONE_WIN
    public const string GamePlatform = "Windows";
#elif UNITY_STANDALONE_LINUX
    public const string GamePlatform = "Linux";
#elif UNITY_ANDROID
    public const string GamePlatform = "Android";
#elif UNITY_PS4
    public const string GamePlatform = "PlayStation 4";
#elif UNITY_XBOXONE
    public const string GamePlatform = "Xbox One";
#elif UNITY_WSA
    public const string GamePlatform = "Universal Windows Platform";
#elif UNITY_WEBGL
    public const string GamePlatform = "WebGl";
#else
    public const string GamePlatform = "Other Platform";
#endif
    /// <summary>
    /// 游戏编译平台标识符
    /// </summary>
#if (UNITY_EDITOR && UNITY_EDITOR_WIN) || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
    public const string GamePlatformIdentifier = "win";
#elif (UNITY_EDITOR && UNITY_EDITOR_OSX) || UNITY_STANDALONE_OSX
    public const string GamePlatformIdentifier = "osx";
#elif UNITY_IOS
    public const string GamePlatformIdentifier = "ios";
#elif UNITY_ANDROID
    public const string GamePlatformIdentifier = "android";
#elif UNITY_PS4
    public const string GamePlatformIdentifier = "ps4";
#elif UNITY_XBOXONE
    public const string GamePlatformIdentifier = "xboxone";
#elif UNITY_WSA
    public const string GamePlatformIdentifier = "wsa";
#elif UNITY_WEBGL
    public const string GamePlatformIdentifier = "webgl";
#else
    public const string GamePlatform = "x";
#endif


    /// <summary>
    /// 游戏编译脚本后端
    /// </summary>
#if ENABLE_MONO
    public const string GameScriptBackend = "Mono";
#elif ENABLE_IL2CPP
        public const string GameScriptBackend = "IL2CPP";
#elif ENABLE_DOTNET
        public const string GameScriptBackend = "Microsoft .NET";
#else
        public const string GameScriptBackend = "Unknow Script Backend";
#endif

    #endregion

    #region 静态配置

    /// <summary>
    /// 配置游戏日志记录器是否启动
    /// </summary>
    public const bool GameLoggerOn = true;
    /// <summary>
    /// 配置游戏日志记录器是否记录至文件
    /// </summary>
    public const bool GameLoggerLogToFile = false;
    /// <summary>
    /// 配置游戏日志记录器记录至文件的路径
    /// </summary>
    public const string GameLoggerLogFile = "./BallanceOutput.log";
    /// <summary>
    /// 配置游戏日志记录器缓冲区最大条数
    /// </summary>
    public const int GameLoggerBufferMax = 1000;

    #endregion

  }
}
