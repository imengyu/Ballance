/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GamePartName.cs
 * 用途：
 * 游戏内核模块包名
 * 
 * 作者：
 * mengyu
 * 
 * 更改历史：
 * 2020-1-1 创建
 *
 */

namespace Ballance2.System.Bridge
{
    /// <summary>
    /// 游戏内置模块名称
    /// </summary>
    [SLua.CustomLuaClass]
    public static class GamePartName
    {
        public const string Core = "core";
        public const string ICManager = "core.icmgr";
        public const string SoundManager = "core.soundmgr";
        public const string BallManager = "core.ballmgr";
        public const string CamManager = "core.cammgr";
        public const string LevelManager = "core.levelmgr";
        public const string LevelLoader = "core.levelloader";
    }
}
