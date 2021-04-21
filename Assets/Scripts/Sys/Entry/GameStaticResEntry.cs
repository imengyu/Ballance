using Ballance2.Sys.Res;
using SubjectNerd.Utilities;
using System.Collections.Generic;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameStaticResEntry.cs
* 
* 用途：
* 静态 Prefab 资源引入脚本
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-13 创建
*
*/

namespace Ballance2.Sys.Entry
{
    public class GameStaticResEntry : MonoBehaviour
    {
        /// <summary>
        /// 静态 Prefab 资源引入
        /// </summary>
        [Reorderable("GamePrefab", true, "Name")]
        public List<GameObjectInfo> GamePrefab = null;
        /// <summary>
        /// 静态资源引入
        /// </summary>
        [Reorderable("GameAssets", true, "Name")]
        public List<GameAssetsInfo> GameAssets = null;

        private void Start()
        {
            GameSystem.FillResEntry(this);
        }
    }
}
