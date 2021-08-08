using System;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameAssetsInfo.cs
* 
* 用途：
* 游戏静态资源引入结构类。
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
    [Serializable]
    public class GameAssetsInfo
    {
        public UnityEngine.Object Object;
        public string Name;

        public override string ToString() { return Name; }
    }
}
