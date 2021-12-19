using System;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameAssetsInfo.cs
* 
* 用途：
* 游戏静态Prefab引入结构类。
*
* 作者：
* mengyu
*/

namespace Ballance2.Res
{
    [Serializable]
    public class GameObjectInfo
    {
        public GameObject Object;
        public string Name;

        public override string ToString() { return Name; }
    }
}
